using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace _5._7.实现自定义的awaitable类型;

class Program
{
    static void Main(string[] args)
    {
        Task t = ProcessAsync();
        t.Wait();

        Console.ReadKey();
    }

    static async Task ProcessAsync()
    {
        var sync = new CustomAwaitable(true);
        Console.WriteLine($"{await sync}");

        var asyncTask = new CustomAwaitable(false);
        Console.WriteLine($"{await asyncTask}");
    }
}

class CustomAwaitable
{
    private readonly bool _completeSynchronously;

    public CustomAwaitable(bool completeSynchronously)
    {
        _completeSynchronously = completeSynchronously;
    }

    public CustomAwaiter GetAwaiter()
    {
        return new CustomAwaiter(_completeSynchronously);
    }
}

class CustomAwaiter : INotifyCompletion
{
    private string _result = "同步完成";
    private readonly bool _completeSynchronously;

    public bool IsCompleted
    {
        get { return _completeSynchronously; }
    }

    public CustomAwaiter(bool completeSynchronously)
    {
        _completeSynchronously = completeSynchronously;
    }

    public string GetResult()
    {
        return _result;
    }

    private string GetInfo()
    {
        return $"任务正在运行，"
            + $"线程id： {Thread.CurrentThread.ManagedThreadId}"
            + $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
    }

    public void OnCompleted(Action continuation)
    {
        ThreadPool.QueueUserWorkItem(state =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _result = GetInfo();
            continuation?.Invoke();
        });
    }
}
