using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;

namespace _1._2.线程等待
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(Common.PrintNumbersWithDelay);
            thread.Start();
            
            // 等待线程
            thread.Join();

            Console.WriteLine("打印完成！");
            Console.ReadKey();
        }
    }
}
