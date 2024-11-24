using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Iterator
{
    public class ListIterator<T> : IIterator<T>
    {
        private readonly List<T> _items;
        private int _position = -1; // Start before the first element

        public ListIterator(List<T> items)
        {
            _items = items;
        }

        public T Current
        {
            get
            {
                if (_position >= 0 && _position < _items.Count)
                {
                    return _items[_position];
                }
                throw new InvalidOperationException();
            }
        }

        public bool MoveNext()
        {
            if (_position < _items.Count - 1)
            {
                _position++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _position = -1;
        }
    }

}
