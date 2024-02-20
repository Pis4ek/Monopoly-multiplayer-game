using Mirror;
using Mirror.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
public abstract class MyNetworkDiscoveryBase<Request, Response> : MonoBehaviour
    where Request : NetworkMessage
    where Response : NetworkMessage
{
    public static bool SupportedOnThisPlatform { get { return Application.platform != RuntimePlatform.WebGLPlayer; } }

    [SerializeField]
    [Tooltip("If true, broadcasts a discovery request every ActiveDiscoveryInterval seconds")]
    public bool enableActiveDiscovery = true;

    // broadcast address needs to be configurable on iOS:
    // https://github.com/vis2k/Mirror/pull/3255
    [Tooltip("iOS may require LAN IP address here (e.g. 192.168.x.x), otherwise leave blank.")]
    public string BroadcastAddress = "";

    [SerializeField]
    [Tooltip("The UDP port the server will listen for multi-cast messages")]
    protected int serverBroadcastListenPort = 47777;

    [SerializeField]
    [Tooltip("Time in seconds between multi-cast messages")]
    [Range(1, 60)]
    float ActiveDiscoveryInterval = 3;

    [Tooltip("Transport to be advertised during discovery")]
    public Transport transport;

    [Tooltip("Invoked when a server is found")]
    public ServerFoundUnityEvent<Response> OnServerFound;

    // Each game should have a random unique handshake,
    // this way you can tell if this is the same game or not
    [HideInInspector]
    public long secretHandshake;

    public long ServerId { get; private set; }

    protected UdpClient serverUdpClient;
    protected List<UdpClient> clientUdpClient = new List<UdpClient>();
    protected int clientUdpClientIndex = 0;

#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (transport == null)
            transport = GetComponent<Transport>();

        if (secretHandshake == 0)
        {
            secretHandshake = RandomLong();
            UnityEditor.Undo.RecordObject(this, "Set secret handshake");
        }
    }
#endif

    /// <summary>
    /// virtual so that inheriting classes' Start() can call base.Start() too
    /// </summary>
    public virtual void Start()
    {
        ServerId = RandomLong();

        // active transport gets initialized in Awake
        // so make sure we set it here in Start() after Awake
        // Or just let the user assign it in the inspector
        if (transport == null)
            transport = Transport.active;

        // Server mode? then start advertising
#if UNITY_SERVER
            AdvertiseServer();
#endif
    }

    public static long RandomLong()
    {
        int value1 = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        int value2 = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        return value1 + ((long)value2 << 32);
    }

    // Ensure the ports are cleared no matter when Game/Unity UI exits
    void OnApplicationQuit()
    {
        //Debug.Log("NetworkDiscoveryBase OnApplicationQuit");
        Shutdown();
    }

    void OnDisable()
    {
        //Debug.Log("NetworkDiscoveryBase OnDisable");
        Shutdown();
    }

    void OnDestroy()
    {
        //Debug.Log("NetworkDiscoveryBase OnDestroy");
        Shutdown();
    }

    void Shutdown()
    {
        EndpMulticastLock();
        if (serverUdpClient != null)
        {
            try
            {
                serverUdpClient.Close();
            }
            catch (Exception)
            {
                // it is just close, swallow the error
            }

            serverUdpClient = null;
        }

        if (clientUdpClient != null)
        {
            try
            {
                foreach (var Client in clientUdpClient)
                {
                    Client.Close();
                }
            }
            catch (Exception)
            {
                // it is just close, swallow the error
            }

            clientUdpClient = null;
        }

        CancelInvoke();
    }

    #region Server

    public void HideServer() 
    {
        EndpMulticastLock();
        if (serverUdpClient != null)
        {
            try
            {
                serverUdpClient.Close();
            }
            catch (Exception)
            {
                // it is just close, swallow the error
            }

            serverUdpClient = null;
        }

        CancelInvoke();
    }

    /// <summary>
    /// Advertise this server in the local network
    /// </summary>
    public void AdvertiseServer()
    {
        if (!SupportedOnThisPlatform)
            throw new PlatformNotSupportedException("Network discovery not supported in this platform");

        StopDiscovery();

        // Setup port -- may throw exception
        serverUdpClient = new UdpClient(serverBroadcastListenPort)
        {
            EnableBroadcast = true,
            MulticastLoopback = false
        };

        // listen for client pings
        _ = ServerListenAsync();
    }

    public async Task ServerListenAsync()
    {
        BeginMulticastLock();
        while (true)
        {
            try
            {
                await ReceiveRequestAsync(serverUdpClient);
            }
            catch (ObjectDisposedException)
            {
                // socket has been closed
                break;
            }
            catch (Exception) { }
        }
    }

    async Task ReceiveRequestAsync(UdpClient udpClient)
    {
        // only proceed if there is available data in network buffer, or otherwise Receive() will block
        // average time for UdpClient.Available : 10 us

        UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();

        using (NetworkReaderPooled networkReader = NetworkReaderPool.Get(udpReceiveResult.Buffer))
        {
            long handshake = networkReader.ReadLong();
            if (handshake != secretHandshake)
            {
                // message is not for us
                throw new ProtocolViolationException("Invalid handshake");
            }

            Request request = networkReader.Read<Request>();

            ProcessClientRequest(request, udpReceiveResult.RemoteEndPoint);
        }
    }

    /// <summary>
    /// Reply to the client to inform it of this server
    /// </summary>
    /// <remarks>
    /// Override if you wish to ignore server requests based on
    /// custom criteria such as language, full server game mode or difficulty
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    protected virtual void ProcessClientRequest(Request request, IPEndPoint endpoint)
    {
        Response info = ProcessRequest(request, endpoint);

        if (info == null)
            return;

        using (NetworkWriterPooled writer = NetworkWriterPool.Get())
        {
            try
            {
                writer.WriteLong(secretHandshake);

                writer.Write(info);

                ArraySegment<byte> data = writer.ToArraySegment();
                // signature matches
                // send response
                serverUdpClient.Send(data.Array, data.Count, endpoint);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }
    }

    /// <summary>
    /// Process the request from a client
    /// </summary>
    /// <remarks>
    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>The message to be sent back to the client or null</returns>
    protected abstract Response ProcessRequest(Request request, IPEndPoint endpoint);

    // Android Multicast fix: https://github.com/vis2k/Mirror/pull/2887
#if UNITY_ANDROID
        AndroidJavaObject multicastLock;
        bool hasMulticastLock;
#endif

    void BeginMulticastLock()
    {
#if UNITY_ANDROID
            if (hasMulticastLock) return;

            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                    {
                        multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "lock");
                        multicastLock.Call("acquire");
                        hasMulticastLock = true;
                    }
                }
			}
