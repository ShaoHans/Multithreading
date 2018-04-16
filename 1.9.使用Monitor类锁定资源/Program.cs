using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._9.使用Monitor类锁定资源
{
    class Program
    {
        static void Main(string[] args)
        {
            YaoGuai yaoGuai = new YaoGuai(1000);
            Player p1 = new Player("孙悟空", 300);
            Player p2 = new Player("猪八戒", 180);

            Thread t1 = new Thread(() => { p1.Attack(yaoGuai); });
            Thread t2 = new Thread(() => { p2.Attack(yaoGuai); });
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Console.WriteLine("===============================");

            YaoGuai yaoGuai2 = new YaoGuai(1000);
            PlayerWithMonitor p3 = new PlayerWithMonitor("孙悟空", 300);
            PlayerWithMonitor p4 = new PlayerWithMonitor("猪八戒", 180);

            Thread t3 = new Thread(() => { p3.Attack(yaoGuai2); });
            Thread t4 = new Thread(() => { p4.Attack(yaoGuai2); });
            t3.Start();
            t4.Start();
            t3.Join();
            t4.Join();

            Console.ReadKey();
        }
    }
}
