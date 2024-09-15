using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsProject
{
    public class TowerTargetingGuiOptions
    {
        public TowerTargetingGuiOptions()
        {
            TargetingButtonLocations = new Dictionary<Point2D, TowerTargeting>()
            {
                [new Point2D() { X = 830, Y = 320 }] = TowerTargeting.First,
                [new Point2D() { X = 900, Y = 320 }] = TowerTargeting.Last,
                [new Point2D() { X = 970, Y = 320 }] = TowerTargeting.Strong,
                [new Point2D() { X = 1040, Y = 320 }] = TowerTargeting.Weak
            };
            Height = 30; // Dimensions of targeting buttons
            Width = 60;
            SelectedInGui = TowerTargeting.First; // Initially have targeting set to "first".
        }

        public int Height { get; }
        public TowerTargeting SelectedInGui { get; set; }
        public Dictionary<Point2D, TowerTargeting> TargetingButtonLocations { get; }
        public int Width { get; }

        public void ClickShape(Point2D pt) // Selects a button if mouse press location is within the button's dimensions
        {
            foreach (var (position, targetOption) in TargetingButtonLocations)
            {
                if (pt.X >= position.X && pt.X <= Width + position.X && pt.Y >= position.Y &&
                    pt.Y <= position.Y + Height)
                {
                    SelectedInGui = targetOption;
                    break;
                }
            }
        }
    }
}