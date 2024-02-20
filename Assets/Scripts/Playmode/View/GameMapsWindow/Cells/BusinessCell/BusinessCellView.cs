using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using UnityEngine;

namespace Playmode.View
{
    public class BusinessCellView : BaseBusinessCellView
    {
        [SerializeField] BranchBusinessCellView _branchView;

        public override void UpdateInfo()
        {
            BGImage.color = _viewConfig.PlayerColors[_cellBData.Owner];
            if (_cellBData.Owner == PlayerID.Nobody)
            {
                _headerText.text = $"{_cellBData.Config.Cost}k";
            }
            else
            {
                _headerText.text = $"{_cellBData.Config.IncomeByLevel[_cellBData.Level]}k";
            }
            _pledgeView.UpdateTurnsCount(_cellBData.TurnsBeforeSelling);
            _pledgeView.UpdateCellLevel(_cellBData.Level);
            _branchView.UpdateCellBranchLevel(_cellBData.Level);
        }
        public override void SetCellData(ClientsCellData cellData)
        {
            base.SetCellData(cellData);
        }
    }
}