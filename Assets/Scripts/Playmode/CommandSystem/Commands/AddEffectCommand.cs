using Playmode.PlayData;
using System;

namespace Playmode.CommandSystem
{
    public class AddEffectCommand<T> : ICommand where T : class, IEffect, new()
    {
        public event Action<ICommand> OnNeedExecuteOtherCommand;

        public PlayerID PlayerID { get; private set; }
        public int TurnsOrUsesCount { get; private set; }

        public AddEffectCommand(PlayerID targetPlayer, int turnsOrUsesCount)
        {
            PlayerID = targetPlayer;
            TurnsOrUsesCount = turnsOrUsesCount;
        }

        public void Execute(GameData gameData)
        {
            var player = gameData.GetPlayerByID(PlayerID);
            T effect = new T();

            if (player.Effects.ContainsKey(typeof(T)))
            {
                player.Effects.Remove(typeof(T));
            }
            effect.Counter = TurnsOrUsesCount;
            player.Effects.Add(typeof(T), effect);
        }        
    }

    public class AddIncreaceIncomeEffectCommand : AddEffectCommand<IncreaceIncomeEffect>
    {
        public float Scaler { get; private set; }

        public AddIncreaceIncomeEffectCommand(PlayerID targetPlayer, int turnsOrUsesCount, float scaler)
            : base(targetPlayer, turnsOrUsesCount)
        {
            Scaler = scaler;
        }

        public new void Execute(GameData gameData)
        {
            base.Execute(gameData);
            var player = gameData.GetPlayerByID(PlayerID);
            var effect = player.Effects[typeof(IncreaceIncomeEffect)] as IncreaceIncomeEffect;
            effect.Scaler = Scaler;

            if (player.Effects.ContainsKey(typeof(DecreaceIncomeEffect)))
            {
                player.Effects.Remove(typeof(DecreaceIncomeEffect));
            }
        }
    }

    public class AddDecreaceIncomeEffectCommand : AddEffectCommand<DecreaceIncomeEffect>
    {
        public float Scaler { get; private set; }

        public AddDecreaceIncomeEffectCommand(PlayerID targetPlayer, int turnsOrUsesCount, float scaler)
            : base(targetPlayer, turnsOrUsesCount)
        {
            Scaler = scaler;
        }

        public new void Execute(GameData gameData)
        {
            base.Execute(gameData);
            var player = gameData.GetPlayerByID(PlayerID);
            var effect = player.Effects[typeof(DecreaceIncomeEffect)] as DecreaceIncomeEffect;
            effect.Scaler = Scaler;

            if (player.Effects.ContainsKey(typeof(IncreaceIncomeEffect)))
            {
                player.Effects.Remove(typeof(IncreaceIncomeEffect));
            }
        }
    }

    public class AddIgnoreRentEffectCommand : AddEffectCommand<IgnoreRentEffect>
    {
        public float Scaler { get; private set; }

        public AddIgnoreRentEffectCommand(PlayerID targetPlayer, int turnsOrUsesCount, float scaler)
            : base(targetPlayer, turnsOrUsesCount)
        {
            Scaler = scaler;
        }

        public new void Execute(GameData gameData)
        {
            base.Execute(gameData);
            var player = gameData.GetPlayerByID(PlayerID);
            var effect = player.Effects[typeof(IgnoreRentEffect)] as IgnoreRentEffect;
            effect.Scaler = Scaler;
        }
    }
}
