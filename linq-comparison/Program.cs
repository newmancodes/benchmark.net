using BenchmarkDotNet.Running;
using System;

namespace linq_comparison
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BenchmarkRunner.Run<SamplesBenchmarks>();
            Console.ReadKey(true);
        }
    }
}
