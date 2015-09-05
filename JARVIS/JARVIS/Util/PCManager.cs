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
        private const string programDirectory64 = @"C:\Program Files";
        private const string programDirectory32 = @"C:\Program Files (x86)";
        private const string system32 = @"C:\windows\system32";

        private Process process = new Process();

        public bool foundProgram = false;

        public PCManager()
        {

        }

        public bool OpenProgram(string name)
        {
            try
            {
                process.StartInfo = new ProcessStartInfo(name + ".exe");
                return process.Start();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CloseAllProgramInstances(string name)
        {
            foreach (Process proc in Process.GetProcessesByName(name))
            {
                proc.CloseMainWindow();
                proc.WaitForExit();
            }
        }

        public string SearchAndOpen(string name)
        {
            foundProgram = false;

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
                    return "Opening " + name;
                }
            }

            System.Console.WriteLine("Done Program Directory 64");

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
                        return "Opening " + name; ;
                    }
                }
            }

            System.Console.WriteLine("Done Program Directory 32");
            
            try
            {
                foundProgram = OpenProgram(name);
            }
            catch (System.UnauthorizedAccessException) { }

            return "One last try at opening " + name;
        }
    }
}
