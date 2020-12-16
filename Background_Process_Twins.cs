using System;
using System.Diagnostics;

/*
Planning / Design
- Read through running processes on Windows
- Check whether twin process is running (If not, execute it)
*/

namespace Background_Process_Twins
{
    class Background_Process_Twin
    {
        static void Main(string[] args)
        {
            Background_Process_Twin main_program = new Background_Process_Twin();
            main_program.check_processes();
        }
        // Currently polling. (Bad).
        public void check_processes()
        {
            Process[] allProcesses = Process.GetProcesses();
            bool process_alive = false;
            while (process_alive == false)
            {
                for (int i = 0; i <= allProcesses.Length - 1; i++)
                {
                    Console.WriteLine(allProcesses[i]);
                    if (allProcesses[i].ToString().Contains("Background_Process_Twin-2"))
                    {
                        Console.WriteLine("[*] Twin Process Is Alive");
                        process_alive = true;
                        break;
                    }
                }
            }
        }
        private void kill_switch()
        {

        }
    }
}
