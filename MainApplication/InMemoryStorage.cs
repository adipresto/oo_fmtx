using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class InMemoryStorage : IItemStorage
    {
        private readonly ConcurrentDictionary<string, ItemString> items = new();

        public void Add(ItemString item)
        {
            if (!items.TryAdd(item.ItemName, item))
                throw new InvalidOperationException("Item already exists");
        }

        public bool Remove(string name) => items.TryRemove(name, out _);

        public ItemString Get(string name)
        {
            items.TryGetValue(name, out var item);
            return item;
        }

        public bool Exists(string name) => items.ContainsKey(name);
    }
}
