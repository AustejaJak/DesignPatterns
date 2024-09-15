using BloonsProject.Models.Extensions;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonsProject
{
    public class BloonController
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton.
        public int ticksSinceLastSentBloon { get; set; } // Keeps track of the last time a bloon was sent.

        public void AddBloon(Bloon bloon) // Adds bloon to the list of bloons in the singleton.
        {
            _gameState.Bloons.Add(bloon);
            _gameState.BloonsSpawned[bloon.Color] += 1; // Increments the number of bloons spawned for the specific colour added.
            ticksSinceLastSentBloon = 0; // When a bloon is added, reset the last time a bloon was added to 0.
        }

        public int BloonsOnScreen(Window window) // Returns the number of bloons on the screen.
        {
            var amountOfBloons = _gameState.Bloons.Count(b =>
                b.Position.X >= 0 && b.Position.X < window.Width && b.Position.Y >= 0 && b.Position.Y < window.Height);
            return amountOfBloons;
        }

        public void CheckBloonHealth() // Checks the health of all the bloons on the screen.
        {
            foreach (var b in _gameState.Bloons.ToList())
            {
                if (b.Color.ToString() == Color.Green.ToString() && b.Health == 2) // If a green bloon has 2 health, then remove it and replace it with a blue bloon.
                { // Pass over the information of the bloon to the newly created one.
                    _gameState.Bloons.Add(new BlueBloon { Position = b.Position, Checkpoint = b.Checkpoint, DistanceTravelled = b.DistanceTravelled });
                    _gameState.Bloons.Remove(b);
                }

                if (b.Color.ToString() == Color.Blue.ToString() && b.Health == 1) // If a blue bloon has 1 health, then remove it and replace it with a red bloon.
                { // Pass over the information of the bloon to the newly created one.
                    _gameState.Bloons.Add(new RedBloon { Position = b.Position, Checkpoint = b.Checkpoint, DistanceTravelled = b.DistanceTravelled });
                    _gameState.Bloons.Remove(b);
                }
                // if the bloon has no health, rmeove it.
                if (b.Health <= 0) _gameState.Bloons.Remove(b);
            }
        }

        public void MoveBloon(Bloon bloon, Map map) // Moves the bloon.
        {
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

            if (SplashKit.PointInRectangle(bloon.Position, new Rectangle // If the bloon reaches a checkpoint, increment it's checkpoint property.
            {
                X = map.Checkpoints[bloon.Checkpoint].X - 4, // Within a distance of 4 of the checkpoint.
                Y = map.Checkpoints[bloon.Checkpoint].Y - 4,
                Height = 4,
                Width = 4
            }))
                bloon.Checkpoint++;
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