#endif
    }

    void EndpMulticastLock()
    {
#if UNITY_ANDROID
            if (!hasMulticastLock) return;

            multicastLock?.Call("release");
            hasMulticastLock = false;
#endif
    }

    #endregion

    #region Client

    /// <summary>
    /// Start Active Discovery
    /// </summary>
    public void StartDiscovery()
    {
        if (!SupportedOnThisPlatform)
            throw new PlatformNotSupportedException("Network discovery not supported in this platform");

        StopDiscovery();

        if (clientUdpClient == null) clientUdpClient = new List<UdpClient>();

        try
        {
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Setup port
                var ipPeops = netInterface.GetIPProperties();

                foreach (var addr in ipPeops.UnicastAddresses)
                {
                    if (addr.IPv4Mask.ToString() == "0.0.0.0") continue;
                    clientUdpClient.Add(new UdpClient(0)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = false
                    });
                }
            }
        }
        catch (Exception)
        {
            // Free the port if we took it
            //Debug.LogError("NetworkDiscoveryBase StartDiscovery Exception");
            Shutdown();
            throw;
        }

        _ = ClientListenAsync();

        if (enableActiveDiscovery) InvokeRepeating(nameof(BroadcastDiscoveryRequest), 0, ActiveDiscoveryInterval);
    }

    /// <summary>
    /// Stop Active Discovery
    /// </summary>
    public void StopDiscovery()
    {
        //Debug.Log("NetworkDiscoveryBase StopDiscovery");
        Shutdown();
    }

    /// <summary>
    /// Awaits for server response
    /// </summary>
    /// <returns>ClientListenAsync Task</returns>
    public async Task ClientListenAsync()
    {
        // while clientUpdClient to fix:
        // https://github.com/vis2k/Mirror/pull/2908
        //
        // If, you cancel discovery the clientUdpClient is set to null.
        // However, nothing cancels ClientListenAsync. If we change the if(true)
        // to check if the client is null. You can properly cancel the discovery,
        // and kill the listen thread.
        //
        // Prior to this fix, if you cancel the discovery search. It crashes the
        // thread, and is super noisy in the output. As well as causes issues on
        // the quest.
        while (clientUdpClient != null)
        {
            try
            {

                Task<(Response, IPEndPoint)>[] tasks = new Task<(Response, IPEndPoint)>[clientUdpClient.Count];

                for (int i = 0; i < clientUdpClient.Count; i++)
                {
                    int index = i; // Захватываем переменную внутри цикла
                    tasks[i] = Task.Run(async () =>
                    {
                        UdpReceiveResult udpReceiveResult = await clientUdpClient[index].ReceiveAsync();

                        using (NetworkReaderPooled networkReader = NetworkReaderPool.Get(udpReceiveResult.Buffer))
                        {
                            if (networkReader.ReadLong() != secretHandshake)
                                return (default(Response), default(IPEndPoint));

                            Response response = networkReader.Read<Response>();
                            // we received a message from the remote endpoint

                            return (response, udpReceiveResult.RemoteEndPoint);
                        }
                    });
                }

                Task<(Response, IPEndPoint)> completedTask = await Task.WhenAny(tasks);

                // Получаем результат завершившейся задачи
                (Response response, IPEndPoint endPoint) = await completedTask;

                ProcessResponse(response, endPoint);
            }
            catch (ObjectDisposedException)
            {
                // socket was closed, no problem
                return;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    /// <summary>
    /// Sends discovery request from client
    /// </summary>
    public void BroadcastDiscoveryRequest()
    {
        if (clientUdpClient == null)
            return;

        if (NetworkClient.isConnected)
        {
            StopDiscovery();
            return;
        }

        IPAddress GetBroadCastIP(IPAddress host, IPAddress mask)
        {
            byte[] broadcastIPBytes = new byte[4];
            byte[] hostBytes = host.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();
            for (int i = 0; i < 4; i++)
            {
                broadcastIPBytes[i] = (byte)(hostBytes[i] | (byte)~maskBytes[i]);
            }
            return new IPAddress(broadcastIPBytes);
        }

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, serverBroadcastListenPort);

        List<IPEndPoint> endPoints = new List<IPEndPoint>();

        foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            IPInterfaceProperties ipProps = netInterface.GetIPProperties();
            foreach (var addr in ipProps.UnicastAddresses)
            {
                if (addr.IPv4Mask.ToString() == "0.0.0.0") continue;

                endPoints.Add(new IPEndPoint(GetBroadCastIP(addr.Address, addr.IPv4Mask), serverBroadcastListenPort));
            }
        }

        if (!string.IsNullOrWhiteSpace(BroadcastAddress))
        {
            try
            {
                endPoint = new IPEndPoint(IPAddress.Parse(BroadcastAddress), serverBroadcastListenPort);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        using (NetworkWriterPooled writer = NetworkWriterPool.Get())
        {
            writer.WriteLong(secretHandshake);

            try
            {
                Request request = GetRequest();

                writer.Write(request);

                ArraySegment<byte> data = writer.ToArraySegment();

                for(int i = 0; i < endPoints.Count; i++) 
                {
                    clientUdpClient[i].SendAsync(data.Array, data.Count, endPoints[i]);
                }
            }
            catch (Exception)
            {
                // It is ok if we can't broadcast to one of the addresses
            }
        }
    }

    /// <summary>
    /// Create a message that will be broadcasted on the network to discover servers
    /// </summary>
    /// <remarks>
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected virtual Request GetRequest() => default;

    async Task ReceiveGameBroadcastAsync(UdpClient udpClient)
    {
        // only proceed if there is available data in network buffer, or otherwise Receive() will block
        // average time for UdpClient.Available : 10 us

        UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();

        Debug.Log($"Receive server from:{udpReceiveResult.RemoteEndPoint.Address}");
        using (NetworkReaderPooled networkReader = NetworkReaderPool.Get(udpReceiveResult.Buffer))
        {
            if (networkReader.ReadLong() != secretHandshake)
                return;

            Response response = networkReader.Read<Response>();

            ProcessResponse(response, udpReceiveResult.RemoteEndPoint);
        }
    }


    /// <summary>
    /// Process the answer from a server
    /// </summary>
    /// <remarks>
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// </remarks>
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected abstract void ProcessResponse(Response response, IPEndPoint endpoint);

    #endregion
}
