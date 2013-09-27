using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

    //todo: a high thread priority is probably not a good idea even for a DBWIN_BUFFER monitor
    // but try it to measure the effects are in favor of the performance of the traced process
    //// set monitor thread's priority to highest
    //// ---------------------------------------------------------
    //bSuccessful = ::SetPriorityClass(
    //    ::GetCurrentProcess(),
    //    REALTIME_PRIORITY_CLASS
    //    );

    //bSuccessful = ::SetThreadPriority(
    //    m_hWinDebugMonitorThread,
    //    THREAD_PRIORITY_TIME_CRITICAL
    //    );

    // maybe integrate this somehow:
    // http://stackoverflow.com/questions/4429254/how-to-make-debugview-work-under-net-4

namespace BugGazer
{
    public class Line
    {
        public long Ticks;
        public int Pid;
        public string Message;
    }

    public class DBWinListener
    {
        //types 
        public enum ListenerType { DbWin32, GlobalDbWin32 };

        // class members
        private ListenerType mListenerType;
        private string mBufferReadyName;
        private string mDataReadyName;
        private string mBufferName;
        private bool mStop = false;
        private Thread mListenerThread;

        private object mLineMapLock = new object();
        private List<Line> mList = new List<Line>();
        private List<Line> mListB = new List<Line>();

        // native interaction
        private IntPtr mBufferReadyEvent;
        private IntPtr mDataReadyEvent;
        private IntPtr mMapping;
        private IntPtr mFile;
        private const int ReadTimeout = 500; //ms

