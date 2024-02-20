using Mirror;
using Other;
using Playmode.NetCommunication;
using Playmode.PlayData.ClientsData;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class MapContextMenu : MonoBehaviour, IInputUIElement
    {
        public event Action<NetworkMessage> OnInputEntered;

        [SerializeField] Image _headerImage;
        [SerializeField] Button _upgradeButton;
        [SerializeField] Button _downgradeButton;

        [SerializeField] Text _cellNameText;
        [SerializeField] Text _cellTypeText;

        [Header("Rents")]
        [SerializeField] Text _baseRentText;
        [SerializeField] Text _star1RentText;
        [SerializeField] Text _star2RentText;
        [SerializeField] Text _star3RentText;
        [SerializeField] Text _star4RentText;
        [SerializeField] Text _maxRentText;

        [Header("Costs")]
        [SerializeField] Text _baseCostText;
        [SerializeField] Text _redemptionCostText;
        [SerializeField] Text _pledgeCostText;
        [SerializeField] Text _branchCostText;

        public bool IsShown => gameObject.activeInHierarchy;

        [Inject] private LastGameClientsSession _session;
        [Inject] private Converter _converter;
        [Inject] private MiddleWindow _middleWindow;
        [Inject] private NetMessageSender _messageSender;
        [Inject] private ClientsGameData _data;
        private RectTransform _transform;
        private ClientsBusinessCellData _cell;
        private bool _canUpgrade = false;
        private bool _canDowngrade = false;

        private void Start()
        {
            _transform = transform as RectTransform;
            _upgradeButton.onClick.AddListener(SendUpgrade);
            _downgradeButton.onClick.AddListener(SendDowngrade);
        }

        private void Update()
        {
            if (IsShown)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_transform.GetWorldRect().Contains(Input.mousePosition) == false)
                    {
                        Hide();
                    }
                }
            }
        }

        public void Show(ClientsBusinessCellData cell, Vector2 position)
        {
            _cell = cell;
            var conf = cell.Config;
            _headerImage.color = _converter.BusinessTypeToColor(conf.Type);
            _cellNameText.text = conf.Name;
            _cellTypeText.text = conf.Type.ToString();

            ShowRentsAndCosts(cell);
            TryShowUpgradeForCell(cell);
            TryShowDowngradeForCell(cell);

            transform.position = position;
            MoveToParentRect();

            this.Activate();
        }

        public void Hide()
        {
            _upgradeButton.Disactivate();
            _downgradeButton.Disactivate();
            this.Disactivate();
        }

        public void SetPermission(IInputRequireNetMessage inputMessage)
        {
            if (inputMessage.InputPermissions[InputType.UpgradeCell])
            {
                _canUpgrade = true;
            }
            if (inputMessage.InputPermissions[InputType.DowngradeCell])
            {
                _canDowngrade = true;
            }
        }

        public void HideInput()
        {
            _canUpgrade = false;
            _canDowngrade = false;
        }

        private void SendUpgrade()
        {
            HideInput();
            Hide();
            var mes = new CellUpgradeNetMessage(_session.PlayerID, true, _cell.Index);
            _messageSender.SendMessage(mes);
        }

        private void SendDowngrade()
        {
            HideInput();
            Hide();
            var mes = new CellUpgradeNetMessage(_session.PlayerID, false, _cell.Index);
            _messageSender.SendMessage(mes);
        }

        private void TryShowUpgradeForCell(ClientsBusinessCellData cell)
        {
            if (cell.Level == 6) return;
            if (cell.Level == 0) 
            {
                _upgradeButton.Activate();
                return;
            }
            if (_canUpgrade && cell.Owner == _session.PlayerID)
            {
                _upgradeButton.Activate();
                var cells = _data.MapData.GetCellsByBusinessType(cell.Config.Type);
                var targetLevel = cell.Level;
                foreach (var c in cells)
                {
                    if (c.Owner != _session.PlayerID)
                    {
                        _upgradeButton.Disactivate();
                        return;
                    }

                    if(c.Level < targetLevel)
                    {
                        _upgradeButton.Disactivate();
                        return;
                    }
                }
            }
        }

        private void TryShowDowngradeForCell(ClientsBusinessCellData cell)
        {
            if (cell.Level == 0) return;
            if (_canDowngrade && cell.Owner == _session.PlayerID)
            {
                _downgradeButton.Activate();
            }
        }

        private void ShowRentsAndCosts(ClientsBusinessCellData cell)
        {
            var conf = cell.Config;
            if (conf.Type == BusinessType.GameDev)
            {
                _baseRentText.text = $"{conf.IncomeByLevel[1]}k";
                _star1RentText.text = $"{conf.IncomeByLevel[2]}k";
            }
            else if (conf.Type == BusinessType.AutoIndustry)
            {
                _baseRentText.text = $"{conf.IncomeByLevel[1]}k";
                _star1RentText.text = $"{conf.IncomeByLevel[2]}k";
                _star2RentText.text = $"{conf.IncomeByLevel[3]}k";
                _star3RentText.text = $"{conf.IncomeByLevel[4]}k";
            }
            else
            {
                _baseRentText.text = $"{conf.IncomeByLevel[1]}k";
                _star1RentText.text = $"{conf.IncomeByLevel[2]}k";
                _star2RentText.text = $"{conf.IncomeByLevel[3]}k";
                _star3RentText.text = $"{conf.IncomeByLevel[4]}k";
                _star4RentText.text = $"{conf.IncomeByLevel[5]}k";
                _maxRentText.text = $"{conf.IncomeByLevel[6]}k";
                _branchCostText.text = $"{conf.BranchCost}k";
            }
            _baseCostText.text = $"{conf.Cost}k";
            _redemptionCostText.text = $"{conf.RedemptionCost}k";
            _pledgeCostText.text = $"{conf.PledgeCost}k";
        }

        private void MoveToParentRect()
        {
            var parentTransform = _middleWindow.Transform;
            var rectTransform = transform as RectTransform;

            if (rectTransform.TryGetOffsetToEnterOtherTransform(parentTransform, out var offset))
            {
                rectTransform.position += offset;
            }
        }
    }
}