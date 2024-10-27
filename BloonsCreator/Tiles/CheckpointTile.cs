using SplashKitSDK;

namespace BloonsCreator
{
    public class CheckpointTile : Tile
    {
        public CheckpointTile() : base(TileType.Checkpoint, new Bitmap("stone", "../../BloonsLibrary/Resources/stone.png"))
        {
        }

        public Point2D Checkpoint => new Point2D() { X = Position.X + Width / 2, Y = Position.Y + Height / 2 };
    }
}