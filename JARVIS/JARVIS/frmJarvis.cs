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
using JARVIS.Util;
using System.Diagnostics;

namespace JARVIS
{
    public partial class frmJarvis : Form
    {
        // Speech recognition engine w/ US English as the langauge
        private SpeechRecognitionEngine recognition;
        // Text to Speech
        private SpeechSynthesizer speech = new SpeechSynthesizer();
        // If speech recognition should be used
        private bool useRecognition = true;
        // If JARVIS should speak
        private bool useSpeech = true;

        // Array for input
        private String[] inputArray;

        // Array for last command stated
        private String[] lastCommand;

        // Converser for casual conversation with user
        Converser converser = new Converser();

        // XMPP interactor for Facebook
        XMPPInteractor facebookInteract;

        public frmJarvis()
        {
            // Loads the form components
            InitializeComponent();
        }

        // Writes a message to the text output field with a date/time stamp and the message 
        public void WriteToOutput(String output)
        {
            // Adds the new output message to a new line in the output text field
            txtOutput.AppendText(System.Environment.NewLine + "[" + System.DateTime.Now + "] " + output);
        }

        // Makes JARVIS say something with text to speech as well as prints it to the console
        public void Say(String message)
        {
            if (useSpeech)
            {
                // Reads the message as speech
                speech.Speak(message);
            }
            // Writes the message to the output text field
            WriteToOutput("JARVIS: " + message);
        }

        // Event handler for when speech is recognised
        public void Recognition_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            // Writes the recognised text to the input text field and outputs it to the output text field
            ReceiveInput(e.Result.Text);
            Converse();
            InterpretInput();
        }

        // Runs when the form is loaded
        private void frmJarvis_Load(object sender, EventArgs e)
        {
            // Creates a grammar to understand most English sentences
            DictationGrammar grammar = new DictationGrammar();
            grammar.Name = "Default Grammar";
            grammar.Enabled = true;

            // Loads speech recognition
            recognition = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            // Loads the grammar to the speech recognition engine
            recognition.LoadGrammarAsync(grammar);

            // Sets the recognition engine to the computer's default audio input device and starts recognising speech
            recognition.SetInputToDefaultAudioDevice();
            // Adds an event handler for when the speech recognition understands something was said
            recognition.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognition_SpeechRecognised);
            // Begins a recognition thread
            recognition.RecognizeAsync(RecognizeMode.Multiple);

            Say("I have been fully loaded.");
        }

        // Runs when the "Read" button is clicked or the ENTER key is pressed
        private void btnInput_Click(object sender, EventArgs e)
        {
            // Outputs the user input to the output text field
            ReceiveInput(txtInput.Text);
            txtInput.Clear();
            Converse();
            InterpretInput();
        }

        // Takes in the input, outputs it, and turns it into an array for processing
        public void ReceiveInput(String input)
        {
            WriteToOutput("USER: " + input);
            inputArray = input.Split(' ');
        }

        public void Converse()
        {
            String input = "";

            for (int i = 0; i < inputArray.Length; i++)
            {
                input = input + inputArray[i];
            }

            Say(converser.Respond(input));
        }

        public void InterpretInput()
        {
            for (int i = 0; i < inputArray.Length; i++)
            {
                bool command = false;
                if (!command)
                {
                    switch (inputArray[i])
                    {
                        case "open":
                            if (!command)
                            {
                                for (int j = i; j < inputArray.Length; j++)
                                {
                                    bool found = false;

                                    if (!found)
                                    {
                                        switch (inputArray[j])
                                        {
                                            case "notepad":
                                                System.Diagnostics.Process.Start("notepad.exe");
                                                found = true;
                                                command = true;
                                                lastCommand = "open notepad".Split(' ');
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        command = true;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "respond":
                            if (!command)
                            {
                                for (int j = i; j < inputArray.Length; j++)
                                {
                                    bool found = false;

                                    if (!found)
                                    {
                                        switch (inputArray[j])
                                        {
                                            case "facebook":
                                                facebookInteract = new XMPPInteractor("chat.facebook.com", "tranngocnam97", "tiengviet");
                                                found = true;
                                                command = true;
                                                lastCommand = "respond facebook".Split(' ');
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        command = true;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "stop":
                            if (!command)
                            {
                                for (int j = i; j < lastCommand.Length; j++)
                                {
                                    bool found = false;

                                    if (!found)
                                    {
                                        switch (lastCommand[j])
                                        {
                                            case "open":
                                                for (int k = j; k < lastCommand.Length; k++)
                                                {
                                                    switch (lastCommand[k])
                                                    {
                                                        case "notepad":
                                                            foreach (Process proc in Process.GetProcessesByName("Notepad"))
                                                            {
                                                                proc.CloseMainWindow();
                                                                proc.WaitForExit();
                                                            }
                                                            found = true;
                                                            command = true;
                                                            break;
                                                    }

                                                }
                                                break;
                                            case "respond":
                                                for (int k = j; k < lastCommand.Length; k++)
                                                {
                                                    switch (lastCommand[k])
                                                    {
                                                        case "facebook":
                                                            facebookInteract.Close();
                                                            found = true;
                                                            command = true;
                                                            break;
                                                    }
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void cbRecognise_CheckedChanged(object sender, EventArgs e)
        {
            useRecognition = !cbRecognise.Checked;
            if (useRecognition)
            {
                recognition.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                recognition.RecognizeAsyncCancel();
            }
        }

        private void cbSynthesis_CheckedChanged(object sender, EventArgs e)
        {
            useSpeech = !cbSynthesis.Checked;
        }
    }
}
