using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class ChanceCell : ICell
    {
        public string Name { get; private set; }
        public int Index { get; private set; }
        public IReadOnlyList<ChanceCellEventType> Events { get; private set; }

        public ChanceCell(int index, ChanceCellInfo info)
        {
            Index = index;
            Events = info.Events;
            Name = info.Name;
        }

        public ChanceCellEventType GetRandomEvent()
        {
            return Events[UnityEngine.Random.Range(0, Events.Count)];
        }
    }
}