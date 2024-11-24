using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Iterator
{
    public interface IIterator<T>
    {
        T Current { get; } // Current item in the collection
        bool MoveNext();   // Advance to the next item
        void Reset();
    }
}
