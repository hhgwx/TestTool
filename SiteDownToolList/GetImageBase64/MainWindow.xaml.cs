using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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

namespace GetImageBase64
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

			this.DataContext = dataForm;
		}
		private void button_selectFile_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Filter = "图片|*.jpeg;*.jpg;*.gif;*.png|所有文件|*.*";
			openFileDialog.ValidateNames = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.InitialDirectory = dataForm.ImgFile;

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dataForm.ImgFile = openFileDialog.FileName;
			}

			dataForm.Base64 = getBase64(dataForm.ImgFile);
		}

		private void button_change_Click(object sender, RoutedEventArgs e)
		{
			if (File.Exists(dataForm.ImgFile))
			{
				dataForm.Base64 = getBase64(dataForm.ImgFile);
			}
			else
			{
				dataForm.Base64 = "";
			}
			
		}

		private string getBase64(string path)
		{
			System.Drawing.Image fromImage = System.Drawing.Image.FromFile(path);
			MemoryStream stream = new MemoryStream();
			fromImage.Save(stream, ImageFormat.Png);
			string base64 = Convert.ToBase64String(stream.GetBuffer());

			return base64;
		}
	}
}
