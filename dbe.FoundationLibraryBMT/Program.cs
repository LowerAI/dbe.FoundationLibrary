using BenchmarkDotNet.Running;

using GNDView.LibraryBMT.DataConvert;

using System;

namespace GNDView.LibraryBMT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            BenchmarkRunner.Run<BitLibBMT>();
        }
    }
}