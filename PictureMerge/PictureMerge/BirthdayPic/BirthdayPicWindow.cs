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

namespace PictureMerge.BirthdayPic
{
    public partial class BirthdayPicWindow : UserControl
    {
        String w2_folderFrom = ""; //整理对象文件夹
        String w2_folderTo = "";   //整理到文件夹
        String w2_editType = "";   //整理类型 0生日 1月
        String w2_birthday = "";   //生日

        String nodePath = "";   //点击了的Node路径

        Thread getPicListThread;

        public BirthdayPicWindow()
        {
            InitializeComponent();
            InitConf();
        }

        public void InitConf()
        {
            w2_folderFrom = ConfigurationManager.AppSettings["w2_folderFrom"];
            w2_folderTo = ConfigurationManager.AppSettings["w2_folderTo"];
            w2_editType = ConfigurationManager.AppSettings["w2_editType"];
            w2_birthday = ConfigurationManager.AppSettings["w2_birthday"];

            this.textBox1.Text = w2_folderFrom;
            this.textBox2.Text = w2_folderTo;
            this.radioButton1.Checked = w2_editType == "1";
            this.radioButton2.Checked = w2_editType == "0";
            this.textBox3.Text = w2_birthday;

            if (this.radioButton1.Checked)
            {
                this.textBox3.Enabled = false;
            }

            if (w2_folderTo != "" && Directory.Exists(w2_folderTo))
            {
                //左面显示文件夹列表
                treeView_Display(w2_folderTo);
            }
        }

        public void GetFolderList()
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.textBox3.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.textBox3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (w2_folderFrom != "")
            {
                folderBrowserDialog1.SelectedPath = w2_folderFrom;
            }

            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                w2_folderFrom = folderBrowserDialog1.SelectedPath;
                this.textBox1.Text = w2_folderFrom;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (w2_folderTo != "")
            {
                folderBrowserDialog1.SelectedPath = w2_folderTo;
            }

            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                w2_folderTo = folderBrowserDialog1.SelectedPath;
                this.textBox2.Text = w2_folderTo;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["w2_folderFrom"].Value = w2_folderFrom;
            cfa.AppSettings.Settings["w2_folderTo"].Value = w2_folderTo;
            cfa.AppSettings.Settings["w2_editType"].Value = w2_editType;
            cfa.AppSettings.Settings["w2_birthday"].Value = w2_birthday;
            cfa.Save();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (w2_folderTo != "" && Directory.Exists(w2_folderTo))
            {
                //左面显示文件夹列表
                treeView_Display(w2_folderTo);
            }
        }

        private void treeView_Display(String path)
        {
            if (!Directory.Exists(w2_folderTo))
            {
                return;
            }

            this.treeView1.Nodes.Clear();

            DirectoryInfo TheFolder = new DirectoryInfo(w2_folderTo);
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                TreeNode rootNode = new TreeNode(NextFolder.FullName);        //载入显示 选择显示  
                rootNode.Tag = NextFolder.FullName;                   //树节点数据  
                rootNode.Text = NextFolder.Name;                  //树节点标签内容  
                rootNode.ImageIndex = IconList.ICON0;                 //设置获取结点显示图片
                rootNode.SelectedImageIndex = IconList.ICON0;         //设置选择显示图片
                this.treeView1.Nodes.Add(rootNode);                   //树中添加根目录 

            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ImageList list = new ImageList();
            String path = (String)e.Node.Tag;

            if (path == "" || !Directory.Exists(path) || path== nodePath)
            {
                return;
            }

            treeView1_draw(path);

        }

        private void treeView1_draw(String path)
        {
            this.listView1.Clear();
            if (getPicListThread != null && getPicListThread.IsAlive)
            {
                getPicListThread.Abort();
            }

            nodePath = path;
            getPicListThread = new Thread(new ThreadStart(getPicList));
            getPicListThread.Start(); //开始

        }

