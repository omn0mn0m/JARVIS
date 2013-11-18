using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JARVIS
{
    public partial class WebBrowser : Form
    {
        string username = "Tony.Stark.is.the.cooliest@gmail.com";
        string password = "i_am_ironman";

        public WebBrowser()
        {
            InitializeComponent();
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string s = webBrowser1.DocumentText;
        }

        public void LoadPage(String website)
        {
            switch (website)
            {
                case "facebook":
                    webBrowser1.Navigate("https://login.facebook.com/login.php?login_attempt=1");
                    break;
                case "pakistan":
                    webBrowser1.Navigate("https://www.youtube.com/watch?v=7B6IfpyRpCI");
                    break;
                case "awesome":
                    webBrowser1.Navigate("http://www.youtube.com/watch?v=xeNbgEanR58&feature=youtu.be&t=1m11s");
                    break;
                case "gmail":
                    webBrowser1.Navigate("https://mail.google.com/mail/?shva=1#inbox");
                    break;
                case "googlePlus":
                    webBrowser1.Navigate("https://plus.google.com/u/0/104389711715819668197/posts");
                    break;
                case "dubstep":
                    webBrowser1.Navigate("https://www.youtube.com/watch?v=-GCRYm1URkw");
                    break;
                default:
                    break;
            }
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        public void Login(String website)
        {
            if (website.Equals("facebook"))
            {
                HtmlElement ele = webBrowser1.Document.GetElementById("email");
                if (ele != null)
                    ele.InnerText = username;
                    ele = webBrowser1.Document.GetElementById("pass");
                if (ele != null)
                    ele.InnerText = password;
                    ele = webBrowser1.Document.GetElementById("Login");
                if (ele != null)
                    ele.InvokeMember("click");
            }
            if (website.Equals("gmail"))
            {
                HtmlElement ele = webBrowser1.Document.GetElementById("Email");
                if (ele != null)
                    ele.InnerText = username;
                ele = webBrowser1.Document.GetElementById("Passwd");
                if (ele != null)
                    ele.InnerText = password;
                ele = webBrowser1.Document.GetElementById("signIn");
                if (ele != null)
                    ele.InvokeMember("Click");
            }
        }
    }
}