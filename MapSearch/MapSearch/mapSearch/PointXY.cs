using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSearch.mapSearch
{
    class PointXY
    {
        public int pointX
        {
            get;
            set;
        }
        public int pointY
        {
            get;
            set;
        }

        /**
         * @param pointY 行目
         * @param pointX 列目
         */
        public PointXY(int pointY, int pointX)
        {
            this.pointY = pointY;
            this.pointX = pointX;
        }
    }
}
