using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct PerFrameData
	{
		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EMovementData_003C_003EAccessor : IMemberAccessor<PerFrameData, MovementData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in MovementData? value)
			{
				owner.MovementData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out MovementData? value)
			{
				value = owner.MovementData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003ESwitchWeaponData_003C_003EAccessor : IMemberAccessor<PerFrameData, SwitchWeaponData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in SwitchWeaponData? value)
			{
				owner.SwitchWeaponData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out SwitchWeaponData? value)
			{
				value = owner.SwitchWeaponData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EShootData_003C_003EAccessor : IMemberAccessor<PerFrameData, ShootData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in ShootData? value)
			{
				owner.ShootData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out ShootData? value)
			{
				value = owner.ShootData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EAnimationData_003C_003EAccessor : IMemberAccessor<PerFrameData, AnimationData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in AnimationData? value)
			{
				owner.AnimationData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out AnimationData? value)
			{
				value = owner.AnimationData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EControlSwitchesData_003C_003EAccessor : IMemberAccessor<PerFrameData, ControlSwitchesData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in ControlSwitchesData? value)
			{
				owner.ControlSwitchesData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out ControlSwitchesData? value)
			{
				value = owner.ControlSwitchesData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EUseData_003C_003EAccessor : IMemberAccessor<PerFrameData, UseData?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerFrameData owner, in UseData? value)
			{
				owner.UseData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerFrameData owner, out UseData? value)
			{
				value = owner.UseData;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_PerFrameData_003C_003EActor : IActivator, IActivator<PerFrameData>
		{
			private sealed override object CreateInstance()
			{
				return default(PerFrameData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override PerFrameData CreateInstance()
			{
				return (PerFrameData)(object)default(PerFrameData);
			}

			PerFrameData IActivator<PerFrameData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(64)]
		public MovementData? MovementData;

		[ProtoMember(67)]
		public SwitchWeaponData? SwitchWeaponData;

		[ProtoMember(70)]
		public ShootData? ShootData;

		[ProtoMember(73)]
		public AnimationData? AnimationData;

		[ProtoMember(76)]
		public ControlSwitchesData? ControlSwitchesData;

		[ProtoMember(79)]
		public UseData? UseData;

		public bool ShouldSerializeSwitchWeaponData()
		{
			if (!MyObjectBuilderType.IsValidTypeName(SwitchWeaponData?.WeaponDefinition?.TypeIdString))
			{
				if (!SwitchWeaponData.HasValue)
				{
					return false;
				}
				SwitchWeaponData value = SwitchWeaponData.Value;
				value.WeaponDefinition = null;
				SwitchWeaponData = value;
			}
			return true;
		}

		public override string ToString()
		{
			if (MovementData.HasValue)
			{
				return MovementData.Value.MoveVector.ToString() + "\n" + MovementData.Value.RotateVector.ToString() + "\n" + MovementData.Value.MovementFlags;
			}
			return ((ValueType)this).ToString();
		}
	}
}
