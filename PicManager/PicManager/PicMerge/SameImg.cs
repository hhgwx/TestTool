using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicManager.PicMerge
{
    class SameImg:INotifyPropertyChanged
    {
        private string _imageSrc;
        private string _imageSame;

        public event PropertyChangedEventHandler PropertyChanged;

        public string imageSrc
        {
            get { return _imageSrc; }
            set {
                _imageSrc = value;
                PropertyChanged(this,new PropertyChangedEventArgs(imageSrc));
            }
        }

        public string imageSame
        {
            get { return _imageSame; }
            set {
                _imageSame = value;
                PropertyChanged(this, new PropertyChangedEventArgs(imageSame));
            }
        }

        public SameImg(string _imageSrc,string _imageSame)
        {
            this._imageSrc = _imageSrc;
            this._imageSame = _imageSame;
        }
    }
}
