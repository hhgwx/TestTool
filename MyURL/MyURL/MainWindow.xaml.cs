using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using System.Diagnostics;

namespace MyURL
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<string> classList = new ObservableCollection<string>(); //书签类别下拉列表
		ObservableCollection<BookMarkBean> bookMarkList = new ObservableCollection<BookMarkBean>(); //当前显示用书签List
		String password;
		Boolean changeFlg = false; //变更FLG  最后修正保存判断用
		Boolean loginOK = false;

		public Dictionary<string, string> browser_list = new Dictionary<string, string>() {
{"1","Chrome.exe"},
{"2","Firefox.exe"},
{"3","iexplore.exe"},
{"4","opera.exe"},
{"5","Safari.exe"},
{"6","Netscape.exe"},
{"7","Maxthon.exe"},
{"8","360SE.exe"},
{"9","sogouexplorer.exe"},
{"10","The world.exe"},
{"11","CMD"}
};

		Dictionary<string, ObservableCollection<BookMarkBean>> dic = new Dictionary<string, ObservableCollection<BookMarkBean>>();

		public MainWindow()
		{
			InitializeComponent();

			InitData();
            //showData("1");

            listView_View.ContextMenu = ReContextMenu(classList);
		}

		private void InitData()
		{
			//int status = 0;//0:密码  1:类别  2:书签详细
			int lineNum = 1; //按顺序排列，通过求余知道是什么内容

			string classNameNow = ""; //当前循环的书签类名
			string classNameNew = ""; //新循环的书签类名
			ObservableCollection<BookMarkBean> bookMarkListTemp = new ObservableCollection<BookMarkBean>();//当前循环的书签List
			BookMarkBean bookMarkTemp = new BookMarkBean();//当前循环的书签

			StreamReader sr = (new System.IO.StreamReader(@"const.ini"));
			String lineStr = sr.ReadLine();
			while (lineStr != null)
			{
				if (lineNum == 1)
				{   //第一行密码
					password = Des.DESDecrypt(lineStr);

				}
				else
				{
					switch ((lineNum - 1) % 6)
					{
						case 1:
							classNameNew = Des.DESDecrypt(lineStr);

							if (classNameNew != classNameNow && lineNum != 2)
							{
								//新书签List时，把上一个保存到字典中
								dic.Add(classNameNow, bookMarkListTemp);
								bookMarkListTemp = new ObservableCollection<BookMarkBean>();
								classList.Add(classNameNow);
							}
							classNameNow = classNameNew;

							bookMarkTemp = new BookMarkBean();
							//bookMarkTemp.ClassName = lineStr;

							break;
						case 2:
							bookMarkTemp.No = lineStr;
							break;
						case 3:
							bookMarkTemp.ShuoMing = Des.DESDecrypt(lineStr);
							break;
						case 4:
							bookMarkTemp.Url = Des.DESDecrypt(lineStr);
							break;
						case 5:
							bookMarkTemp.User = Des.DESDecrypt(lineStr);
							break;
						case 0:
							//书签区块的最后一行时，保存在同类书签List中
							bookMarkTemp.Psw = Des.DESDecrypt(lineStr);
							if (bookMarkTemp.No != "") //No==""无效的空白书签
							{
								bookMarkListTemp.Add(bookMarkTemp);
							}
							break;
					}
				}

				lineNum++;
				lineStr = sr.ReadLine();
				continue;
			}
			//读到最后，将没保存的最后一个保存
			dic.Add(classNameNow, bookMarkListTemp);
			classList.Add(classNameNow);
			sr.Close();

			this.comboBox.ItemsSource = classList;

			//初期选择
			//int beforSelected = classList.IndexOf(ConfigurationManager.AppSettings["defaultClass"]);
			if (ConfigurationManager.AppSettings["defaultClass"].StartsWith("*"))
			{
				//前回选择的有密码，就选第一个没密码的
				classSelectChange(-1, true);
			}
			else
			{
				classSelectChange(classList.IndexOf(ConfigurationManager.AppSettings["defaultClass"]), true);
			}

			//浏览器区块设置
			this.comboBox_browser.ItemsSource = browser_list;
			this.comboBox_browser.SelectedValuePath = "Key";
			this.comboBox_browser.DisplayMemberPath = "Value";
			this.comboBox_browser.SelectedValue = ConfigurationManager.AppSettings["defaultBrowser"];

			//3个参数设置
			this.textBox_parm1.Text = ConfigurationManager.AppSettings["defaultParm1"];
			this.textBox_parm2.Text = ConfigurationManager.AppSettings["defaultParm2"];
			this.textBox_parm3.Text = ConfigurationManager.AppSettings["defaultParm3"];


			listView_View.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(listView_View_MouseLeftButtonDown), true);
		}

		private void btn_EditBookMark_Click(object sender, RoutedEventArgs e)
		{
			if ((string)btn_EditBookMark.Content == "编辑书签")
			{
				if (!isPasswordOK())
				{
					return;
				}

				btn_EditBookMark.Content = "保存书签";
				listView_View.Visibility = Visibility.Hidden;
				listView_Edit.Visibility = Visibility.Visible;
			}
			else
			{
				btn_EditBookMark.Content = "编辑书签";
				listView_View.Visibility = Visibility.Visible;
				listView_Edit.Visibility = Visibility.Hidden;
			}

			changeFlg = true;
		}

		//添加书签类
		private void btn_AddClass_Click(object sender, RoutedEventArgs e)
		{
			String newClass = textBox.Text;
			if (newClass == "" || newClass.Trim() == "")
			{
				MessageBox.Show("请输入新分类名");
				this.textBox.Focus();
				return;
			}

			if (this.CheckBox_Personal.IsChecked.GetValueOrDefault())
			{
				newClass = "*" + newClass;
			}
			else
			{
				newClass = "  " + newClass;
			}

			if (classList.Contains(newClass))
			{
				MessageBox.Show("已存在的分类名，请重新输入");
				this.textBox.Focus();
				return;
			}

			ObservableCollection<BookMarkBean> bookMarkListTemp = new ObservableCollection<BookMarkBean>();
			classList.Add(newClass);
			dic.Add(newClass, bookMarkListTemp);

			classSelectChange(classList.IndexOf(newClass), false);

			//textBox.Text = "";
			changeFlg = true;

            //重构右键菜单
            listView_View.ContextMenu = ReContextMenu(classList);
        }

		//修改书签类
		private void btn_ChangeClass_Click(object sender, RoutedEventArgs e)
		{
			String newClassName = textBox.Text;
			String oldClassName = comboBox.Text;
			if (newClassName == "" || newClassName.Trim() == "")
			{
				MessageBox.Show("请输入修正后分类名");
				this.textBox.Focus();
				return;
			}

			if (this.CheckBox_Personal.IsChecked.GetValueOrDefault())
			{
				newClassName = "*" + newClassName;
			}
			else
			{
				newClassName = "  " + newClassName;
			}

			if (classList.Contains(newClassName))
			{
				MessageBox.Show("已存在的分类名，请重新输入");
				this.textBox.Focus();
				return;
			}

			int indexTemp = comboBox.SelectedIndex;
			classList[comboBox.SelectedIndex] = newClassName;
			comboBox.SelectedIndex = indexTemp;

			KeyValuePair<string, ObservableCollection<BookMarkBean>> dicTemp = new KeyValuePair<string, ObservableCollection<BookMarkBean>>(newClassName, dic[oldClassName]);
			dic = dic.ToDictionary(k => k.Key == oldClassName ? newClassName : k.Key, k => k.Value); //修改DIC值

			changeFlg = true;

            //重构右键菜单
            listView_View.ContextMenu = ReContextMenu(classList);
        }

		//删除书签类
		private void btn_DelClass_Click(object sender, RoutedEventArgs e)
		{
			String nowClass = comboBox.Text;
			if (dic.ContainsKey(nowClass))
			{
				ObservableCollection<BookMarkBean> BookMarkBeanTemp = dic[nowClass];

				if (BookMarkBeanTemp.Count == 0
				   || (BookMarkBeanTemp[0].No == null || BookMarkBeanTemp[0].No == ""))
				{
					classList.Remove(nowClass);
					dic.Remove(nowClass);

					classSelectChange(-1, true);//重新选择第一项
				}
				else
				{
					MessageBoxResult confirmToDel = MessageBox.Show("这个书签类别中还有书签，全部删除吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
					if (confirmToDel == MessageBoxResult.Yes)
					{
						classList.Remove(nowClass);
						dic.Remove(nowClass);
					}
				}

				changeFlg = true;

                //重构右键菜单
                listView_View.ContextMenu = ReContextMenu(classList);
            }
		}

		private void btn_AddBookMark_Click(object sender, RoutedEventArgs e)
		{
			AddBookMark addBookMark = new AddBookMark();
			addBookMark.ShowDialog();

			if (addBookMark.textBox_Url.Text != "")
			{
				BookMarkBean bookMarkBeanTemp = new BookMarkBean();
				bookMarkBeanTemp.ShuoMing = addBookMark.textBox_ShuoMing.Text;
				bookMarkBeanTemp.Url = addBookMark.textBox_Url.Text;
				bookMarkBeanTemp.User = addBookMark.textBox_User.Text;
				bookMarkBeanTemp.Psw = addBookMark.textBox_Psw.Text;

				if (bookMarkList.Count > 0)
				{
					int no = 0;
					try
					{
						no = int.Parse(bookMarkList[bookMarkList.Count - 1].No);
					}
					catch (Exception)
					{
						bookMarkList.Clear();
					}
					bookMarkBeanTemp.No = no + 1 + "";
				}
				else
				{
					bookMarkBeanTemp.No = "1";
				}

				bookMarkList.Add(bookMarkBeanTemp);
				changeFlg = true;
			}
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (changeFlg) //发生修正
			{
				MessageBoxResult confirmToDel = MessageBox.Show("要保存所有修改吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (confirmToDel == MessageBoxResult.Yes)
				{
					saveData();
				}
			}

			saveConf();
		}

		private void comboBox_DropDownClosed(object sender, EventArgs e)
		{
			if (dic.Count > 0)
			{

				classSelectChange(classList.IndexOf(comboBox.Text), true);
			}
		}

		//URL跳转按钮按下
		private void btn_LineRun_Click(object sender, RoutedEventArgs e)
		{
			string url = (sender as Button).Tag.ToString();

			//URL中是否有参数，有的话，替换
			if (url.IndexOf("$") > 0)
			{
				url = url.Replace("$1", this.textBox_parm1.Text);
				url = url.Replace("$2", this.textBox_parm2.Text);
				url = url.Replace("$3", this.textBox_parm3.Text);
			}

			string browser = this.comboBox_browser.Text;
			if (browser == "CMD")
			{
				//MessageBox.Show(runCmd(url));
				if (url.IndexOf(" ") >= 0)
				{
					Open(url.Substring(0, url.IndexOf(" ")),url.Substring(url.IndexOf(" ")));
				}
				else
				{
					Open(url);
				}

				Clipboard.SetDataObject((sender as Button).Uid.ToString());
			}
			else
			{
				try
				{
					System.Diagnostics.Process.Start(browser, url);
				}
				catch (Exception)
				{
					foreach (KeyValuePair<string, string> browserTemp in browser_list)
					{
						try
						{
							System.Diagnostics.Process.Start(browserTemp.Value, url);
							break;
						}
						catch (Exception)
						{
							continue;
						}
					}
				}
			}
		}

		//conf文件保存
		public void saveConf()
		{
			Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			cfa.AppSettings.Settings["defaultBrowser"].Value = this.comboBox_browser.SelectedValue.ToString();
			cfa.AppSettings.Settings["defaultClass"].Value = this.comboBox.Text;
			cfa.AppSettings.Settings["defaultParm1"].Value = this.textBox_parm1.Text;
			cfa.AppSettings.Settings["defaultParm2"].Value = this.textBox_parm2.Text;
			cfa.AppSettings.Settings["defaultParm3"].Value = this.textBox_parm3.Text;
			cfa.Save();
		}

		/**********************
		 * index:要显示的第几个书签列表，-1时，显示第一个没密码的
		 * passwordMust：私密列表要密码吗。（添加时不要输入密码）
		 * ******************/
		public void classSelectChange(int index, Boolean passwordMust)
		{
			if (index < 0)
			{
				for (int i = 0; i < classList.Count; i++)
				{
					if (!classList[i].StartsWith("*"))
					{
						index = i;
						break;
					}
				}
			}

			if (dic.Count > index)//重新选择第一项
			{
				string nowClassName = classList[index];

				if (nowClassName.StartsWith("*"))
				{
					if (passwordMust && !isPasswordOK())
					{
						return;
					}

					textBox.Text = nowClassName.Substring(1);
					CheckBox_Personal.IsChecked = true;
				}
				else if (nowClassName.StartsWith("  "))
				{
					textBox.Text = nowClassName.Substring(2);
					CheckBox_Personal.IsChecked = false;
				}
				else
				{
					textBox.Text = nowClassName;
					CheckBox_Personal.IsChecked = false;
				}

				bookMarkList = dic[classList[index]];
				this.comboBox.SelectedIndex = index;
			}
			else
			{
				bookMarkList.Clear();
			}
			listView_View.ItemsSource = bookMarkList;
			listView_Edit.ItemsSource = bookMarkList;
		}

		//书签行删除按钮
		private void btn_LineDel_Click(object sender, RoutedEventArgs e)
		{
			string lineNum = (sender as Button).Tag.ToString();

			BookMarkBean bookMarkBeanTemp = getbookMarkBeanByNo(lineNum);
			if (bookMarkBeanTemp != null)
			{
				bookMarkList.Remove(bookMarkBeanTemp);
			}
		}

		private BookMarkBean getbookMarkBeanByNo(string no)
		{
			BookMarkBean bookMarkBeanReturn = null;
			foreach (BookMarkBean bookMarkBeanTemp in bookMarkList)
			{
				if (no == bookMarkBeanTemp.No)
				{
					bookMarkBeanReturn = bookMarkBeanTemp;
					break;
				}
			}
			return bookMarkBeanReturn;
		}

		private string runCmd(string cmd)
		{
			Process proc = new Process();

			proc.StartInfo.CreateNoWindow = true;
			proc.StartInfo.FileName = "cmd.exe";
			proc.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
			proc.StartInfo.RedirectStandardError = false;//重定向标准错误输出
			proc.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
			proc.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
			proc.Start();
			proc.StandardInput.WriteLine(cmd);
			proc.StandardInput.WriteLine("exit");
			string outStr = proc.StandardOutput.ReadToEnd();
			proc.Close();
			return outStr;
		}

		/// <summary>
		/// 正常启动window应用程序（d:\*.exe）
		/// </summary>
		public static void Open(String exe)
		{
			System.Diagnostics.Process.Start(exe);
		}

		/// <summary>
		/// 正常启动window应用程序（d:\*.exe）,并传递初始命令参数
		/// </summary>
		public static void Open(String exe, String args)
		{
			System.Diagnostics.Process.Start(exe, args);
		}

		private void saveData()
		{
			String stBuffer = "";
			stBuffer = stBuffer + Des.DESEncrypt(password);
			foreach (KeyValuePair<string, ObservableCollection<BookMarkBean>> dicTemp in dic)
			{
				int numCnt = 1;
				if (dicTemp.Value.Count > 0)
				{
					foreach (BookMarkBean bookMarkBeanTemp in dicTemp.Value)
					{
						stBuffer = stBuffer + "\n" + Des.DESEncrypt(dicTemp.Key) + "\n";
						stBuffer = stBuffer + numCnt + "\n";
						stBuffer = stBuffer + Des.DESEncrypt(bookMarkBeanTemp.ShuoMing) + "\n";
						stBuffer = stBuffer + Des.DESEncrypt(bookMarkBeanTemp.Url) + "\n";
						stBuffer = stBuffer + Des.DESEncrypt(bookMarkBeanTemp.User) + "\n";
						stBuffer = stBuffer + Des.DESEncrypt(bookMarkBeanTemp.Psw);
						numCnt++;
					}
				}
				else
				{
					stBuffer = stBuffer + "\n" + Des.DESEncrypt(dicTemp.Key) + "\n";
					stBuffer = stBuffer + "" + "\n";
					stBuffer = stBuffer + "" + "\n";
					stBuffer = stBuffer + "" + "\n";
					stBuffer = stBuffer + "" + "\n";
					stBuffer = stBuffer + "";
				}
			}
			System.IO.StreamWriter sw = (new System.IO.StreamWriter(@"const.ini", false));
			sw.WriteLine(stBuffer);
			sw.Close();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			ChangePassword changePassword = new ChangePassword();
			changePassword.password = password;
			changePassword.ShowDialog();

			if (changePassword.OKFlg)
			{
				this.password = changePassword.password;
				saveData();
			}
		}

		//鼠标拖动，变更顺序用
		int changeOrderStartNum = -1;
		int changeOrderEndNum = -1;
		bool changeOrderFlg = false;
		//鼠标拖动，变更顺序用
		private void listView_View_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			changeOrderStartNum = this.listView_View.SelectedIndex;
			changeOrderFlg = true;
		}

		//鼠标拖动，变更顺序用
		private void listView_View_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			changeOrderFlg = false;
		}
		//鼠标拖动，变更顺序用
		private void listView_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (changeOrderFlg  //拖动中
			&& this.listView_View.SelectedIndex != -1 //且有被选中
			&& changeOrderStartNum != -1 //且起始被选择块有
			&& changeOrderStartNum != this.listView_View.SelectedIndex) //且起始被选择块 和 当前选择块不一致
			{
				changeOrderEndNum = this.listView_View.SelectedIndex;

				//交换
				BookMarkBean bookMarkBeanTemp = bookMarkList[changeOrderStartNum];
				bookMarkList[changeOrderStartNum] = bookMarkList[changeOrderEndNum];
				bookMarkList[changeOrderEndNum] = bookMarkBeanTemp;

				changeOrderStartNum = changeOrderEndNum;

				listView_View.SelectedIndex = changeOrderEndNum;

				changeFlg = true;
			}
		}

		private Boolean isPasswordOK()
		{
			if (loginOK == false)
			{
				safetyPassword sPassword = new safetyPassword();
				sPassword.password = password;
				sPassword.ShowDialog();

				if (!sPassword.OKFlg)
				{
					return false;
				}
				else
				{
					loginOK = true;
					return true;
				}
			}
			else
			{
				return true;
			}
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Width = 900;

			listView_View.Height = this.ActualHeight - 130;
			listView_Edit.Height = this.ActualHeight - 130;
		}

        //重新设置右键菜单
        private ContextMenu ReContextMenu(Collection<string> list)
        {
            ContextMenu cm = new ContextMenu();
            //ContextMenu style = (ContextMenu)this.FindResource("ContextMenu");
            //cm.Style _ style.Style;

            MenuItem menu = new MenuItem();
            //menu.style = (Style)this.FindResource("aaaItem");
            menu.Header = "移动到";
            //menu.Click = dc.Value;
            cm.Items.Add(menu);

            foreach (string dc in list)
            {
                MenuItem menuSub = new MenuItem();
                menuSub.Header = dc;
                menuSub.Click += MoveTo_Click;
                menu.Items.Add(menuSub);
            }

            return cm;
        }

        //右键的移动到 别的书签点击后
        private void MoveTo_Click(Object sender, RoutedEventArgs e)
        {
            if(((MenuItem)sender).Header.ToString() == comboBox.Text)
            {
                //移动到当前书签，直接无视
                return;
            }

            Collection<BookMarkBean> nowSelectedItems = new Collection<BookMarkBean>();
            foreach (BookMarkBean beanTemp in listView_View.SelectedItems) {
                //移动到书签类中添加
                dic[((MenuItem)sender).Header.ToString()].Add(beanTemp);

                nowSelectedItems.Add(beanTemp);
                //当前书签的删除不可以在这里。循环会报异常

                changeFlg = true;
            }

            foreach(BookMarkBean beanTemp in nowSelectedItems)
            {
                //当前书签类中删除
                bookMarkList.Remove(beanTemp);
            }
            

        }
	}
}
