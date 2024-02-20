using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class GameData
    {
        public readonly MapData MapData;
        public readonly PlayerData PlayerData;
        public readonly TurnData TurnData;
        public readonly LoggerData LoggerData;

        public GameData(GameMapConfig mapConfig, int playersCount)
        {
            MapData = new MapData(mapConfig);
            PlayerData = new PlayerData(MapData, playersCount);
            TurnData = new(playersCount);
            LoggerData = new();
        }
        #region Iteractors
        public IPlayer this[PlayerID id] => PlayerData[id];
        public ICell this[int id] => MapData[id];
        #endregion

        #region GetPlayer
        public IPlayer GetPlayerByID(PlayerID id) 
            => PlayerData.GetPlayerByID(id);
        public IPlayer GetActivePlayer()
            => PlayerData[TurnData.ActivePlayer];
        public bool TryGetPlayerByID(PlayerID id, out IPlayer player) 
            => PlayerData.TryGetPlayerByID(id, out player);
        #endregion

        #region GetCellByIndex
        public ICell GetCellByIndex(int index) 
            => MapData.GetCellByIndex(index);
        public bool TryGetCellByIndex(int index, out ICell cell) 
            => MapData.TryGetCellByIndex(index, out cell);
        public List<ICell> GetCellsByIndex(ICollection<int> indexes) 
            => MapData.GetCellsByIndex(indexes);
        #endregion

        #region GetCellByOther
        public IReadOnlyList<IBusinessCell> GetCellsByBusinessType(BusinessType type)
            => MapData.GetCellsByBusinessType(type);

        public int GetPlayersCellsCountByType(PlayerID id, BusinessType type)
            => MapData.GetPlayersCellsCountByType(id, type);
        public IReadOnlyList<IBusinessCell> GetCellsByPlayer(PlayerID id)
            => MapData.GetCellsByPlayer(id);
        #endregion

        public void AddLog(PlayerID author, string text) => LoggerData.Add(author, text);
    }
}
