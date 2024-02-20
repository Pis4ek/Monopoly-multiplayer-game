using System.Collections.Generic;
using UnityEngine;

namespace Playmode.PlayData
{
    [CreateAssetMenu(fileName = "GameMapConfig", menuName = "Config/GameMap")]
    public class GameMapConfig : ScriptableObject
    {
        public IReadOnlyDictionary<int, ChanceCellInfo> ChanceCells => _chanceCells;
        public IReadOnlyDictionary<int, BusinessCellInfo> BusinessCells => _businessCells;

        private Dictionary<int, ChanceCellInfo> _chanceCells;
        private Dictionary<int, BusinessCellInfo> _businessCells;

        public void Init()
        {
            InitBusinessCells();
            InitChanceCells();
        }

        private void InitBusinessCells()
        {
            _businessCells = new()
            {
                { (int)CellID.B1N1, new BusinessCellInfo("Chanel", BusinessType.Perfumery, 
                new int[] { 0, 20, 100, 300, 900, 1600, 2500 }, 600, 500) }, 
                { (int)CellID.B1N2, new BusinessCellInfo("Hugo Boss", BusinessType.Perfumery, 
                new int[] { 0, 40, 200, 600, 1800, 3200, 4500 }, 600, 500) }, 

                { (int)CellID.B2N1, new BusinessCellInfo("Adidas", BusinessType.Clothes, 
                new int[] { 0, 60, 300, 900, 2700, 4000, 5500 }, 1000, 500) }, 
                { (int)CellID.B2N2, new BusinessCellInfo("Puma", BusinessType.Clothes, 
                new int[] { 0, 60, 300, 900, 2700, 4000, 5500 }, 1000, 500) }, 
                { (int)CellID.B2N3, new BusinessCellInfo("Lacoste", BusinessType.Clothes, 
                new int[] { 0, 80, 400, 1000, 3000, 4500, 6000 }, 1200, 500) }, 

                { (int)CellID.B3N1, new BusinessCellInfo("YouTube", BusinessType.WebService, 
                new int[] { 0, 100, 500, 1500, 4500, 6250, 7500 }, 1400, 750) }, 
                { (int)CellID.B3N2, new BusinessCellInfo("Facebook", BusinessType.WebService, 
                new int[] { 0, 100, 500, 1500, 4500, 6250, 7500 }, 1400, 750) }, 
                { (int)CellID.B3N3, new BusinessCellInfo("Twitter", BusinessType.WebService, 
                new int[] { 0, 120, 600, 1800, 5000, 7000, 9000 }, 1600, 750) },
                 
                { (int)CellID.B4N1, new BusinessCellInfo("CocaCola", BusinessType.Drinks, 
                new int[] { 0, 140, 700, 2000, 5500, 7500, 9500 }, 1800, 1000) },
                { (int)CellID.B4N2, new BusinessCellInfo("Pepsi", BusinessType.Drinks, 
                new int[] { 0, 140, 700, 2000, 5500, 7500, 9500 }, 1800, 1000) },
                { (int)CellID.B4N3, new BusinessCellInfo("Fanta", BusinessType.Drinks, 
                new int[] { 0, 160, 800, 2200, 6000, 8000, 10000 }, 2000, 1000) },
                
                { (int)CellID.B5N1, new BusinessCellInfo("Qatar Airways", BusinessType.Airlines, 
                new int[] { 0, 180, 900, 2500, 7000, 8750, 10500 }, 2200, 1250) },
                { (int)CellID.B5N2, new BusinessCellInfo("Lufthansa", BusinessType.Airlines, 
                new int[] { 0, 180, 900, 2500, 7000, 8750, 10500 }, 2200, 1250) },
                { (int)CellID.B5N3, new BusinessCellInfo("British Airways", BusinessType.Airlines, 
                new int[] { 0, 200, 1000, 3000, 7500, 9250, 11000 }, 2400, 1250) },

                { (int)CellID.B6N1, new BusinessCellInfo("McDonalds", BusinessType.Fastfood, 
                new int[] { 0, 220, 1100, 3300, 8000, 9750, 11500 }, 2600, 1500) },
                { (int)CellID.B6N2, new BusinessCellInfo("Burger King", BusinessType.Fastfood, 
                new int[] { 0, 220, 1100, 3300, 8000, 9750, 11500 }, 2600, 1500) },
                { (int)CellID.B6N3, new BusinessCellInfo("KFC", BusinessType.Fastfood, 
                new int[] { 0, 240, 1200, 3600, 8500, 10250, 12000 }, 2800, 1500) },

                { (int)CellID.B7N1, new BusinessCellInfo("Hilton", BusinessType.Hotels, 
                new int[] { 0, 260, 1300, 3900, 9000, 11000, 12750 }, 3000, 1750) },
                { (int)CellID.B7N2, new BusinessCellInfo("Radisson", BusinessType.Hotels, 
                new int[] { 0, 260, 1300, 3900, 9000, 11000, 12750 }, 3000, 1750) },
                { (int)CellID.B7N3, new BusinessCellInfo("Marriott", BusinessType.Hotels, 
                new int[] { 0, 280, 1500, 4500, 10000, 12000, 14000 }, 3200, 1750) },

                { (int)CellID.B8N1, new BusinessCellInfo("Apple", BusinessType.Electronics, 
                new int[] { 0, 350, 1750, 5000, 11000, 13000, 15000 }, 3500, 2000) },
                { (int)CellID.B8N2, new BusinessCellInfo("Samsung", BusinessType.Electronics, 
                new int[] { 0, 500, 2000, 6000, 14000, 17000, 20000 }, 3500, 2000) },

                { (int)CellID.C1, new BusinessCellInfo("Sega", BusinessType.GameDev, 
                new int[] { 0, 100, 250}, 1500, 9999999) },
                { (int)CellID.C2, new BusinessCellInfo("Electronic Arts", BusinessType.GameDev, 
                new int[] { 0, 100, 250}, 1500, 9999999) },

                
                { (int)CellID.I1, new BusinessCellInfo("Volkswagen", BusinessType.AutoIndustry, 
                new int[] { 0, 250, 500, 1000, 2000}, 2000, 9999999) },
                { (int)CellID.I2, new BusinessCellInfo("Audi", BusinessType.AutoIndustry, 
                new int[] { 0, 250, 500, 1000, 2000}, 2000, 9999999) },
                { (int)CellID.I3, new BusinessCellInfo("Ford", BusinessType.AutoIndustry, 
                new int[] { 0, 250, 500, 1000, 2000}, 2000, 9999999) },
                { (int)CellID.I4, new BusinessCellInfo("Ferrari", BusinessType.AutoIndustry, 
                new int[] { 0, 250, 500, 1000, 2000}, 2000, 9999999) },
            };
        }

