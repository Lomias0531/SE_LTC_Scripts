using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_BotMemory : MyObjectBuilder_Base
	{
		[ProtoContract]
		public class BehaviorTreeBlackboardMemory
		{
			protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeBlackboardMemory_003C_003EMemberName_003C_003EAccessor : IMemberAccessor<BehaviorTreeBlackboardMemory, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BehaviorTreeBlackboardMemory owner, in string value)
				{
					owner.MemberName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BehaviorTreeBlackboardMemory owner, out string value)
				{
					value = owner.MemberName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeBlackboardMemory_003C_003EValue_003C_003EAccessor : IMemberAccessor<BehaviorTreeBlackboardMemory, MyBBMemoryValue>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BehaviorTreeBlackboardMemory owner, in MyBBMemoryValue value)
				{
					owner.Value = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BehaviorTreeBlackboardMemory owner, out MyBBMemoryValue value)
				{
					value = owner.Value;
				}
			}

			private class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeBlackboardMemory_003C_003EActor : IActivator, IActivator<BehaviorTreeBlackboardMemory>
			{
				private sealed override object CreateInstance()
				{
					return new BehaviorTreeBlackboardMemory();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BehaviorTreeBlackboardMemory CreateInstance()
				{
					return new BehaviorTreeBlackboardMemory();
				}

				BehaviorTreeBlackboardMemory IActivator<BehaviorTreeBlackboardMemory>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string MemberName;

			[ProtoMember(4)]
			[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyBBMemoryValue>))]
			public MyBBMemoryValue Value;
		}

		[ProtoContract]
		public class BehaviorTreeNodesMemory
		{
			protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeNodesMemory_003C_003EBehaviorName_003C_003EAccessor : IMemberAccessor<BehaviorTreeNodesMemory, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BehaviorTreeNodesMemory owner, in string value)
				{
					owner.BehaviorName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BehaviorTreeNodesMemory owner, out string value)
				{
					value = owner.BehaviorName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeNodesMemory_003C_003EMemory_003C_003EAccessor : IMemberAccessor<BehaviorTreeNodesMemory, List<MyObjectBuilder_BehaviorTreeNodeMemory>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BehaviorTreeNodesMemory owner, in List<MyObjectBuilder_BehaviorTreeNodeMemory> value)
				{
					owner.Memory = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BehaviorTreeNodesMemory owner, out List<MyObjectBuilder_BehaviorTreeNodeMemory> value)
				{
					value = owner.Memory;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeNodesMemory_003C_003EBlackboardMemory_003C_003EAccessor : IMemberAccessor<BehaviorTreeNodesMemory, List<BehaviorTreeBlackboardMemory>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BehaviorTreeNodesMemory owner, in List<BehaviorTreeBlackboardMemory> value)
				{
					owner.BlackboardMemory = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BehaviorTreeNodesMemory owner, out List<BehaviorTreeBlackboardMemory> value)
				{
					value = owner.BlackboardMemory;
				}
			}

			private class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeNodesMemory_003C_003EActor : IActivator, IActivator<BehaviorTreeNodesMemory>
			{
				private sealed override object CreateInstance()
				{
					return new BehaviorTreeNodesMemory();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BehaviorTreeNodesMemory CreateInstance()
				{
					return new BehaviorTreeNodesMemory();
				}

				BehaviorTreeNodesMemory IActivator<BehaviorTreeNodesMemory>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(7)]
			public string BehaviorName;

			[DynamicNullableObjectBuilderItem(false)]
			[ProtoMember(10)]
			[XmlArrayItem("Node", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BehaviorTreeNodeMemory>))]
			public List<MyObjectBuilder_BehaviorTreeNodeMemory> Memory;

			[XmlArrayItem("BBMem")]
			[ProtoMember(13)]
			public List<BehaviorTreeBlackboardMemory> BlackboardMemory;
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EBehaviorTreeMemory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, BehaviorTreeNodesMemory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BotMemory owner, in BehaviorTreeNodesMemory value)
			{
				owner.BehaviorTreeMemory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BotMemory owner, out BehaviorTreeNodesMemory value)
			{
				value = owner.BehaviorTreeMemory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003ENewPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, List<int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BotMemory owner, in List<int> value)
			{
				owner.NewPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BotMemory owner, out List<int> value)
			{
				value = owner.NewPath;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003EOldPath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, List<int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BotMemory owner, in List<int> value)
			{
				owner.OldPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BotMemory owner, out List<int> value)
			{
				value = owner.OldPath;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003ELastRunningNodeIndex_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BotMemory owner, in int value)
			{
				owner.LastRunningNodeIndex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BotMemory owner, out int value)
			{
				value = owner.LastRunningNodeIndex;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BotMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BotMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BotMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BotMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BotMemory owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BotMemory owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BotMemory_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BotMemory owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BotMemory owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BotMemory_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BotMemory>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BotMemory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BotMemory CreateInstance()
			{
				return new MyObjectBuilder_BotMemory();
			}

			MyObjectBuilder_BotMemory IActivator<MyObjectBuilder_BotMemory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public BehaviorTreeNodesMemory BehaviorTreeMemory;

		[ProtoMember(19)]
		public List<int> NewPath;

		[ProtoMember(22)]
		public List<int> OldPath;

		[ProtoMember(25)]
		public int LastRunningNodeIndex = -1;
	}
}
