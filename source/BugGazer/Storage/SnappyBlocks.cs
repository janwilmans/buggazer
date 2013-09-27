using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace BugGazer
{
    public class SnappyBlocks
    {
        const int blockSize = 400;
        Dictionary<int, byte[]> mBlocks = new Dictionary<int, byte[]>();
        List<UTF8String> mReadList = new List<UTF8String>();
        List<UTF8String> mWriteList = new List<UTF8String>();
        SnappyCompressor mSnappy = new SnappyCompressor(256 * blockSize);   // optimize for max. 256 chars per line
        int mReadBlockIndex = -1;
        int mWriteBlockIndex = 0;

        // an index is returned that can be used to retrieve the string
        public int Add(string s)
        {
            int id = mWriteList.Count;
            mWriteList.Add(s);
            int result = (mWriteBlockIndex*blockSize) + id;
            if (id == (blockSize-1))
            {
                Controller.WriteLine("CompressStringList for blockid: " + mWriteBlockIndex);
                mBlocks[mWriteBlockIndex] = CompressStringList(mWriteList);
                mWriteBlockIndex++;
            }
            return result;
        }

        private byte[] CompressStringList(List<UTF8String> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
            {
                sb.Append(s);
                sb.Append('\0');
            }
            string prepared = sb.ToString().TrimEnd('\0');
            list.Clear();
            return mSnappy.Compress(prepared);
        }

        private List<UTF8String> DecompressStringList(byte [] archive)
        {
            List<UTF8String> list = new List<UTF8String>();
            string stringList = mSnappy.Decompress(archive);
            foreach (string s in stringList.Split('\0'))
            {
                list.Add(s);
            }
            return list;
        }

        private int GetBlockIndex(int stringId)
        {
            return (stringId / blockSize);
        }

        private int GetId(int stringId)
        {
            return (stringId % blockSize);
        }

        // retrievel operator
        public string this[int stringId] 
        { 
            get
            {
                return GetText(stringId);
            }
        }

        string GetText(int stringId)
        {
            int blockId = GetBlockIndex(stringId);
            int id = GetId(stringId);

            if (blockId == mWriteBlockIndex)
            {
                return mWriteList[id];
            }

            if (mReadBlockIndex != blockId)
            {
                Controller.WriteLine("DecompressStringList for blockid: " + blockId);
                mReadList = DecompressStringList(mBlocks[blockId]);
                mReadBlockIndex = blockId;
            }
            return mReadList[id];
        }

        public void Clear()
        {
            mWriteBlockIndex = 0;
            mReadBlockIndex = -1;
            mBlocks.Clear();
            mReadList.Clear();
            mWriteList.Clear();
        }

        class Archive
        {
            public Archive(byte[] buffer, long length)
            {
                OriginalLength = length;
                Buffer = buffer;
            }
            public long OriginalLength;
            public byte[] Buffer;
        }

        private Archive Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            if (buffer.Length > 300)
            {
                Controller.WriteLine("Using gzip'ed storage for {0} characters", text.Length);
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(buffer, 0, buffer.Length);
                zip.Close();
                ms.Position = 0;
                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);
                Controller.WriteLine("  => compressed into {0} bytes", compressed.Length);
                return new Archive(compressed, buffer.Length);
            }
            return new Archive(buffer, 0);  // store uncompressed
        }

        private string Decompress(Archive archive)
        {
            if (archive.OriginalLength > 0)
            {
                byte[] gzBuffer = archive.Buffer;
                MemoryStream ms = new MemoryStream();
                ms.Write(gzBuffer, 0, gzBuffer.Length);
                ms.Position = 0;
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
                byte[] buffer = new byte[archive.OriginalLength];
                zip.Read(buffer, 0, buffer.Length);
                zip.Close();
                return Encoding.UTF8.GetString(buffer);
            }
            return Encoding.UTF8.GetString(archive.Buffer);
        }
    }
}

