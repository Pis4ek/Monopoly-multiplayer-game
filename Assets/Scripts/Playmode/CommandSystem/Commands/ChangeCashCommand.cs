using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;

namespace Playmode.CommandSystem
{
    public class ChangeCashCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID PlayerId { get; private set; }
        public int Cash { get; private set; }

        public ChangeCashCommand(PlayerID playerId, int cash)
        {
            PlayerId = playerId;
            Cash = cash;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(PlayerId) as Player;
            player.Cash += Cash;
        }
    }
}