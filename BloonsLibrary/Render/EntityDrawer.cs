using SplashKitSDK;

namespace BloonsProject
{
    internal class EntityDrawer
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton

        public void DrawBloon(Bloon bloon) // Draws a circle when the bloons position is.
        {
            SplashKit.FillCircle(bloon.Color, bloon.Position.X, bloon.Position.Y, bloon.Radius);
        }

        public void DrawTower(Tower tower) // Draws a tower when the tower position is.
        {
            SplashKit.DrawBitmap(tower.TowerBitmap, tower.Position.X - 13, tower.Position.Y - 13);
        }

        public void DrawTowerRange(Tower tower) // Draws the towers range
        {
            var towerCentre = new Point2D
            { X = tower.Position.X + Tower.Length / 2, Y = tower.Position.Y + Tower.Length / 2 };
            SplashKit.FillCircle(new Color() { A = 160, B = 1, G = 1, R = 1 }, new Circle { Center = towerCentre, Radius = tower.Range });
        }

        public void TowerProjectileRenderer() // Draw all projectiles
        {
            if (_gameState.ProjectileManager.ProjectilesOnScreen == null) return;
            foreach (Projectile projectile in _gameState.ProjectileManager.ProjectilesOnScreen)
            {
                SplashKit.DrawBitmap(projectile.ProjectileShotType.ProjectileBitmap, projectile.ProjectileLocation.X, projectile.ProjectileLocation.Y);
            }
        }
    }
}