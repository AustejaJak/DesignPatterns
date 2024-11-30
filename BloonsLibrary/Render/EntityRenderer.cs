using BloonLibrary;
using SplashKitSDK;
using System.Linq;
using System.Threading.Tasks;

namespace BloonsProject
{
    internal class EntityRenderer
    {
        private readonly EntityDrawer _entityDrawer;
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton
        private readonly GameClient _gameClient;
        
        public EntityRenderer(GameClient gameClient)
        {
            _gameClient = gameClient; // Initialize the GameClient instance
            _entityDrawer = new EntityDrawer(gameClient);
        }

        public void DisplayTowerDebugStats(Tower tower)
        {
            string debugText;
            debugText = tower.ShotType.TimeSinceLastShot > tower.ShotType.ShotSpeed // Cooldown is displayed while on cooldown
                ? "Ready" // Otherwise display "ready".
                : tower.ShotType.TimeSinceLastShot.ToString() + "/" + tower.ShotType.ShotSpeed;
            SplashKit.DrawText(debugText, Color.Black, tower.Position.X, tower.Position.Y - 20);
        }

        public async Task RenderBloons(BloonController bloonController, Map map)
        {
            // Iterate over the values of the ConcurrentDictionary
            foreach (var bloon in _gameState.Bloons.Values) // Draws every bloon in the game
            {
                _entityDrawer.DrawBloon(bloon); // Draw the bloon on the map
                bloonController.MoveBloon(bloon, map); // Move the bloon according to game logic
                
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
            _entityDrawer.TowerProjectileRenderer();
        }

        public void RenderTowers(TowerController towerController) // Renders all towers, their range and determines whether to display their debug mode.
        { var TowerIterator = _gameState.Towers.CreateIterator();
            while (TowerIterator.MoveNext())
            {
                _entityDrawer.DrawTower(TowerIterator.Current);
                RenderTowerDebugMode(TowerIterator.Current);
                if (!TowerIterator.Current.Selected) continue;
                _entityDrawer.DrawTowerRange(TowerIterator.Current);
            }
        }
    }
}