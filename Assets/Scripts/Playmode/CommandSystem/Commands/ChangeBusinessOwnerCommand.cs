using Playmode.PlayData;
using System;
using System.Collections.Generic;

namespace Playmode.CommandSystem
{
    public class ChangeBusinessOwnerCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public ICollection<int> CellIndexes { get; private set; }
        public PlayerID NewOwner { get; private set; }

        public ChangeBusinessOwnerCommand(int cellIndex, PlayerID newOwner)
        {
            CellIndexes = new List<int>(1) { cellIndex };
            NewOwner = newOwner;
        }

        public ChangeBusinessOwnerCommand(ICollection<int> cellIndex, PlayerID newOwner)
        {
            CellIndexes = cellIndex;
            NewOwner = newOwner;
        }

        public void Execute(GameData gameData)
        {
            if(CellIndexes != null)
            {
                var cells = gameData.GetCellsByIndex(CellIndexes);

                foreach (var cell in cells)
                {
                    if (cell is BusinessCell businessCell)
                    {
                        if(businessCell.Owner == NewOwner)
                        {
                            UnityEngine.Debug.LogError("SwitchBusinessOwnerCommand tried to " +
                                "switch same cell owner.");
                        }
                        businessCell.Owner = NewOwner;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("SwitchBusinessOwnerCommand has unvalid cell.");
                    }
                }
            }            
        }
    }
}