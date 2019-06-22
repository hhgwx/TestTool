using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceColor
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FromFolder;
		private Boolean _ImgType1;
		private Boolean _ImgType2;
		private Boolean _ImgType3;
		private Boolean _SubFolder;
		private Boolean _ReMake; 
		private String _OutFolder;
		private Boolean _BlackWhiteRadio;
		private Boolean _ColorRadio;
		private String _ReplaceColor;

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
		public Boolean BlackWhiteRadio
		{
			get
			{
				return _BlackWhiteRadio;
			}
			set
			{
				_BlackWhiteRadio = value;
				OnPropertyChanged("BlackWhiteRadio");
			}
		}
		public Boolean ColorRadio
		{
			get
			{
				return _ColorRadio;
			}
			set
			{
				_ColorRadio = value;
				OnPropertyChanged("ColorRadio");
			}
		}
		public String ReplaceColor
		{
			get
			{
				return _ReplaceColor;
			}
			set
			{
				_ReplaceColor = value;
				OnPropertyChanged("ReplaceColor");
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
