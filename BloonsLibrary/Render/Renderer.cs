

using System;
using System.IO;
using BloonLibrary;
using Microsoft.VisualBasic;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using Color = SplashKitSDK.Color;

namespace BloonsProject
{
    public class Renderer
    {
        private readonly GameState _bloonSingleton = GameState.GetGameStateInstance();
        private readonly EntityRenderer _entityRenderer;
        private readonly GuiRenderer _guiRenderer = new GuiRenderer();
        private readonly Map _map;
        private readonly TowerOptionsRenderer _towerOptionsRenderer = new TowerOptionsRenderer();
        private readonly Window _window;
        private readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private Cursor cursor;
        private GameClient _gameClient;

        private Queue<string> _messageQueue = new();
        private DateTime _messageDisplayStartTime;
        private bool _isDisplayingMessage = false;
        private const int MessageDuration = 5;
        private string TowerActionMessage;
        private string _currentMessage;
        private bool _displayMessage = false;
        private DateTime _lastMessageTime;
        private readonly StandardBloonTowerFactory _towerFactory = new StandardBloonTowerFactory();


        // Call this method to add a message to the queue
        public void QueueMessage(string message)
        {
            _messageQueue.Enqueue(message);
        }

        public Renderer(Window window, Map map, GameClient gameClient)
        {
            _window = window;
            _map = map;
            _gameClient = gameClient;
            _entityRenderer = new EntityRenderer(_gameClient);
            SplashKit.LoadFont("BloonFont", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\BloonFont.ttf")); // Load custom font.
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
                var selectedTower = _towerFactory.CreateTowerOfType(towerPlacer.SelectedInGui, _gameClient.Username);
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


        public void RenderCursor(){
            Cursor cursor = new Cursor(Color.Black);
            _guiRenderer.DrawCursor(cursor);
        }

        // Call this method within your main render loop
        public void RenderMessages()
        {
            if (!_isDisplayingMessage && _messageQueue.Count > 0)
            {
                // Start displaying the next message
                _isDisplayingMessage = true;
                _messageDisplayStartTime = DateTime.Now;
            }

            if (_isDisplayingMessage && _messageQueue.Count > 0)
            {
                // Get the current message
                var currentMessage = _messageQueue.Peek();

                // Check if the message should still be displayed
                if ((DateTime.Now - _messageDisplayStartTime).TotalSeconds < MessageDuration)
                {
                    // Display the message at the top left of the screen
                    SplashKit.DrawText(currentMessage, Color.White, "BloonFont", 20, 20, 20);
                }
                else
                {
                    // Remove the message from the queue after 5 seconds
                    _messageQueue.Dequeue();
                    _isDisplayingMessage = false; // Ready to display the next message
                }
            }
        }

        public void RenderMessagesRight(string message)
        {
            _currentMessage = message;
            _lastMessageTime = DateTime.Now;
            _displayMessage = true; // Indicate that there’s a message to display
        }

        public void UpdateMessageDisplay()
        {
            if (_displayMessage && (DateTime.Now - _lastMessageTime).TotalSeconds < 5)
            {
                int screenWidth = SplashKit.ScreenWidth();
                int textWidth = SplashKit.TextWidth(_currentMessage, "BloonFont", 20);
                int xPosition = screenWidth - textWidth - 20; // 20 px margin from the right

                // Display the message at the top right of the screen
                SplashKit.DrawText(_currentMessage, Color.White, "BloonFont", 20, xPosition, 20);
            }
            else
            {
                // Stop displaying the message after 5 seconds
                _displayMessage = false;
            }
        }
    }
}
