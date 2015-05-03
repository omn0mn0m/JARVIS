using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    public class WAUnits
    {
        /// <summary>
        /// Image of the units / scale
        /// </summary>
        public WAImage Image { get; set; }
        /// <summary>
        /// List of units that correspond with any image / map provided in the pod
        /// </summary>
        public List<WAUnit> Units { get; set; }
        /// <summary>
        /// Parses the Units from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAUnits(XmlNode node)
        {
            WALogger.Write("Parsing units", WALogLevel.Debug);
            Units = new List<WAUnit>();
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "unit":
                        Units.Add(new WAUnit(child));
                        break;
                    case "img":
                        Image = new WAImage(child);
                        break;
                }
            }
        }
    }
}
