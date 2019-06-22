using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicManager
{
    class InfValue//:INotifyPropertyChanged
    {
        public Dictionary<string, string> inf_win1_comboBox_percent_list = new Dictionary<string, string>() {
            {"70","70"},
            {"80","80"},
            {"85","85"},
            {"90","90"},
            {"95","95"},
            {"100","100"}
            };

        private string _inf_win1_comboBox_percent;  //百分比下拉框选择值
        private string _inf_win1_checkbox_merge;     //文件夹合并
        private string _inf_win1_checkBox_sameDel;   //相同直接删除

//        public event PropertyChangedEventHandler PropertyChanged;

        public string inf_win1_comboBox_percent
        {
            get { return _inf_win1_comboBox_percent; }
            set
            {
                _inf_win1_comboBox_percent = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(inf_win1_comboBox_percent));
            }
        }

        public string inf_win1_checkbox_merge
        {
            get { return _inf_win1_checkbox_merge; }
            set
            {
                _inf_win1_checkbox_merge = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(inf_win1_checkbox_merge));
            }
        }

        public string inf_win1_checkBox_sameDel
        {
            get { return _inf_win1_checkBox_sameDel; }
            set
            {
                _inf_win1_checkBox_sameDel = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(inf_win1_checkBox_sameDel));
            }
        }
    }
}
