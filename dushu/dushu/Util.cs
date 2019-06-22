using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dushu
{
    static class Util
    {
        public static bool isNumberic(string message, out int time)
        {
            time = -1;   //time 定义为out 用来输出值
            try
            {
                //当数字字符串的为是少于4时，以下三种都可以转换，任选一种
                //如果位数超过4的话，请选用Convert.ToInt32() 和int.Parse()

                //time = int.Parse(message);
                //time = Convert.ToInt16(message);
                time = Convert.ToInt32(message.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
