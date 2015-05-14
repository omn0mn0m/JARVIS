using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WAReinterpret
    {
        public string Text { get; set; }
        public string New { get; set; }
        public List<string> Alternatives { get; set; }

        public WAReinterpret(XmlNode node)
        {
            WALogger.Write("Parsing a reinterpret", WALogLevel.Debug);
            Text = node.Attributes["text"].Value;
            New = node.Attributes["new"].Value;
            Alternatives = new List<string>();
            if (!node.HasChildNodes) return;
            foreach (XmlNode cNode in node.ChildNodes)
                Alternatives.Add(cNode.InnerText);
        }
    }
}
