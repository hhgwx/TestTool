using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64ChangeSimpleVer
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FileName;
		private Boolean _TxtToBase64;
		private Boolean _Base64ToTxt;
		private String _ResultMsg;

		public String FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				_FileName = value;
				OnPropertyChanged("FileName");
			}
		}
		public Boolean TxtToBase64
		{
			get
			{
				return _TxtToBase64;
			}
			set
			{
				_TxtToBase64 = value;
				OnPropertyChanged("TxtToBase64");
			}
		}

		public Boolean Base64ToTxt
		{
			get
			{
				return _Base64ToTxt;
			}
			set
			{
				_Base64ToTxt = value;
				OnPropertyChanged("Base64ToTxt");
			}
		}

		public String ResultMsg
		{
			get
			{
				return _ResultMsg;
			}
			set
			{
				_ResultMsg = value;
				OnPropertyChanged("ResultMsg");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
