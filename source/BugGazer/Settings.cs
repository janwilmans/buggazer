using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugGazer
{
    public class Settings : Serializable<Settings>
    {
        public bool AutoScrollDown;
        public bool CaptureWin32;
        public bool CaptureGlobalWin32;
        public bool ShowGridLines;
        public bool VisualStyle;
        public bool ResolveProcessName;
        public bool ResolveBrickboxName;
    }
}
