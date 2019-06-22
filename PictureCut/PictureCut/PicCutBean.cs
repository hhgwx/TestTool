using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureCut
{
    class PicCutBean
    {
        public string Name { get; set; }
        public bool CutRowFlg { get; set; } //true:行分割 false:分割
        public int CutNum { get; set; }
        public List<int> listCutPoint;

        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }

        public PicCutBean()
        {
            listCutPoint = new List<int>();
        }
    }
}
