using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connectedUsernames.Remove(_username);
        return base.OnDisconnectedAsync(exception);
    }
}
