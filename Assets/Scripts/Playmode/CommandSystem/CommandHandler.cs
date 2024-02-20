using Playmode.PlayData;
using Playmode.ServerEnteties;
using System.Collections.Generic;
using Zenject;

namespace Playmode.CommandSystem
{
    public class CommandHandler
    {
        [Inject] private GameData _gameData;

        public void Handle(ICommand command)
        {
            if(command == null) return;
            command.OnNeedExecuteOtherCommand += Handle;
            command?.Execute(_gameData);
            command.OnNeedExecuteOtherCommand -= Handle;
        }

        public void Handle(ICollection<ICommand> commands)
        {
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    Handle(command);
                }
            }
        }
    }
}