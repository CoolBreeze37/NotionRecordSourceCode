using System.Collections.Concurrent;

namespace C_.异步编程_信号量
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //#region Demo1
            //Console.WriteLine("Main线程"+Thread.CurrentThread.ManagedThreadId);
            //Task.Run(()=>new DualProducerConsumer.Start());
            //Console.WriteLine("我继续向下走。。。");
                      
            //Task.Delay(10000).Wait();
            //#endregion

            #region Demo2
            AsyncWrite asyncWrite = new AsyncWrite();
            Task.Run(() => asyncWrite.Write());
            Task.Delay(10000).Wait();
            #endregion
        }
        
    }

}
