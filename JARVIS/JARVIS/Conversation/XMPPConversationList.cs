using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using JARVIS.Util;

namespace JARVIS.Conversation
{
    class XMPPConversationList
    {
        private List<XMPPConversation> conversationList = new List<XMPPConversation>();
        private bool defaultUseJarvisTag;

        public XMPPConversationList(bool defaultUseJarvisTag)
        {
            this.defaultUseJarvisTag = defaultUseJarvisTag;
        }

        public string RespondToMessage(Jid to, string input)
        {
            for (int i = 0; i < conversationList.Count; i++)
            {
                if (conversationList[i].GetTo().Equals(to))
                {
                    System.Console.WriteLine("Found conversation");
                    return conversationList[i].GetResponse(input);
                }
            }

            System.Console.WriteLine("Created new conversation");
            conversationList.Add(new XMPPConversation(to, defaultUseJarvisTag));
            return conversationList[conversationList.Count - 1].GetResponse(input);
        }

        private class XMPPConversation
        {
            private Jid to;
            private Converser conversation = new Converser();

            private bool useJarvisTag;

            public XMPPConversation(Jid to, bool useJarvisTag)
            {
                this.to = to;
                this.useJarvisTag = useJarvisTag;
            }

            public Jid GetTo()
            {
                return to;
            }

            public Converser GetConversation()
            {
                return conversation;
            }

            public string GetResponse(string input)
            {
                if (useJarvisTag)
                {
                    return "JARVIS: " + conversation.Respond(input);
                }
                else
                {
                    return conversation.Respond(input);
                }
            }
        }
    }
}
