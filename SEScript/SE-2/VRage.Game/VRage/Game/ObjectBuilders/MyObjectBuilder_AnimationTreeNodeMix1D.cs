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
	/// Linear mixing node. Maps child nodes on 1D axis, interpolates according to parameter value.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTreeNodeMix1D : MyObjectBuilder_AnimationTreeNode
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003EParameterName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value)
			{
				owner.ParameterName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value)
			{
				value = owner.ParameterName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003ECircular_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in bool value)
			{
				owner.Circular = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out bool value)
			{
				value = owner.Circular;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003ESensitivity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in float value)
			{
				owner.Sensitivity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out float value)
			{
				value = owner.Sensitivity;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003EMaxChange_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in float? value)
			{
				owner.MaxChange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out float? value)
			{
				value = owner.MaxChange;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003EChildren_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyParameterAnimTreeNodeMapping[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in MyParameterAnimTreeNodeMapping[] value)
			{
				owner.Children = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out MyParameterAnimTreeNodeMapping[] value)
			{
				value = owner.Children;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeMix1D>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeMix1D();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeMix1D CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeMix1D();
			}

			MyObjectBuilder_AnimationTreeNodeMix1D IActivator<MyObjectBuilder_AnimationTreeNodeMix1D>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Name of parameter controlling blending inside this node.
		/// </summary>
		[ProtoMember(37)]
		public string ParameterName;

		/// <summary>
		/// True if the value wraps around.
		/// </summary>
		[ProtoMember(40)]
		public bool Circular;

		/// <summary>
		/// Sensitivity to changes of parameter value. 1=immediate change, 0=no sensitivity.
		/// </summary>
		[ProtoMember(43)]
		public float Sensitivity = 1f;

		/// <summary>
		/// Threshold: maximum change of variable to take sensitivity in account, if crossed, value is set immediatelly.
		/// </summary>
		[ProtoMember(46)]
		public float? MaxChange;

		/// <summary>
		/// Mapping children to axis. Each child has assigned its value.
		/// </summary>
		[ProtoMember(49)]
		[XmlElement("Child")]
		public MyParameterAnimTreeNodeMapping[] Children;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool flag = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeMix1D myObjectBuilder_AnimationTreeNodeMix1D = new MyObjectBuilder_AnimationTreeNodeMix1D();
			myObjectBuilder_AnimationTreeNodeMix1D.ParameterName = ParameterName;
			myObjectBuilder_AnimationTreeNodeMix1D.Circular = Circular;
			myObjectBuilder_AnimationTreeNodeMix1D.Sensitivity = Sensitivity;
			myObjectBuilder_AnimationTreeNodeMix1D.MaxChange = MaxChange;
			myObjectBuilder_AnimationTreeNodeMix1D.Children = null;
			myObjectBuilder_AnimationTreeNodeMix1D.EdPos = EdPos;
			if (Children != null)
			{
				myObjectBuilder_AnimationTreeNodeMix1D.Children = new MyParameterAnimTreeNodeMapping[Children.Length];
				for (int i = 0; i < Children.Length; i++)
				{
					MyObjectBuilder_AnimationTreeNode node = null;
					if (Children[i].Node != null)
					{
						node = Children[i].Node.DeepCopyWithMask(selectedNodes, flag ? myObjectBuilder_AnimationTreeNodeMix1D : null, orphans);
					}
					myObjectBuilder_AnimationTreeNodeMix1D.Children[i].Param = Children[i].Param;
					myObjectBuilder_AnimationTreeNodeMix1D.Children[i].Node = node;
				}
			}
			if (!flag)
			{
				return null;
			}
			if (parentNode == null)
			{
				orphans.Add(myObjectBuilder_AnimationTreeNodeMix1D);
			}
			return myObjectBuilder_AnimationTreeNodeMix1D;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			if (Children != null)
			{
				List<MyObjectBuilder_AnimationTreeNode> list = new List<MyObjectBuilder_AnimationTreeNode>();
				for (int i = 0; i < Children.Length; i++)
				{
					if (Children[i].Node != null)
					{
						list.Add(Children[i].Node);
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}
	}
}
