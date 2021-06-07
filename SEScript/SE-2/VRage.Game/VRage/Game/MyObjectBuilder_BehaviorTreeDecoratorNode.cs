using ProtoBuf;
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
	public class MyObjectBuilder_BehaviorTreeDecoratorNode : MyObjectBuilder_BehaviorTreeNode
	{
		[ProtoContract]
		public abstract class Logic
		{
		}

		[ProtoContract]
		public class TimerLogic : Logic
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003ETimerLogic_003C_003ETimeInMs_003C_003EAccessor : IMemberAccessor<TimerLogic, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TimerLogic owner, in long value)
				{
					owner.TimeInMs = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TimerLogic owner, out long value)
				{
					value = owner.TimeInMs;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003ETimerLogic_003C_003EActor : IActivator, IActivator<TimerLogic>
			{
				private sealed override object CreateInstance()
				{
					return new TimerLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override TimerLogic CreateInstance()
				{
					return new TimerLogic();
				}

				TimerLogic IActivator<TimerLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long TimeInMs;
		}

		[ProtoContract]
		public class CounterLogic : Logic
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003ECounterLogic_003C_003ECount_003C_003EAccessor : IMemberAccessor<CounterLogic, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CounterLogic owner, in int value)
				{
					owner.Count = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CounterLogic owner, out int value)
				{
					value = owner.Count;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003ECounterLogic_003C_003EActor : IActivator, IActivator<CounterLogic>
			{
				private sealed override object CreateInstance()
				{
					return new CounterLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CounterLogic CreateInstance()
				{
					return new CounterLogic();
				}

				CounterLogic IActivator<CounterLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(4)]
			public int Count;
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003EBTNode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyObjectBuilder_BehaviorTreeNode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in MyObjectBuilder_BehaviorTreeNode value)
			{
				owner.BTNode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out MyObjectBuilder_BehaviorTreeNode value)
			{
				value = owner.BTNode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003EDecoratorLogic_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, Logic>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in Logic value)
			{
				owner.DecoratorLogic = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out Logic value)
			{
				value = owner.DecoratorLogic;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003EDefaultReturnValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyDecoratorDefaultReturnValues>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in MyDecoratorDefaultReturnValues value)
			{
				owner.DefaultReturnValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out MyDecoratorDefaultReturnValues value)
			{
				value = owner.DefaultReturnValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeDecoratorNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BehaviorTreeDecoratorNode CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeDecoratorNode();
			}

			MyObjectBuilder_BehaviorTreeDecoratorNode IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public MyObjectBuilder_BehaviorTreeNode BTNode;

		[ProtoMember(10)]
		public Logic DecoratorLogic;

		[ProtoMember(13)]
		public MyDecoratorDefaultReturnValues DefaultReturnValue = MyDecoratorDefaultReturnValues.SUCCESS;
	}
}
