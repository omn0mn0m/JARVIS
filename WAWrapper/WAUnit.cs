using System.Xml;

namespace WAWrapper
{
    public class WAUnit
    {
        /// <summary>
        /// The short form of the unit
        /// </summary>
        public string Short { get; set; }
        /// <summary>
        /// The long form of the unit
        /// </summary>
        public string Long { get; set; }
        /// <summary>
        /// Parses the unit from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAUnit(XmlNode node)
        {
            WALogger.Write("Parsing unit", WALogLevel.Debug);
            Short = node.Attributes["short"].Value;
            Long = node.Attributes["long"].Value;
        }
    }
}
