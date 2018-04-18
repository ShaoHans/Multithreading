using System;
using System.Threading;

namespace _2._5.使用CountdownEvent类
{
    class Program
    {
        static CountdownEvent _event = new CountdownEvent(3);

        static void Test(int seconds)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name}线程开始执行...");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine($"{Thread.CurrentThread.Name}线程执行完成！") ;
            _event.Signal();
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(() => { Test(2); });
            t1.Name = "t1";
            Thread t2 = new Thread(() => { Test(4); });
            t2.Name = "t2";
            Thread t3 = new Thread(() => { Test(6); });
            t3.Name = "t3";

            t1.Start();
            t2.Start();
            t3.Start();

            _event.Wait();
            Console.WriteLine("所有的线程执行完毕！");
            _event.Dispose();

            Console.ReadKey();
        }
    }
}
