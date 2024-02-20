using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BusinessCellConfig", menuName = "Config/BusinessCell")]
public class BusinessCellConfig : CellConfig
{
    [field: SerializeField] public BusinessType Type { get; private set; }

    [field: SerializeField] public int BaseIncome { get; private set; }
    [ValidateInput("Has7Element", "Count must be 7")]
    [field: SerializeField] public int[] LevelScaledIncome;

    [field: SerializeField] public int BaseCost { get; private set; }
    public int PledgeCost => BaseCost / 2; //залог
    public int RedemptionCost => (int)(PledgeCost * 1.1f); //выкуп
    [field: SerializeField] public int BranchCost { get; private set; } //филиал

    private bool Has5Element(int[] list) => list.Length == 5;
}
