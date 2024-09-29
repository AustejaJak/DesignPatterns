using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class GameHub : Hub
{
    public async Task RegisterPlayer(string playerName)
    {
        await Clients.All.SendAsync("PlayerRegistered", playerName);
    }
    public async Task PlaceTower(string playerName, int towerId, int x, int y)
    {
        await Clients.All.SendAsync("TowerPlaced", playerName, towerId, x, y);
    }
    public async Task ShootEnemy(string playerName, int towerId, int enemyId)
    {
        await Clients.All.SendAsync("EnemyShot", playerName, towerId, enemyId);
    }
}
