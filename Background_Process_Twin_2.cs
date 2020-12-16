using System;
using System.Diagnostics;

namespace Background_Process_Twins
{
    class Background_Process_Twin_2
    {
        static void Main(string[] args)
        {
            Background_Process_Twin_2 main_program = new Background_Process_Twin_2();
            main_program.check_processes();
        }
        public void check_processes()
        {
            Process[] allProcesses = Process.GetProcesses();
            bool process_alive = false;
            while (process_alive == false)
            {
                for (int i = 0; i <= allProcesses.Length - 1; i++)
                {
                    Console.WriteLine(allProcesses[i]);
                    if (allProcesses[i].ToString().Contains("Background_Process_Twin"))
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
