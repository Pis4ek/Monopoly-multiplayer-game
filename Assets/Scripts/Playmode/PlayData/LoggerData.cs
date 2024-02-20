using Playmode.ServerEnteties;
using System;
using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class LoggerData
    {
        public event Action OnLogAdded;

        public Dictionary<int, Log> Logs { get; private set; } = new(32);
        public Log LastLog { get; private set; }

        private int _idForNewLog = 0;

        public void Add(PlayerID author, string text)
        {
            LastLog = new(author, text, _idForNewLog);
            Logs.Add(_idForNewLog, LastLog);
            _idForNewLog++;
            OnLogAdded?.Invoke();
        }

        #region Prison
        public void AddEscapePrisonLog(PlayerID author, ThrowCubesResult result)
        {
            if (result.IsDouble)
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices {result.Cube1Result}:{result.Cube2Result} and escaped from prison.");
            }
            else
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices {result.Cube1Result}:{result.Cube2Result} and can not escape from prison.");
            }
        }

        public void AddPledgePrisonLog(PlayerID author)
        {
            Add(author, $"<cp>{(int)author}</cp> pay to left prison.");
        }
        #endregion

        #region ThrowCubes
        public void AddThrowCubesLog(PlayerID author, ThrowCubesResult result)
        {
            if (result.IsDouble)
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices <b>{result.Cube1Result}:{result.Cube2Result}</b> and gets one more turn because he got a double.");
            }
            else
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices <b>{result.Cube1Result}:{result.Cube2Result}</b>.");
            }
        }
        public void AddThrowCubesReverciveLog(PlayerID author, ThrowCubesResult result)
        {
            if (result.IsDouble)
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices <b>{result.Cube1Result}:{result.Cube2Result}</b> and " +
                    $"gets one more turn because he got a double. Also because he has ReversiveMoveEffect he had just moved revecive");
            }
            else
            {
                Add(author, $"<cp>{(int)author}</cp> has thrown dices <b>{result.Cube1Result}:{result.Cube2Result}</b> " +
                    $"but because he has ReversiveMoveEffect he had just moved revecive.");
            }
        }
        public void AddTripleDoubleDicesLog(PlayerID author, ThrowCubesResult result)
        {
            Add(author, $"<cp>{(int)author}</cp> has thrown dices <b>{result.Cube1Result}:{result.Cube2Result}</b> and " +
                    $"because it is third double he go to prison");
        }
        #endregion

        #region Auction
        public void AddAuctionRiseStakeLog(PlayerID author, int stake)
        {
            Add(author, $"<cp>{(int)author}</cp> has rise stake to <b>{stake}k</b>.");
        }
        public void AddAuctionFallLog(PlayerID author)
        {
            Add(author, $"<cp>{(int)author}</cp> has left auction.");
        }
        public void AddAuctionWinLog(PlayerID author, IBusinessCell cell, int stake)
        {
            Add(author, $"<cp>{(int)author}</cp> won auction and paid {stake}k to buy {cell.Name}");
        }
        #endregion

        #region CellUpgrade
        public void AddUpgradeCellLog(PlayerID author, IBusinessCell cell, bool isUpgrade)
        {
            if (isUpgrade)
            {
                if(cell.Level >= 2)
                {
                    Add(author, $"<cp>{(int)author}</cp> has upgrade {cell.Name} to {cell.Level - 1} stars.");
                }
                else
                {
                    Add(author, $"<cp>{(int)author}</cp> has upgrade {cell.Name} to normal state.");
                }
            }
            else
            {
                if (cell.Level >= 2)
                {
                    Add(author, $"<cp>{(int)author}</cp> has downgrade {cell.Name} to {cell.Level - 1} stars.");
                }
                else if(cell.Level == 1)
                {
                    Add(author, $"<cp>{(int)author}</cp> has downgrade {cell.Name} to normal state.");
                }
                else
                {
                    Add(author, $"<cp>{(int)author}</cp> has pladged {cell.Name}.");
                }
            }
        }
        #endregion

        #region BuyOrAuction
        public void AddBuyCellLog(PlayerID author, IBusinessCell cell)
        {
            Add(author, $"<cp>{(int)author}</cp> has bought {cell.Name} for {cell.Config.Cost}k.");
        }
        public void AddLeaveToAuctionCellLog(PlayerID author, IBusinessCell cell)
        {
            Add(author, $"<cp>{(int)author}</cp> has rejected to buy {cell.Name}.");
        }
        #endregion

        #region Trade
        public void AddMakeProposeLog(PlayerID proposer, PlayerID reciever)
        {
            Add(proposer, $"<cp>{(int)proposer}</cp> do some propose for <cp>{(int)reciever}</cp>.");
        }
        public void AddRejectProposeLog(PlayerID proposer, PlayerID reciever)
        {
            Add(proposer, $"<cp>{(int)reciever}</cp> has rejected propose from {(int)proposer}.");
        }
        public void AddAcceptProposeLog(TradeOfferInfo info)
        {
            Add(info.Proposer, $"<cp>{(int)info.Reciever}</cp> has accept propose from {(int)info.Proposer}.");
        }
        #endregion

        #region StandOnCell
        public void AddPayForfeitLog(ForfeitInfo info)
        {
            if(info.Reciever != PlayerID.Nobody)
            {
                Add(info.Payer, $"<cp>{(int)info.Payer}</cp> pay {(int)info.CashToPay}k to <cp>{(int)info.Reciever}</cp>.");
            }
            else
            {
                Add(info.Payer, $"<cp>{(int)info.Payer}</cp> pay {(int)info.CashToPay}k.");
            }
        }
        public void AddStandOnBusinessCellLog(PlayerID author, IBusinessCell cell)
        {
            if (cell.Owner == PlayerID.Nobody)
            {
                Add(author, $"<cp>{(int)author}</cp> stand on {cell.Name} and think about buy one");
            }
            else if(cell.Owner == author)
            {
                Add(author, $"<cp>{(int)author}</cp> stand on self cell {cell.Name}.");
            }
            else
            {
                Add(author, $"<cp>{(int)author}</cp> stand on {cell.Name} and think about buy one");
            }
        }
        #endregion
    }
}
