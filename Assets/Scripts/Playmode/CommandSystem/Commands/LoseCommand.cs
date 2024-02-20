using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class LoseCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID Player { get; private set; }

        public LoseCommand(PlayerID player) 
        {
            Player = player;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(Player) as Player;
            if (player.State == PlayerState.Lost) return;
            else
            {
                player.State = PlayerState.Lost;
                player.Cash = 0;
                player.Effects.Clear();
                foreach (var cell in gameData.GetCellsByPlayer(player.ID))
                {
                    var c = cell as BusinessCell;
                    c.Owner = PlayerID.Nobody;
                    c.Level = 1;
                    c.TurnsBeforeSelling = 15;
                }
            }
            OnNeedExecuteOtherCommand.Invoke(new EndTurnCommand());

            var turnData = gameData.TurnData;

            turnData.PlayablePlayers.Remove(player.ID);
            if(turnData.PlayablePlayers.Count == 1)
            {
                turnData.OnPlayerWon?.Invoke(turnData.PlayablePlayers[0]);
                UnityEngine.Debug.Log($"{turnData.PlayablePlayers[0]} won!!!");
            }
        }
    }
}
