using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRageRender.Animations
{
	[ProtoContract]
	[XmlType("Property")]
	public class GenerationProperty
	{
		protected class VRageRender_Animations_GenerationProperty_003C_003EName_003C_003EAccessor : IMemberAccessor<GenerationProperty, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EAnimationType_003C_003EAccessor : IMemberAccessor<GenerationProperty, PropertyAnimationType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in PropertyAnimationType value)
			{
				owner.AnimationType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out PropertyAnimationType value)
			{
				value = owner.AnimationType;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EType_003C_003EAccessor : IMemberAccessor<GenerationProperty, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueFloat_003C_003EAccessor : IMemberAccessor<GenerationProperty, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in float value)
			{
				owner.ValueFloat = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out float value)
			{
				value = owner.ValueFloat;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueBool_003C_003EAccessor : IMemberAccessor<GenerationProperty, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in bool value)
			{
				owner.ValueBool = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out bool value)
			{
				value = owner.ValueBool;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueInt_003C_003EAccessor : IMemberAccessor<GenerationProperty, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in int value)
			{
				owner.ValueInt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out int value)
			{
				value = owner.ValueInt;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueString_003C_003EAccessor : IMemberAccessor<GenerationProperty, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in string value)
			{
				owner.ValueString = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out string value)
			{
				value = owner.ValueString;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueVector3_003C_003EAccessor : IMemberAccessor<GenerationProperty, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in Vector3 value)
			{
				owner.ValueVector3 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out Vector3 value)
			{
				value = owner.ValueVector3;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EValueVector4_003C_003EAccessor : IMemberAccessor<GenerationProperty, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in Vector4 value)
			{
				owner.ValueVector4 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out Vector4 value)
			{
				value = owner.ValueVector4;
			}
		}

		protected class VRageRender_Animations_GenerationProperty_003C_003EKeys_003C_003EAccessor : IMemberAccessor<GenerationProperty, List<AnimationKey>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref GenerationProperty owner, in List<AnimationKey> value)
			{
				owner.Keys = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref GenerationProperty owner, out List<AnimationKey> value)
			{
				value = owner.Keys;
			}
		}

		[ProtoMember(1)]
		[XmlAttribute("Name")]
		public string Name = "";

		[ProtoMember(4)]
		[XmlAttribute("AnimationType")]
		public PropertyAnimationType AnimationType;

		[ProtoMember(7)]
		[XmlAttribute("Type")]
		public string Type = "";

		[ProtoMember(10)]
		public float ValueFloat;

		[ProtoMember(13)]
		public bool ValueBool;

		[ProtoMember(16)]
		public int ValueInt;

		[ProtoMember(19)]
		public string ValueString = "";

		[ProtoMember(22)]
		public Vector3 ValueVector3;

		[ProtoMember(25)]
		public Vector4 ValueVector4;

		[ProtoMember(28)]
		public List<AnimationKey> Keys;
	}
}
