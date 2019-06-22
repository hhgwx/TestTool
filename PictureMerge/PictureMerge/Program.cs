using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureMerge
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //            Merge merge = new Merge();
            //            for (int i=0;i<1;i++) { 
            //                merge.MergePic();
            //            }
            //TODO
            //1 jpeg gif 多后缀
            //2 读入文件排序
            //3 文件合并选项 OK
            //4 双击打开图像
            //5 初期选择路径的保存 OK
            //6 Test用的写死的路径删除 OK
            //7 签名制作 打赏
            //8 进度条 OK
            //9 只读文件测试
            //10 完全相同直接删除 OK
        }


    }
}
