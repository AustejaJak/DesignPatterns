using System.Collections.Generic;
using System.Linq;
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
        
        public void AddTower(Tower tower)
        {
            Towers.Add(tower);
        }
        
        public void AddBloon(Bloon bloon)
        {
            Bloons.Add(bloon);
        }
        
        public Bloon GetBloon(string bloonType)
        {
            return Bloons.FirstOrDefault(b => b.Name == bloonType);
        }
        
        public List<Bloon> GetAllBloons()
        {
            return Bloons.ToList();
        }
        
    }
}