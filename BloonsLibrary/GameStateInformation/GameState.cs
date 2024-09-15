using System.Collections.Generic;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class GameState // Singleton containing information about the gamestate.
    {
        private static GameState _state;
        public readonly List<Bloon> Bloons = new List<Bloon>();
        public readonly List<Tower> Towers = new List<Tower>();
        public Dictionary<Color, int> BloonsSpawned = new Dictionary<Color, int>();
        public Dictionary<Color, int> BloonsToBeSpawned = new Dictionary<Color, int>();
        public readonly Player Player = new Player();
        public readonly ProjectileManager ProjectileManager = new ProjectileManager();

        private static readonly object Locker = new object();

        protected GameState()
        {
        }

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
    }
}