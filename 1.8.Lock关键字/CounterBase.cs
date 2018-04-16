using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._8.Lock关键字
{
    public abstract class CounterBase
    {
        public abstract void Increase();
        public abstract void Decrease();
    }
}
