using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _4._6.并行运行任务;

class Program
{
    static void Main(string[] args)
    {
        var t1 = new Task<int>(() => TaskMethod("t1", 3));
        var t2 = new Task<int>(() => TaskMethod("t2", 2));
        var waitAll = Task.WhenAll(t1, t2);
        waitAll.ContinueWith(
            t =>
            {
                Console.WriteLine($"t1的结果：{t.Result[0]},t2的结果：{t.Result[1]}");
            },
            TaskContinuationOptions.OnlyOnRanToCompletion
        );

        t1.Start();
        t2.Start();

        Thread.Sleep(TimeSpan.FromSeconds(4));

        var tasks = new List<Task<int>>();
        for (int i = 1; i <= 4; i++)
        {
            int counter = i;
            var task = new Task<int>(() => TaskMethod($"t{counter}", counter));
            tasks.Add(task);
            task.Start();
        }

        while (tasks.Count > 0)
        {
            var completeTask = Task.WhenAny(tasks).Result;
            tasks.Remove(completeTask);
            Console.WriteLine($"任务结果：{completeTask.Result}");
        }

        Console.ReadKey();
    }

    static int TaskMethod(string taskName, int seconds)
    {
        Console.WriteLine(
            $"任务 {taskName} 正在运行，"
                + $"线程id： {Thread.CurrentThread.ManagedThreadId}"
                + $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}"
        );
        Thread.Sleep(TimeSpan.FromSeconds(seconds));
        return 36 * seconds;
    }
}
