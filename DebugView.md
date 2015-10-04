About DebugView, a tool by Mark Russinovich

# Introduction #

DebugView v4.81 is the latest version which works fine on Windows7/8, however, v4.76 seems to be the last version that works good on WindowsXP.


# Details #

Some statistics are measured against DebugView because I consider it the de-facto standard tool to monitor OutputDebugString messages.

Some facts:

  * a Unittest that outputs 160.000 lines as fast as possible, takes 2 seconds when no trace application is started.
  * the same test takes 40 seconds when DebugView is running
  * the same test takes 3.5 seconds when BugGazer is running

  * DebugView initially allocates 1.1 MB and consumes 34.9 MB of memory to store 160.000 lines of data totalling 6.9MB
  * BugGazer initially allocates 4.6 MB and consumes 15.4 MB of memory for the same data.
