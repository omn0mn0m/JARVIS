using System.Xml;

namespace WAWrapper
{
    public class WAWarning
    {
        public WASpellCheck SpellCheck { get; set; }
        public string Delimiters { get; set; }
        public WATranslation Translation { get; set; }

        public WAReinterpret Reinterpret { get; set; }

        public WAWarning(XmlNode node)
        {
            WALogger.Write("Parsing warning", WALogLevel.Debug);
            switch (node.Name)
            {
                case "spellcheck":
                    SpellCheck = new WASpellCheck(node);
                    break;
                case "delimiters":
                    Delimiters = node.Attributes["text"].Value;
                    break;
                case "translation":
                    Translation = new WATranslation(node);
                    break;
                case "reinterpret":
                    Reinterpret = new WAReinterpret(node);
                    break;

            }
        }
    }
}
