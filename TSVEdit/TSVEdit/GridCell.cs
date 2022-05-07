using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVEdit
{
	class GridCell : INotifyPropertyChanged
	{
		private String _name = "";
		private String _rowColumn = "";
		public int RowNum { get; set; } //为了检索机能，增加的 行号
		public int ColumnNum { get; set; } //为了检索机能，增加 列号
		public string name
		{
			set
			{
				_name = value;
				if (PropertyChanged != null)//有改变
				{
					PropertyChanged(this, new PropertyChangedEventArgs("name"));//对Name进行监听
				}
			}
			get
			{
				return _name;
			}
		}
		public string rowColumn
		{
			set
			{
				_rowColumn = value;
				if (PropertyChanged != null)//有改变
				{
					PropertyChanged(this, new PropertyChangedEventArgs("rowColumn"));//对Name进行监听
				}
			}
			get
			{
				return _rowColumn;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
