using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.GameServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
	[StaticEventOwner]
	[MyComponentBuilder(typeof(MyObjectBuilder_AssetModifierComponent), true)]
	public class MyAssetModifierComponent : MyEntityComponentBase
	{
		protected sealed class ResetAssetModifierSync_003C_003ESystem_Int64_0023VRage_GameServices_MyGameInventoryItemSlot : ICallSite<IMyEventOwner, long, MyGameInventoryItemSlot, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in MyGameInventoryItemSlot slot, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ResetAssetModifierSync(entityId, slot);
			}
		}

		protected sealed class ApplyAssetModifierSync_003C_003ESystem_Int64_0023System_Byte_003C_0023_003E_0023System_Boolean : ICallSite<IMyEventOwner, long, byte[], bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in byte[] checkData, in bool addToList, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ApplyAssetModifierSync(entityId, checkData, addToList);
			}
		}

		protected sealed class ApplyAssetModifierSync_003C_003ESystem_Int64_0023System_String_0023System_Boolean : ICallSite<IMyEventOwner, long, string, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in string assetModifierId, in bool addToList, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ApplyAssetModifierSync(entityId, assetModifierId, addToList);
			}
		}

		private class Sandbox_Game_EntityComponents_MyAssetModifierComponent_003C_003EActor : IActivator, IActivator<MyAssetModifierComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyAssetModifierComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAssetModifierComponent CreateInstance()
			{
				return new MyAssetModifierComponent();
			}

			MyAssetModifierComponent IActivator<MyAssetModifierComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyDefinitionId> m_assetModifiers;

		private MySessionComponentAssetModifiers m_sessionComponent;

		[ProtoMember(1, IsRequired = false)]
		public List<MyDefinitionId> AssetModifiers => m_assetModifiers;

		public override string ComponentTypeDebugString => "Asset Modifier Component";

		public MyAssetModifierComponent()
		{
			InitSessionComponent();
		}

		private void InitSessionComponent()
		{
			if (m_sessionComponent == null && MySession.Static != null)
			{
				m_sessionComponent = MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
			}
			if (MyFakes.OWN_ALL_ITEMS)
			{
				MyGameService.AddUnownedItems();
			}
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_AssetModifierComponent myObjectBuilder_AssetModifierComponent = base.Serialize(copy) as MyObjectBuilder_AssetModifierComponent;
			if (m_assetModifiers != null && m_assetModifiers.Count > 0)
			{
				myObjectBuilder_AssetModifierComponent.AssetModifiers = new List<SerializableDefinitionId>();
				{
					foreach (MyDefinitionId assetModifier in m_assetModifiers)
					{
						myObjectBuilder_AssetModifierComponent.AssetModifiers.Add(assetModifier);
					}
					return myObjectBuilder_AssetModifierComponent;
				}
			}
			return myObjectBuilder_AssetModifierComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_AssetModifierComponent myObjectBuilder_AssetModifierComponent = builder as MyObjectBuilder_AssetModifierComponent;
			if (myObjectBuilder_AssetModifierComponent.AssetModifiers != null && myObjectBuilder_AssetModifierComponent.AssetModifiers.Count > 0 && (m_assetModifiers == null || m_assetModifiers.Count == 0))
			{
				m_assetModifiers = new List<MyDefinitionId>();
				foreach (SerializableDefinitionId assetModifier in myObjectBuilder_AssetModifierComponent.AssetModifiers)
				{
					m_assetModifiers.Add(assetModifier);
				}
				InitSessionComponent();
				m_sessionComponent.RegisterComponentForLazyUpdate(this);
			}
		}

		public bool LazyUpdate()
		{
			if (m_assetModifiers != null && base.Entity != null)
			{
				MyEntity entityById = MyEntities.GetEntityById(base.Entity.EntityId);
				if (entityById == null || !entityById.InScene)
				{
					return false;
				}
				foreach (MyDefinitionId assetModifier in m_assetModifiers)
				{
					if (MyGameService.IsActive)
					{
						MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(assetModifier.SubtypeName);
						if (inventoryItemDefinition == null)
						{
							return false;
						}
						_ = inventoryItemDefinition.ItemSlot;
					}
					ApplyAssetModifierSync(base.Entity.EntityId, (byte[])null, addToList: false);
				}
			}
			return true;
		}

		public void ResetSlot(MyGameInventoryItemSlot slot)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ResetAssetModifierSync, base.Entity.EntityId, slot);
		}

		[Event(null, 127)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void ResetAssetModifierSync(long entityId, MyGameInventoryItemSlot slot)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			if (entityById != null && entityById.Components.TryGet(out MyAssetModifierComponent component))
			{
				component.RemoveModifiers(entityById, slot);
			}
		}

		private void RemoveModifiers(MyEntity entity, MyGameInventoryItemSlot slot)
		{
			if (m_assetModifiers == null)
			{
				return;
			}
			int num = 0;
			MyDefinitionId item;
			while (true)
			{
				if (num < m_assetModifiers.Count)
				{
					item = m_assetModifiers[num];
					MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(item.SubtypeName);
					if (inventoryItemDefinition == null || inventoryItemDefinition.ItemSlot == slot)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_assetModifiers.Remove(item);
			num--;
			if (entity.Render != null)
			{
				switch (slot)
				{
				case MyGameInventoryItemSlot.Face:
					SetDefaultTextures(entity, "Astronaut_head");
					break;
				case MyGameInventoryItemSlot.Helmet:
					SetDefaultTextures(entity, "Head");
					SetDefaultTextures(entity, "Astronaut_head");
					SetDefaultTextures(entity, "Spacesuit_hood");
					break;
				case MyGameInventoryItemSlot.Gloves:
					SetDefaultTextures(entity, "LeftGlove");
					SetDefaultTextures(entity, "RightGlove");
					break;
				case MyGameInventoryItemSlot.Boots:
					SetDefaultTextures(entity, "Boots");
					break;
				case MyGameInventoryItemSlot.Suit:
					SetDefaultTextures(entity, "Arms");
					SetDefaultTextures(entity, "RightArm");
					SetDefaultTextures(entity, "Gear");
					SetDefaultTextures(entity, "Cloth");
					SetDefaultTextures(entity, "Emissive");
					SetDefaultTextures(entity, "Backpack");
					break;
				case MyGameInventoryItemSlot.Rifle:
					ResetRifle(entity);
					break;
				case MyGameInventoryItemSlot.Welder:
					ResetWelder(entity);
					break;
				case MyGameInventoryItemSlot.Grinder:
					ResetGrinder(entity);
					break;
				case MyGameInventoryItemSlot.Drill:
					ResetDrill(entity);
					break;
				}
			}
		}

		public static void ResetDrill(MyEntity entity)
		{
			MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "HandDrill");
		}

		public static void ResetGrinder(MyEntity entity)
		{
			MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "AngleGrinder");
		}

		public static void ResetWelder(MyEntity entity)
		{
			MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "Welder");
		}

		public static void ResetRifle(MyEntity entity)
		{
			MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "AutomaticRifle");
		}

		public static void SetDefaultTextures(MyEntity entity, string materialName)
		{
			MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], materialName);
		}

		public bool TryAddAssetModifier(byte[] checkData)
		{
			if (base.Entity == null || base.Entity.Closed || !base.Entity.InScene)
			{
				return false;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ApplyAssetModifierSync, base.Entity.EntityId, checkData, arg4: true);
			return true;
		}

		public bool TryAddAssetModifier(string assetModifierId)
		{
			if (base.Entity == null || base.Entity.Closed || !base.Entity.InScene)
			{
				return false;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ApplyAssetModifierSync, base.Entity.EntityId, assetModifierId, arg4: true);
			return true;
		}

		[Event(null, 252)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void ApplyAssetModifierSync(long entityId, byte[] checkData, bool addToList)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive && checkData != null && checkData != MySessionComponentAssetModifiers.INVALID_CHECK_DATA)
			{
				bool checkResult = false;
				List<MyGameInventoryItem> items = MyGameService.CheckItemData(checkData, out checkResult);
				if (checkResult)
				{
					ApplyAssetModifier(entityId, items, addToList);
				}
			}
		}

		[Event(null, 270)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void ApplyAssetModifierSync(long entityId, string assetModifierId, bool addToList)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
				List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
				MyGameInventoryItem item = MyGameService.InventoryItems.First((MyGameInventoryItem x) => x.ItemDefinition.AssetModifierId == assetModifierId);
				list.Add(item);
				ApplyAssetModifier(entityId, list, addToList);
			}
		}

		private static void ApplyAssetModifier(long entityId, List<MyGameInventoryItem> items, bool addToList)
		{
			foreach (MyGameInventoryItem item in items)
			{
				if (MyGameService.GetInventoryItemDefinition(item.ItemDefinition.AssetModifierId) == null)
				{
					break;
				}
				MyEntity entityById = MyEntities.GetEntityById(entityId);
				MyDefinitionManager.MyAssetModifiers assetModifierDefinitionForRender = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(item.ItemDefinition.AssetModifierId);
				if (entityById != null && assetModifierDefinitionForRender.SkinTextureChanges != null)
				{
					if (addToList && entityById.Components.TryGet(out MyAssetModifierComponent component))
					{
						MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), item.ItemDefinition.AssetModifierId);
						component.AddAssetModifier(id, item.ItemDefinition.ItemSlot);
					}
					if (entityById.Render != null && entityById.Render.RenderObjectIDs[0] != uint.MaxValue)
					{
						MyRenderProxy.ChangeMaterialTexture(entityById.Render.RenderObjectIDs[0], assetModifierDefinitionForRender.SkinTextureChanges);
					}
				}
			}
		}

		private void AddAssetModifier(MyDefinitionId id, MyGameInventoryItemSlot itemSlot)
		{
			if (m_assetModifiers == null)
			{
				m_assetModifiers = new List<MyDefinitionId>();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				for (int i = 0; i < m_assetModifiers.Count; i++)
				{
					MyDefinitionId item = m_assetModifiers[i];
					MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(item.SubtypeName);
					if (inventoryItemDefinition == null)
					{
						m_assetModifiers.Remove(item);
						i--;
					}
					else if (inventoryItemDefinition.ItemSlot == itemSlot)
					{
						m_assetModifiers.Remove(item);
						i--;
					}
				}
			}
			m_assetModifiers.Add(id);
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			m_sessionComponent = null;
		}

		public override bool IsSerialized()
		{
			return true;
		}
	}
}
