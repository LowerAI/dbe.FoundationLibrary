using BenchmarkDotNet.Attributes;

using GNDView.Library.Common;
//using GNDView.Library.DataConvert;
using GNDView.Library.Util;

using System;
using System.Text;

namespace GNDView.LibraryBMT.DataConvert
{
    [MaxColumn, MinColumn, MemoryDiagnoser]
    public class BitLibBMT
    {
        private bool[] inputs;

        [GlobalSetup]
        public void Setup()
        {
            inputs = new bool[] { true, false, true, false, true, false };
        }

        [Benchmark]
        public void GetBitArrayFromBitArray()
        {
            //var result = BitLib.GetBitArrayFromBitArray(inputs, 2, 3);
            //var result = inputs.SubBitArray(2, 3);
        }

        [Benchmark]
        public void GetBitSpanFromBitArray()
        {
            var inputSpan = new Span<bool>(inputs);
            //var result = BitLib.GetBitSpanFromBitArray(inputSpan, 2, 3);
        }

        [Benchmark]
        public void GetCRC()
        {
            string str = "Hello World!";
            var buffer = Encoding.UTF8.GetBytes(str);
            var crcTypes = Enum.GetNames(typeof(CrcType));
            for (int i = 0; i < crcTypes.Length; i++)
            {
                var hexStr = BitConverter.ToString(CheckSum.GetCRC(i, buffer)).Replace("-", "");
                //Console.WriteLine($"{CheckSum.CrcInfoTab[i].Name} ({str}) = 0x{hexStr}");
            }
        }
    }
}