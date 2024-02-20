using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class PayForBusinessLevelCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID Player { get; private set; }

        public PayForBusinessLevelCommand(PlayerID player)
        {
            Player = player;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(Player) as Player;

            int price = 0;

            foreach (var cell in gameData.GetCellsByPlayer(player.ID))
            {
                if (cell.Level == 6)
                    price += 4;
                if (cell.Level > 1)
                    price += cell.Level - 1;
            }
            player.Cash -= price * 250;
        }
    }
}
