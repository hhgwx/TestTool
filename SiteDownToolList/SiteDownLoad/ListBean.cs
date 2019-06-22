using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownLoad
{
	class ListBean : INotifyPropertyChanged
	{
		private int _No;
		private String _Name;
		private String _URLFrom;
		private int _PageNo;
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
		public String Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
				OnPropertyChanged("Name");
			}
		}

		public String URLFrom
		{
			get
			{
				return _URLFrom;
			}
			set
			{
				_URLFrom = value;
				OnPropertyChanged("URLFrom");
			}
		}
		public int PageNo
		{
			get
			{
				return _PageNo;
			}
			set
			{
				_PageNo = value;
				OnPropertyChanged("PageNo");
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
