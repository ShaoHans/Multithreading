using System;
using System.Threading;

namespace Infrastructure
{
    public static class Common
    {
        public static void PrintNumbers()
        {
            Console.WriteLine("开始打印数字...");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"当前线程Id：{Thread.CurrentThread.ManagedThreadId}，数字：{i}");
            }
        }

        public static void PrintNumbersWithDelay()
        {
            Console.WriteLine("开始打印数字...");
            for (int i = 0; i < 10; i++)
            {
                // 暂停线程
                Thread.Sleep(1000);
                Console.WriteLine($"当前线程Id：{Thread.CurrentThread.ManagedThreadId}，数字：{i}");
            }
        }

        public static void PrintNumbersWithStatus()
        {
            Console.WriteLine("开始打印数字...");
            Console.WriteLine($"当前线程{Thread.CurrentThread.Name}的状态：{Thread.CurrentThread.ThreadState.ToString()}");
            for (int i = 0; i < 10; i++)
            {
                // 暂停线程
                Thread.Sleep(1000);
                Console.WriteLine($"当前线程Id：{Thread.CurrentThread.ManagedThreadId}，数字：{i}");
            }
        }
    }
}
