using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharos.EmbeddedIISExpress;

namespace Pharos.POS.ClientService
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //获取当前工作区宽度和高度（工作区不包含状态栏）
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            //计算窗体显示的坐标值，可以根据需要微调几个像素
            int x = ScreenWidth - this.Width - 5;
            int y = ScreenHeight - this.Height - 5;

            this.Location = new Point(x, y);

            this.Load += MainForm_Load;
            notifyIcon1.Visible = true;
            Current = this;
        }
        public static MainForm Current { get; set; }
        void MainForm_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                this.Invoke(new Action(() =>
                {
                    this.Hide();
                }));
            });
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void 进入管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var portStr = ConfigurationManager.AppSettings["ApiPort"];
            var port = 4001;
            if (string.IsNullOrEmpty(portStr) || !int.TryParse(portStr, out port))
            {
                port = 4001;
            }
            var manageSite = string.Format("http://127.0.0.1:{0}/Index.html", portStr);
            System.Diagnostics.Process.Start(manageSite);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
