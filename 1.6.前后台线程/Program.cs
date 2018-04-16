using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._6.前后台线程
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleForeground = new ThreadSample(10);
            var sampleBackground = new ThreadSample(20);

            // 区别：进程会等待所有的前台线程完成后再结束工作，但是如果最后只剩下后台线程，则会直接结束工作
            var t1 = new Thread(sampleForeground.CountNumbers);
            t1.Name = "前台线程";
            var t2 = new Thread(sampleBackground.CountNumbers);
            t2.Name = "后台线程";
            t2.IsBackground = true;

            t1.Start();
            t2.Start();

        }
    }

    class ThreadSample
    {
        private readonly int _count;

        public ThreadSample(int count)
        {
            _count = count;
        }

        public void CountNumbers()
        {
            for (int i = 0; i < _count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine($"{Thread.CurrentThread.Name} 输出 {i}");
            }
        }
    }
}
