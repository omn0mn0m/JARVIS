using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp;

namespace JARVIS.Util
{
    class Input
    {
        private static string input;
        private static string[] inputArray;

        public static bool useCensor = true;
        private static string[] censoredWords = { "fuck", "shit", "nigger", "dick", "ass" };

        public Input()
        {

        }

        public void ReceiveInput(string input)
        {
            Input.input = input;
            inputArray = Input.input.ToLower().Split(' ');

            if (useCensor)
            {
                CensorInput();
            }
        }

        public string GetInput()
        {
            return input;
        }

        public string GetInputPastPoint(int point)
        {
            string result = "";

            for (int i = point; i < inputArray.Length; i++)
            {
                result = result + " " + inputArray[i];
            }

            return result;
        }

        public int GetInputArrayLength()
        {
            return inputArray.Length;
        }

        public string GetWord(int index)
        {
            return inputArray[index];
        }

        public static void CensorInput()
        {
            bool censoredSomething = false;

            for (int i = 0; i < inputArray.Length; i++)
            {
                for (int j = 0; j < censoredWords.Length; j++)
                {
                    if (inputArray[i].ToLower().Equals(censoredWords[j]))
                    {
                        inputArray[i] = "*";
                        censoredSomething = true;
                    }
                }
            }

            if (censoredSomething)
            {
                input = string.Join(" ", inputArray);
            }
        }

        public static string CensorInput(string rawMessage)
        {
            string[] messageArray = rawMessage.Split(' ');

            bool censoredSomething = false;

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
                return string.Join(" ", messageArray);
            }
            else
            {
                return rawMessage;
            }
        }
    }
}
