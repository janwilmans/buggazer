Introducing BugGazer a .NET based OutputDebugString viewer

# Introduction #

BugGazer started because of a small annoyance I had with DebugView.
Now I can't remember what it was ;) but the project now lives a life of its own. I did not intent to replace DebugView.
So exactly what does BugGazer feature:

  * Build using C# on .NET 2.0 runtime
  * Capture both Win32 and GlobalWin32 messages on Windows XP, 7 and 8
  * Compressed memory buffers using Google Snappy (LZ77) based compression, which, on average, halves memory requirements.
  * Fast responsive UI even up to 2^32 lines in buffer
  * Minimal delay of the traced application, compared to debugview, a factor of 10 smaller.
  * responsive UI even with 100ths of incoming messages/second

# Details #

To enable BugGazer to capture GlobalWin32 message, you need administrative (or at least 'debug') rights. Run as administrator will do the trick in most cases.