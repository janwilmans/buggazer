# buggazer

This project is now nolonger being developed, all feature of BugGazer (and more) will be implemented in DebugView++, see: https://github.com/djeedjay/DebugViewPP

It is a C++/WTL based project, which is even faster and has no runtime .NET dependencies, it is a standalone single executable.

BugGazer is still useful as a reference to learn how to use work with Virtual listviews and GoogleSnappy in C#.

BugGazer started because of a small annoyance I had with DebugView. Now I can't remember what it was ;) but the project lived a life of its own. I did not intent to replace DebugView.

So exactly what does BugGazer feature:

Build using C# on .NET 2.0 runtime
Capture both Win32 and GlobalWin32 messages on Windows XP, 7 and 8
Compressed memory buffers using Google Snappy based compression, which, on average, halves memory requirements.
Fast responsive UI even up to 2^32 lines in buffer
Minimal delay of the traced application, compared to debugview, a factor of 10 smaller.
responsive UI even with 100ths of incoming messages/second
