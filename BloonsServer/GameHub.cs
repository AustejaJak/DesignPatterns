using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class GameHub : Hub
{
    public async Task SendUsername(string username)
    {
        await Clients.All.SendAsync("SendUsername", username);
    }
}
