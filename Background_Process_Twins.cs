using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
/*
https://stackoverflow.com/questions/8455873/how-to-detect-a-process-start-end-using-c-sharp-in-windows
Planning / Design
- Read through running processes on Windows.
- Check whether twin process is running (If not, execute it).
- Kill switch to kill both processes before one executing itself again.
- Prevent user from right-clicking application in taskbar and selecting "Close all Windows".
*/
namespace Background_Process_Twins
{
    class Background_Process_Twin
    {
        #region <Hiding console application from taskbar.>
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/73ba89de-bc92-44bd-81aa-462bd5ee7e46/how-to-hide-a-c-console-application-not-a-winform-app-from-taskbar?forum=windowsuidevelopment
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        // const int SW_SHOW = 5;
        #endregion

        #region <Used to change app user model ID.>
        /*
        // https://stackoverflow.com/questions/48539789/pinning-to-the-taskbar-a-chained-process/58559875#58559875
        // https://stackoverflow.com/questions/34901124/how-to-group-different-apps-in-windows-task-bar
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern void SetCurrentProcessExplicitAppUserModelID(
            [MarshalAs(UnmanagedType.LPWStr)] string AppID);
        private static string AppID;
        */
        #endregion

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // Hide

            #region <Used to set AUMID.>
            /*
            // Twin applications are grouped together in the taskbar when running.
            // Setting different AUMID for every instance of the program that launches should ideally seperate them in the taskbar.
            AppID = Guid.NewGuid().ToString();
            SetCurrentProcessExplicitAppUserModelID(AppID);
            Debug.WriteLine(AppID);
            Thread.Sleep(5000);
            */
            #endregion

            Background_Process_Twin main_program = new Background_Process_Twin();
            while (true)
            {
                Thread.Sleep(1000); // Sleep reduces CPU Usage from 40% down to 2.1%.
                if (main_program.check_processes() == false)
                {
                    Debug.WriteLine("[*] Twin Process Dead [2]");
                    main_program.execute_twin_process();
                }
                #region Example of practical usage (Continuously closes task manager).
                /*
                try
                {
                    Process[] proc = Process.GetProcessesByName("Taskmgr");
                    if (proc.Length >= 1)
                    {
                        Debug.WriteLine(proc[0]);
                        Debug.WriteLine("[*] Task Manager Killed");
                        proc[0].Kill();
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                */
                #endregion
            }
        }
        // Currently polling. (Bad).
        private bool check_processes()
        {
            Process[] allProcesses = Process.GetProcesses();
            bool process_alive = false;
            int counter = 0, kill_switch_counter = 0;
            Debug.WriteLine("=================================================================================");
            for (int i = 0; i <= allProcesses.Length - 1; i++)
            {
                if (allProcesses[i].ToString().Contains("Background_Process_Twin"))
                {
                    Debug.WriteLine(allProcesses[i].ToString().Substring(27));
                    Debug.WriteLine("[*] Twin Process Is Alive");
                    counter++;
                }
                else if (allProcesses[i].ToString().Contains("notepad"))
                {
                    // If two or more notepad processes are found, kill switch is activated.
                    Debug.WriteLine("[*] Notepad Found. Counter: " + kill_switch_counter);
                    kill_switch_counter++;
                    if (kill_switch_counter >= 2)
                    {
                        kill_switch();
                    }
                }
            }
            if (counter >= 2)
            {
                process_alive = true;
            }
            return process_alive;
        }
        // Need to seperate both processes and treat them as seperate applications.
        // Remove the cause for child processes.
        // https://stackoverflow.com/questions/8434379/start-new-process-without-being-a-child-of-the-spawning-process
        private void execute_twin_process()
        {
            Process process = new Process();
            string exe_location = Directory.GetCurrentDirectory() + @"\Background_Process_Twin.exe";
            Debug.WriteLine(exe_location);
            process.StartInfo.FileName = exe_location;
            // Spawns new process without it becoming a child.
            process.StartInfo.UseShellExecute = true;
            //process.StartInfo.CreateNoWindow = true;
            // Treats app as background process (Hidden from taskbar).
            //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.Start();
            Debug.WriteLine("[+] Twin Process Executed");
        }
        private void kill_switch()
        {
            Debug.WriteLine("[+] Kill Switch Activated");
            System.Environment.Exit(0);
        }
    }
}
