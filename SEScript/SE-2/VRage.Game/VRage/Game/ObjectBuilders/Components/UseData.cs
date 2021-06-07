using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct UseData
	{
		protected class VRage_Game_ObjectBuilders_Components_UseData_003C_003EUse_003C_003EAccessor : IMemberAccessor<UseData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref UseData owner, in bool value)
			{
				owner.Use = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref UseData owner, out bool value)
			{
				value = owner.Use;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_UseData_003C_003EUseContinues_003C_003EAccessor : IMemberAccessor<UseData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref UseData owner, in bool value)
			{
				owner.UseContinues = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref UseData owner, out bool value)
			{
				value = owner.UseContinues;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_UseData_003C_003EUseFinished_003C_003EAccessor : IMemberAccessor<UseData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref UseData owner, in bool value)
			{
				owner.UseFinished = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref UseData owner, out bool value)
			{
				value = owner.UseFinished;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_UseData_003C_003EActor : IActivator, IActivator<UseData>
		{
			private sealed override object CreateInstance()
			{
				return default(UseData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override UseData CreateInstance()
			{
				return (UseData)(object)default(UseData);
			}

			UseData IActivator<UseData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(55)]
		public bool Use;

		[ProtoMember(58)]
		public bool UseContinues;

		[ProtoMember(61)]
		public bool UseFinished;
	}
}
