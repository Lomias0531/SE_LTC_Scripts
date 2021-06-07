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
	public class MyObjectBuilder_AnimationTreeNodeTrack : MyObjectBuilder_AnimationTreeNode
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EPathToModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value)
			{
				owner.PathToModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value)
			{
				value = owner.PathToModel;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EAnimationName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value)
			{
				owner.AnimationName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value)
			{
				value = owner.AnimationName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ELoop_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in bool value)
			{
				owner.Loop = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out bool value)
			{
				value = owner.Loop;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ESpeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in double value)
			{
				owner.Speed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out double value)
			{
				value = owner.Speed;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EInterpolate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in bool value)
			{
				owner.Interpolate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out bool value)
			{
				value = owner.Interpolate;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ESynchronizeWithLayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value)
			{
				owner.SynchronizeWithLayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value)
			{
				value = owner.SynchronizeWithLayer;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EEdPos_003C_003EAccessor : VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in Vector2I? value)
			{
				Set(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out Vector2I? value)
			{
				Get(ref *(MyObjectBuilder_AnimationTreeNode*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeTrack>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeTrack();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationTreeNodeTrack CreateInstance()
			{
				return new MyObjectBuilder_AnimationTreeNodeTrack();
			}

			MyObjectBuilder_AnimationTreeNodeTrack IActivator<MyObjectBuilder_AnimationTreeNodeTrack>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Path to MWM file.
		/// </summary>
		[ProtoMember(13)]
		public string PathToModel;

		/// <summary>
		/// Name of used track (animation) in MWM file.
		/// </summary>
		[ProtoMember(16)]
		public string AnimationName;

		/// <summary>
		/// If true, animation will be looped. Default value is true.
		/// </summary>
		[ProtoMember(19)]
		public bool Loop = true;

		/// <summary>
		/// Playing speed multiplier.
		/// </summary>
		[ProtoMember(22)]
		public double Speed = 1.0;

		/// <summary>
		/// Interpolate between keyframes. If false, track will be played frame by frame.
		/// </summary>
		[ProtoMember(25)]
		public bool Interpolate = true;

		/// <summary>
		/// Synchronize time in this track with the specified layer.
		/// </summary>
		[ProtoMember(28)]
		public string SynchronizeWithLayer;

		protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans)
		{
			bool num = selectedNodes?.Contains(this) ?? true;
			MyObjectBuilder_AnimationTreeNodeTrack myObjectBuilder_AnimationTreeNodeTrack = new MyObjectBuilder_AnimationTreeNodeTrack
			{
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
				orphans.Add(myObjectBuilder_AnimationTreeNodeTrack);
			}
			return myObjectBuilder_AnimationTreeNodeTrack;
		}

		public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
		{
			return null;
		}
	}
}
