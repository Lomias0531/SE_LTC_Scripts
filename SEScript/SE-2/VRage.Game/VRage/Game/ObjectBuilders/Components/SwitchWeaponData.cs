using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct SwitchWeaponData
	{
		protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData_003C_003EWeaponDefinition_003C_003EAccessor : IMemberAccessor<SwitchWeaponData, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SwitchWeaponData owner, in SerializableDefinitionId? value)
			{
				owner.WeaponDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SwitchWeaponData owner, out SerializableDefinitionId? value)
			{
				value = owner.WeaponDefinition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData_003C_003EInventoryItemId_003C_003EAccessor : IMemberAccessor<SwitchWeaponData, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SwitchWeaponData owner, in uint? value)
			{
				owner.InventoryItemId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SwitchWeaponData owner, out uint? value)
			{
				value = owner.InventoryItemId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_SwitchWeaponData_003C_003EWeaponEntityId_003C_003EAccessor : IMemberAccessor<SwitchWeaponData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SwitchWeaponData owner, in long value)
			{
				owner.WeaponEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SwitchWeaponData owner, out long value)
			{
				value = owner.WeaponEntityId;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_SwitchWeaponData_003C_003EActor : IActivator, IActivator<SwitchWeaponData>
		{
			private sealed override object CreateInstance()
			{
				return default(SwitchWeaponData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SwitchWeaponData CreateInstance()
			{
				return (SwitchWeaponData)(object)default(SwitchWeaponData);
			}

			SwitchWeaponData IActivator<SwitchWeaponData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(19)]
		public SerializableDefinitionId? WeaponDefinition;

		[ProtoMember(22)]
		public uint? InventoryItemId;

		[ProtoMember(25)]
		public long WeaponEntityId;
	}
}
