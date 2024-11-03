using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.IO;

namespace BloonsProject
{
    public class TowerGuiOptions
    {
        public TowerGuiOptions()
        {
            UpgradeOptionsInGui = new Dictionary<Point2D, string> // Location of upgrade buttons.
            {
                [new Point2D { X = 850, Y = 355 }] = "Upgrade Range",
                [new Point2D { X = 990, Y = 355 }] = "Upgrade Firerate",
                [new Point2D { X = 920, Y = 455 }] = "Sell"
            };
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // current Folder path
            Height = 50; // Dimensions of buttons
            Width = 100;
            SelectedInGui = "none";
            SellTowerBitmap = new Bitmap("Sell", // Bitmaps for buttons.
                Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\sellTower.png"));
            UpgradeFirerateBitmap = new Bitmap("Firerrate",
                Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\firerateUpgrade.png"));
            UpgradeRangeBitmap = new Bitmap("Range",
                Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\rangeUpgrade.png"));
            ClickableShapeImages = new List<Bitmap>() { UpgradeRangeBitmap, UpgradeFirerateBitmap, SellTowerBitmap }; // List of buttons.
        }

        public List<Bitmap> ClickableShapeImages { get; set; }
        public int Height { get; }
        public string SelectedInGui { get; set; }
        public Bitmap SellTowerBitmap { get; }
        public Bitmap UpgradeFirerateBitmap { get; }
        public Dictionary<Point2D, string> UpgradeOptionsInGui { get; }
        public Bitmap UpgradeRangeBitmap { get; }
        public int Width { get; }

        public void ClickShape(Point2D pt) // Selects a button if mouse press location is within the button's dimensions
        {
            foreach (var (position, towerOption) in UpgradeOptionsInGui)
            {
                if (pt.X >= position.X && pt.X <= Width + position.X && pt.Y >= position.Y &&
                    pt.Y <= position.Y + Height)
                {
                    SelectedInGui = towerOption; // Sets SelectedInGui to the selected button.
                    break;
                }
                SelectedInGui = "none";
            }
        }
    }
}