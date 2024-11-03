using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BloonsServer.Observer
{
    public class RangeUpgradeListener : ITowerEventListener
    {
        private readonly string _clientId;
        private readonly IHubCallerClients _clients;

        public RangeUpgradeListener(string clientId, IHubCallerClients clients)
        {
            _clientId = clientId;
            _clients = clients;
        }

        public async Task SendMessage(string username)
        {
            await _clients.Client(_clientId).SendAsync("RangeUpgradeMessage", username + " has upgraded tower range");
        }

        public string GetListenerId()
        {
            return _clientId;
        }
    }
}
