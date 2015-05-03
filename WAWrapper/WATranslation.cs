using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WATranslation
    {
        public string Phrase { get; set; }
        public string Translation { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }

        public WATranslation(XmlNode node)
        {
            WALogger.Write("Parsing translation", WALogLevel.Debug);
            Phrase = node.Attributes["phrase"].Value;
            Translation = node.Attributes["trans"].Value;
            Language = node.Attributes["lang"].Value;
            Text = node.Attributes["text"].Value;

        }
        
    }
}
