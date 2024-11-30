using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsProject
{
    // Abstract base class defining the template
    public abstract class GuiOptionsBase
    {
        public int Height { get; set; }
        public int Width { get; set; }

        // Template method that defines the algorithm structure
        public void HandleClick(Point2D pt)
        {
            if (!IsClickValid(pt))
            {
                HandleInvalidClick();
                return;
            }

            foreach (var option in GetClickableOptions())
            {
                if (IsWithinBounds(pt, option.Position))
                {
                    UpdateSelection(option.Value);
                    break;
                }
            }
        }

        // Hook method - can be overridden by subclasses
        protected virtual bool IsClickValid(Point2D pt)
        {
            return true;
        }

        // Abstract methods that must be implemented by subclasses
        protected abstract IEnumerable<(Point2D Position, object Value)> GetClickableOptions();
        protected abstract void UpdateSelection(object value);
        protected abstract void HandleInvalidClick();

        // Common functionality for all GUI options
        protected bool IsWithinBounds(Point2D pt, Point2D position)
        {
            return pt.X >= position.X && 
                   pt.X <= Width + position.X && 
                   pt.Y >= position.Y && 
                   pt.Y <= Height + position.Y;
        }
    }
}