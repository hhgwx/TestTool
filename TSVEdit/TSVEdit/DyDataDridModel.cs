﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVEdit
{
	class DyDataDridModel : DynamicObject
	{
		// 用来保存这个动态类型的所有属性;
		// string为属性的名字;
		// object为属性的值（同时也包含了类型）;
		public Dictionary<string, object> Properties = new Dictionary<string, object>();

		// 用来保存中文列名与属性的对应关系;
//		Dictionary<string, string> ColName_Property = new Dictionary<string, string>();

		// 为动态类型动态添加成员;
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (!Properties.Keys.Contains(binder.Name))
			{
				Properties.Add(binder.Name, value);
			}
			return true;
		}

		// 为动态类型动态添加方法;
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			// 可以通过调用方法的手段添加属性;
			if (binder.Name == "AddProperty" && binder.CallInfo.ArgumentCount == 2)
			{
				string name = args[0] as string;
				if (name == null)
				{
					//throw new ArgumentException("name");  
					result = null;
					return false;
				}
				// 向属性列表添加属性及其值;
				object value = args[1];
				Properties.Add(name, value);

				// 添加列名与属性列表的映射关系;
/*				string column_name = args[2] as string;
				ColName_Property.Add(column_name, name);*/

				result = value;
				return true;
			}
/*			if (binder.Name == "JudgePropertyName_StartEditing" && binder.CallInfo.ArgumentCount == 1)
			{
				string columnname = args[0] as string;
				if (columnname == null)
				{
					result = null;
					return false;
				}

				// 在当前列名于属性列表中查找，看是否有匹配项;
				if (ColName_Property.ContainsKey(columnname))
				{
					string key = ColName_Property[columnname];
					if (Properties.ContainsKey(key))
					{
						object property = Properties[key];
					}
				}
				else
				{

				}

			}*/

			return base.TryInvokeMember(binder, args, out result);
		}

		// 获取属性;
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return Properties.TryGetValue(binder.Name, out result);
		}

	}
}
