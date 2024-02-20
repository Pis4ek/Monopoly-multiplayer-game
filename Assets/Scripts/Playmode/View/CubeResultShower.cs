using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Playmode.View
{
    public class CubeResultShower : MonoBehaviour, IAnimatable
    {
        public event System.Action<AnimationContainer> OnAnimationCreated;

        [SerializeField] Camera _renderCamera;
        [SerializeField] Image _cubesRenderTexture;
        [SerializeField] GameObject _cube1;
        [SerializeField] GameObject _cube2;
        [SerializeField] Vector3Int _rotator = new Vector3Int(720, 360);
        [SerializeField] float _rotatingAnimDuration = 0.5f;
        [SerializeField] float _waitingAnimDuration = 0.5f;

        private AnimationContainer _container;
        private Vector3[] _adders = new Vector3[6];
        private Vector3 _cube1DefaultPosition;
        private Vector3 _cube2DefaultPosition;
        private int _endAnimCounter = 0;

        private void Start()
        {
            _adders[0] = new Vector3(90, 0, 0);
            _adders[1] = new Vector3(0, 270, 0);
            _adders[2] = new Vector3(270, 0, 0);
            _adders[3] = new Vector3(0, 0, 0);
            _adders[4] = new Vector3(180, 0, 0);
            _adders[5] = new Vector3(0, 90, 0);

            _cube1DefaultPosition = _cube1.transform.position;
            _cube2DefaultPosition = _cube2.transform.position;
        }

        public void ShowResult(ThrowCubesResult result)
        {

            var cube1Rotator = _rotator + _adders[result.Cube1Result - 1];
            var cube2Rotator = _rotator + _adders[result.Cube2Result - 1];

            _container = new AnimationContainer(AnimationType.Cubes);

            Animate(_cube1.transform, cube1Rotator);
            Animate(_cube2.transform, cube2Rotator);

            _container.OnComplete += OnCubeAnimationEnded;
            _container.OnStart += () =>
            {
                _cube1.SetActive(true);
                _cube2.SetActive(true);
                _cubesRenderTexture.Activate();
                _renderCamera.Activate();
            };

            OnAnimationCreated?.Invoke(_container);
        }

        private void Animate(Transform transform, Vector3 endRotation)
        {
            //var endPos = transform.position + new Vector3(Random.Range(0.3f, 2.4f), Random.Range(0.3f, 2.4f));
            var endPos = transform.position + new Vector3(1.5f, 2f);
            var movingTween = transform.DOMove(endPos, _rotatingAnimDuration).Play();
            _container.Add(DOTween.Sequence().Append(movingTween));

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DORotate(endRotation, _rotatingAnimDuration, RotateMode.LocalAxisAdd));
            sequence.AppendInterval(_waitingAnimDuration);
            //sequence.onComplete += OnCubeAnimationEnded;
            _container.Add(sequence);
        }

        private void OnCubeAnimationEnded()
        {
            _endAnimCounter = 10;
            if (_endAnimCounter > 1)
            {
                _endAnimCounter = 0;
                _cube1.SetActive(false);
                _cube2.SetActive(false);
                _cube1.transform.rotation = Quaternion.Euler(Vector3.zero);
                _cube2.transform.rotation = Quaternion.Euler(Vector3.zero);
                _cube1.transform.position = _cube1DefaultPosition;
                _cube2.transform.position = _cube2DefaultPosition;

                _cubesRenderTexture.Disactivate();
                _renderCamera.Disactivate();
            }
        }
    }
}