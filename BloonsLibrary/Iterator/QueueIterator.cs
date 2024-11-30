using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Iterator
{
    public class QueueIterator<T> : IIterator<T>
    {
        private readonly Queue<T> _items;
        //private T[] _arrayRepresentation;
        //private int _position = -1;

        public T Current { get; private set; }

        public QueueIterator(Queue<T> items)
        {
            _items = items;
            //_arrayRepresentation = _items.ToArray(); // Convert to array for indexed access
        }

        //public T Current
        //{
        //    get
        //    {
        //        if (_position >= 0 && _position < _arrayRepresentation.Length)
        //        {
        //            return _arrayRepresentation[_position];
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException();
        //        }
        //    }
        //}

        public bool MoveNext()
        {
            T result;
            if (_items.TryDequeue(out result))
            {
                this.Current = result;
                return true;
            }
            Current = default;
            return false;
        }

        public void Reset()
        {
            //_position = -1;
        }
    }

}
