using System.Xml;

namespace WAWrapper
{
    public class WAState
    {
        /// <summary>
        /// The name of the state
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The string used in a query to get this resultant state
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Parses the State from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAState(XmlNode node)
        {
            WALogger.Write("Parsing state", WALogLevel.Debug);
            Name = node.Attributes["name"].Value;
            Input = node.Attributes["input"].Value;
        }
    }
}
