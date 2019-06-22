using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllImgToBlank
{
	class ListBean : INotifyPropertyChanged
	{
		private int _No;
		private String _Name;
		private System.Drawing.Imaging.ImageFormat _ImageFormat;
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
		public System.Drawing.Imaging.ImageFormat ImageFormat
		{
			get
			{
				return _ImageFormat;
			}
			set
			{
				_ImageFormat = value;
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
