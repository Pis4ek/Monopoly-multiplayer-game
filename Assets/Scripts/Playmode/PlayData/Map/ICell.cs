using Playmode.CommandSystem;
using System.Collections.Generic;

namespace Playmode.PlayData
{
    public interface ICell
    {
        public string Name { get; }
        public int Index { get; }
    }
}
