using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("VoxelMaterial")]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_VoxelMaterialDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMaterialTypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.MaterialTypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.MaterialTypeName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinedOre_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.MinedOre = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.MinedOre;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinedOreRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value)
			{
				owner.MinedOreRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value)
			{
				value = owner.MinedOreRatio;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ECanBeHarvested_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				owner.CanBeHarvested = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				value = owner.CanBeHarvested;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EIsRare_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				owner.IsRare = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				value = owner.IsRare;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EUseTwoTextures_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				owner.UseTwoTextures = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				value = owner.UseTwoTextures;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EVoxelHandPreview_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.VoxelHandPreview = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.VoxelHandPreview;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMinVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value)
			{
				owner.MinVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value)
			{
				value = owner.MinVersion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EMaxVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value)
			{
				owner.MaxVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value)
			{
				value = owner.MaxVersion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ESpawnsInAsteroids_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				owner.SpawnsInAsteroids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				value = owner.SpawnsInAsteroids;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ESpawnsFromMeteorites_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				owner.SpawnsFromMeteorites = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				value = owner.SpawnsFromMeteorites;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDamagedMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.DamagedMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.DamagedMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EFriction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value)
			{
				owner.Friction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Friction;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ERestitution_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value)
			{
				owner.Restitution = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value)
			{
				value = owner.Restitution;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EColorKey_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, ColorDefinitionRGBA?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in ColorDefinitionRGBA? value)
			{
				owner.ColorKey = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out ColorDefinitionRGBA? value)
			{
				value = owner.ColorKey;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ELandingEffect_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.LandingEffect = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.LandingEffect;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EAsteroidGeneratorSpawnProbabilityMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value)
			{
				owner.AsteroidGeneratorSpawnProbabilityMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value)
			{
				value = owner.AsteroidGeneratorSpawnProbabilityMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EBareVariant_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				owner.BareVariant = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				value = owner.BareVariant;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VoxelMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VoxelMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VoxelMaterialDefinition CreateInstance()
			{
				return new MyObjectBuilder_VoxelMaterialDefinition();
			}

			MyObjectBuilder_VoxelMaterialDefinition IActivator<MyObjectBuilder_VoxelMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string MaterialTypeName = "Rock";

		[ProtoMember(4)]
		public string MinedOre;

		[ProtoMember(7)]
		public float MinedOreRatio;

		[ProtoMember(10)]
		public bool CanBeHarvested;

		[ProtoMember(13)]
		public bool IsRare;

		[ProtoMember(16)]
		public bool UseTwoTextures;

		[ProtoMember(19)]
		[ModdableContentFile("dds")]
		public string VoxelHandPreview;

		[ProtoMember(22)]
		public int MinVersion;

		[ProtoMember(25)]
		public int MaxVersion = int.MaxValue;

		[ProtoMember(28)]
		public bool SpawnsInAsteroids = true;

		[ProtoMember(31)]
		public bool SpawnsFromMeteorites = true;

		public string DamagedMaterial;

		[ProtoMember(34, IsRequired = false)]
		public float Friction = 1f;

		[ProtoMember(37, IsRequired = false)]
		public float Restitution = 1f;

		[ProtoMember(40, IsRequired = false)]
		public ColorDefinitionRGBA? ColorKey;

		[ProtoMember(43, IsRequired = false)]
		[DefaultValue("")]
		public string LandingEffect;

		[ProtoMember(46, IsRequired = false)]
		public int AsteroidGeneratorSpawnProbabilityMultiplier = 1;

		[ProtoMember(49, IsRequired = false)]
		[DefaultValue("")]
		public string BareVariant;
	}
}
