using System;
using System.Threading;

namespace _1._8.Lock关键字;

class Program
{
    static void Main(string[] args)
    {
        CounterNoLock counterNoLock = new CounterNoLock();

        Thread t1 = new Thread(() =>
        {
            TestCounter(counterNoLock);
        });
        t1.Name = "t1";
        Thread t2 = new Thread(() =>
        {
            TestCounter(counterNoLock);
        });
        t2.Name = "t2";
        Thread t3 = new Thread(() =>
        {
            TestCounter(counterNoLock);
        });
        t3.Name = "t3";

        t1.Start();
        t2.Start();
        t3.Start();
        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine($"没有锁的计数器结果：{counterNoLock.Count}");

        CounterWithLock counterWithLock = new CounterWithLock();

        Thread tt1 = new Thread(() =>
        {
            TestCounter(counterWithLock);
        });
        tt1.Name = "tt1";
        Thread tt2 = new Thread(() =>
        {
            TestCounter(counterWithLock);
        });
        tt2.Name = "tt2";
        Thread tt3 = new Thread(() =>
        {
            TestCounter(counterWithLock);
        });
        tt3.Name = "tt3";

        tt1.Start();
        tt2.Start();
        tt3.Start();
        tt1.Join();
        tt2.Join();
        tt3.Join();
        Console.WriteLine($"有锁的计数器结果：{counterWithLock.Count}");

        Console.ReadKey();
    }

    static void TestCounter(CounterBase counter)
    {
        for (int i = 0; i < 100000; i++)
        {
            counter.Increase();
            counter.Decrease();
        }
    }
}
