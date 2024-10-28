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
    private readonly GameState _gameState = GameState.GetGameStateInstance();
    private static List<string> _connectedUsernames = new List<string>();
    private string _username;


    public async Task SendUsername(string username)
    {
        _connectedUsernames.Add(username);
        _username = username;

        await Clients.Group("inGame").SendAsync("SendUsername", username);
    }
    
    public async Task BroadcastBloonStates(BloonStateRequest request)
    {
        var gameSession = GameSession.GetInstance();

        // Get all bloon states that match the request's name
        var bloonStates = gameSession.GameState.GetAllBloonStates()
            .Where(b => b.Name == request.Name).ToList(); // Use Where to filter and convert to a list

 
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

                Console.WriteLine($"Broadcasting state for Bloon ID {updatedBloonState.Name}: Position ({updatedBloonState.Position.X}, {updatedBloonState.Position.Y})");
                await Clients.Group("inGame").SendAsync("UpdateBloonState", updatedBloonState); // Send the updated bloon state
            }
        }
        else
        {
            Console.WriteLine($"No bloon states found for ID {request.Name}.");
        }
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
    
    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_username != null)
        {
            _connectedUsernames.Remove(_username);
        }

        return base.OnDisconnectedAsync(exception);
    }
    
}
