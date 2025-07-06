using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace StorageLibrary
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IItemStorage storage;

        public RepositoryManager(IItemStorage storage)
        {
            this.storage = storage;
        }

        public void Register(string itemName, string itemContent, int itemTypeInt)
        {
            if (!Enum.IsDefined(typeof(ItemType), itemTypeInt))
                throw new NotSupportedException("Invalid item type");

            var itemType = (ItemType)itemTypeInt;

            if (storage.Exists(itemName))
                throw new InvalidOperationException("Item already exists");

            var validator = ItemValidatorFactory.GetValidator(itemType);
            if (!validator.IsValid(itemContent))
                throw new InvalidDataException("Item content is invalid for the specified type");

            storage.Add(new ItemString
            {
                ItemName = itemName,
                ItemContent = itemContent,
                ItemType = itemType
            });
        }

        public string Retrieve(string itemName)
        {
            var item = storage.Get(itemName);
            if (item == null)
                throw new KeyNotFoundException($"{itemName} does not exist");

            return item.ItemContent;
        }

        public void Deregister(string itemName)
        {
            if (!storage.Remove(itemName))
                throw new KeyNotFoundException($"{itemName} does not exist");
        }

        public int GetType(string itemName)
        {
            var item = storage.Get(itemName);
            if (item == null)
                throw new KeyNotFoundException($"{itemName} does not exist");

            return (int)item.ItemType;
        }

    }
}
