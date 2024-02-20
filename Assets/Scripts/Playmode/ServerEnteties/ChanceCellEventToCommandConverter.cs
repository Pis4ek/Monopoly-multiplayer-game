using Playmode.CommandSystem;
using Playmode.PlayData;
using System;
using System.Collections.Generic;

namespace Playmode.ServerEnteties
{
    public class ChanceCellEventToCommandConverter
    {
        private GameData _data;

        public ChanceCellEventToCommandConverter(GameData data)
        {
            _data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="commands"></param>
        /// <returns>Return null if forfeit not need</returns>
        public ForfeitInfo Convert(ChanceCellEventType ev, out List<ICommand> commands)
        {
            commands = new List<ICommand>();
            ForfeitInfo info = null;
            var p = _data.GetActivePlayer();

            //UnityEngine.Debug.Log($"{p.Name} stand on chance cell and recive {ev} event");

            if(ev == ChanceCellEventType.BigTax)
            {
                info = new ForfeitInfo(2000, p.ID);
                commands.Add(new ChangeCashCommand(p.ID, -2000));
            }
            else if (ev == ChanceCellEventType.SmallTax)
            {
                info = new ForfeitInfo(1000, p.ID);
                commands.Add(new ChangeCashCommand(p.ID, -1000));
            }
            else if(ev == ChanceCellEventType.BranchTax)
            {
                int cash = 0;
                foreach(var cell in _data.GetCellsByPlayer(p.ID))
                {
                    if(cell.Level > 5)
                    {
                        cash += 1000;
                    }
                    else if(cell.Level > 1) 
                    {
                        cash += (cell.Level - 1) * 250;
                    }
                }
                if(cash > 0)
                {
                    info = new ForfeitInfo(cash, p.ID);
                    commands.Add(new ChangeCashCommand(p.ID, -cash));
                }
            }
            else if (ev == ChanceCellEventType.Start)
            {
                commands.Add(new ChangeCashCommand(p.ID, 1000));
            }
            else if (ev == ChanceCellEventType.MoveToPrison)
            {
                commands.Add(new SetPrisonPlayerStateCommand(p.ID, true));
            }
            else if (ev == ChanceCellEventType.Casino)
            {
                if(p.Cash > 500)
                {
                    int result = UnityEngine.Random.Range(0, 4);
                    if(result == 0) commands.Add(new ChangeCashCommand(p.ID, 500));
                    else if (result == 1) commands.Add(new ChangeCashCommand(p.ID, 250));
                    else if (result == 2) commands.Add(new ChangeCashCommand(p.ID, -250));
                    else if (result == 3) commands.Add(new ChangeCashCommand(p.ID, -500));
                }
            }
            else if (ev == ChanceCellEventType.Birthday)
            {
                int cash = 0;
                foreach (IPlayer player in _data.PlayerData)
                {
                    if(player.ID != p.ID)
                    {
                        if(player.Cash > 300)
                        {
                            cash += 300;
                            commands.Add(new ChangeCashCommand(player.ID, -300));
                        }
                    }
                }
                commands.Add(new ChangeCashCommand(p.ID, cash));
            }
            else if (ev == ChanceCellEventType.SetSkipTurnEffect)
            {
                commands.Add(new AddEffectCommand<SkipTurnEffect>(p.ID, 1));
            }
            else if (ev == ChanceCellEventType.SetReverceMoveEffect)
            {
                commands.Add(new AddEffectCommand<ReversiveMoveEffect>(p.ID, 1));
            }
            else if (ev == ChanceCellEventType.SetIgnoreRentEffect)
            {
                var turnsCount = 2 * _data.PlayerData.Count;
                commands.Add(new AddIgnoreRentEffectCommand(p.ID, turnsCount, 0.25f));
            }
            else if (ev == ChanceCellEventType.SetIncreaceIncomeEffect)
            {
                var turnsCount = 2 * _data.PlayerData.Count;
                commands.Add(new AddIncreaceIncomeEffectCommand(p.ID, turnsCount, 1.25f));
            }
            else if (ev == ChanceCellEventType.SetDecreaceIncomeEffect)
            {
                var turnsCount = 2 * _data.PlayerData.Count;
                commands.Add(new AddDecreaceIncomeEffectCommand(p.ID, turnsCount, 0.75f));
            }

            return info;
        }
    }
}
