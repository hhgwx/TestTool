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

namespace InchPhotosCSharp
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool isMouseLeftButtonDown = false;
		Point previousMousePoint = new Point(0, 0);
		private int[,] sizeArr = {{25,35},{35,53}, {0,0}};
		private int sizeSelected = 0;
		private double sizePer = 1.0; //像素和CM的比率
		private int Rectangle_x = 0; //四角形的开始点X
		private int Rectangle_y = 0; //四角形的开始点Y
		private int Rectangle_width = 0; //四角形的宽
		private int Rectangle_height = 0; //四角形的高

		private int[,] paperSizeArr = { { 89, 127 }, { 127, 178 }, { 0, 0 } };
		private int paperSelected = 0;
		//		private double paperPer = 1.0; //像素和CM的比率
		//		private int resultCanas_x = 0; //结果出力画布的开始点X
		//		private int resultCanas_y = 0; //结果出力画布的开始点Y
		//		private int resultCanas_width = 0; //结果出力画布的宽
		//		private int resultCanas_height = 0; //结果出力画布的高

		private int dpiValue = 96;
		private int dpiValueMulti = 16;

		private int distance = 4; //间隔

		public MainWindow()
		{
			InitializeComponent();

			Init();
		}

		private void Init()
		{
			inputImg_textBox.Text = @"D:\mergeImg\2.jpg";
			outputImg_textBox.Text = @"D:\mergeImg\canvas.png";

			statusCheck();
		}

		private void image_show_MouseDown(object sender, MouseButtonEventArgs e)
		{
			isMouseLeftButtonDown = true;
			previousMousePoint = e.GetPosition(image_show);
		}

		private void image_show_MouseMove(object sender, MouseEventArgs e)
		{
			if (isMouseLeftButtonDown) {
				Point position = e.GetPosition(image_show);
				tlt.X += position.X - this.previousMousePoint.X;
				tlt.Y += position.Y - this.previousMousePoint.Y;
			}
		}

		private void image_show_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isMouseLeftButtonDown = false;
		}

		private void slider_1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			isMouseLeftButtonDown = false;
			sfr.ScaleX = slider_1.Value / 25;
			sfr.ScaleY = slider_1.Value / 25;
		}
		private void loadImg_button_Click(object sender, RoutedEventArgs e)
		{
			string picPath = "";

			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "图片文件|*.jpg;*.jpeg;*.png";
			if (dialog.ShowDialog() == true)
			{
				picPath = dialog.FileName;
				inputImg_textBox.Text = dialog.FileName;
			}

			BitmapImage img = new BitmapImage();
			img.BeginInit();
			img.StreamSource = new MemoryStream(File.ReadAllBytes(picPath));
			img.EndInit();
			this.image_show.Source = img;

		}
