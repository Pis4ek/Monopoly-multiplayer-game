namespace Playmode.PlayData
{
    public class Log
    {
        public PlayerID Author { get; private set; }
        public string Text { get; private set; }
        public int Index { get; private set; }

        public Log(PlayerID author, string text, int index)
        {
            Author = author;
            Text = text;
            Index = index;
        }
    }
}
