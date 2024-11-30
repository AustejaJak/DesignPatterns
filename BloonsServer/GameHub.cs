using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BloonLibrary;
using BloonsProject;
using SplashKitSDK;
using System.Linq;
using BloonsServer.Observer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;

public class GameHub : Hub
{
    private readonly GameState _gameState = GameState.GetGameStateInstance();
    private static List<string> _connectedUsernames = new List<string>();
    private static readonly Dictionary<string, string> _userConnectionMap = new Dictionary<string, string>();

    private static Dictionary<string, string> _playerMaps = new Dictionary<string, string>();

    private static Dictionary<string, bool> _playerReadyStatus = new Dictionary<string, bool>();
    private string _username;
    private static readonly NotificationService _notificationService = new NotificationService();
    private readonly StandardBloonTowerFactory _bloonTowerFactory = new StandardBloonTowerFactory();
    private readonly ExtremeBloonTowerFactory _extremeBloonTowerFactory = new ExtremeBloonTowerFactory();

    public async Task SendUsername(string username)
    {
        _connectedUsernames.Add(username);
        _username = username;
        _userConnectionMap[username] = Context.ConnectionId; // Add to the map
        _playerReadyStatus[username] = false;

        ITowerEventListener listener1 = new RangeUpgradeListener(Context.ConnectionId, Clients);
        ITowerEventListener listener2 = new FireRateUpgradeListener(Context.ConnectionId, Clients);
        _notificationService.Subscribe(TowerEvent.Range, listener1);
        _notificationService.Subscribe(TowerEvent.FireRate, listener2);

        await Clients.Group("inGame").SendAsync("SendUsername", username);
        await UpdatePlayerStatuses();
    }

    public async Task SetPlayerReady(string username, bool isReady)
    {
        // First validate that the player has selected a map
        if (!_playerMaps.ContainsKey(username))
        {
            await Clients.Caller.SendAsync("MapValidationFailed", "Please select a map before getting ready.");
            return;
        }

        // Then validate that all players have the same map
        var distinctMaps = _playerMaps.Values.Distinct().ToList();
        if (distinctMaps.Count > 1)
        {
            await Clients.Caller.SendAsync("MapValidationFailed", "All players must select the same map before getting ready.");
            return;
        }

        _playerReadyStatus[username] = isReady;
        await UpdatePlayerStatuses();
        
        // Check if all players are ready
        if (_playerReadyStatus.Count >= 2 && _playerReadyStatus.All(p => p.Value))
        {
            await Clients.Group("inGame").SendAsync("AllPlayersReady");
        }
    }

    private async Task UpdatePlayerStatuses()
    {
        var playerStatuses = _connectedUsernames.Select(username => new PlayerStatus
        {
            Username = username,
            ReadyStatus = _playerReadyStatus.ContainsKey(username) && _playerReadyStatus[username] 
                ? "Ready" 
                : "Not Ready",
            SelectedMap = _playerMaps.ContainsKey(username) ? _playerMaps[username] : "No Map"
        }).ToList();

        await Clients.Group("inGame").SendAsync("UpdatePlayerList", playerStatuses);
    }

    public async Task SendGameOverStats(string message)
    {
        await Clients.Group("inGame").SendAsync("SendGameOverStats", message);
    }
    
    public async Task BroadcastBloonStates(BloonStateRequest request)
    {
        var gameSession = GameSession.GetInstance();

        var bloonStates = gameSession.GameState.GetAllBloonStates()
            .Where(b => b.Name == request.Name).ToList();

        if (bloonStates.Count > 0)
        {
            foreach (var bloonState in bloonStates) 
            {
                var networkPosition = new NetworkPoint2D(request.Position.X, request.Position.Y);

                var updatedBloonState = new BloonState(
                    bloonState.Name,
                    request.Health,
                    networkPosition,
                    request.Checkpoint,
                    request.DistanceTravelled
                );
                await Clients.Group("inGame").SendAsync("UpdateBloonState", updatedBloonState);
            }
        }
    }
    public async Task PlaceTower(PlaceTowerRequest request)
    {
        var towerInstance = _bloonTowerFactory.CreateTowerOfType(request.TowerType, request.Username);
        towerInstance.Position = new Point2D()
        {
            X = request.Position.X,
            Y = request.Position.Y
        };
        var gameSession = GameSession.GetInstance();
        gameSession.GameState.AddTower(towerInstance);

        var response = new SynchronizeTower(request.TowerType, NetworkPoint2D.Serialize(towerInstance.Position), request.Username);
        await Clients.Group("inGame").SendAsync("AddTower", response);
    }

    public async Task UpgradeTowerFireRate(UpgradeOrSellTowerRequest request, string senderUsername)
    {
        await _notificationService.Notify(TowerEvent.FireRate, senderUsername);
        await Clients.Group("inGame").SendAsync("UpgradeTowerFireRate", request);
    }

    public async Task UpgradeTowerRange(UpgradeOrSellTowerRequest request, string senderUsername)
    {
        await _notificationService.Notify(TowerEvent.Range, senderUsername);
        await Clients.Group("inGame").SendAsync("UpgradeTowerRange", request);
    }

