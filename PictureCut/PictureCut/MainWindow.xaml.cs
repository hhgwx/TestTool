using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PictureCut
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<PicCutBean> picList = new ObservableCollection<PicCutBean>(); //文件列表
        string fileFromPath = "";
        string fileType;
        string fileCutType;
        int cutNum=0;
        string fileOutPath;
        Line[] lines = new Line[8];
        Line lineNow;

        double scaleImg = 1; //图片被缩小的比例


        public MainWindow()
        {
            InitializeComponent();

            Init();

        }

        private void Init()
        {
            lines[0] = this.Line0;
            lines[1] = this.Line1;
            lines[2] = this.Line2;
            lines[3] = this.Line3;
            lines[4] = this.Line4;
            lines[5] = this.Line5;
            lines[6] = this.Line6;
            lines[7] = this.Line7;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            picList.Clear();

            fileFromPath = this.textBox_FromPath.Text;
            if (!fileFromPath.EndsWith(@"\"))
            {
                fileFromPath = fileFromPath + @"\";
            }

            fileType = this.comboBox_Type.Text;
            fileOutPath = this.textBox_out.Text;
            cutNum = int.Parse(this.textBox_CutNum.Text);
            fileCutType = this.comboBox_RowCol.Text;

            //check TODO

            //获取文件
            string[] fileList = Directory.GetFiles(fileFromPath, "*." + fileType, SearchOption.TopDirectoryOnly);

            PicCutBean picCutBeanTemp;
            foreach (string fileTemp in fileList)
            {
                picCutBeanTemp = new PicCutBean();
                picCutBeanTemp.Name = fileTemp.Substring(fileTemp.LastIndexOf(@"\") + 1);
                picCutBeanTemp.CutNum = cutNum;

                if (fileCutType == "行")
                {
                    picCutBeanTemp.CutRowFlg = true;
                } else
                {
                    picCutBeanTemp.CutRowFlg = false;
                }
                
                picList.Add(picCutBeanTemp);
            }


            listView_PicList.ItemsSource = picList;
        }
        

        private void listView_PicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView_PicList.SelectedIndex != -1)
            {
                PicCutBean picCutBeanTemp = picList[listView_PicList.SelectedIndex];
                string picPath = fileFromPath + picCutBeanTemp.Name;

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = new MemoryStream(File.ReadAllBytes(picPath));
                img.EndInit();
                this.image_show.Source = img;

                if (picCutBeanTemp.listCutPoint.Count == 0)
                {
                    picCutBeanTemp.ImgWidth = img.PixelWidth;
                    picCutBeanTemp.ImgHeight = img.PixelHeight;

                    //具体分割点初始化
                    setlistCutPoing(picCutBeanTemp);
                }

                //DrawLine
                showLine(picCutBeanTemp);

                //MessageBox.Show(img.Width + "  " + img.Height  + "---" + img.PixelWidth+ " " + img.PixelHeight);
            }
        }

        private void setlistCutPoing(PicCutBean picCutBeanTemp)
        {
            //具体分割点初始化
            Decimal pointAvg = 0;
            if (picCutBeanTemp.listCutPoint.Count == 0)
            {
                if (picCutBeanTemp.CutRowFlg) //按行切割
                {
                    pointAvg = Math.Ceiling((Decimal)picCutBeanTemp.ImgHeight / picCutBeanTemp.CutNum);
                }
                else
                {
                    pointAvg = Math.Ceiling((Decimal)picCutBeanTemp.ImgWidth / picCutBeanTemp.CutNum);
                }
                for (int i = 1; i <= picCutBeanTemp.CutNum - 1; i++)
                {
                    picCutBeanTemp.listCutPoint.Add(int.Parse(pointAvg.ToString()) * i);
                }
            }
        }

        private void showLine(PicCutBean picCutBeanTemp)
        {
            //Line再初期化
            foreach (Line lineTemp in lines)
            {
                lineTemp.X1 = 0;
                lineTemp.X2 = 0;
                lineTemp.Y1 = 0;
                lineTemp.Y2 = 0;
            }

            if (picCutBeanTemp.ImgWidth > this.image_show.Width)
            {
                scaleImg = this.image_show.Width / picCutBeanTemp.ImgWidth;
            } else if (picCutBeanTemp.ImgHeight > this.image_show.Height)
            {
                scaleImg = this.image_show.Height / picCutBeanTemp.ImgHeight;
            }

            int addP = 0;
            for (int i = 0; i < picCutBeanTemp.listCutPoint.Count; i++)
            {
                addP = picCutBeanTemp.listCutPoint[i];

                if (picCutBeanTemp.CutRowFlg) //按行切割
                {
                    lines[i].X1 = 0;
                    lines[i].X2 = picCutBeanTemp.ImgWidth * scaleImg;
                    lines[i].Y1 = addP * scaleImg;
                    lines[i].Y2 = addP * scaleImg;
                }
                else
                {
                    lines[i].X1 = addP * scaleImg;
                    lines[i].X2 = addP * scaleImg;
                    lines[i].Y1 = 0;
                    lines[i].Y2 = picCutBeanTemp.ImgHeight * scaleImg;
                }
            }
        }

        bool moveFlg = false;
        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveFlg = true;

            foreach (Line lineTemp in lines)
            {
                if (lineTemp == (Line)sender)
                {
                    lineNow = lineTemp;
                }
            }
        }

        private void Line_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //System.Console.WriteLine("Line_MouseLeftButtonUp");
            if (moveFlg)
            {
                newlinePositionSave();
                moveFlg = false;
            }
        }


        private void newlinePositionSave()
        {
            PicCutBean picCutBeanTemp = new PicCutBean();
            if (listView_PicList.SelectedIndex != -1)
            {
                picCutBeanTemp = picList[listView_PicList.SelectedIndex];
            }

            int j = 0;
            int positionTemp = 0;
            for (int i =0;i< lines.Length;i++)
            {
                if (picCutBeanTemp.CutRowFlg)
                {
                    if (lines[i].Y1 != 0 && lines[i].Y2 != 0)
                    {
                        positionTemp = (int)(lines[i].Y1 / scaleImg);
                        if (positionTemp >0 && positionTemp < picCutBeanTemp.ImgHeight)
                        {
                            picCutBeanTemp.listCutPoint[j] = positionTemp;
                            j++;
                        }
                    }
                } else
                {
                    if (lines[i].X1 != 0 && lines[i].X2 != 0)
                    {
                        positionTemp = (int)(lines[i].X1 / scaleImg);
                        if (positionTemp > 0 && positionTemp < picCutBeanTemp.ImgWidth)
                        {
                            picCutBeanTemp.listCutPoint[j] = positionTemp;
                            j++;
                        }
                    }
                }
            }
        }

        private void image_show_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && moveFlg)
            {
                System.Windows.Point mPoint = e.GetPosition(this.GridMouse);
                // ((Line)sender).
                if (fileCutType == "行")
                {
                    lineNow.Y1 = mPoint.Y;
                    lineNow.Y2 = mPoint.Y;
                }
                else
                {
                    lineNow.X1 = mPoint.X;
                    lineNow.X2 = mPoint.X;
                }
            }
        }

        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Console.WriteLine("Grid_PreviewMouseLeftButtonUp");
        }

        private void button_CutStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (PicCutBean picCutBeanTemp in picList)
            {
                CutImage(picCutBeanTemp,
                    this.textBox_out.Text,
                    comboBox_Type.Text,
                    this.textBox_posX.Text,
                    this.textBox_posY.Text,
                    this.textBox_mark.Text,
                    new Font("黑体", 20));
            }
        }

        /// <summary>
        /// 图像切割
        /// </summary>
        /// <param name="url">图像文件名称</param>
        /// <param name="width">切割后图像宽度</param>
        /// <param name="height">切割后图像高度</param>
        /// <param name="savePath">切割后图像文件保存路径</param>
        /// <param name="fileExt">切割后图像文件扩展名</param>
        private void CutImage(PicCutBean picCutBeanTemp, string savePath, string fileExt, string markX,string markY,string markMsg, Font markFont)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            Bitmap bitmap = new Bitmap(fileFromPath + picCutBeanTemp.Name);
            picCutBeanTemp.ImgWidth = bitmap.Width;
            picCutBeanTemp.ImgHeight = bitmap.Height;

            if (picCutBeanTemp.listCutPoint.Count == 0)
            {
                setlistCutPoing(picCutBeanTemp);
            }

            Bitmap bmpCut = null;
            int widthCut = 0;
            int heightCut = 0;
            for (int i = 0;i<= picCutBeanTemp.listCutPoint.Count;i++)
            {
                string filename = picCutBeanTemp.Name + "_" + i.ToString() + "." + fileExt;
                if (picCutBeanTemp.CutRowFlg)
                {
                    widthCut = picCutBeanTemp.ImgWidth;
                    if (i==0 && picCutBeanTemp.listCutPoint.Count >0)
                    {
                        heightCut = picCutBeanTemp.listCutPoint[i];
                    } else if (i < picCutBeanTemp.listCutPoint.Count)
                    {
                        heightCut = picCutBeanTemp.listCutPoint[i] - picCutBeanTemp.listCutPoint[i - 1];
                    }
                    else if (i == picCutBeanTemp.listCutPoint.Count && picCutBeanTemp.listCutPoint.Count>0)
                    {
                        heightCut = picCutBeanTemp.ImgHeight - picCutBeanTemp.listCutPoint[i-1];
                    } else
                    {
                        heightCut = picCutBeanTemp.ImgHeight;
                    }
                    bmpCut = new Bitmap(widthCut, heightCut);


                    int startPoint = 0;
                    if (i > 0)
                    {
                        startPoint = picCutBeanTemp.listCutPoint[i - 1];
                    }
                    for (int offsetX = 0; offsetX < widthCut; offsetX++)
                    {
                        for (int offsetY = 0; offsetY < heightCut; offsetY++)
                        {
                            bmpCut.SetPixel(offsetX, offsetY, bitmap.GetPixel(offsetX, startPoint + offsetY));
                        }
                    }
                } else
                {
                    heightCut = picCutBeanTemp.ImgHeight;
                    if (i == 0 && picCutBeanTemp.listCutPoint.Count > 0)
                    {
                        widthCut = picCutBeanTemp.listCutPoint[i];
                    }
                    else if (i < picCutBeanTemp.listCutPoint.Count)
                    {
                        widthCut = picCutBeanTemp.listCutPoint[i] - picCutBeanTemp.listCutPoint[i - 1];
                    }
                    else if (i == picCutBeanTemp.listCutPoint.Count && picCutBeanTemp.listCutPoint.Count > 0)
                    {
                        widthCut = picCutBeanTemp.ImgWidth - picCutBeanTemp.listCutPoint[i-1];
                    } else
                    {
                        widthCut = picCutBeanTemp.ImgWidth;
                    }
                    bmpCut = new Bitmap(widthCut, heightCut);


                    int startPoint = 0;
                    if (i > 0)
                    {
                        startPoint = picCutBeanTemp.listCutPoint[i - 1];
                    }
                    for (int offsetX = 0; offsetX < widthCut; offsetX++)
                    {
                        for (int offsetY = 0; offsetY < heightCut; offsetY++)
                        {
                            bmpCut.SetPixel(offsetX, offsetY, bitmap.GetPixel(startPoint + offsetX, offsetY));
                        }
                    }
                }

                if (markMsg != "")
                {
                    Graphics g = Graphics.FromImage(bmpCut);

                    int px = 0;
                    int py = 0;
                    try
                    {
                        px = int.Parse(markX);
                        py = int.Parse(markY);
                    }
                    catch
                    {
                    }

                    g.DrawString(markMsg, markFont, new SolidBrush(System.Drawing.Color.FromArgb(70, System.Drawing.Color.WhiteSmoke)), px, py);//加水印
                }

                ImageFormat format = ImageFormat.Png;
                switch (fileExt.ToLower())
                {
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    case "webp":
                        format = ImageFormat.Gif;
                        break;
                }
                bmpCut.Save(savePath + "//" + filename, format);
            }

        }

        private void button_addLine_Click(object sender, RoutedEventArgs e)
        {
            if (listView_PicList.SelectedIndex != -1)
            {
                PicCutBean picCutBeanTemp = picList[listView_PicList.SelectedIndex];
                if (picCutBeanTemp.CutNum < 9)
                {
                    picCutBeanTemp.CutNum++;
                    picCutBeanTemp.listCutPoint.Clear();
                    setlistCutPoing(picCutBeanTemp);

                    showLine(picCutBeanTemp);
                }
            }
        }

        private void button_delLine_Click(object sender, RoutedEventArgs e)
        {
            if (listView_PicList.SelectedIndex != -1)
            {
                PicCutBean picCutBeanTemp = picList[listView_PicList.SelectedIndex];
                if (picCutBeanTemp.CutNum > 0)
                {
                    picCutBeanTemp.CutNum--;
                    picCutBeanTemp.listCutPoint.Clear();
                    setlistCutPoing(picCutBeanTemp);

                    showLine(picCutBeanTemp);
                }
            }
        }

        /*       /// <summary>
               /// 图像切割
               /// </summary>
               /// <param name="url">图像文件名称</param>
               /// <param name="width">切割后图像宽度</param>
               /// <param name="height">切割后图像高度</param>
               /// <param name="savePath">切割后图像文件保存路径</param>
               /// <param name="fileExt">切割后图像文件扩展名</param>
               public static void Cut(string url, int width, int height, string savePath, string fileExt, string logofile)
               {
                   Bitmap bitmap = new Bitmap(url);
                   Decimal MaxRow = Math.Ceiling((Decimal)bitmap.Height / height);
                   Decimal MaxColumn = Math.Ceiling((decimal)bitmap.Width / width);
                   for (decimal i = 0; i < MaxRow; i++)
                   {
                       for (decimal j = 0; j < MaxColumn; j++)
                       {
                           string filename = i.ToString() + "," + j.ToString() + "." + fileExt;
                           Bitmap bmp = new Bitmap(width, height);
                           for (int offsetX = 0; offsetX < width; offsetX++)
                           {
                               for (int offsetY = 0; offsetY < height; offsetY++)
                               {
                                   if (((j * width + offsetX) < bitmap.Width) && ((i * height + offsetY) < bitmap.Height))
                                   {
                                       bmp.SetPixel(offsetX, offsetY, bitmap.GetPixel((int)(j * width + offsetX), (int)(i * height + offsetY)));
                                   }
                               }
                           }
                           Graphics g = Graphics.FromImage(bmp);
                           g.DrawString("脚本之家", new Font("黑体", 20), new SolidBrush(System.Drawing.Color.FromArgb(70, System.Drawing.Color.WhiteSmoke)), 60, height / 2);//加水印
                           ImageFormat format = ImageFormat.Png;
                           switch (fileExt.ToLower())
                           {
                               case "png":
                                   format = ImageFormat.Png;
                                   break;
                               case "bmp":
                                   format = ImageFormat.Bmp;
                                   break;
                               case "gif":
                                   format = ImageFormat.Gif;
                                   break;
                               case "webp":
                                   format = ImageFormat.Gif;
                                   break;
                           }
                           bmp.Save(savePath + "//" + filename, format);
                       }
                   }
               }*/
    }
}
