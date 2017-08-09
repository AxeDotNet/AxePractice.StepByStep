#include <tchar.h>
#include <cstdio>
#include "GCCommon.h"

using namespace System;
using namespace System::Collections::Generic;

int main()
{
	/*
	 * In this test, we would like to show that object may be moved and may
	 * be not when its generation is upgraded. We will first create fragmented
	 * gen1 and gen2 segments, then we add a non fragmented gen0 such as: 
	 *
	 * ┌---------------------┬---------------------┬-------┐
	 * | Gen 2 with fragment | Gen 1 with fragment | Gen 0 |
	 * └---------------------┴---------------------┴-------┘
	 *
	 * If we do a full GC, we can see that the upgraded GEN1 moved in memory
	 * because of memory compacting. But if GEN1 and GEN2 are not fragmented.
	 * The generation upgrade for GEN0 will not move at all.
	 */

	const size_t block_size = 100;
	const size_t block_count = 5000;

	auto gen_2 = gcnew List<buffer^>();
	initialize_block_list(gen_2, block_size, block_count);
	
	GC::Collect();

	auto gen_1 = gcnew List<buffer^>();
	initialize_block_list(gen_1, block_size, block_count);

	GC::Collect();

	auto gen_0 = gcnew List<buffer^>();
	initialize_block_list(gen_0, block_size, block_count);

	_tprintf(_T("Generation for gen_2: %d\r\n"), GC::GetGeneration(gen_2));
	_tprintf(_T("Generation for gen_1: %d\r\n"), GC::GetGeneration(gen_1));
	_tprintf(_T("Generation for gen_0: %d\r\n"), GC::GetGeneration(gen_0));

	/*  
	 * If we comment the next 2 lines, the upgrade of GEN0 may not move any memory.
	 */
	make_it_fragmented(gen_2);
	make_it_fragmented(gen_1);

	buffer^ buffer_at_gen0 = gcnew buffer(10);
	interior_ptr<__int8> ptr_buffer_at_gen0 = address_of_buffer(buffer_at_gen0);

	_tprintf(
		_T("Gen %d before collection: %p\r\n"), 
		GC::GetGeneration(buffer_at_gen0), 
		ptr_buffer_at_gen0);

	GC::Collect();

	_tprintf(
		_T("After upgrade to gen %d: %p\r\n"), 
		GC::GetGeneration(buffer_at_gen0),
		ptr_buffer_at_gen0);

	GC::KeepAlive(gen_2);
	GC::KeepAlive(gen_1);
	GC::KeepAlive(gen_0);
    return 0;
}

