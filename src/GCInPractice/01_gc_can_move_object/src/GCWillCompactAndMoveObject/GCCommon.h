#pragma once

#ifndef _GC_COMMON_H_LX_
#define _GC_COMMON_H_LX_

ref class rand_buffer_ref {
private:
	array<__int8>^ buffer;

public:
	rand_buffer_ref(int size) : buffer(gcnew array<__int8>(size)) { }

	property int Length {
		int get() {
			return buffer->Length;
		}
	}
};

ref class type_to_take_address {
public:
	int data;
	type_to_take_address() : data(0) {}
};

#endif