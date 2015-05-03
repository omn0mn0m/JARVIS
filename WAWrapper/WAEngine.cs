using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace WAWrapper
{
    /// <summary>
    /// Main Wolfram Alpha Engine component.  Make sure you set the APIKey.
    /// </summary>
    public class WAEngine
    {
        /// <summary>
        /// Enter your APPID here.  Get your key from https://developer.wolframalpha.com/portal/myapps/
        /// </summary>
        public string APIKey { get; set; }

       
        /// <summary>
        /// Initialize a new instance of the Wolfram Alpha wrapper.
        /// </summary>
        public WAEngine()
        {
            WALogger.LogLevel = WALogLevel.Verbose;
            WALogger.ConsoleLogLevel = WALogLevel.Error;
        }

        /// <summary>
        /// Runs a basic query
        /// </summary>
        /// <param name="input">Query text</param>
        /// <returns>QueryResult</returns>
        public WAQueryResult RunQuery(string input)
        {
            
            if (String.IsNullOrEmpty(APIKey))
            {
                throw new NullAppIDException("You must specify your App ID in the APIKey field.");
            }

            var result = GetResult(new WAQuery(APIKey, input).FormatQuery());
            try
            {
                return new WAQueryResult(result);
            }
            catch (Exception e)
            {
                WALogger.Write(e.Message, WALogLevel.Error);
                return new WAQueryResult()
                {
                    Success = false
                };
            }

        }
        /// <summary>
        /// Runs a basic query
        /// </summary>
        /// <param name="query">A WAQuery object</param>
        /// <returns>QueryResult</returns>
        public WAQueryResult RunQuery(WAQuery query)
        {
            if (String.IsNullOrEmpty(query.AppID))
            {
                if (string.IsNullOrEmpty(APIKey))
                    throw new NullAppIDException("You must specify your App ID in the APIKey field.");
                else
                    query.AppID = APIKey;
            }
            var result = GetResult(query.FormatQuery());
            return new WAQueryResult(result);
        }

        /// <summary>
        /// Gets the XmlResult from WolframAlpha and returns as an XmlDocument
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private XmlDocument GetResult(string url)
        {
            var doc = new XmlDocument();
            var stream = XmlReader.Create(url);
            doc.Load(stream);
            return doc;
        }

        
    }
}
