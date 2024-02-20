using System.Collections.Generic;
using UnityEngine;

public class Converter
{
    public IReadOnlyDictionary<PlayerID, Color> PlayerColors => _playerColors;
    public IReadOnlyDictionary<BusinessType, Color> BusinessColors => _businessColors;

    private Dictionary<PlayerID, Color> _playerColors;
    private Dictionary<BusinessType, Color> _businessColors;

    public Converter()
    {
        SetPlayerColors();
        SetBusinessColors();
    }

    public Color PlayerIDToColor(PlayerID id) => _playerColors[id];
    public Color BusinessTypeToColor(BusinessType type) => _businessColors[type];

    #region Inits
    private void SetPlayerColors()
    {
        _playerColors = new()
        {
            { PlayerID.Player1, new Color(222 / 255f, 79 / 255f, 87 / 255f) },
            { PlayerID.Player2, new Color(69 / 255f, 178 / 255f, 231 / 255f) },
            { PlayerID.Player3, new Color(135 / 255f, 199 / 255f, 94 / 255f) },
            { PlayerID.Player4, new Color(181 / 255f, 131 / 255f, 232 / 255f) },
            { PlayerID.Player5, new Color(125 / 255f, 154 / 255f, 175 / 255f) },
            { PlayerID.Nobody, Color.white }
        };
    }
    private void SetBusinessColors()
    {
        _businessColors = new()
        {
            { BusinessType.Perfumery, new Color(234 / 255f, 134f / 255f, 192 / 255f) },
            { BusinessType.Clothes, new Color(223 / 255f, 179 / 255f, 69 / 255f) },
            { BusinessType.WebService, new Color(59 / 255f, 178 / 255f, 149 / 255f) },
            { BusinessType.Drinks, new Color(74 / 255f, 131 / 255f, 206 / 255f) },
            { BusinessType.Airlines, new Color(141 / 255f, 193 / 255f, 89 / 255f) },
            { BusinessType.Fastfood, new Color(84 / 255f, 194 / 255f, 231 / 255f) },
            { BusinessType.Hotels, new Color(150 / 255f, 123 / 255f, 217 / 255f) },
            { BusinessType.Electronics, new Color(101 / 255f, 109 / 255f, 120 / 255f) },
            { BusinessType.AutoIndustry, new Color(246 / 255f, 86 / 255f, 65 / 255f) },
            { BusinessType.GameDev, new Color(126 / 255f, 29 / 255f, 20 / 255f) }
        };
    }
    #endregion
}
