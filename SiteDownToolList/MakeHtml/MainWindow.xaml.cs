using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MakeHtml
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DataForm dataForm = new DataForm();
		ObservableCollection<ListBean> dataList = new ObservableCollection<ListBean>();
        Thread[] downThread = new Thread[10];
        Boolean[] threadEndFlg = new Boolean[10];
        Boolean stopFlag = false;

        string allHtmlStrTemp = "";

        string lastTimePath = "";

        public MainWindow()
		{
			InitializeComponent();
			dataForm.HtmlName = "index.html";
			this.DataContext = dataForm;
			this.listView_dataList.ItemsSource = dataList;
		}

		private void button_selectFolder_Click(object sender, RoutedEventArgs e)
		{
			String oldFile = dataForm.FromFolder;
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = dataForm.FromFolder;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dataForm.FromFolder = folderBrowserDialog.SelectedPath;
			}
		}
		private void getFileList(String folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				MessageBox.Show("文件夹不存在");
				return;
			}

            dataList.Clear();

            string[] folderList = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
			int i = 1;
			foreach (string folder in folderList)
			{
				ListBean listBeanTemp = new ListBean();
				listBeanTemp.No = i;
				listBeanTemp.Name = folder.Substring(folder.LastIndexOf("/")+1);
				listBeanTemp.NameWithPath = folder + "/";
				dataList.Add(listBeanTemp);
				i++;
			}
		}


		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			if ((string)button_start.Content == "开始处理")
			{
				if (dataForm.FromFolder.Trim() == "" || dataForm.HtmlName.Trim() == "")
				{
					MessageBox.Show("未入力");
					return;
				}
				dataForm.FromFolder = (dataForm.FromFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
				if ((dataList == null || dataList.Count == 0) || lastTimePath != dataForm.FromFolder)
				{
                    lastTimePath = dataForm.FromFolder;
                    getFileList(dataForm.FromFolder);
				}

				if (dataList == null || dataList.Count == 0)
				{
					MessageBox.Show("没有处理对象");
					return;
				}

				if (MessageBox.Show("数据导入完毕。\n立即开始处理吗？", "标题", MessageBoxButton.YesNo) == MessageBoxResult.No)
				{
					return;
				}

				button_start.Content = "停止处理";
                stopFlag = false;

				//makeTopListPage
				makeTopListPage();

				//readFile
				allHtmlStrTemp = readFile(System.Windows.Forms.Application.StartupPath + "/index.html");

                for (int i = 0; i < downThread.Length; i++)
                {
                    downThread[i] = new Thread(new ParameterizedThreadStart(makeThread));
                    threadEndFlg[i] = false;
                    downThread[i].Start(i);
                }

                Thread stopThread = new Thread(stopThreadStart);
                stopThread.Start();
            }
            else
            {
                button_start.Content = "停止中...";
                button_start.IsEnabled = false;

                stopFlag = true;
            }
        }
		private void makeTopListPage()
		{
			if (File.Exists(dataForm.FromFolder + "top.html")) {
				File.Delete(dataForm.FromFolder + "top.html");
			}
			string htmlStr = "";
			foreach (ListBean listBeanTemp in dataList)
			{
				htmlStr = htmlStr + "<a href='./" + listBeanTemp.Name + "/" + dataForm.HtmlName + "'>" + listBeanTemp.Name + "</a><br>\n";
			}
			htmlStr = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body>\n" + htmlStr + "</body></html>";

			writeFile(dataForm.FromFolder + "top.html", htmlStr);
		}
		private void stopThreadStart()
        {
            for (int i = 0; i < threadEndFlg.Length; i++)
            {
                if (!threadEndFlg[i])
                {
                    i--;
                    Thread.Sleep(5000);
                }
            }
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(changeValueByInvoke), 0);
        }
        void changeValueByInvoke(object item)
        {
            button_start.Content = "开始处理";
            button_start.IsEnabled = true;
        }
        private void makeThread(object index)
		{
            int indexInt = Convert.ToInt32(index);
            string toFile = "";
			foreach (ListBean listBeanTemp in dataList)
			{
                if (stopFlag)
                {
                    threadEndFlg[indexInt] = true;
                    return;
                }

				if (listBeanTemp.Result != "OK" && listBeanTemp.Result != "DO")
				{
					listBeanTemp.Result = "DO";

					if (!dataForm.ReMake && File.Exists(listBeanTemp.NameWithPath + "/" + dataForm.HtmlName))
					{
						listBeanTemp.Result = "OK";
						continue;
					}
					toFile = listBeanTemp.NameWithPath + dataForm.HtmlName;
					if (File.Exists(toFile) && dataForm.ReMake)
					{
						File.Delete(toFile);
					}

					string[] folderList = Directory.GetDirectories(listBeanTemp.NameWithPath, "*", SearchOption.TopDirectoryOnly);
					int minPage = 1000;
					int maxPage = 1;
					int nowPage = 1;
					foreach (string folder in folderList)
					{
						nowPage = Convert.ToInt32(folder.Substring(folder.LastIndexOf("/")+1));
						if (minPage > nowPage) {
							minPage = nowPage;
						}
						if (maxPage < nowPage)
						{
							maxPage = nowPage;
						}
					}

                    string div_top = "";
                    string div_page = "";
                    string allHtmlStr = allHtmlStrTemp;

                    div_top = "<a href='#' onclick='allPage()'>allPage</a><br/>\n";
                    for (int i= minPage;i<= maxPage;i++)
					{
                        div_top = div_top + "<a href='#' onclick='changePageTo(" + i +")'>Page" + i + "</a><br/>\n";

                        string[] fileList = Directory.GetFiles(listBeanTemp.NameWithPath + i + "/", "*.*", SearchOption.TopDirectoryOnly);
						fileList = fileList.OrderBy(p => p).ToArray();
                        div_page = div_page + "<script type='text/x-template' id='div_" + i + "'>\n";
						div_page = div_page + "<p>Page_" + i + " Start</p>\n";
						foreach (string file in fileList)
						{
                            div_page = div_page + "<p><img src='" + i +"/" + file.Substring(file.LastIndexOf("/")+1) + "'/></p>\n";
						}
						div_page = div_page + "<p>Page_" + i + " End</p>\n";
						div_page = div_page + "<BR/></script>\n";
                    }
					div_top = div_top + "<BR/><a href='../top.html'>TopPage</a><br/>\n";
					allHtmlStr = allHtmlStr.Replace("$div_top", div_top).Replace("$div_page", div_page).Replace("$allPageNum", maxPage+"");

                    //writeFile
                    writeFile(listBeanTemp.NameWithPath + "/" + dataForm.HtmlName.Trim(), allHtmlStr);

                    listBeanTemp.Result = "OK";
				}
			}
            threadEndFlg[indexInt] = true;
        }

        private string readFile(String filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                MessageBox.Show("文件不存在");
                return "";
            }

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader read = new StreamReader(fs);
            //read.BaseStream.Seek(0, SeekOrigin.Begin);

            string allHtml = "";
            allHtml += read.ReadToEnd();

 //           while (read.Peek() > -1)
 //           {
 //               allHtml += read.ReadToEndAsync();
