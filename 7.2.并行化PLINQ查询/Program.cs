using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace _7._2.并行化PLINQ查询;

class Program
{
    static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        // 1
        var query = from p in GetProjects() select ProcessProject(p);
        foreach (var name in query)
        {
            PrintProject(name);
        }
        sw.Stop();
        Console.WriteLine($"============使用LINQ查询时间：{sw.ElapsedMilliseconds}");

        sw.Restart();
        // 2
        // 不同的线程处理不同的项目
        var parallelQuery =
            from p in ParallelEnumerable.AsParallel(GetProjects())
            select ProcessProject(p);
        foreach (var name in parallelQuery)
        {
            PrintProject(name);
        }
        sw.Stop();
        Console.WriteLine($"============使用PLINQ查询，查得的结果在一个线程上输出，总共花费时间：{sw.ElapsedMilliseconds}");

        sw.Restart();
        // 3
        // 单个处理项目和打印项目是同一个线程
        parallelQuery = from p in GetProjects().AsParallel() select ProcessProject(p);
        parallelQuery.ForAll(PrintProject);
        sw.Stop();
        Console.WriteLine($"============使用PLINQ查询，查得的结果并行输出，总共花费时间：{sw.ElapsedMilliseconds}");

        sw.Restart();
        // 4 和第1种情况一样
        query = from p in GetProjects().AsParallel().AsSequential() select ProcessProject(p);
        foreach (var name in query)
        {
            PrintProject(name);
        }
        sw.Stop();
        Console.WriteLine($"============使用PLINQ查询，转成顺序计算，查得的结果在一个线程上输出，总共花费时间：{sw.ElapsedMilliseconds}");

        Console.ReadKey();
    }

    static IEnumerable<string> GetProjects()
    {
        for (int i = 1; i <= 20; i++)
        {
            yield return $"项目{i}";
        }
    }

    static void PrintProject(string projectName)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(150));
        Console.WriteLine($"{projectName}在{Environment.CurrentManagedThreadId}号线程上被输出");
    }

    static string ProcessProject(string projectName)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(150));
        Console.WriteLine($"{Environment.CurrentManagedThreadId}号线程处理{projectName}");
        return projectName;
    }
}
