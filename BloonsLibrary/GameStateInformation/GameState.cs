using BloonLibrary;
using SplashKitSDK;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Linq;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class GameState // Singleton containing information about the gamestate.
    {
        private static GameState _state;
        public ConcurrentDictionary<string, Bloon> Bloons { get; private set; }
        public  List<Tower> Towers = new List<Tower>();
        public Dictionary<Color, int> BloonsSpawned = new Dictionary<Color, int>();
        public Dictionary<Color, int> BloonsToBeSpawned = new Dictionary<Color, int>();
        public  Player Player = new Player();
        public  ProjectileManager ProjectileManager = new ProjectileManager();
        public List<string> OtherPlayerStats = new List<string>();
        public Queue<string> TowerEventMessages = new Queue<string>();

        private static readonly object Locker = new object();

        protected GameState()
        {
            Bloons = new ConcurrentDictionary<string, Bloon>();
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

        public void AddGameStats(string message)
        {
            OtherPlayerStats.Add(message);
        }

        public void AddBloon(Bloon bloon)
        {
            Bloons.TryAdd(bloon.Name, bloon);
        }
        

        public void upgradeOrSellTower(Point2D point, string option, int upgradeCount)
        {
            if (Towers.Count == 0) return;
            foreach (var tower in Towers)
            {
                if (tower.Position.Equals(point))
                {
                    
                    switch (option) // Depending on the option, either upgrade or sell tower.
                    {
                        case "Upgrade Range":
                            if (tower.ShotType.RangeUpgradeCount == 3 || tower.ShotType.RangeUpgradeCount == upgradeCount) break; // Can't upgrade more than 3 times
                            tower.Range += 50; // Increase range by 50.
                            tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost; // Add 70% of the price put into the upgrade to the sell price
                            tower.ShotType.RangeUpgradeCount++;
                            break;

                        case "Upgrade Firerate":
                            if (tower.ShotType.FirerateUpgradeCount == 3 || tower.ShotType.FirerateUpgradeCount == upgradeCount) break; // Repeat for firerate
                            tower.ShotType.ShotSpeed -= 10;
                            
                            tower.ShotType.FirerateUpgradeCount++;
                            tower.SellPrice += 0.7 * tower.ShotType.FirerateUpgradeCost;
                            break;

                        case "Sell":
                            Towers.Remove(tower);
                            break;
                    }
                    return;
                }
            }
        }
        
        //public void AddBloon(Bloon bloon)
        //{
        //    Bloons.Add(bloon);
        //}
        
        public List<BloonState> GetAllBloonStates()
        {
            // Use the .Values property to get all values from the ConcurrentDictionary
            return Bloons.Values.Select(bloon => new BloonState(
                bloon.Name,
                bloon.Health,
                NetworkPoint2D.Serialize(bloon.Position),
                bloon.Checkpoint,
                bloon.DistanceTravelled
            )).ToList();
        }
        
        public void UpdateBloonState(BloonState state)
        {
            // Attempt to retrieve the bloon from the ConcurrentDictionary using the name
            if (Bloons.TryGetValue(state.Name, out var bloon))
            {
                // Update the properties of the bloon with the values from the state
                bloon.Position = new Point2D()
                {
                    X = state.Position.X, // Update with the new position from state
                    Y = state.Position.Y
                };
                bloon.Health = state.Health;
                bloon.Checkpoint = state.Checkpoint;
                bloon.DistanceTravelled = state.DistanceTravelled;
            }

        }
    }
}