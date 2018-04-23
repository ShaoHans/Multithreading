using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _5._4.对并行执行的异步任务使用await操作符
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = ProcessAsync();
            //t.Wait();
            Console.WriteLine("main");
            Console.ReadKey();
        }

        async static Task ProcessAsync()
        {
            Task<string> t1 = GetInfoAsync("t1", 4);
            Task<string> t2 = GetInfoAsync("t2", 3);

            string[] results = await Task.WhenAll(t1, t2);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        async static Task<string> GetInfoAsync(string taskName, int seconds)
        {
            Console.WriteLine($"{taskName}任务开始了");
            /*Task.Delay在幕后使用了一个计时器，从线程池中获取工作线程，将等待Task.Delay方法返回结果，
             * 然后，Task.Delay方法启动计时器并且指定一块代码，该代码会在计时器时间到了Task.Delay方法中指定的时间后被调用。
             * 之后立即将工作线程返回到线程池。当计时器时间运行时，
             * 我们线程池中任意获取一个可用的工作线程（可能就是运行一个任务时使用的线程）并运行计时器提供给他的代码。*/
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            //await Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(seconds)); });
            return $"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}
