using SplashKitSDK;

namespace BloonsCreator
{
    public class Tile
    {
        public Tile(TileType isBloonsTrack, Bitmap bitmap)
        {
            TileType = isBloonsTrack;
            Bitmap = bitmap;
            Height = 50;
            Width = 50;
        }

        public Bitmap Bitmap { get; }
        public int Height { get; }
        public TileType TileType { get; }
        public Point2D Position { get; set; }
        public int Width { get; }
    }
}