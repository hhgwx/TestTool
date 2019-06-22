using ForAll;
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

namespace dushu
{
    public partial class Form1 : Form
    {
		//Icon ico = new Icon("APP.ico");
		private string ToolName = "dushu";

		public int delayTime = 5; //キー表示時間
		public int wordNum = 10; //文字数
		public Boolean isHidden = false; //true:隐藏
		public Boolean isStoped = false; //true:停止
		public Boolean isPrev = false; //true:向前翻
		Thread messageThread;
        Form2 messageWin;
		String level = "3";

		HotKeys hotkey = new HotKeys();

        List<String> wordList;


        public Form1()
        {
            InitializeComponent();
			init();

			// 热键注册 不可以放在这
			//。。。。。
		}

		private void init()
		{
			this.Height = this.Height - this.panel1.Height;

			RegeditMng myReg = new RegeditMng(ToolName);
			level = myReg.getRegValue("level");

			if (level.Length==0)
			{
				level = "1";
				this.textBox1.Text = "4";
				this.textBox2.Text = "15";
				this.text_nowLine.Text = "0";
				this.checkBox1.Checked = false;

				this.hidden_ctrl.Checked = true;
				this.hidden_alt.Checked = true;
				this.hidden_shift.Checked = false;
				this.hidden_key.Text = "C";

				this.stop_ctrl.Checked = true;
				this.stop_alt.Checked = true;
				this.stop_shift.Checked = false;
				this.stop_key.Text = "S";

				this.pre_ctrl.Checked = true;
				this.pre_alt.Checked = true;
				this.pre_shift.Checked = false;
				this.pre_key.Text = "←";

				this.next_ctrl.Checked = true;
				this.next_alt.Checked = true;
				this.next_shift.Checked = false;
				this.next_key.Text = "→";

				this.button_fontColor.ForeColor = Color.Black;
				this.comboBox_fontSize.Text = "9";
			}
			else
			{
				try
				{
					this.textBox1.Text = myReg.getRegValue("time_" + level);
					this.textBox2.Text = myReg.getRegValue("wordNum_" + level);
					this.text_nowLine.Text = myReg.getRegValue("line_" + level);
					this.checkBox1.Checked = myReg.getRegBoolValue("base64_" + level);

					this.hidden_ctrl.Checked = myReg.getRegBoolValue("hidden_ctrl");
					this.hidden_alt.Checked = myReg.getRegBoolValue("hidden_alt");
					this.hidden_shift.Checked = myReg.getRegBoolValue("hidden_shift");
					this.hidden_key.Text = myReg.getRegValue("hidden_key");

					this.stop_ctrl.Checked = myReg.getRegBoolValue("stop_ctrl");
					this.stop_alt.Checked = myReg.getRegBoolValue("stop_alt");
					this.stop_shift.Checked = myReg.getRegBoolValue("stop_shift");
					this.stop_key.Text = myReg.getRegValue("stop_key");

					this.pre_ctrl.Checked = myReg.getRegBoolValue("pre_ctrl");
					this.pre_alt.Checked = myReg.getRegBoolValue("pre_alt");
					this.pre_shift.Checked = myReg.getRegBoolValue("pre_shift");
					this.pre_key.Text = myReg.getRegValue("pre_key");

					this.next_ctrl.Checked = myReg.getRegBoolValue("next_ctrl");
					this.next_alt.Checked = myReg.getRegBoolValue("next_alt");
					this.next_shift.Checked = myReg.getRegBoolValue("next_shift");
					this.next_key.Text = myReg.getRegValue("next_key");

					this.button_fontColor.ForeColor = ColorTranslator.FromHtml(myReg.getRegValue("font_color"));
					this.comboBox_fontSize.Text = myReg.getRegValue("font_size");
				}
				catch (Exception) {
					level = "1";
					this.textBox1.Text = "4";
					this.textBox2.Text = "15";
					this.text_nowLine.Text = "0";
					this.checkBox1.Checked = false;

					this.hidden_ctrl.Checked = true;
					this.hidden_alt.Checked = true;
					this.hidden_shift.Checked = false;
					this.hidden_key.Text = "C";

					this.stop_ctrl.Checked = true;
					this.stop_alt.Checked = true;
					this.stop_shift.Checked = false;
					this.stop_key.Text = "S";

					this.pre_ctrl.Checked = true;
					this.pre_alt.Checked = true;
					this.pre_shift.Checked = false;
					this.pre_key.Text = "←";

					this.next_ctrl.Checked = true;
					this.next_alt.Checked = true;
					this.next_shift.Checked = false;
					this.next_key.Text = "→";

					this.button_fontColor.ForeColor = Color.Black;
					this.comboBox_fontSize.Text = "9";
				}
			}
			myReg.CloseReg();

			switch (level)
			{
				case "2":
					radioButton2.Checked = true; break;
				case "3":
					radioButton3.Checked = true; break;
				case "4":
					radioButton4.Checked = true; break;
				default:
					radioButton1.Checked = true; break;
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
				//隐藏
				hotkey.Regist(this.Handle
					, this.hidden_ctrl.Checked
					, this.hidden_alt.Checked
					, this.hidden_shift.Checked
					, this.hidden_key.Text
					, hiddenKey_Click);
				//停止
				hotkey.Regist(this.Handle
					, this.stop_ctrl.Checked
					, this.stop_alt.Checked
					, this.stop_shift.Checked
					, this.stop_key.Text
					, stopKey_Click);
				//快速前页
				hotkey.Regist(this.Handle
					, this.pre_ctrl.Checked
					, this.pre_alt.Checked
					, this.pre_shift.Checked
					, this.pre_key.Text
					, preKey_Click);
				//快速后页
				hotkey.Regist(this.Handle
					, this.next_ctrl.Checked
					, this.next_alt.Checked
					, this.next_shift.Checked
					, this.next_key.Text
					, nextKey_Click);

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

				this.text_nowLine.Text = messageWin.line_pre + "";

				//热键解消
				hotkey.UnRegist(this.Handle, hiddenKey_Click);
				hotkey.UnRegist(this.Handle, stopKey_Click);
				hotkey.UnRegist(this.Handle, preKey_Click);
				hotkey.UnRegist(this.Handle, nextKey_Click);

				messageWin.Visible = false;
				isHidden = true;
			}
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
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
        private void hiddenKey_Click()
        {
//            MessageBox.Show("快捷键被调用！C");
           if (isHidden)
            {
				isHidden = false;
				if (messageWin == null || messageWin.IsDisposed)
				{
					messageWin = new Form2();
					messageWin.Owner = this;
					messageWin.Show();
				} else if (!messageWin.Visible) {
					messageWin.Visible = true;
				}
            } else
            {
				isHidden = true;
                if (messageWin != null && messageWin.Visible)
                {
					//messageWin.Close();
					messageWin.Visible = false;
				}
            }
        }
		private void stopKey_Click()
		{
			if (isStoped)
			{
				isStoped = false;
			}
			else
			{
				messageWin.label1.Text = messageWin.label1.Text + "<停止中>";
				isStoped = true;
			}
		}
		private void preKey_Click()
		{
			isPrev = true;
			if (messageWin.wordNumCount_pre > 0)
			{
				messageWin.wordNumCount_pre--;
			}
			else
			{
				messageWin.wordNumCount_pre = 0;
				if (messageWin.line_pre > 0)
				{
					messageWin.line_pre--;
				}
			}
		}
		private void nextKey_Click()
		{
			isPrev = true;
			messageWin.wordNumCount_pre++;
		}
		protected override void WndProc(ref Message m)
        {
			//窗口消息处理函数
			hotkey.ProcessHotKey(m);
            base.WndProc(ref m);
        }

        public void setting_Click(object sender, EventArgs e)
        {
            //还原窗体显示 
            WindowState = FormWindowState.Normal;
            //激活窗体并给予它焦点 
            this.Activate();
            //任务栏区显示图标 
            this.ShowInTaskbar = true;
            //托盘区图标隐藏 
            this.notifyIcon1.Visible = false;
			this.text_nowLine.Text = messageWin.line_pre + "";

			messageWin.Visible = false;
			isHidden = true;
		}

        public void exist_Click(object sender, EventArgs e)
        {
/*            if (messageThread != null)
            {
                messageThread.Abort();
            }
            //messageThread.Abort();
            if (messageWin != null) { 
                messageWin.Close();
            }

			//热键解消
			hotkey.UnRegist(this.Handle, hiddenKey_Click);
			hotkey.UnRegist(this.Handle, stopKey_Click);
			hotkey.UnRegist(this.Handle, preKey_Click);
			hotkey.UnRegist(this.Handle, nextKey_Click);*/
			this.Close();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
			if (this.radioButton1.Checked) {
				level = "1";
			} else if (this.radioButton2.Checked)
			{
				level = "2";
			}
			else if (this.radioButton3.Checked)
			{
				level = "3";
			}
			else if (this.radioButton4.Checked)
			{
				level = "4";
			}
			RegeditMng myReg = new RegeditMng(ToolName);
			String time_now = myReg.getRegValue("time_" + level);
			String wordNum_now = myReg.getRegValue("wordNum_" + level);
			String line_now = myReg.getRegValue("line_" + level);
			this.checkBox1.Checked = myReg.getRegBoolValue("base64_" + level);
			myReg.CloseReg();

			this.textBox1.Text = time_now == "" ? "4" : time_now ;
			this.textBox2.Text = wordNum_now == "" ? "15" : wordNum_now;
			this.text_nowLine.Text = line_now == "" ? "0" : line_now;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// レーベルが選択されない場合
			if (!this.radioButton4.Checked
				&& !this.radioButton3.Checked
				&& !this.radioButton2.Checked
				&& !this.radioButton1.Checked)
			{
				MessageBox.Show("请选择1，2，3，4");
				return;
			}

			// 時間が設定されない場合
			if (!Util.isNumberic(this.textBox1.Text, out delayTime))
			{
				MessageBox.Show("持续时间没有设置");
				return;
			}
			// 文字数を設定
			if (!Util.isNumberic(this.textBox2.Text, out wordNum))
			{
				MessageBox.Show("一次显示字数没有设置");
				return;
			}
			// 热键check
			if (this.hidden_key.Text.Length != 1 || this.stop_key.Text.Length != 1 || this.pre_key.Text.Length != 1)
			{
				MessageBox.Show("快捷键没有设置字母");
				return;
			}

			RegeditMng myReg = new RegeditMng(ToolName);
			myReg.setRegValue("level", level);
			myReg.setRegValue("time_" + level, this.textBox1.Text);
			myReg.setRegValue("wordNum_" + level, this.textBox2.Text);
			myReg.setRegValue("line_" + level, this.text_nowLine.Text);
			myReg.setRegValue("base64_" + level, this.checkBox1.Checked.ToString());

			myReg.setRegValue("hidden_ctrl", this.hidden_ctrl.Checked.ToString());
			myReg.setRegValue("hidden_alt", this.hidden_alt.Checked.ToString());
			myReg.setRegValue("hidden_shift", this.hidden_shift.Checked.ToString());
			myReg.setRegValue("hidden_key", this.hidden_key.Text);
			myReg.setRegValue("stop_ctrl", this.stop_ctrl.Checked.ToString());
			myReg.setRegValue("stop_alt", this.stop_alt.Checked.ToString());
			myReg.setRegValue("stop_shift", this.stop_shift.Checked.ToString());
			myReg.setRegValue("stop_key", this.stop_key.Text);
			myReg.setRegValue("pre_ctrl", this.pre_ctrl.Checked.ToString());
			myReg.setRegValue("pre_alt", this.pre_alt.Checked.ToString());
			myReg.setRegValue("pre_shift", this.pre_shift.Checked.ToString());
			myReg.setRegValue("pre_key", this.pre_key.Text);
			myReg.setRegValue("next_ctrl", this.next_ctrl.Checked.ToString());
			myReg.setRegValue("next_alt", this.next_alt.Checked.ToString());
			myReg.setRegValue("next_shift", this.next_shift.Checked.ToString());
			myReg.setRegValue("next_key", this.next_key.Text);

			myReg.setRegValue("font_color", ColorTranslator.ToHtml(this.button_fontColor.ForeColor));
			myReg.setRegValue("font_size", this.comboBox_fontSize.Text);
			myReg.CloseReg();

			//若设置界面是可见的，隐藏
			if (panel1.Visible)
			{
				panel1.Visible = false;
				this.Height = this.Height - this.panel1.Height;
			}

			// 最小化 到 任务栏
			this.WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
			this.notifyIcon1.Icon = IconResource.MiniIco;
			this.notifyIcon1.Visible = true;

			isHidden = false;

			ReadWriteFile rFile = new ReadWriteFile();
			wordList = rFile.readFile(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @".txt", this.checkBox1.Checked);

			/*if (messageWin == null || messageWin.IsDisposed)
			{
				messageWin = new Form2();
				messageWin.Show();

				messageWin.level = level;

				//thread start
				messageThread = new Thread(popWinThread);
				messageThread.Start();
			} else if (!messageWin.Visible) {
				messageWin.Visible = true;
				isHidden = false;
			}*/

			if (messageWin != null) {
				messageWin.Close();
			}
			if (messageThread != null) {
				messageThread.Abort();
			}

			messageWin = new Form2();
			messageWin.Owner = this;
			messageWin.Show();

			messageWin.level = level;

			//thread start
			messageThread = new Thread(popWinThread);
			messageThread.Start();
        }

        public void popWinThread()
        {
            Random random = new Random();
            String message = "";
//			Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			RegeditMng myReg = new RegeditMng(ToolName);

			ReadWriteFile rFile = new ReadWriteFile();
			String fileSize_new = rFile.getFileSize(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + level + @".txt");
			//前回情報取得
			int line_pre;
			int wordNum_pre;
			int wordNumCount_pre;
			String base64_pre = myReg.getRegValue("base64_" + level);
			String fileSize_pre = myReg.getRegValue("fileSize_" + level);

			Util.isNumberic(myReg.getRegValue("line_" + level), out line_pre);
			Util.isNumberic(myReg.getRegValue("wordNum_" + level), out wordNum_pre);
			Util.isNumberic(myReg.getRegValue("wordNumCount_" + level), out wordNumCount_pre);

			messageWin.label1.ForeColor = ColorTranslator.FromHtml(myReg.getRegValue("font_color"));
			Font newFont = new Font(messageWin.label1.Font.FontFamily, float.Parse(myReg.getRegValue("font_size")));
			messageWin.label1.Font = newFont;

			messageWin.line_pre = line_pre;
			messageWin.wordNum_pre = wordNum_pre;
			messageWin.wordNumCount_pre = wordNumCount_pre;

			if (fileSize_pre != fileSize_new)
			{ //前後サイズ違い
				messageWin.line_pre = 0;
				messageWin.wordNum_pre = wordNum;
				messageWin.wordNumCount_pre = 0;

				myReg.setRegValue("line_" + level, messageWin.line_pre + "");
				myReg.setRegValue("wordNum_" + level, messageWin.wordNum_pre + "");
				myReg.setRegValue("wordNumCount_" + level, messageWin.wordNumCount_pre + "");
				myReg.setRegValue("fileSize_" + level, fileSize_new);
			} else if (messageWin.wordNum_pre != wordNum) {
				messageWin.wordNumCount_pre = 0;
				messageWin.wordNum_pre = wordNum;

				myReg.setRegValue("wordNumCount_" + level, messageWin.wordNumCount_pre + "");
				myReg.setRegValue("wordNum_" + level, messageWin.wordNum_pre + "");
			}

			Boolean breakToTopFlg = false;
            while (true)
            {
				breakToTopFlg = false;
				if (!isHidden && (!isStoped || isPrev)) //没有隐藏，没有停止或正在翻页
                {
					isPrev = false;
					if (messageWin.line_pre >= wordList.Count)
                    {
						//messageWin.Close();
						messageWin.label1.Text = "<结束>";
						break;
                    }

                    if (wordList != null && wordList.Count > 0)
                    {
						if (messageWin.wordNumCount_pre * messageWin.wordNum_pre >= wordList[messageWin.line_pre].Length)
						{
							message = "";
						}
						else
						{
							message = wordList[messageWin.line_pre].Substring(messageWin.wordNumCount_pre * messageWin.wordNum_pre);

							while (message.Length > 0)
							{
								if (isStoped)
								{
									messageWin.label1.Text = message.Substring(0, Math.Min(wordNum, message.Length)) + "<停止中>";
								}
								else
								{
									messageWin.label1.Text = message.Substring(0, Math.Min(wordNum, message.Length));
								}
								message = message.Substring(Math.Min(wordNum, message.Length));

								for (int i = 0; i < delayTime * 2; i++)
								{
									Thread.Sleep(500);
									if (isPrev)
									{
										breakToTopFlg = true;
										break;
									}
								}
								if (breakToTopFlg) break;

								messageWin.wordNumCount_pre = messageWin.wordNumCount_pre + 1;
								myReg.setRegValue("wordNumCount_" + level, messageWin.wordNumCount_pre + "");

								while ((isStoped || isHidden) && !isPrev) //隐藏 或者 停止 且 没在翻页
								{
									Thread.Sleep(500);
								}
							}
							if (breakToTopFlg) continue;
						}



						/*		while (message.Length > wordNum) {
									messageWin.label1.Text = message.Substring(0,wordNum);
									message = message.Substring(wordNum);
									Thread.Sleep(delayTime * 1000);

									messageWin.wordNumCount_pre = messageWin.wordNumCount_pre + 1;
									myReg.setRegValue("wordNumCount_" + level, messageWin.wordNumCount_pre + "");

									while (isStoped || isHidden)
									{
										Thread.Sleep(delayTime * 1000);
									}
								}
								if (message.Length > 0) {
									messageWin.label1.Text = message;
									Thread.Sleep(delayTime * 1000);
								}*/
						messageWin.line_pre = messageWin.line_pre + 1;
						messageWin.wordNumCount_pre = 0;

						myReg.setRegValue("line_" + level, messageWin.line_pre + "");
						myReg.setRegValue("wordNumCount_" + level, messageWin.wordNumCount_pre + "");
					}
                    else
                    {
                        messageWin.label1.Text = "<没有内容>";
                        //messageWin.Close();
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void label4_Click(object sender, EventArgs e)
		{
			if (panel1.Visible)
			{
				panel1.Visible = false;
				this.Height = this.Height - this.panel1.Height;
			}
			else
			{
				panel1.Visible = true;
				this.Height = this.Height + this.panel1.Height;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (messageThread != null)
			{
				messageThread.Abort();
			}
			//messageThread.Abort();
			if (messageWin != null)
			{
				messageWin.Close();
			}

			//热键解消
			hotkey.UnRegist(this.Handle, hiddenKey_Click);
			hotkey.UnRegist(this.Handle, stopKey_Click);
			hotkey.UnRegist(this.Handle, preKey_Click);
			hotkey.UnRegist(this.Handle, nextKey_Click);
		}

		private void button_fontColor_Click(object sender, EventArgs e)
		{
			ColorDialog ColorForm = new ColorDialog();
			if (ColorForm.ShowDialog() == DialogResult.OK)
			{
				Color GetColor = ColorForm.Color;
				//String a = ColorTranslator.ToHtml(GetColor);
				//GetColor就是用户选择的颜色，接下来就可以使用该颜色了
				this.button_fontColor.ForeColor = GetColor;
			}
		}

		private void label12_Click(object sender, EventArgs e)
		{
			Form3 form3 = new Form3();
			form3.Show();
		}
	}
}
