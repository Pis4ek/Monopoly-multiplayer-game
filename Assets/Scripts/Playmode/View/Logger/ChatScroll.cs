using Playmode.PlayData.ClientsData;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Playmode.View
{
    public class ChatScroll : MonoBehaviour
    {
        [SerializeField] ScrollRect _scrollView;

        private UIFactory _uiFactory;
        private ClientsLogData _data;
        private ObjectPool<TextMeshProUGUI> _chatScrollPool;

        public void Init(UIFactory uiFactory, ClientsGameData data)
        {
            _uiFactory = uiFactory;

            //_chatScrollPool = new(Create);

            _scrollView.onValueChanged.AddListener((v1) => { });
            _data = data.LogData;

            _data.OnNewLogAdded += OnNewLogAdded;
        }

        private void OnNewLogAdded()
        {
            _uiFactory.CreateTextLogElement(_scrollView.content);
        }

        private void Create() 
        {
        }
    }
}