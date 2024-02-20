using Playmode.PlayData.ClientsData;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.View
{
    public class GameMapPlayersView : IAnimatable
    {
        public event System.Action<AnimationContainer> OnAnimationCreated;

        private Dictionary<int, Rect> _points => _pointsGenerator.PlayerContainers;

        private Dictionary<PlayerID, PlayerOnMapView> _players;
        private GameMapPointsGenerator _pointsGenerator;
        private Transform _playersParent;
        private UIFactory _uiFactory;
        private ClientsGameData _gameData;
        private Converter _viewConfig;
        private List<MapPlayersLayout> _playerContainers = new(5);

        public GameMapPlayersView(GameMapPointsGenerator pointsGenerator, Transform playersParent, 
            UIFactory uiFactory, ClientsGameData gameData, Converter viewConfig)
        {
            _pointsGenerator = pointsGenerator;
            _playersParent = playersParent;
            _uiFactory = uiFactory;
            _gameData = gameData;
            _viewConfig = viewConfig;

            for (int i = 0; i < 5; i++)
            {
                var layout = new MapPlayersLayout();
                _playerContainers.Add(layout);
                layout.OnLayoutUpdated += (newPositions) => { OnLayoutUpdated(layout, newPositions); };
            }
            _playerContainers[0].TrySetPosition(_gameData.MapData[0], _points[0]);

            CreatePlayers();
        }

        private void CreatePlayers()
        {
            _players = new();
            var playersHolder = _gameData.PlayerData;

            for (int i = 0; i < playersHolder.Count; i++)
            {
                var playerID = (PlayerID)i;
                var playerView = _uiFactory.CreatePlayerOnMap(_points[0].center, _playersParent);
                _players.Add(playerID, playerView);

                playerView.Color = _viewConfig.PlayerColors[playerID];
                playerView.PlayerID = playerID;
                playerView.Move(_points[0].center, 1f, 0);

                _gameData.PlayerData.GetPlayerByID(playerID).OnAnyValueChanged += UpdatePlayer;

                _playerContainers[0].Add(playerView);
                //playerView.PositionIndex = 0;
            }
        }

        private void UpdatePlayer(ClientsPlayer playerInfo)
        {
            if (playerInfo.State == PlayerState.Lost)
            {
                _players[playerInfo.ID].Disactivate();
                _gameData.PlayerData.GetPlayerByID(playerInfo.ID).OnAnyValueChanged -= UpdatePlayer;
            }
            else
            {
                var index = playerInfo.CurrentCell;
                var playerView = _players[playerInfo.ID];
                if (index == playerView.PositionIndex) return;

                var layout = GetPlayersLayoutForPosition(index);

                if (layout != null)
                {
                    if (layout.IsEmpty)
                    {
                        layout.TrySetPosition(_gameData.MapData[index], _points[index]);
                    }
                    GetPlayersLayoutForPosition(playerView.PositionIndex).Remove(playerView);
                    layout.Add(playerView);
                }
                else
                {
                    playerView.Move(_points[playerInfo.CurrentCell].center, 1f, index);
                }
            }
        }

        private MapPlayersLayout GetPlayersLayoutForPosition(int positionIndex)
        {
            foreach (var layout in _playerContainers)
            {
                if (layout.PositionIndex == positionIndex)
                    return layout;
            }

            foreach (var layout in _playerContainers)
            {
                if (layout.IsEmpty)
                    return layout;
            }

            Debug.LogError($"{GetType()} has not any empty layout for player!!");
            return null;
        }

        private void OnLayoutUpdated(MapPlayersLayout layout, Dictionary<PlayerID, Vector2> newPositions)
        {
            var container = new AnimationContainer(AnimationType.PlayerOnMap);
            foreach (var item in newPositions)
            {
                var playerView = _players[item.Key];
                var previousPosition = playerView.PositionIndex;
                var newPosition = layout.PositionIndex;
                var indexDifference = Mathf.Abs(playerView.PositionIndex - layout.PositionIndex);

                if (previousPosition.GetDigitNumber(2) == newPosition.GetDigitNumber(2))
                {
                    container.Merge(playerView.AnimatedMove(item.Value, layout.Scale, newPosition));
                }
                else if(newPosition == 40)
                {
                    container.Merge(playerView.DistanceAnimatedMove(item.Value, layout.Scale, newPosition));
                }
                else if (previousPosition.GetDigitNumber(2) < newPosition.GetDigitNumber(2))
                {
                    var transitionalPosition = newPosition.GetDigitNumber(2) * 10;
                    var poitions = new Vector2[] { _points[transitionalPosition].center, item.Value };
                    container.Merge(playerView.AnimatedMove(poitions, layout.Scale, newPosition));
                }
                else
                {
                    if(indexDifference > 12)
                    {
                        var transitionalPosition = newPosition.GetDigitNumber(2) * 10;
                        var poitions = new Vector2[] { _points[transitionalPosition].center, item.Value };
                        container.Merge(playerView.AnimatedMove(poitions, layout.Scale, newPosition));
                    }
                    else
                    {
                        var transitionalPosition = previousPosition.GetDigitNumber(2) * 10;
                        var poitions = new Vector2[] { _points[transitionalPosition].center, item.Value };
                        container.Merge(playerView.AnimatedMove(poitions, layout.Scale, newPosition));
                    }
                }
            }
            OnAnimationCreated?.Invoke(container);
        }
    }
}
