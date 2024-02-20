using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.View
{
    public class MapPlayersLayout
    {
        public event Action<Dictionary<PlayerID, Vector2>> OnLayoutUpdated;

        public int PositionIndex { get; private set; }
        public Rect PositionRect { get; private set; }
        public int Count => _players.Count; 
        public bool IsEmpty => _players.Count == 0;
        public float Scale { get; private set; } = 1f;

        private List<PlayerOnMapView> _players = new(5);
        private CustomLayout _layout = new();
        private ClientsCellData _cell;
        private List<Vector2> _points = new();

        public bool TrySetPosition(ClientsCellData cell, Rect cellRect)
        {
            if (IsEmpty)
            {
                PositionRect = cellRect;
                PositionIndex = cell.Index;
                _cell = cell;
                return true;
            }
            return false;
        }

        public void Add(PlayerOnMapView mapView)
        {
            _players.Add(mapView);
            UpdateLayout();
        }

        public void Remove(PlayerOnMapView mapView)
        {
            _players.Remove(mapView);
            if (Count == 0) return;
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (Count == 1)
            {
                Scale = 1f;
                OnLayoutUpdated?.Invoke(new() { { _players[0].PlayerID, PositionRect.center } });
                return;
            }

            var playersList = new List<RectTransform>(_players.Count);
            foreach (var p in _players)
                playersList.Add(p.RectTransform);

            float scale;
            if (_cell.Type == CellType.Edge)
            {
                _points = _layout.CalcRoundLayout(playersList, PositionRect, out scale);
            }
            else
            {
                if (_cell.Direction == CellDirection.Top || _cell.Direction == CellDirection.Down)
                {
                    _points = _layout.CalcVerticalLayout(playersList, PositionRect, out scale);
                }
                else
                {
                    _points = _layout.CalcHorizontalLayout(playersList, PositionRect, out scale);
                }
            }
            Scale = scale;

            var result = new Dictionary<PlayerID, Vector2>(5);
            for (int i = 0; i < _players.Count; i++)
            {
                result.Add(_players[i].PlayerID, _points[i]);
            }

            OnLayoutUpdated?.Invoke(result);
        }
    }
}
