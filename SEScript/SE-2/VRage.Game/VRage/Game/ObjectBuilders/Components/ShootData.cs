using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public struct ShootData
	{
		protected class VRage_Game_ObjectBuilders_Components_ShootData_003C_003EBegin_003C_003EAccessor : IMemberAccessor<ShootData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShootData owner, in bool value)
			{
				owner.Begin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShootData owner, out bool value)
			{
				value = owner.Begin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_ShootData_003C_003EShootAction_003C_003EAccessor : IMemberAccessor<ShootData, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShootData owner, in byte value)
			{
				owner.ShootAction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShootData owner, out byte value)
			{
				value = owner.ShootAction;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_ShootData_003C_003EActor : IActivator, IActivator<ShootData>
		{
			private sealed override object CreateInstance()
			{
				return default(ShootData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ShootData CreateInstance()
			{
				return (ShootData)(object)default(ShootData);
			}

			ShootData IActivator<ShootData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(28)]
		public bool Begin;

		[ProtoMember(31)]
		public byte ShootAction;
	}
}
