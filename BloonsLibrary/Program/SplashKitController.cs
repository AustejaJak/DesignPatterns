using SplashKitSDK;
using System;
using System.Diagnostics;

namespace BloonsProject
{
    public class SplashKitController : IProgramController
    {
        private readonly BloonController _bloonController = new BloonController();
        private readonly Stopwatch _bloonStopWatch = new Stopwatch();
        private readonly GameController _gameController = new GameController();
        private readonly GameState _gameState = GameState.GetGameStateInstance();
        private readonly Map _map;
        private readonly MapController _mapController = new MapController();
        private readonly Renderer _renderer;
        private readonly TowerTargetingGuiOptions _targetOptions = new TowerTargetingGuiOptions();
        private readonly TowerController _towerController = new TowerController();
        private readonly TowerGuiOptions _towerOptions = new TowerGuiOptions();
        private readonly TowerPlacerGuiOptions _towerPlacer = new TowerPlacerGuiOptions();
        private readonly Window _window;
        private bool _isPaused;

        public SplashKitController(Map map)
        {
            _map = map;
            _window = new Window("Bloons", 1135, 550);
            _renderer = new Renderer(_window, _map);
        }

        public event Action LoseEventHandler;

        public event Action PauseEventHandler;

        public void SetIsGameRunningTo(bool isRunning)
        {
            _isPaused = isRunning;
        }

        public void Start()
        {
            StartUpGame();
            do
            {
                SplashKit.RefreshScreen(60);
                SplashKit.ProcessEvents();

                if (_isPaused) continue; // If game is paused, stop running the game loop.

                DrawBloonsGame(); // Renders everything
                GameEvents(); // Checks game events

                if (SplashKit.MouseClicked(MouseButton.LeftButton)) // If a left click is made, iterate through events relating to the selection of a tower.
                {
                    SelectedTowerEvents();
                }

                if (SplashKit.MouseClicked(MouseButton.RightButton))  // If a right click is made, iterate through events relating to a tower's debug mode.
                {
                    SelectedDebugTowerEvents();
                }

                if (SplashKit.KeyTyped(KeyCode.PKey)) // If p is pressed, pause.
                {
                    PauseEventHandler?.Invoke(); // Communicate to WPF project to display the pause screen.
                    SetIsGameRunningTo(true);
                }
            } while (!SplashKit.WindowCloseRequested("Bloons"));
        }

        private void DrawBloonsGame() // Renders the game
        {
            _renderer.RenderMap();
            _renderer.RenderGuiTowerOptions(_towerPlacer, _towerController, _mapController);
            _renderer.RenderEntities(_bloonController, _towerController, _towerOptions, _targetOptions);
            _renderer.RenderSelectedTowerOptions(_towerOptions, _targetOptions);
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
                var tower = TowerFactory.CreateTowerOfType(_towerPlacer.SelectedInGui);
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