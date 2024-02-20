using UnityEngine;

namespace Playmode.View
{
    [RequireComponent(typeof(RectTransform))]
    public class MiddleWindow : MonoBehaviour
    {
        public RectTransform Transform => transform as RectTransform;
        public Rect Rect => Transform.rect;
    }
}