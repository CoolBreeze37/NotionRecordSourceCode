using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm.分页
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridView;
        private Button btnPrevious, btnNext;
        private Label lblPageInfo;

        private DataTable dataTable;
        private int pageSize = 20;
        private int currentPageIndex = 1;
        private int totalPageCount;
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // 初始化控件
            dataGridView = new DataGridView { Dock = DockStyle.Top, Height = 400 };
            btnPrevious = new Button { Text = "Previous", Dock = DockStyle.Left, Enabled = false };
            btnNext = new Button { Text = "Next", Dock = DockStyle.Right };
            lblPageInfo = new Label { Text = "Page 1", Dock = DockStyle.Bottom, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

            btnPrevious.Click += BtnPrevious_Click;
            btnNext.Click += BtnNext_Click;

            Controls.Add(dataGridView);
            Controls.Add(btnPrevious);
            Controls.Add(btnNext);
            Controls.Add(lblPageInfo);

            // 初始化数据源
            InitializeDataTable();
            totalPageCount = (int)Math.Ceiling(dataTable.Rows.Count / (double)pageSize);
            LoadPage();
        }

        private void InitializeDataTable()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));

            for (int i = 1; i <= 1000; i++)
            {
                dataTable.Rows.Add(i, "Name " + i);
            }
        }

        private void LoadPage()
        {
            var pagedTable = dataTable.AsEnumerable()
                .Skip((currentPageIndex - 1) * pageSize)
                .Take(pageSize)
                .CopyToDataTable();

            dataGridView.DataSource = pagedTable;
            lblPageInfo.Text = $"Page {currentPageIndex} of {totalPageCount}";

            btnPrevious.Enabled = currentPageIndex > 1;
            btnNext.Enabled = currentPageIndex < totalPageCount;
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 1)
            {
                currentPageIndex--;
                LoadPage();
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (currentPageIndex < totalPageCount)
            {
                currentPageIndex++;
                LoadPage();
            }
        }

    }
}
