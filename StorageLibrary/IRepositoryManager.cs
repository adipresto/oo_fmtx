using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLibrary
{
    public interface IRepositoryManager
    {
        /// <summary>
        /// Store an item to the repository
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemContent"></param>
        /// <param name="itemType">1. JSON, 2. XML</param>
        void Register(string itemName, string itemContent, int itemType);

        /// <summary>
        /// Retrieve an item from the repository
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        string Retrieve(string itemName);

        /// <summary>
        /// Retrieve the type (JSON or XML)
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        int GetType(string itemName);

        /// <summary>
        /// Remove an item from the repository
        /// </summary>
        /// <param name="itemName"></param>
        void Deregister(string itemName);   

    }
}
