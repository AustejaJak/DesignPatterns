using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsCreator
{
    public class Renderer
    {
        public Renderer(int tileHeight, int tileWidth)
        {
            TileHeight = tileHeight;
            TileWidth = tileWidth;
        }

        private int TileHeight { get; }
        private int TileWidth { get; }

        public void RenderButton(Bitmap bitmap, Rectangle rectangle)
        {
            SplashKit.FillRectangle(Color.SwinburneRed, rectangle);
            SplashKit.DrawBitmap(bitmap, rectangle.X + 2, rectangle.Y + 15);
        }

        public void RenderGrid()
        {
            foreach (var horizontalLine in GridCalculations.GetGridHorizontalLines(TileWidth, TileHeight))
            {
                SplashKit.DrawLine(Color.Black, horizontalLine);
            }
            foreach (var verticalLine in GridCalculations.GetGridVerticalLines(TileWidth, TileHeight))
            {
                SplashKit.DrawLine(Color.Black, verticalLine);
            }
        }

        public void RenderTemplateTiles(List<TileButton> tileButtons)
        {
            tileButtons.ForEach(t => SplashKit.DrawBitmap(t.TemplateTileBitmap, t.Position.X, t.Position.Y));
        }

        public void RenderTiles(List<Tile> tileManagerTiles)
        {
            tileManagerTiles.ForEach(t =>
            {
                SplashKit.DrawBitmap(t.Bitmap, t.Position.X, t.Position.Y);
            });
        }

        public void ShadeButton(Rectangle rectangle)
        {
            var tileToShade = new Rectangle() { Height = rectangle.Height, Width = rectangle.Width, X = rectangle.X, Y = rectangle.Y };
            SplashKit.FillRectangle(new Color() { A = 200, B = 1, G = 1, R = 1 }, tileToShade);
        }
    }
}