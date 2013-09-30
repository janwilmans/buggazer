using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;      // used by .OrderBy
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Drawing2D;

namespace BugGazer
{
    public partial class DBWinListView : DockContent, IBugGazerControl
    {
        int mLastUpdatedItemIndex;
        Session mSession = new Session();
        ListViewItem mCachedItem;
        IStorage<StoredLine> mStorage;

        // this UserControl actually never shows, it is a container to allow the listview to be designed 
        public DBWinListView(IStorage<StoredLine> storage)
        {
            mStorage = storage;
            InitializeComponent();
            MyExtensions.SetDoubleBuffered(this.listView, true);

            listView.DrawSubItem += OnListViewDrawSubItem;
            listView.DrawColumnHeader += listView_DrawColumnHeader;
            listView.DrawItem += listView_DrawItem;

            Controller.WriteLine("Using C# ListView in VirtualMode.");
            this.listView.VirtualMode = true;
            this.listView.RetrieveVirtualItem += RetrieveVirtualItem;
        }

        #region IBugGazerControl interface

        public bool GridLines
        {
            set
            {
                listView.GridLines = value;
            }
        }

        public bool AutoScrollDown { get; set; }

        #endregion

        #region IBugGazerControl interface

        private void StoreLine(Line line)
        {
            StoredLine displayLine;
            displayLine.Ticks = line.Ticks;
            displayLine.Pid = line.Pid;
            mStorage.Add(displayLine, line.Message);
        }

        private void RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (mCachedItem == null || mLastUpdatedItemIndex != e.ItemIndex)
            {
                e.Item = new ListViewItem(e.ItemIndex.ToString());
                mCachedItem = e.Item;
                StoredLine line = mStorage[e.ItemIndex];
                e.Item.SubItems.Add(mSession.GetTimestamp(line.Ticks));
                e.Item.SubItems.Add(mSession.GetProcessName(line.Pid) ?? line.Pid.ToString());
                e.Item.SubItems.Add(mStorage.GetString(e.ItemIndex));
                mLastUpdatedItemIndex = e.ItemIndex;
            }
            else
            {
                // we cache the listviewitem because the listview requests the same item very often!?
                e.Item = mCachedItem;
            }
        }

        public void CreateItem(Line line)
        {
            int index = listView.Items.Count;
            ListViewItem item = new ListViewItem(index.ToString());
            item.SubItems.Add(mSession.GetTimestamp(line.Ticks));
            item.SubItems.Add(mSession.GetProcessName(line.Pid) ?? line.Pid.ToString());
            item.SubItems.Add(line.Message);
            listView.Items.Add(item);
        }

        public void Add(IList<Line> lines)
        {
            // in virtual mode add the lines to storage
            foreach (Line line in lines)
            {
                StoreLine(line);
            }

            listView.SetVirtualListSize(mStorage.Count);
            if (AutoScrollDown)
            {
                ScrollDownNow();
            }
        }

        public void ScrollDownNow()
        {
            int lastIndex = listView.Items.Count - 1;
            ScrollToIndex(lastIndex, false);
        }

        public void Clear()
        {
            mLastUpdatedItemIndex = -1;
            listView.BeginUpdate();
            
            listView.VirtualListSize = 0;
            mStorage.Clear();
            mSession.Clear();
            listView.EndUpdate();
        }

        public void CopySelectedToClipboard()
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();

            Controller.WriteLine("~CopySelectedToClipboard from virtual listview");
            // in virtual mode we must re-format to make sure off-screen items are copied also.
            foreach (int index in listView.SelectedIndices)
            {
                StoredLine line = mStorage[index];
                sb.Append(index);
                sb.Append('\t');
                sb.Append(mSession.GetTimestamp(line.Ticks));
                sb.Append('\t');
                sb.Append(mSession.GetProcessName(line.Pid));
                sb.Append('\t');
                sb.Append(mStorage.GetString(index).TrimEnd());
                sb.Append("\r\n");
                count++;
            }

