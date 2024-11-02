using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BloonsServer.Observer
{
    public class FireRateUpgradeListener : ITowerEventListener
    {
        private readonly string _clientId;
        private readonly IHubCallerClients _clients;

        public FireRateUpgradeListener(string clientId, IHubCallerClients clients)
        {
            _clientId = clientId;
            _clients = clients;
        }

        public async Task SendMessage(string username)
        {
            await _clients.Client(_clientId).SendAsync("FireRateUpgradeMessage", username + " has upgraded tower firerate");
        }

        public string GetListenerId()
        {
            return _clientId;
        }
    }
}
