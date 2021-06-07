using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
	public class MyItemComparer : IComparer<MyGuiControlListbox.Item>
	{
		private Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int> comparator;

		public MyItemComparer(Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int> comp)
		{
			comparator = comp;
		}

		public int Compare(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y)
		{
			if (comparator != null)
			{
				return comparator(x, y);
			}
			return 0;
		}
	}
}
