using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicManager.PicMerge
{
    class ListBox_win1_folder_data
    {
        public int Id { get; set; }
        public string PathStr { get; set; }

        public ListBox_win1_folder_data(int Id, string PathStr)
        {
            this.Id = Id;
            this.PathStr = PathStr;
        }
    }
}
