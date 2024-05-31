namespace WinForm.分页
{
    partial class Pagination
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PageTablePanel = new DevExpress.Utils.Layout.TablePanel();
            ((System.ComponentModel.ISupportInitialize)(this.PageTablePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // PageTablePanel
            // 
            this.PageTablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.PageTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageTablePanel.Location = new System.Drawing.Point(0, 0);
            this.PageTablePanel.Name = "PageTablePanel";
            this.PageTablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 50F)});
            this.PageTablePanel.Size = new System.Drawing.Size(1108, 585);
            this.PageTablePanel.TabIndex = 0;
            this.PageTablePanel.UseSkinIndents = true;
            // 
            // Pagination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PageTablePanel);
            this.Name = "Pagination";
            this.Size = new System.Drawing.Size(1108, 585);
            ((System.ComponentModel.ISupportInitialize)(this.PageTablePanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel PageTablePanel;
    }
}
