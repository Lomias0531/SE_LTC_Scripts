using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
	[ProtoContract]
	public struct ToolbarItem
	{
		protected class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EEntityID_003C_003EAccessor : IMemberAccessor<ToolbarItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolbarItem owner, in long value)
			{
				owner.EntityID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolbarItem owner, out long value)
			{
				value = owner.EntityID;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EGroupName_003C_003EAccessor : IMemberAccessor<ToolbarItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolbarItem owner, in string value)
			{
				owner.GroupName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolbarItem owner, out string value)
			{
				value = owner.GroupName;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EAction_003C_003EAccessor : IMemberAccessor<ToolbarItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolbarItem owner, in string value)
			{
				owner.Action = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolbarItem owner, out string value)
			{
				value = owner.Action;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EParameters_003C_003EAccessor : IMemberAccessor<ToolbarItem, List<MyObjectBuilder_ToolbarItemActionParameter>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolbarItem owner, in List<MyObjectBuilder_ToolbarItemActionParameter> value)
			{
				owner.Parameters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolbarItem owner, out List<MyObjectBuilder_ToolbarItemActionParameter> value)
			{
				value = owner.Parameters;
			}
		}

		protected class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EGunId_003C_003EAccessor : IMemberAccessor<ToolbarItem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolbarItem owner, in SerializableDefinitionId? value)
			{
				owner.GunId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolbarItem owner, out SerializableDefinitionId? value)
			{
				value = owner.GunId;
			}
		}

		private class Sandbox_Game_Entities_Blocks_ToolbarItem_003C_003EActor : IActivator, IActivator<ToolbarItem>
		{
			private sealed override object CreateInstance()
			{
				return default(ToolbarItem);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ToolbarItem CreateInstance()
			{
				return (ToolbarItem)(object)default(ToolbarItem);
			}

			ToolbarItem IActivator<ToolbarItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long EntityID;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string GroupName;

		[ProtoMember(7)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Action;

		[ProtoMember(10)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<MyObjectBuilder_ToolbarItemActionParameter> Parameters;

		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableDefinitionId? GunId;

		public static ToolbarItem FromItem(MyToolbarItem item)
		{
			ToolbarItem result = default(ToolbarItem);
			result.EntityID = 0L;
			if (item is MyToolbarItemTerminalBlock)
			{
				MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemTerminalBlock;
				result.EntityID = myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId;
				result.Action = myObjectBuilder_ToolbarItemTerminalBlock._Action;
				result.Parameters = myObjectBuilder_ToolbarItemTerminalBlock.Parameters;
			}
			else if (item is MyToolbarItemTerminalGroup)
			{
				MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemTerminalGroup;
				result.EntityID = myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId;
				result.Action = myObjectBuilder_ToolbarItemTerminalGroup._Action;
				result.GroupName = myObjectBuilder_ToolbarItemTerminalGroup.GroupName;
				result.Parameters = myObjectBuilder_ToolbarItemTerminalGroup.Parameters;
			}
			else if (item is MyToolbarItemWeapon)
			{
				MyObjectBuilder_ToolbarItemWeapon myObjectBuilder_ToolbarItemWeapon = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemWeapon;
				result.GunId = myObjectBuilder_ToolbarItemWeapon.DefinitionId;
			}
			return result;
		}

		public static MyToolbarItem ToItem(ToolbarItem msgItem)
		{
			MyToolbarItem result = null;
			MyCubeBlock entity2;
			if (msgItem.GunId.HasValue)
			{
				MyObjectBuilder_ToolbarItemWeapon myObjectBuilder_ToolbarItemWeapon = MyToolbarItemFactory.WeaponObjectBuilder();
				myObjectBuilder_ToolbarItemWeapon.defId = msgItem.GunId.Value;
				result = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemWeapon);
			}
			else if (string.IsNullOrEmpty(msgItem.GroupName))
			{
				if (MyEntities.TryGetEntityById(msgItem.EntityID, out MyTerminalBlock entity))
				{
					MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
					myObjectBuilder_ToolbarItemTerminalBlock._Action = msgItem.Action;
					myObjectBuilder_ToolbarItemTerminalBlock.Parameters = msgItem.Parameters;
					result = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalBlock);
				}
			}
			else if (MyEntities.TryGetEntityById(msgItem.EntityID, out entity2))
			{
				MyCubeGrid cubeGrid = entity2.CubeGrid;
				string groupName = msgItem.GroupName;
				MyBlockGroup myBlockGroup = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((MyBlockGroup x) => x.Name.ToString() == groupName);
				if (myBlockGroup != null)
				{
					MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(myBlockGroup);
					myObjectBuilder_ToolbarItemTerminalGroup._Action = msgItem.Action;
					myObjectBuilder_ToolbarItemTerminalGroup.Parameters = msgItem.Parameters;
					myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId = msgItem.EntityID;
					result = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalGroup);
				}
			}
			return result;
		}

		public bool ShouldSerializeParameters()
		{
			if (Parameters != null)
			{
				return Parameters.Count > 0;
			}
			return false;
		}
	}
}
