using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BugGazer
{
    public class Controller
    {
        public DBWinListener ListenerWin32 = new DBWinListener(DBWinListener.ListenerType.DbWin32);
        public DBWinListener ListenerGlobalWin32 = new DBWinListener(DBWinListener.ListenerType.GlobalDbWin32);

        static bool mConsoleOpened = false;
        static List<string> mNoConsoleBuffer = new List<string>();
        IBugGazerControl mBugGazerControl;

        public Controller(IBugGazerControl bugGazerControl)
        {
            mBugGazerControl = bugGazerControl;
#if DEBUG
            OpenConsole();
#endif
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        public void OpenConsole()
        {
            // opening a console twice is not supported by windows (even after FreeConsole), so we prevent it.
            if (!mConsoleOpened)
            {
                AllocConsole();     // open console for debug messages
                Console.WriteLine("BugGazer Console Ready.");
                foreach (string s in mNoConsoleBuffer)
                {
                    Console.WriteLine(s);
                }
                mConsoleOpened = true;
                mNoConsoleBuffer.Clear();
            }
        }

        public static void WriteLine(string msg, params object[] args)
        {
            if (mConsoleOpened)
            {
                Console.WriteLine(msg, args);
            }
            else
            {
                if (Environment.CommandLine.ToLower().Contains("/debug"))
                {
                    // this will leak memory, so is normally off
                    mNoConsoleBuffer.Add(string.Format(msg, args));
                }
            }
        }

        protected void Dispose(bool disposing)
        {
            //todo: why is this never called? find out tear-down procedure
            if (disposing)
            {
                EnableWin32(false);
                EnableWin32Global(false);
            }
        }

        public void EnableSound(bool value)
        {

        }

        public bool EnableWin32(bool value)
        {
            if (value)
            {
                if (ListenerWin32.Initialize())
                {
                    ListenerWin32.StartListening();
                    Controller.WriteLine("Now listening for Win32 Debug Messages...");
                    return true;
                }
                else
                {
                    MessageBox.Show("Unable to capture Win32 Messages.\nMake sure you have appropriate permissions.", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                ListenerWin32.StopListening();
                ListenerWin32.Uninitialize();
                Controller.WriteLine("Stopped listening for Win32 Debug Messages...");
            }
            return false;
        }

        public bool EnableWin32Global(bool value)
        {
            if (value)
            {
                if (ListenerGlobalWin32.Initialize())
                {
                    ListenerGlobalWin32.StartListening();
                    Controller.WriteLine("Now listening for Global Win32 Debug Messages...");
                    return true;
                }
                else
                {
                    MessageBox.Show("Unable to capture Global Win32 Messages.\n\nMake sure you have appropriate permissions.\n\n" +
                        "You may need to start this application by right-clicking it and selecting 'Run As Administator' " + 
                        "even if you have administrator rights.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                ListenerGlobalWin32.StopListening();
                ListenerGlobalWin32.Uninitialize();
                Controller.WriteLine("Stopped listening for Global Win32 Debug Messages...");
            }
            return false;
        }

        public void Search(string key, bool forward)
        {
            Controller.WriteLine("Search for key: {0} ({1})", key, forward ? "Forward" : "Backwards");
            if (forward)
            {
                int startIndex = mBugGazerControl.CurrentIndex + 1;
                for (int i = startIndex; i < mBugGazerControl.Count; i++)
                {
                    string str = mBugGazerControl.GetString(i);
                    if (str.ToLower().Contains(key.ToLower()))
                    {
                        mBugGazerControl.ScrollToIndex(i, true);
                        break;
                    }
                }
            }
            else
            {
                int startIndex = mBugGazerControl.CurrentIndex - 1;
                for (int i = startIndex; i > 0; i--)
                {
                    string str = mBugGazerControl.GetString(i);
                    if (str.ToLower().Contains(key.ToLower()))
                    {
                        mBugGazerControl.ScrollToIndex(i, true);
                        break;
                    }
                }
            }
        }

    }
}
