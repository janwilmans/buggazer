using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SnappyPI;

namespace Snappy.Demo
{
	#region helpers

	/// <summary>Some helper methods.</summary>
	internal static class Helpers
	{
		public static string Optional(this string[] args, int index, string defaultValue)
		{
			return index < args.Length ? args[index] : defaultValue;
		}

		public static string MD5(this byte[] buffer)
		{
			var md5 = MD5CryptoServiceProvider.Create();
			md5.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
			md5.TransformFinalBlock(new byte[0], 0, 0);
			return string.Concat(md5.Hash.Select(b => b.ToString("x2")));
		}
	}

	#endregion

	#region class Program

	class Program
	{
		/// <summary>Compresses the specified input file.</summary>
		/// <param name="input">The input file.</param>
		/// <param name="output">The output file.</param>
		private static void Compress(string input, string output)
		{
			byte[] original = File.ReadAllBytes(input);
			var timer = Stopwatch.StartNew();
			var compressed = SnappyCodec.Compress(original, 0, original.Length);
			timer.Stop();
			Console.WriteLine("Compression:");
			Console.WriteLine("  Speed: {0:0.00}MB/s", (double)original.Length / 1024 / 1024 / timer.Elapsed.TotalSeconds);
			Console.WriteLine("  Ratio: {0:0.00}%", (double)compressed.Length * 100 / original.Length);
			Console.WriteLine("  Hash: {0}", original.MD5());
			File.WriteAllBytes(output, compressed);
		}

		/// <summary>Uncompresses the specified input file.</summary>
		/// <param name="input">The input file.</param>
		/// <param name="output">The output file.</param>
		private static void Uncompress(string input, string output)
		{
			byte[] compressed = File.ReadAllBytes(input);
			var timer = Stopwatch.StartNew();
			var decompressed = SnappyCodec.Uncompress(compressed, 0, compressed.Length);
			timer.Stop();
			Console.WriteLine("Decompression:");
			Console.WriteLine("  Speed: {0:0.00}MB/s", (double)decompressed.Length / 1024 / 1024 / timer.Elapsed.TotalSeconds);
			Console.WriteLine("  Ratio: {0:0.00}%", (double)compressed.Length * 100 / decompressed.Length);
			Console.WriteLine("  Hash: {0}", decompressed.MD5());
			File.WriteAllBytes(output, decompressed);
		}

		/// <summary>Main.</summary>
		/// <param name="args">The args.</param>
		/// <returns><c>0</c> if succeeded, error code otherwise.</returns>
		static int Main(string[] args)
		{
			try
			{
				var command = args[0].ToLower();
				string input, output;

				switch (command)
				{
					case "c":
						input = args[1];
						output = args.Optional(2, input + ".snappy");
						Compress(input, output);
						break;
					case "d":
						input = args[1];
						output = args.Optional(2, input + ".decompressed");
						Uncompress(input, output);
						break;
					default:
						throw new ArgumentException(
							string.Format("Unrecognized command: {0}", command));
				}

				return 0;
			}
			catch (Exception e)
			{
				Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
				string exe_name = Path.GetFileName(typeof(Program).Assembly.Location);
				Console.WriteLine("Compress: {0} c <input> <output>", exe_name);
				Console.WriteLine("Decompress: {0} d <input> <output>", exe_name);
				Console.WriteLine("Press <enter>...");
				Console.ReadLine();
				return 1;
			}
		}
	}

	#endregion
}
