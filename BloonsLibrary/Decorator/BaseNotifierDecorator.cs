using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Decorator
{
    public abstract class BaseNotifierDecorator : INotifier
    {
        private INotifier notifier;

        public BaseNotifierDecorator(INotifier notify) 
        {
            this.notifier = notify;
        }
        public void send(string message)
        {
            notifier.send(message);
        }
    }
}
