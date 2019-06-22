using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownLoad
{
	class DataForm : INotifyPropertyChanged
	{
		private String _BatFile;
		private String _OutFolder;
		private String _Reg;

		public String BatFile
		{
			get
			{
				return _BatFile;
			}
			set
			{
				_BatFile = value;
				OnPropertyChanged("BatFile");
			}
		}
		public String OutFolder
		{
			get
			{
				return _OutFolder;
			}
			set
			{
				_OutFolder = value;
				OnPropertyChanged("OutFolder");
			}
		}

		public String Reg
		{
			get
			{
				return _Reg;
			}
			set
			{
				_Reg = value;
				OnPropertyChanged("Reg");
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
