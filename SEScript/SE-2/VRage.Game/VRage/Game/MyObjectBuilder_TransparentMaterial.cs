using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TransparentMaterial : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EAlphaMistingEnable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.AlphaMistingEnable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.AlphaMistingEnable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EAlphaMistingStart_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value)
			{
				owner.AlphaMistingStart = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value)
			{
				value = owner.AlphaMistingStart;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EAlphaMistingEnd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value)
			{
				owner.AlphaMistingEnd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value)
			{
				value = owner.AlphaMistingEnd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EAlphaSaturation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value)
			{
				owner.AlphaSaturation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value)
			{
				value = owner.AlphaSaturation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003ECanBeAffectedByOtherLights_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.CanBeAffectedByOtherLights = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.CanBeAffectedByOtherLights;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EEmissivity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value)
			{
				owner.Emissivity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value)
			{
				value = owner.Emissivity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EIgnoreDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.IgnoreDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.IgnoreDepth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003ENeedSort_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.NeedSort = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.NeedSort;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003ESoftParticleDistanceScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in float value)
			{
				owner.SoftParticleDistanceScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out float value)
			{
				value = owner.SoftParticleDistanceScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EUseAtlas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.UseAtlas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.UseAtlas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EUVOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in Vector2 value)
			{
				owner.UVOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out Vector2 value)
			{
				value = owner.UVOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EUVSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in Vector2 value)
			{
				owner.UVSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out Vector2 value)
			{
				value = owner.UVSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EReflection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterial, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in bool value)
			{
				owner.Reflection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out bool value)
			{
				value = owner.Reflection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterial, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterial owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterial owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TransparentMaterial_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TransparentMaterial>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TransparentMaterial();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TransparentMaterial CreateInstance()
			{
				return new MyObjectBuilder_TransparentMaterial();
			}

			MyObjectBuilder_TransparentMaterial IActivator<MyObjectBuilder_TransparentMaterial>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(4)]
		public bool AlphaMistingEnable;

		[ProtoMember(7)]
		[DefaultValue(1)]
		public float AlphaMistingStart = 1f;

		[ProtoMember(10)]
		[DefaultValue(4)]
		public float AlphaMistingEnd = 4f;

		[ProtoMember(13)]
		[DefaultValue(1)]
		public float AlphaSaturation = 1f;

		[ProtoMember(16)]
		public bool CanBeAffectedByOtherLights;

		[ProtoMember(19)]
		public float Emissivity;

		[ProtoMember(22)]
		public bool IgnoreDepth;

		[ProtoMember(25)]
		[DefaultValue(true)]
		public bool NeedSort = true;

		[ProtoMember(28)]
		public float SoftParticleDistanceScale;

		[ProtoMember(31)]
		public string Texture;

		[ProtoMember(34)]
		public bool UseAtlas;

		[ProtoMember(37)]
		public Vector2 UVOffset = new Vector2(0f, 0f);

		[ProtoMember(40)]
		public Vector2 UVSize = new Vector2(1f, 1f);

		[ProtoMember(43)]
		public bool Reflection;
	}
}
