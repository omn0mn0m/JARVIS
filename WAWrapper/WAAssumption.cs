using System;
using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    /// <summary>
    /// An assumption made by wolfram alpha based on part or all of the query given
    /// </summary>
    public class WAAssumption
    {
        /// <summary>
        /// What type of assumption is being made
        /// </summary>
        public string AssumptionType { get; set; }
        /// <summary>
        /// What part of the query is being assumed
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// How many different options there are
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The different assumption options that can be used
        /// </summary>
        public List<WAAssumptionValue> Values { get; set; }

        public string Template { get; set; }
        /// <summary>
        /// Initializes a new Assumption
        /// </summary>
        public WAAssumption()
        {
            Values = new List<WAAssumptionValue>();
        }

        /// <summary>
        /// Parses the Assumption from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAAssumption(XmlNode node)
        {
            WALogger.Write("Parsing Assumption", WALogLevel.Debug);
            Values = new List<WAAssumptionValue>();
            AssumptionType = node.Attributes["type"].Value;
            Word = (node.Attributes["word"] == null) ? "" : node.Attributes["word"].Value;
            Count = Int32.Parse(node.Attributes["count"].Value);
            foreach (XmlNode val in node.ChildNodes)
                Values.Add(new WAAssumptionValue(val));
            Template = (node.Attributes["template"] == null) ? "" : node.Attributes["template"].Value;
            if (!string.IsNullOrEmpty(Template))
            {
                Template = Template.Replace("${word}", Word);
                if (Template.Contains("${desc1}"))
                    Template = Template.Replace("${desc1}", Values[0].Description);
                if (Template.Contains("${desc2}"))
                    Template = Template.Replace("${desc2}", Values[1].Description);
                if (Template.Contains("${desc3}"))
                    Template = Template.Replace("${desc3}", Values[2].Description);
                if (Template.Contains("${desc4}"))
                    Template = Template.Replace("${desc4}", Values[3].Description);
            }

        }
    }

    public class WAAssumptionValue
    {
        public string Name { get; set; }
        /// <summary>
        /// Description of what the assumption is
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// What needs to be added to the query to get the result of this assumption
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Parses the Assumption Value from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAAssumptionValue(XmlNode node)
        {
            WALogger.Write("Parsing Assumption Value", WALogLevel.Debug);
            Name = node.Attributes["name"].Value;
            Description = node.Attributes["desc"].Value;
            Input = node.Attributes["input"].Value;
        }
    }
}
