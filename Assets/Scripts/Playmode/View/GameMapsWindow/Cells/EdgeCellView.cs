using Assets.Scripts.Other;
using Playmode.Installers;
using Zenject;

namespace Playmode.View
{
    public class EdgeCellView : CellView
    {
        [Inject] IconProvaider _provaider;

        private new void Start()
        {
            base.Start();
            Icon.sprite = _provaider[cellData.Name];
        }

        protected override void AdaptateToPosition() { }
    }
}