using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressFolder
{
	class ListBean : INotifyPropertyChanged
	{
		private int _No;
		private String _FromName;
		private String _FromNameWithPath;
		private String _ToName;
		private String _ToNameWithPath;
		private String _Password;
		private String _Result;

		public int No
		{
			get
			{
				return _No;
			}
			set
			{
				_No = value;
				OnPropertyChanged("No");
			}
		}
		public String FromName
		{
			get
			{
				return _FromName;
			}
			set
			{
				_FromName = value;
				OnPropertyChanged("FromName");
			}
		}
		public String FromNameWithPath
		{
			get
			{
				return _FromNameWithPath;
			}
			set
			{
				_FromNameWithPath = value;
			}
		}
		public String ToName
		{
			get
			{
				return _ToName;
			}
			set
			{
				_ToName = value;
				OnPropertyChanged("ToName");
			}
		}
		public String ToNameWithPath
		{
			get
			{
				return _ToNameWithPath;
			}
			set
			{
				_ToNameWithPath = value;
				OnPropertyChanged("ToNameWithPath");
			}
		}
		public String Password
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
		public String Result
		{
			get
			{
				return _Result;
			}
			set
			{
				_Result = value;
				OnPropertyChanged("Result");
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
