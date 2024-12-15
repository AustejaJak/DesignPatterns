using BloonsProject;

namespace BloonLibrary.Proxy
{
    using SplashKitSDK;

    public class RealEntityRenderer : IEntityRenderer
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance();

        public void DrawBloon(Bloon bloon)
        {
            SplashKit.FillCircle(bloon.Color, bloon.Position.X, bloon.Position.Y, bloon.Radius);
        }

        public void DrawTower(Tower tower)
        {
            SplashKit.DrawBitmap(tower.TowerBitmap, tower.Position.X - 13, tower.Position.Y - 13);
            SplashKit.DrawText(tower.Username, Color.Black, tower.Position.X - 10, tower.Position.Y - 10);
        }

        public void DrawTowerRange(Tower tower)
        {
            var towerCentre = new Point2D
                { X = tower.Position.X + Tower.Length / 2, Y = tower.Position.Y + Tower.Length / 2 };
            SplashKit.FillCircle(new Color() { A = 160, B = 1, G = 1, R = 1 }, new Circle { Center = towerCentre, Radius = tower.Range });
        }
        
        public void TowerProjectileRenderer()
        {
            if (_gameState.ProjectileManager.ProjectilesOnScreen == null) return;

            foreach (Projectile projectile in _gameState.ProjectileManager.ProjectilesOnScreen)
            {
                SplashKit.DrawBitmap(projectile.ProjectileShotType.ProjectileBitmap, projectile.ProjectileLocation.X, projectile.ProjectileLocation.Y);
            }
        }
    }

}