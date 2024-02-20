using System;
using System.Collections.Generic;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsPlayer
    {
        public event Action<ClientsPlayer> OnAnyValueChanged;

        public string Name { get; private set; }
        public PlayerID ID { get; private set; }
        public int Cash { get; private set; } = 15000;
        public PlayerState State { get; private set; } = PlayerState.Default;
        public int CurrentCell { get; private set; } = 0;
        public IReadOnlyDictionary<EffectType, int> Effects => _effects;

        public Dictionary<EffectType, int> _effects = new();

        public ClientsPlayer(string name, PlayerID iD)
        {
            Name = name;
            ID = iD;
        }

        public void Update(PlayerInfoPackage info)
        {
            Cash = info.Cash;
            State = info.State;
            CurrentCell = info.PositionCellIndex;
            _effects.Clear();
            for (int i = 0; i < info.EffectTypes.Count; i++)
            {
                _effects.Add(info.EffectTypes[i], info.EffectCounters[i]);
            }
            OnAnyValueChanged?.Invoke(this);
        }
    }
}
