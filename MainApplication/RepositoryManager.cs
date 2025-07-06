using System;
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
        private List<ItemString> items { get; set; }
        public RepositoryManager()
        {
            items = new List<ItemString>();
        }

        public void Deregister(string itemName)
        {
            var item = items.Where(item => item.ItemName == itemName).FirstOrDefault();
            if (item != null)
            {
                items.Remove(item);
            }
            else
            {
                throw new KeyNotFoundException($"{itemName} is not exist");
            }
        }

        public int GetType(string itemName)
        {
            var item = items.Where(item => item.ItemName == itemName).FirstOrDefault();
            if (item != null)
            {
                return (int)item.ItemType;
            }
            else
            {
                throw new KeyNotFoundException($"{itemName} is not exist");
            }
        }

        public void Register(string itemName, string itemContent, int itemType)
        {
            if (!items.Any(item => item.ItemName == itemName)) 
            {
                switch (itemType)
                {
                    case 1:
                        if (!IsValidJson(itemContent))
                        {
                            throw new InvalidDataException("Item content is invalid for the specified type");
                        }
                        break;
                    case 2:
                        if (!IsValidXml(itemContent))
                        {
                            throw new InvalidDataException("Item content is invalid for the specified type");
                        }
                        break;
                    default:
                        throw new InvalidDataException("Item content is invalid for the specified type");
                }
                items.Add(new ItemString()
                {
                    ItemName = itemName,
                    ItemContent = itemContent,
                    ItemType = (ItemType)itemType,
                });
            } else
            {
                throw new InvalidOperationException("Item already exists");
            }
        }

        public string Retrieve(string itemName)
        {
            var item = items.Where(item => item.ItemName == itemName).FirstOrDefault();
            if (item != null)
            {
                return item.ItemContent;
            }
            else
            {
                throw new KeyNotFoundException($"{itemName} is not exist");
            }
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
