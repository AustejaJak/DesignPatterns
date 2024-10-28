using BloonsProject;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SplashKitSDK;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BloonLibrary
{
    public class PlayerStatus
    {
        public string Username { get; set; }
        public string ReadyStatus { get; set; }
    }

    public class GameClient
    {
        private HubConnection _connection;

        public event Action<List<PlayerStatus>> PlayerListUpdated;
        public event Action AllPlayersReady;

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

            _connection.On<UpgradeOrSellTowerRequest>("UpgradeOrSellTower", (request) =>
            {
                Point2D position = new Point2D() { X = request.Position.X, Y = request.Position.Y };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.upgradeOrSellTower(position, request.option, request.upgradeCount);
            });

            //_connection.On<SynchronizeBloon>("AddBloon", (request) =>
            //{
            //    var bloon = BloonFactory.CreateBloon(request.BloonType);
            //    bloon.Position = new Point2D()
            //    {
            //        X = request.Position.X,
            //        Y = request.Position.Y
            //    };
            //    var gameSession = GameSession.GetInstance();
            //    gameSession.GameState.AddBloon(bloon);
            //});
            

            _connection.On<SynchronizeBloon>("AddBloon", (request) =>
            {
                var bloon = BloonFactory.CreateBloonOfType(request.Name);
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.AddBloon(bloon);
            });
            
            _connection.On<string>("UserJoined", (username) =>
            {
                Console.WriteLine($"{username} has joined the game.");
            });

            _connection.On<List<PlayerStatus>>("UpdatePlayerList", (players) =>
            {
                PlayerListUpdated?.Invoke(players);
            });

            _connection.On("AllPlayersReady", () =>
            {
                AllPlayersReady?.Invoke();
            });
            
            _connection.On("GameStarted", () =>
            {
                Console.WriteLine("The game has started!");
                
                var gameState = GameState.GetGameStateInstance();

            });
            _connection.On<string>("SendGameOverStats", (message) =>
            {
                Console.WriteLine("Game is over");

                var gameState = GameState.GetGameStateInstance();
                gameState.AddGameStats(message);

            });


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

        public async Task SendUsernameAsync(string username)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                Username = username;
                await _connection.InvokeAsync("SendUsername", username);
            }
        }


        public async Task SendGameOverStats(string message)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendGameOverStats", message);
            }
        }

        public async Task SetPlayerReadyAsync(bool isReady)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SetPlayerReady", Username, isReady);

            }
        }

        public async Task PlaceTowerAsync(PlaceTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("PlaceTower", request);
            }
        }

        public async Task UpgradeOrSellTowerAsync(UpgradeOrSellTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UpgradeOrSellTower", request);
            }
        }

        //public async Task PlaceBloonAsync(PlaceBloonRequest request)
        //{
        //    if (_connection != null && _connection.State == HubConnectionState.Connected)
        //    {
        //        await _connection.InvokeAsync("PlaceBloon", request);
        //    }
        //}

        //public async Task<bool> JoinGameAsync(string username)
        //{
        //    if (_connection != null && _connection.State == HubConnectionState.Connected)
        //    {
        //        try
        //        {
        //            return await _connection.InvokeAsync<bool>("JoinGame", username);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error while joining game: {ex.Message}");
        //            return false;
        //        }
        //    }

        //    return false;
        //}



        public async Task PlaceBloonAsync(PlaceBloonRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("PlaceBloon", request);
            }
        }

        public async Task JoinGameAsync(string username)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("JoinGame", username);
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