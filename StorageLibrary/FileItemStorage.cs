using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StorageLibrary
{
    public class FileItemStorage : IItemStorage
    {
        private readonly string filePath;
        private readonly object fileLock = new();

        public FileItemStorage(string path)
        {
            filePath = path;
            if (!File.Exists(filePath) || string.IsNullOrWhiteSpace(File.ReadAllText(filePath)))
            {
                File.WriteAllText(filePath, "[]");
            }
        }

        private List<ItemString> LoadAll()
        {
            lock (fileLock)
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<ItemString>>(json) ?? new List<ItemString>();
            }
        }

        private void SaveAll(List<ItemString> items)
        {
            lock (fileLock)
            {
                var json = JsonSerializer.Serialize(items);
                File.WriteAllText(filePath, json);
            }
        }

        public void Add(ItemString item)
        {
            var items = LoadAll();
            if (items.Any(i => i.ItemName == item.ItemName))
                throw new InvalidOperationException("Item already exists");

            items.Add(item);
            SaveAll(items);
        }

        public bool Remove(string name)
        {
            var items = LoadAll();
            var removed = items.RemoveAll(i => i.ItemName == name) > 0;
            if (removed) SaveAll(items);
            return removed;
        }

        public ItemString Get(string name)
        {
            var items = LoadAll();
            return items.FirstOrDefault(i => i.ItemName == name);
        }

        public bool Exists(string name)
        {
            var items = LoadAll();
            return items.Any(i => i.ItemName == name);
        }
    }

}
