using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Playmode.View
{
    public class LogsContainer : MonoBehaviour
    {
        [SerializeField] Transform _unactiveObjectsContainer;

        public PlayerID LogsOwnerID { get; private set; }

        private ObjectPool<TextMeshProUGUI> _textsPool;

        public void Init()
        {
            _textsPool = new(CreateText, defaultCapacity: 6);
        }

        public void SwitchOwner(PlayerID owner)
        {
            LogsOwnerID = owner;

            _textsPool.Clear();
        }

        public void Add(string text)
        {
            _textsPool.Get().text = text;
        }

        private TextMeshProUGUI CreateText()
        {
            var obj = Instantiate(new GameObject(), _unactiveObjectsContainer);
            return obj.AddComponent<TextMeshProUGUI>();
        }
    }
}