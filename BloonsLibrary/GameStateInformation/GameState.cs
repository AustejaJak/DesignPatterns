using System.Collections.Generic;
using System.Text.Json;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class GameState // Singleton containing information about the gamestate.
    {
        private static GameState _state;
        public List<Bloon> Bloons = new List<Bloon>();
        public  List<Tower> Towers = new List<Tower>();
        public Dictionary<Color, int> BloonsSpawned = new Dictionary<Color, int>();
        public Dictionary<Color, int> BloonsToBeSpawned = new Dictionary<Color, int>();
        public  Player Player = new Player();
        public  ProjectileManager ProjectileManager = new ProjectileManager();

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

        public List<Tower> GetTowers()
        {
            return new List<Tower>(Towers);
        }

        public void AddTower(Tower tower)
        {
            Towers.Add(tower);
        }

        // Retrieve the list of bloons
        public List<Bloon> GetBloons()
        {
            return new List<Bloon>(Bloons);
        }
    }
}