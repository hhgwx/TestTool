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

                } else
                {
                    switch ((lineNum - 1) % 6) {
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
            //初期选择第一个
            classSelectChange(classList.IndexOf(ConfigurationManager.AppSettings["defaultClass"]));

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
                safetyPassword sPassword = new safetyPassword();
                sPassword.password = password;
                sPassword.ShowDialog();

                if (!sPassword.OKFlg)
                {
                    return;
                }

                btn_EditBookMark.Content = "保存书签";
                listView_View.Visibility = Visibility.Hidden;
                listView_Edit.Visibility = Visibility.Visible;
            } else
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
            else if (classList.Contains(newClass))
            {
                MessageBox.Show("已存在的分类名，请重新输入");
                this.textBox.Focus();
                return;
            }

            ObservableCollection<BookMarkBean> bookMarkListTemp = new ObservableCollection<BookMarkBean>();
            classList.Add(newClass);
            dic.Add(newClass, bookMarkListTemp);

            classSelectChange(classList.IndexOf(newClass));

            textBox.Text = "";
            changeFlg = true;
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

            int indexTemp = comboBox.SelectedIndex;
            classList[comboBox.SelectedIndex] = newClassName;
            comboBox.SelectedIndex = indexTemp;

            KeyValuePair<string, ObservableCollection<BookMarkBean>> dicTemp = new KeyValuePair<string, ObservableCollection<BookMarkBean>>(newClassName, dic[oldClassName]);
            dic = dic.ToDictionary(k => k.Key == oldClassName ? newClassName : k.Key, k => k.Value); //修改DIC值

            changeFlg = true;
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

                classSelectChange(0);//重新选择第一项

                changeFlg = true;
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
                    try {
                        no = int.Parse(bookMarkList[bookMarkList.Count - 1].No);
                    }
                    catch (Exception)
                    {
                        bookMarkList.Clear();
                    }
                    bookMarkBeanTemp.No = no + 1 + "";
                } else
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
            if (dic.Count > 0) {
                bookMarkList = dic[comboBox.Text];
                listView_View.ItemsSource = bookMarkList;
                listView_Edit.ItemsSource = bookMarkList;
            }
        }

        //URL跳转按钮按下
        private void btn_LineRun_Click(object sender, RoutedEventArgs e)
        {
            string url = (sender as Button).Tag.ToString();

            //URL中是否有参数，有的话，替换
            if (url.IndexOf("$") > 0)
            {
                url = url.Replace("$1",this.textBox_parm1.Text);
                url = url.Replace("$2", this.textBox_parm2.Text);
                url =  url.Replace("$3", this.textBox_parm3.Text);
            }

            string browser = this.comboBox_browser.Text;
            if (browser == "CMD")
            {
                //MessageBox.Show(runCmd(url));
                Open(url);
            } else { 
                try {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
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

        public void classSelectChange(int index)
        {
            if (index < 0)
            {
                index = 0;
            }

            if (dic.Count > index)//重新选择第一项
            {
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
                && changeOrderStartNum!=-1 //且起始被选择块有
                && changeOrderStartNum!= this.listView_View.SelectedIndex) //且起始被选择块 和 当前选择块不一致
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

    }
}
