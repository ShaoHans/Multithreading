using System;
using System.Threading;
using Infrastructure;

namespace _1._3.终止线程;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("主线程开始...");
        //创建一个子线程并启动
        Thread t = new Thread(Common.PrintNumbersWithDelay);
        t.Start();

        // 主线程暂停6秒
        Thread.Sleep(TimeSpan.FromSeconds(6));

        // 终止子线程t
        t.Abort();
        Console.WriteLine("子线程终止了...");

        Thread t1 = new Thread(Common.PrintNumbers);
        t1.Start();
        Common.PrintNumbers();

        Console.ReadKey();
    }
}
