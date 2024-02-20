using System;
using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class TurnData
    {
        public event Action OnAnyValueChanged;
        public event Action OnTurnEnded;
        public Action<PlayerID> OnPlayerWon;

        public List<PlayerID> PlayablePlayers = new();

        public PlayerID ActivePlayer { 
            get => _activePlayer; 
            set
            {
                _activePlayer = value;
                OnTurnEnded?.Invoke();
                OnAnyValueChanged?.Invoke();
            } 
        }
        public int TurnNumber { 
            get => _turnNumber; 
            set
            {
                _turnNumber = value;
                OnAnyValueChanged?.Invoke();
            }
        }
        public int TurnCycleNumber { 
            get => _turnCycleNumber;
            set
            {
                _turnCycleNumber = value;
                OnAnyValueChanged?.Invoke();
            }
        }

        private PlayerID _activePlayer = PlayerID.Player1;
        private int _turnNumber = 1;
        private int _turnCycleNumber = 1;

        public TurnData(int playersCount)
        {
            for(int i = 0; i < playersCount; i++)
            {
                PlayablePlayers.Add((PlayerID)i);
            }
        }
    }
}
