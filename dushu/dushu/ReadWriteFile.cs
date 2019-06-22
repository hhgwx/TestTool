using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dushu
{
    class ReadWriteFile
    {
        public List<String> readFile(String path, Boolean base64Flg)
        {
            List<String> wordList = new List<String>();
            try
            {
				Encoding encoding = GetType(path);

				// StreamReader の新しいインスタンスを生成する
				StreamReader sreader = (
                    new StreamReader(path, encoding)
                    );

                // 読み込んだ結果をすべて格納するための変数を宣言する
                string readline = string.Empty;

                // 読み込みできる文字がなくなるまで繰り返す
                while (sreader.Peek() >= 0)
                {
                    // ファイルを 1 行ずつ読み込む
                    readline = sreader.ReadLine().ToString();
                    if (readline.Length >= 1)
                    {
						if (base64Flg)
						{
							readline = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(readline));
						}
						wordList.Add(readline);
                    }   // end if
                }   // end while

                // sreader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
                sreader.Close();

                return wordList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("設定ファイルの読込み処理でエラーが発生しました。：" + ex.Message.ToString());
                return wordList;
            }

        }

        //flag false:上書き/ true:追加
        public void writeFile(String path, String message, Boolean flag)
        {
            List<String> wordList = new List<String>();
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(
                    path,
                    flag,  //  （ false:上書き/ true:追加 ）
                    Encoding.GetEncoding("UTF-8"));

                writer.WriteLine(message);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("レコード登録でエラーが発生しました。：" + ex.Message.ToString());
            }

        }

		public String getFileSize(String path)
		{
			System.IO.FileInfo fileInfo = null;
			try
			{
				fileInfo = new System.IO.FileInfo(path);
				return fileInfo.Length + "";
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				// 其他处理异常的代码
			}
			return "0";
		}


		/// <summary> 
		/// 通过给定的文件流，判断文件的编码类型 
		/// </summary> 
		/// <param name=“fs“>文件流</param> 
		/// <returns>文件的编码类型</returns> 
		public Encoding GetType(string filePath)
		{
			byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
			byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
			byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
			Encoding reVal = Encoding.GetEncoding("GB2312");

			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

			BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
			int i;
			int.TryParse(fs.Length.ToString(), out i);
			byte[] ss = r.ReadBytes(i);
			if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
			{
				reVal = Encoding.UTF8;
			}
			else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
			{
				reVal = Encoding.BigEndianUnicode;
			}
			else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
			{
				reVal = Encoding.Unicode;
			}
			r.Close();
			fs.Close();
			return reVal;

		}

		/// <summary> 
		/// 判断是否是不带 BOM 的 UTF8 格式 
		/// </summary> 
		/// <param name=“data“></param> 
		/// <returns></returns> 
		private bool IsUTF8Bytes(byte[] data)
		{
			int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
			byte curByte; //当前分析的字节. 
			for (int i = 0; i < data.Length; i++)
			{
				curByte = data[i];
				if (charByteCounter == 1)
				{
					if (curByte >= 0x80)
					{
						//判断当前 
						while (((curByte <<= 1) & 0x80) != 0)
						{
							charByteCounter++;
						}
						//标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
						if (charByteCounter == 1 || charByteCounter > 6)
						{
							return false;
						}
					}
				}
				else
				{
					//若是UTF-8 此时第一位必须为1 
					if ((curByte & 0xC0) != 0x80)
					{
						return false;
					}
					charByteCounter--;
				}
			}
			if (charByteCounter > 1)
			{
				throw new Exception("非预期的byte格式");
			}
			return true;
		}
	}
}
