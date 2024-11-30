using BloonLibrary;
using SplashKitSDK;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Linq;
using Color = SplashKitSDK.Color;
using BloonLibrary.Controllers.Bridge;
using BloonLibrary.Iterator;

namespace BloonsProject
{
    public class GameState // Singleton containing information about the gamestate.
    {
        private static GameState _state;
        public ConcurrentDictionary<string, Bloon> Bloons { get; private set; }
        public ListAggregate<Tower> Towers = new ListAggregate<Tower>();
        public Dictionary<Color, int> BloonsSpawned = new Dictionary<Color, int>();
        public Dictionary<Color, int> BloonsToBeSpawned = new Dictionary<Color, int>();
        public  Player Player = new Player();
        public  ProjectileManager ProjectileManager = new ProjectileManager();
        public List<string> OtherPlayerStats = new List<string>();
        //public Queue<string> TowerEventMessages = new Queue<string>();
        public QueueAggregate<string> TowerEventMessages = new QueueAggregate<string>();
        //public List<TowerContols> TowerControlls = new List<TowerContols>();
        public ListAggregate<TowerContols> TowerControlls = new ListAggregate<TowerContols>();
        public string InvalidTowerEventMessage;

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
            Towers.AddItem(tower);
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
            var TowerIterator = Towers.CreateIterator();
            while (TowerIterator.MoveNext())
            {
                if (TowerIterator.Current.Position.Equals(point))
                {

                    switch (option) // Depending on the option, either upgrade or sell tower.
                    {
                        case "Upgrade Range":
                            if (TowerIterator.Current.ShotType.RangeUpgradeCount == 3 || TowerIterator.Current.ShotType.RangeUpgradeCount == upgradeCount) break; // Can't upgrade more than 3 times
                            TowerIterator.Current.Range += 50; // Increase range by 50.
                            TowerIterator.Current.SellPrice += 0.7 * TowerIterator.Current.ShotType.RangeUpgradeCost; // Add 70% of the price put into the upgrade to the sell price
                            TowerIterator.Current.ShotType.RangeUpgradeCount++;
                            break;

                        case "Upgrade Firerate":
                            if (TowerIterator.Current.ShotType.FirerateUpgradeCount == 3 || TowerIterator.Current.ShotType.FirerateUpgradeCount == upgradeCount) break; // Repeat for firerate
                            TowerIterator.Current.ShotType.ShotSpeed -= 10;
                            TowerIterator.Current.UpdateDecorator();
                            TowerIterator.Current.ShotType.FirerateUpgradeCount++;
                            TowerIterator.Current.SellPrice += 0.7 * TowerIterator.Current.ShotType.FirerateUpgradeCost;
                            break;

                        case "Sell":
                            
                            var ControllsIterator = TowerControlls.CreateIterator();
                            while (ControllsIterator.MoveNext())
                            {
                                if (ControllsIterator.Current.GetTower() == null || ControllsIterator.Current.GetTower() == TowerIterator.Current)
                                {
                                    TowerControlls.RemoveItem(ControllsIterator.Current);
                                    break;
                                }
                            }
                            Towers.RemoveItem(TowerIterator.Current);
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