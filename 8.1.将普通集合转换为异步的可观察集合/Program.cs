using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace _8._1.将普通集合转换为异步的可观察集合
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var i in GenerateIntList())
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("IEnumerable");


            IObservable<int> o = GenerateIntList().ToObservable();
            using (IDisposable sub = o.Subscribe(Console.WriteLine))
            {
                Console.WriteLine("IObservable");
            }

            IObservable<int> o2 = GenerateIntList().ToObservable().SubscribeOn(TaskPoolScheduler.Default);
            using (IDisposable sub = o2.Subscribe(Console.WriteLine))
            {
                //Thread.Sleep(1000);
                Console.WriteLine("IObservable async");
            }
            Console.ReadKey();
        }

        static IEnumerable<int> GenerateIntList()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                yield return i;
            }
        }
    }
}
