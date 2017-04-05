﻿using Pharos.Logic.ApiData.Pos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharos.POS.ClientService
{
    public partial class HideFrom : Form
    {
        public HideFrom(Action callback)
        {

            InitializeComponent();
            this.Top = -500;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Load += HideFrom_Load;
            Task.Factory.StartNew(() =>
            {
                callback();
            });
            //this.VisibleChanged += HideFrom_Load;
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(2000);
            //    this.Invoke(new Action(() =>
            //    {
            //        HideFrom_Load(null, null);
            //    }));
            //});
        }

        void HideFrom_Load(object sender, EventArgs e)
        {
            for (var i = 0; i < 5; i++)
            {
                if (this.Visible)
                {
                    this.Hide();
                }
            }
        }

        private void 进入管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://127.0.0.1:4001");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Process.GetCurrentProcess().Kill();
        }

        private void 上传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StoreManager.PubEvent("SyncDatabase", "");
        }
    }
}
