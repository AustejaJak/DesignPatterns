using SplashKitSDK;

namespace BloonsCreator
{
    public class GrassTile : Tile
    {
        public GrassTile() : base(TileType.Normal, new Bitmap("grass", "../../BloonsLibrary/Resources/grass.png"))
        {
        }
    }
}