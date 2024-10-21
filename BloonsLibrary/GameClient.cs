using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< Updated upstream
=======
using SplashKitSDK;
using BloonLibrary;
>>>>>>> Stashed changes

namespace BloonLibrary
{
    public class GameClient
    {
        private HubConnection _connection;
<<<<<<< Updated upstream

        public string Username{get; set;}
=======

        // Add the Username property to keep track of the player's username
        public string Username { get; set; }

        private EntityDrawer _entityDrawer;

        public event Action<string> OnPlayerJoined;
        public event Action<string, string> OnPlayerReady;
        public event Action<string> OnStartGame;
>>>>>>> Stashed changes

        public async Task ConnectToServer(string url)
        {
            _connection = new HubConnectionBuilder()
                            .WithUrl(url)
                            .Build();

            _connection.On<string>("SendUsername", (message) =>
            {
                Console.WriteLine($"Received username from the server: {message}");
            });

            _connection.On<string>("SendTowerLocation", (message) =>
            {
<<<<<<< Updated upstream
                Console.WriteLine($"Received tower location from the server: {message}");
            });
=======
                var tower = TowerFactory.CreateTowerOfType(request.TowerType, request.PlayerName);
                tower.Position = new Point2D()
                {
                    X = request.Position.X,
                    Y = request.Position.Y
                };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.AddTower(tower);

            });
            
            _connection.On<string, string>("UserJoined", (username, playersList) =>
            {
                Console.WriteLine($"{username} has joined the game.");
                OnPlayerJoined?.Invoke(playersList); // Update the list of players on the client
            });

            _connection.On<string, string>("PlayerReady", (username, map) =>
            {
                OnPlayerReady?.Invoke(username, map);
            });

            _connection.On<string>("StartGame", (map) =>
            {
                OnStartGame?.Invoke(map);
            });
>>>>>>> Stashed changes

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Connected to SignalR server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }

<<<<<<< Updated upstream
        public async Task SendUsernameToServer(string username)
=======
        // Method to send the username to the server
        public async Task SendUsernameAsync(string username)
>>>>>>> Stashed changes
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                Username = username;
                await _connection.InvokeAsync("SendUsername", username); // SignalR hub method on server
            }
        }

<<<<<<< Updated upstream
        public async Task SendTowerLocationToServer(string location)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendTowerLocation", location);
=======
        // Method to join the game session
        public async Task JoinGameAsync(string username)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("JoinGame", username); // SignalR hub method on server
            }
        }

        // Add the PlaceTowerAsync method
        public async Task PlaceTowerAsync(PlaceTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("PlaceTower", request);
            }
        }

        public async Task SendReadyStatus(string map)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SetPlayerReady", Username, map);
>>>>>>> Stashed changes
            }
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
