using BloonsProject;
using NUnit.Framework;
using SplashKitSDK;

namespace BloonsTests
{
    public class TestProjectiles
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly ProjectileManager _projectileManager = new ProjectileManager();
        private readonly GameController _gameController = new GameController();
        private Projectile _projectile;
        private readonly Map _map = MapManager.GetAllMaps()[0];
        private readonly BlueBloon _blueBloon = new BlueBloon();

        [SetUp]
        public void Setup()
        {
            _projectile = new Projectile(new Point2D() { X = 100, Y = 100 }, new Point2D() { X = 300, Y = 100 },
                new LaserShot());
        }

        [Test]
        public void TestAddProjectile()
        {
            // Arrange
            var projectile = _projectile;

            // Act
            _projectileManager.AddProjectile(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType);

            // Assert
            var actual = _projectileManager.ProjectilesOnScreen.Count;
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementProjectile()
        {
            // Arrange
            var projectile = _projectile;

            // Act
            _projectileManager.ProjectilesOnScreen.Clear();
            _projectileManager.AddProjectile(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType);
            _projectileManager.IncrementAllProjectiles();

            // Assert
            var actual = _projectileManager.ProjectilesOnScreen[0].ProjectileLocation.X > 100;
            Assert.IsTrue(actual);
        }

        [Test]
        public void TestProjectileRemovalAtEndpoint()
        {
            // Arrange
            var projectile = _projectile;

            // Act
            _projectileManager.ProjectilesOnScreen.Clear();
            projectile.ProjectileLocation = projectile.ProjectileDestination;
            _projectileManager.AddProjectile(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType);
            _projectileManager.IncrementAllProjectiles();

            // Assert
            var actual = _projectileManager.ProjectilesOnScreen.Count;
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLingeringProjectileDoesntRemoveAtEndpoint()
        {
            // Arrange
            var projectile = _projectile;

            // Act
            _projectileManager.ProjectilesOnScreen.Clear();
            _projectile.ProjectileShotType = new SniperShot();
            projectile.ProjectileLocation = projectile.ProjectileDestination;
            _projectileManager.AddProjectile(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType);
            _projectileManager.IncrementAllProjectiles();

            // Assert
            var actual = _projectileManager.ProjectilesOnScreen.Count;
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestProjectileDoesNotRemoveWhenInRangeOfBloon()
        {
            // Arrange
            var projectile = _projectile;
            var bloonInRangeOfProjectile = _blueBloon;

            // Act
            _projectileManager.ProjectilesOnScreen.Clear();
            bloonInRangeOfProjectile.Position = projectile.ProjectileLocation;
            _projectileManager.AddProjectile(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType);
            _gameState.Bloons.Add(bloonInRangeOfProjectile);
            _projectileManager.IncrementAllProjectiles();

            // Assert
            var actual = _projectileManager.ProjectilesOnScreen.Count;
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }
    }
}