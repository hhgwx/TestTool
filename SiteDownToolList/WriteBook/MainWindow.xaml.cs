using ForAll;
using System;
using System.Collections.Generic;
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

namespace WriteBook
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		string nowFilePath = "";

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_KeyDown_1(object sender, KeyEventArgs e)
		{
			if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.O)
			{
				String oldFile = nowFilePath;
				System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
				openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
				openFileDialog.ValidateNames = true;
				openFileDialog.CheckPathExists = true;
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					nowFilePath = openFileDialog.FileName;
				}
				if (oldFile != nowFilePath)
				{
					ReadWriteFile rwFile = new ReadWriteFile();
					//nowFile Save
					rwFile.writeFile(oldFile, this.text_edit.Text, false);

					//newFile read
					this.text_edit.Text = rwFile.readFileStr(nowFilePath, false);

					this.text_edit.Focus();
					//设置光标的位置到文本尾
					this.text_edit.Select(this.text_edit.Text.Length, 0);
					//滚动到最后
					this.text_edit.ScrollToEnd();
				}

				e.Handled = true;
			}
			else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
			{
				//nowFile Save
				if (nowFilePath != "") {
					ReadWriteFile rwFile = new ReadWriteFile();
					rwFile.writeFile(nowFilePath, this.text_edit.Text, false);
				}

				e.Handled = true;
			}
			else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Q)
			{
				if (nowFilePath != "")
				{
					ReadWriteFile rwFile = new ReadWriteFile();
					rwFile.writeFile(nowFilePath, this.text_edit.Text, false);
				}
				this.Close();
			}
			else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.H)
			{
				MessageBox.Show("CTRL+O  打开\nCTRL+S  保存\nCTRL+Q  退出\nCTRL+H  帮助");
			}
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			this.text_edit.Focus();
		}

		private void text_edit_MouseEnter(object sender, MouseEventArgs e)
		{
			this.text_edit.Background = Brushes.Gray;
		}

		private void text_edit_MouseLeave(object sender, MouseEventArgs e)
		{
			this.text_edit.Background = Brushes.Transparent;
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
				this.text_edit.Focus();
				this.text_edit.Background = Brushes.Transparent;
			}
		}
	}
}
