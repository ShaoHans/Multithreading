using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _5._8.对动态类型使用await
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
            var sync = GetDynamicAwaitableObject(true);
            Console.WriteLine($"{await sync}");

            var asyncTask =  GetDynamicAwaitableObject(false);
            Console.WriteLine($"{await asyncTask}");
        }

        static dynamic GetDynamicAwaitableObject(bool completeSynchronously)
        {
            dynamic result = new ExpandoObject();
            dynamic awaiter = new ExpandoObject();

            awaiter.Message = "同步完成";
            awaiter.IsCompleted = completeSynchronously;
            awaiter.GetResult = (Func<string>)(() => awaiter.Message);
            awaiter.OnCompleted = (Action<Action>)(callback =>
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    awaiter.Message = GetInfo();
                    callback?.Invoke();
                });
            });

            IAwaiter<string> proxy = ImpromptuInterface.Impromptu.ActLike(awaiter);
            result.GetAwaiter = (Func<dynamic>)(() => proxy);

            return result;
        }

        static string GetInfo()
        {
            return $"任务正在运行，" +
                $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
        }
    }

    public interface IAwaiter<T> : INotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }
}
