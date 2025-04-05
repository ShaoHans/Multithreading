using System;
using System.Threading;
using System.Threading.Tasks;

namespace _4._5.处理任务中的异常
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var task1 = Task.Run(() => TaskMethod("task1", 2));
                Console.WriteLine($"task1返回的结果是：{task1.Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到了task1的异常：{ex.InnerException.Message}");
            }

            try
            {
                var task2 = Task.Run(() => TaskMethod("task2", 2));
                // 通过GetAwaiter().GetResult()方法来访问任务结果，如果只有一个底层任务，那么一次只能获取一个原始异常
                Console.WriteLine($"task2返回的结果是：{task2.GetAwaiter().GetResult()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到了task2的异常：{ex.Message}");
            }

            var task3 = new Task<int>(() => TaskMethod("task3", 2));
            var task4 = new Task<int>(() => TaskMethod("task4", 2));
            var complexTasks = Task.WhenAll(task3, task4);
            complexTasks.ContinueWith(t =>
            {
                Console.WriteLine($"异常：{t.Exception}");
            }, TaskContinuationOptions.OnlyOnFaulted);
            task3.Start();
            task4.Start();

            Console.ReadKey();
        }

        static int TaskMethod(string taskName, int seconds)
        {
            Console.WriteLine($"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception($"{taskName}炸啦");
            return 36 * seconds;
        }
    }
}
