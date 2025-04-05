using System;
using System.Diagnostics;
using System.Threading;

namespace _1._5.线程优先级;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"主线程{Thread.CurrentThread.Name}的优先级是{Thread.CurrentThread.Priority}");
        Console.WriteLine("在所有CPU内核上运行线程");
        RunThreads();
        Thread.Sleep(TimeSpan.FromSeconds(3));
        Console.WriteLine("在一个CPU内核上运行线程");
        Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
        RunThreads();
        Console.ReadKey();
    }

    static void RunThreads()
    {
        ThreadSample sample = new ThreadSample();
        Thread t1 = new Thread(sample.CountNumbers);
        t1.Name = "t1";
        Thread t2 = new Thread(sample.CountNumbers);
        t2.Name = "t2";

        t1.Priority = ThreadPriority.Highest;
        t2.Priority = ThreadPriority.Lowest;

        t1.Start();
        t2.Start();

        Thread.Sleep(TimeSpan.FromSeconds(2));
        sample.Stop();
    }
}

class ThreadSample
{
    private bool _isStopped = false;

    public void Stop()
    {
        _isStopped = true;
    }

    public void CountNumbers()
    {
        long count = 0;
        while (!_isStopped)
        {
            count++;
        }

        Console.WriteLine(
            $"线程{Thread.CurrentThread.Name}的优先级是{Thread.CurrentThread.Priority},计数器的值为{count}"
        );
    }
}
