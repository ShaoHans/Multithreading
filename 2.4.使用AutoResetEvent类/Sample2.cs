using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace _2._4.使用AutoResetEvent类
{
    class Sample2
    {
        public static void Start()
        {
            RemoteRequest request = new RemoteRequest();

            new Thread(request.RequestInterfaceA).Start();
            new Thread(request.RequestInterfaceB).Start();

            AutoResetEvent.WaitAll(request._resetEvents.ToArray());

            request.RequestInterfaceC();

        }
    }

    class RemoteRequest
    {
        public IList<AutoResetEvent> _resetEvents;

        public RemoteRequest()
        {
            _resetEvents = new List<AutoResetEvent>();
            _resetEvents.Add(new AutoResetEvent(false));
            _resetEvents.Add(new AutoResetEvent(false));
        }

        public void RequestInterfaceA()
        {
            Console.WriteLine("异步调用远程接口A获取用户数据...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _resetEvents[0].Set();
            Console.WriteLine("接口A数据获取完成！");
        }

        public void RequestInterfaceB()
        {
            Console.WriteLine("异步调用远程接口B获取订单数据...");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _resetEvents[1].Set();
            Console.WriteLine("接口B订单数据获取完成！");
        }

        public void RequestInterfaceC()
        {
            Console.WriteLine("接口A和接口B的数据获取完成，开始处理C接口数据...");
        }
    }
}
