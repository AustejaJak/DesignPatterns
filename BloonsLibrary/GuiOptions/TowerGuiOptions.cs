using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.IO;

namespace BloonsProject
{
    // Concrete implementation for Tower GUI Options
    public class TowerGuiOptions : GuiOptionsBase
    {
        public Dictionary<Point2D, string> UpgradeOptionsInGui { get; }
        public string SelectedInGui { get; set; }
        public Bitmap SellTowerBitmap { get; }
        public Bitmap UpgradeFirerateBitmap { get; }
        public Bitmap UpgradeRangeBitmap { get; }
        public List<Bitmap> ClickableShapeImages { get; set; }

        public TowerGuiOptions()
        {
            Height = 50;
            Width = 100;
            SelectedInGui = "none";

            UpgradeOptionsInGui = new Dictionary<Point2D, string>
            {
                [new Point2D { X = 850, Y = 355 }] = "Upgrade Range",
                [new Point2D { X = 990, Y = 355 }] = "Upgrade Firerate",
                [new Point2D { X = 920, Y = 455 }] = "Sell"
            };

            SellTowerBitmap = FlyweightFactory.GetBitmap("Sell", TowerResources.SellIcon);
            UpgradeFirerateBitmap = FlyweightFactory.GetBitmap("Firerate", TowerResources.UpgradeFirerate);
            UpgradeRangeBitmap = FlyweightFactory.GetBitmap("Range", TowerResources.UpgradeRange);
            
            ClickableShapeImages = new List<Bitmap>() { UpgradeRangeBitmap, UpgradeFirerateBitmap, SellTowerBitmap };
        }

        protected override IEnumerable<(Point2D Position, object Value)> GetClickableOptions()
        {
            foreach (var option in UpgradeOptionsInGui)
            {
                yield return (option.Key, option.Value);
            }
        }

        protected override void UpdateSelection(object value)
        {
            SelectedInGui = (string)value;
        }

        protected override void HandleInvalidClick()
        {
            SelectedInGui = "none";
        }
    }
}