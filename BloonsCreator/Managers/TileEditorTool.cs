using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using Rectangle = SplashKitSDK.Rectangle;

namespace BloonsCreator
{
    public class TileEditorTool
    {
        private readonly CreatorState _creatorState = CreatorState.GetClickHandlerEvents();

        public TileEditorTool()
        {
            TemplateTiles = new List<TileButton>
            {
                TileButtonFactory.CreateTileOfType(TileType.Normal, new Point2D() {X = 100, Y = 575}),
                TileButtonFactory.CreateTileOfType(TileType.Checkpoint, new Point2D { X = 600, Y = 575 })
        };
            foreach (TileButton tileButton in TemplateTiles)
            {
                _creatorState.Buttons.Add(tileButton);
            }
            SelectedTileType = TileType.Checkpoint;
            _creatorState.buttonClickEvent += OnButtonClicked;
        }

        public TileType SelectedTileType { get; set; }
        public List<TileButton> TemplateTiles { get; }

        public void AddTileAt(Point2D mousePosition)
        {
            if (mousePosition.Y >= 550) return;

            var tileToAdd = GetTileOnGrid(mousePosition);
            var existingTile = _creatorState.Tiles.FirstOrDefault(t => t.Position.X == tileToAdd.Position.X && t.Position.Y == tileToAdd.Position.Y);

            if (SelectedTileType == TileType.Checkpoint)
            {
                if (!CanAddCheckpointTile(tileToAdd as CheckpointTile)) return;

                var checkpointTile = tileToAdd as CheckpointTile;
                _creatorState.Checkpoints.Add(checkpointTile.Checkpoint);
            }
            else if (SelectedTileType == TileType.Normal)
            {
                if (!CanRemoveCheckpointTile(existingTile)) return;
                RemoveCheckpoint(existingTile);
            }
            RemoveTile(existingTile);
            _creatorState.Tiles.Add(tileToAdd);
        }

        public Tile GetTileOnGrid(Point2D mousePosition)
        {
            var tileToAdd = TileFactory.CreateTileOfType(SelectedTileType);
            var gridPoints = GridCalculations.GetGridPoints(tileToAdd.Width, tileToAdd.Height);
            var gridPoint = gridPoints.FirstOrDefault(p => SplashKit.PointInRectangle(mousePosition,
                new Rectangle { Width = tileToAdd.Width, Height = tileToAdd.Height, X = p.X, Y = p.Y }));
            tileToAdd.Position = gridPoint;
            return tileToAdd;
        }

        public bool CanRemoveCheckpointTile(Tile tile)
        {
            var checkpointTiles = _creatorState.Tiles.Where(t => t.TileType == TileType.Checkpoint);
            if (checkpointTiles.Count() == 0) return false;
            return tile.Position.X == checkpointTiles.Last().Position.X && tile.Position.Y == checkpointTiles.Last().Position.Y;
        }

        public bool CanAddCheckpointTile(CheckpointTile checkpointTile)
        {
            if (_creatorState.Tiles.All(t => t.TileType != TileType.Checkpoint)) return true;
            var checkpointTiles = _creatorState.Tiles.Where(t => t.TileType == TileType.Checkpoint);
            var lastCheckPointTile = checkpointTiles.Last() as CheckpointTile;
            var distanceBetweenPoints = Math.Sqrt(
                Math.Pow((lastCheckPointTile.Checkpoint.X - checkpointTile.Checkpoint.X), 2) +
                Math.Pow((lastCheckPointTile.Checkpoint.Y - checkpointTile.Checkpoint.Y), 2));
            return distanceBetweenPoints <= lastCheckPointTile.Height && distanceBetweenPoints != 0;
        }

        public TileButton CurrentSelectedTileButton()
        {
            return TemplateTiles.FirstOrDefault(tileButton => tileButton.TileType == SelectedTileType);
        }

        public void InitializeAllTilesAsGreen()
        {
            foreach (Point2D gridPoint in GridCalculations.GetGridPoints(50, 50))
            {
                _creatorState.Tiles.Add(new GrassTile() { Position = gridPoint });
            }
        }

        public void RemoveCheckpoint(Tile tile)
        {
            var tileAsCheckpoint = tile as CheckpointTile;
            _creatorState.Checkpoints.Remove(tileAsCheckpoint.Checkpoint);
        }

        public void RemoveTile(Tile tile)
        {
            foreach (var t in _creatorState.Tiles.ToList()
                .Where(t => t.Position.X == tile.Position.X && t.Position.Y == tile.Position.Y))
            {
                _creatorState.Tiles.Remove(t);
            }
        }

        public void OnButtonClicked(Button button)
        {
            if (button is not TileButton subjectAsTileButton) return;
            SelectedTileType = subjectAsTileButton.TileType;
        }
    }
}