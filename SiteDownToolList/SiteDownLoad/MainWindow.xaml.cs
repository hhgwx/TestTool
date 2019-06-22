using ForAll;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SiteDownLoad
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ToolName = "SiteDownLoad";

		DataForm dataForm = new DataForm();
		ObservableCollection<ListBean> dataList = new ObservableCollection<ListBean>();
		Thread[] downThread = new Thread[10];
        Boolean[] threadEndFlg = new Boolean[10];
        Boolean stopFlag = false;
		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = dataForm;
			this.listView_dataList.ItemsSource = dataList;

			initData();
		}

		private void initData()
		{
			RegeditMng myReg = new RegeditMng(ToolName);
			dataForm.BatFile = myReg.getRegValue("BatFile");
			dataForm.OutFolder = myReg.getRegValue("OutFolder");
			dataForm.Reg = myReg.getRegValue("Reg");
			myReg.CloseReg();
		}

		private void button_selectFile_Click(object sender, RoutedEventArgs e)
		{
			String oldFile = dataForm.BatFile;
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Filter = "文本文件|*.tsv;*.txt|所有文件|*.*";
			openFileDialog.ValidateNames = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.CheckFileExists = true;
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dataForm.BatFile = openFileDialog.FileName;
			}
			if (oldFile != dataForm.BatFile) {
				if (dataList != null && dataList.Count > 0)
				{
					saveToFile(oldFile);
					dataList.Clear();
				}
				readFile(dataForm.BatFile);
			} else if (dataList == null || dataList.Count == 0)
			{
				readFile(dataForm.BatFile);
			}
			
		}
		private void readFile(String filePath)
		{
			if (!System.IO.File.Exists(filePath))
			{
				MessageBox.Show("文件不存在");
				return;
			}

			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
			StreamReader read = new StreamReader(fs);
			read.BaseStream.Seek(0, SeekOrigin.Begin);

			ListBean listBean;
			int i = 1;
			
			while (read.Peek() > -1)
			{
				String[] lineValue = read.ReadLine().Split('\t');
				if (lineValue.Length >= 3 && lineValue[0]!="")
				{
					listBean = new ListBean();
					listBean.No = i;
					listBean.Name = lineValue[0];
					listBean.URLFrom = lineValue[1];
					listBean.PageNo = Convert.ToInt32(lineValue[2]);
					if (lineValue.Length == 3)
					{
						listBean.Result = "";
					}
					else
					{
						listBean.Result = lineValue[3];
					}
					dataList.Add(listBean);
					i++;
				}
			}
		}
		private void button_startDown_Click(object sender, RoutedEventArgs e)
		{
			if ((string)button_startDown.Content == "开始下载")
			{
				if (dataForm.BatFile.Trim() == "" || dataForm.OutFolder.Trim() == "" || dataForm.Reg.Trim() == "")
				{
					MessageBox.Show("未入力");
					return;
				}
				if (dataList == null || dataList.Count == 0)
				{
					readFile(dataForm.BatFile);
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

				button_startDown.Content = "停止下载";
				stopFlag = false;

				RegeditMng myReg = new RegeditMng(ToolName);
				myReg.setRegValue("BatFile", dataForm.BatFile);
				myReg.setRegValue("OutFolder", dataForm.OutFolder);
				myReg.setRegValue("Reg", dataForm.Reg);
				myReg.CloseReg();

				dataForm.OutFolder = (dataForm.OutFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");

				for (int i = 0; i < downThread.Length; i++)
				{
					downThread[i] = new Thread(new ParameterizedThreadStart(downLoadThread));
                    threadEndFlg[i] = false;
                    downThread[i].Start(i);
				}

				Thread stopThread = new Thread(stopThreadStart);
				stopThread.Start();
			}
			else
			{
				button_startDown.Content = "停止中...";
				button_startDown.IsEnabled = false;

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
            button_startDown.Content = "开始下载";
            button_startDown.IsEnabled = true;
        }

        private async void downLoadThread(object index)
		{
            int indexInt = Convert.ToInt32(index);
			string strTemp;
			foreach (ListBean listBeanTemp in dataList)
			{
				if (stopFlag) {
                    threadEndFlg[indexInt] = true;
                    return;
				}

				if (listBeanTemp.Result != "OK" && listBeanTemp.Result != "DO")
				{
					listBeanTemp.Result = "DO";

					if (!System.IO.Directory.Exists(dataForm.OutFolder))
					{
						Directory.CreateDirectory(dataForm.OutFolder);
					}

					String nowFolder = dataForm.OutFolder + listBeanTemp.Name + "/";
					String nowURL = "";
					if (!System.IO.Directory.Exists(nowFolder))
					{
						Directory.CreateDirectory(nowFolder);
					}
					for (int i = 1; i <= listBeanTemp.PageNo; i++)
					{
						if (!System.IO.Directory.Exists(nowFolder + i))
						{
							Directory.CreateDirectory(nowFolder + i);
						}
						if (listBeanTemp.URLFrom.EndsWith("/"))
						{
							nowURL = (listBeanTemp.URLFrom + i); //TODO
						}
						else
						{
							nowURL = (listBeanTemp.URLFrom + "/" + i); //TODO
						}
						strTemp = await GetHttpResponse(nowURL, nowFolder + i + "/",dataForm.Reg);
						if (strTemp != "") {
							listBeanTemp.Result = "ERR";
							//return; dont stop; download next page
						}
					}
					listBeanTemp.Result = "OK";
				}
			}
            threadEndFlg[indexInt] = true;
        }

		public async Task<string> GetHttpResponse (string url, string toPath, string regStr)
		{
			string responseString = "";
			System.IO.FileStream fs = null;
			try
			{

				using (var client = new HttpClient())
				{
					var uri = new Uri(Uri.EscapeUriString(url));
					responseString = await client.GetStringAsync(uri);

					Regex reg = new Regex(regStr);
					MatchCollection match = reg.Matches(responseString);

					foreach (Match matchURL in match)
					{
                        try { 
						    uri = new Uri(Uri.EscapeUriString(matchURL.Value));
						    byte[] urlContents = await client.GetByteArrayAsync(uri);
						    fs = new System.IO.FileStream(toPath + matchURL.Value.Substring(matchURL.Value.LastIndexOf("/") + 1), System.IO.FileMode.Create);
						    fs.Write(urlContents, 0, urlContents.Length);
                        }
                        catch (Exception)
                        {
                        }

                    }
				}
			}
			catch (Exception e)
			{
				return e.Message;
			}
			finally
			{
				if (fs != null) {
					fs.Flush();
					fs.Close();
				}
			}
			return "";
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//save To file
			saveToFile(dataForm.BatFile);

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
					sw.WriteLine(listBeanTemp.Name + "\t" + listBeanTemp.URLFrom + "\t" + listBeanTemp.PageNo  + "\t" + listBeanTemp.Result);
				}
				sw.Flush();
				sw.Close();
			}
		}
	}
}