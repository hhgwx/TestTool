using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ForAll
{
	class RegeditMng
	{
		private RegistryKey myReg = null;
		public RegeditMng(string parToolName) {
			RegistryKey regRoot = Registry.CurrentUser.CreateSubKey("Software\\GuanwxTool");
			regRoot.CreateSubKey(parToolName);
			myReg = regRoot.OpenSubKey(parToolName, true);
		}

		public string getRegValue(string regCol) {
			string retValue = "";
			try
			{
				retValue = myReg.GetValue(regCol).ToString();
			} catch (Exception) {
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

		public void setRegValue(string regCol,string regValue)
		{
			myReg.SetValue(regCol, regValue);
		}

		public void CloseReg()
		{
			if (myReg != null) {
				myReg.Close();
			}
		}
	}
}
