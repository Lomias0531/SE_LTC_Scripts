using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyRadialMenuSection
	{
		public List<MyRadialMenuItem> Items;

		public MyStringId Label;

		public MyRadialMenuSection()
		{
		}

		public MyRadialMenuSection(List<MyRadialMenuItem> items, MyStringId label)
		{
			Items = items;
			Label = label;
		}

		public void Init(MyObjectBuilder_RadialMenuSection builder)
		{
			Label = builder.Label;
			Items = new List<MyRadialMenuItem>();
			MyObjectBuilder_RadialMenuItem[] items = builder.Items;
			for (int i = 0; i < items.Length; i++)
			{
				MyRadialMenuItem item = MyRadialMenuItemFactory.CreateRadialMenuItem(items[i]);
				Items.Add(item);
			}
		}
	}
}
