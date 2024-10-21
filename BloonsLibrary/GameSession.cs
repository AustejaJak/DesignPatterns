using System.Collections.Generic;

namespace BloonsProject
{
    public class GameSession
    {
        private static GameSession _instance;
        public GameState GameState { get; private set; }
        public List<string> ConnectedPlayers { get; private set; }
        public Dictionary<string, (bool isReady, string selectedMap)> PlayerStatuses { get; private set; } // Track readiness and map selection

        private GameSession()
        { 
            GameState = GameState.GetGameStateInstance();
            ConnectedPlayers = new List<string>();
            PlayerStatuses = new Dictionary<string, (bool isReady, string selectedMap)>();
        }

        public static GameSession GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameSession();
            }
            return _instance;
        }

        public void AddPlayer(string username)
        {
            if (!ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Add(username);
                PlayerStatuses[username] = (false, string.Empty); // Initially not ready, no map selected
            }
        }

        public void RemovePlayer(string username)
        {
            if (ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Remove(username);
                PlayerStatuses.Remove(username);
            }
        }

        // Set a player as ready and their map choice
        public void SetPlayerReady(string username, string map)
        {
            if (PlayerStatuses.ContainsKey(username))
            {
                PlayerStatuses[username] = (true, map);
            }
        }

        // Check if all players are ready and have chosen the same map
        public bool AllPlayersReady()
        {
            //if (ConnectedPlayers.Count < 2) return false; // Ensure two players are connected

            var firstPlayerMap = PlayerStatuses[ConnectedPlayers[0]].selectedMap;
            foreach (var player in ConnectedPlayers)
            {
                var (isReady, selectedMap) = PlayerStatuses[player];
                if (!isReady || selectedMap != firstPlayerMap)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetPlayersList()
        {
            return string.Join(", ", ConnectedPlayers);
        }
    }
}