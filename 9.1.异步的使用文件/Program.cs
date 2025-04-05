using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _9._1.异步的使用文件
{
    class Program
    {
        const int Buffer_Size = 4096;

        static void Main(string[] args)
        {
            Task t = ProcessIOAsync();
            t.Wait();
            ThreadPool.GetMaxThreads(out int workerThreads, out int ioThreads);
            Console.WriteLine($"{workerThreads},{ioThreads}");
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"{workerThreads},{ioThreads}");

            Console.ReadKey();
        }

        static string CreateFileContent()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100000; i++)
            {
                sb.AppendLine($"{new Random(DateTime.Now.Millisecond).Next(0, 99999)}");
            }
            return sb.ToString();
        }

        async static Task<long> SumFileContent(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, Buffer_Size, FileOptions.Asynchronous))
            using (var sr = new StreamReader(stream))
            {
                long sum = 0;
                while (sr.Peek() > -1)
                {
                    string line = await sr.ReadLineAsync();
                    sum += long.Parse(line);
                }
                return sum;
            }
        }

        async static Task ProcessIOAsync()
        {
            using (FileStream stream = new FileStream("test1.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, Buffer_Size))
            {
                Console.WriteLine($"1.使用I/O线程，线程Id：{Thread.CurrentThread.ManagedThreadId}，是否异步：{stream.IsAsync}");
                byte[] buffer = Encoding.UTF8.GetBytes(CreateFileContent());
                var writeTask = Task.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, 0, buffer.Length, null);
                await writeTask;
            }

            using (FileStream stream = new FileStream("test2.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, Buffer_Size, FileOptions.Asynchronous))
            {
                Console.WriteLine($"2.使用I/O线程，线程Id：{Thread.CurrentThread.ManagedThreadId}，是否异步：{stream.IsAsync}");
                byte[] buffer = Encoding.UTF8.GetBytes(CreateFileContent());
                var writeTask = Task.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, 0, buffer.Length, null);
                await writeTask;
            }

            using (var stream = File.Create("test3.txt", Buffer_Size, FileOptions.Asynchronous))
            using (var sw = new StreamWriter(stream))
            {
                Console.WriteLine($"3.使用I/O线程，线程Id：{Thread.CurrentThread.ManagedThreadId}，是否异步：{stream.IsAsync}");
                await sw.WriteAsync(CreateFileContent());
            }

            using (var sw = new StreamWriter("test4.txt", false))
            {
                Console.WriteLine($"4.使用I/O线程，线程Id：{Thread.CurrentThread.ManagedThreadId}，是否异步：{((FileStream)sw.BaseStream).IsAsync}");
                await sw.WriteAsync(CreateFileContent());
            }

            Console.WriteLine("开始并行解析文件内容");

            Task<long>[] readTasks = new Task<long>[4];
            for (int i = 0; i < readTasks.Length; i++)
            {
                readTasks[i] = SumFileContent($"test{i + 1}.txt");
            }

            long[] sums = await Task.WhenAll(readTasks);
            Console.WriteLine($"所有文件中数字总和是：{sums.Sum()}");
        }
    }
}
