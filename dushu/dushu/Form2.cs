using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dushu
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

//            this.MdiParent = this;
            this.TopMost = true;
//            this.Dock = DockStyle.Fill;
            this.ShowInTaskbar = false;

            //Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            Point p = new Point(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.PointToScreen(p);
            this.Location = p;
        }

        //levle
        public String level = "4";

		public int line_pre; //行数
		public int wordNum_pre; //
		public int wordNumCount_pre; //

		Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
		private void label1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				mouseOff = new Point(-e.X, -e.Y); //得到变量的值
				leftFlag = true;                  //点击左键按下时标注为true;
			}
			else if (e.Button == MouseButtons.Right)
			{

			}
		}
		private void label1_MouseMove(object sender, MouseEventArgs e)
		{
			if (leftFlag)
			{
				Point mouseSet = Control.MousePosition;
				mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
				Location = mouseSet;
			}
		}

		private void label1_MouseUp(object sender, MouseEventArgs e)
		{
			if (leftFlag)
			{
				leftFlag = false;//释放鼠标后标注为false;
			}
		}

		private void label1_MouseEnter(object sender, EventArgs e)
		{
			this.label1.BackColor = Color.FromArgb(10,Color.Gray);
		}

		private void label1_MouseLeave(object sender, EventArgs e)
		{
			this.label1.BackColor = System.Drawing.SystemColors.Control;
		}

		private void setting_Click(object sender, EventArgs e)
		{
			Form1 f1 = (Form1)this.Owner;
			f1.setting_Click(sender,e);
		}

		private void exist_Click(object sender, EventArgs e)
		{
			Form1 f1 = (Form1)this.Owner;
			f1.exist_Click(sender, e);
		}

		private void label1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				myMenu.Show(MousePosition);
			}
			else
			{
				if (myMenu.Visible)
				{
					myMenu.Close();
				}
			}
		}
	}
}
