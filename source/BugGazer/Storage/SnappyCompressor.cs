using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;   // for snappy c++ lib

namespace BugGazer
{
    namespace SnappyPI
    {
        public enum SnappyStatus
        {
            /// <summary>OK.</summary>
            OK = 0,

            /// <summary>The input is corrupted.</summary>
            InvalidInput = 1,

            /// <summary>Output buffer is invalid.</summary>
            BufferTooSmall = 2,
        }

        /// <summary>P/Invoke wrapper for x86 assembly</summary>
        internal static class Snappy32
        {
            [DllImport("SnappyDL.x86.dll", EntryPoint = "snappy_compress")]
            public static extern unsafe SnappyStatus Compress(
                byte[] input,
                int inputLength,
                byte[] compressed,
                ref int compressedLength);

            [DllImport("SnappyDL.x86.dll", EntryPoint = "snappy_uncompress")]
            public static extern unsafe SnappyStatus Uncompress(
                byte[] compressed,
                int compressedLength,
                byte[] uncompressed,
                ref int uncompressedLength);

            [DllImport("SnappyDL.x86.dll", EntryPoint = "snappy_max_compressed_length")]
            public static extern int GetMaximumCompressedLength(
                int inputLength);

            [DllImport("SnappyDL.x86.dll", EntryPoint = "snappy_uncompressed_length")]
            public static extern unsafe SnappyStatus GetUncompressedLength(
                byte[] compressed,
                int compressedLength,
                ref int result);

            [DllImport("SnappyDL.x86.dll", EntryPoint = "snappy_validate_compressed_buffer")]
            public static extern unsafe SnappyStatus ValidateCompressedBuffer(
                byte[] compressed,
                int compressedLength);
        }
    }

    public class SnappyCompressor
    {
        // these buffers are an attempt to limit memory fragmentation by preventing allocation
        // of buffers for evert Compress / Uncompress call
        // notice that the size of these buffers are not hardcoded limits, if larger buffers are needed they will still be allocated.
        byte[] mCompressedBuffer;
        byte[] mUncompressedBuffer;

        public SnappyCompressor()
        {
            byte[] mCompressedBuffer = new byte[4 * 1024];
            byte[] mUncompressedBuffer = new byte[4 * 1024];
        }

        public SnappyCompressor(int defaultBuffersize)
        {
            Controller.WriteLine("Using SnappyCompressor with 2x {0} KB buffering", (defaultBuffersize * 1.0)/1024.0);
            mCompressedBuffer = new byte[defaultBuffersize];
            mUncompressedBuffer = new byte[defaultBuffersize];
        }

        public byte[] Compress(string text)
        {
            byte[] compressedBuffer = mCompressedBuffer;
            byte[] uncompressedBuffer = mUncompressedBuffer;

            if (text.Length > uncompressedBuffer.Length)
            {
                uncompressedBuffer = Encoding.UTF8.GetBytes(text);
            }
            else
            {
                Encoding.UTF8.GetBytes(text, 0, text.Length, uncompressedBuffer, 0);
            }

            int neededBufferSize = SnappyPI.Snappy32.GetMaximumCompressedLength(text.Length);
            if (neededBufferSize > compressedBuffer.Length)
            {
                compressedBuffer = new byte[neededBufferSize];  
            }
            int compressedLength = neededBufferSize;
            SnappyPI.Snappy32.Compress(uncompressedBuffer, text.Length, compressedBuffer, ref compressedLength);

            // copy actual compressed data to new shorter buffer
            byte[] shorter = new byte[compressedLength];
            Buffer.BlockCopy(compressedBuffer, 0, shorter, 0, compressedLength);
            Controller.WriteLine("Store: {0} chars in {1} bytes, ratio: {2:0.00}", text.Length, compressedLength, ((compressedLength * 1.0) / text.Length));
            return shorter;
        }

        public string Decompress(byte[] buffer)
        {
            byte[] uncompressedBuffer = mUncompressedBuffer;
            int uncompressedLength = 0;
            SnappyPI.Snappy32.GetUncompressedLength(buffer, buffer.Length, ref uncompressedLength);
            if (uncompressedLength > uncompressedBuffer.Length)
            {
                uncompressedBuffer = new byte[uncompressedLength];
            }
            SnappyPI.Snappy32.Uncompress(buffer, buffer.Length, uncompressedBuffer, ref uncompressedLength);
            string result = Encoding.UTF8.GetString(uncompressedBuffer, 0, uncompressedLength);
            Controller.WriteLine("buffer len: {0} - {1}", buffer.Length, result.Length);
            return result;
        }
    }
}

