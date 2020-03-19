using ForAll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ReNameTool
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ToolName = "ReNameTool";

		DataForm dataForm = new DataForm();
		public MainWindow()
		{
			dataForm.CheckFile = true;
			dataForm.CheckFolder = false;
			dataForm.CheckSubFolder = true;
			dataForm.TextPath = "";
			InitializeComponent();

			this.DataContext = dataForm;

			initData();
		}

		private void initData()
		{
			Dictionary<string, string> type_list = new Dictionary<string, string>() {
				{"0","字符串替换"},
				{"1","文件名替换为作成时间"},
				{"2","文件名替换为更新时间"}
			};

			//浏览器区块设置
			this.replaceType.ItemsSource = type_list;
			this.replaceType.SelectedValuePath = "Key";
			this.replaceType.DisplayMemberPath = "Value";

			RegeditMng myReg = new RegeditMng(ToolName);
			if (ReNameTool.Program.inputArgu != "")
			{
				dataForm.TextPath = ReNameTool.Program.inputArgu;
			}
			else
			{
				dataForm.TextPath = myReg.getRegValue("TextPath");
			}
			dataForm.CheckFolder = myReg.getRegBoolValue("CheckFolder");
			dataForm.CheckFile = myReg.getRegBoolValue("CheckFile");
			dataForm.CheckSubFolder = myReg.getRegBoolValue("CheckSubFolder");
			dataForm.TextFrom = myReg.getRegValue("TextFrom");
			//this.replaceType.SelectedValue = myReg.getRegValue("ReplaceType");
			dataForm.ReplaceType = myReg.getRegValue("ReplaceType");
			dataForm.setTextToArr(0, myReg.getRegValue("TextTo0"));
			dataForm.setTextToArr(1, myReg.getRegValue("TextTo1"));
			dataForm.setTextToArr(2, myReg.getRegValue("TextTo2"));
			if (dataForm.ReplaceType == "") {
				dataForm.ReplaceType = "0";
			}
			if (dataForm.getTextToArr(1) == "")
			{
				dataForm.setTextToArr(1, "YYYYMMDD_HHMISS");
			}
			if (dataForm.getTextToArr(2) == "")
			{
				dataForm.setTextToArr(2, "YYYYMMDD_HHMISS");
			}
			dataForm.TextTo = dataForm.getTextToArr(int.Parse(dataForm.ReplaceType));

			//check右键菜单
			if (myReg.leftMenuChk(ToolName))
			{
				this.button_regedit.Content = "右菜单删除";
			}
			myReg.CloseReg();


		}
		private void button_reset_Click(object sender, RoutedEventArgs e)
		{
			dataForm.TextPath = "";
			dataForm.CheckFile = true;
			dataForm.CheckFolder = false;
			dataForm.CheckSubFolder = true;
			dataForm.TextFrom = "";
			dataForm.ReplaceType = "0";
			dataForm.TextTo = "";
			dataForm.setTextToArr(0,"");
			dataForm.setTextToArr(1, "YYYYMMDD_HHMISS");
			dataForm.setTextToArr(2, "YYYYMMDD_HHMISS");
		}

		private void button_replace_Click(object sender, RoutedEventArgs e)
		{
			if (!Directory.Exists(dataForm.TextPath))
			{
				MessageBox.Show("处理对象[" + dataForm.TextPath + "]文件夹不存在");
				return;
			}



			dataForm.setTextToArr(int.Parse(dataForm.ReplaceType), dataForm.TextTo);

			changeName(dataForm.TextPath, dataForm.CheckFolder, dataForm.CheckFile, dataForm.CheckSubFolder);

			MessageBox.Show("替换完成");
		}

		private void changeName(String path, Boolean folderFlg, Boolean fileFlg, Boolean subFlg)
		{
			if (!path.EndsWith(@"\") && !path.EndsWith(@"/"))
			{
				path = path + @"\";
			}

			string[] fileList = null;
			string nameFrom = "";
			string nameTo = "";

			string nameMain; //文件名主体
			string nameSuff; //文件名后缀

			string replaceToStrEdit = ""; //编辑后替换用文件名

			if (fileFlg)
			{
				fileList = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
				foreach (String file in fileList)
				{
					nameFrom = file.Replace(path, "");
					nameTo = nameFrom;

					nameMain = nameFrom.Substring(0, nameFrom.LastIndexOf("."));
					nameSuff = nameFrom.Substring(nameFrom.LastIndexOf("."));

					string[] textFromArr = dataForm.TextFrom.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					for (int i = 0; i < textFromArr.Length; i++)
					{
						if (dataForm.ReplaceType == "0")
						{
							//字符串替换
							replaceToStrEdit = dataForm.TextTo;
						}
						else
						{
							FileInfo fi = new FileInfo(file);
							if (dataForm.ReplaceType == "1")
							{
								//作成时间
								DateTime time = fi.CreationTime;
								replaceToStrEdit = dataForm.TextTo;
								replaceToStrEdit = replaceToStrEdit.Replace("YYYY", time.Year+"");
								replaceToStrEdit = replaceToStrEdit.Replace("MM", (time.Month + "").PadLeft(2,'0'));
								replaceToStrEdit = replaceToStrEdit.Replace("DD", (time.Day + "").PadLeft(2, '0'));

								replaceToStrEdit = replaceToStrEdit.Replace("HH", (time.Hour + "").PadLeft(2, '0'));
								replaceToStrEdit = replaceToStrEdit.Replace("MI", (time.Minute + "").PadLeft(2, '0'));
								replaceToStrEdit = replaceToStrEdit.Replace("SS", (time.Second + "").PadLeft(2, '0'));
							}
							else if (dataForm.ReplaceType == "2")
							{
								//更新时间
								DateTime time = fi.LastWriteTime;
								replaceToStrEdit = dataForm.TextTo;
								replaceToStrEdit = replaceToStrEdit.Replace("YYYY", time.Year + "");
								replaceToStrEdit = replaceToStrEdit.Replace("MM", (time.Month + "").PadLeft(2, '0'));
								replaceToStrEdit = replaceToStrEdit.Replace("DD", (time.Day + "").PadLeft(2, '0'));

								replaceToStrEdit = replaceToStrEdit.Replace("HH", (time.Hour + "").PadLeft(2, '0'));
								replaceToStrEdit = replaceToStrEdit.Replace("MI", (time.Minute + "").PadLeft(2, '0'));
								replaceToStrEdit = replaceToStrEdit.Replace("SS", (time.Second + "").PadLeft(2, '0'));
							}
						}

						//nameTo文件名设置
						if (textFromArr[i] == null || textFromArr[i] == "")
						{
							nameTo = replaceToStrEdit + nameSuff;
						}
						else
						{
							nameTo = nameTo.Replace(textFromArr[i], replaceToStrEdit);
						}
					}

					if (nameFrom != nameTo)
					{
						try
						{
							File.Move(path + nameFrom, path + nameTo);
						}
						catch (Exception)
						{
							//MessageBox.Show(path + nameTo + " 已经存在。\n" + path + nameFrom + " 的名字替换失败。");

							int index = 2;
							while (File.Exists(path + replaceToStrEdit + "_" + index + nameSuff))
							{
								index++;
							}
							File.Move(path + nameFrom, path + replaceToStrEdit + "_" + index + nameSuff);
						}
					}
				}
			}
			if (folderFlg && dataForm.ReplaceType == "0")
			{
				fileList = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
				foreach (String file in fileList)
				{
					nameFrom = file.Replace(path, "");
					nameTo = nameFrom;

					string[] textFromArr = dataForm.TextFrom.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					for (int i = 0; i < textFromArr.Length; i++)
					{
						if (textFromArr[i] != null && textFromArr[i] != "")
						{
							nameTo = nameTo.Replace(textFromArr[i], dataForm.TextTo);
						}
					}

					if (nameFrom != nameTo)
					{
						try
						{
							Directory.Move(path + nameFrom, path + nameTo);
						}
						catch (Exception)
						{
						}
					}
				}
			}
			if (subFlg)
			{
				fileList = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
				foreach (String file in fileList)
				{
					changeName(file, folderFlg, fileFlg, subFlg);
				}
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			RegeditMng myReg = new RegeditMng(ToolName);
			myReg.setRegValue("TextPath", dataForm.TextPath);
			myReg.setRegValue("CheckFolder", dataForm.CheckFolder.ToString());
			myReg.setRegValue("CheckFile", dataForm.CheckFile.ToString());
			myReg.setRegValue("CheckSubFolder", dataForm.CheckSubFolder.ToString());
			myReg.setRegValue("TextFrom", dataForm.TextFrom.ToString());
			myReg.setRegValue("ReplaceType", dataForm.ReplaceType.ToString());
			myReg.setRegValue("TextTo0", dataForm.getTextToArr(0));
			myReg.setRegValue("TextTo1", dataForm.getTextToArr(1));
			myReg.setRegValue("TextTo2", dataForm.getTextToArr(2));
			myReg.CloseReg();
		}

		private void replaceType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			dataForm.TextTo = dataForm.getTextToArr(int.Parse(dataForm.ReplaceType));
		}

		private void button_regedit_Click(object sender, RoutedEventArgs e)
		{
			RegeditMng myReg = new RegeditMng();

			if (this.button_regedit.Content.ToString() == "右菜单添加")
			{
				if (myReg.leftMenuAdd(ToolName, Process.GetCurrentProcess().MainModule.FileName))
				{
					MessageBox.Show("注册成功!", "提示");
					this.button_regedit.Content = "右菜单删除";
				}
				else
				{
					MessageBox.Show("注册失败!", "提示");
				}
			}
			else
			{
				if (myReg.leftMenuDel(ToolName))
				{
					MessageBox.Show("反注册成功!", "提示");
					this.button_regedit.Content = "右菜单添加";
				}
				else
				{
					MessageBox.Show("反注册失败!", "提示");
				}
			}
		}
	}
}