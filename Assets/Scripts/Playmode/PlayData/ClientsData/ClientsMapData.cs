using System.Collections;
using System.Collections.Generic;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsMapData : IEnumerable
    {
        public IReadOnlyList<ClientsBusinessCellData> BusinessCells => _businessCells;

        private Dictionary<int, ClientsCellData> _cells = new();
        private Dictionary<BusinessType, List<ClientsBusinessCellData>> _cellsByBType = new();
        private List<ClientsBusinessCellData> _businessCells = new();

        public ClientsMapData(GameMapConfig mapConfig)
        {
            foreach (var cellPair in mapConfig.BusinessCells)
            {
                CellDirection direction;
                if (cellPair.Key < 10)
                    direction = CellDirection.Top;
                else if (cellPair.Key < 20)
                    direction = CellDirection.Right;
                else if (cellPair.Key < 30)
                    direction = CellDirection.Down;
                else
                    direction = CellDirection.Left;

                var config = cellPair.Value;
                var cell = new ClientsBusinessCellData(config.Name, cellPair.Key, CellType.Business, direction, config);
                _cells.Add(cellPair.Key, cell);
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

            foreach (var cell in mapConfig.ChanceCells)
            {
                if (cell.Key == 0 || cell.Key == 10 || cell.Key == 20 || cell.Key == 30 || cell.Key == 40)
                {

                    _cells.Add(cell.Key, new ClientsCellData(cell.Value.Name, cell.Key, 
                        CellType.Edge, CellDirection.Top));
                }
                else if(cell.Key < 10)
                {
                    _cells.Add(cell.Key, new ClientsCellData(cell.Value.Name, cell.Key,
                        CellType.Chance, CellDirection.Top));
                }
                else if (cell.Key < 20)
                {
                    _cells.Add(cell.Key, new ClientsCellData(cell.Value.Name, cell.Key,
                        CellType.Chance, CellDirection.Right));
                }
                else if (cell.Key < 30)
                {
                    _cells.Add(cell.Key, new ClientsCellData(cell.Value.Name, cell.Key,
                        CellType.Chance, CellDirection.Down));
                }
                else
                {
                    _cells.Add(cell.Key, new ClientsCellData(cell.Value.Name, cell.Key, 
                        CellType.Chance, CellDirection.Left));
                }
            }
        }

        #region CellsByIndex
        public ClientsCellData this[int index] => GetCellByIndex(index);

        public ClientsCellData GetCellByIndex(int index)
        {
            if (_cells.ContainsKey(index) == false)
            {
                throw new System.Exception($"GameMap contains not cell with such index ({index})");
            }
            return _cells[index];
        }

        public List<ClientsCellData> GetCellsByIndex(ICollection<int> indexes)
        {
            var cells = new List<ClientsCellData>();
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

        public bool TryGetCellByIndex(int index, out ClientsCellData cell) 
            => _cells.TryGetValue(index, out cell);

        #endregion

        #region OtherCellsBy
        public IReadOnlyList<ClientsBusinessCellData> GetCellsByBusinessType(BusinessType type) 
            => _cellsByBType[type];

        public int GetPlayersCellsCountByType(PlayerID id, BusinessType type)
        {
            int count = 0;
            var cellsForCheck = GetCellsByBusinessType(type);
            foreach (var cellForCheck in cellsForCheck)
            {
                if (cellForCheck.Owner == id && cellForCheck.Level > 0)
                {
                    count++;
                }
            }
            return count;
        }

        public List<ClientsBusinessCellData> GetCellsByPlayer(PlayerID id)
        {
            List<ClientsBusinessCellData> list = new();
            foreach (var cell in _businessCells)
            {
                if (cell.Owner == id)
                {
                    list.Add(cell);
                }
            }

            if (list.Count == 0)
            {
                UnityEngine.Debug.LogWarning($"MapData can not give cells of {id}. " +
                    $"Either game has not such PlayerID or this player has not any BusinessCells");
            }
            return list;
        }
        #endregion

        #region Other
        public void Update(CellInfoPackage info)
        {
            var cell = _cells[info.Index] as ClientsBusinessCellData;
            cell.Update(info);
        }
        public void Update(ICollection<CellInfoPackage> infos)
        {
            foreach (var info in infos)
            {
                var cell = _cells[info.Index] as ClientsBusinessCellData;
                cell.Update(info);
            }
        }

        public IEnumerator GetEnumerator() => _cells.Values.GetEnumerator();
        #endregion
    }
}