            if (count > 0)
            {
                Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
            }
        }

        public int CurrentIndex
        {
            get 
            {
                if (listView.SelectedIndices.Count > 0)
                {
                    return listView.SelectedIndices[0];
                }
                return 0;
            }
        }

        public void ScrollToIndex(int index, bool center)
        {
            if (index < 0) return;
            if (index >= mStorage.Count)
            {
                Controller.WriteLine("ScrollToIndex: {0} index not found!", index);
                return;
            }

            foreach (int i in listView.SelectedIndices)
            {
                listView.Items[i].Selected = false;
            }
            listView.SelectedIndices.Add(index);

            // make sure EnsureVisible is _not_ called inside BeginUpdate / EndUpdate 
            // because the will make it flicker on WindowsXP

            listView.EnsureVisible(index);  // the item is now in the view
            // depending on what the previous index was, it will now be either the top-most of bottom-most item.

            if (center)
            {
                int maxExtraItems = listView.GetVisibleItemCount() / 2;
                int maxBottomIndex = Math.Min(mStorage.Count - 1, index + maxExtraItems);
                listView.EnsureVisible(maxBottomIndex);
            }
            listView.Focus();
        }

        public string GetString(int index)
        {
            return mStorage.GetString(index);
        }

        public int Count
        {
            get
            {
                return mStorage.Count;
            }
        }

        public void DumpStatistics()
        {
            var stats = new Dictionary<int, int>();
            for (int i=0; i< mStorage.Count; i++)
            {
                string s = mStorage.GetString(i);
                if (stats.ContainsKey(s.Length))
                {
                    stats[s.Length] = stats[s.Length] + 1;
                }
                else
                {
                    stats[s.Length] = 1;
                }
            }

            var sorted = stats.OrderBy(item => item.Value).ToList();
            foreach (var item in sorted)
            {
                Controller.WriteLine(string.Format("len: {0} count: {1}", item.Key, item.Value));
            }
        }

        #endregion

        // Draws column headers. 
        void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        // Draws the backgrounds for entire ListView items. 
        void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) != ListViewItemStates.Selected) return;
            using (LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, Color.Beige,Color.Red, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }

        private void OnListViewDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            //Controller.WriteLine("e.SubItem.Name: {0}", e.SubItem.Text);

            if (!e.SubItem.Text.Contains("23"))
            {
                e.DrawDefault = true;
                return;
            }

            //e.DrawBackground();
            if ((e.ItemState & ListViewItemStates.Selected) == ListViewItemStates.Selected)
            {
                //
            }

            Rectangle b = new Rectangle(e.Bounds.Left + e.Bounds.Width / 4, e.Bounds.Top, e.Bounds.Width / 2, e.Bounds.Height);
            e.Graphics.FillRectangle(SystemBrushes.Highlight, b);

            e.DrawText(TextFormatFlags.Default);
        }
    }

    // These extentions help to work around listview bugs.
    public static class MyExtensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
        
        // If VirtualListSize is set the scrollposition is affected when the focessed line is not in the view area
        // reproduce: select a line and scroll it out of view, then add some lines to the underlying collection
        // and update VirtualListSize. The listview position will now change. 
        // using SetVirtualListSize instead of setting VirtualListSize directly will prevent this behaviour.
        private static FieldInfo _internalVirtualListSizeField = typeof(ListView).GetField("virtualListSize", System.Reflection.BindingFlags.NonPublic | BindingFlags.Instance);
        public static void SetVirtualListSize(this ListView listView, int size)
        {
            if (size < 0)
            {
                throw new ArgumentException("ListViewVirtualListSizeInvalidArgument");
            }

            _internalVirtualListSizeField.SetValue(listView, size);
            if (listView.IsHandleCreated)
            {
                SendMessage(new HandleRef(listView, listView.Handle), 0x102f, size, 2); // 0x102f = LVM_SETITEMCOUNT, 2 = LVSICF_NOSCROLL
            }
            else
            {
                listView.VirtualListSize = size;
            }
        }

        // this extention sets the private doublebuffered property of a control
        // for listviews this prevents flickering during updates
        // this is only causing more traffic in terminal sessions, 
        // so we do no enable it there.
        public static void SetDoubleBuffered(this Control control, bool enable)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession == false)
            {
                Controller.WriteLine("Using '{0}'", control.ToString());
                Controller.WriteLine(" with SetDoubleBuffered = {0}", enable);
                var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                doubleBufferPropertyInfo.SetValue(control, enable, null);
            }
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static void SetRedraw(this Control target, int value)
        {
            SendMessage(target.Handle, WM_SETREDRAW, value, 0);
        }

        #region LVS_EX
        public enum LVS_EX
        {
            LVS_EX_GRIDLINES = 0x00000001,
            LVS_EX_SUBITEMIMAGES = 0x00000002,
            LVS_EX_CHECKBOXES = 0x00000004,
            LVS_EX_TRACKSELECT = 0x00000008,
            LVS_EX_HEADERDRAGDROP = 0x00000010,
            LVS_EX_FULLROWSELECT = 0x00000020,
            LVS_EX_ONECLICKACTIVATE = 0x00000040,
            LVS_EX_TWOCLICKACTIVATE = 0x00000080,
            LVS_EX_FLATSB = 0x00000100,
            LVS_EX_REGIONAL = 0x00000200,
            LVS_EX_INFOTIP = 0x00000400,
            LVS_EX_UNDERLINEHOT = 0x00000800,
            LVS_EX_UNDERLINECOLD = 0x00001000,
            LVS_EX_MULTIWORKAREAS = 0x00002000,
            LVS_EX_LABELTIP = 0x00004000,
            LVS_EX_BORDERSELECT = 0x00008000,
            LVS_EX_DOUBLEBUFFER = 0x00010000,
            LVS_EX_HIDELABELS = 0x00020000,
            LVS_EX_SINGLEROW = 0x00040000,
            LVS_EX_SNAPTOGRID = 0x00080000,
            LVS_EX_SIMPLESELECT = 0x00100000
        }
        #endregion

        #region LVM
        public enum LVM
        {
            LVM_FIRST = 0x1000,
            LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40),
            LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54),
            LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55),
        }
        #endregion

        public static int GetVisibleItemCount(this Control control)
        {
            return SendMessage(control.Handle, (int)LVM.LVM_GETCOUNTPERPAGE, 0, 0);
        }

        public static void SetExStyles(this Control control, LVS_EX exStyle)
        {
            LVS_EX styles = (LVS_EX)SendMessage(control.Handle, (int)LVM.LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
            styles |= exStyle;
            SendMessage(control.Handle, (int)LVM.LVM_SETEXTENDEDLISTVIEWSTYLE, 0, (int)styles);
        }

        // todo: see BugListView (WM_ERASEBKGND trick)
    }
}
