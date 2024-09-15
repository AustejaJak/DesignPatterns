using BloonsProject;
using System;
using System.Diagnostics;
using System.Windows;

namespace BloonsGame
{
    public partial class LoseWindow : Window
    {
        public LoseWindow()
        {
            InitializeComponent();
            var gameState = GameState.GetGameStateInstance();
            var plural = " rounds.";
            if (gameState.Player.Round == 1)
            {
                plural = " round.";
            }
            RoundSurvivedLabel.Content = "You have survived " + gameState.Player.Round + plural; // Displays the rounds that the player survived in the loss screen.
            RoundSurvivedLabel.Visibility = Visibility.Visible;
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0); // Close the game when the user press
        }

        private void PlayAgainButton_OnClickButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().ProcessName, "");
            Close();
            Process.GetCurrentProcess().Kill(); // If the user presses play again, the process is closed and restarted.
        }
    }
}