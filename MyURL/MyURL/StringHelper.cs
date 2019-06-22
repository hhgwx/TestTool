using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyURL
{
    class StringHelper
    {
        public static string SubString(string str,int byteNum)
        {
            string strResult="";

            if (str != null)
            {
                return strResult;
            }

            if (str.Length >= byteNum)
            {
                strResult = str.Substring(0, byteNum);
            } else
            {
                strResult = str;
            }
            return strResult;
        }
    }
}
