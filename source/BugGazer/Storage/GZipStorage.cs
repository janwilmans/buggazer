using System.Collections.Generic;
using System;

namespace BugGazer
{
    public class GZipStorage<T> : IStorage<T>
    {
        List<T> mList = new List<T>();
        GZipBlocks mStrings = new GZipBlocks();

        public GZipStorage()
        {
            Controller.WriteLine("Using GZipBlocksStorage");
        }

        #region IStorage Members

        public int Count
        {
            get { return mList.Count; }
        }

        public int Add(T t, string s)
        {
            int index = mList.Count;
            mList.Add(t);
            mStrings.Add(s);
            return index;
        }

        public T this[int index]
        {
            get
            {
                return mList[index];
            }
            set
            {
                mList[index] = value;
            }
        }

        public string GetString(int index)
        {
            return mStrings[index];
        }

        public void SetString(int index, string s)
        {
            throw new Exception("Not Implemented!");
        }

        public void Clear()
        {
            mList.Clear();
            mStrings.Clear();
        }

        #endregion
    }

}