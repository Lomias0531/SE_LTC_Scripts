using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_TransparentMaterialDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EGlossTexture_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				owner.GlossTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				value = owner.GlossTexture;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ETextureType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyTransparentMaterialTextureType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in MyTransparentMaterialTextureType value)
			{
				owner.TextureType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out MyTransparentMaterialTextureType value)
			{
				value = owner.TextureType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ECanBeAffectedByOtherLights_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.CanBeAffectedByOtherLights = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.CanBeAffectedByOtherLights;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAlphaMistingEnable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.AlphaMistingEnable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.AlphaMistingEnable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EUseAtlas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.UseAtlas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.UseAtlas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAlphaMistingStart_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.AlphaMistingStart = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.AlphaMistingStart;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAlphaMistingEnd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.AlphaMistingEnd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.AlphaMistingEnd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ESoftParticleDistanceScale_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.SoftParticleDistanceScale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.SoftParticleDistanceScale;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAlphaSaturation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.AlphaSaturation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.AlphaSaturation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in Vector4 value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out Vector4 value)
			{
				value = owner.Color;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EColorAdd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in Vector4 value)
			{
				owner.ColorAdd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out Vector4 value)
			{
				value = owner.ColorAdd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EShadowMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in Vector4 value)
			{
				owner.ShadowMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out Vector4 value)
			{
				value = owner.ShadowMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ELightMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in Vector4 value)
			{
				owner.LightMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out Vector4 value)
			{
				value = owner.LightMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EReflectivity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.Reflectivity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.Reflectivity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EFresnel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.Fresnel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.Fresnel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EReflectionShadow_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.ReflectionShadow = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.ReflectionShadow;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EGloss_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.Gloss = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.Gloss;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EGlossTextureAdd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.GlossTextureAdd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.GlossTextureAdd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ESpecularColorFactor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in float value)
			{
				owner.SpecularColorFactor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out float value)
			{
				value = owner.SpecularColorFactor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EIsFlareOccluder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.IsFlareOccluder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.IsFlareOccluder;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ETriangleFaceCulling_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.TriangleFaceCulling = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.TriangleFaceCulling;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAlphaCutout_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				owner.AlphaCutout = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				value = owner.AlphaCutout;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ETargetSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector2I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in Vector2I value)
			{
				owner.TargetSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out Vector2I value)
			{
				value = owner.TargetSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_TransparentMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_TransparentMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TransparentMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TransparentMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TransparentMaterialDefinition CreateInstance()
			{
				return new MyObjectBuilder_TransparentMaterialDefinition();
			}

			MyObjectBuilder_TransparentMaterialDefinition IActivator<MyObjectBuilder_TransparentMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[ModdableContentFile("dds")]
		public string Texture;

		[ProtoMember(4)]
		[ModdableContentFile("dds")]
		public string GlossTexture;

		[ProtoMember(7)]
		public MyTransparentMaterialTextureType TextureType;

		[ProtoMember(10)]
		public bool CanBeAffectedByOtherLights;

		[ProtoMember(13)]
		public bool AlphaMistingEnable;

		[ProtoMember(16)]
		public bool UseAtlas;

		[ProtoMember(19)]
		public float AlphaMistingStart;

		[ProtoMember(22)]
		public float AlphaMistingEnd;

		[ProtoMember(25)]
		public float SoftParticleDistanceScale;

		[ProtoMember(28)]
		public float AlphaSaturation;

		[ProtoMember(31)]
		public Vector4 Color = Vector4.One;

		[ProtoMember(34)]
		public Vector4 ColorAdd = Vector4.Zero;

		[ProtoMember(37)]
		public Vector4 ShadowMultiplier = Vector4.Zero;

		[ProtoMember(40)]
		public Vector4 LightMultiplier = Vector4.One * 0.1f;

		[ProtoMember(43)]
		public float Reflectivity = 0.6f;

		[ProtoMember(46)]
		public float Fresnel = 1f;

		[ProtoMember(49)]
		public float ReflectionShadow = 0.1f;

		[ProtoMember(52)]
		public float Gloss = 0.4f;

		[ProtoMember(55)]
		public float GlossTextureAdd = 0.55f;

		[ProtoMember(58)]
		public float SpecularColorFactor = 20f;

		[ProtoMember(61)]
		public bool IsFlareOccluder;

		[ProtoMember(62)]
		public bool TriangleFaceCulling = true;

		[ProtoMember(64)]
		public bool AlphaCutout;

		[ProtoMember(67)]
		public Vector2I TargetSize = new Vector2I(-1, -1);
	}
}
