using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win2048
{
    public partial class Form1 : Form
    {
        RunPro runPro;

        public Form1()
        {
            InitializeComponent();

            runPro = new RunPro();
            runPro.newDataAppear(Program.num44);
            this.drawForm(Program.num44);
        }


        public void drawForm(int[,] num44)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    this.textBox[x, y].Text = num44[x, y] + "";
                }
            }
        }

        private void reStart_Click(object sender, EventArgs e)
        {
            runPro.reSetNum(Program.num44);
            runPro.newDataAppear(Program.num44);
            this.drawForm(Program.num44);
        }
    }
}
