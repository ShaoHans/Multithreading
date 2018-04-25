using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7._4.处理PLINQ查询中的异常
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> numbers = Enumerable.Range(-5, 10);
            var query = from n in numbers select 100 / n;
            try
            {
                foreach (var q in query)
                {
                    Console.WriteLine(q);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Linq查询出现异常：{ex.Message}");
            }

            var parallelQuery = from n in numbers.AsParallel() select 100 / n;
            try
            {
                parallelQuery.ForAll(Console.WriteLine);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"PLinq查询出现异常：{ex.Message}");
            }
            catch(AggregateException ex)
            {
                ex.Flatten().Handle(e => {
                    if(e is DivideByZeroException)
                    {
                        Console.WriteLine($"捕获到：{ex.Message}");
                        return true;
                    }
                    return false;
                });
            }

            Console.ReadKey();
        }


    }
}