/*		private void saveImg_button_Click(object sender, RoutedEventArgs e)
		{
			String path = outputImg_textBox.Text;
			if (path == "") {
				MessageBox.Show("please input savePath.");
				return;
			}
			SaveCanvas(this.result_canvas, dpiValue, path);
		}*/
		public void SaveCanvas(Canvas canvas, int dpi, string filename)
		{
			Size size = new Size(canvas.Width, canvas.Height);
			canvas.Measure(size);
			canvas.Arrange(new Rect(size));

			RenderTargetBitmap rtb = new RenderTargetBitmap(
				(int)canvas.Width * dpiValueMulti / 4, //width
				(int)canvas.Height * dpiValueMulti / 4, //height
				dpi * dpiValueMulti / 4, //dpi x
				dpi * dpiValueMulti / 4, //dpi y
				PixelFormats.Pbgra32 // pixelformat
				);

			//移動しないと、正しいエリアが取得できないです
//			canvas.RenderTransform = new TranslateTransform(Rectangle_x*-1, Rectangle_y * -1);
//			canvas.UpdateLayout();
			rtb.Render(canvas);

			SaveRTBAsPNG(rtb, filename);
//			canvas.RenderTransform = new TranslateTransform(0, 0);
//			canvas.UpdateLayout();
		}
		public RenderTargetBitmap getCanvasImg(Canvas canvas, int dpi)
		{
			Size size = new Size(Rectangle_width, Rectangle_height);
			canvas.Measure(size);
			canvas.Arrange(new Rect(size));

			RenderTargetBitmap rtb = new RenderTargetBitmap(
				Rectangle_width * dpiValueMulti, //width
				Rectangle_height * dpiValueMulti, //height
				dpi * dpiValueMulti, //dpi x
				dpi * dpiValueMulti, //dpi y
				PixelFormats.Pbgra32 // pixelformat
				);

			//移動しないと、正しいエリアが取得できないです
			canvas.RenderTransform = new TranslateTransform(Rectangle_x * -1, Rectangle_y * -1);
			canvas.UpdateLayout();
			rtb.Render(canvas);

			canvas.RenderTransform = new TranslateTransform(0, 0);
			canvas.UpdateLayout();

			return rtb;
		}
		private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
		{
			PngBitmapEncoder enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
			enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

			using (FileStream stm = System.IO.File.Create(filename))
			{
				enc.Save(stm);
			}
		}

		private void size_check_Click(object sender, RoutedEventArgs e)
		{
			statusCheck();
		}
		private void sizeFreeW_text_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				sizeArr[2, 0] = Convert.ToInt16(sizeFreeW_text.Text);
				sizeArr[2, 1] = Convert.ToInt16(sizeFreeH_text.Text);
			}
			catch (Exception)
			{
				//MessageBox.Show("自定义大小不正确");
				return;
			}
			sizeChange(2);
		}
		private void statusCheck()
		{
			if (size1_check.IsChecked.HasValue && (bool)size1_check.IsChecked)
			{
				sizeSelected = 0;
				sizeChange(sizeSelected);
			}
			else if (size2_check.IsChecked.HasValue && (bool)size2_check.IsChecked)
			{
				sizeSelected = 1;
				sizeChange(sizeSelected);
			}
			else if (sizeFree_check.IsChecked.HasValue && (bool)sizeFree_check.IsChecked)
			{
				sizeSelected = 2;
				try
				{
					sizeArr[2, 0] = Convert.ToInt16(sizeFreeW_text.Text);
					sizeArr[2, 1] = Convert.ToInt16(sizeFreeH_text.Text);
				}
				catch (Exception)
				{
					//MessageBox.Show("自定义大小不正确");
					return;
				}
				sizeChange(sizeSelected);
			}
		}
		private void sizeChange(int index)
		{
			int width = sizeArr[index, 0];
			int height = sizeArr[index, 1];

			int width_show = 400;
			int height_show = 400;
			if (width < height)
			{
				width_show = height_show * width / height;
			}
			else
			{
				height_show = width_show * height / width;
			}

			Rectangle_width = width_show;
			Rectangle_height = height_show;
			Rectangle_x = (500 - width_show) / 2;
			Rectangle_y = (500 - height_show) / 2;

			//四角刑を作成
			RectangleGeometry myRectangleGeometry = new RectangleGeometry();
			myRectangleGeometry.Rect = new Rect(Rectangle_x-1, Rectangle_y-1, Rectangle_width + 2, Rectangle_height + 2);

			rectangle_path.Data = myRectangleGeometry;

			sizePer = Rectangle_width / width;
		}


		private void paperSize_check_Click(object sender, RoutedEventArgs e)
		{
			paperStatusCheck();
		}
		private void paperSizeFreeW_text_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				paperSizeArr[2, 0] = Convert.ToInt16(paperSizeFreeW_text.Text);
				paperSizeArr[2, 1] = Convert.ToInt16(paperSizeFreeH_text.Text);
			}
			catch (Exception)
			{
				//MessageBox.Show("自定义大小不正确");
				return;
			}
		}
		private void paperStatusCheck()
		{
			if (paperSize1_check.IsChecked.HasValue && (bool)paperSize1_check.IsChecked)
			{
				paperSelected = 0;
				//paperSizeChange(paperSelected);
			}
			else if (paperSize2_check.IsChecked.HasValue && (bool)paperSize2_check.IsChecked)
			{
				paperSelected = 1;
				//paperSizeChange(paperSelected);
			}
			else if (paperSizeFree_check.IsChecked.HasValue && (bool)paperSizeFree_check.IsChecked)
			{
				paperSelected = 2;
				try
				{
					paperSizeArr[2, 0] = Convert.ToInt16(paperSizeFreeW_text.Text);
					paperSizeArr[2, 1] = Convert.ToInt16(paperSizeFreeH_text.Text);
				}
				catch (Exception)
				{
					//MessageBox.Show("自定义大小不正确");
					return;
				}
				//paperSizeChange(paperSelected);
			}
		}
		private void saveImg_button_Click(object sender, RoutedEventArgs e) // priview_button_Click -> saveImg_button_Click
		{

			String path = outputImg_textBox.Text;
			if (path == "")
			{
				MessageBox.Show("please input savePath.");
				return;
			}

			canvas_sfr.ScaleX = 1;
			canvas_sfr.ScaleY = 1;

			RenderTargetBitmap renderTargetBitmap = getCanvasImg(this.canvas_img, dpiValue);

			int toBig = 1; //为了最终保存时，图片清晰一些，将目标画板增大比率 -> 画板不扩大，但通过高DPi来提高图片质量

			int equalPicWidth = (int)(paperSizeArr[paperSelected, 0] * sizePer);
			int equalPicHeight = (int)(paperSizeArr[paperSelected, 1] * sizePer);

			int i = 0;
			int j = 0;
			//判断，横着放，还是竖着放
			if ((equalPicWidth / (Rectangle_width + distance * 2)) * (equalPicHeight / (Rectangle_height + distance * 2)) <
				(equalPicWidth / (Rectangle_height + distance * 2)) * (equalPicHeight / (Rectangle_width + distance * 2)))
			{
				i = equalPicWidth / Rectangle_height;
				j = equalPicHeight / Rectangle_width;

				//出力画板横竖颠倒
				result_canvas.Width = equalPicHeight * toBig;
				result_canvas.Height = equalPicWidth * toBig;
			}
			else
			{
				i = equalPicWidth / Rectangle_width;
				j = equalPicHeight / Rectangle_height;

				result_canvas.Width = equalPicWidth * toBig;
				result_canvas.Height = equalPicHeight * toBig;
			}

			result_canvas.Children.Clear();

			for (int iTemp = 0;iTemp < i; iTemp++) {
				for (int jTemp = 0; jTemp < j; jTemp++)
				{
					Image image = new Image();
					image.Source = renderTargetBitmap;
					image.Width = Rectangle_width * toBig;
					image.Height = Rectangle_height * toBig;
					image.SetValue(Canvas.LeftProperty, jTemp * (Rectangle_width * toBig + distance * 2) * 1.0 + distance);
					image.SetValue(Canvas.TopProperty, iTemp * (Rectangle_height * toBig + distance * 2) * 1.0 + distance);
					result_canvas.Children.Add(image);
				}
			}

			SaveCanvas(this.result_canvas, dpiValue, path);

			//保存後、サイトが画面にあわせる
			paperSizeChange();
		}
		private void paperSizeChange()
		{
			int width = (int)result_canvas.Width;
			int height = (int)result_canvas.Height;

			canvas_sfr.ScaleX = 500.0 / Math.Max(height,width);
			canvas_sfr.ScaleY = 500.0 / Math.Max(height, width);
		}

		
	}
}
