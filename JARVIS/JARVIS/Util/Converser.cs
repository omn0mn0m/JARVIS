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
        public static bool useCensor = true;

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

        private static string[] censoredWords = { "fuck", "shit", "nigger", "dick", "ass" };

        public Converser() : this(true)
        {
            
        }

        public Converser(bool useCensor)
        {
            // Initialises the chatterbots
            chatterbot = FACTORY.Create(CLEVERBOT);

            // Creates the chat sessions for each bot
            chatSession = chatterbot.CreateSession();
            chatSessionList = new List<ChatterBotSession>();
            chatSessionList.Add(chatSession);

            Converser.useCensor = useCensor;
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
            if (useCensor)
            {
                message = CensorInput(message);
            }

            if (useSpeech)
            {
                speech.Speak(message);                   // Reads the message as speech
            }
            return ("JARVIS: " + message);        // Writes the message to the output text field
        }

        public static String Say(String message, SpeechRecognitionEngine recognition)
        {
            if (useCensor)
            {
                message = CensorInput(message);
            }

            if (useSpeech)
            {
                if (frmJarvis.useRecognition) {
                    recognition.RecognizeAsyncCancel();
                }
                speech.Speak(message);                   // Reads the message as speech

                if (frmJarvis.useRecognition)
                {
                    try
                    {
                        recognition.RecognizeAsync(RecognizeMode.Multiple);
                    }
                    catch (InvalidOperationException) { }
                }
            }

            return ("JARVIS: " + message);        // Writes the message to the output text field
        }

        public static string CensorInput(string message)
        {
            string[] messageArray;
            string fixedMessage = message;

            bool censoredSomething = false;

            messageArray = message.Split(' ');

            for (int i = 0; i < messageArray.Length; i++)
            {
                for (int j = 0; j < censoredWords.Length; j++)
                {
                    if (messageArray[i].ToLower().Equals(censoredWords[j]))
                    {
                        messageArray[i] = "*";
                        censoredSomething = true;
                    }
                }
            }

            if (censoredSomething)
            {
                fixedMessage = string.Join(" ", messageArray);
            }

            return fixedMessage;
        }
    }
}
