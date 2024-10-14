using System.Collections.Generic;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class GameState
    {
        private static GameState _state;
        public readonly List<Bloon> Bloons = new List<Bloon>();
        public readonly List<Tower> Towers = new List<Tower>();

        // Now we're using User instead of Player
        public readonly Dictionary<string, User> Users = new Dictionary<string, User>();

        public readonly ProjectileManager ProjectileManager = new ProjectileManager();

        private static readonly object Locker = new object();

        protected GameState() { }

        public static GameState GetGameStateInstance()
        {
            if (_state == null)
            {
                lock (Locker)
                {
                    if (_state == null)
                    {
                        _state = new GameState();
                    }
                }
            }
            return _state;
        }

        // Get a User object by username
        public User GetUser(string username)
        {
            return Users.ContainsKey(username) ? Users[username] : null;
        }

        // Add a new User to the game
        public void AddUser(string username, User user)
        {
            Users[username] = user;
        }
    }
}
