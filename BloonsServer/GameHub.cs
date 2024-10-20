using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BloonLibrary;
using BloonsProject;
using SplashKitSDK;


public class GameHub : Hub
{
    private static List<string> _connectedUsernames = new List<string>();
    private string _username;

    public async Task SendUsername(string username)
    {
        _connectedUsernames.Add(username);
        _username = username;

        await Clients.All.SendAsync("SendUsername", username);
    }

    public async Task PlaceTower(PlaceTowerRequest request)
    {
        var towerInstance = TowerFactory.CreateTowerOfType(request.TowerType, _username);
        towerInstance.Position = new Point2D()
        {
            X = request.Position.X,
            Y = request.Position.Y
        };
        var gameSession = GameSession.GetInstance();
        gameSession.GameState.AddTower(towerInstance);

        var response = new SynchronizeTower(request.TowerType, NetworkPoint2D.Serialize(towerInstance.Position), _username);
        await Clients.Group("inGame").SendAsync("AddTower", response);
    }

    public async Task JoinGame()
    {
        var gameSession = GameSession.GetInstance();
        gameSession.AddPlayer(_username);

        await Groups.AddToGroupAsync(Context.ConnectionId, "inGame");
        await Clients.Group("inGame").SendAsync("UserJoined", _username);
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
