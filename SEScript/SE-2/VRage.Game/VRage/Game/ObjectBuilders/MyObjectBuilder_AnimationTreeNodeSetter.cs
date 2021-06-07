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
	/// Setter node, storing information about timed variable setting.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTreeNodeSetter : MyObjectBuilder_AnimationTreeNode
	{
		[ProtoContract]
		public struct ValueAssignment
		{
			protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EValueAssignment_003C_003EName_003C_003EAccessor : IMemberAccessor<ValueAssignment, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ValueAssignment owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ValueAssignment owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EValueAssignment_003C_003EValue_003C_003EAccessor : IMemberAccessor<ValueAssignment, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ValueAssignment owner, in float value)
				{
					owner.Value = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ValueAssignment owner, out float value)
				{
					value = owner.Value;
				}
			}

			private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EValueAssignment_003C_003EActor : IActivator, IActivator<ValueAssignment>
			{
				private sealed override object CreateInstance()
				{
					return default(ValueAssignment);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ValueAssignment CreateInstance()
				{
					return (ValueAssignment)(object)default(ValueAssignment);
				}

				ValueAssignment IActivator<ValueAssignment>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			/// <summary>
			/// Name of the variable.
			/// </summary>
			[ProtoMember(70)]
			public string Name;

			/// <summary>
			/// Value to be set.
			/// </summary>
			[ProtoMember(73)]
			public float Value;
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EChild_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyObjectBuilder_AnimationTreeNode>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in MyObjectBuilder_AnimationTreeNode value)
			{
				owner.Child = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out MyObjectBuilder_AnimationTreeNode value)
			{
				value = owner.Child;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003ETime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in float value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out float value)
			{
				value = owner.Time;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, ValueAssignment>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in ValueAssignment value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out ValueAssignment value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EResetValueEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in bool value)
			{
				owner.ResetValueEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out bool value)
			{
				value = owner.ResetValueEnabled;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EResetValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in float value)
			{
				owner.ResetValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out float value)
			{
				value = owner.ResetValue;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeSetter>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeSetter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeSetter CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeSetter();
			}

			MyObjectBuilder_AnimationTreeNodeSetter IActivator<MyObjectBuilder_AnimationTreeNodeSetter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Link to the child node.
		/// </summary>
		[ProtoMember(76)]
		[XmlElement(typeof(MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
		public MyObjectBuilder_AnimationTreeNode Child;

		/// <summary>
		/// Local animation time when the value should be set.
		/// </summary>
		[ProtoMember(79)]
		public float Time;

		/// <summary>
		/// Value that is set to animation storage once time spent in the node exceeds specified time (MyAnimationTreeNodeSetter.Time).
		/// </summary>
		[ProtoMember(82)]
		public ValueAssignment Value;

		/// <summary>
		/// When the, the automatic resetting is enabled.
		/// </summary>
		[ProtoMember(85)]
		public bool ResetValueEnabled;

		/// <summary>
		/// Value that is set to animation storage once we leave current animation state.
		/// </summary>
		[ProtoMember(88)]
		public float ResetValue;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool flag = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeSetter myObjectBuilder_AnimationTreeNodeSetter = new MyObjectBuilder_AnimationTreeNodeSetter();
			myObjectBuilder_AnimationTreeNodeSetter.Value = Value;
			myObjectBuilder_AnimationTreeNodeSetter.ResetValue = ResetValue;
			myObjectBuilder_AnimationTreeNodeSetter.Time = Time;
			myObjectBuilder_AnimationTreeNodeSetter.EdPos = EdPos;
			myObjectBuilder_AnimationTreeNodeSetter.ResetValueEnabled = ResetValueEnabled;
			if (Child != null)
			{
				myObjectBuilder_AnimationTreeNodeSetter.Child = Child.DeepCopyWithMask(selectedNodes, flag ? myObjectBuilder_AnimationTreeNodeSetter : null, orphans);
			}
			if (!flag)
			{
				return null;
			}
			if (parentNode == null)
			{
				orphans.Add(myObjectBuilder_AnimationTreeNodeSetter);
			}
			return myObjectBuilder_AnimationTreeNodeSetter;
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
