using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _5._2.在lambda表达式中使用await操作符
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = ProcessAsync();
            //t.Wait();
            // 如果注释掉 t.Wait()代码后，则ProcessAsync执行完await语句后立马返回，继续执行下面的代码
            Console.WriteLine("main");
            Console.ReadKey();
        }

        async static Task ProcessAsync()
        {
            Func<string, Task<string>> asyncLambda = async taskName =>
             {
                 await Task.Delay(TimeSpan.FromSeconds(2));
                 return $"任务 {taskName} 正在运行，" +
                     $"线程id： {Thread.CurrentThread.ManagedThreadId}" +
                     $"，是否为线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}";
             };

            // 方法执行到await代码行时将立即返回，并没有阻塞程序运行
            string result = await asyncLambda("lambda task");
            // 剩余代码将在后续操作任务中运行
            Console.WriteLine(result);
        }
    }
}
