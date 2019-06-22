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

namespace japanWord
{
    public partial class Form1 : Form
    {
        //Icon ico = new Icon("APP.ico");
        
        public int delayTime = 5; //キー表示時間
        public int delayTime2 = 10;//説明表示時間
		public int index = -1; //番目

        public int status = 0; //1:start
        Thread messageThread;
        Form2 messageWin;

        HotKeys h = new HotKeys();

        List<String> wordList;


        public Form1()
        {
            InitializeComponent();

			// 热键注册 不可以放在这
			//。。。。。

			//初期化
			this.comboBox1.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["comboBox1"]);
			this.comboBox2.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["comboBox2"]);

			this.textBox1.Text = ConfigurationManager.AppSettings["textBox1"];
			this.textBox2.Text = ConfigurationManager.AppSettings["textBox2"];
			index = Convert.ToInt32(ConfigurationManager.AppSettings["index"]);

			String level = ConfigurationManager.AppSettings["level"];
			if (level == "4")
			{
				this.radioButton4.Select();
			}
			else if (level == "3")
			{
				this.radioButton3.Select();
			}
			else if (level == "2")
			{
				this.radioButton2.Select();
			}
			else
			{
				this.radioButton1.Select();
			}
		}


        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // 判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.notifyIcon1.Icon = this.Icon;
                this.notifyIcon1.Visible = true;

                // 热键
                h.Regist(this.Handle, (int)HotKeys.HotkeyModifiers.Control + (int)HotKeys.HotkeyModifiers.Alt, Keys.C, hotKey_Click);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                this.Activate();
                //任务栏区显示图标 
                this.ShowInTaskbar = true;
                //托盘区图标隐藏 
                this.notifyIcon1.Visible = false;

                //热键解消
                h.UnRegist(this.Handle, hotKey_Click);
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                myMenu.Show(MousePosition);
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            status = 1;
            if (messageWin == null || messageWin.IsDisposed)
            {
                messageWin = new Form2();
                messageWin.Show();
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            status = 0;
            if (messageWin != null)
            {
                messageWin.Close();
            }
        }

        private void hotKey_Click()
        {
//            MessageBox.Show("快捷键被调用！C");
           if (status == 0)
            {
                status = 1;
                if (messageWin == null || messageWin.IsDisposed)
                {
                    messageWin = new Form2();
                    messageWin.Show();
                }
            } else
            {
                status = 0;
                if (messageWin != null)
                {
                    messageWin.Close();
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            //窗口消息处理函数
            h.ProcessHotKey(m);
            base.WndProc(ref m);
        }

        private void setting_Click(object sender, EventArgs e)
        {
            //还原窗体显示 
            WindowState = FormWindowState.Normal;
            //激活窗体并给予它焦点 
            this.Activate();
            //任务栏区显示图标 
            this.ShowInTaskbar = true;
            //托盘区图标隐藏 
            this.notifyIcon1.Visible = false;
        }

        private void exist_Click(object sender, EventArgs e)
        {
            if (messageThread != null)
            {
                messageThread.Abort();
            }
            //messageThread.Abort();
            if (messageWin != null) { 
                messageWin.Close();
            }

            //热键解消
            h.UnRegist(this.Handle, hotKey_Click);
            this.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // レーベルが選択されない場合
            if (!this.radioButton4.Checked 
                && !this.radioButton3.Checked
                && !this.radioButton2.Checked
                && !this.radioButton1.Checked)
            {
                MessageBox.Show("Levelを設定してください");
                return;
            }

            // 時間が設定されない場合
            if (!Util.isNumberic(this.textBox1.Text, out delayTime))
            {
                MessageBox.Show("時間を設定してください");
                return;
            }

            if (!Util.isNumberic(this.textBox2.Text, out delayTime2))
            {
                MessageBox.Show("時間を設定してください");
                return;
            }

            int level = 3;
            if (this.radioButton4.Checked)
            {
                level = 4;
            } else if (this.radioButton3.Checked)
            {
                level = 3;
            }
            else if (this.radioButton2.Checked)
            {
                level = 2;
            }
            else if (this.radioButton1.Checked)
            {
                level = 1;
            }

			Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			cfa.AppSettings.Settings["textBox1"].Value = this.textBox1.Text;
			cfa.AppSettings.Settings["textBox2"].Value = this.textBox2.Text;
			cfa.AppSettings.Settings["comboBox1"].Value = this.comboBox1.SelectedIndex + "";
			cfa.AppSettings.Settings["comboBox2"].Value = this.comboBox2.SelectedIndex + "";
			cfa.AppSettings.Settings["level"].Value = level + "";
			cfa.Save();

			// 最小化 到 任务栏
			this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Visible = true;

            status = 1;


            ReadWriteFile rFile = new ReadWriteFile();
            wordList = rFile.readFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @".txt");

            if (messageWin == null || messageWin.IsDisposed)
            {
                messageWin = new Form2();
                messageWin.Show();

                messageWin.level = level;
                messageWin.Form2_GetOKMessage();

                //thread start
                messageThread = new Thread(popWinThread);
                messageThread.Start();
            }
        }

        public void popWinThread()
        {
            Random random = new Random();
            String message = "";

            while (true)
            {
                if (status == 1 && messageWin.stopBt.Text=="||")
                {
                    if (this.comboBox1.SelectedIndex == 0)
                    {
                        index = random.Next(0, wordList.Count-1);
                    } else
                    {
                        index = index + 1;
                        if (index >= wordList.Count || index < 0)
                        {
                            index = 0;
                        }
                    }

                    if (wordList != null && wordList.Count > 0)
                    {
                        while (this.comboBox1.SelectedIndex == 0 && wordList[index].IndexOf("▼") >= 0)
                        {
                            index = index - 1;
                        }

                        message = wordList[index];

                        if (messageWin.OKWordListStr.IndexOf(message) >= 0)
                        {
                            messageWin.addBt.Visible = false;
                            messageWin.delBt.Visible = true;

                            if (this.comboBox2.SelectedIndex == 0)
                            {
                                index = index + 1;
                                while (index < wordList.Count && wordList[index].IndexOf("▼") >= 0)
                                {
                                    index = index + 1;
                                }
                                index = index - 1;
                                continue;
                            }
                        }
                        else
                        {
                            messageWin.addBt.Visible = true;
                            messageWin.delBt.Visible = false;
                        }

						if (message.IndexOf("【") > 0) {
							messageWin.richTextBox1.Text = message.Substring(0, message.IndexOf("【"));
							Thread.Sleep(delayTime * 1000);
						}

                        messageWin.richTextBox1.Text = message;
                        Thread.Sleep(delayTime * 1000);



                        index = index + 1;
                        while (index < wordList.Count && wordList[index].IndexOf("▼") >= 0)
                        {
                            message = message + "\n" + wordList[index];
                            index = index + 1;
                        }
                        index = index - 1;
                        messageWin.richTextBox1.Text = message;

                        Thread.Sleep(delayTime2 * 1000);

						Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
						cfa.AppSettings.Settings["index"].Value = index - 1 + "";
						cfa.Save();
					}
                    else
                    {
                        messageWin.richTextBox1.Text = "NoWord";
                        Thread.Sleep(delayTime2 * 1000);
                        messageWin.Close();
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(delayTime2 * 1000);
                }
            }

/*            for (int i = 0; i <= messageWin.Height; i++)
                        {
                            messageWin.Location = new Point(p.X, p.Y - i);
                            Thread.Sleep(10);//将线程沉睡时间调的越小升起的越快
                        }
*/


        }
    }
}
