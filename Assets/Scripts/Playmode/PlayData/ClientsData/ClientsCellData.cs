namespace Playmode.PlayData.ClientsData
{
    public class ClientsCellData
    {
        public string Name { get; private set; }
        public int Index { get; private set; }
        public CellType Type { get; private set; }
        public CellDirection Direction { get; private set; }

        public ClientsCellData(string name, int index, CellType type, CellDirection direction)
        {
            Name = name;
            Index = index;
            Type = type;
            Direction = direction;
        }
    }
}
