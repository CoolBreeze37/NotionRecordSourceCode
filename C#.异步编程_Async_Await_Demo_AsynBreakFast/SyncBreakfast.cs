using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_.异步编程_Async_Await_Demo_AsynBreakFast
{
    //同步准备的早餐大约需要 30 分钟，因为总数是每项任务的总和。
    //计算机不会像人们那样解释这些指令。计算机将阻止每个语句，直到工作完成，然后再继续下一个语句。
    //这造成了不令人满意的早餐。在完成前面的任务之前，不会启动后面的任务。制作早餐需要更长的时间，有些食物在上桌之前会变冷。
    //如果希望计算机异步执行上述指令，则必须编写异步代码。
    //这些问题对于您今天编写的程序很重要。编写客户端程序时，希望 UI 能够响应用户输入。
    //应用程序不应使手机在从 Web 下载数据时显示为冻结状态。编写服务器程序时，不希望线程被阻塞。
    //这些线程可能正在处理其他请求。当存在异步替代代码时，使用同步代码会损害您以较低的成本向外扩展的能力。您需要为这些被阻止的线程付费。
    //成功的现代应用程序需要异步代码。如果没有语言支持，编写异步代码需要回调、完成事件或其他模糊代码原始意图的方法。
    //同步代码的优点是其分步操作使其易于扫描和理解。传统的异步模型迫使你关注代码的异步性质，而不是代码的基本操作。
    public class SyncBreakfast
    {
        public void Start()
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
}
