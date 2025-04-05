using System.Threading;

namespace _2._1.原子操作
{
    public class CounterWithInterlocked : CounterBase
    {
        private int _count;
        public int Count { get { return _count; } }

        public override void Decrease()
        {
            Interlocked.Decrement(ref _count);
        }

        public override void Increase()
        {
            Interlocked.Increment(ref _count);
        }
    }
}
