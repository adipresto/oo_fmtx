using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MainApplication
{
    public class XmlValidator : IItemValidator
    {
        public bool IsValid(string content)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(content);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
