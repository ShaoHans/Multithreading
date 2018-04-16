using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._9.使用Monitor类锁定资源
{
    public class PlayerWithMonitor
    {
        public string Name { get; private set; }

        public int Atk { get; private set; }

        public PlayerWithMonitor(string name, int atk)
        {
            Name = name;
            Atk = atk;
        }

        public void Attack(YaoGuai yaoGuai)
        {
            while (yaoGuai.Blood > 0)
            {
                Monitor.Enter(yaoGuai);
                Console.WriteLine($"我是{Name}，我来打妖怪~");
                yaoGuai.BeAttacked(Atk);
                Monitor.Exit(yaoGuai);
            }
        }
    }
}
