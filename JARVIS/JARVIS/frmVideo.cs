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
    public partial class frmVideo : Form
    {
        public frmVideo()
        {
            InitializeComponent();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            GoFullscreen();
        }

        public void LoadVideo(string filePath)
        {
            axWindowsMediaPlayer1.URL = filePath;
        }

        public void StopVideo()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        public void PlayVideo()
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        public void PauseVideo()
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        public void GoFullscreen()
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }

        private void frmVideo_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.uiMode = "full";
        }
    }
}
