using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
	[ProtoContract]
	public class MyFuelConverterInfo
	{
		protected class VRage_Game_MyFuelConverterInfo_003C_003EFuelId_003C_003EAccessor : IMemberAccessor<MyFuelConverterInfo, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyFuelConverterInfo owner, in SerializableDefinitionId value)
			{
				owner.FuelId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyFuelConverterInfo owner, out SerializableDefinitionId value)
			{
				value = owner.FuelId;
			}
		}

		protected class VRage_Game_MyFuelConverterInfo_003C_003EEfficiency_003C_003EAccessor : IMemberAccessor<MyFuelConverterInfo, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyFuelConverterInfo owner, in float value)
			{
				owner.Efficiency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyFuelConverterInfo owner, out float value)
			{
				value = owner.Efficiency;
			}
		}

		private class VRage_Game_MyFuelConverterInfo_003C_003EActor : IActivator, IActivator<MyFuelConverterInfo>
		{
			private sealed override object CreateInstance()
			{
				return new MyFuelConverterInfo();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFuelConverterInfo CreateInstance()
			{
				return new MyFuelConverterInfo();
			}

			MyFuelConverterInfo IActivator<MyFuelConverterInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId FuelId;

		[ProtoMember(4)]
		public float Efficiency = 1f;
	}
}
