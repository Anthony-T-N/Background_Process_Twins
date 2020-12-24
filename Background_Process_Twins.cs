using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
/*
https://stackoverflow.com/questions/8455873/how-to-detect-a-process-start-end-using-c-sharp-in-windows
Planning / Design
- Read through running processes on Windows.
- Check whether twin process is running (If not, execute it).
- Kill switch to kill both processes before one executing itself again.
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
                Thread.Sleep(100);
                if (main_program.check_processes() == false)
                {
                    Console.WriteLine("[*] Twin Process Dead [2]");
                    main_program.execute_twin_process();
                }
                /*
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
                */
            }
        }
        // Currently polling. (Bad).
        public bool check_processes()
        {
            Process[] allProcesses = Process.GetProcesses();
            bool process_alive = false;
            int counter = 0;
            int kill_switch_counter = 0;
            Console.WriteLine("=================================================================================");
            for (int i = 0; i <= allProcesses.Length - 1; i++)
            {
                if (allProcesses[i].ToString().Contains("Background_Process_Twin"))
                {
                    Console.WriteLine(allProcesses[i].ToString().Substring(27));
                    Console.WriteLine("[*] Twin Process Is Alive");
                    counter++;
                }
                else if (allProcesses[i].ToString().Contains("notepad"))
                {
                    // If three or more notepad processes are found, kill switch is activated.
                    Console.WriteLine("[*] Notepad Found");
                    Console.WriteLine(kill_switch_counter);
                    kill_switch_counter++;
                    if (kill_switch_counter >= 3)
                    {
                        kill_switch();
                    }
                }
            }
            if (counter == 2)
            {
                process_alive = true;
            }
            return process_alive;
        }
        // Need to seperate both processes and treat them as seperate applications.
        // Remove the cause for child processes.
        // https://stackoverflow.com/questions/8434379/start-new-process-without-being-a-child-of-the-spawning-process
        public void execute_twin_process()
        {
            Process process = new Process();
            string exe_location = Directory.GetCurrentDirectory() + @"\Background_Process_Twin.exe";
            Console.WriteLine(exe_location);
            process.StartInfo.FileName = exe_location;
            // Spawns new process without it becoming a child.
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.Start();
            Console.WriteLine("[+] Twin Process Executed");
        }
        private void kill_switch()
        {
            try
            {
                Console.WriteLine("[+] Kill Switch Activated");
                System.Environment.Exit(0);
                /*
                Process[] proc = Process.GetProcessesByName("Background_Process_Twin");
                Console.WriteLine(proc.Length);
                for (int i = 0; i <= proc.Length; i++)
                {
                    Console.WriteLine(i + ": " + proc[i]);
                    proc[i].Kill();
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
