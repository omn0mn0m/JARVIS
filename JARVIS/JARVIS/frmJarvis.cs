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
using FaceTracking;
using System.Diagnostics;

namespace JARVIS
{
    public partial class frmJarvis : Form
    {
        private SpeechRecognitionEngine recognition;                        // Speech recognition engine w/ US English as the langauge
        private SpeechSynthesizer speech = new SpeechSynthesizer();         // Text to Speech

        private bool useRecognition = true;             // If speech recognition should be used
        private bool useSpeech = true;                  // If JARVIS should speak

        private String[] inputArray;                    // Array for input
        private String[] lastCommand;                   // Array for last command stated
        private Converser converser = new Converser();          // Converser for casual conversation with user

        private XMPPInteractor facebookInteract;                // XMPP interactor for Facebook

        private BackgroundWorker bwGetResponse = new BackgroundWorker();

        private PCManager pcManager = new PCManager();           // Manages system tasks

        private FaceTracking.MainForm faceTracking = new FaceTracking.MainForm();

        public frmJarvis()
        {
            bwGetResponse.DoWork += new DoWorkEventHandler(bwGetResponse_DoWork);
            bwGetResponse.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwGetReponse_RunWorkerCompleted);
            bwGetResponse.ProgressChanged += new ProgressChangedEventHandler(bwGetResponse_ProgressChanged);
            bwGetResponse.WorkerReportsProgress = true;
            bwGetResponse.WorkerSupportsCancellation = true;

            InitializeComponent();      // Loads the form components
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
                speech.Speak(message);                   // Reads the message as speech
            }
            WriteToOutput("JARVIS: " + message);        // Writes the message to the output text field
        }

        // Event handler for when speech is recognised
        public void Recognition_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            // Writes the recognised text to the input text field and outputs it to the output text field
            ReceiveInput(e.Result.Text);

            if (!bwGetResponse.IsBusy)
            {
                bwGetResponse.RunWorkerAsync(e.Result.Text);
            }

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

            faceTracking.Show();

            Say("I have been fully loaded.");
        }

        // Runs when the "Read" button is clicked or the ENTER key is pressed
        private void btnInput_Click(object sender, EventArgs e)
        {
            // Outputs the user input to the output text field
            string input = txtInput.Text;
            txtInput.Clear();
            ReceiveInput(input);
            
            if (!bwGetResponse.IsBusy)
            {
                bwGetResponse.RunWorkerAsync(input);
            }

            InterpretInput();
        }

        // Takes in the input, outputs it, and turns it into an array for processing
        public void ReceiveInput(String input)
        {
            WriteToOutput("USER: " + input);
            inputArray = input.Split(' ');
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
                                                pcManager.OpenProgram("notepad");
                                                found = true;
                                                command = true;
                                                lastCommand = "open notepad".Split(' ');
                                                break;
                                            default:
                                                pcManager.SearchAndOpen(inputArray[j]);
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
                                                            pcManager.CloseAllProgramInstances("Notepad");
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

        private void bwGetResponse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // TODO: Add something here
        }

        private void bwGetReponse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && (e.Error == null))
            {
                string response = (string)e.Result;
                Say(response);
            }
            else if (e.Cancelled)
            {
                Say("User Cancelled");
            }
            else
            {
                Say("An error has occured");
            }
        }

        private void bwGetResponse_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker = (BackgroundWorker)sender;
            string input = (string)e.Argument;

            Converser converser = new Converser();
            e.Result = converser.Respond(input);
        }
    }
}
