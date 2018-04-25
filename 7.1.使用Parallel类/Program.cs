using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _7._1.使用Parallel类
{
    class Program
    {
        static void Main(string[] args)
        {
            // 使用Parallel类处理并行任务
            Parallel.Invoke(() => Process("task1"), 
                () => Process("task2"),
                () =>
                {
                    string result = Process("task3");
                    Console.WriteLine($"task3返回的结果：{result}");
                });

            var cts = new CancellationTokenSource();
            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var loopResult = Parallel.ForEach(numbers,
                new ParallelOptions
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default
                },
                (i, state) =>
                {
                    Console.WriteLine(i);
                    if (i == 8)
                    {
                        //state.Stop(); // 停止所有的并行迭代
                        state.Break();  // 停止i之后的迭代
                        Console.WriteLine($"并行迭代终止：{state.IsStopped}");
                    }
                }
                );

            Console.WriteLine($"并行迭代是否完成：{loopResult.IsCompleted}");
            Console.WriteLine($"并行迭代最低迭代次数：{loopResult.LowestBreakIteration}");

            Console.ReadKey();
        }

        static string Process(string taskName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(100, 500)));
            Console.WriteLine($"任务{taskName}在{Thread.CurrentThread.ManagedThreadId}号线程上处理");
            return taskName;
        }
    }
}
