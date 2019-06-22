using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;

namespace AllImgToBlank
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DataForm dataForm = new DataForm();
		ObservableCollection<ListBean> dataList = new ObservableCollection<ListBean>();

		Thread[] downThread = new Thread[1];
		Boolean[] threadEndFlg = new Boolean[1];
		Boolean stopFlag = false;
		public MainWindow()
		{
			InitializeComponent();

			initData();
			this.DataContext = dataForm;
			this.listView_dataList.ItemsSource = dataList;
		}

		private void initData()
		{
			dataForm.FromFolder = ConfigurationManager.AppSettings["FromFolder"];
			dataForm.ReMake = Convert.ToBoolean(ConfigurationManager.AppSettings["ReMake"]);
			dataForm.SubFolder = Convert.ToBoolean(ConfigurationManager.AppSettings["SubFolder"]);
			dataForm.BackColor = ConfigurationManager.AppSettings["BackColor"];
			dataForm.BolderColor = ConfigurationManager.AppSettings["BolderColor"];
			dataForm.BolderWidth = ConfigurationManager.AppSettings["BolderWidth"];
			dataForm.ImgType1 = Convert.ToBoolean(ConfigurationManager.AppSettings["ImgType1"]);
			dataForm.ImgType2 = Convert.ToBoolean(ConfigurationManager.AppSettings["ImgType2"]);
			dataForm.ImgType3 = Convert.ToBoolean(ConfigurationManager.AppSettings["ImgType3"]);
			dataForm.ToFolder = ConfigurationManager.AppSettings["ToFolder"];
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

		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			if ((string)button_start.Content == "开始处理")
			{
				if (!Directory.Exists(dataForm.FromFolder))
				{
					MessageBox.Show("处理对象[" + dataForm.FromFolder + "]文件夹不存在");
					return;
				}
				if (dataForm.BackColor == "")
				{
					MessageBox.Show("背景色不存在");
					return;
				}
				if ((dataForm.BolderColor == "" && dataForm.BolderWidth != "")
					|| (dataForm.BolderColor != "" && dataForm.BolderWidth == ""))
				{
					MessageBox.Show("边框颜色，宽度不匹配");
					return;
				}
				if (dataForm.ToFolder == "")
				{
					MessageBox.Show("出力文件夹不存在");
					return;
				}
				if (!dataForm.ImgType1 && !dataForm.ImgType2 && !dataForm.ImgType3)
				{
					MessageBox.Show("处理对象类别没选择");
					return;
				}
				if (dataForm.ToFolder == dataForm.FromFolder)
				{
					MessageBox.Show("输入,输出文件夹不可以相同。");
					return;
				}
				if (dataForm.ToFolder != "" && !Directory.Exists(dataForm.ToFolder))
				{
					Directory.CreateDirectory(dataForm.ToFolder);
				}

				//changeName(dataForm.TextPath, dataForm.CheckFolder, dataForm.CheckFile, dataForm.CheckSubFolder);
				dataForm.FromFolder = (dataForm.FromFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
				dataForm.ToFolder = (dataForm.ToFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");

				if (dataList == null || dataList.Count == 0)
				{
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

				Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				cfa.AppSettings.Settings["FromFolder"].Value = dataForm.FromFolder;
				cfa.AppSettings.Settings["ReMake"].Value = dataForm.ReMake.ToString();
				cfa.AppSettings.Settings["SubFolder"].Value = dataForm.SubFolder.ToString();
				cfa.AppSettings.Settings["BackColor"].Value = dataForm.BackColor;
				cfa.AppSettings.Settings["BolderColor"].Value = dataForm.BolderColor;
				cfa.AppSettings.Settings["BolderWidth"].Value = dataForm.BolderWidth;
				cfa.AppSettings.Settings["ImgType1"].Value = dataForm.ImgType1.ToString();
				cfa.AppSettings.Settings["ImgType2"].Value = dataForm.ImgType2.ToString();
				cfa.AppSettings.Settings["ImgType3"].Value = dataForm.ImgType3.ToString();
				cfa.AppSettings.Settings["ToFolder"].Value = dataForm.ToFolder;
				cfa.Save();

				for (int i = 0; i < downThread.Length; i++)
				{
					downThread[i] = new Thread(new ParameterizedThreadStart(replaceThread));
					threadEndFlg[i] = false;
					downThread[i].Start(i);
				}

				Thread stopThread = new Thread(stopThreadStart);
				stopThread.Start();
				MessageBox.Show("替换完成");
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
		private void replaceThread(object index)
		{
			int indexInt = Convert.ToInt32(index);
			string toPath = "";
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

					if (!System.IO.Directory.Exists(dataForm.ToFolder))
					{
						Directory.CreateDirectory(dataForm.ToFolder);
					}

					toPath = listBeanTemp.Name.Replace(dataForm.FromFolder, dataForm.ToFolder);

					replaceImg(listBeanTemp.Name, toPath, listBeanTemp.ImageFormat);

					listBeanTemp.Result = "OK";
				}
			}
			threadEndFlg[indexInt] = true;
		}

		private void replaceImg(string fromPath, string toPath, System.Drawing.Imaging.ImageFormat imgType)
		{
			if (File.Exists(fromPath))
			{
				if (File.Exists(toPath)) //已经有了
				{
					if (dataForm.ReMake)
					{
						File.Delete(toPath);
					}
					else
					{
						return;
					}
				}
				else
				{
					if (!Directory.Exists(toPath.Substring(0,toPath.LastIndexOf("/")))) {
						Directory.CreateDirectory(toPath.Substring(0, toPath.LastIndexOf("/")));
					}
				}

				try
				{
					Bitmap bitMap = new Bitmap(fromPath);

					System.Drawing.Color backColor = ColorTranslator.FromHtml(dataForm.BackColor);
					System.Drawing.Color bolderColor = ColorTranslator.FromHtml(dataForm.BolderColor);
					int bolderWidth = int.Parse(dataForm.BolderWidth);
					for (int x = 0; x < bitMap.Width; x++)
					{
						for (int y = 0; y < bitMap.Height; y++)
						{
							if (x < bolderWidth || y < bolderWidth || x >= bitMap.Width - bolderWidth || y >= bitMap.Height - bolderWidth)
							{
								bitMap.SetPixel(x, y, bolderColor);
							}
							else
							{
								bitMap.SetPixel(x, y, backColor);
							}
						}
					}
					bitMap.Save(toPath, imgType); //Bmp清楚。JPEG不清楚

				}
				catch (Exception e)
				{
					Console.WriteLine(e.StackTrace);
				}
			}
		}

		int i = 1;
		private void getFileList(String folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				MessageBox.Show("文件夹不存在");
				return;
			}

			if (dataForm.SubFolder)
			{
				string[] folderList = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
				foreach (string folder in folderList)
				{
					getFileList(folder + "/");
				}
			}

			string[] fileList = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
			System.Drawing.Imaging.ImageFormat imageFormatTemp = null;
			foreach (string file in fileList)
			{
				imageFormatTemp = isRightImg(file);
				if (imageFormatTemp != null)
				{
					ListBean listBeanTemp = new ListBean();
					listBeanTemp.No = i;
					listBeanTemp.Name = file;
					listBeanTemp.ImageFormat = imageFormatTemp;
					dataList.Add(listBeanTemp);
					i++;
				}
			}
		}
		private System.Drawing.Imaging.ImageFormat isRightImg(string file)
		{
			if (dataForm.ImgType1 && (file.EndsWith(".jpeg") || file.EndsWith(".jpg") || file.EndsWith(".JPEG") || file.EndsWith(".JPG")))
			{
				return System.Drawing.Imaging.ImageFormat.Jpeg;
			}
			else if (dataForm.ImgType2 && (file.EndsWith(".png") || file.EndsWith(".PNG")))
			{
				return System.Drawing.Imaging.ImageFormat.Png;
			}
			else if (dataForm.ImgType3 && (file.EndsWith(".gif") || file.EndsWith(".GIF")))
			{
				return System.Drawing.Imaging.ImageFormat.Gif;
			}
			else
			{
				return null;
			}
		}
	}
}
