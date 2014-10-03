using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Collections;
using ChatterBotAPI;

namespace JARVIS.Util
{
    class XMPPInteractor
    {
        XmppClientConnection xmpp;
        Converser jarvis = new Converser();

        public XMPPInteractor(String service, String username, String password)
        {
            xmpp = new XmppClientConnection(service);
            Login(username, password);
            xmpp.OnError += new ErrorHandler(xmpp_OnError);
            xmpp.OnMessage += new MessageHandler(xmpp_OnMessage);
            xmpp.AutoResolveConnectServer = false;
            xmpp.Open();
        }

        public void Login(String username, String password)
        {
            xmpp.Username = username;
            xmpp.Password = password;
            xmpp.OnLogin += new ObjectHandler(OnLogin);
            xmpp.OnAuthError += new XmppElementHandler(xmpp_OnAuthError);
        }

        private void xmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            if (msg.Chatstate.ToString() == "composing")
                return;

            String a = msg.FirstChild.InnerXml;

            if (a == "")
            {
                return;
            }
            // from
            Jid to = msg.From;

            string s = jarvis.Respond(a);
            agsXMPP.protocol.client.Message nmsg = new agsXMPP.protocol.client.Message();
            nmsg.Type = agsXMPP.protocol.client.MessageType.chat;
            nmsg.To = to;
            nmsg.Body = s;
            xmpp.Send(nmsg);
        }

        private void xmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {

            throw new NotImplementedException();
        }

        private void xmpp_OnError(object sender, Exception ex)
        {
            throw new NotImplementedException();
        }

        private void OnLogin(object sender)
        {
            Presence p = new Presence(ShowType.chat, "Online");
            p.Type = PresenceType.available;
            xmpp.Send(p);
        }
    }
}
