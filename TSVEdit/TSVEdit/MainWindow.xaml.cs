using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TSVEdit
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		Stack redoBeanStack_z = new Stack(); //ctrl+z 撤销用
		Stack redoBeanStack_y = new Stack(); //ctrl+y 反撤销用

		string filePath = @"C:\Users\ke098437\Desktop\a.csv";
		char fileType = '\t'; //TSV:\t  CSV:,
		ObservableCollection<DyDataDridModel> list = new ObservableCollection<DyDataDridModel>();
		int maxI = 0;

		public MainWindow()
		{
			//MessageBox.Show("Environment.GetCommandLineArgs().Length:" + Environment.GetCommandLineArgs().Length);
			
			for (int i=1;i<Environment.GetCommandLineArgs().Length; i++) {
				if (i == 1)
				{
					filePath = Environment.GetCommandLineArgs()[i];
				}
				else
				{
					filePath = filePath + " " + Environment.GetCommandLineArgs()[i];
				}
			}
			//if (Environment.GetCommandLineArgs().Length >= 2) {
			//	filePath = Environment.GetCommandLineArgs()[1];
			//	MessageBox.Show(filePath);
			//}

			InitializeComponent();

			DataGrid_List.AddHandler(Grid.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.DataGrid_List_MouseLeftButtonDown), true); //解决，Grid的MouseLeftButtonDown事件不相应的问题

			try
			{
				//this.WindowState = System.Windows.WindowState.Maximized;
				//this.WindowStyle = System.Windows.WindowStyle.None;
				init();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void init()
		{
			//this.DataGrid_List.EnableColumnVirtualization = true;
			//this.DataGrid_List.EnableRowVirtualization = true;
			this.listView_View.ItemsSource = searchBeanList;

			// 支持动态添加内容的类型
			dynamic model = new DyDataDridModel();

			if (filePath != "") {
				this.Title = filePath + "     " + this.Title;

				if (filePath.EndsWith("TSV") || filePath.EndsWith("tsv") || filePath.EndsWith("Tsv"))
				{
					fileType = '\t';
				}
				else if (filePath.EndsWith("CSV") || filePath.EndsWith("csv") || filePath.EndsWith("Csv"))
				{
					fileType = ',';
				}
				else
				{
					fileType = ',';
				}

				ReadWriteFile readWriteFile = new ReadWriteFile();
				List<string> lineList = readWriteFile.readFile(filePath,false);

				int lineNum = 0;
				foreach (string lineStr in lineList) {
					model = new DyDataDridModel();
					string[] colStr = lineStr.Split(fileType);
					int i = 0;
					for (i = 0; i < colStr.Length; i++)
					{
						model.AddProperty("property" + i, new GridCell() { name = colStr[i], RowNum= lineNum, ColumnNum= i });
					}
					if (maxI < i)
					{
						maxI = i;
					}
					list.Add(model);
					lineNum++;
				}
			}

			// 定义每一列显示的内容以及Binding的对象
			string columnTitle = "";
			for (int i = 0; i < maxI; i++)
			{
				DataGridTextColumn column = new DataGridTextColumn();
				try
				{
					columnTitle = i + "__" + ((dynamic)list[0]).Properties["property" + i].name;
					if (columnTitle == i + "__")
					{
						column.Header = i + "__" + ((dynamic)list[1]).Properties["property" + i].name;  //title上显示第一行的值
					}
					else
					{
						column.Header = columnTitle;
					}
				} catch (Exception) {
					try
					{
						column.Header = i + "__" + ((dynamic)list[1]).Properties["property" + i].name;  //title上显示第二行的值
					}
					catch (Exception)
					{
						column.Header = i;  //title上显示第一行的值
					}
				}
				column.Binding = new Binding("property" + i + ".name");
				this.DataGrid_List.Columns.Add(column);
			}
			this.DataGrid_List.ItemsSource = list;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
			{
				//DataGridCellInfo cell_Info = this.DataGrid_List.SelectedCells[0];


				e.Handled = true; //中断向下传递
				if (list.Count > 0 && filePath != "")
				{
					string message = "";
					string tempStr = "";
					dynamic model = new DyDataDridModel();

					string errorStr = "";
					MessageBoxResult buttonType = MessageBoxResult.None;
					for (int i = 0; i < list.Count; i++)
					{
						if (i > 0)
						{
							message = message + "\n";
						}

						model = list[i];
						for (int j = 0; j < maxI; j++)
						{
							try
							{
								tempStr = (model.Properties)["property" + j].name;
							}
							catch (Exception ex)
							{
								tempStr = "";
								Console.WriteLine(ex.Message);
								errorStr = errorStr + "最大項目数：" + maxI + ";  " + i + "行目、" + j + "列なし\n";

								if (buttonType == MessageBoxResult.None)
								{
									buttonType = MessageBox.Show(errorStr + "\n続いて保存します。空白列を追加して、保存しますか？", "", MessageBoxButton.YesNoCancel);
								}
								if (buttonType == MessageBoxResult.Cancel)
								{
									return;
								}
								else if (buttonType == MessageBoxResult.No)
								{
									continue;
								}
							}

							if (j == 0)
							{
								message = message + tempStr;
							}
							else
							{
								message = message + fileType + tempStr;
							}

							/*							try
														{
															tempStr = (DataGrid_List.Columns[j].GetCellContent(DataGrid_List.Items[i]) as TextBlock).Text;
														} catch (Exception ex) {
															tempStr = "";
															Console.WriteLine(ex.Message);
														}

														if (j == 0)
														{
															//message = message + (model.Properties)["property" + j];
															message = message + tempStr;
														}
														else
														{
															//message = message + "\t" + (model.Properties)["property" + j];
															message = message + "\t" + tempStr;
														}*/
						}

					}

					ReadWriteFile readWriteFile = new ReadWriteFile();
					readWriteFile.writeFile(filePath, message, false);

					if (errorStr != "")
					{
						if (buttonType == MessageBoxResult.Yes)
						{
							MessageBox.Show(errorStr + "\n上記列が追加されて、保存されました。");
						}
						else if (buttonType == MessageBoxResult.No)
						{
							MessageBox.Show(errorStr + "\n上記列がないままで、保存されました。");
						}
					}
					else
					{
						MessageBox.Show("保存されました。");
					}
				}
			}
		}

		private void TextBox_Filter_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				btn_Filter_Click(null,null);
				e.Handled = true; //中断向下传递
			}
		}
		private void btn_Filter_Click(object sender, RoutedEventArgs e)
		{
			string filterStr = (this.TextBox_Filter.Text).Replace("　", "");
			char[] splitChars = { ',', '，', ';', '；', '、', ' ' };
			if (filterStr == "")
			{
				foreach (DataGridTextColumn column in this.DataGrid_List.Columns) {
					column.Visibility = Visibility.Visible;
				}
			}
			else
			{
				if (list.Count == 0)
				{
					return;
				}

				if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault()) {
					filterStr = filterStr.ToLower();
				}
				string[] filterColumns = filterStr.Split(splitChars);

				dynamic model0 = new DyDataDridModel(); //第一行
				dynamic model1 = new DyDataDridModel(); //第二行（一部分数据的头在第二行）
				model0 = list[0];
				if (list.Count > 1) {
					model1 = list[1];
				}
				
				string tempListColumn = "";
				for (int j = 0; j < maxI; j++)
				{
					this.DataGrid_List.Columns[j].Visibility = Visibility.Hidden;
					try
					{
						if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
						{
							tempListColumn = (model0.Properties)["property" + j].name.ToLower();
						}
						else
						{
							tempListColumn = (model0.Properties)["property" + j].name;
						}
						
					}
					catch (Exception)
					{
						tempListColumn = "";
					}

					if (tempListColumn == "")
					{
						try
						{
							if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
							{
								tempListColumn = (model1.Properties)["property" + j].name.ToLower();
							}
							else
							{
								tempListColumn = (model1.Properties)["property" + j].name;
							}
						}
						catch (Exception)
						{
							tempListColumn = "";
						}
					}

					if (tempListColumn != "") {
						foreach (string tempFilterColumn in filterColumns) {
							if (tempFilterColumn != "" && tempListColumn.IndexOf(tempFilterColumn) >= 0) {
								this.DataGrid_List.Columns[j].Visibility = Visibility.Visible;
								break;
							}
						}
					}
					
				}
			}
		}

		private void TextBox_Search_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				btn_Search_Click(null, null);
				e.Handled = true; //中断向下传递
			}
		}

