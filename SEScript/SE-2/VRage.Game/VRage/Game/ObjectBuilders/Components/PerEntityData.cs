using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public class PerEntityData
	{
		protected class VRage_Game_ObjectBuilders_Components_PerEntityData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<PerEntityData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerEntityData owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerEntityData owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_PerEntityData_003C_003EData_003C_003EAccessor : IMemberAccessor<PerEntityData, SerializableDictionary<int, PerFrameData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PerEntityData owner, in SerializableDictionary<int, PerFrameData> value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PerEntityData owner, out SerializableDictionary<int, PerFrameData> value)
			{
				value = owner.Data;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_PerEntityData_003C_003EActor : IActivator, IActivator<PerEntityData>
		{
			private sealed override object CreateInstance()
			{
				return new PerEntityData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override PerEntityData CreateInstance()
			{
				return new PerEntityData();
			}

			PerEntityData IActivator<PerEntityData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public long EntityId;

		[ProtoMember(7)]
		public SerializableDictionary<int, PerFrameData> Data;
	}
}
