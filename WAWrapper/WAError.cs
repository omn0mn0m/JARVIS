using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WAError
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public WAError(XmlNode node)
        {
            
            Code = node["code"].InnerText;
            Message = node["message"].InnerText;
            WALogger.Write("Parsed Error! " + Message, WALogLevel.Error);
        }
    }
}
