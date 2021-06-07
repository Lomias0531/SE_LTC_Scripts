using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyGameItemsEventArgs : EventArgs
	{
		public List<MyGameInventoryItem> NewItems
		{
			get;
			set;
		}

		public byte[] CheckData
		{
			get;
			set;
		}

		public MyGameItemsEventArgs()
		{
			NewItems = new List<MyGameInventoryItem>();
		}
	}
}
