using BloonsProject;
using System.Windows;
using System;
using BloonLibrary;
using System.Collections.Generic;
using System.Windows.Threading;

namespace BloonsGame
{
    public partial class MainWindow : Window
    {
        private PauseWindow _pauseWindow;
        private IProgramController _programController;
        private GameClient _gameClient;
        private bool _isReady = false;
        private DispatcherTimer _countdownTimer;
        private int _countdownSeconds = 5;

        public MainWindow(GameClient gameClient)
        {
            InitializeComponent();
            _gameClient = gameClient;
            MapComboBox.Items.Add("The Original");

            // Subscribe to player list updates
            _gameClient.PlayerListUpdated += UpdatePlayerList;
            _gameClient.AllPlayersReady += StartCountdown;
            
            InitializeCountdownTimer();
        }

        private void InitializeCountdownTimer()
        {
            _countdownTimer = new DispatcherTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += CountdownTick;
        }

        private void CountdownTick(object sender, EventArgs e)
        {
            _countdownSeconds--;
            CountdownLabel.Content = _countdownSeconds.ToString();

            if (_countdownSeconds <= 0)
            {
                _countdownTimer.Stop();
                StartGame();
            }
        }

        private void StartCountdown()
        {
            Dispatcher.Invoke(() =>
            {
                ReadyButton.IsEnabled = false;
                MapComboBox.IsEnabled = false;
                CountdownLabel.Visibility = Visibility.Visible;
                CountdownLabel.Content = _countdownSeconds.ToString();
                _countdownTimer.Start();
            });
        }

        private void StartGame()
        {
            var map = MapManager.GetMapByName(MapComboBox.SelectedItem.ToString());
            _programController = new SplashKitController(map, _gameClient);
            OpenPauseScreen();
            _pauseWindow.Hide();
            Close();
            _programController.PauseEventHandler += OpenPauseScreen;
            _programController.LoseEventHandler += OpenLossScreen;
            _programController.Start();
        }
        private void UpdatePlayerList(List<PlayerStatus> players)
        {
            Dispatcher.Invoke(() =>
            {
                PlayerListView.ItemsSource = players;
            });
        }

        public void OpenLossScreen()
        {
            var loseWindow = new LoseWindow();
            loseWindow.Show();
        }

        public void OpenPauseScreen()
        {
            _pauseWindow = new PauseWindow(_programController);
            _pauseWindow.Show();
        }

        private async void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            if (MapComboBox.SelectedItem == null)
            {
                SelectMapLabelError.Visibility = Visibility.Visible;
                return;
            }

            _isReady = !_isReady;
            ReadyButton.Content = _isReady ? "Not Ready" : "Ready";
            await _gameClient.SetPlayerReadyAsync(_isReady);
        }
    }
}