using System;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
	public class MyItemComparer_Rew : IComparer<MyBlueprintItemInfo>
	{
		private Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int> comparator;

		public MyItemComparer_Rew(Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int> comp)
		{
			comparator = comp;
		}

		public int Compare(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
		{
			if (comparator != null)
			{
				return comparator(x, y);
			}
			return 0;
		}
	}
}
