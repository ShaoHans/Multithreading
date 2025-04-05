using System;
using System.Diagnostics;
using System.Threading;

namespace _3._3.线程池与并行度;

class Program
{
    static void Main(string[] args)
    {
        int threadCount = 500;
        var sw = new Stopwatch();
        sw.Start();
        UseThreads(threadCount);
        sw.Stop();
        Console.WriteLine($"手动创建线程执行操作耗时：{sw.ElapsedMilliseconds}");

        sw.Reset();
        sw.Start();
        UseThreadPool(threadCount);
        sw.Stop();
        Console.WriteLine($"使用线程池执行操作耗时：{sw.ElapsedMilliseconds}");

        Console.ReadKey();
    }

    static void UseThreads(int threadCount)
    {
        using (CountdownEvent cdEvt = new CountdownEvent(threadCount))
        {
            Console.WriteLine("开始创建线程");
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(() =>
                {
                    Console.WriteLine(
                        $"是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}，线程Id：{Thread.CurrentThread.ManagedThreadId}"
                    );
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    cdEvt.Signal();
                });
                thread.Start();
            }
            cdEvt.Wait();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// 线程池的用途是执行时间短的操作，使用线程池可以减少并行度消耗及节省操作系统资源
    /// </summary>
    /// <param name="threadCount"></param>
    static void UseThreadPool(int threadCount)
    {
        using (CountdownEvent cdEvt = new CountdownEvent(threadCount))
        {
            Console.WriteLine("开始使用线程池");
            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Console.WriteLine(
                        $"是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}，线程Id：{Thread.CurrentThread.ManagedThreadId}"
                    );
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    cdEvt.Signal();
                });
            }
            cdEvt.Wait();
            Console.WriteLine();
        }
    }
}
