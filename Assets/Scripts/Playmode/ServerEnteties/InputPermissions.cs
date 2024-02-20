using System.Collections;
using System.Collections.Generic;

namespace Playmode.ServerEnteties
{
    public class InputPermissions : IEnumerable
    {
        public IReadOnlyDictionary<InputType, bool> Permissions => _permissions;

        private Dictionary<InputType, bool> _permissions;

        public InputPermissions()
        {
            _permissions = new()
            {
                { InputType.ThrowCubes, false },
                { InputType.Prison, false },
                { InputType.PrisonWithoutEscape, false },
                { InputType.Casino, false },
                { InputType.Auction, false },
                { InputType.BuyOrAuction, false },
                { InputType.TradeProposing, false },
                { InputType.TradeProposeAccepting, false },
                { InputType.UpgradeCell, false },
                { InputType.DowngradeCell, false },
                { InputType.Forfeit, false }
            };
        }

        public bool this[InputType type]
        {
            get => _permissions[type];
            set => _permissions[type] = value;
        }

        public void Activate(InputType type) => _permissions[type] = true;

        public void Disactivate(InputType type) => _permissions[type] = false;

        public IEnumerator GetEnumerator() => _permissions.GetEnumerator();
    }
}
