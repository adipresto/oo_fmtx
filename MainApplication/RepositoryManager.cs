using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace MainApplication
{
    public class RepositoryManager : IRepositoryManager
    {
        private ConcurrentDictionary<string, ItemString> items { get; set; }
        
        public RepositoryManager()
        {
            items = new ConcurrentDictionary<string, ItemString>();
        }

        public void Deregister(string itemName)
        {
            if (!items.TryRemove(itemName, out _))
            {
                throw new KeyNotFoundException($"{itemName} is not exist");
            }
        }

        public int GetType(string itemName)
        {
            if (items.TryGetValue(itemName, out var item))
            {
                return (int)item.ItemType;
            }
            throw new KeyNotFoundException($"{itemName} is not exist");
        }

        public void Register(string itemName, string itemContent, int itemType)
        {
            var validator = ItemValidatorFactory.GetValidator((ItemType)itemType);
            if (!validator.IsValid(itemContent))
                throw new InvalidDataException("Item content is invalid for the specified type");

            if (!items.TryAdd(itemName, new ItemString
            {
                ItemName = itemName,
                ItemContent = itemContent,
                ItemType = (ItemType)itemType
            }))
            {
                throw new InvalidOperationException("Item already exists");
            }
        }

        public string Retrieve(string itemName)
        {
            if (items.TryGetValue(itemName, out var item))
            {
                return item.ItemContent;
            }
            throw new KeyNotFoundException($"{itemName} is not exist");
        }

        private bool IsValidJson(string content)
        {
            try
            {
                JsonSerializer.Deserialize<object>(content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidXml(string content)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