//		private int nowI = 0;
//		private int nowJ = 0;
		ObservableCollection<GridCell> searchBeanList = new ObservableCollection<GridCell>();
		private void btn_Search_Click(object sender, RoutedEventArgs e)
		{
			string searchStr = (this.TextBox_Search.Text).Replace(" ", "").Replace("　", "");

			searchBeanList.Clear();
			if (searchStr == "") {
				return;
			}

			if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
			{
				searchStr = searchStr.ToLower();
			}

			GridCell gridCell = new GridCell();

			if (list.Count > 0)
			{
				string tempStr = "";
				dynamic model = new DyDataDridModel();

				for (int i = 0; i < list.Count; i++)
				{
					model = list[i];
					for (int j = 0; j < maxI; j++)
					{
						try
						{
							if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
							{
								tempStr = (model.Properties)["property" + j].name.ToLower();
							}
							else
							{
								tempStr = (model.Properties)["property" + j].name;
							}
						}
						catch (Exception)
						{
							tempStr = "";
							continue;
						}

						if (tempStr.IndexOf(searchStr) >= 0)
						{
							gridCell = new GridCell();
							gridCell.RowNum = i;
							gridCell.ColumnNum = j;
							gridCell.name = tempStr;
							gridCell.rowColumn = i + "," + j;
							searchBeanList.Add(gridCell);
						}
						else
						{
						}
					}
				}

				if (searchBeanList != null && searchBeanList.Count > 0 && this.grid_Col3.Width.Value == 0.0)
				{
					this.DataGrid_List.Width = this.ActualWidth - 10 - this.grid_Col2.Width.Value - this.listView_View.Width;
					this.grid_Col3.Width = new GridLength(listView_View.Width);
					this.btn_left.Content = ">";
				}
				else if (searchBeanList != null && searchBeanList.Count > 0) {
					;
				}
				else
				{
					MessageBox.Show("検索結果なし。");
				}
			}
		}

		private void CheckBox_LineOrCell_Checked(object sender, RoutedEventArgs e)
		{
			this.DataGrid_List.SelectionUnit = DataGridSelectionUnit.FullRow;
		}

		private void CheckBox_LineOrCell_Unchecked(object sender, RoutedEventArgs e)
		{
			this.DataGrid_List.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
		}

		//事件  :编辑单元格后，将其保存在Ctrl+z的堆栈里
		private string editBeforeStr = "";
		private void DataGrid_List_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
		{
			//editBeforeStr = (e.Column.GetCellContent(e.Row) as TextBlock).Text;

			//最后一行等，文件中缺失的Cell操作时，要在List中新规出来
			int rowNumber = int.Parse(e.Row.Header.ToString());
			int ColumnNumber = int.Parse(e.Column.Header.ToString().Split('_')[0]);
			dynamic model = null;

			if (rowNumber >= list.Count)
			{
				//行不存在新规行
				model = new DyDataDridModel();
				list.Add(model);
			}
			else
			{
				model = list[rowNumber];
			}
			if (!model.Properties.ContainsKey("property" + ColumnNumber))
			{
				model.AddProperty("property" + ColumnNumber, new GridCell() { name = "", RowNum = rowNumber, ColumnNum = ColumnNumber });
			}


			//dynamic model = new DyDataDridModel();
			//model = list[int.Parse(e.Row.Header.ToString())];
			try
			{
				editBeforeStr = (model.Properties)["property" + ColumnNumber].name;
			}
			catch (Exception)
			{
				editBeforeStr = "";
			}
			//Console.WriteLine("Staring :" + editBeforeStr);
		}
		private void DataGrid_List_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			string newValue = (e.EditingElement as TextBox).Text;
			//Console.WriteLine("After :" + newValue);
			if (newValue != editBeforeStr) {
				//int rowNumber = e.Row.GetIndex();
				//int ColumnNumber = e.Column.DisplayIndex;

				//changeCellColor(rowNumber, ColumnNumber, Colors.Red);
				List<RedoBean> redoBeanList = new List<RedoBean>();
				RedoBean redoBean = new RedoBean();
				redoBean.OldValue = editBeforeStr;
				redoBean.NewValue = newValue;
				redoBean.RowNumber = e.Row.GetIndex();
				redoBean.ColumnNumber = e.Column.DisplayIndex;
				redoBeanList.Add(redoBean);

				redoBeanStack_z.Push(redoBeanList);
				redoBeanStack_y.Clear();
			}
		}

		private static void DoEvents()
		{
			var frame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
				new DispatcherOperationCallback(ExitFrames), frame);
			Dispatcher.PushFrame(frame);
		}
		private static object ExitFrames(object f)
		{
			((DispatcherFrame)f).Continue = false;
			return null;
		}
		//改变单元格颜色

		List<DataGridCell> oldDataGridCellList = new List<DataGridCell>();
		/*****************
		 * rowIndex:要改变的行号
		 * colIndex:要改变的列号
		 * color:要改变的颜色
		 * toValue:ctrl+z ctrl+y 复原时用的值。null时只改变颜色
		 *****************/
		public void changeCellColor(int rowIndex, int colIndex, Color color, string toValue)
		{
			DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(DataGrid_List.Items[rowIndex], DataGrid_List.Columns[colIndex]);

			DataGrid_List.Focus();
			//DataGrid_List.SelectedIndex = rowIndex;
			DataGrid_List.CurrentCell = dataGridCellInfo;

			DataGrid_List.ScrollIntoView(dataGridCellInfo.Item);
			//DoEvents();
			FrameworkElement contentElement = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
			if (contentElement == null)
			{
				return;
			}
			var dataGridCell = contentElement.Parent as DataGridCell;
			if (dataGridCell == null)
			{
				return;
			}
			dataGridCell.Background = new SolidColorBrush(color);
			dataGridCell.Foreground = new SolidColorBrush(Colors.Black);
			dataGridCell.Focus();
			oldDataGridCellList.Add(dataGridCell);

			dynamic model = new DyDataDridModel();
			model = list[rowIndex];

			if (toValue != null) {
				(model.Properties)["property" + colIndex].name = toValue;
			}

			//						(DataGrid_List.Columns[colIndex].GetCellContent(DataGrid_List.Items[rowIndex]) as TextBox).Focus();
			/*		try
					{
						Console.WriteLine("rowIndex:" + rowIndex + "   colIndex:" + colIndex);
						DataGridRow row = (DataGridRow)DataGrid_List.ItemContainerGenerator.ContainerFromIndex(rowIndex);//获取选中单元格所在行
						DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);//函数调用，获取行中所有单元格的集合
						DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(colIndex);//锁定选中单元格（重点）
						if (cell != null)
						{
							DataGrid_List.ScrollIntoView(row, DataGrid_List.Columns[colIndex]);
							cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(colIndex);
							cell.Focus();
							DataGrid_List.ScrollIntoView(cell);
							//				cell.Background = new SolidColorBrush(color);//OK!问题解决，单元格变色
							//				cell.Foreground = new SolidColorBrush(Colors.Black);//OK!问题解决，单元格变色
							//				Console.WriteLine("rowIndex:" + rowIndex + "  colIndex:" + colIndex);
						}
					} catch (Exception) {

					}*/
		}

	//获取行中所有单元格集合的函数
	public static T GetVisualChild<T> (Visual parent) where T : Visual
		{
			try
			{
				T childContent = default(T);
				int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
				for (int i = 0; i < numVisuals; i++)
				{
					Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
					childContent = v as T;
					if (childContent == null)
					{
						childContent = GetVisualChild<T>(v);
					}
					if (childContent != null)
					{ break; }
				}
				return childContent;
			} catch (Exception ex) {
				throw ex;
			}

		}

		private void DataGrid_List_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex()).ToString();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//MessageBox.Show("this.ActualHeight:" + this.ActualHeight + "    this.ActualWidth:" + this.ActualWidth);

			DataGrid_List.Height = this.ActualHeight - 45 - DataGrid_List.ColumnHeaderHeight;
			listView_View.Height = DataGrid_List.Height;
			btn_left.Height = DataGrid_List.Height;
			DataGrid_List.Width = this.ActualWidth - 10 - this.grid_Col2.Width.Value;
		}

		private void listView_View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

		}
		private void listView_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GridCell gridCell = (sender as ListView).SelectedItem as GridCell;

			if (gridCell == null) return;

			foreach (DataGridCell dataGridCellTemp in oldDataGridCellList)
			{
				dataGridCellTemp.Background = new SolidColorBrush(Colors.White);
			}
			oldDataGridCellList.Clear();

			changeCellColor(gridCell.RowNum, gridCell.ColumnNum, Colors.YellowGreen, null);
		}
		//>< 按钮的点击
		private void btn_left_Click(object sender, RoutedEventArgs e)
		{
			if (!moveFlag)
			{
				if (this.btn_left.Content.ToString() == "<")
				{
					this.DataGrid_List.Width = this.ActualWidth - 10 - this.grid_Col2.Width.Value - this.listView_View.Width;
					this.grid_Col3.Width = new GridLength(listView_View.Width);
					this.btn_left.Content = ">";
				}
				else
				{
					this.DataGrid_List.Width = this.ActualWidth - 10 - this.grid_Col2.Width.Value;
					this.grid_Col3.Width = new GridLength(0);
					this.btn_left.Content = "<";
				}
			}
			moveFlag = false;
		}

		//>< 按钮的拖动
		Boolean pushFlag = false; //按下，拖动判断用
		Boolean moveFlag = false; //发生拖动时，置为True，防止触发点击事件。
		double oldX = 0;
		private void btn_left_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			pushFlag = true;
			moveFlag = false;
			oldX = e.GetPosition(null).X;
		}
		private void btn_left_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			pushFlag = false;
		}
		private void btn_left_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (oldX != e.GetPosition(null).X && pushFlag)
			{
				this.DataGrid_List.Width = this.DataGrid_List.Width - (oldX - e.GetPosition(null).X);
				this.grid_Col3.Width = new GridLength(this.grid_Col3.Width.Value + (oldX - e.GetPosition(null).X));
				this.listView_View.Width = this.grid_Col3.Width.Value;
				if (this.listView_View.Width > 100) {
					this.listColumn2.Width = this.listView_View.Width - this.listColumn1.Width;
				}

				oldX = e.GetPosition(null).X;

				moveFlag = true;

				this.btn_left.Content = ">";
			}
		}

		private void DataGrid_List_KeyDown(object sender, KeyEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Z)
			{
				//撤销
				if (redoBeanStack_z.Count > 0)
				{
					foreach (DataGridCell dataGridCellTemp in oldDataGridCellList)
					{
						dataGridCellTemp.Background = new SolidColorBrush(Colors.White);
					}
					oldDataGridCellList.Clear();

					List<RedoBean> redoBeanList = (List<RedoBean>)redoBeanStack_z.Pop();
					foreach (RedoBean redoBean in redoBeanList)
					{
						changeCellColor(redoBean.RowNumber, redoBean.ColumnNumber, Colors.Yellow, redoBean.OldValue);
					}
					redoBeanStack_y.Push(redoBeanList);
				}
				else
				{
					MessageBox.Show("最初状態に戻りました。これ以上戻れません。");
				}
				e.Handled = true; //中断向下传递

			}
			else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Y)
			{
				//反撤销
				if (redoBeanStack_y.Count > 0)
				{
					//RedoBean redoBean = (RedoBean)redoBeanStack_y.Pop();

					foreach (DataGridCell dataGridCellTemp in oldDataGridCellList)
					{
						dataGridCellTemp.Background = new SolidColorBrush(Colors.White);
					}
					oldDataGridCellList.Clear();

					List<RedoBean> redoBeanList = (List<RedoBean>)redoBeanStack_y.Pop();
					foreach (RedoBean redoBean in redoBeanList)
					{
						changeCellColor(redoBean.RowNumber, redoBean.ColumnNumber, Colors.Blue, redoBean.NewValue);
					}
					redoBeanStack_z.Push(redoBeanList);
				}
				else
				{
					MessageBox.Show("最後修正後状態になりました。");
				}
				e.Handled = true; //中断向下传递
			} else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
			{
				IDataObject data = Clipboard.GetDataObject();
				//MessageBox.Show((string)data.GetData(typeof(string)));
				if (data.GetDataPresent(typeof(string))) {
					e.Handled = true; //中断向下传递
					string strTemp = ((string)data.GetData(typeof(string)));

					//string[] clipboardList = strTemp.Split('\t');
					//if (clipboardList.Length > 0 && (clipboardList.Length > 1 || clipboardList[0]!=""))
					//{

					//}

					if (DataGrid_List.SelectedIndex >= 0)
					{   //行粘贴
						//TODO
						parseStartCol = 0;
						parseEndCol = maxI;
					}
					//else
					{   //单元格粘贴（复数个可能）
						/*if (DataGrid_List.SelectedCells.Count != 1)
						{
							MessageBox.Show("貼り付け開始Cellを選択してください。（一つCellです）");
						}
						else
						{
							DataGridCellInfo cell_Info = this.DataGrid_List.SelectedCells[0];
						}*/
						//int[] rows = DataGrid_List.SelectedCells.OfType<DyDataDridModel>().Select(i => DataGrid_List.Items.IndexOf(i)).ToArray();

						//int row = DataGrid_List.Items.IndexOf(DataGrid_List.CurrentItem);
						//MessageBox.Show(DataGrid_List.Columns.IndexOf(DataGrid_List.CurrentColumn) + "  " + row);
						if (strTemp.EndsWith("\r\n")) {
							strTemp = strTemp.Substring(0,strTemp.Length - 2);
						}
						string[] copyedRows = strTemp.Replace("\r\n","\n").Split('\n');
						List<string[]> copyedArr = new List<string[]>();

						for (int i=0; i< copyedRows.Length; i++) {
							string[] copyedColsTemp = copyedRows[i].Split('\t');
							copyedArr.Add(copyedColsTemp);
						}

						foreach (DataGridCell dataGridCellTemp in oldDataGridCellList)
						{
							dataGridCellTemp.Background = new SolidColorBrush(Colors.White);
						}
						oldDataGridCellList.Clear();

						if ((parseEndRow - parseStartRow + 1) % copyedRows.Length == 0 && (parseEndCol - parseStartCol + 1) % copyedArr[0].Length == 0)
						{
							//选择内容全parse

							List<RedoBean> redoBeanList = new List<RedoBean>();
							for (int i = 0; i < parseEndRow - parseStartRow + 1; i++)
							{
								for (int j = 0; j < parseEndCol - parseStartCol + 1; j++)
								{
									RedoBean redoBean = new RedoBean();

									dynamic model = new DyDataDridModel();
									model = list[parseStartRow + i];
									redoBean.OldValue = (model.Properties)["property" + (parseStartCol + j)].name;
									redoBean.NewValue = copyedArr[i%copyedRows.Length][j% copyedArr[0].Length];
									redoBean.RowNumber = parseStartRow + i;
									redoBean.ColumnNumber = parseStartCol + j;
									redoBeanList.Add(redoBean);

									changeCellColor(redoBean.RowNumber, redoBean.ColumnNumber, Colors.Yellow, redoBean.NewValue);
								}
							}
							redoBeanStack_z.Push(redoBeanList);
						}
						else
						{
							//结果贴一次
							List<RedoBean> redoBeanList = new List<RedoBean>();
							for (int i = 0; i < copyedArr.Count; i++)
							{
								for (int j = 0; j < copyedArr[0].Length; j++)
								{
									RedoBean redoBean = new RedoBean();

									dynamic model = new DyDataDridModel();
									model = list[parseStartRow + i];
									redoBean.OldValue = (model.Properties)["property" + (parseStartCol + j)].name;
									redoBean.NewValue = copyedArr[i][j];
									redoBean.RowNumber = parseStartRow + i;
									redoBean.ColumnNumber = parseStartCol + j;
									redoBeanList.Add(redoBean);

									changeCellColor(redoBean.RowNumber, redoBean.ColumnNumber, Colors.Yellow, redoBean.NewValue);
								}
							}
							redoBeanStack_z.Push(redoBeanList);
						}

					}
				}
			}
		}

		//parse 貼り付け　用エリアを取得
		int parseStartRow = -1;
		int parseStartCol = -1;
		int parseEndRow = -1;
		int parseEndCol = -1;
		private void DataGrid_List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void DataGrid_List_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{




		}

		private void DataGrid_List_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			parseStartRow = DataGrid_List.Items.IndexOf(DataGrid_List.CurrentItem);
			parseStartCol = DataGrid_List.Columns.IndexOf(DataGrid_List.CurrentColumn);
			//Console.WriteLine("parseStartRow" + parseStartRow);
			//Console.WriteLine("parseStartCol" + parseStartCol);
		}

		private void DataGrid_List_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			parseEndRow = DataGrid_List.Items.IndexOf(DataGrid_List.CurrentItem);
			parseEndCol = DataGrid_List.Columns.IndexOf(DataGrid_List.CurrentColumn);


			//Console.WriteLine("parseEndRow" + parseEndRow);
			//Console.WriteLine("parseEndCol" + parseEndCol);

			int tempInt;
			if (parseEndRow < parseStartRow)
			{
				tempInt = parseStartRow;
				parseStartRow = parseEndRow;
				parseEndRow = tempInt;
			}
			if (parseEndCol < parseStartCol)
			{
				tempInt = parseStartCol;
				parseStartCol = parseEndCol;
				parseEndCol = tempInt;
			}
		}

		private void btn_Replace_Click(object sender, RoutedEventArgs e)
		{
			string strBefore = this.TextBox_RepalceBefore.Text;
			string strAfter = this.TextBox_RepalceAfter.Text;

			if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
			{
				strBefore = strBefore.ToLower();
			}

			if (strBefore == "") {
				MessageBox.Show("置換元が空白できません。");
				return;
			}

			if (list.Count > 0)
			{
				GridCell gridCell; 

				string tempStr = "";
				dynamic model = new DyDataDridModel();

				searchBeanList.Clear();

				for (int i = 0; i < list.Count; i++)
				{
					model = list[i];
					for (int j = 0; j < maxI; j++)
					{
						try
						{
							if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
							{
								tempStr = (model.Properties)["property" + j].name.ToLower();
							}
							else
							{
								tempStr = (model.Properties)["property" + j].name;
							}
						}
						catch (Exception)
						{
							tempStr = "";
							continue;
						}

						if (tempStr.IndexOf(strBefore) >= 0)
						{
							gridCell = new GridCell();
							gridCell.RowNum = i;
							gridCell.ColumnNum = j;
							gridCell.name = tempStr;
							gridCell.rowColumn = i + "," + j;
							searchBeanList.Add(gridCell);

							if (!this.CheckBox_BigSmall.IsChecked.GetValueOrDefault())
							{

								(model.Properties)["property" + j].name = Strings.Replace((model.Properties)["property" + j].name, strBefore, strAfter, 1, -1, CompareMethod.Text);
							}
							else
							{
								(model.Properties)["property" + j].name = (model.Properties)["property" + j].name.Replace(strBefore, strAfter);
							}
						}
					}
				}

				if (searchBeanList != null && searchBeanList.Count > 0 && this.grid_Col3.Width.Value == 0.0)
				{
					this.DataGrid_List.Width = this.ActualWidth - 10 - this.grid_Col2.Width.Value - this.listView_View.Width;
					this.grid_Col3.Width = new GridLength(listView_View.Width);
				}
			}
		}

		private void btn_Help_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Ctrl+S:保存\nCtrl+C:コピー\nCtrl+V:貼り付け\nCtrl+Z:戻す\nCtrl+Y:逆戻す\n\n拖拽TSV/CSV进来打开文件\n推荐直接把文件打开的关联方式改为这个工具");
		}

		private void Grid_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effects = DragDropEffects.Link;
			}
			else
				e.Effects = DragDropEffects.None;
		}

		private void Grid_Drop(object sender, DragEventArgs e)
		{
			filePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
			try
			{
				//this.WindowState = System.Windows.WindowState.Maximized;
				//this.WindowStyle = System.Windows.WindowStyle.None;
				list.Clear();
				maxI = 0;
				this.DataGrid_List.Columns.Clear();
				init();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
