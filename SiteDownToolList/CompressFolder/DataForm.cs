using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressFolder
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FromFolder;
		private Boolean _ReMake;
		private Boolean _Password;
		private String _ToFolder;

		public String FromFolder
		{
			get
			{
				return _FromFolder;
			}
			set
			{
				_FromFolder = value;
				OnPropertyChanged("FromFolder");
			}
		}
		public Boolean ReMake
		{
			get
			{
				return _ReMake;
			}
			set
			{
				_ReMake = value;
				OnPropertyChanged("ReMake");
			}
		}
		public Boolean Password
		{
			get
			{
				return _Password;
			}
			set
			{
				_Password = value;
				OnPropertyChanged("Password");
			}
		}
		public String ToFolder
		{
			get
			{
				return _ToFolder;
			}
			set
			{
				_ToFolder = value;
				OnPropertyChanged("ToFolder");
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
