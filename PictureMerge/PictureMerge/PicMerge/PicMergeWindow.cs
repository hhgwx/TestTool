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
using System.Threading;
using System.Windows.Forms;

namespace PictureMerge.PicMerge
{
    public partial class PicMergeWindow : UserControl
    {

        String defaultfilePath = "";
        Dictionary<string, string> myDictionary;

        int messageFlg = 0; //1:文件夹合并中 2:画像分析中 3:画像比较中
        double samePercent = 0.85;
        int threadNum = Environment.ProcessorCount;

        //        String textFlg = "";

        public PicMergeWindow()
        {
            InitializeComponent();

            defaultfilePath = ConfigurationManager.AppSettings["folderPath"]; //@"D:\mergeImg\1";
            //this.listBox1.Items.Add(@"D:\gwx\1"); //TODO
        }


        //添加文件夹 按钮按下
        private void button1_Click(object sender, EventArgs e)
        {
            if (defaultfilePath != "")
            {
                folderBrowserDialog1.SelectedPath = defaultfilePath;
            }

            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultfilePath = folderBrowserDialog1.SelectedPath;

                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings["folderPath"].Value = defaultfilePath;
                cfa.Save();

                if (!this.listBox1.Items.Contains(defaultfilePath))
                {
                    this.listBox1.Items.Add(defaultfilePath);
                }
            }
        }

