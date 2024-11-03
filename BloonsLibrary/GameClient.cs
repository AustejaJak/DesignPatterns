using BloonsProject;
//using BloonLibrary.Models;
//using BloonLibrary.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using SplashKitSDK;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Collections.Concurrent;
using BloonLibrary.Controllers.Bridge;

namespace BloonLibrary
{
    public class PlayerStatus
    {
        public string Username { get; set; }
        public string ReadyStatus { get; set; }
        public string SelectedMap { get; set; }
    }

    public class GameClient
    {
        private HubConnection _connection;

        public event Action<List<PlayerStatus>> PlayerListUpdated;
        public event Action<ChatMessage> ChatMessageReceived;
        public event Action AllPlayersReady;

        public string Username { get; set; }
        
        private EntityDrawer _entityDrawer;
        private ConcurrentDictionary<string, Bloon> _bloons; // Use a ConcurrentDictionary
        private readonly object _lockObject = new object();
        public event Action<string> MapValidationFailed;
        private readonly TowerFactory _towerFactory = new TowerFactory();
        private readonly BloonFactory _bloonFactory = new BloonFactory();


        public GameClient()
        {
            _bloons = new ConcurrentDictionary<string, Bloon>(); // Initialize the dictionary
        }

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
                var tower = _towerFactory.CreateTowerOfType(request.TowerType, request.PlayerName);
                tower.Position = new Point2D()
                {
                    X = request.Position.X,
                    Y = request.Position.Y
                };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.AddTower(tower);
                if (tower.Username == Username)
                {
                    MyTowerControl towercontrol = new MyTowerControl(tower, this);
                    gameSession.GameState.TowerControlls.Add(towercontrol);
                }
                else
                {
                    OtherPlayerTowerControl towercontrol = new OtherPlayerTowerControl(tower, this);
                    gameSession.GameState.TowerControlls.Add(towercontrol);
                }
            });

            _connection.On<UpgradeOrSellTowerRequest>("UpgradeTowerRange", (request) =>
            {
                Point2D position = new Point2D() { X = request.Position.X, Y = request.Position.Y };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.upgradeOrSellTower(position, request.option, request.upgradeCount);
            });

            _connection.On<UpgradeOrSellTowerRequest>("UpgradeTowerFireRate", (request) =>
            {
                Point2D position = new Point2D() { X = request.Position.X, Y = request.Position.Y };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.upgradeOrSellTower(position, request.option, request.upgradeCount);
            });

            _connection.On<UpgradeOrSellTowerRequest>("SellTower", (request) =>
            {
                Point2D position = new Point2D() { X = request.Position.X, Y = request.Position.Y };
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.upgradeOrSellTower(position, request.option, request.upgradeCount);
            });

            _connection.On<string>("MapValidationFailed", (message) =>
            {
                MapValidationFailed?.Invoke(message);
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
                var bloon = _bloonFactory.CreateBloonOfType(request.Name);
                var gameSession = GameSession.GetInstance();
                gameSession.GameState.AddBloon(bloon);
            });
            

            _connection.On<BloonState>("UpdateBloonState", (request) =>
            {
                var gameSession = GameSession.GetInstance();

                lock (_lockObject)
                {
                    // Ensure the collection is not null
                    if (gameSession.GameState.Bloons == null)
                    {
                        Console.WriteLine("Bloons collection is null.");
                        return;
                    }

                    // Use TryGetValue to safely retrieve the bloon
                    if (gameSession.GameState.Bloons.TryGetValue(request.Name, out var bloon))
                    {
                        // Update the properties of the bloon with the values from the request
                        bloon.Position = new Point2D()
                        {
                            X = request.Position.X,
                            Y = request.Position.Y
                        };
                        bloon.Health = request.Health;
                        bloon.Checkpoint = request.Checkpoint;
                        bloon.DistanceTravelled = request.DistanceTravelled;
                    }
                }
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

            _connection.On<ChatMessage>("ReceiveChatMessage", (message) =>
            {
                ChatMessageReceived?.Invoke(message);
            });

            _connection.On<string>("RangeUpgradeMessage", (message) =>
            {
                var gameState = GameState.GetGameStateInstance();
                gameState.TowerEventMessages.Enqueue(message);
            });

            _connection.On<string>("FireRateUpgradeMessage", (message) =>
            {
                var gameState = GameState.GetGameStateInstance();
                gameState.TowerEventMessages.Enqueue(message);
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

        public async Task UpgradeTowerRangeAsync(UpgradeOrSellTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UpgradeTowerRange", request, Username);
            }
        }

        public async Task UpgradeTowerFireRateAsync(UpgradeOrSellTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UpgradeTowerFireRate", request, Username);
            }
        }

        public async Task UnsubscribeFromTowerRangeUpgradeMessagesAsync()
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UnsubscribeFromTowerRangeUpgradeMessages");
            }
        }

        public async Task SubscribeFromTowerRangeUpgradeMessagesAsync()
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SubscribeFromTowerRangeUpgradeMessages");
            }
        }

        public async Task UnsubscribeFromTowerFirerateUpgradeMessagesAsync()
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UnsubscribeFromTowerFirerateUpgradeMessages");
            }
        }

        public async Task SubscribeFromTowerFirerateUpgradeMessagesAsync()
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SubscribeFromTowerFirerateUpgradeMessages");
            }
        }

        public async Task SellTowerAsync(UpgradeOrSellTowerRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SellTower", request);
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
        public async Task BroadcastBloonStatesAsync(BloonStateRequest request)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("BroadcastBloonStates", request);
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

        public async Task SendChatMessageAsync(string message)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendChatMessage", Username, message);
            }
        }

        public async Task SendSelectedMapAsync(string mapName)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendSelectedMap", Username, mapName);
            }
        }
    }
}