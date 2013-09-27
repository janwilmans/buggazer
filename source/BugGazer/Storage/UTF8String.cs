using System.Text;

namespace BugGazer
{
    // We're using struct here to prevent the overhead associated with a class (12 bytes per object in x86 mode, 24 bytes in x64 mode)
    // This difference is very notable when storing many MB's of strings.
    public struct UTF8String
    {
        // remember structs are always copied by value
        // but since the only member is a pointer, this should not pose a problem
        byte[] buffer;

        public UTF8String(string s)
        {
            buffer = Encoding.UTF8.GetBytes(s);
        }

        // User-defined conversion from UTF8String to string 
        public static implicit operator string(UTF8String s)
        {
            return Encoding.UTF8.GetString(s.buffer, 0, s.buffer.Length);
        }
        //  User-defined conversion from string to UTF8String 
        public static implicit operator UTF8String(string s)
        {
            return new UTF8String(s);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        public string SubString(int startIndex, int length)
        {
            return Encoding.UTF8.GetString(buffer, startIndex, length);
        }
    }
}
