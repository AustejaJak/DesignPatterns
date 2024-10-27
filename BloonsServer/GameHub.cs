using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using BloonLibrary;
using BloonsProject;
using System.Timers;
using SplashKitSDK;
using Timer = System.Timers.Timer;


public class GameHub : Hub
{
    private static List<string> _connectedUsernames = new List<string>();
    private static List<string> _readyPlayers = new List<string>();
    private const int RequiredPlayers = 2;
    private string _username;
    

    public async Task SendUsername(string username)
    {
        _connectedUsernames.Add(username);
        _username = username;

        await Clients.Group("inGame").SendAsync("SendUsername", username);
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
        var bloonFactory = new BloonFactory();
        var bloonInstance = bloonFactory.CreateBloon(request.BloonType);

        var gameSession = GameSession.GetInstance();
        gameSession.GameState.AddBloon(bloonInstance);

        var response = new SynchronizeBloon(request.BloonType, NetworkPoint2D.Serialize(bloonInstance.Position));
        await Clients.Group("inGame").SendAsync("AddBloon", response);
    }
    
    public async Task<bool> JoinGame(string username)
    {
        var gameSession = GameSession.GetInstance();
        gameSession.AddPlayer(username);
        
        var playerGroupName = $"PlayerGroup{username}";
        await Groups.AddToGroupAsync(Context.ConnectionId, playerGroupName);
        
        await Clients.Caller.SendAsync("JoinedGroup", playerGroupName);
        
        await Clients.Group(playerGroupName).SendAsync("UserJoined", username);
    
        var gameState = gameSession.GetCurrentGameState();
        await Clients.Caller.SendAsync("ReceiveGameState", gameState);
        
        _readyPlayers.Add(username);
        
        if (gameSession.AllPlayersReady(RequiredPlayers))
        {
            await StartGame();
            return true;
        }

        return false;
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

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_username != null)
        {
            _connectedUsernames.Remove(_username);
        }

        return base.OnDisconnectedAsync(exception);
    }
}
