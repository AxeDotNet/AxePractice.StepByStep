#include <tchar.h>
#include <cstdio>
#include "GCCommon.h"

using namespace System;
using namespace System::Collections::Generic;

int main(void) {
	/*
	 * We should create as much fragmentation as we can so that we can observe
	 * the memory compaction. We will allocate ~100 bytes per block and we will
	 * create totaly 10000 blocks, and each block goes with a fragment of ~100
	 * bytes.
	 */
	const int block_size = 100;
	const int allocated_buffer_count = 10000;

	/*
	 * We now have two collections to control which blocks will be collected and
	 * which onces will not be collected.
	 */
	auto fragmented_heap = gcnew List<rand_buffer_ref^>();
	auto gap = gcnew List<rand_buffer_ref^>();

	/*
	 * Now we are creating huge number of fragmented blocks.
	 */
	for (int i = 0; i < allocated_buffer_count; ++i) {
		fragmented_heap->Add(
			gcnew rand_buffer_ref(block_size)
		);
		gap->Add(gcnew rand_buffer_ref(block_size));
	}

	/*
	 * Now we create the data structure to have it address traced. Then we get its
	 * interior pointer so that we can print its address.
	 */
	auto will_be_moved = gcnew type_to_take_address();
	interior_ptr<int> ptr_to_data = &(will_be_moved->data);

	/*
	 * Since generation moving will also cause memory moving. We need to quickly
	 * move allocated blocks to the second generation so that we can ensure that
	 * the memory moving is caused by memory compaction.
	 */
	GC::Collect();
	GC::Collect();
	GC::KeepAlive(gap);

	/*
	 * Print current generation of will_be_moved object before GC.
	 */
	_tprintf(_T("The generation of will_be_moved: %d\r\n"), GC::GetGeneration(will_be_moved));
	_tprintf(_T("The address before collection: %p\r\n"), ptr_to_data);
	gap = nullptr;
	
	/*
	 * Fire in the hole.
	 */
	GC::Collect();

	/*
	 * Now, examine the memory compact result after GC.
	 */
	_tprintf(_T("The address after collection: %p\r\n"), ptr_to_data);

	GC::KeepAlive(fragmented_heap);
	GC::KeepAlive(will_be_moved);
	Console::ReadLine();

    return 0;
}

