using System.Threading.Tasks;

namespace BloonsServer.Observer
{
    public interface ITowerEventListener
    {
        public Task SendMessage(string username);

        public string GetListenerId();
    }
}
