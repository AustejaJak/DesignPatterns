using BloonLibrary;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloonsProject
{
    /// <summary>
    /// Facade for the rendering subsystem that simplifies the rendering interface
    /// </summary>
    public class RenderingFacade
    {
        private readonly Renderer _renderer;
        private readonly EntityRenderer _entityRenderer;
        private readonly GuiRenderer _guiRenderer;
        private readonly TowerOptionsRenderer _towerOptionsRenderer;
        private readonly Window _window;
        private readonly Map _map;
        private readonly GameClient _gameClient;

        //private Queue<string> _messageQueue = new();
        //private DateTime _messageDisplayStartTime;
        //private bool _isDisplayingMessage = false;
        //private const int MessageDuration = 5;
        //private string TowerActionMessage;
        //private string _currentInvalidTowerMessage;
        //private bool _displayInvalidTowerMessage = false;
        //private DateTime _lastInvalidTowerMessageTime;

        public RenderingFacade(Window window, Map map, GameClient gameClient)
        {
            _window = window;
            _map = map;
            _gameClient = gameClient;
            _renderer = new Renderer(window, map, gameClient);
            _entityRenderer = new EntityRenderer(gameClient);
            _guiRenderer = new GuiRenderer();
            _towerOptionsRenderer = new TowerOptionsRenderer();
        }

        /// <summary>
        /// Renders the complete game frame including all entities, GUI, and map
        /// </summary>
        public async Task RenderFrame(
            BloonController bloonController,
            TowerController towerController,
            TowerGuiOptions towerOptions,
            TowerTargetingGuiOptions targetOptions,
            TowerPlacerGuiOptions towerPlacer,
            MapController mapController)
        {
            // Render base map and GUI elements
            RenderBaseElements();

            // Render all game entities
            await RenderGameEntities(bloonController, towerController, towerOptions, targetOptions);

            // Render GUI elements
            RenderGuiElements(towerPlacer, towerController, mapController, towerOptions, targetOptions);

            // Render custom cursor
            RenderCursor();
        }

        /// <summary>
        /// Renders the base map and GUI background
        /// </summary>
        private void RenderBaseElements()
        {
            _renderer.RenderMap();
        }

        /// <summary>
        /// Renders all game entities including bloons, towers and projectiles
        /// </summary>
        private async Task RenderGameEntities(
            BloonController bloonController,
            TowerController towerController,
            TowerGuiOptions towerOptions,
            TowerTargetingGuiOptions targetOptions)
        {
            await _entityRenderer.RenderBloons(bloonController, _map);
            _entityRenderer.RenderTowers(towerController);
            _entityRenderer.RenderTowerProjectiles();
        }

        /// <summary>
        /// Renders all GUI elements including tower options and targeting
        /// </summary>
        private void RenderGuiElements(
            TowerPlacerGuiOptions towerPlacer,
            TowerController towerController,
            MapController mapController,
            TowerGuiOptions towerOptions,
            TowerTargetingGuiOptions targetOptions)
        {
            _renderer.RenderGuiTowerOptions(towerPlacer, towerController, mapController);
            _renderer.RenderSelectedTowerOptions(towerOptions, targetOptions);
        }

        /// <summary>
        /// Renders the custom cursor
        /// </summary>
        private void RenderCursor()
        {
            _renderer.RenderCursor();
        }

        /// <summary>
        /// Updates the window display
        /// </summary>
        public void RefreshDisplay()
        {
            _window.Refresh(60);
        }

        public void QueueMessage(string message)
        {
            _renderer.QueueMessage(message);
        }

        public void RenderTowerUpgradeMessages()
        {
            _renderer.RenderMessages();
        }

        public void RenderInvalidTowerActionMessages(string message)
        {
            _renderer.RenderMessagesRight(message);
        }

        public void UpdateMessageDisplay()
        {
            _renderer.UpdateMessageDisplay();
        }
    }
}