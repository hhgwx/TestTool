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

namespace EngW
{
    public partial class Form1 : Form
    {
        Boolean startFlg = false;
        List<String> wordList = null;
        String record = "0";
        Random random = new System.Random();
        Thread timeThread;
        int checkOkIndex = 0; // check予定Index
        int result = 0; // 分数
        Boolean isEnter = false; // buttonのEnter鍵を反映しないように

        AutoResetEvent autoEvent = new AutoResetEvent(false);

        public Form1(List<String> wordList,String record)
        {
            InitializeComponent();
            this.wordList = wordList;
            this.record = record;
            setNewWord();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!isEnter && !"0".Equals(textBox1.Text)) { 
                timeThread = new Thread(timeThreadStart);

                if (startFlg)
                {
                    startFlg = false;
                } else
                {
                    setNewWord();
                    result = 0;
                    timeThread.Start();
                    startFlg = true;
                }
            } else
            {
                isEnter = false;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            
            if (startFlg)
            {
               if (keyData == Keys.Enter || keyData == Keys.Space)
               {
                    isEnter = true;

                //    Boolean isOK = false;
                    for (int i = 0; i < label2.Length; i++)
                    {
                        if (!"".Equals(label2[i].Text) && label2[i].ForeColor != Color.Red)
                        {
//                            result--;
                            label1.Text = result + "/" + record;
                            return base.ProcessDialogKey(keyData);
                        }
                    }
                    setNewWord();
                    result++;
                    label1.Text = result + "/" + record;
                } else if (keyData.ToString().ToLower().Equals(label2[checkOkIndex].Text.ToLower()))
                {
                    //label2[checkOkIndex].Text = "";
                    label2[checkOkIndex].ForeColor = Color.Red;
                    result++;
                    label1.Text = result + "/" + record;
                    checkOkIndex++;
                } else
                {
//                    result--;
                    label1.Text = result + "/" + record;
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        private void timeThreadStart()
        {
            int time;
            if (isNumberic(textBox1.Text, out time))
            {
                while (time != 0)
                {
                    if (startFlg)
                    {
                        Thread.Sleep(1000);
                        time--;
                        textBox1.Text = time + "";
                    }
                    else
                    {
                        return;
                    }
                }
                startFlg = false;

                // record を登録
                if (result > Convert.ToInt32(record)) {
                    record = result+ " "; //file read programeにスペースがある行が対象ですので、" "を追加

                    ReadWriteFile rFile = new ReadWriteFile();
                    rFile.writeFile(Program.RECORD_PATH,record,false);
                    label1.Text = result + "/" + record + " N";
                }
            }
        }

        protected bool isNumberic(string message, out int time)
        {
            time = -1;   //time 定义为out 用来输出值
            try
            {
                //当数字字符串的为是少于4时，以下三种都可以转换，任选一种
                //如果位数超过4的话，请选用Convert.ToInt32() 和int.Parse()

                //time = int.Parse(message);
                //time = Convert.ToInt16(message);
                time = Convert.ToInt32(message.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void setNewWord()
        {
            String lineStr = wordList[random.Next(1, wordList.Count)];
            String[] word = lineStr.Split(' ');
            checkOkIndex = 0;
            for (int i = 0; i < label2.Length; i++)
            {
                label2[i].Text = "";
                label2[i].ForeColor = Color.Gray;
            }
            for (int i =0;i< word[0].Length;i++)
            {
                label2[i].Text = word[0].Substring(i,1);
            }
            label3.Text = word[1];

            label1.Text = result + "/" + record;
        }
    }
}
