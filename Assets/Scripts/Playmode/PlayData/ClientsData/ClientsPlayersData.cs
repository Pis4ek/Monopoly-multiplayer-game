using System.Collections;
using System.Collections.Generic;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsPlayersData
    {
        public int Count => _players.Count;

        private Dictionary<PlayerID, ClientsPlayer> _players = new();

        public ClientsPlayersData(int playersCount)
        {
            if (playersCount < 2 || playersCount > 5)
            {
                var message = $"PlayerHolder has got not valid value " +
                    $"of players count ({playersCount})";
                throw new System.Exception(message);
            }

            for (int i = 0; i < playersCount; i++)
            {
                PlayerID id = (PlayerID)i;
                _players.Add(id, new ClientsPlayer($"Player{i + 1}", id));
            }
        }

        public ClientsPlayer this[PlayerID id] => _players[id];

        public ClientsPlayer GetPlayerByID(PlayerID id) => _players[id];

        public bool TryGetPlayerByID(PlayerID id, out ClientsPlayer player) => _players.TryGetValue(id, out player);

        #region Other
        public void Update(PlayerInfoPackage info)
        {
            _players[info.ID].Update(info);
        }

        public void Update(ICollection<PlayerInfoPackage> infos)
        {
            foreach (var info in infos)
            {
                _players[info.ID].Update(info);
            }
        }

        public IEnumerator GetEnumerator() => _players.Values.GetEnumerator();
        #endregion
    }
}
