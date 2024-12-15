using BloonsProject.Models.Extensions;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using BloonLibrary;
using BloonLibrary.Bloons;
using BloonLibrary.Extensions;
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

        public async Task AddSingleBloon(Bloon bloon) // Adds bloon to the list of bloons in the singleton.
        {
            await _gameClient.PlaceBloonAsync(new PlaceBloonRequest(bloon.Health, bloon.Name, bloon.Color,
                bloon.VelocityX, bloon.VelocityY));
            _gameState.Bloons.TryAdd(bloon.Name, bloon);
            _gameState.BloonsSpawned[bloon.Color] +=
                1; // Increments the number of bloons spawned for the specific colour added.
        }

        public async Task AddBloon(Bloon bloon)
        {
            if (bloon is CompositeBloon compositeBloon) // Check if it's a composite bloon
            {
                await AddCompositeBloon(compositeBloon); // Call method for adding composite bloons
            }
            else
            {
                await AddSingleBloon(bloon); // Call method for adding individual bloons
            }

            ticksSinceLastSentBloon = 0; // Reset the counter for the last bloon added
        }

        private async Task AddCompositeBloon(CompositeBloon compositeBloon)
        {
            foreach (var childBloon in compositeBloon.Children)
            {
                _gameState.Bloons.TryAdd(childBloon.Name, childBloon);
                await AddSingleBloon(childBloon);
            }
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
            var bloonsToRemove = new List<string>(); // List to hold keys of bloons to remove
            var newBloonsToAdd = new List<Bloon>(); // List to hold new bloons to add

            foreach (var b in _gameState.Bloons.Values.ToList()) // Iterate over the bloons
            {
                if (b.Color.Equals(Color.Green) && b.Health == 2) // If a green bloon has 2 health
                {
                    Bloon bloon = b.CloneToType(typeof(BlueBloon));
                    newBloonsToAdd.Add(bloon);
                    bloonsToRemove.Add(b.Name); // Track the bloon's name for removal
                }
                else if (b.Color.Equals(Color.Blue) && b.Health == 1) // If a blue bloon has 1 health
                {
                    //newBloonsToAdd.Add(new RedBloon
                    //{
                    //    Position = b.Position,
                    //    Checkpoint = b.Checkpoint,
                    //    DistanceTravelled = b.DistanceTravelled
                    //});
                    newBloonsToAdd.Add(b.CloneToType(typeof(RedBloon)));
                    //newBloonsToAdd.Add(b.Clone());
                    bloonsToRemove.Add(b.Name); // Track the bloon's name for removal
                }
                else if (b.Health <= 0) // If the bloon has no health
                {
                    bloonsToRemove.Add(b.Name); // Track the bloon's name for removal
                }
            }

            // Remove the tracked bloons
            foreach (var bloonName in bloonsToRemove)
            {
                _gameState.Bloons.TryRemove(bloonName, out _); // Correctly remove the bloon
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
                bloon.DistanceTravelled +=
                    bloon.VelocityX; // Increment the distance travelled of the bloon which each movement
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

            var bloonStateRequest = new BloonStateRequest(bloon.Name, bloon.Health,
                NetworkPoint2D.Serialize(bloon.Position), bloon.Checkpoint, bloon.DistanceTravelled);
            await _gameClient.BroadcastBloonStatesAsync(bloonStateRequest);
        }

        private async Task AddCompositeBloonFormation()
        {
            CompositeBloon formation = new CompositeBloon(3);

            Point2D initialPosition = new Point2D { X = 100, Y = 100 };

            var blackBloon = new BlackBloon();
            blackBloon.Position = new Point2D { X = initialPosition.X, Y = initialPosition.Y };
            formation.Add(blackBloon);
            await AddSingleBloon(blackBloon);

            var yellowBloon = new YellowBloon();
            yellowBloon.Position = new Point2D { X = initialPosition.X + 100, Y = initialPosition.Y };
            formation.Add(yellowBloon);
            await AddSingleBloon(yellowBloon);

            var orangeBloon = new OrangeBloon();
            orangeBloon.Position = new Point2D { X = initialPosition.X + 180, Y = initialPosition.Y };
            formation.Add(orangeBloon);
            await AddSingleBloon(orangeBloon);

            await AddBloon(formation);

            ticksSinceLastSentBloon = 0;
        }

        private async Task AddCompositeBloonRainbowFormation()
        {
            CompositeBloon formation = new CompositeBloon(6);
            
            Point2D initialPosition = new Point2D { X = 100, Y = 100 };
            
            var blackBloon = new BlackBloon();
            blackBloon.Position = new Point2D { X = initialPosition.X, Y = initialPosition.Y };
            blackBloon.VelocityX = 3;
            blackBloon.VelocityY = 3;
            formation.Add(blackBloon);
            await AddSingleBloon(blackBloon);
            
            var yellowBloon = new YellowBloon();
            yellowBloon.Position = new Point2D { X = initialPosition.X + 100, Y = initialPosition.Y };
            yellowBloon.VelocityX = 3;
            yellowBloon.VelocityY = 3;
            formation.Add(yellowBloon);
            await AddSingleBloon(yellowBloon);
            
            var orangeBloon = new OrangeBloon();
            orangeBloon.Position = new Point2D { X = initialPosition.X + 180, Y = initialPosition.Y };
            orangeBloon.VelocityX = 3;
            orangeBloon.VelocityY = 3;
            formation.Add(orangeBloon);
            await AddSingleBloon(orangeBloon);
            
            var redBloon = new RedBloon();
            redBloon.Position = new Point2D { X = initialPosition.X + 260, Y = initialPosition.Y };
            redBloon.VelocityX = 3; 
            redBloon.VelocityY = 3;
            formation.Add(redBloon);
            await AddSingleBloon(redBloon);
            
            var blueBloon = new BlueBloon();
            blueBloon.Position = new Point2D { X = initialPosition.X + 340, Y = initialPosition.Y };
            blueBloon.VelocityX = 3;
            blueBloon.VelocityY = 3;
            formation.Add(blueBloon);
            await AddSingleBloon(blueBloon);
            
            var greenBloon = new GreenBloon();
            greenBloon.Position = new Point2D { X = initialPosition.X + 420, Y = initialPosition.Y };
            greenBloon.VelocityX = 3;
            greenBloon.VelocityY = 3;
            formation.Add(greenBloon);
            await AddSingleBloon(greenBloon);
            
            await AddBloon(formation);

            ticksSinceLastSentBloon = 0;
        }
        
        public async void ProcessBloons(Player player, Map map)
        {
            ticksSinceLastSentBloon++;
            var sendBloonSpeed = 30 - player.Round;
            if (player.Round >= 20) sendBloonSpeed = 1;
            if (ticksSinceLastSentBloon <= sendBloonSpeed) return;

            var bloonsToAdd = new List<Bloon>
            {
                new RedBloon(), new BlueBloon(), new GreenBloon(), new OrangeBloon(), new YellowBloon()
            };
            var randomBloonSelection = new Random().Next(bloonsToAdd.Count);
            var bloon = bloonsToAdd[randomBloonSelection];

            if (_gameState.BloonsSpawned[bloon.Color] >= _gameState.BloonsToBeSpawned[bloon.Color])
            {
                ticksSinceLastSentBloon = 0;
                return;
            }

            if (new Random().Next(3) == 0) // 33% chance for rainbow formation
            {
                await AddCompositeBloonRainbowFormation();
            }
            else if (new Random().Next(2) == 0) // 50% chance for regular composite formation
            {
                await AddCompositeBloonFormation();
            }
            else
            {
                await AddBloon(bloon);
                bloon.Position = SplashKitExtensions.PointFromVector(map.Checkpoints[0]);
            }

            ticksSinceLastSentBloon = 0;
        }
    }
}