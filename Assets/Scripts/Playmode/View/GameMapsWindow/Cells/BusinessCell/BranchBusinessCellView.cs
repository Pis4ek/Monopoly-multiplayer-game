using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    public class BranchBusinessCellView : MonoBehaviour
    {
        [SerializeField] Image[] _smallStars;
        [SerializeField] Image _bigStar;

        public void UpdateCellBranchLevel(int level)
        {
            HideAll();
            if (level > 5)
            {
                _bigStar.Activate();
            }
            else if (level > 1)
            {
                for(int i = 0;  i < level - 1; i++)
                {
                    _smallStars[i].Activate();
                }
            }
        }

        private void HideAll()
        {
            _bigStar.Disactivate();
            foreach(var item in _smallStars)
            {
                item.Disactivate();
            }
        }
    }
}
