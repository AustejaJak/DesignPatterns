using BloonsProject;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SplashKitSDK;

namespace BloonLibrary
{
    public class GameClient
    {
        private HubConnection _connection;
        private EntityDrawer _entityDrawer;

        public string Username { get; set; }

        public async Task ConnectToServer(string url)
        {
            _connection = new HubConnectionBuilder()
                            .WithUrl(url)
                            .Build();


            _connection.On<string>("SendUsername", (message) =>
            {
                Console.WriteLine($"Received username from the server: {message}");
            });

            _connection.On<SynchronizeTower>("AddTower", (request) =>
            {
                var tower = TowerFactory.CreateTowerOfType(request.TowerType, request.PlayerName);
                tower.Position = new Point2D()
                {
                    X = request.Position.X,
                    Y = request.Position.Y
                };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.AddTower(tower);
            });
            
            try
            {
                await _connection.StartAsync();
                await _connection.InvokeAsync("JoinGame");
                Console.WriteLine("Connected to SignalR server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }

        public async Task SendUsernameToServer(string username)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                Username = username;
                await _connection.InvokeAsync("SendUsername", username);
            }
        }

        public async Task PlaceTowerAsync(PlaceTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("PlaceTower", request);
            }
        }

        private void UpdateLocalGameState(GameState gameState)
        {
            var currentState = GameState.GetGameStateInstance();
            currentState.Bloons = gameState.Bloons;
            currentState.Towers = gameState.Towers;
            currentState.BloonsSpawned = gameState.BloonsSpawned;
            currentState.BloonsToBeSpawned = gameState.BloonsToBeSpawned;
            currentState.Player = gameState.Player;
            currentState.ProjectileManager = gameState.ProjectileManager;
        }

        public async Task Disconnect()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
                Console.WriteLine("Disconnected from server.");
            }
        }
    }
}
