using System;
using System.Threading;
using Infrastructure;

namespace _1._1.暂停线程
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(Common.PrintNumbersWithDelay);
            thread.Start();
            Common.PrintNumbers();
            Console.ReadKey();
        }
    }
}
