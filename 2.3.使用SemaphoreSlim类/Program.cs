using System;
using System.Threading;

namespace _2._3.使用SemaphoreSlim类
{
    class Program
    {
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(10);

        /// <summary>
        /// 在餐厅吃饭
        /// </summary>
        /// <param name="seconds"></param>
        static void EatInDiningRoom(int seconds)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name}等待座位...");
            _semaphoreSlim.Wait();
            Console.WriteLine($"{Thread.CurrentThread.Name}坐下来开始吃饭...");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine($"{Thread.CurrentThread.Name}吃完了！");
            _semaphoreSlim.Release();
        }

        static void Main(string[] args)
        {
            // SemaphoreSlim：信号量

            // 假设餐厅只有10个位置，但有15位顾客要吃饭，所以有5位顾客需要等待
            for (int i = 0; i < 15; i++)
            {
                Thread t = new Thread(() =>
                {
                    EatInDiningRoom(new Random().Next(2, 4));
                });
                t.Name = $"t{i}";
                t.Start();
            }

            Console.ReadKey();
        }
    }
}
