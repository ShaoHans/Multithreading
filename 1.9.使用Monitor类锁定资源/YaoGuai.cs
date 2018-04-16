using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._9.使用Monitor类锁定资源
{
    /// <summary>
    /// 妖怪
    /// </summary>
    public class YaoGuai
    {
        public int Blood { get; private set; }

        public YaoGuai(int blood)
        {
            Blood = blood;
            Console.WriteLine($"我是妖怪，我有{Blood}滴血！");
        }

        public void BeAttacked(int attack)
        {
            if (Blood > 0)
            {
                Blood = Blood - attack;
                if (Blood < 0)
                {
                    Blood = 0;
                }
            }
            Console.WriteLine($"我是妖怪，我剩余{Blood}滴血！");
        }
    }
}