        private void InitChanceCells()
        {
            var defaultCell = new ChanceCellInfo("Chance cell", new List<ChanceCellEventType>() 
            {
                ChanceCellEventType.BranchTax,
                ChanceCellEventType.Birthday, 
                ChanceCellEventType.MoveToPrison,
                ChanceCellEventType.SetSkipTurnEffect,
                ChanceCellEventType.SetReverceMoveEffect,
                ChanceCellEventType.SetIncreaceIncomeEffect, 
                ChanceCellEventType.SetDecreaceIncomeEffect,
                ChanceCellEventType.SetIgnoreRentEffect
            });

            _chanceCells = new()
            {
                { (int)CellID.Start, new ChanceCellInfo("Start", ChanceCellEventType.Start)},
                { (int)CellID.Bank1, new ChanceCellInfo("Bank1", ChanceCellEventType.SmallTax)},
                { (int)CellID.Bank2, new ChanceCellInfo("Bank2", ChanceCellEventType.BigTax)},
                { (int)CellID.DownLeftEdge, new ChanceCellInfo("GoToPrison", ChanceCellEventType.MoveToPrison)},

                { (int)CellID.Casino, new ChanceCellInfo("Casino", ChanceCellEventType.Casino)},
                { (int)CellID.TopRightEdge, new ChanceCellInfo("Excursion", ChanceCellEventType.None) },
                { (int)CellID.Prison, new ChanceCellInfo("Prison", ChanceCellEventType.None) },


                { (int)CellID.ChanceTN1, defaultCell },
                { (int)CellID.ChanceTN2, defaultCell },
                { (int)CellID.ChanceRN1, defaultCell },
                { (int)CellID.ChanceDN1, defaultCell },
                { (int)CellID.ChanceLN1, defaultCell },
                { (int)CellID.ChanceLN2, defaultCell },
            };
        }
    }

    [System.Serializable]
    public class BusinessCellInfo : IBaseCellInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public BusinessType Type { get; private set; }
        [field: SerializeField] public int[] IncomeByLevel { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int BranchCost { get; private set; } //филиал
        public int PledgeCost => Cost / 2; //залог
        public int RedemptionCost => (int)(PledgeCost * 1.1f); //выкуп

        public BusinessCellInfo(string name, BusinessType type, int[] incomeByLevel, int cost, int branchCost)
        {
            Name = name;
            Type = type;
            IncomeByLevel = incomeByLevel;
            Cost = cost;
            BranchCost = branchCost;
        }
    }

    public interface IBaseCellInfo
    {
        public string Name { get; }
    }

    [System.Serializable]
    public class ChanceCellInfo : IBaseCellInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<ChanceCellEventType> Events { get; private set; }

        public ChanceCellInfo(string name, List<ChanceCellEventType> events)
        {
            Name = name;
            Events = events;
        }

        public ChanceCellInfo(string name, ChanceCellEventType @event)
        {
            Name = name;
            Events = new() { @event };
        }
    }
}