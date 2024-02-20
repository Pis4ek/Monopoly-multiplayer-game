using Other;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsGameData
    {
        public readonly ClientsMapData MapData;
        public readonly ClientsPlayersData PlayerData;
        public readonly ClientsTurnData TurnData;
        public readonly ClientsLogData LogData;

        public PlayerID WaitedPlayerID { get; set; }
        public PlayerID ClientsPlayerID { get; private set; }


        public ClientsGameData(GameMapConfig mapConfig, LastGameClientsSession session)
        {
            MapData = new ClientsMapData(mapConfig);
            PlayerData = new ClientsPlayersData(session.PlayersCount);
            TurnData = new();
            LogData = new();

            ClientsPlayerID = session.PlayerID;
        }

        #region Updating
        #endregion
    }
}
