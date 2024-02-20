using System;
using System.Collections;
using System.Collections.Generic;

namespace Playmode.PlayData
{
    public class PlayerData : IEnumerable
    {
        public event Action<IPlayer> OnAnyValueInPlayersChanged;

        public int Count => _players.Count;

        private Dictionary<PlayerID, IPlayer> _players = new Dictionary<PlayerID, IPlayer>();
        private MapData _map;

        public PlayerData(MapData map, int playersCount) 
        {
            _map = map;
            if (playersCount < 2 || playersCount > 5)
            {
                var message = $"PlayerHolder has got not valid value " +
                    $"of players count ({playersCount})";
                throw new System.Exception(message);
            }

            for(int i = 0; i < playersCount; i++)
            {
                PlayerID id = (PlayerID)i;
                _players.Add(id, new Player($"Player{i + 1}", id, _map.GetCellByIndex(0)));
            }

            foreach (var player in _players)
            {
                player.Value.OnAnyValueChanged += (p) => {
                    OnAnyValueInPlayersChanged?.Invoke(p);
                };
            }
        }

        public IPlayer this[PlayerID id] => _players[id];

        public IPlayer GetPlayerByID(PlayerID id) => _players[id];

        public bool TryGetPlayerByID(PlayerID id, out IPlayer player) => _players.TryGetValue(id, out player);

        public IEnumerator GetEnumerator() => _players.Values.GetEnumerator();
    }
}