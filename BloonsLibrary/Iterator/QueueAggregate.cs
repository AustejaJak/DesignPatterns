using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Iterator
{
    public class QueueAggregate<T> : IAggregate<T>
    {
        private readonly Queue<T> _items = new();

        public void EnqueueItem(T item) => _items.Enqueue(item);

        public bool TryDequeueItem(out T result) => _items.TryDequeue(out result);


        public IIterator<T> CreateIterator() => new QueueIterator<T>(_items);
    }
}
