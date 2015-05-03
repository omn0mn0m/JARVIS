using System;
using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    public class WAQueryResult
    {
        public WAQueryResult()
        {
            
        }
        /// <summary>
        /// Parses the result from the XmlDocument provided by WolframAlpha
        /// </summary>
        /// <param name="data"></param>
        public WAQueryResult(XmlDocument data)
        {
            
            WALogger.Write("Parsing XML Document", WALogLevel.Debug);
            var parent = data["queryresult"];
            var values = parent.Attributes;
            Success = (values["success"].Value == "true");
            IsError = (values["error"].Value == "true");
            PodCount = Int32.Parse(values["numpods"].Value);
            DataTypes = new List<string>();
            TimedOut = new List<string>();
            TimedOutPods = new List<string>();
            DataTypes.AddRange(values["datatypes"].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            TimedOut.AddRange(values["timedout"].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            TimedOutPods.AddRange(values["timedoutpods"].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            Timing = float.Parse(values["timing"].Value);
            ParseTiming = float.Parse(values["parsetiming"].Value);
            ParseTimedOut = values["parsetimedout"].Value == "true";
            Recalculate = values["recalculate"].Value;
            ID = values["id"].Value;
            Host = values["host"].Value;
            Server = Int32.Parse(values["server"].Value);
            Related = values["related"].Value;
            APIVersion = new Version(values["version"].Value);

            Pods = new List<WAPod>();
            Assumptions = new List<WAAssumption>();
            Sources = new List<WASource>();
            Warnings = new List<WAWarning>();
            Tips = new List<string>();
            DidYouMeans = new List<string>();
            RelatedExamples = new List<WARelatedExample>();
            //Now get the pods...
            foreach (XmlNode node in parent.ChildNodes)
            {
                switch (node.Name)
                {
                    case "pod":
                        Pods.Add(new WAPod(node));
                        break;
                    case "assumptions":
                        foreach (XmlNode assumption in node.ChildNodes)
                            Assumptions.Add(new WAAssumption(assumption));
                        break;
                    case "sources":
                        foreach (XmlNode source in node.ChildNodes)
                            Sources.Add(new WASource(source));
                        break;
                    case "warnings":
                        foreach (XmlNode warning in node.ChildNodes)
                            Warnings.Add(new WAWarning(warning));
                        break;
                    case "generalization":
                        Generalization = new WAGeneralization(node);
                        break;
                    case "error":
                        
                        Error = new WAError(node);
                        break;
                    case "tips":
                        foreach (XmlNode tip in node.ChildNodes)
                            Tips.Add(tip.Attributes["text"].Value);
                        break;
                    case "didyoumeans":
                        foreach (XmlNode means in node.ChildNodes)
                            DidYouMeans.Add(means.InnerText);
                        break;
                    case "languagemsg":
                        LanguageMsg = new WALanguageMsg(node);
                        break;
                    case "futuretopic":
                        FutureTopic = new WAFutureTopic(node);
                        break;
                    case "relatedexamples":
                        foreach (XmlNode related in node.ChildNodes)
                            RelatedExamples.Add(new WARelatedExample(related));
                        break;
                    case "examplepage":
                        ExamplePage = new WAExamplePage(node);
                        break;
                }
                
                
            }

        }

        

        /// <summary>
        /// Was WolframAlpha Successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Was there an error
        /// </summary>
        public bool IsError { get; set; }
        public WALanguageMsg LanguageMsg { get; set; }
        public WAError Error { get; set; }
        public WAExamplePage ExamplePage { get; set; }
        public List<WARelatedExample> RelatedExamples { get; set; } 
        public WAFutureTopic FutureTopic { get; set; }
        public WAGeneralization Generalization { get; set; }
        public int PodCount { get; set; }
        public List<string> DataTypes { get; set; }
        public List<string> TimedOut { get; set; }
        public List<string> TimedOutPods { get; set; }
        public List<string> Tips { get; set; }
        public List<string> DidYouMeans { get; set; } 
        public float Timing { get; set; }
        public float ParseTiming { get; set; }
        public bool ParseTimedOut { get; set; }
        public string Recalculate { get; set; }
        public string ID { get; set; }
        public string Host { get; set; }
        public int Server { get; set; }
        public string Related { get; set; }
        public Version APIVersion { get; set; }
        public List<WAPod> Pods { get; set; } 
        public List<WAAssumption> Assumptions { get; set; }
        public List<WASource> Sources { get; set; }
        public List<WAWarning> Warnings { get; set; } 

    }
}
