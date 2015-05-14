using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WAGeneralization
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }

        public WAGeneralization(XmlNode node)
        {
            WALogger.Write("Adding Generalization", WALogLevel.Debug);
            Topic = node.Attributes["topic"].Value;
            Description = node.Attributes["desc"].Value;
            URL = node.Attributes["url"].Value;
        }
        
    }
}
