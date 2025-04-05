using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace _8._2.编写自定义的可观察对象
{
    class Program
    {
        static void Main(string[] args)
        {
            var observer = new CustomObserver();

            var goodObservable = new CustomSequence(new int[] { 1, 2, 3, 4, 5, 6, 7 });
            var badObservable = new CustomSequence(null);

            using (IDisposable d = goodObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Thread.Sleep(100);
            }

            using (IDisposable d = badObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Thread.Sleep(100);
            }


            Console.ReadKey();
        }
    }

    class CustomObserver : IObserver<int>
    {
        public void OnCompleted()
        {
            Console.WriteLine("观察结束");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"出现异常：{error.Message}");
        }

        public void OnNext(int value)
        {
            Console.WriteLine($"next value:{value}，线程Id：{Thread.CurrentThread.ManagedThreadId}");
        }
    }

    class CustomSequence : IObservable<int>
    {
        private readonly IEnumerable<int> _numbers;

        public CustomSequence(IEnumerable<int> numbers)
        {
            _numbers = numbers;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            foreach (var n in _numbers)
            {
                observer.OnNext(n);
            }

            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
