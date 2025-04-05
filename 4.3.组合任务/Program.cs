using System;
using System.Threading;
using System.Threading.Tasks;

namespace _4._3.组合任务;

class Program
{
    static void Main(string[] args)
    {
        var firstTask = new Task<int>(() => TaskMethod("task1", 3));
        var secondTask = new Task<int>(() => TaskMethod("task2", 2));

        firstTask.ContinueWith(
            t =>
            {
                // firstTask的后续操作
                Console.WriteLine(
                    $"task1 任务返回结果是{t.Result}，"
                        + $"线程id： {Thread.CurrentThread.ManagedThreadId}"
                        + $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}"
                );
            },
            TaskContinuationOptions.OnlyOnRanToCompletion
        );

        firstTask.Start();
        secondTask.Start();

        Thread.Sleep(TimeSpan.FromSeconds(4));
        Console.WriteLine("暂停4秒结束");

        Task continuation = secondTask.ContinueWith(
            t =>
            {
                // secondTask任务的后续操作
                Console.WriteLine(
                    $"task2 任务返回结果是{t.Result}，"
                        + $"线程id： {Thread.CurrentThread.ManagedThreadId}"
                        + $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}"
                );
            },
            TaskContinuationOptions.OnlyOnRanToCompletion
                | TaskContinuationOptions.ExecuteSynchronously
        );

        // secondTask任务的后续操作的后续操作
        continuation
            .GetAwaiter()
            .OnCompleted(() =>
            {
                Console.WriteLine(
                    $"后续操作continuation执行完成，"
                        + $"线程id： {Thread.CurrentThread.ManagedThreadId}"
                        + $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}"
                );
            });

        Thread.Sleep(TimeSpan.FromSeconds(2));
        Console.WriteLine();

        var thirdTask = new Task<int>(() =>
        {
            // 父子线程，只有所有子任务结束工作，父任务才会完成
            var innerTask = Task.Factory.StartNew(
                () => TaskMethod("task4", 5),
                TaskCreationOptions.AttachedToParent
            );
            // 也可以在子任务上运行后续操作，后续操作也会影响到父任务
            innerTask.ContinueWith(
                t => TaskMethod("task5", 3),
                TaskContinuationOptions.AttachedToParent
            );
            return TaskMethod("task3", 2);
        });
        thirdTask.Start();

        while (!thirdTask.IsCompleted)
        {
            Console.WriteLine($"任务3的状态：{thirdTask.Status}");
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
        }
        Console.WriteLine($"任务3的状态：{thirdTask.Status}");

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
