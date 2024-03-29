using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public class ComponentIndex
	{
		public readonly List<Type> Types;

		public readonly Dictionary<Type, int> Index = new Dictionary<Type, int>();

		public ComponentIndex(List<Type> typeList)
		{
			for (int i = 0; i < typeList.Count; i++)
			{
				Index[typeList[i]] = i;
			}
			Types = typeList;
		}
	}
}
