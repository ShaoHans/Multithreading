using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _5._6.使用async_void方法
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncTask();
            t.Wait();

            // 没有返回值，无法监控该异步操作的状态
            AsyncVoid();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            t = AsyncTaskWithErrors();
            while (!t.IsFaulted)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine(t.Exception);

            // 使用async void方法， 线程池中未处理的异常会终止整个进程
            //try
            //{
            //    AsyncVoidWithErrors();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            // 在lambda表达式中不要忘记对异常的处理
            int[] numbers = { 1, 2, 3, 4, 5 };
            Array.ForEach(numbers, async n =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    if (n == 3)
                    {
                        throw new Exception("碰到3就炸了");
                    }
                    Console.WriteLine(n);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            Console.ReadKey();
        }

        async static Task AsyncTaskWithErrors()
        {
            string result = await GetInfoAsync("AsyncTaskException", 2);
            Console.WriteLine(result);
        }

        async static void AsyncVoidWithErrors()
        {
            string result = await GetInfoAsync("AsyncVoidException", 3);
            Console.WriteLine(result);
        }

        async static Task AsyncTask()
        {
            string result = await GetInfoAsync("AsyncTask", 2);
            Console.WriteLine(result);
        }

        async static void AsyncVoid()
        {
            string result = await GetInfoAsync("AsyncVoid", 3);
            Console.WriteLine(result);
        }

        async static Task<string> GetInfoAsync(string taskName, int seconds)
        {
            Console.WriteLine($"{taskName}任务开始了");
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            if(taskName.Contains("Exception"))
            {
                throw new Exception($"{taskName}炸了");
            }

            return $"任务 {taskName} 运行结束，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}
