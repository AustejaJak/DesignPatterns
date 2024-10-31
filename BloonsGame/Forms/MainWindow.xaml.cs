using BloonsProject;
using System.Windows;
using System;
using BloonLibrary;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls;
//using BloonsLibrary.Commands;
//using BloonsLibrary.Models;
using System.Windows.Input;

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
        private ObservableCollection<ChatMessage> _chatMessages;

        public MainWindow(GameClient gameClient)
        {
            InitializeComponent();
            _gameClient = gameClient;
            MapComboBox.Items.Add("The Original");
            MapComboBox.Items.Add("Farmers Paradise");

            // Initialize chat
            _chatMessages = new ObservableCollection<ChatMessage>();
            ChatListView.ItemsSource = _chatMessages;

            // Subscribe to events
            _gameClient.PlayerListUpdated += UpdatePlayerList;
            _gameClient.AllPlayersReady += StartCountdown;
            _gameClient.ChatMessageReceived += OnChatMessageReceived;
            MapComboBox.SelectionChanged += MapComboBox_SelectionChanged;
            _gameClient.MapValidationFailed += OnMapValidationFailed;

            InitializeCountdownTimer();
        }

        private void OnChatMessageReceived(ChatMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                _chatMessages.Add(message);
                ChatListView.ScrollIntoView(ChatListView.Items[ChatListView.Items.Count - 1]);
            });
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                var command = new SendMessageCommand(_gameClient, MessageTextBox.Text);
                command.Execute();
                MessageTextBox.Clear();
            }
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
            var loseWindow = new LoseWindow(_gameClient);
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

        private void MapComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MapComboBox.SelectedItem != null)
            {
                SelectMapLabelError.Visibility = Visibility.Collapsed;
                var selectedMap = MapComboBox.SelectedItem.ToString();
                _gameClient.SendSelectedMapAsync(selectedMap);

                // Reset ready status when map changes
                if (_isReady)
                {
                    _isReady = false;
                    ReadyButton.Content = "Ready";
                    _gameClient.SetPlayerReadyAsync(false);
                }
            }
        }

        private void OnMapValidationFailed(string message)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, "Map Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                
                // Reset ready status
                _isReady = false;
                ReadyButton.Content = "Ready";
                
                // Make map selection and ready button available again
                ReadyButton.IsEnabled = true;
                MapComboBox.IsEnabled = true;
            });
        }
    }
}