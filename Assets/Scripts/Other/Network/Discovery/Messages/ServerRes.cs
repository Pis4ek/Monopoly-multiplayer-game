﻿using System;
using System.Net;
using Mirror;

namespace Other.Network.Discovery
{
    public struct ServerRes : NetworkMessage
    {
        // The server that sent this
        // this is a property so that it is not serialized,  but the
        // client fills this up after we receive it
        public IPEndPoint EndPoint { get; set; }

        public Uri uri;

        // Prevent duplicate server appearance when a connection can be made via LAN on multiple NICs
        public long serverId;

        public string serverName;

        public int clientsInLobby;

        public int lobbySize;

        public bool hasPassword;
    }
}
