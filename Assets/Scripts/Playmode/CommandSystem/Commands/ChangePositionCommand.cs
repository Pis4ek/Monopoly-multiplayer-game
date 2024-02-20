using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class ChangePositionCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        private const int LAST_CELL_INDEX = 40;

        public int StepSize { get; private set; }
        public PlayerID PlayerID { get; private set; }

        public ChangePositionCommand(int stepSize, PlayerID playerID)
        {
            StepSize = stepSize;
            PlayerID = playerID;
        }

        public ChangePositionCommand(ThrowCubesResult result)
        {
            StepSize = result.Cube1Result + result.Cube2Result;
            PlayerID = result.Thrower;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData[PlayerID] as Player;

            /*            int cellIndex = player.CurrentCell.Index + StepSize;
                        if (player.CurrentCell.Index == LAST_CELL_INDEX)
                        {
                            cellIndex = 10 + StepSize;
                        }

                        if (cellIndex < 0)
                            cellIndex = LAST_CELL_INDEX + cellIndex;
                        cellIndex %= LAST_CELL_INDEX;*/
            int currentIndex = player.CurrentCell.Index;
            int newIndex = currentIndex;

            if (currentIndex == LAST_CELL_INDEX)
            {
                newIndex = 10 + StepSize;
            }
            else
            {
                newIndex = currentIndex + StepSize;
            }

            if (newIndex < 0)
                newIndex = LAST_CELL_INDEX + currentIndex + StepSize;
            newIndex %= LAST_CELL_INDEX;

            player.CurrentCell = gameData[newIndex];
        }
    }
}
