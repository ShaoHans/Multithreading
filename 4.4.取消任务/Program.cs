using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _4._4.取消任务
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            // 传递两次cts.Token，若任务还未开始就被取消，异步操作的代码根本就不会执行
            var task1 = new Task<int>(() => TaskMethod("task1", 10, cts.Token), cts.Token);
            Console.WriteLine($"取消任务前task1的状态：{task1.Status}");
            cts.Cancel();
            Console.WriteLine($"取消任务后task1的状态：{task1.Status}");
            // 如果取消任务后再调用Start()方法启动任务，将会得到InvalidOperationException异常
            //task1.Start();
            Console.WriteLine("task1运行前被取消啦！！！");

            cts = new CancellationTokenSource();
            var task2 = new Task<int>(() => TaskMethod("task2", 10, cts.Token), cts.Token);
            task2.Start();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine($"task2的状态：{task2.Status}");
            }
            cts.Cancel();
            // 取消task2之后，task2的状态还是RanToCompletion（已成功完成执行的任务），因为从TPL的角度来说，该任务正常完成了它的工作
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine($"task2的状态：{task2.Status}");
            }

            Console.WriteLine($"task2任务结束，返回值：{task2.Result}");

            Console.ReadKey();
        }


        static int TaskMethod(string taskName, int seconds, CancellationToken token)
        {
            Console.WriteLine($"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");

            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if(token.IsCancellationRequested)
                {
                    return -1;
                }
            }

            return 36 * seconds;
        }
    }
}
