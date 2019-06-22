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

namespace japanWord
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
        public int level = 4;
        public String OKWordListStr = "";

        public void Form2_GetOKMessage()
        {
            List<String> OKWordList;
            ReadWriteFile rFile = new ReadWriteFile();
            OKWordList = rFile.readFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @"_ok.txt");
            for (int i = 0; i < OKWordList.Count; i++)
            {
                OKWordListStr = OKWordListStr + "+" + OKWordList[i];
            }
            OKWordListStr = OKWordListStr + "+";
            OKWordListStr = OKWordListStr.Replace("++", "+");
        }

        //
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void addBt_Click(object sender, EventArgs e)
        {
            ReadWriteFile writeF = new ReadWriteFile();
            String keyWord = this.richTextBox1.Text;
            if (keyWord.IndexOf("\n") > 0)
            {
                keyWord = keyWord.Substring(0, keyWord.IndexOf("\n"));
            }

            if (OKWordListStr.IndexOf("+" + keyWord + "+") < 0)
            {
                OKWordListStr = OKWordListStr + keyWord + "+";

                writeF.writeFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @"_ok.txt",
                OKWordListStr, false);

                this.addBt.Visible = false;
                this.delBt.Visible = true;
            }
            
        }

        private void delBt_Click(object sender, EventArgs e)
        {
            ReadWriteFile writeF = new ReadWriteFile();
            String keyWord = this.richTextBox1.Text;
            if (keyWord.IndexOf("\n") > 0)
            {
                keyWord = keyWord.Substring(0, keyWord.IndexOf("\n"));
            }

            if (OKWordListStr.IndexOf("+" + keyWord + "+") >= 0)
            {
                OKWordListStr = OKWordListStr.Replace("+" + keyWord + "+", "+");

                writeF.writeFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @"_ok.txt",
                OKWordListStr, false);

                this.addBt.Visible = true;
                this.delBt.Visible = false;
            }
        }

        private void stopBt_Click(object sender, EventArgs e)
        {
            if (this.stopBt.Text == "||") { 
                this.stopBt.Text = ">";
            } else
            {
                this.stopBt.Text = "||";
            }
        }
    }
}
