using ForAll;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;

namespace ReplaceColor
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ToolName = "ReplaceColor";

		DataForm dataForm = new DataForm();
		ObservableCollection<ListBean> dataList = new ObservableCollection<ListBean>();
		static int threadNum = 4;
		string okStr = "";
		string[] okStrArr = new string[threadNum];
		int startXPoint = 210; //最右下的像素距离
		int startYPoint = 55; //最右下的像素距离
		int blackColor = 10; //RGB相差不超过这个值 被认为是黑白片

		string[] logFile = new string[threadNum];

		Thread[] downThread = new Thread[threadNum];
		Boolean[] threadEndFlg = new Boolean[threadNum];
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
			RegeditMng myReg = new RegeditMng(ToolName);
			dataForm.FromFolder = myReg.getRegValue("FromFolder");
			dataForm.ImgType1 = myReg.getRegBoolValue("ImgType1");
			dataForm.ImgType2 = myReg.getRegBoolValue("ImgType2");
			dataForm.ImgType3 = myReg.getRegBoolValue("ImgType3");
			dataForm.SubFolder = myReg.getRegBoolValue("SubFolder");
			dataForm.ReMake = myReg.getRegBoolValue("ReMake");
			dataForm.OutFolder = myReg.getRegValue("OutFolder");
			dataForm.BlackWhiteRadio = myReg.getRegBoolValue("BlackWhiteRadio");
			dataForm.ColorRadio = myReg.getRegBoolValue("ColorRadio");
			dataForm.ReplaceColor = myReg.getRegValue("ReplaceColor");
			myReg.CloseReg();

            text_replaceColor.IsEnabled = true;
 /*           if (dataForm.BlackWhiteRadio)
			{
				text_replaceColor.IsEnabled = false;
			}
			else if (dataForm.ColorRadio)
			{
				text_replaceColor.IsEnabled = true;
			}
			else
			{
				dataForm.BlackWhiteRadio = true;
				text_replaceColor.IsEnabled = false;
			}*/
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

			if (oldFile != dataForm.FromFolder)
			{
				/*	if (dataList != null && dataList.Count > 0)
					{
						saveToFile(dataForm.FromFolder);
						dataList.Clear();
					}
					readFile(oldFile);*/
			}
			else if (dataList == null || dataList.Count == 0)
			{
				//readFile(oldFile);
			}
		}

		private void getFileList(String folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				MessageBox.Show("文件夹不存在");
				return;
			}

			int i = 1;
			if (dataForm.SubFolder)
			{
				string[] folderList = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
				foreach (string folder in folderList)
				{
					ListBean listBeanTemp = new ListBean();
					listBeanTemp.No = i;
					listBeanTemp.Name = folder;
					dataList.Add(listBeanTemp);
					i++;
				}
			}

			string[] fileList = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
			foreach (string file in fileList)
			{
				if (isRightImg(file) != null)
				{
					ListBean listBeanTemp = new ListBean();
					listBeanTemp.No = i;
					listBeanTemp.Name = file;
					dataList.Add(listBeanTemp);
					i++;
				}
			}
		}

		private void button_start_Click(object sender, RoutedEventArgs e)
		{
			if ((string)button_start.Content == "开始处理")
			{
				if (dataForm.FromFolder.Trim() == "" || dataForm.OutFolder.Trim() == "" || (dataForm.ReplaceColor.Trim() == "" && dataForm.ColorRadio))
				{
					MessageBox.Show("未入力");
					return;
				}
				if (!dataForm.ImgType1 && !dataForm.ImgType2 && !dataForm.ImgType3)
				{
					MessageBox.Show("处理对象类别未选择");
					return;
				}
				dataForm.FromFolder = (dataForm.FromFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
				dataForm.OutFolder = (dataForm.OutFolder.Trim() + "/").Replace(@"\", "/").Replace("//", "/").Replace("//", "/");
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

				if (!System.IO.Directory.Exists(dataForm.OutFolder))
				{
					Directory.CreateDirectory(dataForm.OutFolder);
				}

				button_start.Content = "停止处理";
				dataForm.ReplaceColor = dataForm.ReplaceColor.ToUpper();

				RegeditMng myReg = new RegeditMng(ToolName);
				myReg.setRegValue("FromFolder", dataForm.FromFolder.ToString());
				myReg.setRegValue("ImgType1", dataForm.ImgType1.ToString());
				myReg.setRegValue("ImgType2", dataForm.ImgType2.ToString());
				myReg.setRegValue("ImgType3", dataForm.ImgType3.ToString());
				myReg.setRegValue("SubFolder", dataForm.SubFolder.ToString());
				myReg.setRegValue("ReMake", dataForm.ReMake.ToString());
				myReg.setRegValue("OutFolder", dataForm.OutFolder.ToString());
				myReg.setRegValue("BlackWhiteRadio", dataForm.BlackWhiteRadio.ToString());
				myReg.setRegValue("ColorRadio", dataForm.ColorRadio.ToString());
				myReg.setRegValue("ReplaceColor", dataForm.ReplaceColor.ToString());
				myReg.CloseReg();

				//get ok Str
				readOKFile(dataForm.OutFolder + "replaceOkList.txt");
				if (okStr == null || okStr.Length == 0) {
					okStr = "|";
				}

				for (int i = 0; i < downThread.Length; i++)
				{
					logFile[i] = dataForm.OutFolder + "log_" + i + ".txt";
					okStrArr[i] = "|";
					writeLog(logFile[i], "Start");
					downThread[i] = new Thread(new ParameterizedThreadStart(replaceThread));
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

		private void readOKFile(String filePath)
		{
			if (!System.IO.File.Exists(filePath))
			{
				return;
			}

			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
			StreamReader read = new StreamReader(fs);
			read.BaseStream.Seek(0, SeekOrigin.Begin);

			try
			{
				while (read.Peek() > -1)
				{
					okStr = okStr + read.ReadLine();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				fs.Flush();
				fs.Close();
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
				else
				{
					writeLog(logFile[i], "Start");
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
                try { 
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

					    toPath = listBeanTemp.Name.Replace(dataForm.FromFolder, dataForm.OutFolder);

/*					    if (okStr.IndexOf("|" + listBeanTemp.Name + "|") >= 0 && (Directory.Exists(listBeanTemp.Name) || File.Exists(listBeanTemp.Name)))
					    {
						    listBeanTemp.Result = "OK";
						    continue;
					    }*/
					    replaceImg(listBeanTemp.Name, toPath, System.Drawing.Imaging.ImageFormat.Jpeg, indexInt);

					    listBeanTemp.Result = "OK";
					    //okStrArr[indexInt] += okStrArr[indexInt] + listBeanTemp.Name + "|";
				    }
                } catch(Exception e)
                {
                    writeLog(logFile[indexInt], "error:" + e.Message);
                    threadEndFlg[indexInt] = true;
                    return;
                }

            }
			threadEndFlg[indexInt] = true;
		}
		private void replaceImg(string fromPath, string toPath, System.Drawing.Imaging.ImageFormat imgType, int indexInt)
		{
			if (Directory.Exists(fromPath))
			{
				fromPath = fromPath + "/";
				toPath = toPath + "/";
				if (!Directory.Exists(toPath))
				{
					Directory.CreateDirectory(toPath);
				}
				string[] folderList = Directory.GetDirectories(fromPath, "*", SearchOption.TopDirectoryOnly);
				foreach (string folder in folderList)
				{
					string toPathTemp = folder.Replace(fromPath, toPath);
					replaceImg(folder, toPathTemp, imgType, indexInt);
				}

				string[] fileList = Directory.GetFiles(fromPath, "*.*", SearchOption.TopDirectoryOnly);
				foreach (string file in fileList)
				{
					System.Drawing.Imaging.ImageFormat imgTypeTemp = isRightImg(file);
					if (imgTypeTemp != null)
					{
						string toPathTemp = file.Replace(fromPath, toPath);
						replaceImg(file, toPathTemp, imgTypeTemp, indexInt);
					}
				}
			}
			else if (File.Exists(fromPath))
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

				try
				{
					//test１
					/*// 打开文件  
					FileStream fileStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read, FileShare.Read);
					// 读取文件的 byte[]  
					byte[] bytes = new byte[fileStream.Length];
					fileStream.Read(bytes, 0, bytes.Length);
					fileStream.Close();


					FileStream fs = new System.IO.FileStream(toPath, System.IO.FileMode.Create);
					fs.Write(bytes, 0, bytes.Length);*/

					//test2
					Bitmap bitMap = new Bitmap(fromPath);

					//string testStr = "";
					System.Drawing.Color pixelColor;
					System.Drawing.Color changeToPixelColor = System.Drawing.Color.White;
					int startX = bitMap.Width - startXPoint;
					int startY = bitMap.Height - startYPoint;
					if (startX < 0)
					{
						startX = 0;
					}
					if (startY < 0)
					{
						startY = 0;
					}

					Boolean isBlackwhitePic = true;
					if (dataForm.BlackWhiteRadio) {
						//判断是不是黑白图片(通过对角线是否有彩点)
						for (int x = 0, y = 0; x < startX && y < startY; x++,y++)
						{
							pixelColor = bitMap.GetPixel(x, y);
							if (Math.Abs(pixelColor.G - pixelColor.R) > blackColor || Math.Abs(pixelColor.G - pixelColor.B) > blackColor)
							{
								isBlackwhitePic = false;
								break;
							}
						}

						//if (isBlackwhitePic == false) {
						//	writeLog(logFile[indexInt], fromPath + "\t不是黑白图片。");
						//	return;
						//}
					}

					for (int x = startX; x < bitMap.Width; x++)
					{
						//testStr += "\n";
						for (int y = startY; y < bitMap.Height; y++)
						{
							pixelColor = bitMap.GetPixel(x, y);
							if (y == bitMap.Height - startYPoint)
							{
								changeToPixelColor = pixelColor;
							}
							if (dataForm.BlackWhiteRadio && isBlackwhitePic) //黑白图
							{
								if (Math.Abs(pixelColor.G - pixelColor.R) > blackColor || Math.Abs(pixelColor.G - pixelColor.B) > blackColor)
								{
									bitMap.SetPixel(x, y, changeToPixelColor);
								}
							}
                            //else if (dataForm.ReplaceColor.IndexOf(ColorTranslator.ToHtml(pixelColor)) >= 0)
                            else if(dataForm.ColorRadio && !isBlackwhitePic) //彩色图
                            {
                                if (pixelColor.R + blackColor > pixelColor.G && pixelColor.R + blackColor > pixelColor.B)
                                {
                                    bitMap.SetPixel(x, y, changeToPixelColor);
                                }
                            }
                                
                            //TODO getList
                            //if (Math.Abs(pixelColor.G - pixelColor.R) > blackColor || Math.Abs(pixelColor.G - pixelColor.B) > blackColor)
                            //{
                            //    testStr += ColorTranslator.ToHtml(pixelColor);
							//}
						}
					}
					//Console.WriteLine(testStr);
					//bitMap.Save(toPath, System.Drawing.Imaging.ImageFormat.Bmp);// //Bmp清楚。JPEG不清楚
					bitMap.Save(toPath, imgType); //Bmp清楚。JPEG不清楚

				}
				catch (Exception e)
				{
					Console.WriteLine(e.StackTrace);
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

		public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
		{
			if (byteArrayIn == null)
				return null;
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
			{
				System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
				ms.Flush();
				return returnImage;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//save To file
			saveToFile(dataForm.OutFolder + "replaceOkList.txt");
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
			} else 
			{
				File.Create(filePath);
				return;
			}

			FileInfo finfo = new FileInfo(filePath);
			using (FileStream fs = finfo.OpenWrite())
			{
				///根据上面创建的文件流创建写数据流 
				StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));

				///把新的内容写到OKfile 
				for (int i=0;i< okStrArr.Length;i++) {
					if (okStrArr[i].Length > 0) {
						okStr = okStr + "|" + okStrArr[i];
					}
				}
				sw.WriteLine(okStr);
				sw.Flush();
				sw.Close();
			}
		}

		private void writeLog(string filePath, string message)
		{
			if (!System.IO.File.Exists(filePath))
			{
				File.Create(filePath);
				return;
			}

			///根据上面创建的文件流创建写数据流 
			StreamWriter sw = new StreamWriter(filePath, true, System.Text.Encoding.GetEncoding("UTF-8"));

			sw.WriteLine(System.DateTime.Now.ToString() + "\t" + message);
			sw.Flush();
			sw.Close();
		}

		private void radio_Click(object sender, RoutedEventArgs e)
		{
			if (dataForm.BlackWhiteRadio) {
				text_replaceColor.IsEnabled = false;
			} else if (dataForm.ColorRadio) {
				text_replaceColor.IsEnabled = true;
			}
		}
	}
	/*
public static byte[] ImageToBytes(System.Drawing.Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
{

if (Image == null) { return null; }
byte[] data = null;
using (MemoryStream ms = new MemoryStream())
{
	using (Bitmap Bitmap = new Bitmap(Image))
	{
		Bitmap.Save(ms, imageFormat);
		ms.Position = 0;
		data = new byte[ms.Length];
		ms.Read(data, 0, Convert.ToInt32(ms.Length));
		ms.Flush();
	}
}
return data;
}*/

}
