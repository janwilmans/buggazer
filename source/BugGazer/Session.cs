using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BugGazer
{
    class Session
    {
        Dictionary<int, string> mProcessMap = new Dictionary<int, string>();   //todo: what about when a processId is re-used by the system?

        public string GetProcessName(int pid)
        {
            string name;
            if (!mProcessMap.TryGetValue(pid, out name))
            {
                try
                {
                    Process process = Process.GetProcessById(pid);
                    if (process != null)
                    {
                        name = pid.ToString() + "," + process.ProcessName;
                    }
                    
                }
                catch (Exception e)
                {
                    Controller.WriteLine("exception: {0}", e);
                }
                finally
                {
                    mProcessMap[pid] = pid.ToString();
                    name = pid.ToString();
                }
            }
            return name;
        }

        public string GetTimestamp(long ticks)
        {
            DateTime time = new DateTime(ticks);
            return time.ToString("HH:mm:ss.fff");
        }

        public void Clear()
        {
            mProcessMap.Clear();
        }
    }
}
