using Playmode.PlayData;
using System;
using System.Collections.Generic;

namespace Playmode.CommandSystem
{
    public class EndTurnCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;
        
        private PlayerID _nextActivePlayer;

        public void Execute(GameData gameData)
        {
            _nextActivePlayer = gameData.TurnData.ActivePlayer;
            SellPlegedCells(gameData);
            DecreaceTurnEffectsCounter(gameData);
            EndTurn(gameData);
            while(true)
            {
                if (HasAndDecreaceActivePlayersSkipTurnEffect(gameData))
                {
                    EndTurn(gameData);
                }
                else
                {
                    break;
                }
            }

            gameData.TurnData.ActivePlayer = _nextActivePlayer;
        }

        private void SellPlegedCells(GameData gameData)
        {
            foreach(var cell in gameData.MapData.BusinessCells)
            {
                if(cell.Level == 0)
                {
                    cell.TurnsBeforeSelling--;
                    if(cell.TurnsBeforeSelling == 0)
                    {
                        cell.TurnsBeforeSelling = 15;
                        cell.Level = 1;
                        cell.Owner = PlayerID.Nobody;
                    }
                }
            }
        }

        private void EndTurn(GameData gameData)
        {
            var turnData = gameData.TurnData;

            gameData.TurnData.TurnNumber++;

            int index = turnData.PlayablePlayers.IndexOf(_nextActivePlayer) + 1;
            if (index == turnData.PlayablePlayers.Count)
            {
                index = 0;
                turnData.TurnCycleNumber++;
            }

            _nextActivePlayer = turnData.PlayablePlayers[index];
        }
        
        private void DecreaceTurnEffectsCounter(GameData gameData)
        {
            var list = new List<Type>(4);
            foreach (Player player in gameData.PlayerData)
            {
                list.Clear();
                foreach (var effectPair in player.Effects)
                {
                    if(effectPair.Value is ITurnBasedEffect e)
                    {
                        e.Counter--;
                        if(e.Counter == 0)
                        {
                            list.Add(effectPair.Key);
                        }
                    }
                }
                foreach(var typeToDelete in list)
                {
                    player.Effects.Remove(typeToDelete);
                }
            }
        }

        private bool HasAndDecreaceActivePlayersSkipTurnEffect(GameData gameData)
        {
            var player = gameData[_nextActivePlayer];

            if(player.Effects.TryGetValue(typeof(SkipTurnEffect), out var effect))
            {
                UnityEngine.Debug.Log($"Becauce player {player.Name} has SkipTurnEffect " +
                    $"he skip this turn.");
                var skipEffect = effect as SkipTurnEffect;
                skipEffect.Counter--;
                if (skipEffect.Counter == 0)
                {
                    player.Effects.Remove(typeof(SkipTurnEffect));
                }
                return true;
            }
            return false;
        }
    }
}
