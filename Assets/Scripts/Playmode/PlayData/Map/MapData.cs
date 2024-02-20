using System.Collections;
using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class MapData : IEnumerable
    {
        public IReadOnlyList<BusinessCell> BusinessCells => _businessCells;

        private Dictionary<int, ICell> _cells = new();
        private Dictionary<int, CellType> _cellTypes = new();
        private Dictionary<BusinessType, List<IBusinessCell>> _cellsByBType = new();
        private List<BusinessCell> _businessCells = new();

        public MapData(GameMapConfig config)
        {
            foreach(var cellPair in config.BusinessCells)
            {
                var cell = new BusinessCell(cellPair.Key, cellPair.Value);
                _cells.Add(cellPair.Key, cell);
                _cellTypes.Add(cellPair.Key, CellType.Business);
                _businessCells.Add(cell);

                if (_cellsByBType.ContainsKey(cellPair.Value.Type))
                {
                    _cellsByBType[cellPair.Value.Type].Add(cell);
                }
                else 
                {
                    _cellsByBType.Add(cellPair.Value.Type, new());
                    _cellsByBType[cellPair.Value.Type].Add(cell);
                }
            }

            foreach (var cell in config.ChanceCells)
            {
                _cells.Add(cell.Key, new ChanceCell(cell.Key, cell.Value));
                if(cell.Key == 0 || cell.Key == 10 || cell.Key == 20 || cell.Key == 30 || cell.Key == 40)
                {
                    _cellTypes.Add(cell.Key, CellType.Edge);
                }
                else
                {
                    _cellTypes.Add(cell.Key, CellType.Chance);
                }
            }
        }

        #region CellsByIndex
        public ICell this[int index] => GetCellByIndex(index);

        public ICell GetCellByIndex(int index)
        {
            //if (index < 0) { index = 41 - (index % 41);} index = (index % 41) //зацикливание индексов

            if (_cells.ContainsKey(index) == false)
            {
                throw new System.Exception($"GameMap contains not cell with such index ({index})");
            }
            return _cells[index];
        }

        public List<ICell> GetCellsByIndex(ICollection<int> indexes)
        {
            var cells = new List<ICell>();
            foreach (var index in indexes)
            {
                if (_cells.ContainsKey(index) == false)
                {
                    throw new System.Exception($"GameMap contains not cell with such index ({index})");
                }
                cells.Add(_cells[index]);
            }
            return cells;
        }

        public bool TryGetCellByIndex(int index, out ICell cell) => _cells.TryGetValue(index, out cell);

        #endregion

        #region OtherCellsBy
        public IReadOnlyList<IBusinessCell> GetCellsByBusinessType(BusinessType type) => _cellsByBType[type];

        public int GetPlayersCellsCountByType(PlayerID id, BusinessType type)
        {
            int count = 0;
            var cellsForCheck = GetCellsByBusinessType(BusinessType.AutoIndustry);
            foreach (var cellForCheck in cellsForCheck)
            {
                if (cellForCheck.Owner == id && cellForCheck.Level > 0)
                {
                    count ++;
                }
            }
            return count;
        }

        public List<IBusinessCell> GetCellsByPlayer(PlayerID id)
        {
            List<IBusinessCell> list = new();
            foreach (var cell in _businessCells)
            {
                if(cell.Owner == id)
                {
                    list.Add(cell);
                }
            }

            if(list.Count == 0)
            {
                UnityEngine.Debug.LogWarning($"MapData can not give cells of {id}. " +
                    $"Either game has not such PlayerID or this player has not any BusinessCells");
            }
            return list;
        }
        #endregion

        #region Other
        public IEnumerator GetEnumerator() => _cells.Values.GetEnumerator();
        #endregion
    }
}
