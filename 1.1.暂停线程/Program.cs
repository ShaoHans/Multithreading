using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._1.暂停线程
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(PrintNumbersWithDelay);
            thread.Start();
            PrintNumbers();
            Console.ReadKey();
        }

        static void PrintNumbers()
        {
            Console.WriteLine("开始打印数字...");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"当前线程Id：{Thread.CurrentThread.ManagedThreadId}，数字：{i}");
            }
        }

        static void PrintNumbersWithDelay()
        {
            Console.WriteLine("开始打印数字...");
            for (int i = 0; i < 10; i++)
            {
                // 暂停线程
                Thread.Sleep(1000);
                Console.WriteLine($"当前线程Id：{Thread.CurrentThread.ManagedThreadId}，数字：{i}");
            }
        }
    }
}
