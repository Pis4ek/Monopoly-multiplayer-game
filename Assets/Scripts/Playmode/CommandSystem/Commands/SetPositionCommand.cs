using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;

namespace Playmode.CommandSystem
{
    public class SetPositionCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public int CellIndex { get; private set; }
        public PlayerID PlayerID { get; private set; }

        public SetPositionCommand(int cellIndex, PlayerID playerID)
        {
            CellIndex = cellIndex;
            PlayerID = playerID;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData[PlayerID] as Player;
            player.CurrentCell = gameData[CellIndex];
        }
    }
}
