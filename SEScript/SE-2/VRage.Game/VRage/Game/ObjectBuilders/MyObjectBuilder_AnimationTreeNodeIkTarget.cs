using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
	/// <summary>
	/// Track node, storing information about track and playing settings.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTreeNodeIkTarget : MyObjectBuilder_AnimationTreeNode
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003EBoneChain_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in string[] value)
			{
				owner.BoneChain = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out string[] value)
			{
				value = owner.BoneChain;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003ETargetBoneName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in string value)
			{
				owner.TargetBoneName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out string value)
			{
				value = owner.TargetBoneName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003ETargetPoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in string value)
			{
				owner.TargetPoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out string value)
			{
				value = owner.TargetPoint;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeIkTarget, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeIkTarget owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeIkTarget_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeIkTarget>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeIkTarget();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeIkTarget CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeIkTarget();
			}

			MyObjectBuilder_AnimationTreeNodeIkTarget IActivator<MyObjectBuilder_AnimationTreeNodeIkTarget>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Name of the bones which form the chain.
		/// </summary>
		[ProtoMember(61)]
		[XmlArrayItem("Bone")]
		public string[] BoneChain;

		/// <summary>
		/// Name of the bone which serves as a target for the end effector.
		/// </summary>
		/// If both TargetPoint and TargetBoneName are specified, their transforms are combined 
		/// (target point is then the offset from the target bone).
		[ProtoMember(64)]
		public string TargetBoneName;

		/// <summary>
		/// Target point name. 
		/// </summary>
		/// If both TargetPoint and TargetBoneName are specified, their transforms are combined 
		/// (target point is then the offset from the target bone).
		[ProtoMember(67)]
		public string TargetPoint;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool flag = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeIkTarget myObjectBuilder_AnimationTreeNodeIkTarget = new MyObjectBuilder_AnimationTreeNodeIkTarget();
			myObjectBuilder_AnimationTreeNodeIkTarget.EdPos = EdPos;
			myObjectBuilder_AnimationTreeNodeIkTarget.TargetBoneName = TargetBoneName;
			myObjectBuilder_AnimationTreeNodeIkTarget.TargetPoint = TargetPoint;
			myObjectBuilder_AnimationTreeNodeIkTarget.BoneChain = null;
			if (BoneChain != null)
			{
				myObjectBuilder_AnimationTreeNodeIkTarget.BoneChain = new string[BoneChain.Length];
				for (int i = 0; i < BoneChain.Length; i++)
				{
					myObjectBuilder_AnimationTreeNodeIkTarget.BoneChain[i] = BoneChain[i];
				}
			}
			if (!flag)
			{
				return null;
			}
			if (parentNode == null)
			{
				orphans.Add(myObjectBuilder_AnimationTreeNodeIkTarget);
			}
			return myObjectBuilder_AnimationTreeNodeIkTarget;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			return null;
		}
	}
}
