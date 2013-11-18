using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace JARVIS
{
    public partial class frmJARVIS : Form
    {

        private string userInput;
        private Boolean listenForJarvis = true;
        private int inputType = 0;

        SpeechSynthesizer speech = new SpeechSynthesizer();
        private int speechRate = 1;
        private int speechVolume = 100;

        WebBrowser webBrowser = new WebBrowser();

        private String name = "jarvis";

        public frmJARVIS()
        {
            InitializeComponent();
        }

        private void frmJARVIS_Load(object sender, EventArgs e)
        {
            this.Say("Coming, sir.");
        }

        private void btnClearInput_Click(object sender, EventArgs e)
        {
            txtUserInput.Clear();
        }

        public void btnRead_Click(object sender, EventArgs e)
        {
            this.giveCommand(inputType);
        }

        private void txtUserInput_TextChanged(object sender, EventArgs e)
        {
            if (listenForJarvis == true)
            {
                if (txtUserInput.Text.ToLower().Equals(name))
                {
                    listenForJarvis = false;
                    this.Say("yes sir?");
                }
            }
        }

        public void PerformAction(String command)
        {
            switch (command) {
                case "launch missile":
                    this.Say("Watch your missile and please reconsider your life, sir.");
                        webBrowser.Show();
                        webBrowser.LoadPage("pakistan");
                    break;
                case "open facebook":
                    webBrowser.Show();
                    webBrowser.LoadPage("facebook");
                    this.Say("Opening your fake social life, sir.");
                    break;
                case "log into facebook":
                    webBrowser.Login("facebook");
                    this.Say("Entering your credentials, sir.");
                    break;
                case "shutdown computer":
                    this.Say("Shutting down your computer, sir.");
                    System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                    break;
                case "restart computer":
                    this.Say("Restarting your computer, see you again shortly.");
                    System.Diagnostics.Process.Start("shutdown", "/r /t 0");
                    break;
                case "remind me why i'm awesome":
                    this.Say("Your humility is too much, sir.");
                    webBrowser.Show();
                    webBrowser.LoadPage("awesome");
                    break;
                case "check my fanmail":
                    this.Say("What fans, sir?");
                    webBrowser.Show();
                    webBrowser.LoadPage("gmail");
                    break;
                case "log into gmail":
                    webBrowser.Login("gmail");
                    this.Say("Too lazy to type your own username and password, sir?");
                    break;
                case "log into google plus":
                    this.Say("Logging into your actual social life, sir.");
                    webBrowser.Show();
                    webBrowser.LoadPage("googlePlus");
                    break;
                case "drop the bass":
                    this.Say("Consider it dropped, sir.");
                    webBrowser.Show();
                    webBrowser.LoadPage("dubstep");
                    break;
                case "open notepad":
                    this.Say("I didn't know you could read, sir.");
                    System.Diagnostics.Process.Start("notepad.exe");
                    break;
                case "you're dismissed jarvis":
                    this.Say("Until next time, sir.");
                    listenForJarvis = true;
                    break;
                default:
                    this.Say("I don't understand, sir.");
                    break;
            }
        }

        public void Say(String message)
        {
            speech.Rate = speechRate;
            speech.Volume = speechVolume;
            speech.Speak(message);
        }

        private void giveCommand(int response)
        {
            if (listenForJarvis == false)
            {
                userInput = txtUserInput.Text.ToLower();
                if (userInput.Length > 0)
                {
                    if (response == 0)
                    {
                        this.PerformAction(userInput);
                    }
                    else if (response == 1)
                    {
                        name = userInput;
                    }
                }
            }
        }
    }
}
