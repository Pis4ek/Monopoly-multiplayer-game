using Playmode.PlayData.ClientsData;

namespace Playmode.View
{
    public class InfrastructureCellView : BaseBusinessCellView
    {
        public override void SetCellData(ClientsCellData cellData)
        {
            base.SetCellData(cellData);
            var cells = _data.MapData.GetCellsByBusinessType(_cellBData.BusinessType);

            foreach (var cell in cells)
            {
                if (cell.Index != cellData.Index)
                {
                    cell.OnAnyValueChanged += UpdateHeader;
                }
            }
        }

        public override void UpdateInfo()
        {
            BGImage.color = _viewConfig.PlayerColors[_cellBData.Owner];
            if (_cellBData.Owner == PlayerID.Nobody)
            {
                _headerText.text = $"{_cellBData.Config.Cost}k";
            }
            else
            {
                UpdateHeader();
            }
            _pledgeView.UpdateTurnsCount(_cellBData.TurnsBeforeSelling);
            _pledgeView.UpdateCellLevel(_cellBData.Level);
        }

        private void UpdateHeader()
        {
            if (_cellBData.Level == 0)
            {
                _headerText.text = $"{_cellBData.Config.IncomeByLevel[0]}k";
                return;
            }
            var count = _data.MapData.GetPlayersCellsCountByType(_cellBData.Owner, _cellBData.BusinessType);
            _headerText.text = $"{_cellBData.Config.IncomeByLevel[count]}k";
        }
    }
}