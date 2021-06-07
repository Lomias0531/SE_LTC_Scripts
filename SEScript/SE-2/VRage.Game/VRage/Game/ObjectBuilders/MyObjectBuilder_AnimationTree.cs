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
	/// Root node of the whole animation tree. Supports storing of orphaned nodes.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTree : MyObjectBuilder_AnimationTreeNode
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003EChild_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTree, MyObjectBuilder_AnimationTreeNode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in MyObjectBuilder_AnimationTreeNode value)
			{
				owner.Child = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out MyObjectBuilder_AnimationTreeNode value)
			{
				value = owner.Child;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003EOrphans_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTree, MyObjectBuilder_AnimationTreeNode[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in MyObjectBuilder_AnimationTreeNode[] value)
			{
				owner.Orphans = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out MyObjectBuilder_AnimationTreeNode[] value)
			{
				value = owner.Orphans;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTree owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTree owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTree>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTree();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTree CreateInstance()
			{
				return new MyObjectBuilder_AnimationTree();
			}

			MyObjectBuilder_AnimationTree IActivator<MyObjectBuilder_AnimationTree>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		[XmlElement(typeof(MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
		public MyObjectBuilder_AnimationTreeNode Child;

		[ProtoMember(7)]
		[XmlArrayItem(typeof(MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
		public MyObjectBuilder_AnimationTreeNode[] Orphans;

		/// <summary>
		/// Create deep copy of this node and its children.
		/// </summary>
		/// <param name="selectedNodes">the mask</param>
		/// <returns>copied hierarchy</returns>
		public MyObjectBuilder_AnimationTree DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes)
		{
			List<MyObjectBuilder_AnimationTreeNode> orphans = new List<MyObjectBuilder_AnimationTreeNode>();
			if (Orphans != null)
			{
				MyObjectBuilder_AnimationTreeNode[] orphans2 = Orphans;
				for (int i = 0; i < orphans2.Length; i++)
				{
					orphans2[i].DeepCopyWithMask(selectedNodes, null, orphans);
				}
			}
			return (MyObjectBuilder_AnimationTree)DeepCopyWithMask(selectedNodes, null, orphans);
		}

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			MyObjectBuilder_AnimationTree myObjectBuilder_AnimationTree = new MyObjectBuilder_AnimationTree();
			if (Child == null)
			{
				myObjectBuilder_AnimationTree.Child = null;
				myObjectBuilder_AnimationTree.Orphans = ((orphans.Count > 0) ? orphans.ToArray() : null);
				return myObjectBuilder_AnimationTree;
			}
			MyObjectBuilder_AnimationTreeNode child = Child.DeepCopyWithMask(selectedNodes, myObjectBuilder_AnimationTree, orphans);
			myObjectBuilder_AnimationTree.EdPos = EdPos;
			myObjectBuilder_AnimationTree.Child = child;
			myObjectBuilder_AnimationTree.Orphans = ((orphans.Count > 0) ? orphans.ToArray() : null);
			return myObjectBuilder_AnimationTree;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			if (Child != null)
			{
				return new MyObjectBuilder_AnimationTreeNode[1]
				{
					Child
				};
			}
			return null;
		}
	}
}
