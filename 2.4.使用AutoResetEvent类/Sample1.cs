using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _2._4.使用AutoResetEvent类
{
    class Sample1
    {
        //false：无信号，子线程的WaitOne方法不会被自动调用；
        //true：有信号，子线程的WaitOne方法会被自动调用。
        static AutoResetEvent _resetEvent = new AutoResetEvent(false);
        static int number = -1;

        public static void Start()
        {
            Thread t = new Thread(ReadThreadProc);
            t.Name = "Read";
            t.Start();

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Wirte线程写入数字：{i}");
                number = i;
                
                //发信号，通知正在等待的读线程写入操作已完成
                _resetEvent.Set();

                // 暂停1毫秒，给读线程时间读取数字
                Thread.Sleep(1);
            }
            //t.Abort();
        }

        static void ReadThreadProc()
        {
            while (true)
            {
                //读线程等待写线程写入数字
                _resetEvent.WaitOne();
                Console.WriteLine($"{Thread.CurrentThread.Name}线程读取到数字：{number}");
            }
        }
    }
}
