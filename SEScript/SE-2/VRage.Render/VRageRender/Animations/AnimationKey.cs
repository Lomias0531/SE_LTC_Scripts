using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRageRender.Animations
{
	[ProtoContract]
	[XmlType("Key")]
	public class AnimationKey
	{
		protected class VRageRender_Animations_AnimationKey_003C_003ETime_003C_003EAccessor : IMemberAccessor<AnimationKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in float value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out float value)
			{
				value = owner.Time;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueFloat_003C_003EAccessor : IMemberAccessor<AnimationKey, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in float value)
			{
				owner.ValueFloat = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out float value)
			{
				value = owner.ValueFloat;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueBool_003C_003EAccessor : IMemberAccessor<AnimationKey, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in bool value)
			{
				owner.ValueBool = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out bool value)
			{
				value = owner.ValueBool;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueInt_003C_003EAccessor : IMemberAccessor<AnimationKey, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in int value)
			{
				owner.ValueInt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out int value)
			{
				value = owner.ValueInt;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueString_003C_003EAccessor : IMemberAccessor<AnimationKey, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in string value)
			{
				owner.ValueString = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out string value)
			{
				value = owner.ValueString;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueVector3_003C_003EAccessor : IMemberAccessor<AnimationKey, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in Vector3 value)
			{
				owner.ValueVector3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out Vector3 value)
			{
				value = owner.ValueVector3;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValueVector4_003C_003EAccessor : IMemberAccessor<AnimationKey, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in Vector4 value)
			{
				owner.ValueVector4 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out Vector4 value)
			{
				value = owner.ValueVector4;
			}
		}

		protected class VRageRender_Animations_AnimationKey_003C_003EValue2D_003C_003EAccessor : IMemberAccessor<AnimationKey, Generation2DProperty>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AnimationKey owner, in Generation2DProperty value)
			{
				owner.Value2D = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AnimationKey owner, out Generation2DProperty value)
			{
				value = owner.Value2D;
			}
		}

		[ProtoMember(34)]
		public float Time;

		[ProtoMember(37)]
		public float ValueFloat;

		[ProtoMember(40)]
		public bool ValueBool;

		[ProtoMember(43)]
		public int ValueInt;

		[ProtoMember(46)]
		public string ValueString = "";

		[ProtoMember(49)]
		public Vector3 ValueVector3;

		[ProtoMember(52)]
		public Vector4 ValueVector4;

		[ProtoMember(55)]
		public Generation2DProperty Value2D;
	}
}
