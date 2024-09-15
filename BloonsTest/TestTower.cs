using System.IO;
using BloonsProject;
using Newtonsoft.Json;
using NUnit.Framework;
using SplashKitSDK;

namespace BloonsTests
{
    public class TestShooting
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly TowerController _towerController = new TowerController();
        private readonly TowerTargetingGuiOptions _targetOptions = new TowerTargetingGuiOptions();

        [SetUp]
        public void Setup()
        {
            _gameState.Player.Money = 1000;
        }

        [Test]
        public void TestAddTower()
        {
            // Arrange
            var dartTower = new DartTower();
            _towerController.AddTower(dartTower);

            // Act
            var trueStatement = _gameState.Towers.Contains(dartTower);

            // Assert
            Assert.IsTrue(trueStatement);
        }

        [Test]
        public void TestAddTowerUsesMoney()
        {
            // Arrange
            var dartTower = new DartTower();
            _towerController.AddTower(dartTower);

            // Act
            var actualPlayerMoney = _gameState.Player.Money;
            var expectedPlayerMoney = 880;

            // Assert
            Assert.AreEqual(expectedPlayerMoney, actualPlayerMoney);
        }

        [Test]
        public void TestSetTowerTargeting()
        {
            // Arrange
            var dartTower = new DartTower();
            _targetOptions.SelectedInGui = TowerTargeting.Strong;

            // Act
            _towerController.SetTowerTargeting(dartTower, _targetOptions);

            // Assert
            var actual = dartTower.Targeting;
            Assert.IsInstanceOf<TargetStrong>(actual);
        }

        [Test]
        public void TestHaveSufficientFundsToPlaceTower()
        {
            // Arrange

            // Act
            _gameState.Player.Money = 1000;

            // Assert
            Assert.IsTrue(_towerController.HaveSufficientFundsToPlaceTower(new DartTower()));
        }

        [Test]
        public void TestShootBloonsAddsProjectile()
        {
            // Arrange
            var dartTower = new DartTower();
            var bloonInTowerRadius = new RedBloon();
            var map = MapManager.GetAllMaps()[0];

            // Act
            dartTower.Position = new Point2D() { X = 20, Y = 25 };
            dartTower.ShotType.TimeSinceLastShot = 1000;
            _gameState.Towers.Add(dartTower);
            _gameState.Bloons.Add(bloonInTowerRadius);
            bloonInTowerRadius.Position = new Point2D() { X = 24, Y = 40 };
            _towerController.ShootBloons(map);

            // Assert
            var expected = 1;
            var actual = _gameState.ProjectileManager.ProjectilesOnScreen.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUpgradeTowerRange()
        {
            // Arrange
            var dartTower = new DartTower();
            var towerOptions = new TowerGuiOptions();
            // Act
            _gameState.Towers.Clear();
            _gameState.Towers.Add(dartTower);
            towerOptions.SelectedInGui = "Upgrade Range";
            dartTower.Selected = true;
            _towerController.UpgradeOrSellSelectedTower(_towerController, towerOptions);
            // Assert
            var expected = 150;
            var actual = dartTower.Range;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUpgradeFirerate()
        {
            // Arrange
            var dartTower = new DartTower();
            var towerOptions = new TowerGuiOptions();
            // Act
            _gameState.Towers.Clear();
            _gameState.Towers.Add(dartTower);
            towerOptions.SelectedInGui = "Upgrade Firerate";
            dartTower.Selected = true;
            _towerController.UpgradeOrSellSelectedTower(_towerController, towerOptions);
            // Assert
            var expected = 90.0;
            var actual = dartTower.ShotType.ShotSpeed;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSellTower()
        {
            // Arrange
            var dartTower = new DartTower();
            var towerOptions = new TowerGuiOptions();
            // Act
            _gameState.Towers.Clear();
            _gameState.Towers.Add(dartTower);
            towerOptions.SelectedInGui = "Sell";
            dartTower.Selected = true;
            _towerController.UpgradeOrSellSelectedTower(_towerController, towerOptions);
            // Assert
            var actual = _gameState.Towers.Count;
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }
    }
}