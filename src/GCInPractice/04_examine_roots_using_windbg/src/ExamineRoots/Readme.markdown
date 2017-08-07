# GCRoot Example

The program will generate the following referencing diagram when hit `Console.ReadLine()`.

```
             ┌------------┐
globalRoot-->|  (1)NOWR   |                      ┌-------------------┐
             ├------------┤                  ┌-->|    (4)String      |
             |    Name    |------------------┘   ├-------------------┤
             ├------------┤       ┌--------┐     | I am global root  |
             | Reference  |--┬--->| (2)NO  |     └-------------------┘
             └------------┘  |    ├--------┤     ┌-------------------------------┐
globalRootReference----------┘    |  Name  |---->|         (5)String             | 
                                  └--------┘     --------------------------------┤
                                                 | I am referenced by global root|
             ┌------------┐                      └-------------------------------┘
localRoot--->|  (3)NOWR   |
             ├------------┤                      ┌-------------------┐
             |    Name    |--------------------->|    (6)String      |
             ├------------┤                      ├-------------------┤
             | Reference  |                      | I am local root   |
             └------------┘                      └-------------------┘           
```

## Examine GC root for each object.

First we have to distinguish each `NamedObject` instance. First of all, let's get all 3 `NamedObject` instances.

```
!dumpheap -type NamedObject
```

The command produces the following output.

```
         Address               MT     Size
000000eb00004618 00007fff94e65ae0       24     
000000eb00004630 00007fff94e65bd0       32     
000000eb00004690 00007fff94e65bd0       32     

Statistics:
              MT    Count    TotalSize Class Name
00007fff94e65ae0        1           24 ExamineRoots.NamedObject
00007fff94e65bd0        2           64 ExamineRoots.NamedObjectWithReference

```

We can query the *MT* table knowing that the object at `0x000000eb00004618` is the `NamedObject` instance. And the instances at `0x000000eb00004630` and `0x000000eb00004690` are the `NamedObjectWithReference` instances.

Let's find the root for each of the instances. First, the instance at `0x000000eb00004618`. We use the command `!gcroot 0x000000eb00004618` to find all of its roots.

```
Thread 1774:
    000000eb664eea80 00007fff94f70687 ExamineRoots.Program.Main() [04_examine_roots_using_windbg\src\ExamineRoots\Program.cs @ 43]
        rbp-18: 000000eb664eeab8
            ->  000000eb00004690 ExamineRoots.NamedObjectWithReference
            ->  000000eb00004618 ExamineRoots.NamedObject

    000000eb664eea80 00007fff94f70687 ExamineRoots.Program.Main() [04_examine_roots_using_windbg\src\ExamineRoots\Program.cs @ 43]
        rbp-10: 000000eb664eeac0
            ->  000000eb00004618 ExamineRoots.NamedObject

HandleTable:
    000000eb667317d8 (pinned handle)
    -> 000000eb10005970 System.Object[]
    -> 000000eb00004630 ExamineRoots.NamedObjectWithReference
    -> 000000eb00004618 ExamineRoots.NamedObject
```

We can see that two GC roots are the local roots since it is located on stack and stored in *rbp-18* and *rbp-10* registers respectively. Another root is the pinned handle in *HandleTable*, which is the static variable. From the diagram, we know that the instance is the number **(2)** instance.

Let's try another: `0x000000eb00004630`:

```
HandleTable:
    000000eb667317d8 (pinned handle)
    -> 000000eb10005970 System.Object[]
    -> 000000eb00004630 ExamineRoots.NamedObjectWithReference
```

We can see that this is the global root referenced by a static variable. The number **(1)** object.

Then the last one: `0x000000eb00004690`:

```
Thread 1774:
    000000eb664eea80 00007fff94f70687 ExamineRoots.Program.Main() [04_examine_roots_using_windbg\src\ExamineRoots\Program.cs @ 43]
        rbp-18: 000000eb664eeab8
            ->  000000eb00004690 ExamineRoots.NamedObjectWithReference
```

We can see that this is a local root, and it is instance number **(3)**.