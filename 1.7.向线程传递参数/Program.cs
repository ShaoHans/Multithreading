using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._7.向线程传递参数
{
    class Program
    {
        static void Main(string[] args)
        {
            var sample = new ThreadSample(10);
            var t1 = new Thread(sample.CountNumbers);
            t1.Name = "t1";
            t1.Start();
            t1.Join();
            Console.WriteLine("===================================");

            var t2 = new Thread(Count);
            t2.Name = "t2";
            t2.Start(8);
            t2.Join();
            Console.WriteLine("===================================");

            // 使用lambda表达式引用另一个C#对象的方式被称为闭包
            var t3 = new Thread(() => { CountNumbers(9); });
            t3.Name = "t3";
            t3.Start();
            t3.Join();
            Console.WriteLine("===================================");

            // 在lambda表达式中使用任何局部变量时，C#会生成一个类，并将该变量作为该类的一个属性
            // 共享该变量值
            int i = 10;
            var t4 = new Thread(() => { PrintNumbers(i); });
            t4.Name = "t4";
            i = 12;
            var t5 = new Thread(() => { PrintNumbers(i); });
            t5.Name = "t4";
            t4.Start();
            t5.Start();
            Console.ReadKey();
        }

        static void CountNumbers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine($"线程{Thread.CurrentThread.Name}输出{i}");
            }
        }

        static void Count(object obj)
        {
            CountNumbers((int)obj);
        }

        static void PrintNumbers(int number)
        {
            Console.WriteLine($"线程{Thread.CurrentThread.Name}输出{number}");
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
