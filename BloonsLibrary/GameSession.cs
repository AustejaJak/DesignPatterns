using System.Collections.Generic;

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

        public void AddPlayer(string username)
        {
            if (!ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Add(username);
            }
        }

        public void RemovePlayer(string username)
        {
            if (ConnectedPlayers.Contains(username))
            {
                ConnectedPlayers.Remove(username); 
            }
        }
    }
}
