using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;

namespace Other.Network.Lobby
{
    public struct LobbyData : NetworkMessage
    {
        public List<ClientData> Clients;

        public LobbyData(List<ClientData> clients)
        {
            Clients = clients;
        }
    }
}
