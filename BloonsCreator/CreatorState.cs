using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;

namespace BloonsCreator
{
    public class CreatorState
    {
        private static CreatorState _state;

        public List<Point2D> Checkpoints = new List<Point2D>();
        public List<Tile> Tiles = new List<Tile>();
        public List<Button> Buttons = new List<Button>();
        public delegate void ButtonClickHandler(Button button);
        public event ButtonClickHandler buttonClickEvent;
        public Window Window;

        private static readonly object Locker = new object();

        protected CreatorState()
        {
        }

        public static CreatorState GetClickHandlerEvents()
        {
            if (_state == null)
            {
                lock (Locker)
                {
                    if (_state == null)
                    {
                        _state = new CreatorState();
                    }
                }
            }

            return _state;
        }

        public void UpdateOnButtonPress()
        {
            foreach (var button in Buttons.Where(button => SplashKit.PointInRectangle(SplashKit.MousePosition(),
                new Rectangle()
                    { Height = button.Height, Width = button.Width, X = button.Position.X, Y = button.Position.Y })))
            {
                buttonClickEvent?.Invoke(button);
                break;
            }
        }
    }
}