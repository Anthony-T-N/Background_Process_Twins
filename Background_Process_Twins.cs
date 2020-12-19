using System;
using System.Diagnostics;
using System.Threading;
/*
https://stackoverflow.com/questions/8455873/how-to-detect-a-process-start-end-using-c-sharp-in-windows
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
            while (true)
            {
                //Sleep reduces CPU Usage from 40% down to 2.1%.
                Thread.Sleep(5000);
                if (main_program.check_processes() == false)
                {
                    Console.WriteLine("[*] Twin Process Dead [2]");
                    main_program.execute_twin_process();
                }
                try
                {
                    Process[] proc = Process.GetProcessesByName("Taskmgr");
                    Console.WriteLine(proc[0]);
                    proc[0].Kill();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
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
                if (allProcesses[i].ToString().Contains("Background_Process_Twin_2"))
                {
                    Console.WriteLine(allProcesses[i].ToString().Substring(27));
                    Console.WriteLine("[*] Twin Process Is Alive [2]");
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
