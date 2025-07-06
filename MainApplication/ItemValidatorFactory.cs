using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class ItemValidatorFactory
    {
        public static IItemValidator GetValidator(ItemType type)
        {
            return type switch
            {
                ItemType.JSON => new JsonValidator(),
                ItemType.XML => new XmlValidator(),
                _ => throw new NotSupportedException($"Unsupported ItemType: {type}")
            };
        }
    }
}
