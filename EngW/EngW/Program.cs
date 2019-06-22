using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngW
{
    static class Program
    {
        public static string WORD_PATH = System.AppDomain.CurrentDomain.BaseDirectory + @"\english 4.txt";
        public static string RECORD_PATH = System.AppDomain.CurrentDomain.BaseDirectory + @"\record.txt";


        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //txt read
            ReadWriteFile rFile = new ReadWriteFile();
            List<String> wordList = rFile.readFile(Program.WORD_PATH);
            List<String> record = rFile.readFile(Program.RECORD_PATH);
            String recordNo1 = "0";
            if (record != null && record.Count > 0)
            {
                recordNo1 = record[0];
            }
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(wordList, recordNo1));
        }
    }
}
