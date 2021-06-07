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
	public class MyObjectBuilder_BehaviorTreeSelectorNode : MyObjectBuilder_BehaviorControlBaseNode
	{
		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003EBTNodes_003C_003EAccessor : VRage_Game_MyObjectBuilder_BehaviorControlBaseNode_003C_003EBTNodes_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyObjectBuilder_BehaviorTreeNode[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in MyObjectBuilder_BehaviorTreeNode[] value)
			{
				Set(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out MyObjectBuilder_BehaviorTreeNode[] value)
			{
				Get(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003EName_003C_003EAccessor : VRage_Game_MyObjectBuilder_BehaviorControlBaseNode_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003EIsMemorable_003C_003EAccessor : VRage_Game_MyObjectBuilder_BehaviorControlBaseNode_003C_003EIsMemorable_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_BehaviorControlBaseNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeSelectorNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeSelectorNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BehaviorTreeSelectorNode CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeSelectorNode();
			}

			MyObjectBuilder_BehaviorTreeSelectorNode IActivator<MyObjectBuilder_BehaviorTreeSelectorNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
