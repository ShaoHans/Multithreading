using System;
using System.Threading;
using System.Threading.Tasks;

namespace _4._1.创建任务
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = new Task(() => { TaskMethod("task1"); });
            var t2 = new Task(() => { TaskMethod("task2"); });
            t1.Start();
            t2.Start();

            Task.Run(() => { TaskMethod("task3"); });
            Task.Factory.StartNew(() => { TaskMethod("task4"); });
            Task.Factory.StartNew(() => { TaskMethod("task5"); }, TaskCreationOptions.LongRunning);


            Console.ReadKey();
        }

        static void TaskMethod(string taskName)
        {
            Console.WriteLine($"任务 {taskName} 正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");
        }
    }
}
