using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JARVIS.Util
{
    class SettingsScanner
    {
        private XmlTextWriter settingsWriter;

        public SettingsScanner()
        {
            settingsWriter = new XmlTextWriter("settings.xml", System.Text.Encoding.UTF8);
        }

        public void setSettings()
        {

        }

        public void getSettings()
        {

        }
    }
}
