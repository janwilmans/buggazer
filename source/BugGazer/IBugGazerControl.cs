using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BugGazer
{
    public interface IBugGazerControl
    {
        void Add(IList<Line> list);
        int Count { get; }

        // methods
        void Clear();
        void CopySelectedToClipboard();
        void DumpStatistics();

        // user options
        bool GridLines { set; }
        bool AutoScrollDown { set; }
        //bool ResolveProcessName { set; }
        //bool ResolveBrickboxName { set; }

        void ScrollDownNow();
        void ScrollToIndex(int index, bool center);
        int CurrentIndex { get; }
        string GetString(int index);
    }
}
