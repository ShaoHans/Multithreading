using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _5._3.对连续的异步任务使用await操作符
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncWithAwait();
            //t.Wait();
            Console.WriteLine("main");
            Console.ReadKey();
        }

        async static Task AsyncWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Async 1");
                Console.WriteLine(result);
                // 注意：Async2任务只有等待Async1任务完成之后才开始执行，不是和Async1并行执行
                result = await GetInfoAsync("Async 2");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        async static Task<string> GetInfoAsync(string taskName)
        {
            Console.WriteLine($"{taskName}任务开始了");
            await Task.Delay(TimeSpan.FromSeconds(2));
            return $"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}
