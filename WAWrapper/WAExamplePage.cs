using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WAExamplePage
    {
        public string Category { get; set; }
        public string URL { get; set; }

        public WAExamplePage(XmlNode node)
        {
            WALogger.Write("Parsing Example Page", WALogLevel.Debug);
            Category = node.Attributes["category"].Value;
            URL = node.Attributes["url"].Value;
        }
    }
}
