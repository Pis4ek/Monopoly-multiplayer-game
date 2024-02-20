using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Zenject;
using UnityEngine.UI;
using MainMenu.StateMachine;
using System;
using MainMenu;

namespace Other.Network
{
    class CustomAuthenticator : NetworkAuthenticator
    {
        [Header("Server Credentials")]
        public string serverUsername;
        public string serverPassword;
        public bool hasPassword;

        [Header("Client Credentials")]
        public string username;
        public string password;

        readonly HashSet<NetworkConnection> connectionsPendingDisconnect = new();

        [Inject] MainMenuStateMachine _stateMachine;

        [Inject] PopUpMessage _popUpPrefab;
        [Inject] RectTransform _UIcanvas;
        public bool requestPlayerPassword;

        #region Messages

        public struct AuthRequestMessage : NetworkMessage
        {
            // use whatever credentials make sense for your game
            // for example, you might want to pass the accessToken if using oauth
            public string authUsername;
            public string authPassword;
        }

        public struct AuthResponseMessage : NetworkMessage
        {
            public byte code;
            public string message;
        }

        public struct AskPassword : NetworkMessage { }

        public struct AnswerPassword : NetworkMessage 
        {
            public bool Password;
        }
        #endregion

        #region Server

        /// <summary>
        /// Called on server from StartServer to initialize the Authenticator
        /// <para>Server message handlers should be registered in this method.</para>
        /// </summary>
        public override void OnStartServer()
        {
            // register a handler for the authentication request we expect from client
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<AskPassword>(OnAskPasswordMessage, false);
        }

        /// <summary>
        /// Called on server from StopServer to reset the Authenticator
        /// <para>Server message handlers should be unregistered in this method.</para>
        /// </summary>
        public override void OnStopServer()
        {
            // unregister the handler for the authentication request
            NetworkServer.UnregisterHandler<AuthRequestMessage>();
            NetworkServer.UnregisterHandler<AskPassword>();
        }

        /// <summary>
        /// Called on server from OnServerConnectInternal when a client needs to authenticate
        /// </summary>
        /// <param name="conn">Connection to client.</param>
        public override void OnServerAuthenticate(NetworkConnectionToClient conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }

        /// <summary>
        /// Called on server when the client's AuthRequestMessage arrives
        /// </summary>
        /// <param name="conn">Connection to client.</param>
        /// <param name="msg">The message payload</param>
        public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage msg)
        {
            //Debug.Log($"Authentication Request: {msg.authUsername} {msg.authPassword}");

            if (connectionsPendingDisconnect.Contains(conn)) return;

            // check the credentials by calling your web server, database table, playfab api, or any method appropriate.
            if (!hasPassword || msg.authPassword == serverPassword)
            {
                // create and send msg to client so it knows to proceed
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 100,
                    message = "Success"
                };

                conn.Send(authResponseMessage);

                //Debug.Log("Client Authenticated");

                // Accept the successful authentication
                ServerAccept(conn);
            }
            else
            {
                connectionsPendingDisconnect.Add(conn);

                // create and send msg to client so it knows to disconnect
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 200,
                    message = "Invalid Credentials"
                };

                conn.Send(authResponseMessage);

                // must set NetworkConnection isAuthenticated = false
                conn.isAuthenticated = false;

                // disconnect the client after 1 second so that response message gets delivered
                StartCoroutine(DelayedDisconnect(conn, 1f));
            }
        }

        IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            // Reject the unsuccessful authentication
            ServerReject(conn);

            yield return null;

            // remove conn from pending connections
            connectionsPendingDisconnect.Remove(conn);
        }

        public void OnAskPasswordMessage(NetworkConnectionToClient conn, AskPassword msg) 
        {
            conn.Send(new AnswerPassword { Password = hasPassword});
        }

        #endregion

        #region Client

        /// <summary>
        /// Called on client from StartClient to initialize the Authenticator
        /// <para>Client message handlers should be registered in this method.</para>
        /// </summary>
        public override void OnStartClient()
        {
            // register a handler for the authentication response we expect from server
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
            NetworkClient.ReplaceHandler<AnswerPassword>(OnAnswerPasswordMessage, false);
            if (NetworkClient.activeHost)
                requestPlayerPassword = true;
        }
        /// <summary>
        /// Called on client from StopClient to reset the Authenticator
        /// <para>Client message handlers should be unregistered in this method.</para>
        /// </summary>
        public override void OnStopClient()
        {
            // unregister the handler for the authentication response
            NetworkClient.UnregisterHandler<AuthResponseMessage>();
            NetworkClient.UnregisterHandler<AnswerPassword>();
            requestPlayerPassword = false;
        }

        public void OnClientConnected() 
        {
            //Debug.Log("OnClientConnected");
            NetworkClient.Send(new AskPassword());
        }

        /// <summary>
        /// Called on client from OnClientConnectInternal when a client needs to authenticate
        /// </summary>
        public override void OnClientAuthenticate()
        {
            if (requestPlayerPassword)
            {
                AuthRequestMessage authRequestMessage = new AuthRequestMessage
                {
                    authUsername = username,
                    authPassword = password
                };

                NetworkClient.Send(authRequestMessage);
            }
            else OnClientConnected();
        }

        /// <summary>
        /// Called on client when the server's AuthResponseMessage arrives
        /// </summary>
        /// <param name="msg">The message payload</param>
        public void OnAuthResponseMessage(AuthResponseMessage msg)
        {
            if (msg.code == 100)
            {
                //Debug.Log($"Authentication Response: {msg.message}");

                // Authentication has been accepted
                ClientAccept();
            }
            else
            {
                Debug.LogWarning($"Authentication Response: {msg.message}");

                PopUpMessage popUp = Instantiate(_popUpPrefab, _UIcanvas);
                popUp.Initionalize("Invalid Password", "You entered wrong password");
                // Authentication has been rejected
                ClientReject();
            }
        }

        public void OnAnswerPasswordMessage(AnswerPassword msg) 
        {
            //Debug.Log("Answer recieve");
            if (msg.Password)
            {
                _stateMachine.SwitchState<UI_EnterPassword>();
            }
            else
            {
                requestPlayerPassword = true;
                OnClientAuthenticate();
            }
        }

        #endregion
    }
}
