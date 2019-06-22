using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64InOutZIP
{
    class Program
    {
        String ZIP_PATH_IN = System.AppDomain.CurrentDomain.BaseDirectory + @"\IN.ZIP";
        String ZIP_PATH_OUT = System.AppDomain.CurrentDomain.BaseDirectory + @"\OUT.ZIP";
        String TXT_PATH = System.AppDomain.CurrentDomain.BaseDirectory + @"\out.txt";
        static void Main(string[] args)
        {
            Program prg= new Program();

//          prg.zipToStr();
//            prg.strToZip();
			prg.txtToTxt();
		}

        public void zipToStr()
        {
            try
            {
                var str = Convert.ToBase64String(File.ReadAllBytes(ZIP_PATH_IN));

                System.IO.StreamWriter writer = new System.IO.StreamWriter(
                    TXT_PATH,
                    false,  //  （ false:上書き/ true:追加 ）
                    Encoding.GetEncoding("UTF-8"));

                writer.WriteLine(str);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("レコード登録でエラーが発生しました。：" + ex.Message.ToString());
            }
        }

        public void strToZip()
        {
            try
            {
                StreamReader sreader = (
                    new StreamReader(TXT_PATH, System.Text.Encoding.GetEncoding("UTF-8"))
                    );

                String str = sreader.ReadToEnd().ToString();


                byte[] byteData = Convert.FromBase64String(str);

                File.WriteAllBytes(ZIP_PATH_OUT, byteData);
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine("レコード登録でエラーが発生しました。：" + ex.Message.ToString());
            }
        }

		public void txtToTxt()
		{
			System.IO.StreamWriter writer = null;
			try
			{
				String txtIN = System.AppDomain.CurrentDomain.BaseDirectory + @"\1.txt";
				String txtOUT = System.AppDomain.CurrentDomain.BaseDirectory + @"\1_B.txt";

				StreamReader sreader = (
					new StreamReader(txtIN, System.Text.Encoding.GetEncoding("UTF-8"))
					);

				String lineStr = "";
				String lineStrBase64 = "";

				if (File.Exists(txtOUT)) {
					File.Delete(txtOUT);
				}

				writer = new System.IO.StreamWriter(
					txtOUT,
					false,  //  （ false:上書き/ true:追加 ）
					Encoding.GetEncoding("UTF-8"));

				// 読み込みできる文字がなくなるまで繰り返す
				while (sreader.Peek() >= 0)
				{
					// ファイルを 1 行ずつ読み込む
					lineStr = sreader.ReadLine().ToString();
					lineStrBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(lineStr));
					writer.WriteLine(lineStrBase64);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("レコード登録でエラーが発生しました。：" + ex.Message.ToString());
			}
			finally
			{
				if (writer != null) writer.Close();
			}
		}
	}
}
