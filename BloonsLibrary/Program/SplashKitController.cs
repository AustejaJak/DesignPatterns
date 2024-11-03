using BloonLibrary;
using SplashKitSDK;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BloonsProject
{
    public class SplashKitController : IProgramController
    {
        private readonly Stopwatch _bloonStopWatch = new Stopwatch();
        private readonly GameController _gameController = new GameController();
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly Map _map;
        private readonly MapController _mapController = new MapController();
        private readonly TowerTargetingGuiOptions _targetOptions = new TowerTargetingGuiOptions();
        private readonly TowerGuiOptions _towerOptions = new TowerGuiOptions();
        private readonly TowerPlacerGuiOptions _towerPlacer = new TowerPlacerGuiOptions();
        private readonly Window _window;
        private readonly RenderingFacade _renderingFacade;  // Replace Renderer with RenderingFacade
        private bool _isPaused;

        private GameClient _gameClient;
        private readonly TowerController _towerController;
        private readonly BloonController _bloonController;
        private readonly TowerFactory _towerFactory = new TowerFactory();

        public SplashKitController(Map map, GameClient gameClient)
        {
            _map = map;
            _gameClient = gameClient;
            _towerController = new TowerController(_gameClient);
            _bloonController = new BloonController(_gameClient);
            _window = new Window("Bloons", 1135, 550);
            _renderingFacade = new RenderingFacade(_window, _map, _gameClient);  // Initialize facade instead of Renderer
        }

        public event Action LoseEventHandler;
        public event Action PauseEventHandler;

        public void SetIsGameRunningTo(bool isRunning)
        {
            _isPaused = isRunning;
        }

        public async void Start()  // Made async to handle async rendering
        {
            StartUpGame();
            do
            {

                
                if (_isPaused)
                {
                    SplashKit.ProcessEvents();
                    continue;
                } // If game is paused, stop running the game loop.

                await DrawBloonsGame(); // Renders everything

                string message = "";
                _gameState.TowerEventMessages.TryDequeue(out message);
                if (message != null)
                {
                    _renderingFacade.QueueMessage(message);
                }
                if (_gameState.InvalidTowerEventMessage != null)
                {
                    _renderingFacade.RenderInvalidTowerActionMessages(_gameState.InvalidTowerEventMessage);
                    _gameState.InvalidTowerEventMessage = null;
                }
                _renderingFacade.UpdateMessageDisplay();
                _renderingFacade.RenderTowerUpgradeMessages();

                GameEvents(); // Checks game events

                if (SplashKit.MouseClicked(MouseButton.LeftButton)) // If a left click is made, iterate through events relating to the selection of a tower.
                {
                    SelectedTowerEvents();
                }

                if (SplashKit.MouseClicked(MouseButton.RightButton))  // If a right click is made, iterate through events relating to a tower's debug mode.
                {
                    SelectedDebugTowerEvents();
                }

                if (SplashKit.KeyTyped(KeyCode.PKey))
                {
                    PauseEventHandler?.Invoke(); // Communicate to WPF project to display the pause screen.
                    SetIsGameRunningTo(true);
                }

                _renderingFacade.RefreshDisplay();  // Refresh the display using the facade
                SplashKit.ProcessEvents();
                
            } while (!SplashKit.WindowCloseRequested("Bloons"));
        }

        private async Task DrawBloonsGame() // Simplified rendering using facade
        {
            await _renderingFacade.RenderFrame(
                _bloonController,
                _towerController,
                _towerOptions,
                _targetOptions,
                _towerPlacer,
                _mapController
            );
        }

        private void GameEvents() // Checks game events
        {
            if (_gameController.HaveLivesDepleted())
            {
                SplashKit.CloseWindow("Bloons");
                LoseEventHandler?.Invoke();
            }

            _towerController.UpgradeOrSellSelectedTower(_towerController, _towerOptions);
            _towerController.ChangeTowerTargeting(_targetOptions, _towerController);
            _gameController.LoseLivesAndRemoveBloons(_map);
            _towerController.ShootBloons(_map);
            _bloonController.CheckBloonHealth();
            _towerController.TickAllTowers();

            if (_gameController.RequiredBloonsHaveSpawned() && _bloonController.BloonsOnScreen(_window) == 0)
            {
                _gameState.Player.Round++;
                _gameState.Player.Money += 50;
                _gameController.SetRound(_map, _gameState.Player.Round);
            }

            _bloonController.ProcessBloons(_gameState.Player, _map);
        }

        private void SelectedDebugTowerEvents() // Events relating to a tower's debug mode.
        {
            _mapController.ClickOnMap(SplashKit.MousePosition(), _towerOptions, _targetOptions, MouseClickType.right);
        }

        private void SelectedTowerEvents() // Events relating to a tower that has been selected
        {
            _mapController.ClickOnMap(SplashKit.MousePosition(), _towerOptions, _targetOptions, MouseClickType.left);
            if (_mapController.CanPlaceTowerOnMap(SplashKit.MousePosition(), _map))
            {
                if (_towerPlacer.SelectedInGui == "none") return;
                var tower = _towerFactory.CreateTowerOfType(_towerPlacer.SelectedInGui, _gameClient.Username);
                _towerController.AddTower(tower);
                _towerPlacer.SelectedInGui = "none";
            }
            else if (!_mapController.CanPlaceTowerOnMap(SplashKit.MousePosition(), _map))
            {
                _towerPlacer.ClickShape(SplashKit.MousePosition());
                _towerOptions.ClickShape(SplashKit.MousePosition());
                _targetOptions.ClickShape(SplashKit.MousePosition());
            }
        }

        private void StartUpGame() // Start game and set the initial round.
        {
            _bloonStopWatch.Start();
            _gameController.SetRound(_map, _gameState.Player.Round);
        }
    }
}