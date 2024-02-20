using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class DecrementEffectCounter : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID TargetPlayer { get; private set; }
        public Type EffectType { get; private set; }

        public DecrementEffectCounter(PlayerID targetPlayer, Type effectType)
        {
            TargetPlayer = targetPlayer;
            EffectType = effectType;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(TargetPlayer);
            if (player.Effects.TryGetValue(EffectType, out var effect))
            {
                if (effect is ITurnBasedEffect turnEffect)
                {
                    turnEffect.Counter--;
                    if (turnEffect.Counter == 0)
                    {
                        player.Effects.Remove(EffectType);
                    }
                }
                else if (effect is IUseBasedEffect useEffect)
                {
                    useEffect.Counter--;
                    if (useEffect.Counter == 0)
                    {
                        player.Effects.Remove(EffectType);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log($"RemoveEffectCommand tried to remove effect({EffectType}) " +
                    $"that {player.Name} has not");
            }
        }
    }

    public class DecrementEffectCounter<T> : ICommand where T : IEffect
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID TargetPlayer { get; private set; }

        public DecrementEffectCounter(PlayerID targetPlayer)
        {
            TargetPlayer = targetPlayer;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(TargetPlayer);
            if (player.Effects.TryGetValue(typeof(T), out var effect))
            {
                if (effect is ITurnBasedEffect turnEffect)
                {
                    turnEffect.Counter--;
                    if (turnEffect.Counter == 0)
                    {
                        player.Effects.Remove(typeof(T));
                    }
                }
                else if (effect is IUseBasedEffect useEffect)
                {
                    useEffect.Counter--;
                    if (useEffect.Counter == 0)
                    {
                        player.Effects.Remove(typeof(T));
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log($"RemoveEffectCommand tried to remove effect({typeof(T).Name}) " +
                    $"that {player.Name} has not");
            }
        }
    }
}
