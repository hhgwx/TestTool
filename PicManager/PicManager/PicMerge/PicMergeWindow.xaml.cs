using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace PicManager.PicMerge
{
    /// <summary>
    /// PicMergeWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PicMergeWindow : UserControl
    {
        System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        String defaultfilePath = "";
        double samePercent = 0.85;

        public Thread picMergeThread;
        public Thread statusUPdateThread;
        delegate void showSamePicListTOForm();
        delegate void status_update_delegate();

        InfValue infValue = new InfValue();
        public PicMergeWindow()
        {
            InitializeComponent();
            InitConf();
        }

        public void InitConf()
        {
            this.button_win1_left_delete.IsEnabled = false;
            this.button_win1_right_delete.IsEnabled = false;
            this.button_win1_all_delete.IsEnabled = false;
            this.button_win1_all_skip.IsEnabled = false;

            listView_samePic.ItemsSource = sameImgList;
            this.comboBox_percent.ItemsSource = infValue.inf_win1_comboBox_percent_list;
            this.comboBox_percent.SelectedValuePath = "Key";
            this.comboBox_percent.DisplayMemberPath = "Value";

            // this.checkBox_merge
            

/*            infValue.inf_win1_comboBox_percent = ConfigurationManager.AppSettings["inf_win1_comboBox_percent"];
            infValue.inf_win1_checkbox_merge = ConfigurationManager.AppSettings["inf_win1_checkbox_merge"];
            infValue.inf_win1_checkBox_sameDel = ConfigurationManager.AppSettings["inf_win1_checkBox_sameDel"];
*/
            this.checkBox_merge.IsChecked = ConfigurationManager.AppSettings["inf_win1_checkbox_merge"] == "ture" ? true:false;
            this.checkBox_sameDel.IsChecked = ConfigurationManager.AppSettings["inf_win1_checkBox_sameDel"] == "ture" ? true : false;
            this.comboBox_percent.SelectedValue = ConfigurationManager.AppSettings["inf_win1_comboBox_percent"];
            defaultfilePath = ConfigurationManager.AppSettings["inf_win1_folderPath"];
        }

        public void saveConf()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["inf_win1_comboBox_percent"].Value = this.comboBox_percent.SelectedValue.ToString();
//            cfa.AppSettings.Settings["inf_win1_checkbox_merge"].Value = infValue.inf_win1_checkbox_merge;
//            cfa.AppSettings.Settings["inf_win1_checkBox_sameDel"].Value = infValue.inf_win1_checkBox_sameDel;

            cfa.AppSettings.Settings["inf_win1_checkbox_merge"].Value = this.checkBox_merge.IsChecked!=null&&(Boolean)this.checkBox_merge.IsChecked?"true":"false";
            cfa.AppSettings.Settings["inf_win1_checkBox_sameDel"].Value = this.checkBox_sameDel.IsChecked != null && (Boolean)this.checkBox_sameDel.IsChecked ? "true" : "false";
            cfa.AppSettings.Settings["inf_win1_folderPath"].Value = defaultfilePath;
            cfa.Save();
        }

        private void button_add_folder_Click(object sender, RoutedEventArgs e)
        {
            //添加文件夹
            folderBrowserDialog1.SelectedPath = defaultfilePath;

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                defaultfilePath = folderBrowserDialog1.SelectedPath;

                if (!listBox_folder.Items.Contains(defaultfilePath))
                {
                    listBox_folder.Items.Add(defaultfilePath);
                }

            }
        }

        private void button_delete_folder_Click(object sender, RoutedEventArgs e)
        {
            //选择项删除
            for (int a = 0; a < listBox_folder.SelectedItems.Count;)
                listBox_folder.Items.Remove(listBox_folder.SelectedItem.ToString());
        }

        private void button_clear_folder_Click(object sender, RoutedEventArgs e)
        {
            //清空按钮
            listBox_folder.Items.Clear();
        }

        private void status_update_thread()
        {
            while  (picMergeThread!=null && picMergeThread.IsAlive)
            {
                status_update_delegate delegateTemp2 = delegate () {
                    this.textBlock_win1_status.Text = statusStr;
                    
                };
                this.Dispatcher.Invoke(delegateTemp2);

                Thread.Sleep(1000);
            }

            status_update_delegate delegateTemp3 = delegate () {
                this.button_add_folder.IsEnabled = true;
                this.button_clear_folder.IsEnabled = true;
                this.button_delete_folder.IsEnabled = true;
                this.button_win1_left_delete.IsEnabled = true;
                this.button_win1_right_delete.IsEnabled = true;
                this.button_win1_all_delete.IsEnabled = true;
                this.button_win1_all_skip.IsEnabled = true;
                this.button_start.Content = "开始";

                this.textBlock_win1_status.Text = statusStr;
            };
            this.Dispatcher.Invoke(delegateTemp3);
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            saveConf(); //配置保存

            if ((string)button_start.Content == "开始")
            {
                //选择文件夹检查
                if (listBox_folder.Items.Count == 0)
                {
                    MessageBox.Show("请选择文件夹");
                    return;
                }

                for (int a = 0; a < listBox_folder.Items.Count; a++)
                {
                    if (!Directory.Exists(listBox_folder.Items[a].ToString()))
                    {
                        MessageBox.Show(listBox_folder.Items[a].ToString() + "不存在");
                        return;
                    }
                }

                //结果一览清空
                sameImgList.Clear();

                //按钮非活性
                this.button_add_folder.IsEnabled = false;
                this.button_clear_folder.IsEnabled = false;
                this.button_delete_folder.IsEnabled = false;
                this.button_win1_left_delete.IsEnabled = false;
                this.button_win1_right_delete.IsEnabled = false;
                this.button_win1_all_delete.IsEnabled = false;
                this.button_win1_all_skip.IsEnabled = false;
                this.button_start.Content = "结束";

                samePercent = Convert.ToInt32(this.comboBox_percent.SelectedValue.ToString()) / 100.0;

                checkBox_merge_Flag = this.checkBox_merge.IsChecked;
                checkBox_sameDel_Flag = this.checkBox_sameDel.IsChecked;
                pathList = this.listBox_folder.Items;

                threadName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                picMergeThread = new Thread(new ThreadStart(mergeThreadStart));
                picMergeThread.Start();

                //进度显示进程
                statusUPdateThread = new Thread(new ThreadStart(status_update_thread));
                statusUPdateThread.Start();
            }
            else
            {
                //按钮活性
                this.button_add_folder.IsEnabled = true;
                this.button_clear_folder.IsEnabled = true;
                this.button_delete_folder.IsEnabled = true;
                this.button_start.Content = "开始";

                if (picMergeThread.IsAlive)
                {
                    picMergeThread.Abort();
                }

                if (statusUPdateThread.IsAlive)
                {
                    statusUPdateThread.Abort();
                }
            }
        }



        //进程处理
        private Boolean? checkBox_merge_Flag = false;
        private Boolean? checkBox_sameDel_Flag = false;
        private Boolean stop_Flag = false;
        private ItemCollection pathList;

        private string statusStr;

        private string threadName;

        int threadNum = Environment.ProcessorCount;

        Dictionary<string, string> myDictionary;
        ObservableCollection<SameImg> sameImgList = new ObservableCollection<SameImg>(); 

        public void mergeThreadStart()
        {
            if (checkBox_merge_Flag == null)
            {
                checkBox_merge_Flag = false;
            }

            if (checkBox_sameDel_Flag == null)
            {
                checkBox_sameDel_Flag = false;
            }

            if ((Boolean)checkBox_merge_Flag)
            {
                //messageFlg = 1;
                //全部文件合并到第一个文件夹
                String mainFloder = pathList[0].ToString();
                String renameFile;


                for (int a = 1; a < pathList.Count; a++)
                {
                    try
                    {
                        string[] fileList = Directory.GetFiles(pathList[a].ToString(), "*.jpg", SearchOption.TopDirectoryOnly);

                        for (int i = 0; i < fileList.Length; i++)
                        {
                            int index = 1;
                            renameFile = Path.GetFileName(fileList[i]);
                            while (File.Exists(mainFloder + @"\" + renameFile)) //重复时重命名
                            {
                                renameFile = Path.GetFileNameWithoutExtension(fileList[i]) + "_" + index + Path.GetExtension(fileList[i]);
                                index++;
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
                                statusStr = pathList[a].ToString() + " 图片合并中---" + i;
                                if (System.Diagnostics.Process.GetProcessesByName(threadName).ToList().Count == 0)
                                {
                                    return;
                                }

                            }

                            if (stop_Flag)
                            {
                                return;
                            }
                        }
                        statusStr = pathList[a].ToString() + " 图片合并中---完成";
                    }
                    catch (Exception) { }
                }
            }

            //画像KEY取得
            Merge2 merge = new Merge2();
            ArrayList imageKeyList = new ArrayList();

            int imageCount = 0;
            int aCount = pathList.Count;
            if ((Boolean)checkBox_merge_Flag)
            {
                aCount = 1;
            }
            for (int a = 0; a < aCount; a++)
            {
                string[] baseFileList = Directory.GetFiles(pathList[a].ToString(), "*.jpg", SearchOption.TopDirectoryOnly);

                Thread[] th = new Thread[threadNum];
                ThreadClass2[] threadClass = new ThreadClass2[threadNum];
                Boolean threadOKFlg = true;

                //处理文件线程分割用
                int imageListCut = (int)Math.Ceiling(baseFileList.Length * 1.0 / threadNum);
                int startCut = 0;
                int endCut = 0;

                for (int i = 0; i < threadNum; i++)
                {
                    endCut = startCut + imageListCut;

                    threadClass[i] = new ThreadClass2();
                    threadClass[i].startIndex = startCut;
                    threadClass[i].endIndex = endCut;
                    threadClass[i].imageList = baseFileList;

                    threadClass[i].threadName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                    th[i] = new Thread(new ThreadStart(threadClass[i].imageCompareThreadStart));
                    th[i].Start();

                    startCut = endCut;
                }

                while (true)
                {
                    Thread.Sleep(1000);
                    if (System.Diagnostics.Process.GetProcessesByName(threadName).ToList().Count == 0)
                    {
                        return;
                    }

                    imageCount = 0;
                    for (int i = 0; i < threadNum; i++)
                    {
                        imageCount = imageCount + threadClass[i].count;
                    }
                    statusStr = "图片分析中---" + imageCount;

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
                        statusStr = "图片分析---完成";
                        break;
                    }

                    //关闭
                    if (stop_Flag)
                    {
                        return;
                    }
                }

            }

            string key = "";
            myDictionary = new Dictionary<string, string>();
            //比较
            for (int i = 0; i < imageKeyList.Count - 1; i++)
            {
                for (int j = i + 1; j < imageKeyList.Count; j++)
                {
                    //                        textFlg = ((ImageID_KIND2)imageKeyList[j]).Path + "1";
                    //                        backgroundWorker1.ReportProgress(i);

                    // 完全相同直接删除     
                    if ((Boolean)checkBox_sameDel_Flag && merge.GetSameLevel((ImageID_KIND2)imageKeyList[i], (ImageID_KIND2)imageKeyList[j]) == 1.0)
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
                    //关闭
                    if (stop_Flag)
                    {
                        return;
                    }
                }
                if (i % 10 == 0)
                {
                    //   backgroundWorker1.ReportProgress(i);
                    statusStr = "图片比较中---" + i;
                    if (System.Diagnostics.Process.GetProcessesByName(threadName).ToList().Count == 0)
                    {
                        return;
                    }
                }
            }
            statusStr = "图片比较---完成";

            //处理结束，显示相似图片一览
            showSamePicListTOForm delegateTemp = delegate ()
            {
                //this.label3.Text = "比较完了······";

                //按钮活性
                /*this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Text = "开始";
                this.button4.Enabled = true;
                this.button5.Enabled = true;
                this.button6.Enabled = true;
                this.button7.Enabled = true;
                this.button8.Enabled = true;*/

                //显示一览输出
                //this.listView_samePic.BeginUpdate(); //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                foreach (KeyValuePair<string, string> kvp in myDictionary)
                {
                    sameImgList.Add(new SameImg(kvp.Key, kvp.Value));
                }

//                listView_samePic.ItemsSource = sameImgList;

                //初期选择第一行
/*                try
                {
                    ((ListViewItem)listView_samePic.Items[0]).IsSelected = true;
                    //this.listView_samePic.Select();
                }
                catch (Exception)
                {
                }*/
            };
            this.Dispatcher.Invoke(delegateTemp);

        }

        private void listView_samePic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBlock_win1_left_path.Text = pathLeft; //地址栏中显示路径
            textBlock_win1_right_path.Text = pathRight; //地址栏中显示路径
            listView1_SelectedImageShow();
        }

        //显示所选行的画像
        private String pathLeft = "";   //左源画像路径
        private String pathRight = "";  //右画像路径
        private int listView_selectIndex = 0;
        private void listView1_SelectedImageShow()
        {
            if (listView_samePic.SelectedIndex >= 0)
            {
                listView_selectIndex = listView_samePic.SelectedIndex;

                //相似画像路径组
                String[] path2;
                
                pathLeft = sameImgList[listView_selectIndex].imageSrc;
                String path2String = sameImgList[listView_selectIndex].imageSame;
                path2 = path2String.Split(';');
                
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

                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = new MemoryStream(File.ReadAllBytes(pathLeft));
                        img.EndInit();
                        this.image_left.Source = img;

                        BitmapImage img2 = new BitmapImage();
                        img2.BeginInit();
                        img2.StreamSource = new MemoryStream(File.ReadAllBytes(pathRight));
                        img2.EndInit();
                        this.image_right.Source = img2;

                        //this.image_right.Source = new BitmapImage(new Uri(pathRight, UriKind.Absolute));

                        textBlock_win1_left_path.Text = pathLeft; //地址栏中显示路径
                        textBlock_win1_right_path.Text = pathRight; //地址栏中显示路径
                    }
                    else
                    {
                        //先做了后面的行删除，导致这个图片已经被删了，直接把文字列替换掉
                        sameImgList[listView_selectIndex].imageSame = path2String.Replace(path2[0] + ";", "").Replace(path2[0], ""); ;
                        if (sameImgList[listView_selectIndex].imageSame == "")
                        {
                            delete_listView1_row();
                        }

                        listView1_SelectedImageShow();
                    }
                }
            }
        }

        //删除所选行


        private void button_win1_left_delete_Click(object sender, RoutedEventArgs e)
        {
            if (pathLeft != "")
            {
                File.Delete(pathLeft); //删除图片
                pathLeft = "";

                image_left.Source=null;
                delete_listView1_row();
            }
        }

        private void button_win1_right_delete_Click(object sender, RoutedEventArgs e)
        {
            if (pathRight != "")
            {
                File.Delete(pathRight); //删除图片
                pathRight = "";

                image_right.Source = null;
                listView1_SelectedImageShow();
            }
        }

        private void button_win1_all_delete_Click(object sender, RoutedEventArgs e)
        {
            if (pathRight != "")
            {
                File.Delete(pathRight); //删除图片
                pathRight = "";
                image_right.Source = null;
            }

            if (pathLeft != "")
            {
                File.Delete(pathLeft); //删除图片
                pathLeft = "";

                image_left.Source = null;
                delete_listView1_row();
            }
        }

        private void button_win1_all_skip_Click(object sender, RoutedEventArgs e)
        {
            image_right.Source = null;
            image_left.Source = null;
            delete_listView1_row();
        }

        private void delete_listView1_row()
        {
            sameImgList.RemoveAt(listView_selectIndex);//删除所选行
            try
            {
                listView_samePic.Focus();
                listView_samePic.SelectedIndex = listView_selectIndex;
                //((ListViewItem)listView_samePic.Items[listView_selectIndex]).IsSelected = true;

                //this.listView_samePic.Select();
                //listView_samePic.EnsureVisible(listView_selectIndex);//保证可见
            }
            catch (Exception)
            {
                //this.pictureBox1.Image = null;
                //this.pictureBox2.Image = null;
                try
                {
                    listView_samePic.Focus();
                    listView_samePic.SelectedIndex = listView_selectIndex - 1;
                   // ((ListViewItem)listView_samePic.Items[listView_selectIndex - 1]).IsSelected = true;
                    
                    //this.listView_samePic.Select();
                    //listView1.EnsureVisible(listView_selectIndex - 1);//保证可见
                    //listView1.EnsureVisible(listView_selectIndex);//保证可见  Select时会触发变更事件，导致listView_selectIndex值变掉，不用再减1
                }
                catch (Exception)
                {
                    //this.pictureBox1.Image = null;
                    //this.pictureBox2.Image = null;
                }
            }
        }

        private void image_left_bigsee_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pathLeft != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = pathLeft;
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

        private void image_right_bigsee_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pathRight != "")
            {
                //建立新的系统进程    
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名    
                process.StartInfo.FileName = pathRight;
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



        private void listBox_folder_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;//必须加
        }

        private void listBox_folder_PreviewDrop(object sender, DragEventArgs e)
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

                    if (!listBox_folder.Items.Contains(f))
                    {
                        listBox_folder.Items.Add(f);
                    }
                }
            }
        }
    }

    class ThreadClass2
    {
        public int startIndex = 0; //起始位置
        public int endIndex = 0; ////结束位置
        public int count = 0; //处理个数

        public string[] imageList;

        public ArrayList imageKeyList = new ArrayList();

        public Boolean endFlag = false;

        public string threadName;

        public void imageCompareThreadStart()
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

                if (i % 20 == 0 && System.Diagnostics.Process.GetProcessesByName(threadName).ToList().Count == 0)
                {
                    return;
                }
            }

            endFlag = true;
        }

    }
}
