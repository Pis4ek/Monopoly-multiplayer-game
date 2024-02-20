using Playmode.PlayData;
using Playmode.ServerEnteties;
using System;
using UnityEngine;

namespace Playmode.CommandSystem
{
    public class BuyCellUnderPlayerCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID Player { get; private set; }

        public BuyCellUnderPlayerCommand(PlayerID player)
        {
            Player = player;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData[Player] as Player;
            var cell = player.CurrentCell;

            if (cell is BusinessCell c) 
            { 
                if(c.Owner == PlayerID.Nobody)
                {
                    c.Owner = Player;
                    player.Cash -= c.Config.Cost;
                    return;
                }
                Debug.Log("BuyCellUnderPlayerCommand tried to buy cell, that has owner");
            }
            Debug.Log("BuyCellUnderPlayerCommand tried to buy not business cell");
        }
    }
}
