using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class TradeCompanyLayout : MonoBehaviour
    {
        [SerializeField] Transform _content;

        [Inject] UIFactory _factory;
        private Dictionary<int, TradeCellPanel> _companies = new();
        private List<TradeCellPanel> _cells = new(5);

        private void Awake()
        {
            for(int i = 0; i < 5; i++)
            {
                var obj = _factory.CreateTradeCellPanel(_content);
                obj.Disactivate();
                _cells.Add(obj);
            }
        }

        public void AddElement(ClientsBusinessCellData company)
        {
            var cell = GetCellPanel();
            _companies.Add(company.Index, cell);
            cell.Activate();
            cell.SetCompany(company);
        }

        public void RemoveElement(ClientsBusinessCellData company)
        {
            _companies[company.Index].Disactivate();
            _companies.Remove(company.Index);
        }

        public bool Contains(ClientsBusinessCellData company) => _companies.ContainsKey(company.Index);

        public void Clear()
        {
            _companies.Clear();
            foreach(var cell in _cells)
            {
                cell.Disactivate();
            }

        }

        private TradeCellPanel GetCellPanel()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].IsActive() == false)
                {
                    return _cells[i];
                }
            }

            var cell = _factory.CreateTradeCellPanel(_content);
            cell.Disactivate();
            return cell;
        }
    }
}