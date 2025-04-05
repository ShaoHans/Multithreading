using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace _7._5.管理PLINQ查询中的数据分区
{
    class Program
    {
        static void Main(string[] args)
        {
            StringPartitioner stringPartitioner = new StringPartitioner(GetProjects());
            var parallelQuery = from p in stringPartitioner.AsParallel() select ProcessProject(p);
            parallelQuery.ForAll(PrintProject);
            Console.ReadKey();
        }

        static IEnumerable<string> GetProjects()
        {
            IList<string> projects = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                projects.Add($"项目{i}");
            }
            return projects;
        }

        static string ProcessProject(string projectName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(200, 500)));
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}号线程处理{projectName}，" +
                $"{string.Format(projectName.Length % 2 == 0 ? "偶数" : "奇数")}长度");
            return projectName;
        }

        static void PrintProject(string projectName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine($"{projectName}在{Thread.CurrentThread.ManagedThreadId}号线程上被输出");
        }
    }

    class StringPartitioner : Partitioner<string>
    {
        private readonly IEnumerable<string> _data;
        public override bool SupportsDynamicPartitions { get { return false; } }

        public StringPartitioner(IEnumerable<string> data)
        {
            _data = data;
        }

        public override IList<IEnumerator<string>> GetPartitions(int partitionCount)
        {
            var result = new List<IEnumerator<string>>(2);
            result.Add(CreateEnumerator(true));
            result.Add(CreateEnumerator(false));
            return result;
        }

        IEnumerator<string> CreateEnumerator(bool isEven)
        {
            foreach (var d in _data)
            {
                if (!(d.Length % 2 == 0 ^ isEven))
                {
                    yield return d;
                }
            }
        }
    }
}
