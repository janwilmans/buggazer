#include "stdafx.h"

#include "stdafx.h"
#include "windows.h"
#include <stdio.h>
#include <string>

#include <iostream>
using namespace std;
#include <sys/timeb.h>
#include <sstream>
#include <fstream>

int getMilliCount(){
	timeb tb;
	ftime(&tb);
	int nCount = tb.millitm + (tb.time & 0xfffff) * 1000;
	return nCount;
}

void testLongString()
{
	int length = 15000;
	std::ostringstream ss;
	ss << "1234567890ABCDEF-%s {0} \t\r\n";
	ss << length;
	std::string test = ss.str();
	for (size_t i=0;i<length-test.size()-1; i++)
	{
		ss << "X";
	}
	ss << "\n";
	test = ss.str();	
	
	OutputDebugStringA(test.c_str());
}

int _tmain(int argc, _TCHAR* argv[])
{
	if (argc > 1)
	{
		OutputDebugStringA("Output Titan crash log\n");
		std::fstream fs;
	    fs.open ("titan_crash_debugview_43mb.log", std::fstream::in);

		long t1 = getMilliCount();

		while( !fs.eof() ) {
			std::string test;
			getline(fs, test);
			OutputDebugStringA(test.c_str());
			Sleep(50);
		}
		long t2 = getMilliCount();
		fs.close();

		printf("took: %u ms\n", t2-t1);
		Sleep(15000);
		return 0;
	}

	int length = 260;
	std::ostringstream ss;
	ss << "1234567890ABCDEF-";
	ss << length;
	std::string test = ss.str();
	for (size_t i=0;i<length-test.size()-1; i++)
	{
		ss << "X";
	}
	ss << "\n";
	test = ss.str();	
	
	//testLongString();

	while (1)
	{
		long t1 = getMilliCount();

		// write exactly 1MB to the debug buffer;
		for (int i=0; i< 32; i++)
		{
			OutputDebugStringA(test.c_str());
		}
		long t2 = getMilliCount();

		printf("took: %u ms\n", t2-t1);
		Sleep(500);
	}

	return 0;
}

