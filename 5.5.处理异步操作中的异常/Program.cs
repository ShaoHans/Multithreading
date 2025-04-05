using System;
using System.Threading.Tasks;

namespace _5._5.处理异步操作中的异常
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = ProcessAsync();
            Console.ReadKey();
        }

        async static Task ProcessAsync()
        {
            Console.WriteLine("1.单个异常");
            try
            {
                string result = await GetInfoAsync("task1", 2);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常详细信息：{ex}");
            }

            Console.WriteLine();
            Console.WriteLine("2.多个异常");

            try
            {
                var task2 = GetInfoAsync("task2", 2);
                var task3 = GetInfoAsync("task3", 3);
                string[] results = await Task.WhenAll(task2, task3);
                Console.WriteLine(results.Length);
            }
            catch (Exception ex)
            {
                // 只能从底层的AggregateException对象中获取到第一个异常
                Console.WriteLine($"异常详细信息：{ex}");
            }

            Console.WriteLine();
            Console.WriteLine("3.多个异常With AggregateException");

            var task4 = GetInfoAsync("task4", 2);
            var task5 = GetInfoAsync("task5", 3);
            var task6 = Task.WhenAll(task4, task5);
            try
            {
                string[] results = await task6;
                Console.WriteLine(results.Length);
            }
            catch
            {
                var ae = task6.Exception.Flatten();
                var exceptions = ae.InnerExceptions;
                Console.WriteLine($"捕获到{exceptions.Count}个异常");
                foreach (var ex in exceptions)
                {
                    Console.WriteLine($"异常详细信息：{ex}");
                    Console.WriteLine();
                }
            }
        }

        async static Task<string> GetInfoAsync(string taskName, int seconds)
        {
            Console.WriteLine($"{taskName}任务开始了");
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            throw new Exception($"{taskName}炸了");
        }

    }
}
