using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;

namespace Playmode.CommandSystem
{
    public class ChangeCellLevelCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public int CellIndex { get; private set; }
        public bool IsUpgrade { get; private set; }

        public ChangeCellLevelCommand(int cellIndex, bool isUpgrade)
        {
            CellIndex = cellIndex;
            IsUpgrade = isUpgrade;
        }

        public void Execute(GameData gameData)
        {
            var cell = gameData[CellIndex] as BusinessCell;

            if(gameData.TryGetPlayerByID(cell.Owner, out var p))
            {
                var player = p as Player;
                if (IsUpgrade)
                {
                    player.Cash -= cell.Config.BranchCost;
                    cell.Level++;
                }
                else
                {
                    player.Cash += cell.Config.BranchCost / 2;
                    cell.Level--;
                }
            }
            
        }
    }
}