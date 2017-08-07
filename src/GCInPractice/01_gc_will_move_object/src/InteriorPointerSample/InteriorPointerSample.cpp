
#include "stdafx.h"

using namespace System;

ref class SampleManagedClass {
public:
	int _data;

public:
	SampleManagedClass(int data) : _data(data) { }
};

int main()
{
	const int largeAmount = 1000000;

	for (int i = 0; i < largeAmount; ++i) { gcnew SampleManagedClass(3); }

	auto ref_sample = gcnew SampleManagedClass(2);
	interior_ptr<int> ptr_to_data = &(ref_sample->_data);
	_tprintf(_T("Address of data member: %p\r\n"), ptr_to_data);
	_tprintf(_T("Value of data: %d\r\n"), *ptr_to_data);

	GC::Collect(0, GCCollectionMode::Forced);

	_tprintf(_T("Address of data member: %p\r\n"), ptr_to_data);
	_tprintf(_T("Value of data: %d\r\n"), *ptr_to_data);
	
    return 0;
}