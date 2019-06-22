using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSearch.mapSearch
{
    class PieceArea
    {
        public Boolean wallFlg = false; //壁フラグ　false:壁じゃない　true:壁
        public int direction = -1;      //方向  1:右下 2:下 3:左下 4:左 5:左上 6:上 7:右上 8:右
        public int level = -1;          //1:起点 0:終点
        public double distance = 0;     //当該エリアから起点までの距離

        public int keyIndex = 0;        // loop single key
    }
}
