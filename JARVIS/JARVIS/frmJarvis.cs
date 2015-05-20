using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;

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
        private static SpeechRecognitionEngine recognition = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));    // Speech recognition engine w/ US English as the langauge
        public static bool useRecognition = true;             // If speech recognition should be used
        public static string wolframAppID = "LXA9LJ-3835YR8529";

        private static DictationGrammar noiseGrammar;
        private static DictationGrammar dictationGrammar;
        private static Grammar commandGrammar;
        private static Grammar activationGrammar;

        private static bool foundCommand = false;
        private static string commandMessage;

        private static Input input = new Input();
        private static Converser converser = new Converser();          // Converser for casual conversation with user                                                     

        private static XMPPInteractor facebookInteract;                // XMPP interactor for Facebook

        private static BackgroundWorker bwGetResponse = new BackgroundWorker();

        private static PCManager pcManager = new PCManager();           // Manages system tasks

        private static OfficeManager officeManager = new OfficeManager();

        private static string ageOfUltron = @"C:\Users\Nam\Documents\GitHub\JARVIS\JARVIS\JARVIS\Resources\age_of_ultron_trailer.mp4";

        public frmJarvis()
        {
            bwGetResponse.DoWork += new DoWorkEventHandler(bwGetResponse_DoWork);
            bwGetResponse.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwGetReponse_RunWorkerCompleted);
            bwGetResponse.ProgressChanged += new ProgressChangedEventHandler(bwGetResponse_ProgressChanged);
            bwGetResponse.WorkerReportsProgress = true;
            bwGetResponse.WorkerSupportsCancellation = true;

            // Creates a grammar to understand most English sentences
            dictationGrammar = new DictationGrammar();
            dictationGrammar.Name = "Dictation Grammar";
            dictationGrammar.Enabled = false;

            // Creates a grammar to find noise
            noiseGrammar = new DictationGrammar("grammar:dictation#pronunciation");
            noiseGrammar.Name = "Noise Grammar";
            noiseGrammar.Enabled = true;

            // Creates a specific command grammar system to only understand a few phrases
            Choices commandChoices = new Choices();
            commandChoices.Add(new string[] {
                "open palemoon",
                "open notepad",
                "who is barack obama",
                "what is the forecast in horsham pennsylvania",
                "what is the derivative of 3x^3 + 2x",
                "next slide",
                "previous slide",
                "in conclusion",
                "respond to facebook",
                "look at me"
            });
            commandGrammar = new Grammar(commandChoices);
            commandGrammar.Name = "Command Grammar";
            commandGrammar.Enabled = false;

            // Creates the mode activator
            Choices activationChoices = new Choices();
            activationChoices.Add(new string[] { "jarvis", "let's talk", "stop talking" });
            activationGrammar = new Grammar(activationChoices);
            activationGrammar.Name = "Activation Grammar";
            activationGrammar.Enabled = true;

            // Loads the grammar to the speech recognition engine
            recognition.LoadGrammarAsync(activationGrammar);
            recognition.LoadGrammarAsync(dictationGrammar);
            recognition.LoadGrammarAsync(commandGrammar);
            recognition.LoadGrammarAsync(noiseGrammar);

            // Sets the recognition engine to the computer's default audio input device and starts recognising speech
            recognition.SetInputToDefaultAudioDevice();
            // Adds an event handler for when the speech recognition understands something was said
            recognition.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognition_SpeechRecognised);
            // Begins a recognition thread
            recognition.RecognizeAsync(RecognizeMode.Multiple);

            InitializeComponent();      // Loads the form components
        }

        // Writes a message to the text output field with a date/time stamp and the message 
        public void WriteToOutput(String output)
        {
            // Adds the new output message to a new line in the output text field
            txtOutput.AppendText(System.Environment.NewLine + "[" + System.DateTime.Now + "] " + Input.CensorInput(output));
        }

        // Event handler for when speech is recognised
        public void Recognition_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
             ThinkOfResponse(e.Result.Text, e.Result.Grammar.Name);
        }

        public void ThinkOfResponse(string input)
        {
            // Writes the recognised text to the input text field and outputs it to the output text field
            ReceiveInput(input);

            InterpretInput();

            if (!foundCommand)
            {
                if (!bwGetResponse.IsBusy)
                {
                    bwGetResponse.RunWorkerAsync(input);
                }
            }
            else
            {
                WriteToOutput("JARVIS: " + commandMessage);
                Converser.Say(commandMessage, recognition);
            }
        }

        public void ThinkOfResponse(string input, string grammar)
        {
            // Determines which grammars to use...
            switch (grammar)
            {
                case ("Activation Grammar"):
                    ReceiveInput(input);

                    switch (input)
                    {
                        case "jarvis":
                            recognition.Grammars[recognition.Grammars.IndexOf(activationGrammar)].Enabled = false;
                            recognition.Grammars[recognition.Grammars.IndexOf(noiseGrammar)].Enabled = true;
                            recognition.Grammars[recognition.Grammars.IndexOf(commandGrammar)].Enabled = true;
                            recognition.Grammars[recognition.Grammars.IndexOf(dictationGrammar)].Enabled = false;
                            break;
                        case "let's talk":
                            recognition.Grammars[recognition.Grammars.IndexOf(activationGrammar)].Enabled = false;
                            recognition.Grammars[recognition.Grammars.IndexOf(noiseGrammar)].Enabled = false;
                            recognition.Grammars[recognition.Grammars.IndexOf(commandGrammar)].Enabled = false;
                            recognition.Grammars[recognition.Grammars.IndexOf(dictationGrammar)].Enabled = true;
                            break;
                        case "stop talking":
                            recognition.Grammars[recognition.Grammars.IndexOf(activationGrammar)].Enabled = true;
                            recognition.Grammars[recognition.Grammars.IndexOf(noiseGrammar)].Enabled = true;
                            recognition.Grammars[recognition.Grammars.IndexOf(commandGrammar)].Enabled = false;
                            recognition.Grammars[recognition.Grammars.IndexOf(dictationGrammar)].Enabled = false;
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Noise Grammar"):
                    break;
                case ("Command Grammar"):
                    recognition.Grammars[recognition.Grammars.IndexOf(activationGrammar)].Enabled = true;
                    recognition.Grammars[recognition.Grammars.IndexOf(commandGrammar)].Enabled = false;
                    ThinkOfResponse(input);
                    break;
                case ("Dictation Grammar"):
                    recognition.Grammars[recognition.Grammars.IndexOf(activationGrammar)].Enabled = true;

                    ReceiveInput(input);

                    InterpretInput();

                    if (!foundCommand)
                    {
                        if (!bwGetResponse.IsBusy)
                        {
                            bwGetResponse.RunWorkerAsync(input);
                        }
                    }
                    else
                    {
                        WriteToOutput("JARVIS: " + commandMessage);
                        Converser.Say(commandMessage, recognition);
                    }
                    break;
                default:
                    break;
            }
        }

        // Runs when the form is loaded
        private void frmJarvis_Load(object sender, EventArgs e)
        {
            //faceTracking.Show();

            WriteToOutput(Converser.Say("I have been fully loaded.", recognition));
        }

        // Runs when the "Read" button is clicked or the ENTER key is pressed
        private void btnInput_Click(object sender, EventArgs e)
        {
            // Outputs the user input to the output text field
            string input = txtInput.Text;
            txtInput.Clear();

            ThinkOfResponse(input);
        }

        // Takes in the input, outputs it, and turns it into an array for processing
        public void ReceiveInput(String inputString)
        {
            WriteToOutput("USER: " + inputString);
            input.ReceiveInput(inputString);
        }

        public static void InterpretInput()
        {
            foundCommand = false;
            commandMessage = "";

            for (int i = 0; i < input.GetInputArrayLength(); i++)
            {
                if (!foundCommand)
                {
                    switch (input.GetWord(i))
                    {
                        case "open":
                            if (!foundCommand)
                            {
                                for (int j = (i + 1); j < input.GetInputArrayLength(); j++)
                                {
                                    commandMessage = pcManager.SearchAndOpen(input.GetWord(j));
                                    foundCommand = pcManager.foundProgram;
                                }
                            }
                            break;
                        case "respond":
                            if (!foundCommand)
                            {
                                for (int j = i; j < input.GetInputArrayLength(); j++)
                                {
                                    bool found = false;

                                    if (!found)
                                    {
                                        switch (input.GetWord(j))
                                        {
                                            case "facebook":
                                                facebookInteract = new XMPPInteractor("chat.facebook.com", "tranngocnam97", "tiengviet");
                                                found = true;
                                                foundCommand = true;
                                                commandMessage = "Responding to all incoming Facebook messages";
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
                            break;
                        case "powerpoint":
                            officeManager.CheckForApplication(OfficeManager.ApplicationType.PowerPoint);

                            for (int j = i; j < input.GetInputArrayLength(); j++)
                            {
                                bool found = false;

                                if (!found)
                                {
                                    switch (input.GetWord(j))
                                    {
                                        case "first":
                                            officeManager.goToFirstSlide();
                                            commandMessage = "Going to first slide";
                                            break;
                                        case "last":
                                            officeManager.goToLastSlide();
                                            commandMessage = "Going to last slide";
                                            break;
                                        case "next":
                                            officeManager.goToNextSlide();
                                            commandMessage = "Going to next slide";
                                            break;
                                        case "previous":
                                            officeManager.goToPreviousSlide();
                                            commandMessage = "Going to previous slide";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    foundCommand = true;
                                    break;
                                }
                            }
                            break;
                        case "ultron":
                            frmVideo videoPlayer = new frmVideo();
                            //videoPlayer.Invoke((MethodInvoker)delegate() {
                                videoPlayer.Show();
                                videoPlayer.LoadVideo(ageOfUltron);
                            //});
                            foundCommand = true;
                            break;
                        case "look":
                            FaceTracking.MainForm faceTracking = new FaceTracking.MainForm();
                            faceTracking.Show();
                            commandMessage = "Activating facial detection. Please select a camera";
                            foundCommand = true;
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
            Converser.useSpeech = !cbSynthesis.Checked;
        }

        private void bwGetResponse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // TODO: Add something here
        }

        private void bwGetReponse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && (e.Error == null))
            {
                string result = (string)e.Result;
                WriteToOutput("JARVIS: " + result);
                Converser.Say(result, recognition);
            }
            else if (e.Cancelled)
            {
                WriteToOutput(Converser.Say("User Cancelled", recognition));
            }
            else
            {
                WriteToOutput(Converser.Say("An error has occured", recognition));
            }
        }

        private void bwGetResponse_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker = (BackgroundWorker)sender;
            string input = (string)e.Argument;

            e.Result = converser.Respond(input);
        }

        /*
         * This is for resource clean-up when the user closes the program
         */
        private void frmJarvis_FormClosed(object sender, FormClosedEventArgs e)
        {
            recognition.Dispose();
        }
    }
}
