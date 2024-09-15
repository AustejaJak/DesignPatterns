using BloonsProject;
using System.Windows;

namespace BloonsGame
{
    public partial class MainWindow : Window
    {
        private PauseWindow _pauseWindow;
        private IProgramController _programController;

        public MainWindow()
        {
            InitializeComponent();
            foreach (var map in MapManager.GetAllMaps())
                MapComboBox.Items.Add(map.Name); // Adds the maps to the combobox on the WPF display from the map manager.
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectMapLabelError.Visibility = Visibility.Hidden;
            if (MapComboBox.SelectedItem == null)
            {
                SelectMapLabelError.Visibility = Visibility.Visible;
                return; // If a map hasn't been selected and the user attempts to hit "play", the program will tell the user to select a map.
            }
            var map = MapManager.GetMapByName(MapComboBox.SelectedItem.ToString()); // Gets the map name from the combobox and gets the object from its name.
            _programController = new SplashKitController(map); // Runs the program.
            OpenPauseScreen(); // Opens pause screen and hides it immediately, and closes the main window to decrease lag (can demonstrate if you'd like).
            _pauseWindow.Hide();
            Close();
            _programController.PauseEventHandler += OpenPauseScreen; // Event handlers for when the game is paused or the user loses.
            _programController.LoseEventHandler += OpenLossScreen;
            _programController.Start();
        }
    }
}