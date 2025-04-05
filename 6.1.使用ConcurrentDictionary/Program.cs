using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace _6._1.使用ConcurrentDictionary
{
    class Program
    {
        const string Item = "Dictionary Item";
        public static string CurrentItem;

        static void Main(string[] args)
        {
            // ConcurrentDictionary的实现使用了细粒度锁技术
            ConcurrentDictionary<int, string> conDict = new ConcurrentDictionary<int, string>();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dict)
                {
                    dict[i] = Item;
                }
            }
            sw.Stop();
            Console.WriteLine($"往加锁的字典对象里面写入值花费时间：{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                conDict[i] = Item;
            }
            sw.Stop();
            Console.WriteLine($"往并发类型的字典对象里面写入值花费时间：{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dict)
                {
                    CurrentItem = dict[i];
                }
            }
            sw.Stop();
            Console.WriteLine($"从加锁的字典对象里面读取值花费时间：{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                CurrentItem = conDict[i];
            }
            sw.Stop();
            Console.WriteLine($"从并发类型的字典对象里面读取值花费时间：{sw.Elapsed}");

            Console.ReadKey();
        }


    }
}
