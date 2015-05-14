using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAWrapper
{
    public class WAQuery
    {

        private const string MainRoot = "http://api.wolframalpha.com/v1/query.jsp?";

        /// <summary>
        /// Creates a new instance of a query
        /// </summary>
        public WAQuery()
        {
            Substitutions = new List<string>();
            Assumptions = new List<string>();
            PodTitles = new List<string>();
            PodStates = new List<string>();
            Scanners = new List<string>();
        }
        /// <summary>
        /// Creates a new instance of a query and assigns the appid
        /// </summary>
        /// <param name="appid">Your developer API key for this application</param>
        public WAQuery(string appid)
        {
            Substitutions = new List<string>();
            Assumptions = new List<string>();
            PodTitles = new List<string>();
            PodStates = new List<string>();
            Scanners = new List<string>();
            AppID = appid;
        }
        /// <summary>
        /// Creates a new instance of a query and assigns the appid and input text
        /// </summary>
        /// <param name="appid">Your developer API key for this application</param>
        /// <param name="input">Your input question / formula</param>
        public WAQuery(string appid, string input)
        {
            Substitutions = new List<string>();
            Assumptions = new List<string>();
            PodTitles = new List<string>();
            PodStates = new List<string>();
            Scanners = new List<string>();
            Input = input;
            AppID = appid;
        }
        
        /// <summary>
        /// Creates a new instance of a query and assigns values
        /// </summary>
        /// <param name="appid">Your developer API key for this application</param>
        /// <param name="input">Your input question / formula</param>
        /// <param name="subs">Substitutions</param>
        /// <param name="podTitles">The pod titles you want returns</param>
        /// <param name="assumptions">Assumptions you would like to add based on previous results</param>
        /// <param name="states">States you would like to add based on previous results</param>
        public WAQuery(string appid, string input, List<string> subs, List<string> podTitles, List<string> assumptions, List<string> states, List<string> scanners )
        {
            Substitutions = subs ?? new List<string>();
            Assumptions = assumptions ?? new List<string>();
            PodTitles = podTitles ?? new List<string>();
            PodStates = states ?? new List<string>();
            Scanners = scanners ?? new List<string>();
            Input = input;
            AppID = appid;
        }
        /// <summary>
        /// Creates a new instance of a query and assigns values
        /// </summary>
        /// <param name="appid">Your developer API key for this application</param>
        /// <param name="input">Your input question / formula</param>
        /// <param name="subs">Substitutions</param>
        /// <param name="podTitles">The pod titles you want returns</param>
        /// <param name="assumptions">Assumptions you would like to add based on previous results</param>
        /// <param name="states">States you would like to add based on previous results</param>
        /// <param name="format">What format you would like the output to return in.  Use WAQueryFormat for preset formats</param>
        /// <param name="timeLimit">Time in seconds to allow WolframAlpha to attempt to find the result</param>
        /// <param name="allowCaching">Allow WolframAlpha to use previous cached (aged) results</param>
        /// <param name="asynchronous">Dynamic page.  HIGHLY RECOMMEND YOU DO NOT USE WITH THIS LIBRARY</param>
        /// <param name="moreOutput">Moar Output!!</param>
        public WAQuery(string appid, string input, List<string> subs, List<string> podTitles, List<string> assumptions, List<string> states, List<string> scanners, string format = "xml", int timeLimit = 0, bool allowCaching = false, bool asynchronous = false, bool moreOutput = false)
        {
            Substitutions = subs ?? new List<string>();
            Assumptions = assumptions ?? new List<string>();
            PodTitles = podTitles ?? new List<string>();
            PodStates = states ?? new List<string>();
            Scanners = scanners ?? new List<string>();
            TimeLimit = timeLimit;
            Input = input;
            Format = format;
            AllowCaching = allowCaching;
            Asynchronous = asynchronous;
            MoreOutput = moreOutput;
            AppID = appid;
        }
        /// <summary>
        /// Your question / formula / query
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// The Format.  Use WAQueryFormat for preset formats.
        /// </summary>
        public string Format { get; set; }
        public List<string> Substitutions { get; set; }
        public List<string> PodTitles { get; set; }
        public List<string> Assumptions { get; set; }
        public List<string> PodStates { get; set; }
        public List<string> Scanners { get; set; } 
        /// <summary>
        /// Time limit in seconds to allow WolframAlpha to find the result
        /// </summary>
        public int TimeLimit { get; set; }
        /// <summary>
        /// Allow WolframAlpha to use previous cached (aged) results
        /// </summary>
        public bool AllowCaching { get; set; }
        /// <summary>
        /// Dynamic page.  HIGHLY RECOMMEND YOU DO NOT USE WITH THIS LIBRARY
        /// </summary>
        public bool Asynchronous { get; set; }
        /// <summary>
        /// Moar Output!!
        /// </summary>
        public bool MoreOutput { get; set; }
        /// <summary>
        /// Your developer API key for this application
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// Format and return the URL to send to WolframAlpha (includes base of the URL)
        /// </summary>
        /// <returns>Ready to use URL for WolframAlpha</returns>
        public string FormatQuery()
        {
            WALogger.Write("Formatting Query to send to WA...", WALogLevel.Debug);
            if (string.IsNullOrEmpty(AppID))
                throw new NullAppIDException("App ID is missing.");
            var result = string.Format("{0}appid={1}&input={2}", MainRoot, AppID, HttpUtility.UrlEncode(Input));
            if (Format != "xml" &! string.IsNullOrEmpty(Format))
                result += "&format=" + Format;
            if (Substitutions.Count > 0)
                result = Substitutions.Aggregate(result, (current, s) => current + ("&substitution=" + s));
            if (PodTitles.Count > 0)
                result = PodTitles.Aggregate(result, (current, t) => current + ("&podtitle=" + t));
            if (Assumptions.Count > 0)
                result = Assumptions.Aggregate(result, (current, a) => current + ("&assumption=" + a));
            if (PodStates.Count > 0)
                result = PodStates.Aggregate(result, (current, s) => current + ("&podstate=" + s));
            if (Scanners.Count > 0)
                result = Scanners.Aggregate(result, (current, s) => current + ("&scanner=" + s));
            result += string.Format("{0}{1}{2}{3}", (TimeLimit > 0) ? "&timelimit=" + TimeLimit : "",
                (AllowCaching ? "&allowedcached=1" : ""), (Asynchronous ? "&async=1" : ""),
                (MoreOutput ? "&moreoutput=1" : ""));
            WALogger.Write(result, WALogLevel.Verbose);
            return result;
        }
    }
}
