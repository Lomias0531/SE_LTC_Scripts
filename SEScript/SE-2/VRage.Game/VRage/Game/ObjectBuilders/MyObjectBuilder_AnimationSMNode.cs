using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Animation;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationSMNode : MyObjectBuilder_Base
	{
		public enum MySMNodeType
		{
			Normal,
			PassThrough,
			Any,
			AnyExceptTarget
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EStateMachineName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value)
			{
				owner.StateMachineName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value)
			{
				value = owner.StateMachineName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EAnimationTree_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyObjectBuilder_AnimationTree>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in MyObjectBuilder_AnimationTree value)
			{
				owner.AnimationTree = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out MyObjectBuilder_AnimationTree value)
			{
				value = owner.AnimationTree;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EEdPos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in Vector2I? value)
			{
				owner.EdPos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out Vector2I? value)
			{
				value = owner.EdPos;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, MySMNodeType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in MySMNodeType value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out MySMNodeType value)
			{
				value = owner.Type;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EVariables_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, List<MyObjectBuilder_AnimationSMVariable>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in List<MyObjectBuilder_AnimationSMVariable> value)
			{
				owner.Variables = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out List<MyObjectBuilder_AnimationSMVariable> value)
			{
				value = owner.Variables;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationSMNode CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMNode();
			}

			MyObjectBuilder_AnimationSMNode IActivator<MyObjectBuilder_AnimationSMNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(4)]
		public string StateMachineName;

		[ProtoMember(7)]
		public MyObjectBuilder_AnimationTree AnimationTree;

		[ProtoMember(10)]
		public Vector2I? EdPos;

		[ProtoMember(13)]
		public MySMNodeType Type;

		[ProtoMember(16)]
		[XmlArrayItem("Variable")]
		public List<MyObjectBuilder_AnimationSMVariable> Variables = new List<MyObjectBuilder_AnimationSMVariable>();
	}
}
