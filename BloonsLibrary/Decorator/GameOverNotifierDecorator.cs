using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Decorator
{
    public class GameOverNotifierDecorator : INotifier
    {
        GameClient GameClient;
        //private INotifier notifier;


        public GameOverNotifierDecorator(GameClient gameClient)
        {
            //notifier = notify;
            GameClient = gameClient;
        }
        public async void send(string message)
        {
            string GameOverMessage = GameClient.Username + " " + message;
            await GameClient.SendGameOverStats(GameOverMessage);
        }
    }
}
