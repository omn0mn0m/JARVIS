using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WARelatedExample
    {
        public string Input { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CategoryThumb { get; set; }
        public string CategoryPage { get; set; }

        public WARelatedExample(XmlNode node)
        {
            WALogger.Write("Parsing related example", WALogLevel.Debug);
            Input = node.Attributes["input"].Value;
            Description = node.Attributes["desc"].Value;
            Category = node.Attributes["category"].Value;
            CategoryThumb = node.Attributes["categorythumb"].Value;
            CategoryPage = node.Attributes["categorypage"].Value;

        }
    }
}
