using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PictureMerge
{
    public partial class Form1 : Form
    {
        String defaultfilePath = "";
        Dictionary<string, string> myDictionary;

        public Form1()
        {
            InitializeComponent();

        }



        private void FormClosingButton(object sender, FormClosingEventArgs e)
        {
/*            if (backgroundWorker1.IsBusy)
            {
                //取消掉Completed事件,不进行接收
//                backgroundWorker1.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorker1_Completed2);
                backgroundWorker1.CancelAsync();
            }*/
        }

    }
}
