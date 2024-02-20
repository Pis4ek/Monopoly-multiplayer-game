using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;

namespace Playmode.CommandSystem
{
    public class SetPrisonPlayerStateCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public bool Prisoning { get; private set; }
        public PlayerID PlayerID { get; private set; }

        public SetPrisonPlayerStateCommand(PlayerID playerID, bool prison)
        {
            Prisoning = prison;
            PlayerID = playerID;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData[PlayerID] as Player;
            if(Prisoning == true)
            {
                player.State = PlayerState.Prisoned;
                player.CurrentCell = gameData[40];
            }
            else
            {
                player.State = PlayerState.Default;
                player.CurrentCell = gameData[10];
            }
        }
    }
}
