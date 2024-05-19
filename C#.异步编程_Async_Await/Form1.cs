using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace C_.异步编程_Async_Await
{
    /// <summary>
    /// 结论：
    /// 1.事件是按添加顺序触发的，第一个事件没有完成之前第二个事件永远不会执行
    /// 2.调用的异步方法是在其内部发生了挂起进程的操作(比如MultipleEventHandlersAsync方法的
    /// awiat Task.Delay(1000);
    /// 使用await 运算符将进程的执行挂架)，而将线程的控制权返还给调用者，调用异步方法的调用者继续向下执行
    /// 3.await Task.Delay(1000);不会阻塞当前线程
    /// 4.Task.Delay(1000).Wait()会阻塞当前线程
    /// 5.Task.Delay(1000) 只写它并不会有1000ms的等待而是立刻继续向下执行
    /// </summary>
    public partial class Form1 : Form
    {
        static readonly TaskCompletionSource<bool> s_tcs = new TaskCompletionSource<bool>();

        public Form1()
        {
            InitializeComponent();
        }
        public async Task MultipleEventHandlersAsync()
        {
            await Task.Delay(1000);
            Task<bool> secondHandlerFinished = s_tcs.Task;
            var button = new NaiveButton();

            button.Clicked += OnButtonClicked1;
            button.Clicked += OnButtonClicked2Async;
            button.Clicked += OnButtonClicked3;
            Console.WriteLine("Before button.Click() is called...");
            //textBox1.Text = textBox1.Text + "\r\n" + "Before button.Click() is called...";
            button.Click(textBox1);
            await secondHandlerFinished;
            Console.WriteLine("After button.Click() is called...");
            //textBox1.Text = textBox1.Text + "\r\n" + "After button.Click() is called...";

            
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await MultipleEventHandlersAsync();
            Console.WriteLine("结束了");
            //textBox1.Text = textBox1.Text + "\r\n" + "结束了";

        }
        private async void OnButtonClicked1(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fff")} Handler 1 is starting...");
            
            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 1 is starting...";
            await Task.Delay(1000);
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fff")}  Handler 1 is done.");
            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 1 is done.";
        }

        private async void OnButtonClicked2Async(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fff")}   Handler 2 is starting...");
            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 2 is starting...";
            Task.Delay(100);
            Console.WriteLine("   Handler 2 is about to go async...");

            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 2 is about to go async...";
            await Task.Delay(500);
            Console.WriteLine("   Handler 2 is done.");

            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 2 is done.";
            s_tcs.SetResult(true);
        }

        private void OnButtonClicked3(object sender, EventArgs e)
        {
            Console.WriteLine("   Handler 3 is starting...");
            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 3 is starting...";
            Task.Delay(100).Wait();
            Console.WriteLine("   Handler 3 is done.");

            //textBox1.Text = textBox1.Text + "\r\n" + "   Handler 3 is done.";
        }
    }
    public class NaiveButton
    {
        public event EventHandler Clicked;

        public void Click(System.Windows.Forms.TextBox textBox)
        {
            Console.WriteLine("Somebody has clicked a button. Let's raise the event...");
            //textBox.Text = textBox.Text + "\r\n" + "Somebody has clicked a button. Let's raise the event...";
            Clicked?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("All listeners are notified.");
            //textBox.Text = textBox.Text + "\r\n" + "All listeners are notified.";
        }
    }

}
