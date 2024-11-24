using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace BloonsProject
{
    // Concrete implementation for Tower Placer GUI Options
    public class TowerPlacerGuiOptions : GuiOptionsBase
    {
        public Dictionary<Point2D, string> ClickableShapes { get; }
        public string SelectedInGui { get; set; }

        public TowerPlacerGuiOptions()
        {
            Height = 112;
            Width = 82;
            SelectedInGui = "none";
            
            ClickableShapes = new Dictionary<Point2D, string>
            {
                [new Point2D { X = 845, Y = 200 }] = DartTower.Name,
                [new Point2D { X = 935, Y = 200 }] = LaserTower.Name,
                [new Point2D { X = 1025, Y = 200 }] = SniperTower.Name
            };
        }

        public Bitmap ClickableShapeImage(string towerName)
        {
            if (towerName == DartTower.Name) return DartTower.Portrait;
            if (towerName == LaserTower.Name) return LaserTower.Portrait;
            if (towerName == SniperTower.Name) return SniperTower.Portrait;
            throw new Exception();
        }

        protected override IEnumerable<(Point2D Position, object Value)> GetClickableOptions()
        {
            foreach (var shape in ClickableShapes)
            {
                yield return (shape.Key, shape.Value);
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