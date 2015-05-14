using System;
using System.IO;

namespace WAWrapper
{
    public class WALogger
    {
        /// <summary>
        /// LogFile Logging level. Default is Verbose
        /// </summary>
        public static WALogLevel LogLevel { get; set; }
        /// <summary>
        /// Console Output Level.  Default is Error
        /// </summary>
        public static WALogLevel ConsoleLogLevel { get; set; }

        public static void Write(string message, WALogLevel level)
        {
            if (level >= LogLevel)
                using (var sw = new StreamWriter("wa.log",true))
                    sw.WriteLine(message);
            if (level >= ConsoleLogLevel)
                Console.WriteLine(message);
        }
    }
}
