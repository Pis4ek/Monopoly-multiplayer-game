using System;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsBusinessCellData : ClientsCellData
    {
        public event Action OnAnyValueChanged;

        public BusinessType BusinessType { get; private set; }
        public PlayerID Owner { get; private set; } = PlayerID.Nobody;
        public int Level { get; private set; } = 1;
        public int TurnsBeforeSelling { get; private set; } = 15;
        public BusinessCellInfo Config { get; private set; }

        public ClientsBusinessCellData(string name, int index, CellType type, CellDirection direction, 
            BusinessCellInfo config) : base(name, index, type, direction)
        {
            BusinessType = config.Type;
            Config = config;
        }

        public void Update(CellInfoPackage info)
        {
            Owner = info.Owner;
            Level = info.Level;
            TurnsBeforeSelling = info.TurnsBeforeSelling;

            OnAnyValueChanged?.Invoke();
        }
    }
}
