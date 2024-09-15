using System;

namespace BloonsCreator
{
    public class TileFactory
    {
        public static Tile CreateTileOfType(TileType tileType)
        {
            if (tileType == TileType.Checkpoint) return new CheckpointTile();

            if (tileType == TileType.Normal) return new GrassTile();

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}