using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;   // for snappy c++ lib

namespace BugGazer
{
    // This uses Google Snappy LZ77 based code (https://code.google.com/p/snappy/)
    // its does not seem to compress repeated bytes well.
    // 16367	19:09:14.216	[1]	16367 = 1234567890ABCDEF-1024-XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // the ratio for 16kx above test string is 121%

    public class SnappyStorageByLine<T> : IStorage<T>
    {
        List<T> mList = new List<T>();
        List<byte[]> mStrings = new List<byte[]>();
        SnappyCompressor mSnappy = new SnappyCompressor();

        public SnappyStorageByLine()
        {
            Controller.WriteLine("Using SnappyStorageByLine");
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
            mStrings.Add(mSnappy.Compress(s));
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
            return mSnappy.Decompress(mStrings[index]);
        }

        public void SetString(int index, string s)
        {
            mStrings[index] = mSnappy.Compress(s);
        }

        public void Clear()
        {
            mList.Clear();
            mStrings.Clear();
        }

        #endregion
    }


}

