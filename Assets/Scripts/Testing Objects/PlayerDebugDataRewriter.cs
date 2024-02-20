using NaughtyAttributes;
using Playmode.CommandSystem;
using Playmode.PlayData;
using Playmode.ServerEnteties;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Testing_Objects
{
    public class PlayerDebugDataRewriter : MonoBehaviour
    {
        [Inject(Optional = true)] GameData _data;
        [Inject(Optional = true)] CommandHandler _commandHandler;
        [Inject(Optional = true)] UpdatingDataCollector _collector;

        [SerializeField] PlayerID _playerID;
        [SerializeField] int position;
        [SerializeField] int cash;

        [Button("Set position")]
        private void SetPosition()
        {
            if (_data.TryGetPlayerByID(_playerID, out var p))
            {
                _commandHandler.Handle(new SetPositionCommand(position, _playerID));
                _collector.SendUpdateMessage();
            }
        }

        [Button("Add cash")]
        private void SetCash()
        {
            if (_data.TryGetPlayerByID(_playerID, out var p))
            {
                _commandHandler.Handle(new ChangeCashCommand(_playerID, cash));
                _collector.SendUpdateMessage();
            }
        }
    }
}