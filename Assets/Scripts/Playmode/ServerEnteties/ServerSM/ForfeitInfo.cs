namespace Playmode.ServerEnteties
{
    public class ForfeitInfo
    {
        public int CashToPay;
        public PlayerID Payer;
        public PlayerID Reciever = PlayerID.Nobody;

        public ForfeitInfo() { }
        public ForfeitInfo(int cashToPay, PlayerID payer, PlayerID reciever = PlayerID.Nobody)
        {
            CashToPay = cashToPay;
            Payer = payer;
            Reciever = reciever;
        }

        public override string ToString()
        {
            return $"ForfeitInfo: {Payer} must pay {CashToPay}k to {Reciever}";
        }
    }
}
