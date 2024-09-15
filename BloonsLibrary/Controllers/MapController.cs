using BloonsProject.Models.Extensions;
using SplashKitSDK;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace BloonsProject
{
    public class MapController
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton.

        public bool CanPlaceTowerOnMap(Point2D location, Map map) // Determines whether a tower can be placed on the map.
        {
            for (var i = 0; i < map.Checkpoints.Count - 1; i++)
            {
                var line = SplashKit.LineFrom(SplashKitExtensions.PointFromVector(map.Checkpoints[i]), SplashKitExtensions.PointFromVector(map.Checkpoints[i + 1])); // Draws a line between one checkpoint and the next. Iterates through all checkpoints
                if (SplashKit.PointLineDistance(location, line) < map.TrackWidthRadius || // If each four points of the tower's rectangular hitbox is within the radius of the line, return false
                    SplashKit.PointLineDistance(new Point2D { X = location.X + Tower.Length, Y = location.Y + Tower.Length }, line) < map.TrackWidthRadius ||
                    SplashKit.PointLineDistance(new Point2D { X = location.X, Y = location.Y + Tower.Length }, line) < map.TrackWidthRadius ||
                    SplashKit.PointLineDistance(new Point2D { X = location.X + Tower.Length, Y = location.Y }, line) < map.TrackWidthRadius ||
                    location.X > map.Length - Tower.Length || // If the tower is outside of the whole map.
                    location.X < 0 ||
                    location.Y < 0 ||
                    location.Y > map.Height - Tower.Length
                )
                    return false; // Then tower can't be placed
            }

            foreach (var tower in _gameState.Towers) //If the tower to be placed is within another tower, return false.
                if (SplashKit.PointInRectangle(location,
                    new Rectangle
                    {
                        X = tower.Position.X - Tower.Length,
                        Y = tower.Position.Y - Tower.Length,
                        Height = 2 * Tower.Length,
                        Width = 2 * Tower.Length
                    }))
                    return false;
            return true; // Otherwise if the tower is not in the way of anything, return true.
        }

        public void ClickOnMap(Point2D location, TowerGuiOptions towerOptions, TowerTargetingGuiOptions targetOptions, MouseClickType clickType)
        {
            if (towerOptions.UpgradeOptionsInGui.Keys.Any(position => SplashKit.PointInRectangle(location, new Rectangle() // If the user clicks on the tower options (upgrade, sell) then don't unselect the tower
            {
                Height = towerOptions.Height,
                Width = towerOptions.Width,
                X = position.X,
                Y = position.Y
            })))
                return;
            if (targetOptions.TargetingButtonLocations.Keys.Any(position => SplashKit.PointInRectangle(location, new Rectangle() // If the user changes the tower's targeting, then don't unselect the tower.
            {
                Height = targetOptions.Height,
                Width = targetOptions.Width,
                X = position.X,
                Y = position.Y
            })))
                return;

            foreach (var tower in _gameState.Towers) // If the tower left clicks another tower, select that tower.
            {
                var towerRectangle = new Rectangle
                {
                    X = tower.Position.X,
                    Y = tower.Position.Y,
                    Height = Tower.Length,
                    Width = Tower.Length
                };
                if (clickType == MouseClickType.left)
                {
                    tower.Selected = SplashKit.PointInRectangle(location, towerRectangle);
                }
                else
                {
                    tower.DebugModeSelected = SplashKit.PointInRectangle(location, towerRectangle); // If the tower is right clicked, turn debug mode on (display cooldown)
                }
            }
        }
    }
}