using NaughtyAttributes;
using Playmode.CommandSystem;
using Playmode.PlayData;
using Playmode.ServerEnteties;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Testing_Objects
{
    public class CellDebugDataRewriter : MonoBehaviour
    {
        [Inject(Optional = true)] GameData _data;
        [Inject(Optional = true)] CommandHandler _commandHandler;
        [Inject(Optional = true)] UpdatingDataCollector _collector;

        [SerializeField] CellID _cellID;
        [SerializeField] PlayerID _owner;

        [Button("Set Owner")]
        private void SetOwner()
        {
            _commandHandler.Handle(new ChangeBusinessOwnerCommand((int)_cellID, _owner));
            _collector.SendUpdateMessage();
        }
    }
}