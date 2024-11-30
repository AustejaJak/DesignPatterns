using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsProject
{
    // Concrete implementation for Tower Targeting GUI Options
    public class TowerTargetingGuiOptions : GuiOptionsBase
    {
        public Dictionary<Point2D, TowerTargeting> TargetingButtonLocations { get; }
        public TowerTargeting SelectedInGui { get; set; }

        public TowerTargetingGuiOptions()
        {
            Height = 30;
            Width = 60;
            SelectedInGui = TowerTargeting.First;
            
            TargetingButtonLocations = new Dictionary<Point2D, TowerTargeting>()
            {
                [new Point2D() { X = 830, Y = 320 }] = TowerTargeting.First,
                [new Point2D() { X = 900, Y = 320 }] = TowerTargeting.Last,
                [new Point2D() { X = 970, Y = 320 }] = TowerTargeting.Strong,
                [new Point2D() { X = 1040, Y = 320 }] = TowerTargeting.Weak
            };
        }

        protected override IEnumerable<(Point2D Position, object Value)> GetClickableOptions()
        {
            foreach (var option in TargetingButtonLocations)
            {
                yield return (option.Key, option.Value);
            }
        }

        protected override void UpdateSelection(object value)
        {
            SelectedInGui = (TowerTargeting)value;
        }

        protected override void HandleInvalidClick()
        {
            // For targeting, we keep the current selection instead of resetting
        }
    }
}