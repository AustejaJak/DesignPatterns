using BloonsProject;
using System.Windows;
using BloonLibrary;

namespace BloonsGame
{
    public partial class MainWindow : Window
    {
        private PauseWindow _pauseWindow;
        private IProgramController _programController;
        private GameClient _gameclient;
        private string Username;

        public MainWindow(GameClient gameClient, string username)
        {
            InitializeComponent();

            _gameclient = gameClient;
            Username = username;

            MapComboBox.Items.Add("The Original");

            // foreach (var map in MapManager.GetAllMaps())
            //     MapComboBox.Items.Add(map.Name); // Adds the maps to the combobox on the WPF display from the map manager.
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectMapLabelError.Visibility = Visibility.Hidden;
            if (MapComboBox.SelectedItem == null)
            {
                SelectMapLabelError.Visibility = Visibility.Visible;
                return; // If a map hasn't been selected and the user attempts to hit "play", the program will tell the user to select a map.
            }
            var map = MapManager.GetMapByName(MapComboBox.SelectedItem.ToString()); // Gets the map name from the combobox and gets the object from its name.
            bool isJoined = await _gameclient.JoinGameAsync(Username);

            if (!isJoined)
            {
                MessageBox.Show("Not enough players to start the game. Please wait for more players to join.", "Game Start Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _programController = new SplashKitController(map, _gameclient); // Runs the program.
            OpenPauseScreen(); // Opens pause screen and hides it immediately, and closes the main window to decrease lag (can demonstrate if you'd like).
            _pauseWindow.Hide();
            Close();
            _programController.PauseEventHandler += OpenPauseScreen; // Event handlers for when the game is paused or the user loses.
            _programController.LoseEventHandler += OpenLossScreen;
            _programController.Start();
        }
    }
}