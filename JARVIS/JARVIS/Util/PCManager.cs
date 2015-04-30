using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JARVIS.Util
{
    class PCManager
    {
        private string programDirectory64 = "C:\\Program Files";
        private string programDirectory32 = "C:\\Program Files (x86)";

        public PCManager()
        {

        }

        public void OpenProgram(string name)
        {
            System.Diagnostics.Process.Start(name + ".exe");
        }

        public void CloseAllProgramInstances(string name)
        {
            foreach (Process proc in Process.GetProcessesByName(name))
            {
                proc.CloseMainWindow();
                proc.WaitForExit();
            }
        }

        public void SearchAndOpen(string name)
        {
            foreach (string d in Directory.GetDirectories(programDirectory64))
            {
                foreach (string f in Directory.GetFiles(d, name + ".exe"))
                {
                    System.Diagnostics.Process.Start(f);
                }
            }
        }
    }
}
