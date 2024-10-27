using BloonsProject;
using NUnit.Framework;
using SplashKitSDK;

namespace BloonsTests
{
    public class TestMap
    {
        private readonly MapController _mapController = new MapController();
        private readonly Map _map = MapManager.GetAllMaps()[0];

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetMapByName()
        {
            // Arrange
            var debugMap = _map;
            var debugMapFromManagerByName = MapManager.GetMapByName("Debug Map");

            // Act

            // Assert
            var actual = debugMapFromManagerByName;
            var expected = debugMap;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCanPlaceTowerOnMap()
        {
            // Arrange
            var pointOnMap = new Point2D() { X = 20, Y = 20 };
            var map = _map;

            // Act

            // Assert
            var actual = _mapController.CanPlaceTowerOnMap(pointOnMap, map);
            Assert.IsTrue(actual);
        }

        [Test]
        public void TestCannotPlaceTowerOnMap()
        {
            // Arrange
            var map = _map;
            var pointOnBloonPath = new Point2D() { X = map.Checkpoints[1].X, Y = map.Checkpoints[1].Y };

            // Act

            // Assert
            var actual = _mapController.CanPlaceTowerOnMap(pointOnBloonPath, map);
            Assert.IsFalse(actual);
        }

        [Test]
        public void TestCannotPlaceTowerOffMap()
        {
            // Arrange
            var map = _map;
            var pointOnBloonPath = new Point2D() { X = -200, Y = -200 };

            // Act

            // Assert
            var actual = _mapController.CanPlaceTowerOnMap(pointOnBloonPath, map);
            Assert.IsFalse(actual);
        }
    }
}