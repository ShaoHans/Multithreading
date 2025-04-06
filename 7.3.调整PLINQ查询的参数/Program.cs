using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace _7._3.调整PLINQ查询的参数;

class Program
{
    static void Main(string[] args)
    {
        var parallelQuery = from p in GetProjects().AsParallel() select ProcessProject(p);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(1));

        try
        {
            parallelQuery
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .WithExecutionMode(ParallelExecutionMode.Default)
                .WithMergeOptions(ParallelMergeOptions.Default)
                .WithCancellation(cts.Token)
                .ForAll(PrintProject);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("并行查询被取消");
        }

        Console.WriteLine("无序并行查询");
        var unOrderedQuery = from i in ParallelEnumerable.Range(1, 30) select i;
        foreach (var i in unOrderedQuery)
        {
            Console.WriteLine(i);
        }

        Console.WriteLine("有序并行查询");
        var orderedQuery = from i in ParallelEnumerable.Range(1, 30).AsOrdered() select i;
        foreach (var i in orderedQuery)
        {
            Console.WriteLine(i);
        }

        Console.ReadKey();
    }

    static IEnumerable<string> GetProjects()
    {
        for (int i = 1; i <= 20; i++)
        {
            yield return $"项目{i}";
        }
    }

    static string ProcessProject(string projectName)
    {
        Thread.Sleep(
            TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(200, 500))
        );
        Console.WriteLine($"{Environment.CurrentManagedThreadId}号线程处理{projectName}");
        return projectName;
    }

    static void PrintProject(string projectName)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(150));
        Console.WriteLine($"{projectName}在{Environment.CurrentManagedThreadId}号线程上被输出");
    }
}
