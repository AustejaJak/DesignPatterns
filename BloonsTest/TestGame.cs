using System.Linq;
using BloonsProject;
using NUnit.Framework;
using SplashKitSDK;

namespace BloonsTests
{
    public class TestGame
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly BloonController _bloonController = new BloonController();
        private readonly GameController _gameController = new GameController();
        private readonly Map _map = MapManager.GetAllMaps()[0];
        private readonly RedBloon _redBloon = new RedBloon();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBloonsGetRemovedAtFinalCheckpoint()
        {
            // Arrange
            var bloonAtFinalCheckpoint = _redBloon;
            var map = _map;

            // Act
            _gameState.Bloons.Clear();
            bloonAtFinalCheckpoint.Checkpoint = map.Checkpoints.Count;
            _gameState.Bloons.Add(bloonAtFinalCheckpoint);
            _gameController.LoseLivesAndRemoveBloons(map);

            // Assert
            var actual = _gameState.Bloons.Count;
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPlayerLosesLives()
        {
            // Arrange
            var bloonAtFinalCheckpoint = _redBloon;
            var map = _map;

            // Act
            _gameState.Player.Lives = 30;
            bloonAtFinalCheckpoint.Checkpoint = map.Checkpoints.Count;
            _gameState.Bloons.Add(bloonAtFinalCheckpoint);
            _gameController.LoseLivesAndRemoveBloons(map);

            // Assert
            var actual = _gameState.Player.Lives;
            var expected = 29;
            Assert.AreEqual(expected, actual);
        }
    }
}