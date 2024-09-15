using System.Collections.Generic;
using BloonsProject;
using NUnit.Framework;


namespace BloonsTests
{
    public class Tests
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly Tower _tower = new DartTower();
        private readonly RedBloon _redBloon = new RedBloon();
        private readonly BlueBloon _blueBloon = new BlueBloon();
        private readonly GreenBloon _greenBloon = new GreenBloon();
        
        [SetUp]
        public void Setup()
        {
            _redBloon.DistanceTravelled = 50;
            _blueBloon.DistanceTravelled = 1000;
            _greenBloon.DistanceTravelled = 2000;

            _gameState.Bloons.Add(_redBloon);
            _gameState.Bloons.Add(_blueBloon);
            _gameState.Bloons.Add(_greenBloon);
        }

        [Test]
        public void TestTargetLast()
        {
            // Arrange
            var listOfBloons = _gameState.Bloons;
            _tower.Targeting = new TargetLast();
            
            // Act
            var actual = _tower.Targeting.BloonToTarget(listOfBloons);
            var expected = _redBloon;
            
            // Assert
            Assert.AreEqual(expected,actual);
        }
        
        [Test]
        public void TestTargetFirst()
        {
            // Arrange
            var listOfBloons = _gameState.Bloons;
            _tower.Targeting = new TargetFirst();
            
            // Act
            var actual = _tower.Targeting.BloonToTarget(listOfBloons);
            var expected = _greenBloon;
            
            // Assert
            Assert.AreEqual(expected,actual);
        }
        [Test]
        public void TestTargetStrong()
        {
            // Arrange
            var listOfBloons = _gameState.Bloons;
            _tower.Targeting = new TargetStrong();
            
            // Act
            var actual = _tower.Targeting.BloonToTarget(listOfBloons);
            var expected = _greenBloon;
            
            // Assert
            Assert.AreEqual(expected,actual);
        }
        [Test]
        public void TestTargetWeak()
        {
            // Arrange
            var listOfBloons = _gameState.Bloons;
            _tower.Targeting = new TargetWeak();
            
            // Act
            var actual = _tower.Targeting.BloonToTarget(listOfBloons);
            var expected = _redBloon;
            
            // Assert
            Assert.AreEqual(expected,actual);
        }
        [Test]
        public void TestReturnBloonWhenBloonListIsOfSizeOne()
        {
            // Arrange
            var listOfBloons = new List<Bloon>();
            listOfBloons.Add(_blueBloon);
            _tower.Targeting = new TargetWeak();
            
            // Act
            var actual = _tower.Targeting.BloonToTarget(listOfBloons);
            var expected = _blueBloon;
            
            // Assert
            Assert.AreEqual(expected,actual);
        }
        

    }
}