#pragma once

#ifndef _GC_COMMON_H_LX_
#define _GC_COMMON_H_LX_

typedef array<__int8> buffer;

#define address_of_buffer(b) (&((b)[0]))
#define initialize_block_list(_list_ref, _size_of_block, _count_of_block) \
	for (size_t i = 0; i < (_count_of_block); ++i) { \
		(_list_ref)->Add(gcnew buffer(_size_of_block)); \
	}

void make_it_fragmented(System::Collections::Generic::List<buffer^>^ buf_list) {
	for (int i = 0; i < buf_list->Count; ++i) {
		if (i % 3 != 0) { continue; }
		buf_list[i] = nullptr;
	}
}

#endif