using System;

namespace LohSample
{
    class Program
    {
        static void Main()
        {
            var largeObject = new byte[900000];

            int largeObjectGeneration = GC.GetGeneration(largeObject);
            Console.WriteLine($"The generation of largeObject array is: {largeObjectGeneration}.");

            Console.WriteLine("Press enter to exit ...");
            Console.ReadLine();
            GC.KeepAlive(largeObject);
        }
    }
}
