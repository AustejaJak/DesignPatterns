using SplashKitSDK;
using System;

namespace BloonsCreator
{
    public class TileButtonFactory
    {
        public static TileButton CreateTileOfType(TileType tileType, Point2D position)
        {
            if (tileType == TileType.Checkpoint) return new TileButton(TileType.Checkpoint, new Bitmap("stoneBig", "../../BloonsLibrary/Resources/stoneBig.png"), ButtonTypes.AddRegularTile, position);

            if (tileType == TileType.Normal) return new TileButton(TileType.Normal, new Bitmap("grassBig", "../../BloonsLibrary/Resources/grassBig.png"), ButtonTypes.AddCheckpointTile, position);

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}