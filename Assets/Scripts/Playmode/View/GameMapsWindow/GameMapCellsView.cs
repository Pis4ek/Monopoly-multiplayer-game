using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.View
{
    public class GameMapCellsView
    {
        private Dictionary<int, CellView> _cells;
        private readonly GameMapPointsGenerator _pointsGenerator;
        private readonly Transform _cellsParent;
        private readonly UIFactory _uiFactory;
        private readonly ClientsGameData _gameData;

        public GameMapCellsView(GameMapPointsGenerator pointsGenerator, Transform cellsParent, 
            UIFactory uiFactory, ClientsGameData gameData)
        {
            _pointsGenerator = pointsGenerator;
            _cellsParent = cellsParent;
            _uiFactory = uiFactory;
            _gameData = gameData;
            SpawnCellsView();
            //SetDataForCells();
        }

        private void SpawnCellsView()
        {
            _cells = new();

            for (int i = 0; i < 40; i++)
            {
                var cell = _gameData.MapData[i];

                if (cell.Type == CellType.Edge)
                {
                    var view = _uiFactory.CreateEdgeCell(_pointsGenerator.CellPoints[i], _cellsParent);
                    _cells.Add(i, view);
                    view.SetCellData(cell);
                }
                else if (cell.Type == CellType.Chance)
                {
                    var view = _uiFactory.CreateChanceCell(_pointsGenerator.CellPoints[i], _cellsParent);
                    _cells.Add(i, view);
                    view.SetCellData(cell);
                }
                else
                {
                    if(cell is ClientsBusinessCellData cellData)
                    {
                        BaseBusinessCellView view;
                        if(cellData.BusinessType == BusinessType.GameDev)
                        {
                            view = _uiFactory.CreateCommunalCell(_pointsGenerator.CellPoints[i], _cellsParent);
                        }
                        else if(cellData.BusinessType == BusinessType.AutoIndustry)
                        {
                            view = _uiFactory.CreateInfrastructureCell(_pointsGenerator.CellPoints[i], _cellsParent);
                        }
                        else
                        {
                            view = _uiFactory.CreateBusinessCell(_pointsGenerator.CellPoints[i], _cellsParent);

                        }
                        _cells.Add(i, view);
                        view.SetCellData(cell);
                    }
                }
            }
        }
    }
}
