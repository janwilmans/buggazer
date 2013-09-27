namespace BugGazer
{
    public struct StoredLine
    {
        public long Ticks;
        public int Pid;
    }


    public interface IStorage<T>
    {
        int Count { get; }
        int Add(T line, string s);
        T this[int index] { get; set; }
        string GetString(int index);
        void Clear();
    }
}