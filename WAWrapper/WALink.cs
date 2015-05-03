using System.Xml;

namespace WAWrapper
{
    public class WALink
    {
        /// <summary>
        /// Link URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Text for the hyperlink
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Title of the link
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Parses the link from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WALink(XmlNode node)
        {
            WALogger.Write("Parsing Link", WALogLevel.Debug);
            URL = node.Attributes["url"].Value;
            Text = node.Attributes["text"].Value;
            if (node.Attributes["title"] != null)
                Title = node.Attributes["title"].Value;
        }
    }
}
