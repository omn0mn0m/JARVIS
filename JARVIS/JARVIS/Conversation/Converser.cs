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

        private static Input input = new Input();

        private KnowledgeBase knowledgeBase = new KnowledgeBase(frmJarvis.wolframAppID);
        private bool useKnowledgeBase = true;

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

            Input.useCensor = useCensor;
        }

        public String Respond(String input)
        {
            try
            {
                if (useKnowledgeBase && CheckKnowledgeBase())
                {
                    string wolframResult = knowledgeBase.GetResult();

                    if (wolframResult.Equals("No result found"))
                    {
                        return chatSession.Think(input);
                    }
                    else
                    {
                        return wolframResult;
                    }
                }
                else
                {
                    return chatSession.Think(input);
                }
            }
            catch (Exception)
            {
                return "Durrrr";
            }
        }

        public bool CheckKnowledgeBase()
        {
            string word;

            for (int i = 0; i < input.GetInputArrayLength(); i++)
            {
                word = input.GetWord(i);

                if (word.Equals("who") || word.Equals("what") || word.Equals("when") || word.Equals("where") || word.Equals("why") || word.Equals("how"))
                {
                    knowledgeBase.SendQuery(input.GetInputPastPoint(i));
                    return true;
                }
            }

            return false;
        }

        // Makes JARVIS say something with text to speech as well as prints it to the console
        public static String Say(String message)
        {
            input.ReceiveInput(message);

            if (useSpeech)
            {
                speech.Speak(input.GetInput());                   // Reads the message as speech
            }
            return ("JARVIS: " + input.GetInput());        // Writes the message to the output text field
        }

        public static String Say(String message, SpeechRecognitionEngine recognition)
        {
            input.ReceiveInput(message);

            if (useSpeech)
            {
                if (frmJarvis.useRecognition) {
                    recognition.RecognizeAsyncCancel();
                }
                speech.Speak(input.GetInput());                   // Reads the message as speech

                if (frmJarvis.useRecognition)
                {
                    try
                    {
                        recognition.RecognizeAsync(RecognizeMode.Multiple);
                    }
                    catch (InvalidOperationException) { }
                }
            }

            return ("JARVIS: " + input.GetInput());        // Writes the message to the output text field
        }
    }
}
