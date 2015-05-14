using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WAFutureTopic
    {
        public string Topic { get; set; }
        public string Message { get; set; }

        public WAFutureTopic(XmlNode node)
        {
            WALogger.Write("Parsing Future Topic", WALogLevel.Debug);
            Topic = node.Attributes["topic"].Value;
            Message = node.Attributes["msg"].Value;
        }
    }
}
