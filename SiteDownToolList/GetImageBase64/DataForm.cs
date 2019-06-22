using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetImageBase64
{
	class DataForm : INotifyPropertyChanged
	{
		private String _ImgFile;
		private String _Base64;

		public String ImgFile
		{
			get
			{
				return _ImgFile;
			}
			set
			{
				_ImgFile = value;
				OnPropertyChanged("ImgFile");
			}
		}
		public String Base64
		{
			get
			{
				return _Base64;
			}
			set
			{
				_Base64 = value;
				OnPropertyChanged("Base64");
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
