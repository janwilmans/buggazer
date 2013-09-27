using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BugGazer
{
    public interface IDataControl
    {
        void FilterAndDisplay(IList<Line> dataMap);
        Control GetControl();
        void ShowGridLines(bool value);

        // test method
        void BeginUpdate();
        void AddTestLine(Line line);
        void EndUpdate();
    }
}
