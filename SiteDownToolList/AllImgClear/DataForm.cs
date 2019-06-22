using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllImgToBlank
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FromFolder;
		private Boolean _ReMake;
		private Boolean _SubFolder;
		private String _BackColor;
		private String _BolderColor;
		private String _BolderWidth;
		private Boolean _ImgType1;
		private Boolean _ImgType2;
		private Boolean _ImgType3;
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
		public Boolean SubFolder
		{
			get
			{
				return _SubFolder;
			}
			set
			{
				_SubFolder = value;
				OnPropertyChanged("SubFolder");
			}
		}
		public String BackColor
		{
			get
			{
				return _BackColor;
			}
			set
			{
				_BackColor = value;
				OnPropertyChanged("BackColor");
			}
		}
		public String BolderColor
		{
			get
			{
				return _BolderColor;
			}
			set
			{
				_BolderColor = value;
				OnPropertyChanged("BolderColor");
			}
		}
		public String BolderWidth
		{
			get
			{
				return _BolderWidth;
			}
			set
			{
				_BolderWidth = value;
				OnPropertyChanged("BolderWidth");
			}
		}
		public Boolean ImgType1
		{
			get
			{
				return _ImgType1;
			}
			set
			{
				_ImgType1 = value;
				OnPropertyChanged("ImgType1");
			}
		}
		public Boolean ImgType2
		{
			get
			{
				return _ImgType2;
			}
			set
			{
				_ImgType2 = value;
				OnPropertyChanged("ImgType2");
			}
		}
		public Boolean ImgType3
		{
			get
			{
				return _ImgType3;
			}
			set
			{
				_ImgType3 = value;
				OnPropertyChanged("ImgType3");
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
