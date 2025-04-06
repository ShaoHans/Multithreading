using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace _6._2.使用ConcurrentQueue实现异步处理;

class Program
{
    static void Main(string[] args)
    {
        Task t = RunProgram();
        t.Wait();
        Console.ReadKey();
    }

    static async Task RunProgram()
    {
        ConcurrentQueue<CustomTask> taskQueue = new ConcurrentQueue<CustomTask>();
        CancellationTokenSource cts = new CancellationTokenSource();

        var taskSource = Task.Run(() =>
        {
            TaskProducer(taskQueue);
        });

        Task[] processors = new Task[4];
        for (int i = 1; i <= 4; i++)
        {
            string processorId = i.ToString();
            processors[i - 1] = Task.Run(() =>
            {
                TaskProcessor(taskQueue, processorId, cts.Token);
            });
        }

        Console.WriteLine("等待所有任务生产....");
        await taskSource;
        // 若不取消，消费者会一直循环消费空队列
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        await Task.WhenAll(processors);
    }

    /// <summary>
    /// 任务生产者
    /// </summary>
    /// <param name="queue"></param>
    /// <returns></returns>
    static async Task TaskProducer(ConcurrentQueue<CustomTask> queue)
    {
        Console.WriteLine("开始生产任务");
        for (int i = 0; i <= 20; i++)
        {
            await Task.Delay(50);
            var workItem = new CustomTask() { Id = i };
            queue.Enqueue(workItem);
            Console.WriteLine($"任务{workItem.Id}已生成");
        }
        Console.WriteLine("所有任务均生产完毕");
    }

    /// <summary>
    /// 任务消费者
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="name"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    static async Task TaskProcessor(
        ConcurrentQueue<CustomTask> queue,
        string name,
        CancellationToken token
    )
    {
        CustomTask workItem;
        bool dequeueSuccesful = false;

        await GetRandomDelay();
        do
        {
            dequeueSuccesful = queue.TryDequeue(out workItem);
            if (dequeueSuccesful)
            {
                Console.WriteLine($"任务{workItem.Id}被{name}号消费者消费");
            }
            else
            {
                Console.WriteLine("消费任务失败");
            }
            await GetRandomDelay();
        } while (!token.IsCancellationRequested);
    }

    static Task GetRandomDelay()
    {
        int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
        return Task.Delay(delay);
    }
}

class CustomTask
{
    public int Id { get; set; }
}
