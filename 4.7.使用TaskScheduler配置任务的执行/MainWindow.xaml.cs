using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _4._7.使用TaskScheduler配置任务的执行
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 一般来说，UI线程拥有的对象，其他线程是无法操作的。但是.Net有一个很重要的抽象对象——TaskScheduler（任务调度器）。
        /// 它协调着不同任务（线程）的运行，使得线程池中的线程有了操作UI线程的可能。
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;
            try
            {
                string result = TaskMethod(TaskScheduler.Default).Result;
                ContentTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                ContentTextBlock.Text = ex.InnerException.Message;
            }
        }

        private void Async_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;

            //这个函数主要就是这里用了Default，所以用了线程池线程，而TaskMethod里面有操作UI对象，因此出错！  
            Task<string> task = TaskMethod(TaskScheduler.Default);
            task.ContinueWith(t =>
            {
                //这里的后续操作是在UI线程中做的，没有出错。  
                ContentTextBlock.Text = t.Exception.InnerException.Message;
                Mouse.OverrideCursor = null;
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncOk_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;
            // task 并没有运行在线程池中，而是 FromCurrentSynchronizationContext  
            Task<string> task = TaskMethod(TaskScheduler.FromCurrentSynchronizationContext());
            //这句话将让UI线程等待直到UI线程完成task中的内容，但是等待中的UI线程没有办法操作，因此死锁！ 
            //string s = task.Result; 
            task.ContinueWith(t => Mouse.OverrideCursor = null,
            CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 这个TaskMethod方法有一个TaskScheduler参数，方法内，先Delay 5秒钟，类似于sleep。
        /// 然后在delay任务完成后做后续操作，返回当前任务的相关信息到str字符串，
        /// 并且将ContentTextBlock.Text属性修改为str字符串。需要注意的是，
        /// 根据不同scheduler，该后续操作将在不同的线程上运行（可能是线程池线程，也可能是UI线程），
        /// 但是请注意线程池线程是无法完成上述UI线程操作的（因为ContentTextBlock是UI线程中的对象）
        /// </summary>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        Task<string> TaskMethod(TaskScheduler scheduler)
        {
            Task delay = Task.Delay(5000);

            return delay.ContinueWith(t =>
            {
                string str = string.Format("Task is running on a thread id {0}. Is thread pool thread: {1}",
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
                ContentTextBlock.Text = str;
                return str;
            }, scheduler);
        }
    }
}
