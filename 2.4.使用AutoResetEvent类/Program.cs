using System;

namespace _2._4.使用AutoResetEvent类
{
    class Program
    {
        static void Main(string[] args)
        {
            Sample1.Start();
            Console.WriteLine("===================================");
            Sample2.Start();
            Console.ReadKey();
        }
    }
}
