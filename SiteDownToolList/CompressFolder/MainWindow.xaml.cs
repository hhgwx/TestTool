
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace CompressFolder
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DataForm dataForm = new DataForm();
		ObservableCollection<ListBean> dataList = new ObservableCollection<ListBean>();
		string[] changeTo32 = { "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

		static string pwdListFile = "passwordList.txt";
        static int threadNum = 4;
		Thread[] downThread = new Thread[threadNum];
		Boolean[] threadEndFlg = new Boolean[threadNum];
		Boolean stopFlag = false;
		int selectType = 0; //0:压缩   1:解压缩
		string bkFromFoldr = "";

		public MainWindow()
		{
			InitializeComponent();

			dataForm.FromFolder = "D:/test";
			dataForm.ToFolder = "D:/test_zip";

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
			Regex reg = new Regex("^[0-9\\-]*[0-9]");
			if (!Directory.Exists(folderPath))
			{
				MessageBox.Show("文件夹不存在");
				return;
			}

			if (this.radio_type.Text == "压缩")
			{
				selectType = 0;
				string[] folderList = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
				int i = 1;
				foreach (string folder in folderList)
				{
					ListBean listBeanTemp = new ListBean();
					listBeanTemp.No = i;
					listBeanTemp.FromNameWithPath = folder + "/";
					listBeanTemp.FromName = folder.Substring(folder.LastIndexOf("/") + 1);
					listBeanTemp.ToName = "(" + ("00000" + i).Substring(("00000" + i).Length - 5) + ")" + reg.Match(listBeanTemp.FromName).Value + ".zip";
					listBeanTemp.ToNameWithPath = dataForm.ToFolder + listBeanTemp.ToName;
					listBeanTemp.Password = getPassword(listBeanTemp.FromName);
					dataList.Add(listBeanTemp);
					i++;
				}
			}
			else
			{
				selectType = 1;
				FileStream fs = new FileStream(dataForm.FromFolder + pwdListFile, FileMode.OpenOrCreate, FileAccess.Read);
				StreamReader read = new StreamReader(fs);
				read.BaseStream.Seek(0, SeekOrigin.Begin);

				Collection<ListBean> dataListPwd = new Collection<ListBean>();
				ListBean listBeanPwd;

				while (read.Peek() > -1)
				{
					String[] lineValue = read.ReadLine().Split('\t');
					if (lineValue.Length >= 5 && lineValue[0] != "")
					{
						listBeanPwd = new ListBean();
						listBeanPwd.ToName = lineValue[2];
						listBeanPwd.Password = lineValue[3];
						dataListPwd.Add(listBeanPwd);
					}
				}

				string[] fileList = Directory.GetFiles(folderPath, "*.zip", SearchOption.TopDirectoryOnly);
				int i = 1;
				foreach (string file in fileList)
				{
					ListBean listBeanTemp = new ListBean();
					listBeanTemp.No = i;
					listBeanTemp.FromNameWithPath = file;
					listBeanTemp.FromName = file.Substring(file.LastIndexOf("/") + 1);
					foreach (ListBean pwdTemp in dataListPwd) { //从文件取密码
						if (pwdTemp.ToName == listBeanTemp.FromName) {
							listBeanTemp.Password = pwdTemp.Password;
							break;
						}
					}
					dataList.Add(listBeanTemp);
					i++;
				}
			}

		}
		private string getPassword(string name)
		{
			if (name == null || name.Length == 0) {
				return "";
			}
			string getNum = "";
			int waitToChangeValue = 0;
			for (int i =0;i< name.Length;i++) {
				if (int.TryParse(name[i]+"", out waitToChangeValue) == true) //获得所有数字
				{
					getNum = (waitToChangeValue + 1) + getNum;
				}
			}
			if (getNum == "") {
				getNum = "1";
			}
            //waitToChangeValue = Convert.ToInt32(getNum) * name.Length;
            //waitToChangeValue = Math.Abs((name + waitToChangeValue).GetHashCode());
            waitToChangeValue = Math.Abs((name + getNum).GetHashCode());

            int leftValue = 0;
			string password = "";
			while (waitToChangeValue > 0)
			{
				leftValue = waitToChangeValue % changeTo32.Length;
				//if (leftValue == 0) leftValue = changeTo32.Length;
				password = changeTo32[leftValue] + password;
				waitToChangeValue = (waitToChangeValue - leftValue) / changeTo32.Length;
			}
			return password;
		}
		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			if ((string)button_start.Content == "开始处理")
			{
				if (dataForm.FromFolder.Trim() == "" || dataForm.ToFolder.Trim() == "")
				{
					MessageBox.Show("未入力");
					return;
				}
				dataForm.FromFolder = (dataForm.FromFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
				dataForm.ToFolder = (dataForm.ToFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
				if ((dataList == null || dataList.Count == 0) || bkFromFoldr != dataForm.FromFolder)
				{
					bkFromFoldr = dataForm.FromFolder;
					if (dataList.Count > 0) {
						dataList.Clear();
					}
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

				if (!Directory.Exists(dataForm.ToFolder))
				{
					Directory.CreateDirectory(dataForm.ToFolder);
				}

				button_start.Content = "停止处理";
				stopFlag = false;

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
			foreach (ListBean listBeanTemp in dataList)
			{
				if (stopFlag)
				{
					threadEndFlg[indexInt] = true;
					return;
				}
				if (listBeanTemp.Result != "OK" && listBeanTemp.Result != "DO" && listBeanTemp.Result != "NG")
				{
					if (selectType == 0) //压缩
					{
						listBeanTemp.Result = "DO";
						if (File.Exists(listBeanTemp.ToNameWithPath) && dataForm.ReMake)
						{
							File.Delete(listBeanTemp.ToNameWithPath);
						}
						if (dataForm.Password)
						{
							listBeanTemp.Result = ICSharpZip.CreateZip(listBeanTemp.FromNameWithPath, listBeanTemp.ToNameWithPath, listBeanTemp.Password);
						}
						else
						{
							listBeanTemp.Result = ICSharpZip.CreateZip(listBeanTemp.FromNameWithPath, listBeanTemp.ToNameWithPath, "");
						}
					}
					else  //解压缩
					{
						listBeanTemp.Result = "DO";
						if (dataForm.Password)
						{
							listBeanTemp.Result = ICSharpZip.UnZip(listBeanTemp.FromNameWithPath, dataForm.ToFolder, listBeanTemp.Password,dataForm.ReMake);
						}
						else
						{
							listBeanTemp.Result = ICSharpZip.UnZip(listBeanTemp.FromNameWithPath, dataForm.ToFolder, "", dataForm.ReMake);
						}
					}
				}
			}
			threadEndFlg[indexInt] = true;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (selectType == 0) //压缩 
			{
				//save To file
				saveToFile(dataForm.ToFolder + pwdListFile);
			}
		}

		private void saveToFile(string filePath)
		{
			if (dataList == null || dataList.Count == 0)
			{
				return;
			}

			if (System.IO.File.Exists(filePath))
			{
				if (System.IO.File.Exists(filePath + ".bak"))
				{
					System.IO.File.Delete(filePath + ".bak");
					System.IO.File.Move(filePath, filePath + ".bak");
				}
				else
				{
					System.IO.File.Move(filePath, filePath + ".bak");
				}
			}

			FileInfo finfo = new FileInfo(filePath);
			using (FileStream fs = finfo.OpenWrite())
			{
				///根据上面创建的文件流创建写数据流 
				StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));

				///把新的内容写到创建的HTML页面中 
				foreach (ListBean listBeanTemp in dataList)
				{
					sw.WriteLine(listBeanTemp.No + "\t" + listBeanTemp.FromName + "\t" + listBeanTemp.ToName + "\t" + listBeanTemp.Password + "\t" + listBeanTemp.Result);
				}
				sw.Flush();
				sw.Close();
			}
		}

	}
}
