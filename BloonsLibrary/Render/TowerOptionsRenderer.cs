using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;

namespace BloonsProject
{
    public class TowerOptionsRenderer
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton

        public void HighlightTargetingOptionInGui(TowerTargetingGuiOptions targetOptions, Point2D position) // Draw a transparent rectangle over the selected targeting option.
        {
            SplashKit.FillRectangle(new Color() { A = 200, B = 1, G = 1, R = 1 }, position.X, position.Y,
                targetOptions.Width,
                targetOptions.Height);
        }

        public void RenderSelectedTowerOptions(TowerGuiOptions towerOptions, TowerTargetingGuiOptions targetOptions)
        {
            foreach (var tower in _gameState.Towers.ToList().Where(t => t.Selected)) // For every selected tower, render all of its options in the GUI.
            {
                RenderTowerOptions(towerOptions, tower);
                RenderTowerTargetingOptions(targetOptions, tower);
            }
        }

        public void RenderTowerOptions(TowerGuiOptions towerOptions, Tower tower)
        {
            var optionLocationList = new List<Point2D>(towerOptions.UpgradeOptionsInGui.Keys);
            var upgradesList = new List<double>()
                {tower.ShotType.RangeUpgradeCount, tower.ShotType.FirerateUpgradeCount, tower.SellPrice};
            var upgradesPriceList = new List<double>()
                {tower.ShotType.RangeUpgradeCost, tower.ShotType.FirerateUpgradeCost, tower.SellPrice};

            for (var i = 0; i < towerOptions.UpgradeOptionsInGui.Count; i++)
            {
                if (i == towerOptions.UpgradeOptionsInGui.Count - 1) // In the last value of the list (sell), render the sell button and it's corresponding text in a different location separately
                {
                    SplashKit.DrawBitmap(towerOptions.ClickableShapeImages[i], optionLocationList[i].X,
                        optionLocationList[i].Y);
                    SplashKit.DrawText("Sell price: " + upgradesList[i], Color.AntiqueWhite, "BloonFont", 15,
                        optionLocationList[i].X + 8, optionLocationList[i].Y + towerOptions.Height - 6);
                    continue;
                }

                SplashKit.DrawBitmap(towerOptions.ClickableShapeImages[i], optionLocationList[i].X, // Otherwise render both the upgrades at the same y-coordinate.
                    optionLocationList[i].Y);
                SplashKit.DrawText("Upgrades: " + upgradesList[i] + "/3", Color.AntiqueWhite, "BloonFont", 15,
                    optionLocationList[i].X, optionLocationList[i].Y + towerOptions.Height - 5);
                SplashKit.DrawText("Price: $" + upgradesPriceList[i], Color.AntiqueWhite, "BloonFont", 15,
                    optionLocationList[i].X + 20, optionLocationList[i].Y + towerOptions.Height + 18);
            }
        }

        public void RenderTowerTargetingOptions(TowerTargetingGuiOptions targetOptions, Tower tower)
        {
            foreach (var (targetOptionPositionInGui, optionType) in targetOptions.TargetingButtonLocations)
            {
                SplashKit.DrawRectangle(Color.AliceBlue, new Rectangle() // Render the tower targeting options
                {
                    Height = targetOptions.Height,
                    Width = targetOptions.Width,
                    X = targetOptionPositionInGui.X,
                    Y = targetOptionPositionInGui.Y
                }); // Render the text for each of the targeting options (first, last, strong, weak).
                SplashKit.DrawText(optionType.ToString(), Color.AntiqueWhite, "BloonFont", 15, targetOptionPositionInGui.X + 7, targetOptionPositionInGui.Y + 2);
                if (tower.Targeting.TargetType != optionType) continue;
                HighlightTargetingOptionInGui(targetOptions, targetOptionPositionInGui);
            }
        }
    }
}