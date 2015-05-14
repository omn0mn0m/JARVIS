using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WASpellCheck
    {
        public string Word { get; set; }
        public string Suggestion { get; set; }
        public string Text { get; set; }

        public WASpellCheck()
        {
            
        }

        public WASpellCheck(XmlNode node)
        {
            WALogger.Write("Parsing spell check", WALogLevel.Debug);
            Word = node.Attributes["word"].Value;
            Suggestion = node.Attributes["suggestion"].Value;
            Text = node.Attributes["text"].Value;
        }
    }
}
