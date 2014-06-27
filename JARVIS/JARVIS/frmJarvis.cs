using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JARVIS
{
    public partial class frmJarvis : Form
    {
        // Speech recognition engine w/ US English as the langauge
        private SpeechRecognitionEngine recognition = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        // Text to Speech
        private SpeechSynthesizer speech = new SpeechSynthesizer();

        public frmJarvis()
        {
            // Loads the form components
            InitializeComponent();
        }

        // Writes a message to the text output field with a date/time stamp and the message 
        public void writeToOutput(String output)
        {
            // Adds the new output message to a new line in the output text field
            txtOutput.AppendText(System.Environment.NewLine + "[" + System.DateTime.Now + "] " + output);
        }

        // Makes JARVIS say something with text to speech as well as prints it to the console
        public void say(String message)
        {
            // Reads the message as speech
            speech.Speak(message);
            // Writes the message to the output text field
            writeToOutput("JARVIS: " + message);
        }

        // Event handler for when speech is recognised
        public void recognition_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            // Writes the recognised text to the input text field and outputs it to the output text field
            txtInput.Text = e.Result.Text;
            writeToOutput("USER: " + txtInput.Text);
            txtInput.Clear();
        }

        // Runs when the form is loaded
        private void frmJarvis_Load(object sender, EventArgs e)
        {
            // Creates a grammar to understand most English sentences
            DictationGrammar grammar = new DictationGrammar();
            grammar.Name = "Default Grammar";
            grammar.Enabled = true;

            // Loads the grammar to the speech recognition engine
            recognition.LoadGrammarAsync(grammar);

            // Sets the recognition engine to the computer's default audio input device and starts recognising speech
            recognition.SetInputToDefaultAudioDevice();
            // Adds an event handler for when the speech recognition understands something was said
            recognition.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognition_SpeechRecognised);
            // Begins a recognition thread
            recognition.RecognizeAsync(RecognizeMode.Multiple);

            // Message to confirm that JARVIS has been loaded
            say("I have been fully loaded, sir.");
        }

        // Runs when the "Read" button is clicked or the ENTER key is pressed
        private void btnInput_Click(object sender, EventArgs e)
        {
            // Outputs the user input to the output text field
            writeToOutput("USER: " + txtInput.Text);
            txtInput.Clear();
        }
    }
}
