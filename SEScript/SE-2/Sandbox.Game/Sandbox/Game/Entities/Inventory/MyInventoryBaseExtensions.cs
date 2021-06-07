using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
	public static class MyInventoryBaseExtensions
	{
		private static List<MyComponentBase> m_tmpList = new List<MyComponentBase>();

		public static MyInventoryBase GetInventory(this MyEntity entity, MyStringHash inventoryId)
		{
			MyInventoryBase myInventoryBase = null;
			myInventoryBase = entity.Components.Get<MyInventoryBase>();
			if (myInventoryBase != null && inventoryId.Equals(MyStringHash.GetOrCompute(myInventoryBase.InventoryId.ToString())))
			{
				return myInventoryBase;
			}
			if (myInventoryBase is MyInventoryAggregate)
			{
				MyInventoryAggregate aggregate = myInventoryBase as MyInventoryAggregate;
				m_tmpList.Clear();
				aggregate.GetComponentsFlattened(m_tmpList);
				foreach (MyComponentBase tmp in m_tmpList)
				{
					MyInventoryBase myInventoryBase2 = tmp as MyInventoryBase;
					if (inventoryId.Equals(MyStringHash.GetOrCompute(myInventoryBase2.InventoryId.ToString())))
					{
						return myInventoryBase2;
					}
				}
			}
			return null;
		}
	}
}