        delegate void getPicListTOForm();
        private void getPicList()
        {
            string[] fileList = Directory.GetFiles(nodePath, "*.jpg", SearchOption.TopDirectoryOnly);
            getPicListTOForm delegateTemp = delegate ()
            {
                for (int i = 0; i < fileList.Length; i++)
                {
                    imageList2.Images.Add(fileList[i], Image.FromFile(fileList[i]));
                    listView1.Items.Add(Path.GetFileName(fileList[i]));
                    listView1.Items[i].ImageKey = fileList[i];
                    listView1.Items[i].Tag = fileList[i];

                    if (i%100 == 0) { 
                        Thread.Sleep(100);
                        listView1.Refresh();
                    }
                }
                listView1.Refresh();

            };

            listView1.Invoke(delegateTemp);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text=="停止整理")
            {
                button4.Text = "开始整理";
                if (getPicListThread != null && getPicListThread.IsAlive)
                {
                    getPicListThread.Abort();
                }

                // thread停止
                // TODO
                return;
            }

            if (!Directory.Exists(w2_folderFrom))
            {
                MessageBox.Show("处理对象[" + w2_folderFrom + "]文件夹不存在");
                return;
            }
            //处理开始
            button4.Text = "停止整理";
            treeView1.Nodes.Clear();

            if (getPicListThread != null && getPicListThread.IsAlive)
            {
                getPicListThread.Abort();
            }
            getPicListThread = new Thread(new ThreadStart(workBegin));
            getPicListThread.Start(); //开始

            //处理结束
            button4.Text = "开始整理";
        }


        delegate void getPicListFromPath();
        private void workBegin()
        {
            string[] fileList = Directory.GetFiles(w2_folderFrom, "*.jpg", SearchOption.TopDirectoryOnly);

            DateTime dateCreate;
            DateTime dateUpdate;
            String folderName = "";
            String renameFile = "";

            FileInfo fi;

            for (int i = 0; i < fileList.Length; i++)
            {
                fi = new FileInfo(fileList[i]);
                dateCreate = fi.CreationTime;
                dateUpdate = fi.LastWriteTime;

                if (dateUpdate.CompareTo(dateCreate) < 0)
                {
                    dateCreate = dateUpdate;
                }

                folderName = dateCreate.ToString("yyyy_MM");
                folderName = (w2_folderTo + @"\" + folderName).Replace(@"\\",@"\");

                // 文件夹不存在时，做成
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                int index = 1;
                renameFile = dateCreate.ToString("yyyyMMdd_HHmmss");
                while (File.Exists(folderName + @"\" + renameFile + Path.GetExtension(fileList[i]))) //重复时重命名
                {
                    renameFile = renameFile + "_" + index;
                }
                renameFile = renameFile + Path.GetExtension(fileList[i]);

                try
                {
                    File.Move(fileList[i], folderName + @"\" + renameFile);
                }
                catch (Exception)
                {
                    if (File.GetAttributes(fileList[i]).ToString().IndexOf("ReadOnly") != -1)
                    {
                        System.IO.File.SetAttributes(fileList[i], System.IO.FileAttributes.Normal); //去除只读
                        File.Move(fileList[i], folderName + @"\" + renameFile);
                    }
                }
            }

            getPicListFromPath delegateTemp = delegate ()
            {
                treeView_Display(w2_folderTo);
            };

            treeView1.Invoke(delegateTemp);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int width = this.imageList2.ImageSize.Width;
            int height = this.imageList2.ImageSize.Height;
            if (width * 0.8 > 10 && height * 0.8 > 10)
            {
                width = (int)(this.imageList2.ImageSize.Width * 0.8);
                height = (int)(this.imageList2.ImageSize.Height * 0.8);
                this.imageList2.ImageSize = new System.Drawing.Size(width, height);

                treeView1_draw((String)treeView1.SelectedNode.Tag);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int width = this.imageList2.ImageSize.Width;
            int height = this.imageList2.ImageSize.Height;
            if (width * 1.2 < 255 && height * 1.2 < 255)
            {
                width = (int)(this.imageList2.ImageSize.Width * 1.2);
                height = (int)(this.imageList2.ImageSize.Height * 1.2);

                this.imageList2.ImageSize = new System.Drawing.Size(width, height);
                treeView1_draw((String)treeView1.SelectedNode.Tag);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                if ((String)listView1.SelectedItems[0].Tag != "")
                {
                    //建立新的系统进程    
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    //设置文件名，此处为图片的真实路径+文件名    
                    process.StartInfo.FileName = (String)listView1.SelectedItems[0].Tag;
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
    }
}
