using Assets.Scripts.Other;
using Playmode.Installers;
using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Playmode.View
{
    public class TradeCellPanel : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] Text _nameText;
        [SerializeField] Text _priceText;

        [Inject] IconProvaider _iconProvaider;

        public void SetCompany(ClientsBusinessCellData company)
        {
            _icon.sprite = _iconProvaider.GetIcon(company.Name);
            _nameText.text = company.Name;
            _priceText.text = company.Config.Cost.ToString();
        }
    }
}