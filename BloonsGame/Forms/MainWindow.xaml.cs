using BloonsProject;
using System.Windows;
using System;
using BloonLibrary;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

using BloonsGame.Forms;

using BloonsGame.States;


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

        private CommandParser _commandParser;
        private Context _chatContext;

        private DispatcherTimer _infoMessageTimer;


        private UserController _userController;

        

        // The current UI state of the MainWindow
        private IGameUIState _currentState;

        public MainWindow(GameClient gameClient, UserController userController)

        {
            InitializeComponent();
            _gameClient = gameClient;
            _userController = userController;
            MapComboBox.Items.Add("The Original");
            MapComboBox.Items.Add("Farmers Paradise");
            MapComboBox.Items.Add("Ocean Road");

            // Initialize chat components
            _chatMessages = new ObservableCollection<ChatMessage>();
            ChatListView.ItemsSource = _chatMessages;
            _commandParser = new CommandParser();

            // Initialize info message timer
            _infoMessageTimer = new DispatcherTimer();
            _infoMessageTimer.Interval = TimeSpan.FromSeconds(5);
            _infoMessageTimer.Tick += InfoMessageTimer_Tick;

            // Subscribe to events
            _gameClient.PlayerListUpdated += UpdatePlayerList;
            _gameClient.AllPlayersReady += OnAllPlayersReady; // Use the state pattern
            _gameClient.ChatMessageReceived += OnChatMessageReceived;
            _gameClient.PrivateMessageReceived += OnPrivateMessageReceived;
            _gameClient.InfoMessageReceived += OnInfoMessageReceived;
            _gameClient.MessageDeleted += OnMessageDeleted;
            MapComboBox.SelectionChanged += MapComboBox_SelectionChanged;
            _gameClient.MapValidationFailed += OnMapValidationFailed;

            InitializeCountdownTimer();

            // Initialize the UI in the LobbyState
            _currentState = new LobbyState(this, _gameClient);
        }

        private async void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                _chatContext = new Context(_gameClient, _gameClient.Username, MessageTextBox.Text);
                await _commandParser.ParseAndExecute(_chatContext);
                MessageTextBox.Clear();
            }
        }

        private void OnChatMessageReceived(ChatMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                _chatMessages.Add(message);
                ChatListView.ScrollIntoView(ChatListView.Items[ChatListView.Items.Count - 1]);
            });
        }

        private void OnPrivateMessageReceived(ChatMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                // Display the message in InfoTextBlock
                InfoTextBlock.Text = message.Content;
                _infoMessageTimer.Stop(); // Reset the timer
                _infoMessageTimer.Start();
            });
        }

        private void OnInfoMessageReceived(string message)
        {
            Dispatcher.Invoke(() =>
            {
                // Display the message in InfoTextBlock
                InfoTextBlock.Text = message;
                _infoMessageTimer.Stop(); // Reset the timer
                _infoMessageTimer.Start();
            });
        }

        private void InfoMessageTimer_Tick(object sender, EventArgs e)
        {
            InfoTextBlock.Text = "";
            _infoMessageTimer.Stop();
        }

        private void OnMessageDeleted(string messageId)
        {
            Dispatcher.Invoke(() =>
            {
                var messageToDelete = _chatMessages.FirstOrDefault(msg => msg.MessageId == messageId);
                if (messageToDelete != null)
                {
                    _chatMessages.Remove(messageToDelete);
                }
                // Display info message
                InfoTextBlock.Text = "Message deleted";
                _infoMessageTimer.Stop();
                _infoMessageTimer.Start();
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
                // Transition to InGameState now that the game is started
                _currentState = new InGameState(this, _gameClient);
                StartGame();
            }
        }

        public void StartCountdown()
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

        private void TowerFirerateUpgradeMessageCheckbox_Copy_Checked(object sender, RoutedEventArgs e)
        {
            if (TowerFirerateUpgradeMessageCheckbox_Copy.IsChecked == true)
            {
                _gameClient.SubscribeFromTowerFirerateUpgradeMessagesAsync();
            }
            else
            {
                _gameClient.UnsubscribeFromTowerFirerateUpgradeMessagesAsync();
            }
        }

        private void TowerRangeUpgradeMessageCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (TowerRangeUpgradeMessageCheckbox.IsChecked == true)
            {
                _gameClient.SubscribeFromTowerRangeUpgradeMessagesAsync();
            }
            else
            {
                _gameClient.UnsubscribeFromTowerRangeUpgradeMessagesAsync();
            }
        }

        private void MapComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MapComboBox.SelectedItem != null)
            {
                SelectMapLabelError.Visibility = Visibility.Collapsed;
                MapValidationErrorLabel.Visibility = Visibility.Collapsed; // Hide the error message
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
                MapValidationErrorLabel.Content = message;
                MapValidationErrorLabel.Visibility = Visibility.Visible;

                // Reset ready status
                _isReady = false;
                ReadyButton.Content = "Ready";

                // Make map selection and ready button available again
                ReadyButton.IsEnabled = true;
                MapComboBox.IsEnabled = true;
            });
        }


        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var ChangePasswordWindow = new ChangePasswordWindow(_gameClient, _userController);
            ChangePasswordWindow.Show();

            this.Close();
        }

        private void OnAllPlayersReady()
        {
            // Delegate the handling to the current state
            _currentState.HandleAllPlayersReady();
        }

        public void ChangeState(IGameUIState newState)
        {
            System.Diagnostics.Debug.WriteLine($"[State Change] Transitioning to: {newState.GetType().Name}");
            _currentState = newState;
        }


    }
}
