using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVEdit
{
	class ReadWriteFile
	{
		public List<string> readFile(string path, Boolean base64Flg)
		{
			List<string> wordList = new List<string>();
			try
			{
				Encoding encoding = GetCode(path);// GetType(path);

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
				Console.WriteLine("ファイルの読込み処理でエラーが発生しました。：" + ex.Message.ToString());
				return wordList;
			}

		}

		//flag false:上書き/ true:追加
		public void writeFile(String path, String message, Boolean flag)
		{
			List<String> wordList = new List<String>();
			try
			{
				Encoding encoding = Encoding.GetEncoding("UTF-8");
				if (File.Exists(path)) {
					encoding = GetCode(path);// GetType(path);
				}

				System.IO.StreamWriter writer = new System.IO.StreamWriter(
					path,
					flag,  //  （ false:上書き/ true:追加 ）
					encoding);

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
			else
			{
				string encodedStr = Encoding.GetEncoding("Shift-JIS").GetString(ss);
				byte[] encodedByte = Encoding.GetEncoding("Shift-JIS").GetBytes(encodedStr);

				if (ss.Length == encodedByte.Length)
				{
					Boolean shiftJisFlg = true;
					for (int j = 0; j < ss.Length; j++)
					{
						if (!ss[j].Equals(encodedByte[j]))
						{
							shiftJisFlg = false;
							break;
						}
					}
					if (shiftJisFlg == true)
					{
						reVal = Encoding.GetEncoding("Shift-JIS");
					}
				}
				
			}
			r.Close();
			fs.Close();
			return reVal;

		}

		/// <summary>
		/// 文字コードを判別する
		/// </summary>
		/// <remarks>
		/// Jcode.pmのgetcodeメソッドを移植したものです。
		/// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
		/// Jcode.pmの著作権情報
		/// Copyright 1999-2005 Dan Kogai <dankogai@dan.co.jp>
		/// This library is free software; you can redistribute it and/or modify it
		///  under the same terms as Perl itself.
		/// </remarks>
		/// <param name="bytes">文字コードを調べるデータ</param>
		/// <returns>適当と思われるEncodingオブジェクト。
		/// 判断できなかった時はnull。</returns>
		public static System.Text.Encoding GetCode(string filePath)
		{
			//テキストファイルを開く
			byte[] bytes = System.IO.File.ReadAllBytes(filePath);

			const byte bEscape = 0x1B;
			const byte bAt = 0x40;
			const byte bDollar = 0x24;
			const byte bAnd = 0x26;
			const byte bOpen = 0x28;    //'('
			const byte bB = 0x42;
			const byte bD = 0x44;
			const byte bJ = 0x4A;
			const byte bI = 0x49;

			int len = bytes.Length;
			byte b1, b2, b3, b4;

			//Encode::is_utf8 は無視

			bool isBinary = false;
			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];
				if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
				{
					//'binary'
					isBinary = true;
					if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
					{
						//smells like raw unicode
						return System.Text.Encoding.Unicode;
					}
				}
			}
			if (isBinary)
			{
				return null;
			}

			//not Japanese
			bool notJapanese = true;
			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];
				if (b1 == bEscape || 0x80 <= b1)
				{
					notJapanese = false;
					break;
				}
			}
			if (notJapanese)
			{
				return System.Text.Encoding.ASCII;
			}

			for (int i = 0; i < len - 2; i++)
			{
				b1 = bytes[i];
				b2 = bytes[i + 1];
				b3 = bytes[i + 2];

				if (b1 == bEscape)
				{
					if (b2 == bDollar && b3 == bAt)
					{
						//JIS_0208 1978
						//JIS
						return System.Text.Encoding.GetEncoding(50220);
					}
					else if (b2 == bDollar && b3 == bB)
					{
						//JIS_0208 1983
						//JIS
						return System.Text.Encoding.GetEncoding(50220);
					}
					else if (b2 == bOpen && (b3 == bB || b3 == bJ))
					{
						//JIS_ASC
						//JIS
						return System.Text.Encoding.GetEncoding(50220);
					}
					else if (b2 == bOpen && b3 == bI)
					{
						//JIS_KANA
						//JIS
						return System.Text.Encoding.GetEncoding(50220);
					}
					if (i < len - 3)
					{
						b4 = bytes[i + 3];
						if (b2 == bDollar && b3 == bOpen && b4 == bD)
						{
							//JIS_0212
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						}
						if (i < len - 5 &&
							b2 == bAnd && b3 == bAt && b4 == bEscape &&
							bytes[i + 4] == bDollar && bytes[i + 5] == bB)
						{
							//JIS_0208 1990
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						}
					}
				}
			}

			//should be euc|sjis|utf8
			//use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
			int sjis = 0;
			int euc = 0;
			int utf8 = 0;
			for (int i = 0; i < len - 1; i++)
			{
				b1 = bytes[i];
				b2 = bytes[i + 1];
				if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
					((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
				{
					//SJIS_C
					sjis += 2;
					i++;
				}
			}
			for (int i = 0; i < len - 1; i++)
			{
				b1 = bytes[i];
				b2 = bytes[i + 1];
				if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
					(b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
				{
					//EUC_C
					//EUC_KANA
					euc += 2;
					i++;
				}
				else if (i < len - 2)
				{
					b3 = bytes[i + 2];
					if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
						(0xA1 <= b3 && b3 <= 0xFE))
					{
						//EUC_0212
						euc += 3;
						i += 2;
					}
				}
			}
			for (int i = 0; i < len - 1; i++)
			{
				b1 = bytes[i];
				b2 = bytes[i + 1];
				if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
				{
					//UTF8
					utf8 += 2;
					i++;
				}
				else if (i < len - 2)
				{
					b3 = bytes[i + 2];
					if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
						(0x80 <= b3 && b3 <= 0xBF))
					{
						//UTF8
						utf8 += 3;
						i += 2;
					}
				}
			}
			//M. Takahashi's suggestion
			//utf8 += utf8 / 2;

			System.Diagnostics.Debug.WriteLine(
				string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
			if (euc > sjis && euc > utf8)
			{
				//EUC
				return System.Text.Encoding.GetEncoding(51932);
			}
			else if (sjis > euc && sjis > utf8)
			{
				//SJIS
				return System.Text.Encoding.GetEncoding(932);
			}
			else if (utf8 > euc && utf8 > sjis)
			{
				//UTF8
				return System.Text.Encoding.UTF8;
			}

			return null;
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