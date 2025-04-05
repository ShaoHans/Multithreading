using Infrastructure;

using System;
using System.Threading;

namespace _1._1.暂停线程;

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
