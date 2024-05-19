using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.异步编程_信号量
{
    public class DualProducerConsumer
    {
        private ConcurrentQueue<int> _queue1 = new ConcurrentQueue<int>();
        private ConcurrentQueue<int> _queue2 = new ConcurrentQueue<int>();
        private SemaphoreSlim _semaphore1 = new SemaphoreSlim(0);
        private SemaphoreSlim _semaphore2 = new SemaphoreSlim(0);
        private ConcurrentQueue<(int, int)> _combinedQueue = new ConcurrentQueue<(int, int)>();
        private volatile bool _acceptingData = true; // 标志是否接受新数据

        // 生产者：向队列1中添加新值
        public void EnqueueToQueue1(int item)
        {
            if (_acceptingData)
            {
                _queue1.Enqueue(item);
                _semaphore1.Release(); // 通知消费者有新值
            }
        }

        // 生产者：向队列2中添加新值
        public void EnqueueToQueue2(int item)
        {
            if (_acceptingData)
            {
                _queue2.Enqueue(item);
                _semaphore2.Release(); // 通知消费者有新值
            }
        }

        // 停止接受新数据
        public void StopAcceptingData()
        {
            _acceptingData = false;
        }

        // 消费者：从队列1和队列2中读取值并合成
        public async Task ConsumeAndCombineAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    // 等待两个信号量
                    await _semaphore1.WaitAsync(cancellationToken);
                    await _semaphore2.WaitAsync(cancellationToken);
                    //await Task.WhenAll(_semaphore1.WaitAsync(cancellationToken),_semaphore2.WaitAsync(cancellationToken));

                    if (_queue1.TryDequeue(out int item1) && _queue2.TryDequeue(out int item2))
                    {
                        var combined = (item1, item2);
                        _combinedQueue.Enqueue(combined);
                        Process(combined);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 当任务被取消时，优雅地退出循环
                Console.WriteLine("消费任务已取消。");
            }

            // 处理剩余的数据
            while (_queue1.TryDequeue(out int remainingItem1) && _queue2.TryDequeue(out int remainingItem2))
            {
                var combined = (remainingItem1, remainingItem2);
                _combinedQueue.Enqueue(combined);
                Process(combined);
            }
        }

        // 处理合成的数据
        private void Process((int, int) combinedData)
        {
            Console.WriteLine($"Processing combined data: {combinedData.Item1}, {combinedData.Item2}");
        }

        public async void Start()
        {
            Console.WriteLine("线程中 " + Thread.CurrentThread.ManagedThreadId);
            DualProducerConsumer? dualProducerConsumer = new DualProducerConsumer();
            CancellationTokenSource? cts = new CancellationTokenSource();
            Console.WriteLine("线程中 " + Thread.CurrentThread.ManagedThreadId);
            // 启动消费者任务
            var consumerTask = dualProducerConsumer.ConsumeAndCombineAsync(cts.Token);

            // 模拟生产者添加值到队列1和队列2
            for (int i = 0; i < 1000; i++)
            {
                dualProducerConsumer.EnqueueToQueue1(i);
                dualProducerConsumer.EnqueueToQueue2(i * 2);
                //await Task.Delay(0); // 模拟生产间隔
                Thread.Sleep(0);
            }

            // 停止接受新数据并取消消费者任务  
            dualProducerConsumer.StopAcceptingData();
            cts.Cancel();

            // 等待消费者任务处理完剩余数据
            await consumerTask;

            Console.WriteLine("Completed");
        }
    }
}
