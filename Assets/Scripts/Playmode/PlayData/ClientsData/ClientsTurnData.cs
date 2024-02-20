using System;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsTurnData
    {
        public event Action OnAnyValueChanged;

        public PlayerID ActivePlayer { get; private set; }
        public int TurnNumber { get; private set; }
        public int TurnCycleNumber { get; private set; }

        public void Update(TurnDataInfoPackage info)
        {
            ActivePlayer = info.ActivePlayer; 
            TurnNumber = info.TurnNumber;
            TurnCycleNumber = info.TurnCycleNumber;
            OnAnyValueChanged?.Invoke();
        }
    }
}
