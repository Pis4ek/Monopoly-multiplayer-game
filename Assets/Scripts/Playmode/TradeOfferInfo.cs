using System.Collections.Generic;

namespace Playmode
{
    [System.Serializable]
    public class TradeOfferInfo
    {
        public PlayerID Proposer;
        public PlayerID Reciever;
        public int Surcharge = 0;
        public PlayerID Payer;

        public List<int> CellsToProposer = new();
        public List<int> CellsToReciever = new();

        public override string ToString()
        {
            string prpouserIndexies = "";
            foreach (int i in CellsToProposer)
            {
                prpouserIndexies += i.ToString() + ",";
            }
            string recieverIndexies = "";
            foreach (int i in CellsToReciever)
            {
                recieverIndexies += i.ToString() + ",";
            }

            return $"Proposer - {Proposer}\n" +
                $"Reciever - {Reciever}\n" +
                $"Payer - {Payer}\n" +
                $"Surcharge - {Surcharge}\n" +
                $"CellsToProposer - {prpouserIndexies}\n" +
                $"CellsToProposer - {recieverIndexies}\n";
        }
    }
}
