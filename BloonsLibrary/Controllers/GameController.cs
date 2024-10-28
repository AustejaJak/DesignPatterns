using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;

namespace BloonsProject
{
    public class GameController
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game State singleton

        public bool RequiredBloonsHaveSpawned() // Determines whether the required number of bloons have spawned.
        {
            return _gameState.BloonsSpawned.Count == _gameState.BloonsToBeSpawned.Count && !_gameState.BloonsSpawned.Except(_gameState.BloonsToBeSpawned).Any();
        }

        public bool HaveLivesDepleted() // Determines whether the players lives have depleted
        {
            return _gameState.Player.Lives <= 0;
        }

        public void LoseLivesAndRemoveBloons(Map map) // Checks for any bloons at the final checkpoint, removes them, and removes lives depending on the bloon's health.
        {
            if (_gameState.Bloons.Count <= 0) return;

            // Create a list to hold bloons that need to be removed
            var bloonsToBeDeleted = new List<Bloon>();

            // Identify bloons at the final checkpoint
            foreach (var bloon in _gameState.Bloons.Values)
            {
                if (bloon.Checkpoint == map.Checkpoints.Count)
                {
                    bloonsToBeDeleted.Add(bloon); // Add to the list for later removal
                }
            }

            // Process removal and update lives
            foreach (var bloon in bloonsToBeDeleted)
            {
                _gameState.Player.Lives -= bloon.Health; // Deduct lives based on bloon's health
                _gameState.Bloons.TryRemove(bloon.Name, out _); // Safely remove the bloon from the dictionary
            }
        }
        
        public void SetRound(Map map, int round) // Sets the bloons to be spawned for the round and resets the bloons spawned.
        {
            _gameState.BloonsToBeSpawned = map.BloonsPerRound(round);
            _gameState.BloonsSpawned = new Dictionary<Color, int>
            {
                [Color.Red] = 0,
                [Color.Blue] = 0,
                [Color.Green] = 0
            };
            
            Console.WriteLine($"Round {round}: Red: {_gameState.BloonsToBeSpawned[Color.Red]}, Blue: {_gameState.BloonsToBeSpawned[Color.Blue]}, Green: {_gameState.BloonsToBeSpawned[Color.Green]}");
        
        }
    }
}