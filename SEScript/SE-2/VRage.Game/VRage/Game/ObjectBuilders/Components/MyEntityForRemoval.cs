using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public class MyEntityForRemoval
	{
		protected class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval_003C_003ETimeLeft_003C_003EAccessor : IMemberAccessor<MyEntityForRemoval, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEntityForRemoval owner, in int value)
			{
				owner.TimeLeft = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEntityForRemoval owner, out int value)
			{
				value = owner.TimeLeft;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyEntityForRemoval, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyEntityForRemoval owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyEntityForRemoval owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval_003C_003EActor : IActivator, IActivator<MyEntityForRemoval>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityForRemoval();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityForRemoval CreateInstance()
			{
				return new MyEntityForRemoval();
			}

			MyEntityForRemoval IActivator<MyEntityForRemoval>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public int TimeLeft;

		[ProtoMember(10)]
		public long EntityId;

		public MyEntityForRemoval()
		{
		}

		public MyEntityForRemoval(int time, long id)
		{
			TimeLeft = time;
			EntityId = id;
		}
	}
}
