using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLibrary
{
    public enum ItemType
    {
        JSON = 1,
        XML = 2
    }
    public class ItemString
    {
        public string ItemName { get; set; }
        public string ItemContent { get; set; }
        public ItemType ItemType { get; set; }
    }
}
