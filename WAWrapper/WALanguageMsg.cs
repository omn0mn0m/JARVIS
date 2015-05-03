using System.Xml;

namespace WAWrapper
{
    public class WALanguageMsg
    {
        public string English { get; set; }
        public string Other { get; set; }

        public WALanguageMsg(XmlNode node)
        {
            WALogger.Write("Parsing Language Message", WALogLevel.Debug);
            English = node.Attributes["english"].Value;
            Other = node.Attributes["other"].Value;
        }
    }
}
