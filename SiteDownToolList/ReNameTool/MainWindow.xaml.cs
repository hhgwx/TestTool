using ForAll;
using System;
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
			dataForm.CheckFolder = true;
			dataForm.CheckFile = true;
			dataForm.CheckSubFolder = true;
			dataForm.TextPath = "";
			InitializeComponent();

			this.DataContext = dataForm;

			initData();
		}

		private void initData()
		{
			RegeditMng myReg = new RegeditMng(ToolName);
			dataForm.TextPath = myReg.getRegValue("TextPath");
			dataForm.CheckFolder = myReg.getRegBoolValue("CheckFolder");
			dataForm.CheckFile = myReg.getRegBoolValue("CheckFile");
			dataForm.CheckSubFolder = myReg.getRegBoolValue("CheckSubFolder");
			dataForm.TextFrom = myReg.getRegValue("TextFrom");
			dataForm.TextTo = myReg.getRegValue("TextTo");
			myReg.CloseReg();
		}
		private void button_reset_Click(object sender, RoutedEventArgs e)
		{
			dataForm.TextPath = "";
			dataForm.CheckFolder = false;
			dataForm.CheckFile = false;
			dataForm.CheckSubFolder = false;
			dataForm.TextFrom = "";
			dataForm.TextTo = "";
		}

		private void button_replace_Click(object sender, RoutedEventArgs e)
		{
			if (!Directory.Exists(dataForm.TextPath))
			{
				MessageBox.Show("处理对象[" + dataForm.TextPath + "]文件夹不存在");
				return;
			}
			if (dataForm.TextFrom == "")
			{
				MessageBox.Show("要替换文字为空");
				return;
			}

			changeName(dataForm.TextPath, dataForm.CheckFolder, dataForm.CheckFile, dataForm.CheckSubFolder);

			MessageBox.Show("替换完成");
		}

		private void changeName(String path,Boolean folderFlg,Boolean fileFlg,Boolean subFlg)
		{
			string[] fileList = null;
			string nameFrom = "";
			string nameTo = "";
			if (fileFlg) {
				fileList = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
				foreach (String file in fileList)
				{
					nameFrom = file.Replace(path, "");
					nameTo = nameFrom;

					string[] textFromArr = dataForm.TextFrom.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					for (int i=0;i< textFromArr.Length;i++) {
						if (textFromArr[i] != null && textFromArr[i] != "") {
							nameTo = nameTo.Replace(textFromArr[i], dataForm.TextTo);
						}
					}

					if (nameFrom != nameTo)
					{
						try{
							File.Move(path + nameFrom, path + nameTo);
						} catch (Exception) {
						}
					}
				}
			}
			if (folderFlg)
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
					
					if (nameFrom != nameTo) {
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
			if (subFlg) {
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
			myReg.setRegValue("TextTo", dataForm.TextTo.ToString());
			myReg.CloseReg();
		}
	}
}
