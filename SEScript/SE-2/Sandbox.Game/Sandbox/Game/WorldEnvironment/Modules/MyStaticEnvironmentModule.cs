using Sandbox.Definitions;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyStaticEnvironmentModule : MyEnvironmentModuleBase
	{
		private readonly HashSet<int> m_disabledItems = new HashSet<int>();

		private List<MyOrientedBoundingBoxD> m_boxes;

		private int m_minScannedLod = 15;

		public override void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
		{
			base.Init(sector, ob);
			MyPlanetEnvironmentComponent myPlanetEnvironmentComponent = (MyPlanetEnvironmentComponent)sector.Owner;
			if (myPlanetEnvironmentComponent.CollisionCheckEnabled)
			{
				m_boxes = myPlanetEnvironmentComponent.GetCollidedBoxes(sector.Id);
				if (m_boxes != null)
				{
					m_boxes = new List<MyOrientedBoundingBoxD>(m_boxes);
				}
			}
			MyObjectBuilder_StaticEnvironmentModule myObjectBuilder_StaticEnvironmentModule = (MyObjectBuilder_StaticEnvironmentModule)ob;
			if (myObjectBuilder_StaticEnvironmentModule != null)
			{
				HashSet<int> disabledItems = myObjectBuilder_StaticEnvironmentModule.DisabledItems;
				foreach (int item in disabledItems)
				{
					if (!m_disabledItems.Contains(item))
					{
						OnItemEnable(item, enabled: false);
					}
				}
				m_disabledItems.UnionWith(disabledItems);
				if (myObjectBuilder_StaticEnvironmentModule.Boxes != null && myObjectBuilder_StaticEnvironmentModule.MinScanned > 0)
				{
					m_boxes = new List<MyOrientedBoundingBoxD>();
					foreach (SerializableOrientedBoundingBoxD box in myObjectBuilder_StaticEnvironmentModule.Boxes)
					{
						m_boxes.Add(box);
					}
					m_minScannedLod = myObjectBuilder_StaticEnvironmentModule.MinScanned;
				}
			}
			if (m_boxes != null)
			{
				Vector3D worldPos = sector.WorldPos;
				for (int i = 0; i < m_boxes.Count; i++)
				{
					MyOrientedBoundingBoxD value = m_boxes[i];
					value.Center -= worldPos;
					m_boxes[i] = value;
				}
			}
		}

		public unsafe override void ProcessItems(Dictionary<short, MyLodEnvironmentItemSet> items, int changedLodMin, int changedLodMax)
		{
			m_minScannedLod = changedLodMin;
			using (MyEnvironmentModelUpdateBatch myEnvironmentModelUpdateBatch = new MyEnvironmentModelUpdateBatch(Sector))
			{
				foreach (KeyValuePair<short, MyLodEnvironmentItemSet> item in items)
				{
					Sector.GetItemDefinition((ushort)item.Key, out MyRuntimeEnvironmentItemInfo def);
					MyDefinitionId subtypeId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalModelCollectionDefinition), def.Subtype);
					MyPhysicalModelCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelCollectionDefinition>(subtypeId);
					if (definition != null)
					{
						MyLodEnvironmentItemSet value = item.Value;
						for (int i = value.LodOffsets[changedLodMin]; i < value.Items.Count; i++)
						{
							int num = value.Items[i];
							if (!m_disabledItems.Contains(num) && !IsObstructed(num))
							{
								MyDefinitionId modelDef = definition.Items.Sample(MyHashRandomUtils.UniformFloatFromSeed(num));
								myEnvironmentModelUpdateBatch.Add(modelDef, num);
							}
						}
					}
				}
			}
		}

		private bool IsObstructed(int position)
		{
			if (m_boxes != null)
			{
				Sector.GetItem(position, out ItemInfo item);
				for (int i = 0; i < m_boxes.Count; i++)
				{
					if (m_boxes[i].Contains(ref item.Position))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void Close()
		{
		}

		public override MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder()
		{
			if (m_disabledItems.Count > 0)
			{
				MyObjectBuilder_StaticEnvironmentModule myObjectBuilder_StaticEnvironmentModule = new MyObjectBuilder_StaticEnvironmentModule
				{
					DisabledItems = m_disabledItems,
					MinScanned = m_minScannedLod
				};
				if (m_boxes != null)
				{
					foreach (MyOrientedBoundingBoxD box in m_boxes)
					{
						myObjectBuilder_StaticEnvironmentModule.Boxes.Add(box);
					}
					return myObjectBuilder_StaticEnvironmentModule;
				}
				return myObjectBuilder_StaticEnvironmentModule;
			}
			return null;
		}

		public override void OnItemEnable(int itemId, bool enabled)
		{
			if (enabled)
			{
				m_disabledItems.Remove(itemId);
			}
			else
			{
				m_disabledItems.Add(itemId);
			}
			Sector.GetItem(itemId, out ItemInfo item);
			if (item.ModelIndex >= 0 != enabled)
			{
				short modelId = (short)(~item.ModelIndex);
				Sector.UpdateItemModel(itemId, modelId);
			}
		}

		public override void HandleSyncEvent(int logicalItem, object data, bool fromClient)
		{
		}

		public override void DebugDraw()
		{
			if (m_boxes != null)
			{
				for (int i = 0; i < m_boxes.Count; i++)
				{
					MyOrientedBoundingBoxD obb = m_boxes[i];
					obb.Center += Sector.WorldPos;
					MyRenderProxy.DebugDrawOBB(obb, Color.Aquamarine, 0.3f, depthRead: true, smooth: true);
				}
			}
		}
	}
}
