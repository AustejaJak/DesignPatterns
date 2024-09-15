using SplashKitSDK;
using System.Linq;

namespace BloonsProject
{
    internal class EntityRenderer
    {
        private readonly EntityDrawer _entityRenderer = new EntityDrawer();
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton

        public void DisplayTowerDebugStats(Tower tower)
        {
            string debugText;
            debugText = tower.ShotType.TimeSinceLastShot > tower.ShotType.ShotSpeed // Cooldown is displayed while on cooldown
                ? "Ready" // Otherwise display "ready".
                : tower.ShotType.TimeSinceLastShot.ToString() + "/" + tower.ShotType.ShotSpeed;
            SplashKit.DrawText(debugText, Color.Black, tower.Position.X, tower.Position.Y - 20);
        }

        public void RenderBloons(BloonController bloonController, Map map)
        {
            foreach (var bloon in _gameState.Bloons) // Draws every bloon in the game
            {
                _entityRenderer.DrawBloon(bloon);
                bloonController.MoveBloon(bloon, map);
            }
        }

        public void RenderTowerDebugMode(Tower tower) // Displays debug stats if tower has been right clicked
        {
            if (tower.DebugModeSelected)
            {
                DisplayTowerDebugStats(tower);
            }
        }

        public void RenderTowerProjectiles() // Draws all projectiles
        {
            _entityRenderer.TowerProjectileRenderer();
        }

        public void RenderTowers(TowerController towerController) // Renders all towers, their range and determines whether to display their debug mode.
        {
            foreach (var tower in _gameState.Towers.ToList())
            {
                _entityRenderer.DrawTower(tower);
                RenderTowerDebugMode(tower);
                if (!tower.Selected) continue;
                _entityRenderer.DrawTowerRange(tower);
            }
        }
    }
}