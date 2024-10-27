using BloonLibrary;
using BloonLibrary.Decorator;
using BloonsProject;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace BloonsGame
{
    public partial class LoseWindow : Window
    {
        private GameClient gameClient;
        private DispatcherTimer updateTimer;

        public LoseWindow(GameClient client)
        {
            gameClient = client;
            InitializeComponent();

            var gameState = GameState.GetGameStateInstance();
            var plural = gameState.Player.Round == 1 ? " round" : " rounds";
            string message = $"{client.Username} survived {gameState.Player.Round}{plural}, and had {gameState.Player.Money} gold";

            //RoundSurvivedLabel.Content = $"You have survived {gameState.Player.Round}{plural}, and had {gameState.Player.Money} gold";
            //RoundSurvivedLabel.Visibility = Visibility.Visible;

            WaitForOtherPlayerMessages(gameState, message);

            // Set up the timer for live updates
            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Adjust interval as needed
            };
            updateTimer.Tick += (s, e) => UpdateStats(gameState);
            updateTimer.Start();
        }

        private async void WaitForOtherPlayerMessages(GameState gameState, string message)
        {
            await gameClient.SendGameOverStats(message);
            UpdateStats(gameState);
        }

        private void UpdateStats(GameState gameState)
        {
            if (gameState.OtherPlayerStats.Count > 0 && gameState.OtherPlayerStats[0] != null)
                Stats1.Content = gameState.OtherPlayerStats[0];
            else
                Stats1.Content = string.Empty;

            if (gameState.OtherPlayerStats.Count > 1 && gameState.OtherPlayerStats[1] != null)
                Stats2.Content = gameState.OtherPlayerStats[1];
            else
                Stats2.Content = string.Empty;

            if (gameState.OtherPlayerStats.Count > 2 && gameState.OtherPlayerStats[2] != null)
                Stats3.Content = gameState.OtherPlayerStats[2];
            else
                Stats3.Content = string.Empty;
            if (gameState.OtherPlayerStats.Count > 3 && gameState.OtherPlayerStats[3] != null)
                Stats4.Content = gameState.OtherPlayerStats[3];
            else
                Stats4.Content = string.Empty;
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void PlayAgainButton_OnClickButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().ProcessName, "");
            Close();
            Process.GetCurrentProcess().Kill();
        }
    }
}