using NaughtyAttributes;
using Playmode.PlayData;
using Playmode.PlayData.ClientsData;
using System;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    [RequireComponent(typeof(RectTransform))]
    public class GameMapWindow : MonoBehaviour, IAnimatable
    {
        public event Action<AnimationContainer> OnAnimationCreated;

        [SerializeField] Transform _playersParent;
        [SerializeField] Transform _cellsParent;
        [SerializeField] float _offset;

        private GameMapPointsGenerator _generator;
        private GameMapCellsView _cellsView;
        private GameMapPlayersView _playersView;

        [Inject]
        public void Init(UIFactory uiFactory, ClientsGameData gameData, Converter viewConfig)
        {
            _generator = new(gameData, transform.position, _offset);
            _cellsView = new(_generator, _cellsParent, uiFactory, gameData);
            _playersView = new(_generator, _playersParent, uiFactory, gameData, viewConfig);

            _playersView.OnAnimationCreated += (container) => { OnAnimationCreated?.Invoke(container); };
        }

        [Button("Adaptate")]
        public void AdaptateScale()
        {
            var t = transform as RectTransform;
            float coefficient;
            if(t.rect.width <= t.rect.height)
            {
                coefficient = t.rect.width / 1080f;
            }
            else
            {
                coefficient = t.rect.height / 1080f;
            }
            transform.localScale = new Vector3(coefficient, coefficient, 1f);
        }
    }
}
