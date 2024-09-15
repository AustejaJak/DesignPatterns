using SplashKitSDK;

namespace BloonsCreator

{
    public class TileButton : Button
    {
        public TileButton(TileType tileType, Bitmap templateTileBitmap, ButtonTypes buttonType, Point2D position) : base(100, 100, templateTileBitmap, buttonType)
        {
            TileType = tileType;
            Position = position;
        }

        public TileType TileType { get; }
    }
}