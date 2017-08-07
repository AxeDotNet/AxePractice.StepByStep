using System;
using System.Diagnostics.CodeAnalysis;

namespace ExamineRoots
{
    class NamedObject
    {
        readonly string name;

        public NamedObject(string name)
        {
            this.name = name;
        }

        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public string Name => name;
    }

    class NamedObjectWithReference : NamedObject
    {
        public object Reference { get; }

        public NamedObjectWithReference(string name, object reference) : base(name)
        {
            Reference = reference;
        }
    }

    class Program
    {
        static readonly NamedObjectWithReference globalRoot = new NamedObjectWithReference(
            "I am global root", 
            new NamedObject("I am referenced by global root."));

        static void Main()
        {
            object globalRootReference = globalRoot.Reference;
            var localRoot = new NamedObjectWithReference(
                "I am local root.",
                globalRootReference);

            Console.ReadLine();

            GC.KeepAlive(localRoot);
        }
    }
}
