namespace Playmode.PlayData
{
    public struct CellInfoPackage
    {
        public int Index;
        public PlayerID Owner;
        public int Level;
        public int TurnsBeforeSelling;

        public CellInfoPackage(IBusinessCell cell)
        {
            Index = cell.Index;
            Owner = cell.Owner;
            Level = cell.Level;
            TurnsBeforeSelling = cell.TurnsBeforeSelling;
        }
    }
}
