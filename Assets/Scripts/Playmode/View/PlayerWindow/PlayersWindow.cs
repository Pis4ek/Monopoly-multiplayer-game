using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Playmode.View
{
    public class PlayersWindow : MonoBehaviour
    {
        [SerializeField] Transform _panelsParent;

        private Dictionary<PlayerID, PlayerInfoPanel> _playerElements = new();
        private ClientsGameData _gameData;
        private UIFactory _uiFactory;
        private Converter _viewConfig;
        private PlayerID _waitedPlayer;

        [Inject]
        public void Init(ClientsGameData gameData, UIFactory uIFactory, Converter viewConfig)
        {
            _gameData = gameData;
            _uiFactory = uIFactory;
            _viewConfig = viewConfig;
            CreatePlayerPanels();
        }

        public void SetWaitedPlayer(PlayerID player, DateTime waitedDate)
        {
            _playerElements[_waitedPlayer].DisactivateWaiting();
            _playerElements[_waitedPlayer].BGImage.color = new Color(0.3f, 0.3f, 0.3f, 0.4f);

            _waitedPlayer = player;
            var playerColor = _viewConfig.PlayerIDToColor(player);
            playerColor.a = 0.4f;
            _playerElements[_waitedPlayer].BGImage.color = playerColor;
            _playerElements[_waitedPlayer].ActivateWaiting(waitedDate);
        }

        private void CreatePlayerPanels()
        {
            for(int i = 0; i < _gameData.PlayerData.Count; i++)
            {
                var playerID = (PlayerID)i;
                var playerData = _gameData.PlayerData.GetPlayerByID(playerID);
                var playerPanel = _uiFactory.CreatePlayerInfoPanel(Vector2.zero, playerData, _panelsParent);
                
                _playerElements.Add(playerID, playerPanel);
            }
        }
    }
}