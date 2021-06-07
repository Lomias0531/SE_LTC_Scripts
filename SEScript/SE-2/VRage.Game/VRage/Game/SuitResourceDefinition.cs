using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
	[ProtoContract]
	public class SuitResourceDefinition
	{
		protected class VRage_Game_SuitResourceDefinition_003C_003EId_003C_003EAccessor : IMemberAccessor<SuitResourceDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SuitResourceDefinition owner, in SerializableDefinitionId value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SuitResourceDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_SuitResourceDefinition_003C_003EMaxCapacity_003C_003EAccessor : IMemberAccessor<SuitResourceDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SuitResourceDefinition owner, in float value)
			{
				owner.MaxCapacity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SuitResourceDefinition owner, out float value)
			{
				value = owner.MaxCapacity;
			}
		}

		protected class VRage_Game_SuitResourceDefinition_003C_003EThroughput_003C_003EAccessor : IMemberAccessor<SuitResourceDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SuitResourceDefinition owner, in float value)
			{
				owner.Throughput = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SuitResourceDefinition owner, out float value)
			{
				value = owner.Throughput;
			}
		}

		private class VRage_Game_SuitResourceDefinition_003C_003EActor : IActivator, IActivator<SuitResourceDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new SuitResourceDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SuitResourceDefinition CreateInstance()
			{
				return new SuitResourceDefinition();
			}

			SuitResourceDefinition IActivator<SuitResourceDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(6)]
		public SerializableDefinitionId Id;

		[ProtoMember(7)]
		public float MaxCapacity;

		[ProtoMember(8)]
		public float Throughput;
	}
}
