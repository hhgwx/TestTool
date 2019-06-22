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

namespace MouseFinger
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private Boolean beginFlag = false;
        private Boolean clickFlag = false;

        private Thread clickThread;
        private int delayTime ;


        public Form1()
        {
            InitializeComponent();

            KeyBordHook k_hook = new KeyBordHook();
            k_hook.OnKeyDownEvent += new KeyEventHandler(KeyDown);//关联处理函数
            k_hook.Start();
        }

        private void StartMouseFinger(object sender, EventArgs e)
        {
            // 時間が設定されない場合
            try
            {
                delayTime = Convert.ToInt32(this.textBox1.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请设置整数时间");
                return;
            }

            if (beginFlag)
            {   //点击停止
                beginFlag = false;
                clickFlag = false;
                if (clickThread != null && clickThread.IsAlive)
                {
                    clickThread.Abort();
                }
                this.Start.Text = "START";
            } else {
                //点击开始
                beginFlag = true;
                this.Start.Text = "STOP";
                this.WindowState = FormWindowState.Minimized;
            }
            
            //   mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, 500, 500, 0, 0);
            //   mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public new void KeyDown(object sender, KeyEventArgs e)
        {
            if (beginFlag) // 程序启动
            {
                if (e.KeyValue == 0x20) // 空格键按下
                {
                    
                    if (!clickFlag) // 第一次空格键 开始
                    {
                        clickFlag = true;
                        clickThread = new Thread(clickClick);
                        clickThread.Start();
                    }
                    else // 第二次空格键 停止
                    {
//                        beginFlag = false; //程序完全停止
                        clickFlag = false;
                        if (clickThread!=null && clickThread.IsAlive) { 
                            clickThread.Abort();
                        }
                    }
                    
                }
            }
            //Console.WriteLine(e.KeyValue.ToString());
        }

        private void clickClick()
        {
            //Point p1 = MousePosition;
            while (true) 
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(delayTime);
            }
        }
    }
}
