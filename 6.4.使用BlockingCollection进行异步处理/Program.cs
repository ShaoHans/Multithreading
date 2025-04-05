using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _6._4.使用BlockingCollection进行异步处理
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("BlockingCollection默认是使用并发队列容器实现");
            //Task t = RunProgram();
            //t.Wait();

            Console.WriteLine("BlockingCollection传递参数通过使用并发堆栈容器实现");
            Task t = RunProgram(new ConcurrentStack<CustomTask>());
            t.Wait();
            Console.ReadKey();
        }

        static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
        {
            BlockingCollection<CustomTask> taskCollection = new BlockingCollection<CustomTask>();
            if (null != collection)
            {
                taskCollection = new BlockingCollection<CustomTask>(collection);
            }

            var taskSource = Task.Run(() => { TaskProducer(taskCollection); });

            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProcessor(taskCollection, processorId);
                });
            }

            Console.WriteLine("等待所有任务生产....");
            await taskSource;
            await Task.WhenAll(processors);
        }

        /// <summary>
        /// 任务生产者
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        static async Task TaskProducer(BlockingCollection<CustomTask> collection)
        {
            Console.WriteLine("开始生产任务");
            for (int i = 0; i <= 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask() { Id = i };
                collection.Add(workItem);
                Console.WriteLine($"任务{workItem.Id}已生成");
            }
            collection.CompleteAdding();
            Console.WriteLine("所有任务均生产完毕");
        }

        /// <summary>
        /// 任务消费者
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static async Task TaskProcessor(BlockingCollection<CustomTask> collection, string name)
        {
            await GetRandomDelay();
            foreach (CustomTask workItem in collection.GetConsumingEnumerable())
            {
                Console.WriteLine($"任务{workItem.Id}被{name}号消费者消费");
                await GetRandomDelay();
            }
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
}
