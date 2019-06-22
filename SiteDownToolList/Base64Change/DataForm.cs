using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64Change
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FileName;
		private Boolean _TxtToBase64;
		private Boolean _ZipToBase64;
		private Boolean _Base64ToTxt;
		private Boolean _Base64ToZip;
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
		public Boolean ZipToBase64
		{
			get
			{
				return _ZipToBase64;
			}
			set
			{
				_ZipToBase64 = value;
				OnPropertyChanged("ZipToBase64");
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
		public Boolean Base64ToZip
		{
			get
			{
				return _Base64ToZip;
			}
			set
			{
				_Base64ToZip = value;
				OnPropertyChanged("Base64ToZip");
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
