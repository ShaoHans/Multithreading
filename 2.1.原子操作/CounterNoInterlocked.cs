using System;
using System.Collections.Generic;
using System.Text;

namespace _2._1.原子操作
{
    public class CounterNoInterlocked : CounterBase
    {
        public int Count { get; private set; }
        public override void Decrease()
        {
            Count--;
        }

        public override void Increase()
        {
            Count++;
        }
    }
}
