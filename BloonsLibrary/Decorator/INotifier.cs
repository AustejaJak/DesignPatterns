using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Decorator
{
    public interface INotifier
    {
        void send(string message);
    }
}
