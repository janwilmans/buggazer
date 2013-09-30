using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

// prerequisites:
// - .dot 2.0/3.5 runtime

// features: 
// - modurate memory usage, comparable to DebugView even through BugGazer is .net based
// - can catch both Win32 and Global Win32 outputdebugstring messages
// - little, difficult to measure, delay of the traced application
// - responsive UI even with 100ths of incoming messages/second

// limitations:
// - max 2^32 buffered lines per session (or out of memory, which ever occurs first :)
// - 

// Tested logging consisted of lines that are 90% below 100 chars in length

// look at : http://logging.apache.org/log4j/1.2/
// 

//- Press '-' to add an exclude filter with the exact text of the line
//x Capture Global Win32
//x [pid] na processnaam
//x F2 to find previous
//x measure performance / create unittest
//- selected line on XP is darkblue+black (unreadable)
//- selected line should always be darkblue background+ white foreground, also on colorized lines (now colorization takes presedence)
//x bookmarks ctrl-1 set bookbar, '1' jump to bookmark

//x Memory usage indicator (50% green = normalized memory usage, overlay with red = current memory usage
//- blinking dot (trigger dot, incoming trace lines)
//x implement RLE/trie inspired string compression. (Google Snappy)
//- first try to intern the entire string? (not done: measure performance, not applicable, cannot determine whether inline succeeds)
//-

// todo features:
// - option to filter empty lines from the input
// - option to split messages that contain new characters ('\n' or '\r')
// - 

//http://stackoverflow.com/questions/5055015/issue-capturing-global-session-0-outputdebugstring-messages-via-win32-api
//http://research.microsoft.com/en-us/projects/bio/mbf.aspx
//http://social.msdn.microsoft.com/Forums/en-US/f743bc52-0281-4c2f-8971-97b207b57cf7/merge-strings-and-remove-overlap

//http://msdn.microsoft.com/en-us/library/system.string.intersect(v=vs.90).aspx

namespace BugGazer
{
    public partial class MainForm : Form
    {
        const int ScreenUpdateTime = 1000;   // update screen every 100ms
        const int GetLinesTimeout = 5;      // maximum time to wait for a lock 5ms
        const Keys HoldToAutoscrollKey = Keys.Z;
        const Keys PressToStopAllScrolling = Keys.Escape;

        // move into controller
        Dictionary<int, int> mBookmarks = new Dictionary<int, int>();

        System.Windows.Forms.Timer mUpdateTimer = new System.Windows.Forms.Timer();
        Settings mSettings;
        Controller mController;
        IBugGazerControl mBugGazerControl;
        IBugGazerControl mBugGazerControl2;

        IStorage<StoredLine> mStorage = new SnappyStorage<StoredLine>();

        public MainForm()
        {
            mBugGazerControl = new DBWinListView(mStorage);       // working normal + virtual
            //mBugGazerControl = new DBWinObjectListView(mStorage);   // working normal + virtual
            mController = new Controller(mBugGazerControl);

            //this.Controls.Add(mBugGazerControl);
            InitializeComponent();
            this.Controls.SetChildIndex(searchBar, 0);  // searchbar on top

            DockContent content = (DockContent)mBugGazerControl;
            content.Text = "PID: 123";
            content.Show(this.dockPanel1);

            mBugGazerControl2 = new DBWinObjectListView(mStorage);
            DockContent content2 = (DockContent)mBugGazerControl2;
            content2.Text = "Feibbox.exe";
            content2.Show(this.dockPanel1);
            Resize += new EventHandler(MainForm_Resize);
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        public void Initialize(Settings settings)
        {
            mSettings = settings;
            searchBar.Initialize(mController);

            ActivateSettings();

            KeyPreview = true;
            KeyDown += new KeyEventHandler(Main_KeyDown);
            KeyUp += new KeyEventHandler(Main_KeyUp);

            mUpdateTimer.Interval = ScreenUpdateTime;
            mUpdateTimer.Tick += OnUpdateTimerTick;
            mUpdateTimer.Start();
        }

        void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (autoScrollMenuItem.Checked == false)
            {
                if ((e.KeyCode & HoldToAutoscrollKey) == HoldToAutoscrollKey)
                {
                    mBugGazerControl.AutoScrollDown = true;
                    mBugGazerControl.ScrollDownNow();
                }
            }
            if ((e.KeyCode & PressToStopAllScrolling) == PressToStopAllScrolling)
            {
                autoScrollMenuItem.Checked = false;
                mBugGazerControl.AutoScrollDown = false;
            }

            // CTRL-F Search
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        searchBar.FocusControl();
                        break;
                    default:
                        break;
                }
            }

