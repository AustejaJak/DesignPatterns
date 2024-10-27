using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsCreator
{
    public class Button
    {
        public Button(int width, int height, Bitmap templateTileBitmap, ButtonTypes buttonTypes)
        {
            Width = width;
            Height = height;
            TemplateTileBitmap = templateTileBitmap;
            ButtonType = buttonTypes;
        }

        public ButtonTypes ButtonType { get; set; }
        public int Height { get; }
        public Point2D Position { get; set; }
        public Bitmap TemplateTileBitmap { get; }
        public int Width { get; }

    }
}