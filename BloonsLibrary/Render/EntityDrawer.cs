using System.Reflection;
using System.Threading.Tasks;
using BloonLibrary;
using BloonLibrary.Proxy;
using SplashKitSDK;

namespace BloonsProject
{
    internal class EntityDrawer
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton
        private readonly GameClient _gameClient;
        private readonly IEntityRenderer _entityRenderer;

        public EntityDrawer(GameClient gameClient)
        {
            _gameClient = gameClient; // Store the GameClient instance
            _entityRenderer = new EntityRendererProxy();
        }

        public void DrawBloon(Bloon bloon) // Draws a circle when the bloons position is.
        {
            _entityRenderer.DrawBloon(bloon);
        }

        public void DrawTower(Tower tower) // Draws a tower when the tower position is.
        {
            _entityRenderer.DrawTower(tower);
        }


        public void DrawTowerRange(Tower tower) // Draws the towers range
        {
            _entityRenderer.DrawTowerRange(tower);
        }

        public void TowerProjectileRenderer() // Draw all projectiles
        {
            _entityRenderer.TowerProjectileRenderer();
        }
    }
}