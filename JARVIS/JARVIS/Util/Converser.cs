using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatterBotAPI;                // https://code.google.com/p/chatter-bot-api/
using System.Threading;

namespace JARVIS.Util
{
    class Converser
    {
        // Declares a class to construct chatterbots
        private ChatterBotFactory FACTORY = new ChatterBotFactory();

        // Declares a chatterbot for JARVIS to use
        private ChatterBot chatterbot;

        // Declares a chat session for the chatterbot to use
        private ChatterBotSession chatSession;

        // Different chatterbots
        private const ChatterBotType CLEVERBOT = ChatterBotType.CLEVERBOT;
        private const ChatterBotType PANDORABOTS = ChatterBotType.PANDORABOTS;
        private const ChatterBotType JABBERWACKY = ChatterBotType.JABBERWACKY;

        public Converser()
        {
            // Initialises the chatterbots
            chatterbot = FACTORY.Create(CLEVERBOT);

            // Creates the chat sessions for each bot
            chatSession = chatterbot.CreateSession();
        }

        public String Respond(String input)
        {
            return chatSession.Think(input);
        }
    }
}
