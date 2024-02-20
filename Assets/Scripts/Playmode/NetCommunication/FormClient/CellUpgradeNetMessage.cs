using Mirror;

namespace Playmode.NetCommunication
{
    public struct CellUpgradeNetMessage : INetMessage
    {
        public PlayerID ByWho;
        public bool IsUpgrade;
        public int CellIndex;

        public CellUpgradeNetMessage(PlayerID byWho, bool isUpgrade, int cellIndex)
        {
            ByWho = byWho;
            IsUpgrade = isUpgrade;
            CellIndex = cellIndex;
        }

        public void SendToServer()
        {
            NetworkClient.Send(this);
        }

        public void SendToClient()
        {
            NetworkServer.SendToAll(this);
        }
    }
}
