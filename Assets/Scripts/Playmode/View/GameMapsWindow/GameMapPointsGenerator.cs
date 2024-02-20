using Playmode.PlayData.ClientsData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.View
{
    public class GameMapPointsGenerator
    {
        public event Action OnPointsRegenerated;

        public Dictionary<int, Vector2> CellPoints { get; private set; }
        public Dictionary<int, Rect> PlayerContainers { get; private set; }

        private ClientsGameData _gameData;
        private Vector2 _center;
        private float _cellWidth;
        private float _offset;
        private float _edgeOffset;

        public GameMapPointsGenerator(ClientsGameData gameData, Vector2 center, float offset)
        {
            _gameData = gameData;
            _center = Vector2.zero;
            _cellWidth = 75f;
            _offset = offset;
            _edgeOffset = 5 * _cellWidth + _cellWidth / 2 + 5 * _offset;

            //var tstWigth = (1080 - 75 * 0.4f * 2 - _offset * 10) / 13;

            GeneratePoints();
            GeneratePlayerPositions(); 
            //LogPoints();
        }

        private void GeneratePoints()
        {
            CellPoints = new();
            Vector2 startPosition = _center;
            startPosition.y += _edgeOffset;
            startPosition.x -= _edgeOffset;

            var edgeToCellOffset = _cellWidth + _cellWidth / 2 + _offset;
            var cellToCellOffset = _cellWidth + _offset;

            CellPoints.Add(0, startPosition);

            #region TopCells 1-10
            var position = CellPoints[0] + new Vector2(edgeToCellOffset, 0);
            CellPoints.Add(1, position);

            for (int i = 2; i < 10; i++)
            {
                var pos = CellPoints[i - 1] + new Vector2(cellToCellOffset, 0);
                CellPoints.Add(i, pos);
            }

            position = CellPoints[9] + new Vector2(edgeToCellOffset, 0);
            CellPoints.Add(10, position);
            #endregion

            #region TopCells 11-20
            position = CellPoints[10] - new Vector2(0, edgeToCellOffset);
            CellPoints.Add(11, position);

            for (int i = 12; i < 20; i++)
            {
                var pos = CellPoints[i - 1] - new Vector2(0, cellToCellOffset);
                CellPoints.Add(i, pos);
            }

            position = CellPoints[19] - new Vector2(0, edgeToCellOffset);
            CellPoints.Add(20, position);
            #endregion

            #region TopCells 21-30
            position = CellPoints[20] - new Vector2(edgeToCellOffset, 0);
            CellPoints.Add(21, position);

            for (int i = 22; i < 30; i++)
            {
                var pos = CellPoints[i - 1] - new Vector2(cellToCellOffset, 0);
                CellPoints.Add(i, pos);
            }

            position = CellPoints[29] - new Vector2(edgeToCellOffset, 0);
            CellPoints.Add(30, position);
            #endregion

            #region TopCells 31-40
            position = CellPoints[30] + new Vector2(0, edgeToCellOffset);
            CellPoints.Add(31, position);

            for (int i = 32; i < 40; i++)
            {
                var pos = CellPoints[i - 1] + new Vector2(0, cellToCellOffset);
                CellPoints.Add(i, pos);
            }
            #endregion
        }

        public void GeneratePlayerPositions()
        {
            PlayerContainers = new();
            for (int i = 0; i < CellPoints.Count; i++)
            {
                Rect rect;
                var cell = _gameData.MapData.GetCellByIndex(i);

                if (cell.Type == CellType.Edge)
                {
                    var position = CellPoints[i] - new Vector2(_cellWidth, _cellWidth);
                    var size = new Vector2(_cellWidth, _cellWidth) * 2;
                    rect = new Rect(position, size);
                }
                else if (cell.Direction == CellDirection.Top || cell.Direction == CellDirection.Down)
                {
                    var position = CellPoints[i] - new Vector2(_cellWidth / 2, _cellWidth);
                    var size = new Vector2(_cellWidth, _cellWidth * 2);
                    rect = new Rect(position, size);
                }
                else
                {
                    var position = CellPoints[i] - new Vector2(_cellWidth, _cellWidth / 2);
                    var size = new Vector2(_cellWidth * 2, _cellWidth);
                    rect = new Rect(position, size);
                }

                PlayerContainers.Add(i, rect);
            }

            var prisonRect = PlayerContainers[10];
            PlayerContainers.Remove(10);

            var prisonSmallRectSize =  new Vector2(_cellWidth, _cellWidth);
            PlayerContainers.Add(10, new Rect(prisonRect.center, prisonSmallRectSize));
            PlayerContainers.Add(40, new Rect(prisonRect.position, prisonSmallRectSize));
        }

        private void LogPoints()
        {
            var msg = "";
            for (int i = 0; i < CellPoints.Count; i++)
            {
                msg += $"Index:{i}, CPoint{CellPoints[i]}, PPoint{PlayerContainers[i].center}\n";
            }
            msg += $"Prison PPoint = {PlayerContainers[40]}";
            Debug.Log(msg);
        }
    }
}
