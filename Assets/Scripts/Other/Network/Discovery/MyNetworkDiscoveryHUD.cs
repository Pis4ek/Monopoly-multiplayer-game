using Other.Network.Discovery;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Other.Network;

[DisallowMultipleComponent]
[AddComponentMenu("Network/MyNetwork Discovery HUD")]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
[RequireComponent(typeof(MyNetworkDiscovery))]
public class MyNetworkDiscoveryHUD : MonoBehaviour
{
    readonly Dictionary<long, ServerRes> discoveredServers = new Dictionary<long, ServerRes>();
    Vector2 scrollViewPos = Vector2.zero;

    public MyNetworkDiscovery networkDiscovery;
    public NetworkAdapter NetworkAdapter;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<MyNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects((UnityEngine.Object[])(new object[] { this, networkDiscovery }), "Set NetworkDiscovery");
        }
    }
#endif

    void OnGUI()
    {
        if (NetworkManager.singleton == null)
            return;

        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
            DrawGUI();

        if (NetworkServer.active || NetworkClient.active)
            StopButtons();
    }

    void DrawGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 500));
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Find Servers"))
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }

        // LAN Host
        if (GUILayout.Button("Start Host"))
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        // Dedicated server
        if (GUILayout.Button("Start Server"))
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }

        GUILayout.EndHorizontal();

        // show list of found server

        GUILayout.Label($"Discovered Servers [{discoveredServers.Count}]:");

        // servers
        scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);

        foreach (ServerRes info in discoveredServers.Values)
            if (GUILayout.Button(info.EndPoint.Address.ToString()))
                Connect(info);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void StopButtons()
    {
        GUILayout.BeginArea(new Rect(10, 40, 100, 60));

        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Host"))
            {
                NetworkManager.singleton.StopHost();
                networkDiscovery.StopDiscovery();
            }

            if (GUILayout.Button("Start game")) 
            {
                NetworkAdapter.StartGame();
            }

        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Client"))
            {
                NetworkManager.singleton.StopClient();
                networkDiscovery.StopDiscovery();
            }
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            if (GUILayout.Button("Stop Server"))
            {
                NetworkManager.singleton.StopServer();
                networkDiscovery.StopDiscovery();
            }
        }

        GUILayout.EndArea();
    }

    void Connect(ServerRes info)
    {
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerRes info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        //Debug.Log($"uri:{info.uri} serverID:{info.serverId}");
        discoveredServers[info.serverId] = info;
    }
}
