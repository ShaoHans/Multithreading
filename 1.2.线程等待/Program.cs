using Infrastructure;

using System;
using System.Threading;

namespace _1._2.线程等待;

class Program
{
    static void Main(string[] args)
    {
        Thread thread = new Thread(Common.PrintNumbersWithDelay);
        thread.Start();

        // 等待线程
        thread.Join();

        Console.WriteLine("打印完成！");
        Console.ReadKey();
    }
}
