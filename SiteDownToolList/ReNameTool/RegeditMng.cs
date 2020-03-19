using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

namespace ForAll
{
	class RegeditMng
	{
		private RegistryKey myReg = null;
		public RegeditMng(string parToolName)
		{
			RegistryKey regRoot = Registry.CurrentUser.CreateSubKey("Software\\GuanwxTool");
			regRoot.CreateSubKey(parToolName);
			myReg = regRoot.OpenSubKey(parToolName, true);
		}
		public RegeditMng()
		{
		}

		public string getRegValue(string regCol)
		{
			string retValue = "";
			try
			{
				retValue = myReg.GetValue(regCol).ToString();
			}
			catch (Exception)
			{
				retValue = "";
			}
			return retValue;
		}

		public Boolean getRegBoolValue(string regCol)
		{
			Boolean retValue = false;
			try
			{
				retValue = Convert.ToBoolean(myReg.GetValue(regCol).ToString());
			}
			catch (Exception)
			{
				retValue = false;
			}
			return retValue;
		}

		public void setRegValue(string regCol, string regValue)
		{
			myReg.SetValue(regCol, regValue);
		}

		public void CloseReg()
		{
			if (myReg != null)
			{
				myReg.Close();
			}
		}

		//添加右键菜单
		// 添加到注册表
		public bool leftMenuAdd(string parToolName , string path)
		{
			try
			{
				RegistryKey shell = Registry.ClassesRoot.OpenSubKey("directory", true).OpenSubKey("background", true).OpenSubKey("shell", true);
				if (shell == null) shell = Registry.ClassesRoot.OpenSubKey("directory", true).OpenSubKey("background", true).CreateSubKey("shell");
				RegistryKey custome = shell.CreateSubKey(parToolName);
				RegistryKey cmd = custome.CreateSubKey("command");
				//cmd.SetValue("", path + " %1");
				cmd.SetValue("", "\"" + path + "\" \"%V\"");
				cmd.Close();
				custome.Close();
				shell.Close();
			} catch (Exception) {
				return false;
			} 
			return true;
			//MessageBox.Show("注册成功!", "提示");
		}

		// 反注册
		public bool leftMenuDel(string parToolName)
		{
			try
			{
				RegistryKey shell = Registry.ClassesRoot.OpenSubKey("directory", true).OpenSubKey("background", true).OpenSubKey("shell", true);
				if (shell != null) shell.DeleteSubKeyTree(parToolName);

				shell.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
			return true;
		}

		// check
		public bool leftMenuChk(string parToolName)
		{
			bool returnVal;
			try
			{
				RegistryKey shell = Registry.ClassesRoot.OpenSubKey("directory", true).OpenSubKey("background", true).OpenSubKey("shell", true).OpenSubKey(parToolName, true);
				if (shell != null)
				{
					returnVal = true;
				}
				else
				{
					
					returnVal = false;
				}
				shell.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return returnVal;
		}
	}
}