using System;
using System.Threading;

namespace _3._4.取消异步操作;

class Program
{
    static void Main(string[] args)
    {
        using (var cts = new CancellationTokenSource())
        {
            CancellationToken token = cts.Token;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AsyncOperation1(token);
            });
            Thread.Sleep(TimeSpan.FromSeconds(2));
            cts.Cancel();
        }

        using (var cts = new CancellationTokenSource())
        {
            CancellationToken token = cts.Token;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AsyncOperation2(token);
            });
            Thread.Sleep(TimeSpan.FromSeconds(2));
            cts.Cancel();
        }

        using (var cts = new CancellationTokenSource())
        {
            CancellationToken token = cts.Token;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AsyncOperation3(token);
            });
            Thread.Sleep(TimeSpan.FromSeconds(2));
            cts.Cancel();
        }

        Console.ReadKey();
    }

    static void AsyncOperation1(CancellationToken token)
    {
        Console.WriteLine("启动第一个异步操作");
        for (int i = 0; i < 5; i++)
        {
            // 轮询检查IsCancellationRequested属性
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("第一个异步操作被取消啦~~~");
                return;
            }
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
        Console.WriteLine("第一个异步操作完成");
    }

    static void AsyncOperation2(CancellationToken token)
    {
        try
        {
            Console.WriteLine("启动第二个异步操作");
            for (int i = 0; i < 5; i++)
            {
                // 抛出异常，取消操作时，通过操作之外的代码来处理
                token.ThrowIfCancellationRequested();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine("第二个异步操作完成");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"第二个异步操作被取消，异常消息：{ex.Message}");
        }
    }

    static void AsyncOperation3(CancellationToken token)
    {
        bool cancellationFlag = false;
        // 注册回调函数，当操作被取消时，线程池将调用该回调函数
        token.Register(() =>
        {
            cancellationFlag = true;
        });
        Console.WriteLine("启动第三个异步操作");
        for (int i = 0; i < 5; i++)
        {
            if (cancellationFlag)
            {
                Console.WriteLine("第三个异步操作被取消啦~~~");
                return;
            }
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
        Console.WriteLine("第三个异步操作完成");
    }
}
