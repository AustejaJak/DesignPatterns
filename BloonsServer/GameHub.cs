using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BloonLibrary;
using BloonsProject;
using SplashKitSDK;
using System.Linq;

public class GameHub : Hub
{
    private static List<string> _connectedUsernames = new List<string>();
    private static Dictionary<string, bool> _playerReadyStatus = new Dictionary<string, bool>();
    private string _username;

    public async Task SendUsername(string username)
    {
        _connectedUsernames.Add(username);
        _username = username;
        _playerReadyStatus[username] = false;

        await Clients.Group("inGame").SendAsync("SendUsername", username);
        await UpdatePlayerStatuses();
    }

    public async Task SetPlayerReady(string username, bool isReady)
    {
        _playerReadyStatus[username] = isReady;
        await UpdatePlayerStatuses();
        
        // Check if all players are ready
        if (_playerReadyStatus.Count >= 2 && _playerReadyStatus.All(p => p.Value))
        {
            // Start countdown on all clients
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
                : "Not Ready"
        }).ToList();

        await Clients.Group("inGame").SendAsync("UpdatePlayerList", playerStatuses);
    }

    public async Task PlaceTower(PlaceTowerRequest request)
    {
        var towerInstance = TowerFactory.CreateTowerOfType(request.TowerType, request.Username);
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
    
    public async Task PlaceBloon(PlaceBloonRequest request)
    {
        var bloonInstance = BloonFactory.CreateBloonOfType(request.Name);

        var gameSession = GameSession.GetInstance();
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
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (_username != null)
        {
            _connectedUsernames.Remove(_username);
            _playerReadyStatus.Remove(_username);
            await UpdatePlayerStatuses();
        }

        await base.OnDisconnectedAsync(exception);
    }
}