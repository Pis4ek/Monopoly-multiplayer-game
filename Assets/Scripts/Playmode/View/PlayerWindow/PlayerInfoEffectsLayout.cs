using Playmode.PlayData.ClientsData;
using UnityEngine;
using Zenject;

namespace Playmode.View
{
    public class PlayerInfoEffectsLayout : MonoBehaviour
    {
        [SerializeField] EffectView _cashEffect;
        [SerializeField] EffectView _ignoreRentEffect;
        [SerializeField] EffectView _skipEffect;
        [SerializeField] EffectView _reverceMoveEffect;

        [Inject]private Converter _converter;
        private ClientsPlayer _player;

        public void SetPlayer(ClientsPlayer player)
        {
            if (_player != null)
            {
                _player.OnAnyValueChanged -= UpdateEffects;
            }
            _player = player;
            _player.OnAnyValueChanged += UpdateEffects;


            _ignoreRentEffect.Color = _converter.PlayerColors[PlayerID.Player2];
            _skipEffect.Color = _converter.PlayerColors[PlayerID.Player5];
            _reverceMoveEffect.Color = _converter.PlayerColors[PlayerID.Player4];
        }

        private void UpdateEffects(ClientsPlayer player)
        {
            _cashEffect.Disactivate();
            _ignoreRentEffect.Disactivate();
            _skipEffect.Disactivate();
            _reverceMoveEffect.Disactivate();

            if (_player.Effects.TryGetValue(EffectType.IncreaceRent, out var IncreaceCounter))
            {
                _cashEffect.Activate();
                _cashEffect.SetCounter(IncreaceCounter);
                _cashEffect.Color = _converter.PlayerColors[PlayerID.Player3];
            }
            else if (_player.Effects.TryGetValue(EffectType.DecreaceRent, out var DecreaceCounter))
            {
                _cashEffect.Activate();
                _cashEffect.SetCounter(DecreaceCounter);
                _cashEffect.Color = _converter.PlayerColors[PlayerID.Player1];
            }

            if (_player.Effects.TryGetValue(EffectType.IgnoreRent, out var IgnoreCounter))
            {
                _ignoreRentEffect.Activate();
                _ignoreRentEffect.SetCounter(IgnoreCounter);
            }
            if (_player.Effects.TryGetValue(EffectType.SkipTurn, out var SkipCounter))
            {
                _skipEffect.Activate();
                _skipEffect.SetCounter(SkipCounter);
            }
            if (_player.Effects.TryGetValue(EffectType.ReverceMove, out var ReverceCounter))
            {
                _reverceMoveEffect.Activate();
                _reverceMoveEffect.SetCounter(ReverceCounter);
            }
        }
    }
}
