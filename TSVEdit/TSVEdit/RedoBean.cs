using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVEdit
{
	class RedoBean
	{
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public int RowNumber { get; set; }
		public int ColumnNumber { get; set; }
	}
}
