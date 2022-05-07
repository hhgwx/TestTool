using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReNameTool
{
    class DataForm : INotifyPropertyChanged
    {
        private String _TextPath;
        private Boolean _CheckFolder;
        private Boolean _CheckFile;
        private Boolean _CheckSubFolder;
        private Boolean _CheckPaishe;
        private String _TextFrom;
        private String _TextTo;

        private String _ReplaceType;
        private String[] _TextToArr = {"", "", ""};

		public String TextPath
		{
			get
			{
				return _TextPath;
			}
			set
			{
				_TextPath = value;
				OnPropertyChanged("TextPath");
			}
		}

		public Boolean CheckFolder
		{
			get
			{
				return _CheckFolder;
			}
			set
			{
				_CheckFolder = value;
				OnPropertyChanged("CheckFolder");
			}
		}
		public Boolean CheckFile
		{
			get
			{
				return _CheckFile;
			}
			set
			{
				_CheckFile = value;
				OnPropertyChanged("CheckFile");
			}
		}

		public Boolean CheckSubFolder
		{
			get
			{
				return _CheckSubFolder;
			}
			set
			{
				_CheckSubFolder = value;
				OnPropertyChanged("CheckSubFolder");
			}
		}

        public Boolean CheckPaishe
        {
            get
            {
                return _CheckPaishe;
            }
            set
            {
                _CheckPaishe = value;
                OnPropertyChanged("CheckPaishe");
            }
        }
        public String TextFrom
		{
			get
			{
				return _TextFrom;
			}
			set
			{
				_TextFrom = value;
				OnPropertyChanged("TextFrom");
			}
		}

		public String TextTo
		{
			get
			{
				return _TextTo;
			}
			set
			{
				_TextTo = value;
				OnPropertyChanged("TextTo");
			}
		}

        public String ReplaceType
        {
            get
            {
                return _ReplaceType;
            }
            set
            {
                _ReplaceType = value;
                OnPropertyChanged("ReplaceType");
            }
        }

        public void setTextToArr(int index, String value)
        {
            _TextToArr[index] = value;
        }

        public String getTextToArr(int index)
        {
            return _TextToArr[index];
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
