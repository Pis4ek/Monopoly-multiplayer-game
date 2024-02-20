using Mirror;
using Playmode.PlayData;
using System.Collections.Generic;

namespace Playmode.NetCommunication
{
    public struct UpdateGameDataNetMessage : INetMessage
    {
        public List<CellInfoPackage> CellsData;
        public List<PlayerInfoPackage> PlayersData;
        public TurnDataInfoPackage TurnData;
        public List<Log> LogsData;

        public UpdateGameDataNetMessage(List<CellInfoPackage> cellsData, List<PlayerInfoPackage> playersData, 
            TurnDataInfoPackage turnData, List<Log> logsData)
        {
            CellsData = cellsData;
            PlayersData = playersData;
            TurnData = turnData;
            LogsData = logsData;
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
