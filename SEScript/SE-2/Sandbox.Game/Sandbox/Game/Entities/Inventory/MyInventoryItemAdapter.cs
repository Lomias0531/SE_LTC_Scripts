using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
	public class MyInventoryItemAdapter : IMyInventoryItemAdapter
	{
		[ThreadStatic]
		private static MyInventoryItemAdapter m_static = new MyInventoryItemAdapter();

		private MyPhysicalItemDefinition m_physItem;

		private MyCubeBlockDefinition m_blockDef;

		public static MyInventoryItemAdapter Static
		{
			get
			{
				if (m_static == null)
				{
					m_static = new MyInventoryItemAdapter();
				}
				return m_static;
			}
		}

		public float Mass
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.Mass;
				}
				if (m_blockDef != null)
				{
					if (MyDestructionData.Static != null && Sync.IsServer)
					{
						return MyDestructionHelper.MassFromHavok(MyDestructionData.Static.GetBlockMass(m_blockDef.Model, m_blockDef));
					}
					return m_blockDef.Mass;
				}
				return 0f;
			}
		}

		public float Volume
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.Volume;
				}
				if (m_blockDef != null)
				{
					float cubeSize = MyDefinitionManager.Static.GetCubeSize(m_blockDef.CubeSize);
					return (float)m_blockDef.Size.Size * cubeSize * cubeSize * cubeSize;
				}
				return 0f;
			}
		}

		public bool HasIntegralAmounts
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.HasIntegralAmounts;
				}
				if (m_blockDef != null)
				{
					return true;
				}
				return false;
			}
		}

		public MyFixedPoint MaxStackAmount
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.MaxStackAmount;
				}
				if (m_blockDef != null)
				{
					return 1;
				}
				return MyFixedPoint.MaxValue;
			}
		}

		public string DisplayNameText
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.DisplayNameText;
				}
				if (m_blockDef != null)
				{
					return m_blockDef.DisplayNameText;
				}
				return "";
			}
		}

		public string[] Icons
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.Icons;
				}
				if (m_blockDef != null)
				{
					return m_blockDef.Icons;
				}
				return new string[1]
				{
					""
				};
			}
		}

		public MyStringId? IconSymbol
		{
			get
			{
				if (m_physItem != null)
				{
					return m_physItem.IconSymbol;
				}
				_ = m_blockDef;
				return null;
			}
		}

		public void Adapt(IMyInventoryItem inventoryItem)
		{
			m_physItem = null;
			m_blockDef = null;
			MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = inventoryItem.Content as MyObjectBuilder_PhysicalObject;
			if (myObjectBuilder_PhysicalObject != null)
			{
				Adapt(myObjectBuilder_PhysicalObject.GetObjectId());
			}
			else
			{
				Adapt(inventoryItem.GetDefinitionId());
			}
		}

		public void Adapt(MyDefinitionId itemDefinition)
		{
			if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(itemDefinition, out m_physItem))
			{
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(itemDefinition, out m_blockDef);
			}
		}

		public bool TryAdapt(MyDefinitionId itemDefinition)
		{
			m_physItem = null;
			m_blockDef = null;
			if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(itemDefinition, out m_physItem))
			{
				return true;
			}
			if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(itemDefinition, out m_blockDef))
			{
				return true;
			}
			return false;
		}
	}
}
