using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_BehaviorTreeDecoratorNodeMemory : MyObjectBuilder_BehaviorTreeNodeMemory
	{
		[ProtoContract]
		public abstract class LogicMemoryBuilder
		{
		}

		[ProtoContract]
		public class TimerLogicMemoryBuilder : LogicMemoryBuilder
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ETimerLogicMemoryBuilder_003C_003ECurrentTime_003C_003EAccessor : IMemberAccessor<TimerLogicMemoryBuilder, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimerLogicMemoryBuilder owner, in long value)
				{
					owner.CurrentTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimerLogicMemoryBuilder owner, out long value)
				{
					value = owner.CurrentTime;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ETimerLogicMemoryBuilder_003C_003ETimeLimitReached_003C_003EAccessor : IMemberAccessor<TimerLogicMemoryBuilder, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimerLogicMemoryBuilder owner, in bool value)
				{
					owner.TimeLimitReached = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimerLogicMemoryBuilder owner, out bool value)
				{
					value = owner.TimeLimitReached;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ETimerLogicMemoryBuilder_003C_003EActor : IActivator, IActivator<TimerLogicMemoryBuilder>
			{
				private sealed override object CreateInstance()
				{
					return new TimerLogicMemoryBuilder();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override TimerLogicMemoryBuilder CreateInstance()
				{
					return new TimerLogicMemoryBuilder();
				}

				TimerLogicMemoryBuilder IActivator<TimerLogicMemoryBuilder>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long CurrentTime;

			[ProtoMember(4)]
			public bool TimeLimitReached;
		}

		[ProtoContract]
		public class CounterLogicMemoryBuilder : LogicMemoryBuilder
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ECounterLogicMemoryBuilder_003C_003ECurrentCount_003C_003EAccessor : IMemberAccessor<CounterLogicMemoryBuilder, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CounterLogicMemoryBuilder owner, in int value)
				{
					owner.CurrentCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CounterLogicMemoryBuilder owner, out int value)
				{
					value = owner.CurrentCount;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ECounterLogicMemoryBuilder_003C_003EActor : IActivator, IActivator<CounterLogicMemoryBuilder>
			{
				private sealed override object CreateInstance()
				{
					return new CounterLogicMemoryBuilder();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CounterLogicMemoryBuilder CreateInstance()
				{
					return new CounterLogicMemoryBuilder();
				}

				CounterLogicMemoryBuilder IActivator<CounterLogicMemoryBuilder>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(7)]
			public int CurrentCount;
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003EChildState_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyBehaviorTreeState>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in MyBehaviorTreeState value)
			{
				owner.ChildState = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out MyBehaviorTreeState value)
			{
				value = owner.ChildState;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ELogic_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, LogicMemoryBuilder>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in LogicMemoryBuilder value)
			{
				owner.Logic = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out LogicMemoryBuilder value)
			{
				value = owner.Logic;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003EInitCalled_003C_003EAccessor : VRage_Game_MyObjectBuilder_BehaviorTreeNodeMemory_003C_003EInitCalled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_BehaviorTreeNodeMemory*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_BehaviorTreeNodeMemory*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BehaviorTreeDecoratorNodeMemory CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory();
			}

			MyObjectBuilder_BehaviorTreeDecoratorNodeMemory IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlAttribute]
		[ProtoMember(10)]
		[DefaultValue(MyBehaviorTreeState.NOT_TICKED)]
		public MyBehaviorTreeState ChildState;

		[ProtoMember(13)]
		public LogicMemoryBuilder Logic;
	}
}
