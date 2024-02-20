using System;

namespace Playmode.PlayData
{
    public interface IBusinessCell : ICell
    {
        public event Action<IBusinessCell> OnAnyValueChanged;

        public BusinessType Type { get; }
        public PlayerID Owner { get; }
        public int Level { get; }
        public int TurnsBeforeSelling { get; }
        public BusinessCellInfo Config { get; }

        public void Update(CellInfoPackage info);
    }
}
