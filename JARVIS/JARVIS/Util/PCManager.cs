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
        private string system32 = "C:\\windows\\system32";

        public PCManager()
        {

        }

        public void OpenProgram(string name)
        {
            try
            {
                System.Diagnostics.Process.Start(name + ".exe");
            }
            catch (System.ComponentModel.Win32Exception) { }
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
            bool foundProgram = false;

            foreach (string d in Directory.GetDirectories(programDirectory64))
            {
                try
                {
                    foreach (string f in Directory.GetFiles(d, name + ".exe"))
                    {
                        System.Diagnostics.Process.Start(f);
                        foundProgram = true;
                    }
                }
                catch (System.UnauthorizedAccessException) { }

                if (foundProgram)
                {
                    break;
                }
            }

            if (!foundProgram)
            {
                foreach (string d in Directory.GetDirectories(programDirectory32))
                {
                    try
                    {
                        foreach (string f in Directory.GetFiles(d, name + ".exe"))
                        {
                            System.Diagnostics.Process.Start(f);
                            foundProgram = true;
                        }
                    }
                    catch (System.UnauthorizedAccessException) { }

                    if (foundProgram)
                    {
                        break;
                    }
                }
            }

            if (!foundProgram)
            {
                OpenProgram(name);
            }
        }
    }
}
