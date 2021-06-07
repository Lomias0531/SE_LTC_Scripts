using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectBuilder_SessionComponentMission
	{
		[Serializable]
		[ProtoContract]
		public struct pair
		{
			protected class VRage_Game_MyObjectBuilder_SessionComponentMission_003C_003Epair_003C_003Estm_003C_003EAccessor : IMemberAccessor<pair, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref pair owner, in ulong value)
				{
					owner.stm = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref pair owner, out ulong value)
				{
					value = owner.stm;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SessionComponentMission_003C_003Epair_003C_003Eser_003C_003EAccessor : IMemberAccessor<pair, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref pair owner, in int value)
				{
					owner.ser = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref pair owner, out int value)
				{
					value = owner.ser;
				}
			}

			private class VRage_Game_MyObjectBuilder_SessionComponentMission_003C_003Epair_003C_003EActor : IActivator, IActivator<pair>
			{
				private sealed override object CreateInstance()
				{
					return default(pair);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override pair CreateInstance()
				{
					return (pair)(object)default(pair);
				}

				pair IActivator<pair>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public ulong stm;

			public int ser;

			public pair(ulong p1, int p2)
			{
				stm = p1;
				ser = p2;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionComponentMission_003C_003ETriggers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentMission, SerializableDictionary<pair, MyObjectBuilder_MissionTriggers>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentMission owner, in SerializableDictionary<pair, MyObjectBuilder_MissionTriggers> value)
			{
				owner.Triggers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentMission owner, out SerializableDictionary<pair, MyObjectBuilder_MissionTriggers> value)
			{
				value = owner.Triggers;
			}
		}

		private class VRage_Game_MyObjectBuilder_SessionComponentMission_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentMission>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentMission();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionComponentMission CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentMission();
			}

			MyObjectBuilder_SessionComponentMission IActivator<MyObjectBuilder_SessionComponentMission>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDictionary<pair, MyObjectBuilder_MissionTriggers> Triggers = new SerializableDictionary<pair, MyObjectBuilder_MissionTriggers>();
	}
}
