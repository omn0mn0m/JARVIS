using System;
using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    public class WAStateList
    {
        /// <summary>
        /// The number of states in this statelist
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The value of the statelist
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Any delimiters
        /// </summary>
        public string Delimiters { get; set; }
        /// <summary>
        /// List of states in the statelist
        /// </summary>
        public List<WAState> States { get; set; }
        /// <summary>
        /// Parses the statelist from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAStateList(XmlNode node)
        {
            WALogger.Write("Parsing statelist", WALogLevel.Debug);
            Count = Int32.Parse(node.Attributes["count"].Value);
            Value = node.Attributes["value"].Value;
            Delimiters = node.Attributes["delimiters"].Value;
            States = new List<WAState>();
            foreach (XmlNode state in node.ChildNodes)
                States.Add(new WAState(state));
        }

        public WAStateList()
        {
            States = new List<WAState>();
        }
    }
}