//            }
            return allHtml;
        }
        private void writeFile(string filePath,string fileStr)
        {
            FileInfo finfo = new FileInfo(filePath);
            using (FileStream fs = finfo.OpenWrite())
            {
                ///根据上面创建的文件流创建写数据流 
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));

                ///把新的内容写到创建的HTML页面中 
                sw.Write(fileStr);
                sw.Flush();
                sw.Close();
            }
        }


        /*		public class MyDateSorter : IComparer
                {
                    #region IComparer Members   
                    public int Compare(object x, object y)
                    {
                        if (x == null && y == null)
                        {
                            return 0;
                        }
                        if (x == null)
                        {
                            return -1;
                        }
                        if (y == null)
                        {
                            return 1;
                        }
                        FileInfo xInfo = (FileInfo)x;
                        FileInfo yInfo = (FileInfo)y;


                        //依名稱排序   
                        return xInfo.FullName.CompareTo(yInfo.FullName);//遞增
                        //return yInfo.FullName.CompareTo(xInfo.FullName);//遞減   

                        //依修改日期排序   
                        //return xInfo.LastWriteTime.CompareTo(yInfo.LastWriteTime);//遞增   
                        //return yInfo.LastWriteTime.CompareTo(xInfo.LastWriteTime);//遞減   
                    }
                    #endregion
                }*/
    }
}
