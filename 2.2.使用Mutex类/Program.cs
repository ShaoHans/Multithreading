using System;
using System.Threading;

namespace _2._2.使用Mutex类
{
    class Program
    {
        static void Main(string[] args)
        {
            // 应用场景：WinForm应用程序只能启动一个
            string mutexName = "互斥量";
            using (var mutex = new Mutex(false, mutexName))
            {
                if (mutex.WaitOne(TimeSpan.FromSeconds(5), false))
                {
                    Console.WriteLine("运行中....");
                    Console.ReadLine();
                    mutex.ReleaseMutex();
                }
                else
                {
                    Console.WriteLine("第二个程序运行啦。。。。");
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}
