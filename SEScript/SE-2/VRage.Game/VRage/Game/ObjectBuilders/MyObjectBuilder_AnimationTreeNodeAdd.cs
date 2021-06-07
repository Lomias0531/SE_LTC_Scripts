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
	/// Additive node. Child nodes are base node + additive node.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTreeNodeAdd : MyObjectBuilder_AnimationTreeNode
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003EParameterName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value)
			{
				owner.ParameterName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value)
			{
				value = owner.ParameterName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003EBaseNode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyParameterAnimTreeNodeMapping>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyParameterAnimTreeNodeMapping value)
			{
				owner.BaseNode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out MyParameterAnimTreeNodeMapping value)
			{
				value = owner.BaseNode;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003EAddNode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyParameterAnimTreeNodeMapping>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyParameterAnimTreeNodeMapping value)
			{
				owner.AddNode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out MyParameterAnimTreeNodeMapping value)
			{
				value = owner.AddNode;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeAdd>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeAdd();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeAdd CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeAdd();
			}

			MyObjectBuilder_AnimationTreeNodeAdd IActivator<MyObjectBuilder_AnimationTreeNodeAdd>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Name of parameter controlling blending inside this node.
		/// </summary>
		[ProtoMember(52)]
		public string ParameterName;

		/// <summary>
		/// Child node, base "layer".
		/// </summary>
		[ProtoMember(55)]
		public MyParameterAnimTreeNodeMapping BaseNode;

		/// <summary>
		/// Child node, additive "layer".
		/// </summary>
		[ProtoMember(58)]
		public MyParameterAnimTreeNodeMapping AddNode;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool flag = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeAdd myObjectBuilder_AnimationTreeNodeAdd = new MyObjectBuilder_AnimationTreeNodeAdd();
			myObjectBuilder_AnimationTreeNodeAdd.EdPos = EdPos;
			myObjectBuilder_AnimationTreeNodeAdd.ParameterName = ParameterName;
			myObjectBuilder_AnimationTreeNodeAdd.BaseNode.Param = BaseNode.Param;
			myObjectBuilder_AnimationTreeNodeAdd.BaseNode.Node = null;
			myObjectBuilder_AnimationTreeNodeAdd.AddNode.Param = AddNode.Param;
			myObjectBuilder_AnimationTreeNodeAdd.AddNode.Node = null;
			myObjectBuilder_AnimationTreeNodeAdd.BaseNode.Node = BaseNode.Node.DeepCopyWithMask(selectedNodes, flag ? myObjectBuilder_AnimationTreeNodeAdd : null, orphans);
			myObjectBuilder_AnimationTreeNodeAdd.AddNode.Node = AddNode.Node.DeepCopyWithMask(selectedNodes, flag ? myObjectBuilder_AnimationTreeNodeAdd : null, orphans);
			if (!flag)
			{
				return null;
			}
			if (parentNode == null)
			{
				orphans.Add(myObjectBuilder_AnimationTreeNodeAdd);
			}
			return myObjectBuilder_AnimationTreeNodeAdd;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			List<MyObjectBuilder_AnimationTreeNode> list = new List<MyObjectBuilder_AnimationTreeNode>();
			if (BaseNode.Node != null)
			{
				list.Add(BaseNode.Node);
			}
			if (AddNode.Node != null)
			{
				list.Add(AddNode.Node);
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}
	}
}
