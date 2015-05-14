using System;
using System.Xml;

namespace WAWrapper
{
    public class WAImage
    {
        /// <summary>
        /// The URL to the image
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Alternate Text for when the image can't be displayed
        /// </summary>
        public string Alt { get; set; }
        /// <summary>
        /// The title of the image
        /// </summary>
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Parses the Image from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAImage(XmlNode node)
        {
            WALogger.Write("Parsing Image", WALogLevel.Debug);
            URL = node.Attributes["src"].Value;
            Alt = node.Attributes["alt"] == null ? "" : node.Attributes["alt"].Value;
            Title = node.Attributes["title"] == null ? "" : node.Attributes["title"].Value;
            Width = node.Attributes["width"] == null ? 0 : Int32.Parse(node.Attributes["width"].Value);
            Height = node.Attributes["height"] == null ? 0 : Int32.Parse(node.Attributes["height"].Value);

        }
    }
}
