using System;
using System.Diagnostics;

/*
Planning / Design
- Read through running processes on Windows
- Check whether twin proccess is running (If not, execute it)
*/

namespace Background_Process_Twins
{
    class Background_Process_Twins
    {
        static void Main(string[] args)
        {
            Process[] allProcesses = Process.GetProcesses();
            for (int i = 0; i <= allProcesses.Length - 1; i++)
            {
                Console.WriteLine(allProcesses[i]);
                if (allProcesses[i].ToString().Contains("Background_Process_Twins"))
                {
                    Console.WriteLine("[*] Twin Process Alive");
                }
            }
        }
    }
}
