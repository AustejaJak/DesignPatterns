using System.Collections.Generic;
using BloonsProject;
using NUnit.Framework;
using SplashKitSDK;

namespace BloonsTests
{
    public class TestBloonProperties
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly BloonController _bloonController = new BloonController();
        private readonly RedBloon _redBloon = new RedBloon();
        private readonly BlueBloon _blueBloon = new BlueBloon();
        private readonly GreenBloon _greenBloon = new GreenBloon();

        [SetUp]
        public void Setup()
        {
            _redBloon.DistanceTravelled = 50;
            _blueBloon.DistanceTravelled = 1000;
            _greenBloon.DistanceTravelled = 2000;
        }

        [Test]
        public void TestCountBloonsOnScreen()
        {
            // Arrange

            // Act
            _gameState.Bloons.Clear();
            _gameState.Bloons.Add(_redBloon);
            _gameState.Bloons.Add(_blueBloon);
            _gameState.Bloons.Add(_greenBloon);

            // Assert
            var actual = _bloonController.BloonsOnScreen(new Window("TestWindow", 600, 1000));
            var expected = 3;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBloonHealthCheck()
        {
            // Arrange

            // Act
            _gameState.Bloons.Clear();
            _gameState.Bloons.Add(_redBloon);
            _gameState.Bloons[0].Health = 0;
            _bloonController.CheckBloonHealth();

            // Assert
            var actual = _bloonController.BloonsOnScreen(new Window("TestWindow", 600, 1000));
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }
    }
}