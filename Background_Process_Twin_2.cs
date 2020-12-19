using System;
using System.Diagnostics;
using System.Threading;

namespace Background_Process_Twins
{
    class Background_Process_Twin_2
    {
        static void Main(string[] args)
        {
            Background_Process_Twin_2 main_program = new Background_Process_Twin_2();
            while (true)
            {
                //Sleep reduces CPU Usage from 40% down to 2.1%.
                Thread.Sleep(5000);
                if (main_program.check_processes() == false)
                {
                    Console.WriteLine("[*] Twin Process Dead [1]");
                    main_program.execute_twin_process();
                }
            }
        }
        // Currently polling. (Bad).
        public bool check_processes()
        {
            Process[] allProcesses = Process.GetProcesses();
            bool process_alive = false;
            for (int i = 0; i <= allProcesses.Length - 1; i++)
            {
                if (allProcesses[i].ToString().Contains("Background_Process_Twin") && !allProcesses[i].ToString().Contains("2"))
                {
                    Console.WriteLine(allProcesses[i].ToString().Substring(27));
                    Console.WriteLine("[*] Twin Process Is Alive [1]");
                    process_alive = true;
                    break;
                }
            }
            return process_alive;
        }
        public void execute_twin_process()
        {
            Process.Start(@"---");
            Console.WriteLine("[+] Twin Process Executed");
        }
        private void kill_switch()
        {

        }
    }
}
