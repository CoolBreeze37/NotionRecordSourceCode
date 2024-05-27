using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace C_.异步编程_原生异步方法
{
    internal class Program
    {
        public static ConcurrentDictionary<int, int> keyValuePairs = new ConcurrentDictionary<int, int>();
        static void Main(string[] args)
        {

            //Parallel.For(1, 1000, pageNumber =>
            //{
            //    Thread.Sleep(200);
            //    Console.WriteLine($"当前线程{Thread.CurrentThread.ManagedThreadId}");
            //    keyValuePairs.TryAdd(pageNumber,pageNumber);
            //    Console.WriteLine($"执行第{pageNumber}次");
            //});

            //var sw = Stopwatch.StartNew();
            //Parallel.For(0, 1000000, i =>
            //{
            //    // CPU 密集型任务
            //    double result = Math.Sqrt(i);
            //});
            //sw.Stop();
            //Console.WriteLine($"Parallel.For elapsed time: {sw.ElapsedMilliseconds} ms");

            //sw.Restart();
            //for (int i = 0; i < 1000000; i++)
            //{
            //    // CPU 密集型任务
            //    double result = Math.Sqrt(i);
            //}
            //sw.Stop();
            //Console.WriteLine($"Sequential for elapsed time: {sw.ElapsedMilliseconds} ms");

            var keyValuePairs = new ConcurrentDictionary<int, double>();

            var sw = Stopwatch.StartNew();
            Parallel.For(1, 10000, pageNumber =>
            {
                FileStream imageStream = new FileStream(Path.Combine(pageNumber + ".bmp"), FileMode.Create);
                imageStream.Close();
                //Thread.Sleep(54);
                double result = Math.Sqrt(pageNumber);
                keyValuePairs.TryAdd(pageNumber, result);
                Console.WriteLine(result);
            });
            sw.Stop();
            Console.WriteLine($"Parallel.For elapsed time: {sw.ElapsedMilliseconds} ms");

            keyValuePairs.Clear();
            sw.Restart();
            for (int pageNumber = 1; pageNumber < 1000; pageNumber++)
            {
                Thread.Sleep(10);
                double result = Math.Sqrt(pageNumber);
                keyValuePairs.TryAdd(pageNumber, result);
            }
            sw.Stop();
            Console.WriteLine($"Sequential for elapsed time: {sw.ElapsedMilliseconds} ms");
        }
    }
}
