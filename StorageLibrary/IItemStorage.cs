using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLibrary
{
    public interface IItemStorage
    {
        void Add(ItemString item);
        bool Remove(string name);
        ItemString Get(string name);
        bool Exists(string name);
    }
}
