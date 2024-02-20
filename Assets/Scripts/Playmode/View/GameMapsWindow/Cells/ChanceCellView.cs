using Assets.Scripts.Other;
using Playmode.Installers;
using Zenject;

namespace Playmode.View
{
    public class ChanceCellView : CellView
    {
        [Inject] IconProvaider _provaider;

        private new void Start()
        {
            base.Start();
            Icon.sprite = _provaider["ChanceCell"];
        }
    }
}