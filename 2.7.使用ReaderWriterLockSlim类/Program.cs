using System;
using System.Collections.Generic;
using System.Threading;

namespace _2._7.使用ReaderWriterLockSlim类
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();

            new Thread(() => Write("线程 1"))
            {
                IsBackground = true
            }.Start();
            new Thread(() => Write("线程 2"))
            {
                IsBackground = true
            }.Start();

            Thread.Sleep(TimeSpan.FromSeconds(30));

            Console.ReadKey();
        }

        //读写锁的概念很简单，允许多个线程同时获取读锁，但同一时间只允许一个线程获得写锁，因此也称作共享-独占锁。
        //在C#中，推荐使用ReaderWriterLockSlim类来完成读写锁的功能。
        //某些场合下，对一个对象的读取次数远远大于修改次数，如果只是简单的用lock方式加锁，则会影响读取的效率。
        //而如果采用读写锁，则多个线程可以同时读取该对象，只有等到对象被写入锁占用的时候，才会阻塞。
        private static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        private static Dictionary<int, int> _item = new Dictionary<int, int>();

        private static void Read()
        {
            Console.WriteLine("①读取字典的内容");
            while (true)
            {
                try
                {
                    //尝试进入读取模式锁定状态。
                    _rw.EnterReadLock();
                    foreach (var key in _item.Keys)
                    {
                        Console.WriteLine("③读取模式---键 {0} ", key);
                        Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    //减少读取模式的递归计数，并在生成的计数为 0（零）时退出读取模式。
                    _rw.ExitReadLock();
                }
            }
        }

        private static void Write(string threadName)
        {
            while (true)
            {
                try
                {
                    int newKey = new Random().Next(250);
                    //可升级读模式，这种方式和读模式的区别在于它还有通过调用 EnterWriteLock 或 TryEnterWriteLock 方法升级为写入模式
                    //因为每次只能有一个线程处于可升级模式。进入可升级模式的线程，不会影响读取模式的线程，即当一个线程进入可升级模式，
                    //任意数量线程可以同时进入读取模式，不会阻塞。
                    //如果有多个线程已经在等待获取写入锁，那么运行EnterUpgradeableReadLock将会阻塞，直到那些线程超时或者退出写入锁。
                    _rw.EnterUpgradeableReadLock();
                    if (!_item.ContainsKey(newKey))
                    {
                        try
                        {
                            //尝试进入写入模式锁定状态
                            _rw.EnterWriteLock();
                            _item[newKey] = 1;
                            Console.WriteLine("②新键 {0} 添加到字典中通过 {1}", newKey, threadName);
                        }
                        finally
                        {
                            //减少写入模式的递归计数，并在生成的计数为 0（零）时退出写入模式。
                            _rw.ExitWriteLock();
                        }
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
                finally
                {
                    _rw.ExitUpgradeableReadLock();
                }
            }
        }
    }
}
