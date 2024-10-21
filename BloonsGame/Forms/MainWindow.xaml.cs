using BloonsProject;
using System.Windows;
using BloonLibrary;
using System.Threading.Tasks;

namespace BloonsGame
{
    public partial class MainWindow : Window
    {
        private GameClient _gameClient;

        public MainWindow(GameClient gameClient)
        {
            InitializeComponent();
            _gameClient = gameClient;

            MapComboBox.Items.Add("The Original");

            // Listen for player join updates from the server
            _gameClient.OnPlayerJoined += UpdatePlayersList;
            _gameClient.OnPlayerReady += PlayerIsReady;
            _gameClient.OnStartGame += StartGame;
        }

        private void UpdatePlayersList(string playersList)
        {
            Dispatcher.Invoke(() =>
            {
                PlayersListLabel.Content = "Players: " + playersList;
            });
        }


        private void PlayerIsReady(string username, string map)
        {
            Dispatcher.Invoke(() =>
            {
                //MessageBox.Show($"{username} is ready with map: {map}");
            });
        }

        private async void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            SelectMapLabelError.Visibility = Visibility.Hidden;
            if (MapComboBox.SelectedItem == null)
            {
                SelectMapLabelError.Visibility = Visibility.Visible;
                return;
            }

            // Send the ready status to the server
            var selectedMap = MapComboBox.SelectedItem.ToString();
            await _gameClient.SendReadyStatus(selectedMap);
        }

        private void StartGame(string map)
        {
            Dispatcher.Invoke(() =>
            {
                //MessageBox.Show("All players are ready. Starting the game on map: " + map);
                // Start the game logic here
                var mapObj = MapManager.GetMapByName(map);
                var controller = new SplashKitController(mapObj, _gameClient);
                controller.Start();
                Close();
            });
        }
    }
}
