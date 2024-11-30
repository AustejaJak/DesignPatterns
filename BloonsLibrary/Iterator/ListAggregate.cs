using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Iterator
{
    public class ListAggregate<T> : IAggregate<T>
    {
        private readonly List<T> _items = new();

        public void AddItem(T item) => _items.Add(item);

        public void RemoveItem(T item) => _items.Remove(item);

        public IIterator<T> CreateIterator() => new ListIterator<T>(_items);
    }
}
