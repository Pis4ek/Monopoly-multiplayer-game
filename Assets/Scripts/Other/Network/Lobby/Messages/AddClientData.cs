using Mirror;
using UnityEngine;

namespace Other.Network.Lobby
{
    public struct AddClientData : NetworkMessage
    {
        public ClientData ClientData;

        public AddClientData(ClientData clientData)
        {
            ClientData = clientData;
        }
    }
}
