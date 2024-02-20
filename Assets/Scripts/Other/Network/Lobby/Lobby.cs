using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Mirror;
using UniRx;


namespace Other.Network.Lobby
{
    public class Lobby : MonoBehaviour
    {
        [Inject] CustomAuthenticator authenticator;
        [Inject] GlobalClientData thisClientData;

        List<NetworkConnectionToClient> _clientsConns = new();
        [HideInInspector] public ReactiveCollection<ClientData> Clients = new();

        #region Server
        public void OnStartServer()
        {
            NetworkServer.RegisterHandler<ClientData>(OnRecieveClientData);
            NetworkServer.OnDisconnectedEvent += OnClientDisconnected;
            authenticator.OnServerAuthenticated.AddListener(OnClientAuthenticated);
        }

        public void OnStopServer()
        {
            NetworkServer.UnregisterHandler<ClientData>();
            NetworkServer.OnDisconnectedEvent -= OnClientDisconnected;
            authenticator.OnServerAuthenticated.RemoveListener(OnClientAuthenticated);
        }

        private void OnClientAuthenticated(NetworkConnectionToClient conn) 
        {
            conn.Send(new LobbyData(new List<ClientData>(Clients)));
        }

        private void OnRecieveClientData(NetworkConnectionToClient conn, ClientData clientData) 
        {
            Clients.Add(clientData);
            _clientsConns.Add(conn);
            NetworkServer.SendToAll(new AddClientData(clientData));
        }

        private void OnClientDisconnected(NetworkConnectionToClient conn) 
        {
            int index = _clientsConns.IndexOf(conn);
            NetworkServer.SendToAll(new RemoveClientData(Clients[index].Nickname));
            Clients.RemoveAt(index);
            _clientsConns.RemoveAt(index);
        }
        #endregion

        #region Client
        public void OnStartClient()
        {
            NetworkClient.RegisterHandler<LobbyData>(OnRecieveLobbyData);
            NetworkClient.RegisterHandler<AddClientData>(OnRecieveAddClient);
            NetworkClient.RegisterHandler<RemoveClientData>(OnRecieveRemoveClient);
        }

        public void OnStopClient()
        {
            NetworkClient.UnregisterHandler<LobbyData>();
            NetworkClient.UnregisterHandler<AddClientData>();
            NetworkClient.UnregisterHandler<RemoveClientData>();
        }

        private void OnRecieveLobbyData(LobbyData lobbyData) 
        {
            NetworkClient.Send(new ClientData(thisClientData.Image, thisClientData.Nickname));
            if (!NetworkServer.active) 
            {
                foreach (ClientData client in lobbyData.Clients) 
                {
                    Clients.Add(client);
                }
            }
        }

        private void OnRecieveAddClient(AddClientData msg) 
        {
            if (!NetworkServer.active) 
            {
                Clients.Add(msg.ClientData);
            }
        }

        private void OnRecieveRemoveClient(RemoveClientData clientData) 
        {
            if (!NetworkServer.active)
            {
                foreach (ClientData client in Clients) 
                {
                    if (client.Nickname == clientData.Nickname)
                        Clients.Remove(client);
                }
            }
        }
        #endregion
    }
}
