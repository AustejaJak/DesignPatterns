using BloonsProject;
using System.Diagnostics;
using System.Windows;

namespace BloonsGame
{
    public partial class PauseWindow : Window
    {
        private readonly IProgramController _programController;

        public PauseWindow(IProgramController _programController)
        {
            InitializeComponent();
            this._programController = _programController;
        }

        private void OnContinueButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _programController.SetIsGameRunningTo(false);
            Close(); // When the player presses continue, the game is set to resume.
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Process.Start(Process.GetCurrentProcess().ProcessName, "");
            Close();
            Process.GetCurrentProcess().Kill(); // If the user presses exit, the process is closed and restarted.
        }
    }
}