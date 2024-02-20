using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class RemoveEffectCommand : ICommand
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID TargetPlayer { get; private set; }
        public Type EffectType { get; private set; }

        public RemoveEffectCommand(PlayerID targetPlayer, Type effectType)
        {
            TargetPlayer = targetPlayer;
            EffectType = effectType;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(TargetPlayer);
            if (player.Effects.ContainsKey(EffectType))
            {
                player.Effects.Remove(EffectType);
            }
            else
            {
                UnityEngine.Debug.Log($"RemoveEffectCommand tried to remove effect({EffectType}) " +
                    $"that {player.Name} has not");
            }
        }
    }

    public class RemoveEffectCommand<T> : ICommand where T : IEffect
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID TargetPlayer { get; private set; }

        public RemoveEffectCommand(PlayerID targetPlayer)
        {
            TargetPlayer = targetPlayer;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(TargetPlayer);
            if (player.Effects.ContainsKey(typeof(T)))
            {
                player.Effects.Remove(typeof(T));
            }
            else
            {
                UnityEngine.Debug.Log($"RemoveEffectCommand tried to remove effect({typeof(T).Name}) " +
                    $"that {player.Name} has not");
            }
        }
    }
}
