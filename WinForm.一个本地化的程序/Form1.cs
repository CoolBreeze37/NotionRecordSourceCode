using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm.一个本地化的程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void SetCulture(string cultureName)
        {
            // 设置当前线程的文化信息
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            // 重新加载控件以应用新的文化信息
            var resources = new ComponentResourceManager(typeof(Form1));
            resources.ApplyResources(this, "$this");
            ApplyResources(this, resources);
        }

        private void ApplyResources(Control control, ComponentResourceManager resources)
        {
            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                ApplyResources(c, resources);
            }
        }

        private void buttonEnglish_Click(object sender, EventArgs e)
        {
            SetCulture("en-US");
        }

        private void buttonFrench_Click(object sender, EventArgs e)
        {
            SetCulture("fr-FR");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new Form2();
            form.Show();
        }
    }
}
