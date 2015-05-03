using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    public class WAInfo
    {
        /// <summary>
        /// What the info is about
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// An image, what else?
        /// </summary>
        public WAImage Image { get; set; }
        /// <summary>
        /// List of links for the information
        /// </summary>
        public List<WALink> Links { get; set; }
        /// <summary>
        /// Some results return units used (for example, size of a land mass, maps)
        /// </summary>
        public WAUnits Units { get; set; } 
        /// <summary>
        /// Parses the info from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAInfo(XmlNode node)
        {
            WALogger.Write("Parsing Infos", WALogLevel.Debug);
            Text = (node.Attributes["text"] != null) ? node.Attributes["text"].Value : "";
            Links = new List<WALink>();
            foreach (XmlNode subnode in node.ChildNodes)
            {
                switch (subnode.Name)
                {
                    case "link":
                        Links.Add(new WALink(subnode));
                        break;
                    case "img":
                        Image = new WAImage(node["img"]);
                        break;
                    case "units":
                        Units = new WAUnits(subnode);
                        break;

                }
                if (subnode.Name == "link")
                    Links.Add(new WALink(subnode));
            }

        }
    }
}
