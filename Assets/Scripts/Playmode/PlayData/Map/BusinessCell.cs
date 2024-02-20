using System;

namespace Playmode.PlayData
{
    public class BusinessCell : IBusinessCell
    {
        public event Action<IBusinessCell> OnAnyValueChanged;

        public string Name => Config.Name;
        public int Index { get; private set; }
        public BusinessType Type => Config.Type;
        public BusinessCellInfo Config { get; private set; }
        public PlayerID Owner { 
            get => _owner; 
            set
            {
                _owner = value;
                OnAnyValueChanged?.Invoke(this);
            } 
        }
        public int Level { 
            get => _level; 
            set
            {
                _level = value;
                if(_level == 0)
                {
                    TurnsBeforeSelling = 15;
                }
                OnAnyValueChanged?.Invoke(this);
            }
        }
        public int TurnsBeforeSelling { get; set; } = 15;
        public int Income => Config.IncomeByLevel[_level];

        private PlayerID _owner = PlayerID.Nobody;
        private int _level = 1;

        public BusinessCell(int index, BusinessCellInfo config)
        {
            Index = index;
            Config = config;
        }

        public void Update(CellInfoPackage info)
        {
            _level = info.Level;
            TurnsBeforeSelling = info.TurnsBeforeSelling;
            _owner = info.Owner;
            OnAnyValueChanged?.Invoke(this);
        }

        public override string ToString()
        {
            return $"Name - {Name}, " +
                $"Index - {Index}, " +
                $"Owner - {Owner}, " +
                $"Level - {Level}, " +
                $"Type - {Config.Type}.";
        }
    }
}
