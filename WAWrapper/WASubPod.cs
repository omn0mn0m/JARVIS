using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace WAWrapper
{
    public class WASubPod
    {
        /// <summary>
        /// The title of the subpod (usually empty)
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The text of the subpod / response / result
        /// </summary>
        public string PlainText { get; set; }
        /// <summary>
        /// An image, if one was given by WolframAlpha
        /// </summary>
        public WAImage Image { get; set; }

        /// <summary>
        /// Parses the Sub Pod from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WASubPod(XmlNode node)
        {
            WALogger.Write("Parsing subpod", WALogLevel.Debug);
            Title = node.Attributes["title"].Value;
            PlainText = node["plaintext"] != null ? node["plaintext"].InnerText : "";
            PlainText = UnicodeConversion.Convert(PlainText);
            if (node["img"] != null)
                Image = new WAImage(node["img"]);
        }

        
    }
}