    public async Task SellTower(UpgradeOrSellTowerRequest request)
    {
        await Clients.Group("inGame").SendAsync("SellTower", request);
    }

    public async Task UnsubscribeFromTowerRangeUpgradeMessages()
    {
        _notificationService.Unsubscribe(TowerEvent.Range, Context.ConnectionId);
    }

    public async Task SubscribeFromTowerRangeUpgradeMessages()
    {
        ITowerEventListener listener1 = new RangeUpgradeListener(Context.ConnectionId, Clients);
        _notificationService.Subscribe(TowerEvent.Range, listener1);
    }

    public async Task UnsubscribeFromTowerFirerateUpgradeMessages()
    {
        _notificationService.Unsubscribe(TowerEvent.FireRate, Context.ConnectionId);
    }

    public async Task SubscribeFromTowerFirerateUpgradeMessages()
    {
        ITowerEventListener listener2 = new FireRateUpgradeListener(Context.ConnectionId, Clients);
        _notificationService.Subscribe(TowerEvent.FireRate, listener2);
    }

    public async Task PlaceBloon(PlaceBloonRequest request)
    {
        var bloonInstance = _bloonTowerFactory.CreateBloonOfType(request.Name);
        
        var gameSession = GameSession.GetInstance();

        if (gameSession.GameState.Player.Round > 5)
        {
            bloonInstance = _extremeBloonTowerFactory.CreateBloonOfType(request.Name);
        }
        
        gameSession.GameState.AddBloon(bloonInstance);
        
        var response = new SynchronizeBloon(request.Health, request.Name, request.Color, request.VelocityX, request.VelocityY);
        await Clients.Group("inGame").SendAsync("AddBloon", response);
    }
    
    public async Task JoinGame(string username)
    {
        var gameSession = GameSession.GetInstance();
        gameSession.AddPlayer(username);

        await Groups.AddToGroupAsync(Context.ConnectionId, "inGame");
        await Clients.Group("inGame").SendAsync("UserJoined", username);
    }

    public async Task StartGame()
    {
        var gameSession = GameSession.GetInstance();
        var players = gameSession.GetPlayers();
        
        foreach (var player in players)
        {
            var playerGroupName = $"PlayerGroup{player}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, playerGroupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "inGame");
        }
        
        await Clients.Group("inGame").SendAsync("GameStarted");
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (_username != null)
        {
            _connectedUsernames.Remove(_username);
            _playerReadyStatus.Remove(_username);
            _userConnectionMap.Remove(_username); // Remove from the map
            await UpdatePlayerStatuses();
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string GetConnectionIdByUsername(string username)
    {
        if (_userConnectionMap.TryGetValue(username, out var connectionId))
        {
            return connectionId;
        }

        return null; // Return null if the username is not found
    }

    public async Task SendSelectedMap(string username, string mapName)
    {
        _playerMaps[username] = mapName;
        await ValidateMapSelection();
    }
    
    private async Task ValidateMapSelection()
    {
        // Only validate when we have at least 2 players with map selections
        if (_playerMaps.Count >= 2)
        {
            var distinctMaps = _playerMaps.Values.Distinct().ToList();
            if (distinctMaps.Count > 1)
            {
                // Maps don't match - notify all clients
                var message = "All players must select the same map before starting the game.";
                await Clients.Group("inGame").SendAsync("MapValidationFailed", message);

                // Reset ready status for all players
                foreach (var username in _playerReadyStatus.Keys.ToList())
                {
                    _playerReadyStatus[username] = false;
                }
                await UpdatePlayerStatuses();
            }
        }
    }

    public async Task SendChatMessage(string username, string message, string messageId)
    {
        var chatMessage = new ChatMessage
        {
            Username = username,
            Content = message,
            Timestamp = DateTime.Now.ToString("HH:mm"),
            MessageId = messageId
        };

        await Clients.Group("inGame").SendAsync("ReceiveChatMessage", chatMessage);
    }

    public async Task DeleteMessage(string username, string messageId)
    {
        // Notify all clients to remove the message with the specified ID
        await Clients.Group("inGame").SendAsync("MessageDeleted", messageId);
    }

    public async Task SendPrivateMessage(string senderUsername, string targetUsername, string message)
    {
        var senderConnectionId = GetConnectionIdByUsername(senderUsername);
        var targetConnectionId = GetConnectionIdByUsername(targetUsername);

        var chatMessage = new ChatMessage
        {
            Username = senderUsername,
            Content = $"[PM] {message}",
            Timestamp = DateTime.Now.ToString("HH:mm")
        };

        if (!string.IsNullOrEmpty(targetConnectionId))
        {
            // Send the private message to both sender and recipient
            await Clients.Client(senderConnectionId).SendAsync("ReceiveChatMessage", chatMessage);
            await Clients.Client(targetConnectionId).SendAsync("ReceiveChatMessage", chatMessage);
        }
        else
        {
            // Notify sender that the target user was not found
            await SendInfoMessage(senderUsername, $"User {targetUsername} not found.");
        }
    }

    public async Task SendInfoMessage(string username, string message)
    {
        var connectionId = GetConnectionIdByUsername(username);

        await Clients.Client(connectionId).SendAsync("ReceiveInfoMessage", message);
    }
}
