using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicManager.BirthdayPic
{
    class FolderMap: INotifyPropertyChanged
    {
        private string _DisplayName;
        private string _Icon;
        private string _FullPath;

        public string Icon {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
                PropertyChanged(this,new PropertyChangedEventArgs(Icon));
            }
        }
        public string DisplayName {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(DisplayName));
            }
        }

        public string FullPath
        {
            get
            {
                return _FullPath;
            }
            set
            {
                _FullPath = value;
                PropertyChanged(this, new PropertyChangedEventArgs(FullPath));
            }
        }

        public ObservableCollection<FolderMap> Children { get; set; }

        public FolderMap(string _DisplayName, string _Icon, string _FullPath)
        {
            Children = new ObservableCollection<FolderMap>();

            this._DisplayName = _DisplayName;
            this._Icon = _Icon;
            this._FullPath = _FullPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
