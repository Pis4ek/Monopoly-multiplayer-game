using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Zenject;
using Other.Network.Lobby;

namespace Other.Network
{
    public class NetworkAdapter : NetworkManager
    {
        [Inject] LastGameClientsSession _clientsSession;
        [Inject] Lobby.Lobby _lobby;

        List<NetworkConnectionToClient> players = new List<NetworkConnectionToClient>();
        public static new NetworkAdapter singleton { get; private set; }

        [Inject]
        public void Init(LastGameClientsSession lastGameClientsSession) 
        {
            _clientsSession = lastGameClientsSession;
        }

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        public void StartGame() 
        {
            for(int i = 0; i < players.Count; i++) 
            {
                players[i].Send(new MSG_LobbyData((PlayerID)Enum.GetValues(typeof(PlayerID)).GetValue(i), NetworkServer.connections.Count));
            }
            singleton.ServerChangeScene("Playmode");
        }

        private void RecieveLobbyData(MSG_LobbyData lobbyData) 
        {
            _clientsSession.PlayerID = lobbyData.playerID;
            _clientsSession.PlayersCount = lobbyData.LobbySize;
            
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            _lobby.OnStartServer();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            _lobby.OnStopServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<MSG_LobbyData>(RecieveLobbyData);
            _lobby.OnStartClient();
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            NetworkClient.UnregisterHandler<MSG_LobbyData>();
            _lobby.OnStopClient();
        }
        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            //Debug.Log("serverAddPlayer");
            players.Add(conn);
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            players.Remove(conn);
        }

    }
}
