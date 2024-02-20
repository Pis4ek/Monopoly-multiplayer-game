using Assets.Scripts.Other;
using Playmode.Installers;
using Playmode.PlayData.ClientsData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public abstract class BaseBusinessCellView : CellView, IPointerClickHandler
    {
        [SerializeField] protected Image _header;
        [SerializeField] protected Text _headerText;
        [SerializeField] protected PledgeBusinessCellView _pledgeView;

        protected ClientsBusinessCellData _cellBData;
        protected Converter _viewConfig;
        protected IconProvaider _iconProvaider;
        protected MapContextMenu _contextMenu;
        protected ViewInputStateMachine _viewInputSM;
        protected TradeProposingInputWindow _tradeInputWindow;
        protected ClientsGameData _data;

        [Inject]
        public void Init(Converter viewConfig, IconProvaider iconProvaider,
            MapContextMenu contextMenu, ViewInputStateMachine viewInputSM,
            TradeProposingInputWindow tradeInputWindow, ClientsGameData data)
        {
            _viewConfig = viewConfig;
            _iconProvaider = iconProvaider;
            _contextMenu = contextMenu;
            _viewInputSM = viewInputSM;
            _tradeInputWindow = tradeInputWindow;
            _data = data;
        }


        public override void SetCellData(ClientsCellData cellData)
        {
            base.SetCellData(cellData);
            if (_cellBData != null)
            {
                _cellBData.OnAnyValueChanged -= UpdateInfo;
            }
            _cellBData = cellData as ClientsBusinessCellData;
            _cellBData.OnAnyValueChanged += UpdateInfo;
            _header.color = _viewConfig.BusinessColors[_cellBData.Config.Type];
            Icon.sprite = _iconProvaider[cellData.Name];
            UpdateInfo();
        }

        public abstract void UpdateInfo();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_viewInputSM.State == ViewInputState.Default)
            {
                _contextMenu.Show(_cellBData, transform.position);
            }
            else if (_viewInputSM.State == ViewInputState.Trade)
            {
                _tradeInputWindow.TryAddOrDeleteCell(_cellBData);
            }
        }

        protected override void AdaptateToPosition()
        {
            base.AdaptateToPosition();

            if (cellData.Direction == CellDirection.Down)
            {
                var rotation = _headerText.transform.localRotation.eulerAngles + new Vector3(0, 0, 180);
                _headerText.transform.localRotation = Quaternion.Euler(rotation);
            }
        }
    }
}