using Playmode.CommandSystem;
using System.Collections.Generic;

namespace Playmode.ServerEnteties
{
    public class Turn
    {
        public PlayerID Actor { get; private set; }
        public int Number { get; private set; }
        public List<ICommand> Commands { get; private set; } = new List<ICommand>();

    }
}