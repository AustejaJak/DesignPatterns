using SplashKitSDK;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class Renderer
    {
        private readonly GameState _bloonSingleton = GameState.GetGameStateInstance();
        private readonly EntityRenderer _entityRenderer = new EntityRenderer();
        private readonly GuiRenderer _guiRenderer = new GuiRenderer();
        private readonly Map _map;
        private readonly TowerOptionsRenderer _towerOptionsRenderer = new TowerOptionsRenderer();
        private readonly Window _window;

        public Renderer(Window window, Map map)
        {
            _window = window;
            _map = map;
            SplashKit.LoadFont("BloonFont", "../BloonsLibrary/Resources/BloonFont.ttf"); // Load custom font.
        }

        public void RenderEntities(BloonController bloonController, TowerController towerController, TowerGuiOptions towerOptions, TowerTargetingGuiOptions targetOptions)
        {
            _entityRenderer.RenderBloons(bloonController, _map); // Render bloons, towers and projectiles.
            _entityRenderer.RenderTowers(towerController);
            _entityRenderer.RenderTowerProjectiles();
        }

        public void RenderGuiTowerOptions(TowerPlacerGuiOptions towerPlacer, TowerController towerController, MapController mapController)
        {
            // Render tower select images
            foreach (var (towerPositionInGui, towerName) in towerPlacer.ClickableShapes)
            {
                // Render towers in GUI that can be placed.
                SplashKit.DrawBitmap(towerPlacer.ClickableShapeImage(towerName), towerPositionInGui.X, towerPositionInGui.Y);

                if (towerPlacer.SelectedInGui != towerName) continue;

                // Render outline around selected tower select image
                _guiRenderer.HighlightTowerInGui(towerPlacer, towerPositionInGui);

                // Create a new tower depending on the selected tower and write tower description in GUI
                var selectedTower = TowerFactory.CreateTowerOfType(towerPlacer.SelectedInGui);
                _guiRenderer.WriteTowerDescription(towerPlacer, selectedTower);

                // If you have enough money, selecting tower will draw the tower at your mouses location to place.
                if (!towerController.HaveSufficientFundsToPlaceTower(selectedTower)) continue;

                var validPlacement = mapController.CanPlaceTowerOnMap(SplashKit.MousePosition(), _map);
                _guiRenderer.RenderTowerOnCursorWhilePlacing(selectedTower, validPlacement);
            }
        }

        public void RenderMap()
        {
            SplashKit.DrawBitmapOnWindow(_window, SplashKit.LoadBitmap("map", _map.BloonsMap), 0, 0); // Renders map and GUI
            SplashKit.DrawBitmapOnWindow(_window, Gui.GuiBitmap, 800, 0);
            SplashKit.DrawText(_bloonSingleton.Player.Round.ToString(), Color.AntiqueWhite, "BloonFont", 20, 950, 25); // Renders money, lives and round.
            SplashKit.DrawText(_bloonSingleton.Player.Money.ToString(), Color.AntiqueWhite, "BloonFont", 20, 950, 65);
            SplashKit.DrawText(_bloonSingleton.Player.Lives.ToString(), Color.AntiqueWhite, "BloonFont", 20, 950, 100);
        }

        public void RenderSelectedTowerOptions(TowerGuiOptions towerOptions, TowerTargetingGuiOptions targetOptions)
        {
            _towerOptionsRenderer.RenderSelectedTowerOptions(towerOptions, targetOptions); // If tower is selected, render it's options (buy, sell, targeting).
        }
    }
}
