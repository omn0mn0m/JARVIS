using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAWrapper;

namespace JARVIS.Util
{
    class KnowledgeBase
    {
        private string wolframAppID;

        private WAEngine engine;
        private WAQueryResult result;

        public KnowledgeBase(string wolframAppID)
        {
            this.wolframAppID = wolframAppID;

            engine = new WAEngine { APIKey = wolframAppID };
            WALogger.LogLevel = WALogLevel.None;
            WALogger.ConsoleLogLevel = WALogLevel.Verbose;
        }

        public void SendQuery(string question)
        {
            WAQuery query = new WAQuery() { Input = question, Format = WAQueryFormat.PlainText };
            query.PodStates.Add("test");
            query.AppID = engine.APIKey;
            string url = query.FormatQuery();
            result = engine.RunQuery(query);
        }

        public List<string> getPods() {
            List<string> pods = new List<string>();
            
            foreach (var pod in result.Pods)
            {
                pods.Add(pod.Title);
            }

            return pods;
        }

        public Dictionary<string, string> getSubpodsInPod(WAPod pod)
        {
            Dictionary<string, string> subpods = new Dictionary<string, string>();

            foreach (var subpod in pod.SubPods)
            {
                if (!string.IsNullOrEmpty(subpod.Title))
                {
                    subpods.Add("Title", subpod.Title);
                }
                if (subpod.Image != null)
                {
                    subpods.Add("Image", subpod.Image.URL);
                }
                if (!string.IsNullOrEmpty(subpod.PlainText))
                {
                    subpods.Add("Plaintext", subpod.PlainText);
                }
            }

            return subpods;
        }

        public List<string> getSources()
        {
            List<string> sources = new List<string>();

            if (result.Sources.Count > 0)
            {
                foreach (var s in result.Sources)
                {
                    sources.Add(s.Text + ": " + s.URL);
                }
            }

            return sources;
        }
    }
}
