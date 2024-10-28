using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using BloonLibrary;
using System.Linq;
using SplashKitSDK;
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
        
        public void AddBloon(Bloon bloon)
        {
            Bloons.TryAdd(bloon.Name, bloon);
        }
        
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