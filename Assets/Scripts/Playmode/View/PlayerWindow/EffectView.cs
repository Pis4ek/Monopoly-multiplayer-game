using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    public class EffectView : MonoBehaviour
    {
        [SerializeField] Text _counter;
        [SerializeField] Image _bgImage;

        public Color Color { get => _bgImage.color; set => _bgImage.color = value; }

        public void SetCounter(int counter)
        {
            _counter.text = counter.ToString();
        }
    }
}
