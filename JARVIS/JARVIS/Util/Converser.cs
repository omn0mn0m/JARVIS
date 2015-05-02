using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using ChatterBotAPI;                // https://code.google.com/p/chatter-bot-api/
using System.Threading;
using System.Speech.Recognition;

namespace JARVIS.Util
{
    class Converser
    {
        private static SpeechSynthesizer speech = new SpeechSynthesizer();         // Text to Speech
        public static bool useSpeech = true;

        // Declares a class to construct chatterbots
        private ChatterBotFactory FACTORY = new ChatterBotFactory();

        // Declares a chatterbot for JARVIS to use
        private ChatterBot chatterbot;

        // Declares a chat session for the chatterbot to use
        private ChatterBotSession chatSession;
        private List<ChatterBotSession> chatSessionList;

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
            chatSessionList = new List<ChatterBotSession>();
            chatSessionList.Add(chatSession);
        }

        public String Respond(String input)
        {
            try
            {
                return chatSession.Think(input);
            }
            catch (Exception)
            {
                return "Durrrr";
            }
        }

        // Makes JARVIS say something with text to speech as well as prints it to the console
        public static String Say(String message)
        {
            if (useSpeech)
            {
                speech.SpeakAsync(message);                   // Reads the message as speech
            }
            return ("JARVIS: " + message);        // Writes the message to the output text field
        }

        public static String Say(String message, SpeechRecognitionEngine recognition)
        {
            if (useSpeech)
            {
                recognition.RecognizeAsyncCancel();
                speech.Speak(message);                   // Reads the message as speech

                try
                {
                    recognition.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (InvalidOperationException) { }
            }

            return ("JARVIS: " + message);        // Writes the message to the output text field
        }
    }
}
