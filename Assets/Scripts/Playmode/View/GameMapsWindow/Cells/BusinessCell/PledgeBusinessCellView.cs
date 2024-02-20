using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    public class PledgeBusinessCellView : MonoBehaviour
    {
        [SerializeField] Image _pledgeView;
        [SerializeField] Text _pledgeText;

        public void UpdateTurnsCount(int count)
        {
            _pledgeText.text = count.ToString();
        }

        public void UpdateCellLevel(int level)
        {
            if(level == 0)
            {
                this.Activate();
            }
            else
            {
                this.Disactivate();
            }
        }
    }
}
