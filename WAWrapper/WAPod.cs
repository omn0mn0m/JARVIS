using System;
using System.Collections.Generic;
using System.Xml;

namespace WAWrapper
{
    public class WAPod
    {
        public string Title { get; set; }
        public string Scanner { get; set; }
        public string ID { get; set; }
        public int Position { get; set; }
        public bool Error { get; set; }
        public int SubPodCount { get; set; }
        /// <summary>
        /// Use this if you want the quickest answer (Pods.Where(x => x.Primary))
        /// </summary>
        public bool Primary { get; set; }

        public List<WASubPod> SubPods { get; set; }
        public List<WAInfo> Infos { get; set; }
        public List<WAStateList> StateList { get; set; }
        public List<WASound> Sounds { get; set; } 
        /// <summary>
        /// Parses the pod from the XmlNode given
        /// </summary>
        /// <param name="node">The XmlNode from the WolframAlpha response</param>
        public WAPod(XmlNode node)
        {
            WALogger.Write("Parsing Pod", WALogLevel.Debug);
            Title = node.Attributes["title"].Value;
            Scanner = node.Attributes["scanner"].Value;
            ID = node.Attributes["id"].Value;
            Position = Int32.Parse(node.Attributes["position"].Value);
            Error = node.Attributes["error"].Value == "true";
            SubPodCount = Int32.Parse(node.Attributes["numsubpods"].Value);
            Primary = node.Attributes["primary"] != null;

            SubPods = new List<WASubPod>();
            Infos = new List<WAInfo>();
            StateList = new List<WAStateList>();
            Sounds = new List<WASound>();
            foreach (XmlNode subnode in node.ChildNodes)
            {
                switch (subnode.Name)
                {
                    case "subpod":
                        SubPods.Add(new WASubPod(subnode));
                        break;
                    case "states":
                        foreach (XmlNode stateList in subnode.ChildNodes)
                        {
                            switch (stateList.Name)
                            {
                                case "statelist":
                                    StateList.Add(new WAStateList(stateList));
                                    break;
                                case"state":
                                    StateList.Add(new WAStateList(){Count = 1,Delimiters = "", States = new List<WAState>(){new WAState(stateList)}});
                                    break;
                            }
                        }
                        break;
                    case "infos":
                        foreach (XmlNode info in subnode.ChildNodes)
                            Infos.Add(new WAInfo(info));
                        break;
                    case "sounds":
                        foreach (XmlNode sound in subnode.ChildNodes)
                            Sounds.Add(new WASound(sound));
                        break;
                }

            }
        }
    }
}
