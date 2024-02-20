using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;

namespace Other.Network
{
    public struct MSG_LobbyData : NetworkMessage
    {
        public PlayerID playerID;
        public int LobbySize;

        public MSG_LobbyData(PlayerID playerID, int lobbySize)
        {
            this.playerID = playerID;
            LobbySize = lobbySize;
        }
    }
}
