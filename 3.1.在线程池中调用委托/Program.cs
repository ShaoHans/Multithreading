using System;
using System.Threading;

namespace _3._1.在线程池中调用委托;

class Program
{
    // 使用BeginXXX/EndXXX和IAsyncResult对象的方式被称为异步编程模型（APM模式）

    delegate string RunOnThreadPool(out int threadId);

    static void Main(string[] args)
    {
        int threadId = 0;
        // 给委托变量赋值
        RunOnThreadPool poolDelegate = Test;

        var t = new Thread(() =>
        {
            Test(out threadId);
        });
        t.Start();
        t.Join();
        Console.WriteLine($"1.返回的线程Id：{threadId}");

        // 通过调用委托变量的BeginInvoke方法来运行委托（执行Test方法）
        IAsyncResult ar = poolDelegate.BeginInvoke(out threadId, Callback, "异步委托调用");
        ar.AsyncWaitHandle.WaitOne();
        // 调用委托的EndInvoke会等待异步操作（Test方法）完成
        // 异步操作执行完成之后，开始执行回掉函数；异步操作和回掉函数很可能会被线程池中同一个工作线程执行
        string result = poolDelegate.EndInvoke(out threadId, ar);
        Console.WriteLine($"2.返回的线程Id：{threadId}");
        Console.WriteLine($"返回值：{result}");

        Console.ReadKey();
    }

    static void Callback(IAsyncResult ar)
    {
        Console.WriteLine("开始执行回掉函数...");
        Console.WriteLine($"异步状态：{ar.AsyncState}");
        Console.WriteLine($"是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");
        Console.WriteLine($"线程池工作线程Id：{Thread.CurrentThread.ManagedThreadId}");
    }

    static string Test(out int threadId)
    {
        Console.WriteLine("开始测试方法...");
        Console.WriteLine($"是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");
        Thread.Sleep(TimeSpan.FromSeconds(2));
        threadId = Thread.CurrentThread.ManagedThreadId;
        return $"线程Id：{threadId}";
    }
}
