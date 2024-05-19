using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.异步编程_信号量
{
    public class AsyncWrite
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static async Task WriteDataAsync(IEnumerable<string[]> data, int channelIndex, string delimiter, StreamWriter dataWriter)
        {
            foreach (var item in data)
            {
                if (item[channelIndex] == "2")
                {
                    // Use SemaphoreSlim to ensure thread-safe and ordered writes
                    await _semaphore.WaitAsync();
                    try
                    {
                        await dataWriter.WriteLineAsync(string.Join(delimiter, item));
                        await dataWriter.FlushAsync();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }
        }

        public async Task Write()
        {
            var data = new List<string[]>
            {
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            new string[] { "1", "2", "3" },
            new string[] { "4", "2", "6" },
            new string[] { "7", "8", "2" },
            };
            string delimiter = ",";
            int channelIndex = 1;

            using (StreamWriter dataWriter = new StreamWriter("output.txt", false, Encoding.UTF8))
            {
                await WriteDataAsync(data, channelIndex, delimiter, dataWriter);
            }

            Console.WriteLine("Data written to file.");
        }
    }
}
