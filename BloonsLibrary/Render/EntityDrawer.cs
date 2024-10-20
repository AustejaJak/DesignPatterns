using System.Reflection;
using System.Threading.Tasks;
using BloonLibrary;
using SplashKitSDK;

namespace BloonsProject
{
    internal class EntityDrawer
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton
        private readonly GameClient _gameClient;

        public EntityDrawer(GameClient gameClient)
        {
            _gameClient = gameClient; // Store the GameClient instance
        }

        public void DrawBloon(Bloon bloon) // Draws a circle when the bloons position is.
        {
            SplashKit.FillCircle(bloon.Color, bloon.Position.X, bloon.Position.Y, bloon.Radius);
        }

        public void DrawTower(Tower tower) // Draws a tower when the tower position is.
        {
            // Draw the tower bitmap at its position
            SplashKit.DrawBitmap(tower.TowerBitmap, tower.Position.X - 13, tower.Position.Y - 13);
            
            // Draw the username on the tower
            SplashKit.DrawText(tower.Username, Color.Black, tower.Position.X - 10, tower.Position.Y - 10);
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