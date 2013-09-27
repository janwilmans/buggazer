using System.Collections.Generic;

namespace BugGazer
{
    public class UTF8Storage<T> : IStorage<T>
    {
        List<T> mList = new List<T>();
        List<UTF8String> mStrings = new List<UTF8String>();

        public UTF8Storage()
        {
            Controller.WriteLine("Using UTF8Storage");
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
            mStrings[index] = s;
        }

        public void Clear()
        {
            mList.Clear();
            mStrings.Clear();
        }

        #endregion
    }

}