        public DBWinListener(ListenerType type)
        {
            mListenerType = type;
            switch (mListenerType)
            {
                case ListenerType.DbWin32:
                    mBufferReadyName = "DBWIN_BUFFER_READY";
                    mDataReadyName = "DBWIN_DATA_READY";
                    mBufferName = "DBWIN_BUFFER";
                    break;
                case ListenerType.GlobalDbWin32:
                    mBufferReadyName = "Global\\DBWIN_BUFFER_READY";
                    mDataReadyName = "Global\\DBWIN_DATA_READY";
                    mBufferName = "Global\\DBWIN_BUFFER";
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }

        public bool Initialize()
        {
            bool permission = true;
            SECURITY_DESCRIPTOR sd = new SECURITY_DESCRIPTOR();
            // Initialize the security descriptor.
            if (!InitializeSecurityDescriptor(ref sd, SECURITY_DESCRIPTOR_REVISION))
            {
                Controller.WriteLine("Failed to Initialize Security Descriptor!");
                permission = false;
            }

            // Set information in a discretionary access control list
            if (!SetSecurityDescriptorDacl(ref sd, true, IntPtr.Zero, false))
            {
                Controller.WriteLine("Failed to Set Security Descriptor Dacl!");
                permission = false;
            }

            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.nLength = Marshal.SizeOf(sa);
            sa.lpSecurityDescriptor = Marshal.AllocHGlobal(Marshal.SizeOf(sd));
            Marshal.StructureToPtr(sd, sa.lpSecurityDescriptor, false);

            mBufferReadyEvent = CreateEvent(ref sa, false, false, mBufferReadyName);
            if (mBufferReadyEvent == IntPtr.Zero)
            {
                int hr = Marshal.GetLastWin32Error();
                Controller.WriteLine("failed to access {0}, hr = {1}", mBufferReadyName, hr);
                permission = false;
            }

            mDataReadyEvent = CreateEvent(ref sa, false, false, mDataReadyName);
            if (mDataReadyEvent == IntPtr.Zero)
            {
                int hr = Marshal.GetLastWin32Error();
                Controller.WriteLine("failed to access {0}, hr = {1}", mDataReadyName, hr);
                permission = false;
            }

            mMapping = CreateFileMapping(new IntPtr(-1), ref sa, PageProtection.ReadWrite, 0, 4096, mBufferName);
            if (mMapping == IntPtr.Zero)
            {
                int hr = Marshal.GetLastWin32Error();
                Controller.WriteLine("failed to access {0}, hr = {1}", mBufferName, hr);
                permission = false;
            }

            mFile = MapViewOfFile(mMapping, FileMapAccess.FileMapRead, 0, 0, 1024);
            if (mFile == IntPtr.Zero)
            {
                int hr = Marshal.GetLastWin32Error();
                Controller.WriteLine("failed to access MapViewOfFile, hr = {0}", hr);
                permission = false;
            }

            if (permission)
            {
                Clear();
            }
            return permission;
        }

        public void StartListening()
        {
            mStop = false;
            mListenerThread = new Thread(Listen);
            mListenerThread.IsBackground = true;
            mListenerThread.Start();
        }

        public void StopListening()
        {
            mStop = true;
            if (mListenerThread != null)
            {
                mListenerThread.Join();
            }
        }

        public void Clear()
        {
            mList.Clear();
        }

        private void Listen()
        {
            try
            {
                do
                {
                    if (mStop)
                    {
                        Controller.WriteLine("Thread stop was requested!");
                        return;
                    }

                    // any buffer will be trimmed to 4091 characters! (tested)
                    SetEvent(mBufferReadyEvent);
                    uint wait = WaitForSingleObject(mDataReadyEvent, ReadTimeout);
                    if (wait == WAIT_OBJECT_0) // we don't care about other return values
                    {
                        int pid = Marshal.ReadInt32(mFile);
                        string text = Marshal.PtrToStringAnsi(new IntPtr(mFile.ToInt32() + Marshal.SizeOf(typeof(int)))).TrimEnd(null);
                        //if ((string.IsNullOrEmpty(text)) && (mRemoveEmptyLines)) continue;

                        // todo: allocation of new objects for each line might get expensive over time:
                        // - remember that the traced application is blocked by anything that happens in this loop
                        // - allocation of new chuncks of memory may be too expensive to do here
                        // - the Line objects will not be released until the entire buffer is released, 
                        //   so memory fragmentation might be limited.

                        Line line = new Line();
                        line.Ticks = DateTime.Now.Ticks;
                        line.Pid = pid;
                        line.Message = text; // todo: use Line.Assign here

                        //if (line.Message.Length > 1000)
                        //{
                        //    Controller.WriteLine("long string of {0} chars!", line.Message.Length);
                        //    Controller.WriteLine("{0}", line.Message);
                        //}

                        //Controller.WriteLine("pid: {0} at {1}: '{2}'", pid, line.Ticks, line.Message);
                        lock (mLineMapLock)
                        {
                            mList.Add(line);
                        }
                    }
                }
                while (true);
            }
            catch (Exception e)
            {
                Controller.WriteLine("Read Exception: {0} ", e);
            }
        }

        public IList<Line> GetLines(int milliseconds)
        {
            // we use two collections here for two reasons:
            // - we can keep the lock internal an dthe caller is not concerned with it
            // - processing operations of the caller do not block incoming items from Listen()
            List<Line> result = null;
            if (Monitor.TryEnter(mLineMapLock, milliseconds))
            {
                try
                {
                    // swap the queues and release the lock asap
                    List<Line> lineList = mList;
                    mList = mListB;
                    mList.Clear();
                    mListB = lineList;
                    result = lineList;
                }
                finally
                {
                    Monitor.Exit(mLineMapLock);
                }
            }
            return result;
        }
 
        public void Uninitialize()
        {
            if (mBufferReadyEvent != IntPtr.Zero)
            {
                CloseHandle(mBufferReadyEvent);
                mBufferReadyEvent = IntPtr.Zero;
            }

            if (mDataReadyEvent != IntPtr.Zero)
            {
                CloseHandle(mDataReadyEvent);
                mDataReadyEvent = IntPtr.Zero;
            }

            if (mFile != IntPtr.Zero)
            {
                UnmapViewOfFile(mFile);
                mFile = IntPtr.Zero;
            }

            if (mMapping != IntPtr.Zero)
            {
                CloseHandle(mMapping);
                mMapping = IntPtr.Zero;
            }
        }
        static Dictionary<int, string> mProcessMap = new Dictionary<int, string>();     //todo: what about when a processId is re-used by the system?

        #region Win32 API Imports

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_DESCRIPTOR
        {
            public byte revision;
            public byte size;
            public short control;
            public IntPtr owner;
            public IntPtr group;
            public IntPtr sacl;
            public IntPtr dacl;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [Flags]
        private enum PageProtection : uint
        {
            NoAccess = 0x01,
            Readonly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            Guard = 0x100,
            NoCache = 0x200,
            WriteCombine = 0x400,
        }

        [Flags]
        private enum FileMapAccess
        {
            FileMapCopy = 0x0001,
            FileMapWrite = 0x0002,
            FileMapRead = 0x0004,
            FileMapAllAccess = 0x001f,
            FileMapExecute = 0x0020,
        }

        private const int WAIT_OBJECT_0 = 0;
        private const uint SECURITY_DESCRIPTOR_REVISION = 1;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, FileMapAccess dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool InitializeSecurityDescriptor(ref SECURITY_DESCRIPTOR sd, uint dwRevision);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetSecurityDescriptorDacl(ref SECURITY_DESCRIPTOR sd, bool daclPresent, IntPtr dacl, bool daclDefaulted);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(ref SECURITY_ATTRIBUTES sa, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll")]
        private static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile,
            ref SECURITY_ATTRIBUTES lpFileMappingAttributes, PageProtection flProtect, uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        private static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);
        #endregion
    }
}