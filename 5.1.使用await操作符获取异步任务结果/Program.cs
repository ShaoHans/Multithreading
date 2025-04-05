using System;
using System.Threading;
using System.Threading.Tasks;

namespace _5._1.使用await操作符获取异步任务结果
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始执行异步操作");
            Task t1 = AsynchronyWithTPL();
            //t1.Wait();
            Task t2 = AsynchronyWithAwait();
            //t2.Wait();
            Console.WriteLine("结束了");
            Console.ReadKey();
        }

        async static Task<string> GetInfoAsync(string taskName)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            //throw new Exception("炸了");
            return $"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
        }

        static Task AsynchronyWithTPL()
        {
            Task<string> t = GetInfoAsync("TPL task");

            Task t2 = t.ContinueWith(task => Console.WriteLine(task.Result), TaskContinuationOptions.NotOnFaulted);
            Task t3 = t.ContinueWith(task => Console.WriteLine(task.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

            return Task.WhenAny(t2, t3);
        }

        async static Task AsynchronyWithAwait()
        {
            try
            {
                Console.WriteLine("AsynchronyWithAwait begin");
                Console.WriteLine($"{await GetInfoAsync("Await task")}");
                Console.WriteLine("AsynchronyWithAwait end");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
