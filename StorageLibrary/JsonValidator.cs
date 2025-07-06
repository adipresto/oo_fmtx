using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StorageLibrary
{
    public class JsonValidator : IItemValidator
    {
        public bool IsValid(string content)
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
    }
}
