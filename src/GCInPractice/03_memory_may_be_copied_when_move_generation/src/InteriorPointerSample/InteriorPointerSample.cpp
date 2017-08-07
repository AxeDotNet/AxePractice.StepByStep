
#include "stdafx.h"

using namespace System;
using namespace System::Linq;

ref class SampleManagedClass {
public:
	int _data;

public:
	SampleManagedClass(int data) : _data(data) { }
};

int main()
{
	for (int i = 0; i < 10000; ++i) { gcnew SampleManagedClass(3); }

	auto ref = gcnew SampleManagedClass(3);
	int generation = GC::GetGeneration(ref);
	interior_ptr<int> ptr_data = &(ref->_data);
	_tprintf(
		_T("Managed class at gen %d. Pointer to member at %p\r\n"),
		generation,
		ptr_data);

	for (int i = 0; i < 10000; ++i) { gcnew SampleManagedClass(3); }

	GC::Collect(0, GCCollectionMode::Forced);

	int generation_after_one_collection = GC::GetGeneration(ref);
	_tprintf(
		_T("Managed class at gen %d. Pointer to member at %p\r\n"),
		generation_after_one_collection,
		ptr_data);

	GC::Collect(0, GCCollectionMode::Forced);
	GC::Collect(1, GCCollectionMode::Forced);
	int generation_after_two_collections = GC::GetGeneration(ref);
	_tprintf(
		_T("Managed class at gen %d. Pointer to member at %p\r\n"),
		generation_after_two_collections,
		ptr_data);
	
    return 0;
}
