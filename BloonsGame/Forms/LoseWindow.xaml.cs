using BloonLibrary;
using BloonLibrary.Decorator;
using BloonsProject;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace BloonsGame
{
    public partial class LoseWindow : Window
    {
        GameClient gameClient;
        public LoseWindow(GameClient client)
        {
            gameClient = client;
            InitializeComponent();
            var gameState = GameState.GetGameStateInstance();
            var plural = " rounds.";
            if (gameState.Player.Round == 1)
            {
                plural = " round.";
            }
            string message = "You have survived " + gameState.Player.Round + plural + " ,you had " + gameState.Player.Money +  " money";
            //BaseNotifierDecorator baseNotifierDecorator = new GameOverNotifierDecorator(client);
            
            RoundSurvivedLabel.Content = message; // Displays the rounds that the player survived in the loss screen.
            RoundSurvivedLabel.Visibility = Visibility.Visible;
            client.SendGameOverStats(message);
            OtherP_ayerStatsLabel.Content = gameState.OtherPlayerStats.ToString();
            WaitForOtherPLayerMessgaes(gameState, message);
        }

        private async void WaitForOtherPLayerMessgaes(GameState gameState, string message)
        {
            await gameClient.SendGameOverStats(message);
            OtherP_ayerStatsLabel.Content = gameState.OtherPlayerStats.FirstOrDefault();
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