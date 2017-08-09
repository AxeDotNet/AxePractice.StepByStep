# Examine the large object heap

Please execute the program until reaching the `Console.Readline()` statement. The launch the winDBG and attach to the process.

We can use the `-min` filter along with the `-type` filter to quickly find the large byte array.

```
!dumpheap -type System.Byte[] -min 90000
```

And here is the result.

```
         Address               MT     Size
0000005810009968 00007ffce40c27a8   900024     

Statistics:
              MT    Count    TotalSize Class Name
00007ffce40c27a8        1       900024 System.Byte[]
Total 1 objects
```

Since we have got the object address, we can now examine where on the heap it is located. We first dump the structure of the heap.

```
!eeheap -gc
```

It should come with the result as shown below.

```
Number of GC Heaps: 1
generation 0 starts at 0x0000005800001030
generation 1 starts at 0x0000005800001018
generation 2 starts at 0x0000005800001000
ephemeral segment allocation context: none
         segment             begin         allocated              size
0000005800000000  0000005800001000  0000005800007fe8  0x6fe8(28648)
Large object heap starts at 0x0000005810001000
         segment             begin         allocated              size
0000005810000000  0000005810001000  00000058100e5520  0xe4520(935200)
Total Size:              Size: 0xeb508 (963848) bytes.
------------------------------
GC Heap Size:            Size: 0xeb508 (963848) bytes.

```

There are many information we can retrive from the output.

* The last line indicates the total size of the GC heaps. for now the GC heap size is 963848 bytes.
* The GC is made up of several heaps, and each heap contains segements. 
* We can see that the GC heap is composed by several segments. Form the output above, we can conclude that there are 2 segments allocated on GC. One segment starts from 0x0000005800001000, and the other starts at 0x0000005810001000.
* The first heap is a small object heap. There are 3 generations: 0, 1 and 2. Layout as follows:

|Generation|Start|End|Allocated Size|
|---|---|---|---|
|2|0x0000005800001000|0x0000005800001017|24 bytes|
|1|0x0000005800001018|0x000000580000102F|24 bytes|
|0|0x0000005800001030|0x0000005800007FE7|28600 bytes|

* The second segment however, is the segment of so called *Large Object Heap*. It will be used to store some data strcuture that is larger than 80000 bytes.
* The large array we allocated is located at 0x0000005810009968, and it is not hard to figure out that the object is on the segment of the LOH. And object on the large object heap is directly from GEN 2.