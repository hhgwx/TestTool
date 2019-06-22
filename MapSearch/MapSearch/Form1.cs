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

namespace MapSearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            bit = new Bitmap(length, length);
            Graphics g = Graphics.FromImage(bit);
            g.Clear(this.BackColor);
            g.DrawRectangle(Pens.SlateGray, new Rectangle(0, 0, length, length));
            g.Dispose();
            textureBrush = new TextureBrush(bit);//使用TextureBrush可以有效减少窗体拉伸时的闪烁

            mapSearch2 = new mapSearch.MapSearch2(areaY / length, areaX / length);

            mapSearch2.setStartPoint(10,10);
            mapSearch2.setEndPoints(30, 30);
            mapSearch2.setEndPoints(35, 1);
            mapSearch2.setEndPoints(5, 16);

            for (int i = 0; i < 30; i++)
            {
                mapSearch2.setWallPoint(5 + i, 15);
            }
            
        }

        TextureBrush textureBrush;
        Bitmap bit;
        int x = 0;
        int y = 0;
        //        bool showRec = false;

        int length = 5;
        int areaX = 300;
        int areaY = 300;

        int runFlg = 0; //0:  1:start  2:stop  3:end
        Thread t1 = null; //startPoint move　Thread
        Thread t2 = null; //endPoint move　Thread

        mapSearch.MapSearch2 mapSearch2;

        List<mapSearch.PointXY> pathList = null;

        Image imageStart = Image.FromFile(@"start.png");
        Image imageEnd = Image.FromFile(@"end.png");


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X / length;
                y = e.Y / length;

                if (e.X < areaX && e.Y < areaY)
                {
                    if (this.startButton.Checked)
                    {
                        if (mapSearch2.isStartPoint(y, x))
                        {
                            mapSearch2.clearStartPoint(y, x);
                        }
                        else
                        {
                            mapSearch2.setStartPoint(y, x);
                        }
                    }
                    else if (this.endButton.Checked)
                    {
                        if (mapSearch2.isEndPoint(y, x))
                        {
                            mapSearch2.clearPoint(y, x);
                        }
                        else
                        {
                            mapSearch2.setEndPoints(y, x);
                        }
                    }
                    else if (this.wallButton.Checked)
                    {
                        if (mapSearch2.isWallPoint(y, x))
                        {
                            mapSearch2.clearPoint(y, x);
                        }
                        else
                        {
                            mapSearch2.setWallPoint(y, x);
                        }
                    }

                    if (pathList != null && pathList.Count > 0)
                    {
                        pathList.Clear();
                    }
                    this.Invalidate();
                }
                //                MessageBox.Show("第" + x + "×" + y + "个矩形 " + "坐标为:(" + e.X + "," + e.Y + ")");

            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(textureBrush, new Rectangle(0, 0, 301, 301)); //this.ClientRectangle

            /*            String temp = "";
                        if (pathList != null && pathList.Count > 0)
                        {
                            foreach (mapSearch.PointXY tempPoint in pathList)
                            {
                                temp += "[" + tempPoint.pointY + "," + tempPoint.pointX + "]";
                            }
                        }
                        */
            for (int y = 0; y < areaY / length; y++)
            {
                for (int x = 0; x < areaX / length; x++)
                {
                    if (mapSearch2.isStartPoint(y, x))
                    {
                        //e.Graphics.FillRectangle(new SolidBrush(Color.White), x * length, y * length, length, length);
                        //e.Graphics.DrawString("S", new Font(FontFamily.GenericSansSerif, 5), Brushes.Blue, x * length, y * length);
                        e.Graphics.DrawImage(imageStart, new Point(x * length, y * length));
                    }
                    else if (mapSearch2.isEndPoint(y, x))
                    {
                        //e.Graphics.FillRectangle(new SolidBrush(Color.White), x * length, y * length, length, length);
                        //e.Graphics.DrawString("E", new Font(FontFamily.GenericSansSerif, 5), Brushes.Crimson, x * length, y * length);
                        e.Graphics.DrawImage(imageEnd, new Point(x * length, y * length));
                    }
                    else
                    {
                        if (mapSearch2.isWallPoint(y, x))
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Black), x * length, y * length, length, length);
                        }

/*                        if (!"".Equals(temp) && temp.IndexOf("[" + y + "," + x + "]") > 0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.White), x * length, y * length, length, length);
                        }
                        */
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
/*                        pathList = mapSearch2.searchStart();
                        foreach (mapSearch.PointXY tempPoint in pathList)
                        {
                            mapSearch2.setStartPoint(tempPoint.pointY, tempPoint.pointX);
                            this.Invalidate();
                            System.Threading.Thread.Sleep(500);
                        }*/
                        
            runFlg = 1;
//            startRunThread();
            t1 = new Thread(startThread);
            t1.Start();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X / length;
                y = e.Y / length;

                if (e.X < areaX && e.Y < areaY)
                {
                    if (this.wallButton.Checked)
                    {
                        mapSearch2.setWallPoint(y, x);
                    }

                    if (pathList != null && pathList.Count > 0)
                    {
                        pathList.Clear();
                    }
                }
                this.Invalidate();
            }
        }

        private void startThread()
        {
            while (runFlg == 1)
            {
                pathList = mapSearch2.searchStart();
                if (pathList != null && pathList.Count > 1)
                {
                    System.Console.Write(pathList[1].pointY + "," + pathList[1].pointX);
                    if (mapSearch2.isEndPoint(pathList[1].pointY, pathList[1].pointX))
                    {
                        runFlg = 0;
                        this.Invalidate();
                        t1.Abort();
                    }
                    mapSearch2.setStartPoint(pathList[1].pointY, pathList[1].pointX);
                    this.Invalidate();
                    Thread.Sleep(1000);

                }
                else
                {
                    break;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t1 != null)
            {
                t1.Abort();
            }
        }
    }


}
