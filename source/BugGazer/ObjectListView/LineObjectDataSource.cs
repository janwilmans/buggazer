using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightIdeasSoftware;
using System.Windows.Forms;

namespace BugGazer
{
    public class LineObjectDataSource : AbstractVirtualListDataSource
    {
        IStorage<StoredLine> mStorage;

        public LineObjectDataSource(VirtualObjectListView objectListview, IStorage<StoredLine> storage)
            : base(objectListview)
        {
            mStorage = storage;
        }

        public override object GetNthObject(int n)
        {
            if (n >= mStorage.Count)
            {
                MessageBox.Show("index not found!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            StoredLine storedLine = mStorage[n];
            DisplayLine line = new DisplayLine(storedLine.Ticks, storedLine.Pid, mStorage.GetString(n));
            line.Index = n;
            return line;
        }

        public void Add(long ticks, int pid, string message) 
        {
            StoredLine displayLine;
            displayLine.Ticks = ticks;
            displayLine.Pid = pid;
            mStorage.Add(displayLine, message);
        }

        public override int GetObjectCount()
        {
            return mStorage.Count;
        }

        public void Clear()
        {
            mStorage.Clear();
        }
    }

    public class DisplayLine
    {
        static Session mSession = new Session();
        public int Index;
        public long Ticks;
        public int Pid;
        public UTF8String Message;

        public DisplayLine(long ticks, int pid, string message)
        {
            Ticks = ticks;
            Pid = pid;
            Message = message;
        }

        public string Process
        {
            get
            {
                return mSession.GetProcessName(Pid);
            }
        }

        public string Timestamp
        {
            get
            {
                return mSession.GetTimestamp(Ticks);
            }
        }
    }
}
