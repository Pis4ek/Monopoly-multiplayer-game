using System.Collections.Generic;

namespace Playmode.PlayData
{
    [System.Serializable]
    public struct PlayerInfoPackage
    {
        public PlayerID ID;
        public PlayerState State;
        public int Cash;
        public int PositionCellIndex;
        public List<EffectType> EffectTypes;
        public List<int> EffectCounters;


        public PlayerInfoPackage(IPlayer player)
        {
            ID = player.ID;
            State = player.State;
            Cash = player.Cash;
            PositionCellIndex = player.CurrentCell.Index;
            EffectTypes = new();
            EffectCounters = new();

            if(player.Effects.Count > 0)
            {
                foreach(var effect in player.Effects.Values)
                {
                    EffectCounters.Add(effect.Counter);
                    EffectTypes.Add(effect.Type);
                }
            }
        }
    }
}
