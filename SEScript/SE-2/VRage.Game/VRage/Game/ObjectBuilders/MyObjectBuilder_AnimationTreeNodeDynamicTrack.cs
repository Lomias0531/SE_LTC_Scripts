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
	/// Track node, playing track given from action
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationTreeNodeDynamicTrack : MyObjectBuilder_AnimationTreeNodeTrack
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EDefaultAnimation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				owner.DefaultAnimation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				value = owner.DefaultAnimation;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EPathToModel_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EPathToModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EAnimationName_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EAnimationName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003ELoop_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ELoop_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003ESpeed_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ESpeed_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in double value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out double value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EInterpolate_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EInterpolate_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003ESynchronizeWithLayer_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ESynchronizeWithLayer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNodeTrack*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeDynamicTrack>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeDynamicTrack();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeDynamicTrack CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeDynamicTrack();
			}

			MyObjectBuilder_AnimationTreeNodeDynamicTrack IActivator<MyObjectBuilder_AnimationTreeNodeDynamicTrack>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public string DefaultAnimation;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool num = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeDynamicTrack myObjectBuilder_AnimationTreeNodeDynamicTrack = new MyObjectBuilder_AnimationTreeNodeDynamicTrack
			{
				DefaultAnimation = DefaultAnimation,
				PathToModel = PathToModel,
				AnimationName = AnimationName,
				Loop = Loop,
				Speed = Speed,
				Interpolate = Interpolate,
				SynchronizeWithLayer = SynchronizeWithLayer,
				EdPos = EdPos
			};
			if (!num)
			{
				return null;
			}
			if (parentNode == null)
			{
				orphans.Add(myObjectBuilder_AnimationTreeNodeDynamicTrack);
			}
			return myObjectBuilder_AnimationTreeNodeDynamicTrack;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			return null;
		}
	}
}
