using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;

namespace BugGazer
{
    public partial class DBWinObjectListView : DockContent, IBugGazerControl
    {
        LineObjectDataSource mDataSource;        // used in virtual mode

        public DBWinObjectListView(IStorage<StoredLine> storage)
        {
            InitializeComponent();
            mDataSource = new LineObjectDataSource(listview, storage);
            listview.VirtualListDataSource = mDataSource;

            /*
            this.columnIndex.AspectToStringConverter = delegate (object x) { 
                return "#" + x.ToString(); 
            };
             */

            Controller.WriteLine("Using ObjectListView in virtual mode.");
            listview.FormatRow +=  new EventHandler<FormatRowEventArgs>(listview_FormatRow);
            //listview.FormatCell += new EventHandler<FormatCellEventArgs>(listview_FormatCell);
        }

        void listview_FormatCell(object sender, FormatCellEventArgs e)
        {
            //if (e.ColumnIndex == 2)
            {
                e.Item.BackColor = Color.Black;
            }
        }

        private void listview_FormatRow(object sender, FormatRowEventArgs e)
        {
            Controller.WriteLine("listview_FormatRow: index: {0}", e.Item.Index);
            if ((e.Item.Index & 1) == 1)
            {
                e.Item.ForeColor = Color.Red;
            }
        }

        #region IBugGazerControl interface

        public bool AutoScrollDown { get; set; }

        public void ScrollDownNow()
        {
            int lastIndex = listview.Items.Count - 1;
            ScrollToIndex(lastIndex, false);
        }

        public bool GridLines
        {
            set
            {
                listview.GridLines = value;
            }
        }

        public void CopySelectedToClipboard()
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();

            // in the display items are created in formatted form from the virtual data source
            foreach (int index in listview.SelectedIndices)
            {
                DisplayLine line = (DisplayLine)mDataSource.GetNthObject(index);
                sb.Append(index);
                sb.Append('\t');
                sb.Append(line.Ticks);
                sb.Append('\t');
                sb.Append(line.Process);
                sb.Append('\t');
                sb.AppendLine(line.Message);
                count++;
            }

            if (count > 0)
            {
                Clipboard.SetText(sb.ToString().TrimEnd('\t'), TextDataFormat.Text);
            }
        }

        public void Clear()
        {
            listview.BeginUpdate();
            mDataSource.Clear();
            listview.VirtualListSize = 0;
            listview.EndUpdate();
        }

        public string GetString(int index)
        {
            throw new Exception("Not implemented");
        }

        #endregion

        public void Add(IList<Line> list)
        {
            foreach(Line line in list)
            {
                mDataSource.Add(line.Ticks, line.Pid, line.Message);
            }
            listview.VirtualListSize = mDataSource.GetObjectCount();
            if (AutoScrollDown)
            {
                ScrollDownNow();
            }
        }

        public void ScrollToIndex(int index, bool center)
        {
            if (index >= mDataSource.GetObjectCount())
            {
                Controller.WriteLine("ScrollToIndex: {0} index not found!", index);
                return;
            }

            foreach (int i in listview.SelectedIndices)
            {
                listview.Items[i].Selected = false;
            }
            listview.SelectedIndices.Add(index);

            // make sure EnsureVisible is _not_ called inside BeginUpdate / EndUpdate 
            // because the will make it flicker on WindowsXP

            listview.EnsureVisible(index);  // the item is now in the view
            // depending on what the previous index was, it will now be either the top-most of bottom-most item.

            if (center)
            {
                int maxExtraItems = listview.GetVisibleItemCount() / 2;
                int maxBottomIndex = Math.Min(mDataSource.GetObjectCount() - 1, index + maxExtraItems);
                listview.EnsureVisible(maxBottomIndex);
            }
            listview.Focus();
        }

        public int Count
        {
            get
            {
                throw new Exception("Not implemented");
            }
        }

        public int CurrentIndex
        {
            get { throw new Exception("Not implemented"); }
        }

        public void DumpStatistics()
        {
            throw new Exception("Not implemented");
        }

    }
}
