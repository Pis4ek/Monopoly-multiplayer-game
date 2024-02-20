using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Playmode.View
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayerOnMapView : MonoBehaviour
    {
        [SerializeField] Image _image;

        public Color Color { 
            get => _image.color; 
            set => _image.color = value; 
        }
        public PlayerID PlayerID { get; set; }
        public int PositionIndex { get; set; }
        public RectTransform RectTransform { get; private set; }
        public Rect Rect => RectTransform.rect;

        private AnimationContainer _container;

        public void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        public void Move(Vector2 position, float scale, int index)
        {
            transform.localPosition = position;
            PositionIndex = index;
            transform.localScale = new Vector3(scale, scale, 1f);
        }

        public AnimationContainer AnimatedMove(Vector2 position, float scale, int positionIndex)
        {
            float time;
            if(positionIndex > PositionIndex)
            {
                time = (positionIndex - PositionIndex) * 0.1f;
            }
            else if(positionIndex == PositionIndex)
            {
                time = 0.3f;
            }
            else
            {
                time = (40 + positionIndex - PositionIndex) * 0.1f;
            }

            return CreateContainer(new[] { position }, time, scale, time / 2, positionIndex);
        }

        public AnimationContainer AnimatedMove(Vector2[] positions, float scale, int positionIndex)
        {
            float fullTime;
            if (positionIndex > PositionIndex) 
                fullTime = (positionIndex - PositionIndex) * 0.1f;
            else if (positionIndex == PositionIndex) 
                fullTime = 0.3f;
            else 
                fullTime = (40 + positionIndex - PositionIndex) * 0.1f;

            return CreateContainer(positions, fullTime / positions.Length, scale, fullTime / 2, positionIndex);
        }

        public AnimationContainer DistanceAnimatedMove(Vector2 position, float scale, int positionIndex)
        {
            var time = 0.3f + 0.001f * Vector2.Distance(position, (Vector2)transform.localPosition);
            return CreateContainer(new[] { position }, time, scale, time / 2, positionIndex);
        }

        private AnimationContainer CreateContainer(Vector2[] positions, float eachPostime, float scale, float scaleTime, int positionIndex)
        {
            _container = new AnimationContainer(AnimationType.PlayerOnMap);

            var moving = DOTween.Sequence();
            foreach(var pos in positions)
            {
                moving.Append(transform.DOLocalMove(pos, eachPostime).SetEase(Ease.InCubic));
            }
            _container.Add(moving);

            if (scale != transform.localScale.x)
            {
                var scaling = DOTween.Sequence();
                scaling.Append(transform.DOScale(new Vector3(scale, scale, 1f), scaleTime).SetEase(Ease.InCubic));
                _container.Add(scaling);
            }

            PositionIndex = positionIndex;
            return _container;
        }
    }
}
