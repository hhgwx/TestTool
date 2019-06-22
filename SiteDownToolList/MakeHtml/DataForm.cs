using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeHtml
{
	class DataForm : INotifyPropertyChanged
	{
		private String _FromFolder;
		private Boolean _ReMake;
		private String _HtmlName;

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
		public String HtmlName
		{
			get
			{
				return _HtmlName;
			}
			set
			{
				_HtmlName = value;
				OnPropertyChanged("HtmlName");
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
