using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct ControlSwitchesData
	{
		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchThrusts_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchThrusts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchThrusts;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchDamping_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchDamping = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchDamping;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchLights_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchLights = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchLights;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchLandingGears_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchLandingGears = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchLandingGears;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchReactors_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchReactors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchReactors;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003ESwitchHelmet_003C_003EAccessor : IMemberAccessor<ControlSwitchesData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ControlSwitchesData owner, in bool value)
			{
				owner.SwitchHelmet = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ControlSwitchesData owner, out bool value)
			{
				value = owner.SwitchHelmet;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_ControlSwitchesData_003C_003EActor : IActivator, IActivator<ControlSwitchesData>
		{
			private sealed override object CreateInstance()
			{
				return default(ControlSwitchesData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ControlSwitchesData CreateInstance()
			{
				return (ControlSwitchesData)(object)default(ControlSwitchesData);
			}

			ControlSwitchesData IActivator<ControlSwitchesData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(37)]
		public bool SwitchThrusts;

		[ProtoMember(40)]
		public bool SwitchDamping;

		[ProtoMember(43)]
		public bool SwitchLights;

		[ProtoMember(46)]
		public bool SwitchLandingGears;

		[ProtoMember(49)]
		public bool SwitchReactors;

		[ProtoMember(52)]
		public bool SwitchHelmet;
	}
}
