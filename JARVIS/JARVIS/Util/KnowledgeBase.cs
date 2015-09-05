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

        public string GetSome()
        {
            StringBuilder wolframResult = new StringBuilder();
            bool foundResult = false;

            if (!result.DataTypes.Contains("Word"))
            {
                foreach (var pod in this.result.Pods)
                {
                    if (!(pod.Title.Equals("Input interpretation") || pod.Title.Equals("Response")))
                    {
                        foreach (var subpod in pod.SubPods)
                        {
                            if (string.IsNullOrEmpty(subpod.Title) && !string.IsNullOrEmpty(subpod.PlainText))
                            {
                                System.Console.WriteLine("Wrong");
                                wolframResult.AppendLine();
                                wolframResult.AppendLine();
                                wolframResult.Append(pod.Title);
                                wolframResult.AppendLine();
                                wolframResult.Append(subpod.PlainText);

                                foundResult = true;
                            }
                        }
                    }
                    else { }
                }
            }

            if (!foundResult)
            {
                return "No result found";
            }

            return wolframResult.ToString();
        }

        //public string GetMost()
        //{
        //    StringBuilder wolframResult = new StringBuilder();
        //    bool foundResult = false;

        //    foreach (var pod in this.result.Pods)
        //    {
        //        foreach (var subpod in pod.SubPods)
        //        {
        //            if (string.IsNullOrEmpty(subpod.Title) /*&& !string.Equals() */ && !string.IsNullOrEmpty(subpod.PlainText))
        //            {
        //                wolframResult.AppendLine();
        //                wolframResult.AppendLine();

        //                wolframResult.Append(pod.Title);
        //                wolframResult.AppendLine();
        //                wolframResult.Append(subpod.PlainText);

        //                foundResult = true;
        //            }
        //        }
        //    }
        //}

        public string GetAll()
        {
            StringBuilder wolframResult = new StringBuilder();
            bool foundResult = false;

            foreach (var pod in this.result.Pods)
            {
                foreach (var subpod in pod.SubPods)
                {
                    if (string.IsNullOrEmpty(subpod.Title) && !string.IsNullOrEmpty(subpod.PlainText))
                    {
                        wolframResult.AppendLine();
                        wolframResult.AppendLine();

                        wolframResult.Append(pod.Title);
                        wolframResult.AppendLine();
                        wolframResult.Append(subpod.PlainText);

                        foundResult = true;
                    }
                }
            }

            if (!foundResult)
            {
                return "No result found";
            }

            return wolframResult.ToString();
        }

        public List<string> GetPods() {
            List<string> pods = new List<string>();
            
            foreach (var pod in result.Pods)
            {
                pods.Add(pod.Title);
            }

            return pods;
        }

        public Dictionary<string, string> GetSubpodsInPod(WAPod pod)
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

        public List<string> GetSources()
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
