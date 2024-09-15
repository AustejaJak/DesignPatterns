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

        public void LoseLivesAndRemoveBloons(Map map) // Checks for any bloons at the final checkpoint, removes them and removes lives depending on the bloon's health.
        {
            if (_gameState.Bloons.Count <= 0) return;
            var bloonsToBeDeleted = _gameState.Bloons.Where(b => b.Checkpoint == map.Checkpoints.Count).ToList();
            foreach (var bloon in bloonsToBeDeleted)
            {
                _gameState.Player.Lives -= bloon.Health;
                _gameState.Bloons.Remove(bloon);
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
        }
    }
}