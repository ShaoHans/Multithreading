using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _7._6.使用PLINQ查询的聚合
{
    class Program
    {
        static int NUM_INTS = 500000000;

        static IEnumerable<int> GenerateInputeData()
        {
            return Enumerable.Range(1, NUM_INTS);
        }

        static ParallelQuery<int> GenerateInputeData4Parallel()
        {
            return ParallelEnumerable.Range(1, NUM_INTS);
        }

        static void Main(string[] args)
        {
            var seqTarget = GenerateInputeData();

            Console.WriteLine("============================================================");
            Console.WriteLine("TEST NORMAL LINQ");
            Console.WriteLine("============================================================");
            var swatchpn = Stopwatch.StartNew();

            var seqQuery = (from intNum in seqTarget
                            where ((intNum % 5) == 0)
                            select (intNum / Math.PI)).Average();
            swatchpn.Stop();

            Console.WriteLine("LINQ Result: " + seqQuery + "    LINQ Use Time: {0}", swatchpn.Elapsed);



            var palTarget = GenerateInputeData4Parallel();
            Console.WriteLine("\n\n");
            Console.WriteLine("============================================================");
            Console.WriteLine("TEST PARALLEL LINQ");
            Console.WriteLine("============================================================");
            var swatchp = Stopwatch.StartNew();

            var palQuery = (from intNum in palTarget.AsParallel()
                            where ((intNum % 5) == 0)
                            select (intNum / Math.PI)).Average();
            swatchp.Stop();

            Console.WriteLine("PLINQ Result: " + palQuery + "    LINQ Use Time: {0}", swatchp.Elapsed);

            Console.ReadLine();

        }
    }
}
