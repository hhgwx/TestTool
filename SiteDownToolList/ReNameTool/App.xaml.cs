using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReNameTool
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
	}

	//https://blog.csdn.net/martin_cheng/article/details/41351013
	//让main函数接受参数
	class Program
	{
		public static string inputArgu = "";
		[STAThread]
		public static void Main(string[] args)
		{
			if (args.Length >= 1)
			{
				inputArgu = args[0];
				//MessageBox.Show(inputArgu);
			}

			App app = new App();
			app.InitializeComponent();
			//MainWindow mainWindow = new MainWindow();
			//app.MainWindow = mainWindow;
			app.Run();
		}
	}
}