            // Forward Enter-Key to the search control
            if ((e.KeyCode & Keys.Enter) == Keys.Enter)
            {
                searchBar.SearchTextBox_KeyDown(sender, e);
            }

            // BOOKMARKS
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        mBookmarks[1] = mBugGazerControl.CurrentIndex;
                        break;
                    case Keys.D2:
                        mBookmarks[2] = mBugGazerControl.CurrentIndex;
                        break;
                    case Keys.D3:
                        mBookmarks[3] = mBugGazerControl.CurrentIndex;
                        break;
                    case Keys.D4:
                        mBookmarks[4] = mBugGazerControl.CurrentIndex;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        if (mBookmarks.ContainsKey(1))
                        {
                            mBugGazerControl.ScrollToIndex(mBookmarks[1], true);
                        }
                        break;
                    case Keys.D2:
                        if (mBookmarks.ContainsKey(2))
                        {
                            mBugGazerControl.ScrollToIndex(mBookmarks[2], true);
                        }
                        break;
                    case Keys.D3:
                        if (mBookmarks.ContainsKey(3))
                        {
                            mBugGazerControl.ScrollToIndex(mBookmarks[3], true);
                        }
                        break;
                    case Keys.D4:
                        if (mBookmarks.ContainsKey(4))
                        {
                            mBugGazerControl.ScrollToIndex(mBookmarks[4], true);
                        }
                        break;
                    default:
                        break;
                }
            }

            // F12 (hide menu)
            if (e.KeyCode == Keys.F12)
            {
                menuStrip.Visible = !menuStrip.Visible;
            }
        }

        void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (autoScrollMenuItem.Checked == false)
            {
                if ((e.KeyCode & HoldToAutoscrollKey) == HoldToAutoscrollKey)
                {
                    mBugGazerControl.AutoScrollDown = false;
                }
            }
        }

        private void ActivateSettings()
        {
            captureWin32MenuItem.Checked = mSettings.CaptureWin32;
            captureWin32GlobalMenuItem.Checked = mSettings.CaptureGlobalWin32;
            showGridLinesMenuItem.Checked = mSettings.ShowGridLines;
            autoScrollMenuItem.Checked = mSettings.AutoScrollDown;
            resolveProcessNameMenuItem.Checked = mSettings.ResolveProcessName;
            resolveBrickBoxesToolStripMenuItem.Checked = mSettings.ResolveBrickboxName;

            // activate settings
            if (captureWin32MenuItem.Checked)
            {
                captureWin32MenuItem.Checked = mController.EnableWin32(true);
                if (!captureWin32MenuItem.Checked)
                {
                    mSettings.CaptureWin32 = false;         // disable the setting if capture fails
                }
            }

            if (captureWin32GlobalMenuItem.Checked)
            {
                captureWin32GlobalMenuItem.Checked = mController.EnableWin32Global(true);
                if (!captureWin32GlobalMenuItem.Checked)
                {
                    mSettings.CaptureGlobalWin32 = false;   // disable the setting if capture fails
                }
            }

            mBugGazerControl.AutoScrollDown = autoScrollMenuItem.Checked;
            mBugGazerControl.GridLines = showGridLinesMenuItem.Checked;
            mBugGazerControl.AutoScrollDown = autoScrollMenuItem.Checked;

            mSettings.SerializeToFile();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void enableSoundsMenuItem_Click(object sender, EventArgs e)
        {
            mController.EnableSound(enableSoundsMenuItem.Checked);
        }

        private void captureWin32MenuItem_Click(object sender, EventArgs e)
        {
            captureWin32MenuItem.Checked = mController.EnableWin32(!captureWin32MenuItem.Checked);
            mSettings.CaptureWin32 = captureWin32MenuItem.Checked;
            mSettings.SerializeToFile();
        }

        private void captureWin32GlobalMenuItem_Click(object sender, EventArgs e)
        {
            bool beforeState = captureWin32GlobalMenuItem.Checked;
            captureWin32GlobalMenuItem.Checked = mController.EnableWin32Global(!captureWin32GlobalMenuItem.Checked);
            if (captureWin32GlobalMenuItem.Checked != beforeState)
            {
                mSettings.CaptureGlobalWin32 = captureWin32MenuItem.Checked;
                mSettings.SerializeToFile();
            }
        }

        private void OnUpdateTimerTick(object sender, EventArgs e)
        {
            var list = mController.ListenerWin32.GetLines(GetLinesTimeout);
            if (list != null && list.Count > 0)
            {
                mBugGazerControl.Add(list);
                if (mBugGazerControl2 != null)  mBugGazerControl2.Add(list);
            }
            list = mController.ListenerGlobalWin32.GetLines(GetLinesTimeout);
            if (list != null && list.Count > 0)
            {
                mBugGazerControl.Add(list);
                if (mBugGazerControl2 != null) mBugGazerControl2.Add(list);
            }
            UpdateMemoryUsage();

        }

        void UpdateMemoryUsage()
        {
            double mem = GC.GetTotalMemory(true);
            memUsageLabel.Text = string.Format("Managed usage: {0:0.00} MB", mem / (1024.0 * 1024.0));
        }

        private int mTestIndex = 0;
        private void testMemoryUsageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.WriteLine("Test class: '{0}'", mBugGazerControl.ToString());
            Controller.WriteLine("Add 16 MB of data to the buffer");

            int length = 1024;  // create string of exactly 1024 bytes

            string test = "1234567890ABCDEF-" + length + "-";
            while (test.Length < length - 1)
            {
                test += "X";
            }
            test += "\n";

            GC.Collect();
            GC.Collect();
            double initial = GC.GetTotalMemory(true);

            Controller.WriteLine("Initial memory usage: {0:0.00}MB", initial / (1024.0 * 1024.0));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Line> lines = new List<Line>();
            {
                for (int j = 0; j < 16 * 1024; j++)
                {
                    Line line = new Line();
                    line.Pid = 1;
                    line.Ticks = DateTime.Now.Ticks;
                    line.Message = string.Format("{0} = {1}", j, test);
                    line.Message.Remove(length);
                    //line.Message = test;
                    lines.Add(line);
                   
                    mTestIndex++;
                }
            }
            mBugGazerControl.Add(lines);
            lines.Clear();

            stopwatch.Stop();

            Controller.WriteLine("Time elapsed: {0} ms ", stopwatch.ElapsedMilliseconds);

            GC.Collect();
            GC.Collect();
            double after = GC.GetTotalMemory(true);

            double memoryInUse = after - initial;
            double oneObjectSize = length + 8 + 8 + 8; // data+index+pid+timestamp
            double rawSize = oneObjectSize * mTestIndex;
            double overhead = memoryInUse - rawSize;

            double bytesPerObject = memoryInUse / mTestIndex;
            double ratio = bytesPerObject / oneObjectSize;
            Controller.WriteLine("{0} objects, {1:0.0} bytes/object ({2}) ({3:0.00}x) = {4:0.00}MB, Overhead: {5:0.00}MB",
                    mTestIndex, bytesPerObject, oneObjectSize, ratio, memoryInUse / (1024.0 * 1024.0), overhead / (1024.0 * 1024.0));
            Thread.Sleep(1000);
        }

        private void addAFewTestLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                List<Line> lines = new List<Line>();
                for (int i = 0; i < 16; i++)
                {
                    Line line = new Line();
                    line.Pid = 1;
                    line.Ticks = DateTime.Now.Ticks;
                    line.Message = " Test Line...." + mTestIndex + " followed by some text";
                    lines.Add(line);
                    mTestIndex++;
                }
                mBugGazerControl.Add(lines);
                lines.Clear();
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    Trace.WriteLine(" Test Line...." + mTestIndex);
                    mTestIndex++;
                }
            }
        }

        private void showGridLinesMenuItem_Click(object sender, EventArgs e)
        {
            mBugGazerControl.GridLines = showGridLinesMenuItem.Checked;
            mSettings.ShowGridLines = showGridLinesMenuItem.Checked;
            mSettings.SerializeToFile();
        }

        private void autoScrollMenuItem_Click(object sender, EventArgs e)
        {
            mBugGazerControl.AutoScrollDown = autoScrollMenuItem.Checked;
            if (mBugGazerControl2 != null) mBugGazerControl2.AutoScrollDown = autoScrollMenuItem.Checked;
            mSettings.AutoScrollDown = autoScrollMenuItem.Checked;
            mSettings.SerializeToFile();
        }

        private void clearBufferMenuItem_Click(object sender, EventArgs e)
        {
            mBugGazerControl.Clear();
            GC.Collect();
            GC.Collect();
        }

        private void copyLineMenuItem_Click(object sender, EventArgs e)
        {
            mBugGazerControl.CopySelectedToClipboard();
        }

        private void openConfigDirectoryMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("\"" + Path.GetDirectoryName(Settings.ConfigurationFilePath) + "\"");
        }

        private void openDebugConsoleMenuItem_Click(object sender, EventArgs e)
        {
            if (!openDebugConsoleMenuItem.Checked)
            {
                mController.OpenConsole();
                openDebugConsoleMenuItem.Checked = true;
            }
            else
            {
                MessageBox.Show("The Debug Console cannot be closed, you wanted it, you got it :)",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void findPreviousMenuItem_Click(object sender, EventArgs e)
        {
            searchBar.SearchPrevious();
        }

        private void findNextMenuItem_Click(object sender, EventArgs e)
        {
            searchBar.SearchNext();
        }

        private void resolveProcessNameMenuItem_Click(object sender, EventArgs e)
        {
            //mBugGazerControl.ResolveProcessName = resolveProcessNameMenuItem.Checked;
            //mSettings.ResolveProcessName = resolveProcessNameMenuItem.Checked;
            //mSettings.SerializeToFile();
        }

        private void resolveBrickBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mBugGazerControl.ResolveBrickboxName = resolveProcessNameMenuItem.Checked;
            //mSettings.ResolveProcessName = resolveProcessNameMenuItem.Checked;
            //mSettings.SerializeToFile();
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBugGazerControl.DumpStatistics();
        }

        private void filterMenu_Click(object sender, EventArgs e)
        {
            Filters filters = new Filters();
            filters.Show();
        }

        /*
        // * really bad on windows8 when visual styles are off
        // * 100% cpu on winXP? when _does_ this work?
        // I suspect this method is incompatible with using VirtualMode listviews
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // WinXP = 5.1, WinXP 64 bit = 5.2
                // Win7 = 6.1, Win7 = 6.2
                if (Environment.OSVersion.Version.Major < 6)
                {
                    cp.ExStyle |= 0x02000000;   //WS_CLIPCHILDREN
                }
                return cp;
            }
        }
        */
        
    }
}
