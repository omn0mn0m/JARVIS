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
        string input;
        string[] inputArray;

        public Input()
        {

        }

        public void ReceiveInput(string input)
        {
            this.input = input;
            inputArray = this.input.ToLower().Split(' ');
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
    }
}
