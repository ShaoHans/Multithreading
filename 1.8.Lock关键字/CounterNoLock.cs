namespace _1._8.Lock关键字;

public class CounterNoLock : CounterBase
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
