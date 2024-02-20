using Playmode.PlayData;
using System;
using System.Collections.Generic;
using Zenject;

namespace Playmode.ServerEnteties
{
    public class TurnSystem
    {
        public event Action OnTurnEnded;

        [Inject(Id = "Server")] GameData _gameData;
        private TurnData _turnData => _gameData.TurnData;

        public void EndTurn()
        {
            SetNextPlayer();
            OnTurnEnded?.Invoke();
        }

        private void SetNextPlayer()
        {
            _turnData.TurnNumber++;
            int index = (int)_turnData.ActivePlayer + 1;
            if(index == _gameData.PlayerData.Count)
            {
                index = 0;
                _turnData.TurnCycleNumber++;
            }

            _turnData.ActivePlayer = (PlayerID)index;
        }
        
    }
}