using System.Xml;

namespace WAWrapper
{
    public class WASource
    {
        /// <summary>
        /// The URL of the source for the data
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// The Name of the source
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Parses the source from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WASource(XmlNode node)
        {
            WALogger.Write("Parsing source", WALogLevel.Debug);
            URL = node.Attributes["url"].Value;
            Text = node.Attributes["text"].Value;
        }
    }
}
