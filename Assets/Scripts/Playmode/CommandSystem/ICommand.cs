using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public interface ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public void Execute(GameData gameData);
    }
}
