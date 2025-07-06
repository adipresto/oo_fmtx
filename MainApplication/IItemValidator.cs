using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public interface IItemValidator
    {
        bool IsValid(string content);
    }
}