        //删除文件夹 按钮按下
        private void button2_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < listBox1.SelectedItems.Count;)
                listBox1.Items.Remove(listBox1.SelectedItem.ToString());
        }

        //开始 按钮按下
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "开始")
            {
                //选择文件夹检查
                if (listBox1.Items.Count == 0)
                {
                    MessageBox.Show("请选择文件夹");
                    return;
                }

                for (int a = 0; a < listBox1.Items.Count; a++)
                {
                    if (!Directory.Exists(listBox1.Items[a].ToString()))
                    {
                        MessageBox.Show(listBox1.Items[a].ToString() + "不存在");
                        return;
                    }
                }

                //结果一览清空
                this.listView1.Items.Clear();

                //按钮非活性
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Text = "结束";
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;
                this.button7.Enabled = false;
                this.button8.Enabled = false;

                samePercent = Convert.ToInt32(this.comboBox1.SelectedItem.ToString()) / 100.0;

                // 发时间处理放后台
                BackgroundWorkBean backgroundWorkBean = new BackgroundWorkBean();
                backgroundWorkBean.checkBox1Flag = this.checkBox1.Checked;
                backgroundWorkBean.checkBox2Flag = this.checkBox2.Checked;
                backgroundWorkBean.pathArr = new string[listBox1.Items.Count];
                for (int a = 0; a < listBox1.Items.Count; a++)
                {
                    backgroundWorkBean.pathArr[a] = listBox1.Items[a].ToString();
                }
                backgroundWorker1.RunWorkerAsync(backgroundWorkBean);
            }
            else
            {
                if (backgroundWorker1.IsBusy)
                {
                    //按钮活性
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.button3.Text = "开始";

                    backgroundWorker1.CancelAsync();

                }
            }



        }

        //相似对象一览中 电击
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = pathLeft; //地址栏中显示路径
            label2.Text = pathRight; //地址栏中显示路径
            listView1_SelectedImageShow();
        }

        //显示所选行的画像
        private String pathLeft = "";   //左源画像路径
        private String pathRight = "";  //右画像路径
        private int listView_selectIndex = 0;
        private void listView1_SelectedImageShow()
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                listView_selectIndex = listView1.SelectedItems[0].Index;

                //相似画像路径组
                String[] path2;

                ListView.SelectedIndexCollection c = listView1.SelectedIndices;
                pathLeft = listView1.Items[c[0]].Text;
                String path2String = listView1.Items[c[0]].SubItems[1].Text;
                path2 = path2String.Split(';');

                //                MessageBox.Show(path2[0]);

                if (pathLeft == null || pathLeft == "" || !File.Exists(pathLeft))
                {
                    delete_listView1_row();
                    return;
                }

                if (path2.Length > 0)
                {
                    if (File.Exists(path2[0]))
                    {
                        pathRight = path2[0];

                        Stream s = File.Open(pathLeft, FileMode.Open);
                        this.pictureBox1.Image = Image.FromStream(s);
                        s.Close();

                        s = File.Open(pathRight, FileMode.Open);
                        this.pictureBox2.Image = Image.FromStream(s);
                        s.Close();

                        label1.Text = pathLeft; //地址栏中显示路径
                        label2.Text = pathRight; //地址栏中显示路径
                    }
                    else
                    {
                        //先做了后面的行删除，导致这个图片已经被删了，直接把文字列替换掉
                        listView1.Items[c[0]].SubItems[1].Text = path2String.Replace(path2[0] + ";", "").Replace(path2[0], ""); ;
                        if (listView1.Items[c[0]].SubItems[1].Text == "")
                        {
                            delete_listView1_row();
                        }

                        listView1_SelectedImageShow();
                    }
                }
            }
        }

        //左删除按钮按下
        private void button4_Click(object sender, EventArgs e)
        {
            if (pathLeft != "")
            {
                File.Delete(pathLeft); //删除图片
                pathLeft = "";

                delete_listView1_row();
            }
        }

        //删除所选行
        private void delete_listView1_row()
        {
            listView1.Items[listView_selectIndex].Remove(); //删除所选行
            try
            {

                listView1.Items[listView_selectIndex].Selected = true;
                this.listView1.Select();
                listView1.EnsureVisible(listView_selectIndex);//保证可见
            }
            catch (Exception)
            {
                this.pictureBox1.Image = null;
                this.pictureBox2.Image = null;
                try
                {
                    listView1.Items[listView_selectIndex - 1].Selected = true;
                    this.listView1.Select();
                    //listView1.EnsureVisible(listView_selectIndex - 1);//保证可见
                    listView1.EnsureVisible(listView_selectIndex);//保证可见  Select时会触发变更事件，导致listView_selectIndex值变掉，不用再减1
                }
                catch (Exception)
                {
                    this.pictureBox1.Image = null;
                    this.pictureBox2.Image = null;
                }
            }
        }

        //右删除 按钮按下
        private void button5_Click(object sender, EventArgs e)
        {
            if (pathRight != "")
            {
                File.Delete(pathRight); //删除图片
                pathRight = "";

                listView1_SelectedImageShow();
            }
        }

        //全删除 按钮按下
        private void button6_Click(object sender, EventArgs e)
        {
            if (pathRight != "")
            {
                File.Delete(pathRight); //删除图片
                pathRight = "";
            }

            if (pathLeft != "")
            {
                File.Delete(pathLeft); //删除图片
                pathLeft = "";

                delete_listView1_row();
            }
        }

        // 跳过 按钮按下
        private void button7_Click(object sender, EventArgs e)
        {
            delete_listView1_row();
        }

        //长时间处理放到后台作，不让页面假死掉
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int imageCount = 0; //进度用

            BackgroundWorkBean bean = (BackgroundWorkBean)e.Argument;

            try
            {
                if (bean.checkBox1Flag)
                {
                    messageFlg = 1;
                    //全部文件合并到第一个文件夹
                    String mainFloder = bean.pathArr[0];
                    String renameFile;


                    for (int a = 1; a < bean.pathArr.Length; a++)
                    {
                        try
                        {
                            string[] fileList = Directory.GetFiles(bean.pathArr[a], "*.jpg", SearchOption.TopDirectoryOnly);

                            for (int i = 0; i < fileList.Length; i++)
                            {
                                imageCount++;
                                int index = 1;
                                renameFile = Path.GetFileName(fileList[i]);
                                while (File.Exists(mainFloder + @"\" + renameFile)) //重复时重命名
                                {
                                    renameFile = Path.GetFileNameWithoutExtension(fileList[i]) + "_" + index + Path.GetExtension(fileList[i]);
                                }

                                try
                                {
                                    File.Move(fileList[i], mainFloder + @"\" + renameFile);
                                }
                                catch (Exception)
                                {
                                    if (File.GetAttributes(fileList[i]).ToString().IndexOf("ReadOnly") != -1)
                                    {
                                        System.IO.File.SetAttributes(fileList[i], System.IO.FileAttributes.Normal); //去除只读
                                        File.Move(fileList[i], mainFloder + @"\" + renameFile);
                                    }
                                }

                                if (i % 10 == 0)
                                {
                                    backgroundWorker1.ReportProgress(imageCount);
                                }

                                if (backgroundWorker1.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }

                //画像KEY取得
                Merge2 merge = new Merge2();
                ArrayList imageKeyList = new ArrayList();

                messageFlg = 2;
                imageCount = 0;
                int aCount = bean.pathArr.Length;
                if (bean.checkBox1Flag)
                {
                    aCount = 1;
                }
                for (int a = 0; a < aCount; a++)
                {
                    string[] baseFileList = Directory.GetFiles(bean.pathArr[a], "*.jpg", SearchOption.TopDirectoryOnly);
                    /*                    for (int i = 0; i < baseFileList.Length; i++)
                                        {
                                            imageCount++;
                                            imageKeyList.Add(merge.MergePic(baseFileList[i]));

                                            if (i % 10 == 0)
                                            {
                                                backgroundWorker1.ReportProgress(imageCount);
                                            }
                                            if (e.Cancel || backgroundWorker1.CancellationPending)
                                            {
                                                return;
                                            }
                                        }*/

                    Thread[] th = new Thread[threadNum];
                    ThreadClass[] threadClass = new ThreadClass[threadNum];
                    Boolean threadOKFlg = true;

                    //处理文件线程分割用
                    int imageListCut = (int)Math.Ceiling(baseFileList.Length * 1.0 / threadNum);
                    int startCut = 0;
                    int endCut = 0;

                    for (int i = 0; i < threadNum; i++)
                    {
                        endCut = startCut + imageListCut;

                        threadClass[i] = new ThreadClass();
                        threadClass[i].startIndex = startCut;
                        threadClass[i].endIndex = endCut;
                        threadClass[i].imageList = baseFileList;

                        th[i] = new Thread(new ThreadStart(threadClass[i].imageThreadStart));
                        th[i].Start();

                        startCut = endCut;
                    }

                    while (true)
                    {
                        Thread.Sleep(1000);
                        imageCount = 0;
                        for (int i = 0; i < threadNum; i++)
                        {
                            imageCount = imageCount + threadClass[i].count;
                        }
                        backgroundWorker1.ReportProgress(imageCount);

                        //启动进程完了否
                        threadOKFlg = true;
                        for (int i = 0; i < threadNum; i++)
                        {
                            threadOKFlg = threadOKFlg && threadClass[i].endFlag;
                        }

                        //启动进程完了时处理
                        if (threadOKFlg)
                        {
                            for (int i = 0; i < threadNum; i++)
                            {
                                imageKeyList.AddRange(threadClass[i].imageKeyList);
                            }
                            break;
                        }

                        //关闭
                        if (backgroundWorker1.CancellationPending)
                        {
                            for (int i = 0; i < threadNum; i++)
                            {
                                th[i].Abort();
                            }
                            e.Cancel = true;
                            return;
                        }
                    }

                }


                string key = "";
                myDictionary = new Dictionary<string, string>();
                //比较
                messageFlg = 3;
                for (int i = 0; i < imageKeyList.Count - 1; i++)
                {
                    for (int j = i + 1; j < imageKeyList.Count; j++)
                    {
                        //                        textFlg = ((ImageID_KIND2)imageKeyList[j]).Path + "1";
                        //                        backgroundWorker1.ReportProgress(i);

                        // 完全相同直接删除     
                        if (bean.checkBox1Flag && merge.GetSameLevel((ImageID_KIND2)imageKeyList[i], (ImageID_KIND2)imageKeyList[j]) == 1.0)
                        {
                            try
                            {
                                //                                textFlg = ((ImageID_KIND2)imageKeyList[j]).Path + "2";
                                //                                backgroundWorker1.ReportProgress(i);

                                File.Delete(((ImageID_KIND2)imageKeyList[j]).Path);

                                imageKeyList.Remove(imageKeyList[j]);
                                j--;
                            }
                            catch (Exception)
                            {
                                if (File.GetAttributes(((ImageID_KIND2)imageKeyList[j]).Path).ToString().IndexOf("ReadOnly") != -1)
                                {
                                    System.IO.File.SetAttributes(((ImageID_KIND2)imageKeyList[j]).Path, System.IO.FileAttributes.Normal); //去除只读
                                    File.Delete(((ImageID_KIND2)imageKeyList[j]).Path);
                                    j--;
                                }
                            }
                            continue;
                        }
                        else if (merge.GetSameLevel((ImageID_KIND2)imageKeyList[i], (ImageID_KIND2)imageKeyList[j]) >= samePercent)
                        {
                            //                            textFlg = ((ImageID_KIND2)imageKeyList[j]).Path + "3";
                            //                            backgroundWorker1.ReportProgress(i);

                            key = ((ImageID_KIND2)imageKeyList[i]).Path;
                            if (myDictionary.ContainsKey(key))
                            {
                                myDictionary[key] = myDictionary[key] + ";" + ((ImageID_KIND2)imageKeyList[j]).Path;
                            }
                            else
                            {
                                myDictionary.Add(key, ((ImageID_KIND2)imageKeyList[j]).Path);
                            }
                            //                           textFlg = ((ImageID_KIND2)imageKeyList[j]).Path + "4";
                            //                           backgroundWorker1.ReportProgress(i);
                        }
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    //                if (i % 10 == 0)
                    {
                        backgroundWorker1.ReportProgress(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (messageFlg == 1)
            {
                this.label3.Text = "文件夹合并中······" + e.ProgressPercentage;
            }
            else if (messageFlg == 2)
            {
                this.label3.Text = "画像分析中······" + e.ProgressPercentage;
            }
            else if (messageFlg == 3)
            {
                this.label3.Text = "画像比较中······" + e.ProgressPercentage;
            }
            else
            {
                this.label3.Text = "准备中······";
            }
        }

        private void backgroundWorker1_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.label3.Text = "处理终止······";
                return;
            }

            this.label3.Text = "比较完了······";

            //按钮活性
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.button3.Text = "开始";
            this.button4.Enabled = true;
            this.button5.Enabled = true;
            this.button6.Enabled = true;
            this.button7.Enabled = true;
            this.button8.Enabled = true;

            //显示一览输出
            this.listView1.BeginUpdate(); //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            ListViewItem lvi;
            foreach (KeyValuePair<string, string> kvp in myDictionary)
            {
                lvi = new ListViewItem();

                lvi.Text = kvp.Key;
                lvi.SubItems.Add(kvp.Value);
                lvi.SubItems.Add("");

                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            //初期选择第一行
            try
            {
                listView1.Items[0].Selected = true;
                this.listView1.Select();
            }
            catch (Exception)
            {
            }
        }

        private void backgroundWorker1_Completed2(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FormClosingButton(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                //取消掉Completed事件,不进行接收
                //                backgroundWorker1.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorker1_Completed2);
                backgroundWorker1.CancelAsync();
            }
        }

/*        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.label1.Text != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = this.label1.Text;
                //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。    
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
                process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设    
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.label2.Text != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = this.label2.Text;
                //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。    
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
                process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设    
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
        }*/

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string f in files)
                {
                    if (!Directory.Exists(f))
                    {
                        continue;
                    }
                    if (!this.listBox1.Items.Contains(f))
                    {
                        this.listBox1.Items.Add(f);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.label1.Text != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = this.label1.Text;
                //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。    
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
                process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设    
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (this.label2.Text != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = this.label2.Text;
                //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。    
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
                process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设    
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
        }

    }

    class ThreadClass
    {
        public int startIndex = 0; //起始位置
        public int endIndex = 0; ////结束位置
        public int count = 0; //处理个数

        public string[] imageList;

        public ArrayList imageKeyList = new ArrayList();

        public Boolean endFlag = false;

        public void imageThreadStart()
        {
            if (startIndex >= imageList.Count())
            {
                endFlag = true;
                return;
            }

            if (endIndex >= imageList.Count())
            {
                endIndex = imageList.Count();
            }

            Merge2 merge = new Merge2();
            for (int i = startIndex; i < endIndex; i++)
            {
                count++;
                imageKeyList.Add(merge.MergePic(imageList[i]));
            }

            endFlag = true;
        }

    }
}
