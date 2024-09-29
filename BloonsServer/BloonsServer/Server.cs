using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace BloonsServer
{
    public class Server : PersistentConnection
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast(data);
        }
    }
}
