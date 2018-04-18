using System;
using System.Threading;

namespace _2._4.使用ManualResetEventSlim类
{
    class Program
    {
        static ManualResetEventSlim _event = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            var t1 = new Thread(() => { ThroughGates(5); });
            var t2 = new Thread(() => { ThroughGates(6); });
            var t3 = new Thread(() => { ThroughGates(12); });
            t1.Name = "t1";
            t2.Name = "t2";
            t3.Name = "t3";
            t1.Start();
            t2.Start();
            t3.Start();

            Console.WriteLine("主线程暂停6秒");
            Thread.Sleep(TimeSpan.FromSeconds(6));
            Console.WriteLine("主线程打开传送门...");
            _event.Set();

            Console.WriteLine("主线程暂停2秒");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("主线程关闭传送门...");
            _event.Reset();

            Console.WriteLine("主线程暂停10秒");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("主线程第二次打开传送门...");
            _event.Set();

            Console.WriteLine("主线程暂停2秒");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("主线程第二次关闭传送门...");
            _event.Reset();

            Console.ReadKey();
        }

        static void ThroughGates(int seconds)
        {
            Console.WriteLine($"线程{Thread.CurrentThread.Name}进入传送门之前先暂停{seconds}秒。");
            //for (int i = 1; i <= seconds; i++)
            //{
            //    Console.Write($"{i}...");
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //}
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine($"线程{Thread.CurrentThread.Name}暂停完毕，等待传送门的开启。");
            _event.Wait();
            Console.WriteLine($"线程{Thread.CurrentThread.Name}进入传送门！");
        }
    }
}
