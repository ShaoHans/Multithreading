using System;
using System.Threading;

namespace _2._6.使用Barrier类
{
    class Program
    {
        // 当您需要一组任务并行地运行一连串的阶段，
        // 但是每一个阶段都要等待所有其他任务都完成前一阶段之后才能开始，
        // 你可以通过Barrier实例来同步这一类协同工作
        static Barrier _barrier = new Barrier(3, b => {
            Console.WriteLine($"=========当前是{b.CurrentPhaseNumber+1}阶段===============");
        });

        static void Test(string step1, string step2, string step3, int seconds)
        {
            string threadName = Thread.CurrentThread.Name;
            Console.WriteLine($"线程{threadName}开始执行第一阶段操作：{step1}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            _barrier.SignalAndWait();

            Console.WriteLine($"线程{threadName}开始执行第二阶段操作：{step2}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            _barrier.SignalAndWait();

            Console.WriteLine($"线程{threadName}开始执行第三阶段操作：{step3}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            _barrier.SignalAndWait();

            Console.WriteLine($"线程{threadName}所有操作执行完毕");
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(() => { Test("步兵列阵", "步兵冲锋", "步兵退守", 3); });
            t1.Name = "t1";
            Thread t2 = new Thread(() => { Test("骑兵列阵", "骑兵冲锋", "骑兵退守", 4); });
            t2.Name = "t2";
            Thread t3 = new Thread(() => { Test("炮兵列阵", "炮兵装弹", "炮兵开炮", 5); });
            t3.Name = "t3";

            t1.Start();
            t2.Start();
            t3.Start();

            Console.ReadKey();
        }
    }
}
