using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace PicManager.BirthdayPic
{
	/// <summary>
	/// BirthdayPicWindows.xaml の相互作用ロジック
	/// </summary>
	public partial class BirthdayPicWindows : UserControl
	{
		System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

		ObservableCollection<FolderMap> itemList = new ObservableCollection<FolderMap>();//左边树形结构list
		ObservableCollection<ImageValue> imagelist = new ObservableCollection<ImageValue>();//右边图像list

		public Thread getPicListThread;//图像取得用Thread
		public Thread getPicComThread;//图像分类用Thread

		String w2_folderFrom = ""; //整理对象文件夹
		String w2_folderTo = "";   //整理到文件夹
		String w2_editType = "";   //整理类型 0生日 1月
		String w2_birthday = "";   //生日
		String w2_delete_flg = "";   //源文件删除flg
		DateTime w2_birthday_date;

		public BirthdayPicWindows()
		{
			InitializeComponent();
			InitConf();
		}

		//初期配置
		public void InitConf()
		{
			this.treeView_folder.ItemsSource = itemList;
			this.listBox_image.ItemsSource = imagelist;

			w2_folderFrom = ConfigurationManager.AppSettings["w2_folderFrom"];
			w2_folderTo = ConfigurationManager.AppSettings["w2_folderTo"];
			w2_editType = ConfigurationManager.AppSettings["w2_editType"];
			w2_birthday = ConfigurationManager.AppSettings["w2_birthday"];
			w2_delete_flg = ConfigurationManager.AppSettings["w2_delete_flg"];

			this.textBox_folder_from.Text = w2_folderFrom;
			this.textBox_folder_to.Text = w2_folderTo;
			this.radio_age.IsChecked = w2_editType == "0";
			this.radio_month.IsChecked = w2_editType == "1";
			this.checkBox_delete_flg.IsChecked = w2_delete_flg == "1";
			this.textBox_birthday.Text = w2_birthday;

			if ((Boolean)this.radio_month.IsChecked)
			{
				this.textBox_birthday.IsEnabled = false;
			}

		}

		private void button_folder_from_Click(object sender, RoutedEventArgs e)
		{
			if (w2_folderFrom != "")
			{
				folderBrowserDialog1.SelectedPath = w2_folderFrom;
			}

			if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				w2_folderFrom = folderBrowserDialog1.SelectedPath;
				this.textBox_folder_from.Text = w2_folderFrom;
			}
		}

		private void button_folder_to_Click(object sender, RoutedEventArgs e)
		{
			if (w2_folderTo != "")
			{
				folderBrowserDialog1.SelectedPath = w2_folderTo;
			}

			if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				w2_folderTo = folderBrowserDialog1.SelectedPath;
				this.textBox_folder_to.Text = w2_folderTo;
			}
		}

		private void button_save_Click(object sender, RoutedEventArgs e)
		{

			w2_folderFrom = this.textBox_folder_from.Text;
			w2_folderTo = this.textBox_folder_to.Text;
			w2_birthday = this.textBox_birthday.Text;

			Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			cfa.AppSettings.Settings["w2_folderFrom"].Value = w2_folderFrom;
			cfa.AppSettings.Settings["w2_folderTo"].Value = w2_folderTo;
			cfa.AppSettings.Settings["w2_editType"].Value = w2_editType;
			cfa.AppSettings.Settings["w2_birthday"].Value = w2_birthday;
			cfa.AppSettings.Settings["w2_delete_flg"].Value = w2_delete_flg;
			cfa.Save();
		}

		private void textBox_folder_to_TextChanged(object sender, TextChangedEventArgs e)
		{
			w2_folderTo = this.textBox_folder_to.Text;
			if (w2_folderTo != "" && Directory.Exists(w2_folderTo))
			{
				//左面显示文件夹列表
				treeView_Display(w2_folderTo);
			}
		}
		void treeView_Display(object a)
		{
			if (!Directory.Exists(w2_folderTo))
			{
				return;
			}

			if (itemList.Count == 0)
			{
				FolderMap node_age = new FolderMap("年龄单位", @"\img\1.ico", "");
				FolderMap node_month = new FolderMap("年月单位", @"\img\1.ico", "");
				FolderMap node_other = new FolderMap("其他", @"\img\1.ico", "");

				itemList.Add(node_age);
				itemList.Add(node_month);
				itemList.Add(node_other);
			}
			else
			{
				//itemList.Clear();
				itemList[0].Children.Clear();
				itemList[1].Children.Clear();
				itemList[2].Children.Clear();
			}

			DirectoryInfo TheFolder = new DirectoryInfo(w2_folderTo);
			string[] fileList;
			foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
			{
				fileList = Directory.GetFiles(NextFolder.FullName, "*.*", SearchOption.TopDirectoryOnly);
				int okFileCount = 0;
				foreach (string fileCheck in fileList)
				{
					if (isRightImg(fileCheck))
					{
						okFileCount++;
					}
				}

				FolderMap subNode_temp = new FolderMap(NextFolder.Name + " (" + okFileCount + ")", @"\img\2.ico", NextFolder.FullName);

				if (NextFolder.Name.StartsWith("年龄_"))
				{
					sortInsert(itemList[0].Children, subNode_temp);
				}
				else if (NextFolder.Name.StartsWith("年月_"))
				{
					sortInsert(itemList[1].Children, subNode_temp);
				}
				else
				{
					sortInsert(itemList[2].Children, subNode_temp);
				}
			}

			//按钮非活性
			button_folder_from.IsEnabled = true;
			button_folder_to.IsEnabled = true;
			textBox_folder_from.IsEnabled = true;
			textBox_folder_to.IsEnabled = true;
			radio_month.IsEnabled = true;
			radio_age.IsEnabled = true;
			button_start.IsEnabled = true;
		}

		void sortInsert(ObservableCollection<FolderMap> coll, FolderMap pice)
		{
			if (coll.Count == 0)
			{
				coll.Add(pice); //空的直接插
			}
			else
			{
				int i = 0;
				while (true)
				{
					if (i >= coll.Count)
					{
						coll.Add(pice); //最后了
						break;
					}
					if (coll[i].DisplayName.CompareTo(pice.DisplayName) > 0)
					{
						coll.Insert(i, pice);
						break;
					}
					i++;
				}
			}
		}

		private void radio_age_Checked(object sender, RoutedEventArgs e)
		{
			this.textBox_birthday.IsEnabled = true;
			w2_editType = "0";
		}

		private void radio_month_Checked(object sender, RoutedEventArgs e)
		{
			this.textBox_birthday.IsEnabled = false;
			w2_editType = "1";
		}

		private string nodePath_before; //重复选择相同文件夹，不重新执行判断用
		private void treeView_folder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			FolderMap folderMapTemp = (FolderMap)this.treeView_folder.SelectedItem;
			String path = folderMapTemp.FullPath;

			if (path == "" || !Directory.Exists(path) || path == nodePath_before)
			{
				return;
			}
			nodePath_before = path;

			treeView1_draw(path);
		}

		private void treeView1_draw(String path)
		{
			imagelist.Clear();
			if (getPicListThread != null && getPicListThread.IsAlive)
			{
				getPicListThread.Abort();
			}

			getPicListThread = new Thread(new ThreadStart(getPicList));
			getPicListThread.Start(); //开始
									  //getPicList();
		}

		private void getPicList()
		{
			string[] fileList = Directory.GetFiles(nodePath_before, "*.*", SearchOption.TopDirectoryOnly);

			for (int i = 0; i < fileList.Length; i++)
			{
				if (!isRightImg(fileList[i]))
				{
					continue;
				}
				ImageValue imageTemp = new ImageValue();
				imageTemp.imagePath = fileList[i];
				imageTemp.imageName = fileList[i].Substring(fileList[i].LastIndexOf("\\") + 1);
				imageTemp.bitmapFrame = Const.getThumbImage(fileList[i], 120, 120);

				//不能直接调用主线程的数据绑定LIST 用Dispather
				Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(AddItem), imageTemp);
			}
		}
		void AddItem(object item)
		{
			imagelist.Add((ImageValue)item);
		}

		//开始整理 按钮
		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			if (!Directory.Exists(w2_folderFrom))
			{
				MessageBox.Show("处理对象[" + w2_folderFrom + "]文件夹不存在");
				return;
			}

			//处理开始
			//按钮非活性
			button_folder_from.IsEnabled = false;
			button_folder_to.IsEnabled = false;
			textBox_folder_from.IsEnabled = false;
			textBox_folder_to.IsEnabled = false;
			radio_month.IsEnabled = false;
			radio_age.IsEnabled = false;
			button_start.IsEnabled = false;


			w2_birthday = this.textBox_birthday.Text;

			DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
			dtFormat.ShortDatePattern = "yyyy/MM/dd";
			w2_birthday_date = Convert.ToDateTime(w2_birthday, dtFormat);

			if (getPicListThread != null && getPicListThread.IsAlive)
			{
				getPicListThread.Abort();
			}

			if (getPicComThread != null && getPicComThread.IsAlive)
			{
				getPicComThread.Abort();
			}

			getPicComThread = new Thread(new ThreadStart(workBegin));
			getPicComThread.Start(); //开始
		}

		//文件夹整理线程
		private void workBegin()
		{
			string[] fileList = Directory.GetFiles(w2_folderFrom, "*.*", SearchOption.TopDirectoryOnly);

			DateTime dateCreate;
			DateTime dateUpdate;
			String folderName = "";
			String renameFile = "";

			FileInfo fi;

			for (int i = 0; i < fileList.Length; i++)
			{
				if (!isRightImg(fileList[i]))
				{
					if (i % 10 == 0)
					{
						Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(showStatus), i);
					}
					continue;
				}
				fi = new FileInfo(fileList[i]);
				dateCreate = fi.CreationTime;
				dateUpdate = fi.LastWriteTime;

				if (dateUpdate.CompareTo(dateCreate) < 0)
				{
					dateCreate = dateUpdate;
				}

				if (w2_editType == "0") //生日
				{
					folderName = getAge(w2_birthday_date, dateCreate);
					folderName = (w2_folderTo + @"\年龄_" + folderName).Replace(@"\\", @"\");
				}
				else //年月
				{
					folderName = dateCreate.ToString("yyyy_MM");
					folderName = (w2_folderTo + @"\年月_" + folderName).Replace(@"\\", @"\");
				}

				// 文件夹不存在时，做成
				if (!Directory.Exists(folderName))
				{
					Directory.CreateDirectory(folderName);
				}

				int index = 1;
				renameFile = dateCreate.ToString("yyyyMMdd_HHmmss");
				while (File.Exists(folderName + @"\" + renameFile + System.IO.Path.GetExtension(fileList[i]))) //重复时重命名
				{
					renameFile = dateCreate.ToString("yyyyMMdd_HHmmss") + "_" + index;
					index++;
				}
				renameFile = renameFile + System.IO.Path.GetExtension(fileList[i]);

				try
				{
					if (w2_delete_flg == "0") //copy
					{
						File.Copy(fileList[i], folderName + @"\" + renameFile);
					}
					else //删除
					{
						File.Move(fileList[i], folderName + @"\" + renameFile);
					}

				}
				catch (Exception)
				{
					if (File.GetAttributes(fileList[i]).ToString().IndexOf("ReadOnly") != -1)
					{
						System.IO.File.SetAttributes(fileList[i], System.IO.FileAttributes.Normal); //去除只读
						if (w2_delete_flg == "0") //copy
						{
							File.Copy(fileList[i], folderName + @"\" + renameFile);
						}
						else //删除
						{
							File.Move(fileList[i], folderName + @"\" + renameFile);
						}
					}
				}

				if (i % 10 == 0)
				{
					Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(showStatus), i);
				}
			}

			Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(showStatus), -1);

			//不能直接调用主线程的数据绑定LIST 用Dispather 左边的树重新画
			Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(treeView_Display), "");

		}

		private void showStatus(object o)
		{
			if ((int)o == -1)
			{
				this.textBlock_status.Text = "全部处理完成";
			}
			else
			{
				this.textBlock_status.Text = "处理完了枚数:" + (int)o;
			}
		}

		private string getAge(DateTime birthday, DateTime picCreateDay)
		{
			string mess = "";

			//TimeSpan age = picCreateDay.Subtract(birthday);
			int year_span = picCreateDay.Year - birthday.Year;
			int month_span = picCreateDay.Month - birthday.Month;
			int day_span = picCreateDay.Day - birthday.Day;

			if (picCreateDay.CompareTo(birthday) < 0)
			{
				mess = " 出生前";
			}
			else
			{
				if (day_span < 0)
				{
					month_span--;
				}

				if (month_span < 0)
				{
					month_span += 12;
					year_span--;
				}

				if (year_span <= 0)
				{
					mess = year_span.ToString("00") + "岁_" + month_span.ToString("00") + "个月";
				}
				else
				{
					mess = year_span.ToString("00") + "岁";
				}
			}

			return mess;
		}

		private void checkBox_delete_flg_Checked(object sender, RoutedEventArgs e)
		{
			w2_delete_flg = "1";
		}

		private void checkBox_delete_flg_Unchecked(object sender, RoutedEventArgs e)
		{
			w2_delete_flg = "0";
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				string path = ((Image)sender).ToolTip.ToString();
				if (path != "")
				{
					/*                    //建立新的系统进程    
										System.Diagnostics.Process process = new System.Diagnostics.Process();
										//设置文件名，此处为图片的真实路径+文件名    
										process.StartInfo.FileName = path;
										//此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。    
										process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
										//此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true    
										process.StartInfo.UseShellExecute = true;
										//此处可以更改进程所打开窗体的显示样式，可以不设    
										process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
										process.Start();
										process.Close();*/
					Process proc = new Process();

					proc.StartInfo.CreateNoWindow = true;
					proc.StartInfo.FileName = "cmd.exe";
					proc.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
					proc.StartInfo.RedirectStandardError = false;//重定向标准错误输出
					proc.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
					proc.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
					proc.Start();
					proc.StandardInput.WriteLine(path);
					proc.StandardInput.WriteLine("exit");
					string outStr = proc.StandardOutput.ReadToEnd();
					proc.Close();
				}
			}
		}

		private Boolean isRightImg(string file)
		{
			file = file.ToLower();
			if (file.EndsWith(".jpeg")
				|| file.EndsWith(".jpg")
				|| file.EndsWith(".png")
				|| file.EndsWith(".gif")
				|| file.EndsWith(".mp4")
				|| file.EndsWith(".rm")
				|| file.EndsWith(".rmvb")
				|| file.EndsWith(".mov")
				|| file.EndsWith(".wmv")
				|| file.EndsWith(".avi")
				|| file.EndsWith(".3gp")
				|| file.EndsWith(".flv")
				|| file.EndsWith(".txt")
			)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void listBox_image_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				try
				{
					int index = listBox_image.SelectedIndex;
					if (index >= 0)
					{
						string path = ((ImageValue)listBox_image.SelectedItem).imagePath;
						imagelist.RemoveAt(index);
						//删除文件到回收站
						Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
					}
				}
				catch (Exception) { }
			}
		}

		private void button_deleteBat_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Collection<ImageValue> imageDeleteList = new Collection<ImageValue>();

				//批量删除开始
				foreach (ImageValue imageValueTemp in listBox_image.SelectedItems)
				{
					imageDeleteList.Add(imageValueTemp);
				}

				//批量删除开始
				foreach (ImageValue imageValueTemp in imageDeleteList) {
					imagelist.Remove(imageValueTemp);
					//删除文件到回收站
					Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(imageValueTemp.imagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
				}
			}
			catch (Exception) { }
		}

		private void checkBox_multiple_flg_Checked(object sender, RoutedEventArgs e)
		{
			listBox_image.SelectionMode = SelectionMode.Multiple;
		}

		private void checkBox_multiple_flg_Unchecked(object sender, RoutedEventArgs e)
		{
			listBox_image.SelectionMode = SelectionMode.Single;
		}
	}
}
