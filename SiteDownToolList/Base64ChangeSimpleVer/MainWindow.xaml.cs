using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Base64ChangeSimpleVer
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DataForm dataForm = new DataForm();

		public MainWindow()
		{
			InitializeComponent();

			dataForm.TxtToBase64 = true;
			this.DataContext = dataForm;
		}

		private void button_replace_Click(object sender, RoutedEventArgs e)
		{
			if (!File.Exists(dataForm.FileName))
			{
				MessageBox.Show(dataForm.FileName + " 文件不存在。");
				return;
			}

			else if (!dataForm.FileName.EndsWith("txt") && !dataForm.FileName.EndsWith("TXT"))
			{
				MessageBox.Show(dataForm.FileName + " 不是TXT文件。");
				return;
			}

			if (dataForm.TxtToBase64)
			{
				TxtToBase64();
			}
			else if (dataForm.Base64ToTxt)
			{
				Base64ToTxt();
			}
		}

		private void button_selectFile_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Filter = "文本文件|*.txt";
			openFileDialog.ValidateNames = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.InitialDirectory = dataForm.FileName;

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dataForm.FileName = openFileDialog.FileName;
			}
		}
		public void TxtToBase64()
		{
			System.IO.StreamWriter writer = null;
			try
			{
				Encoding encoding = GetType(dataForm.FileName);

				String tofileName = dataForm.FileName.Replace(".txt", "_加密.txt").Replace(".TXT", "_加密.txt");

				StreamReader sreader = (
					new StreamReader(dataForm.FileName, encoding)
					);

				String lineStr = "";
				String lineStrBase64 = "";

//				if (File.Exists(txtOUT))
//				{
//					File.Delete(txtOUT);
//				}

				writer = new System.IO.StreamWriter(
					tofileName,
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
				dataForm.ResultMsg = "做成文件:" + tofileName;
			}
			catch (Exception ex)
			{
				MessageBox.Show("エラーが発生しました。：" + ex.Message.ToString());
			}
			finally
			{
				if (writer != null) writer.Close();
			}
		}

		public void Base64ToTxt()
		{
			System.IO.StreamWriter writer = null;
			try
			{
				String tofileName = dataForm.FileName.Replace(".txt", "_解密.txt").Replace(".TXT", "_解密.txt");

				StreamReader sreader = (
					new StreamReader(dataForm.FileName, System.Text.Encoding.GetEncoding("UTF-8"))
					);

				String lineStr = "";
				String lineStrBase64 = "";

				//				if (File.Exists(txtOUT))
				//				{
				//					File.Delete(txtOUT);
				//				}

				writer = new System.IO.StreamWriter(
					tofileName,
					false,  //  （ false:上書き/ true:追加 ）
					Encoding.GetEncoding("UTF-8"));

				// 読み込みできる文字がなくなるまで繰り返す
				while (sreader.Peek() >= 0)
				{
					// ファイルを 1 行ずつ読み込む
					lineStr = sreader.ReadLine().ToString();
					lineStrBase64 = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(lineStr));
					writer.WriteLine(lineStrBase64);
				}

				dataForm.ResultMsg = "做成文件:" + tofileName;
			}
			catch (Exception ex)
			{
				MessageBox.Show("エラーが発生しました。：" + ex.Message.ToString());
			}
			finally
			{
				if (writer != null) writer.Close();
			}
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
				throw new Exception("非预期的格式");
			}
			return true;
		}
	}
}
