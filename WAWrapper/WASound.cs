using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    public class WASound
    {
        public string URL { get; set; }
        public string SoundType { get; set; }

        public WASound(XmlNode node)
        {
            WALogger.Write("Parsing sound", WALogLevel.Debug);
            URL = node.Attributes["url"].Value;
            SoundType = node.Attributes["type"].Value;
        }
    }
}
