using BloonsProject.Models.Extensions;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloonLibrary;
using BloonsProject;
using Microsoft.AspNetCore.SignalR;

namespace BloonsProject
{
    public class BloonController
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton.
        private GameClient _gameClient;
        private readonly object _broadcastLock = new object();

        public BloonController(GameClient gameClient)
        {
            _gameClient = gameClient;
        }
        
        public int ticksSinceLastSentBloon { get; set; } // Keeps track of the last time a bloon was sent.

        public async Task AddBloon(Bloon bloon) // Adds bloon to the list of bloons in the singleton.
        {
            //_gameState.Bloons.Add(bloon);
            await _gameClient.PlaceBloonAsync(new PlaceBloonRequest(bloon.Health, bloon.Name, bloon.Color, bloon.VelocityX, bloon.VelocityY));
            _gameState.BloonsSpawned[bloon.Color] += 1; // Increments the number of bloons spawned for the specific colour added.
            ticksSinceLastSentBloon = 0; // When a bloon is added, reset the last time a bloon was added to 0.
        }

        public int BloonsOnScreen(Window window) // Returns the number of bloons on the screen.
        {
            var amountOfBloons = _gameState.Bloons.Values.Count(bloon =>
                bloon.Position.X >= 0 && bloon.Position.X < window.Width && 
                bloon.Position.Y >= 0 && bloon.Position.Y < window.Height);
            return amountOfBloons;
        }


        public void CheckBloonHealth() // Checks the health of all the bloons on the screen.
        {
            // Create a list to hold bloons to remove and add
            var bloonsToRemove = new List<string>(); // To track bloon keys to remove
            var newBloonsToAdd = new List<Bloon>(); // To track new bloons to add

            foreach (var bloon in _gameState.Bloons.Values) // Iterate over values
            {
                if (bloon.Color.ToString() == Color.Green.ToString() && bloon.Health == 2)
                {
                    // Replace with a new BlueBloon
                    newBloonsToAdd.Add(new BlueBloon
                    {
                        Position = bloon.Position,
                        Checkpoint = bloon.Checkpoint,
                        DistanceTravelled = bloon.DistanceTravelled
                    });
                    bloonsToRemove.Add(bloon.Name); // Add key for removal
                }

                if (bloon.Color.ToString() == Color.Blue.ToString() && bloon.Health == 1)
                {
                    // Replace with a new RedBloon
                    newBloonsToAdd.Add(new RedBloon
                    {
                        Position = bloon.Position,
                        Checkpoint = bloon.Checkpoint,
                        DistanceTravelled = bloon.DistanceTravelled
                    });
                    bloonsToRemove.Add(bloon.Name); // Add key for removal
                }

                // If the bloon has no health, remove it
                if (bloon.Health <= 0)
                {
                    bloonsToRemove.Add(bloon.Name); // Add key for removal
                }
            }

            // Remove bloons from the dictionary
            foreach (var bloonName in bloonsToRemove)
            {
                _gameState.Bloons.TryRemove(bloonName, out _);
            }

            // Add new bloons to the dictionary
            foreach (var newBloon in newBloonsToAdd)
            {
                _gameState.Bloons.TryAdd(newBloon.Name, newBloon);
            }
        }


        public async Task MoveBloon(Bloon bloon, Map map) // Moves the bloon.
        {
            var initialPosition = bloon.Position;
            if (bloon.Position.X <= map.Checkpoints[bloon.Checkpoint].X) // Move to the direction of the checkpoint.
            {
                bloon.MoveBloonInDirection(Direction.Right);
                bloon.DistanceTravelled += bloon.VelocityX; // Increment the distance travelled of the bloon which each movement
            }

            if (bloon.Position.Y <= map.Checkpoints[bloon.Checkpoint].Y)
            {
                bloon.MoveBloonInDirection(Direction.Down);
                bloon.DistanceTravelled += bloon.VelocityY;
            }

            if (bloon.Position.X >= map.Checkpoints[bloon.Checkpoint].X)
            {
                bloon.MoveBloonInDirection(Direction.Left);
                bloon.DistanceTravelled += bloon.VelocityX;
            }

            if (bloon.Position.Y >= map.Checkpoints[bloon.Checkpoint].Y)
            {
                bloon.MoveBloonInDirection(Direction.Up);
                bloon.DistanceTravelled += bloon.VelocityY;
            }

            if (SplashKit.PointInRectangle(bloon.Position,
                    new Rectangle // If the bloon reaches a checkpoint, increment it's checkpoint property.
                    {
                        X = map.Checkpoints[bloon.Checkpoint].X - 4, // Within a distance of 4 of the checkpoint.
                        Y = map.Checkpoints[bloon.Checkpoint].Y - 4,
                        Height = 4,
                        Width = 4
                    }))
                bloon.Checkpoint++;
            var bloonStateRequest = new BloonStateRequest(bloon.Name, bloon.Health, NetworkPoint2D.Serialize(bloon.Position), bloon.Checkpoint, bloon.DistanceTravelled);
            await _gameClient.BroadcastBloonStatesAsync(bloonStateRequest);
        }

        public void ProcessBloons(Player player, Map map)
        {
            ticksSinceLastSentBloon++;
            var sendBloonSpeed = 30 - player.Round; // Speed at which bloons are sent increase with rounds.
            if (player.Round >= 20) sendBloonSpeed = 1;
            if (ticksSinceLastSentBloon <= sendBloonSpeed) return; //If a bloon has recently spawned, return
            var bloonsToAdd = new List<Bloon> { new RedBloon(), new BlueBloon(), new GreenBloon() }; // Creates a list of each bloons and randomly selects from it
            var randomBloonSelection = new Random().Next(bloonsToAdd.Count);
            var bloon = bloonsToAdd[randomBloonSelection];
            if (_gameState.BloonsSpawned[bloon.Color] >= _gameState.BloonsToBeSpawned[bloon.Color]) return; // If the chosen bloon has already had its number of spawns for that round, return.
            AddBloon(bloon); // Otherwise add bloon
            bloon.Position = SplashKitExtensions.PointFromVector(map.Checkpoints[0]); // Convert bloons position from a serializable vector to Point2D.
        }
    }
}