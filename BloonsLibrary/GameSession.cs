using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloonsProject
{
    public class GameSession
    {

        private static GameSession _instance;
        public GameState GameState { get; private set; }
        public List<string> ConnectedPlayers { get; private set; }

        private GameSession()
        { 
            GameState = GameState.GetGameStateInstance();
            ConnectedPlayers = new List<string>();
        }
        public static GameSession GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameSession();
            }
            return _instance;
        }

        public bool AllPlayersReady(int requiredPlayers)
        {
            return ConnectedPlayers.Count >= requiredPlayers;
        }

        public void AddPlayer(string username)
        {
            if (!ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Add(username);
            }
        }

        public List<string> GetPlayers()
        {
            return ConnectedPlayers;
        }

        public void RemovePlayer(string username)
        {
            if (ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Remove(username); 
            }
        }
        
        public GameState GetCurrentGameState()
        {
            return GameState;
        }
    }
}