using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("PlanetGeneratorDefinition")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PlanetGeneratorDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EPlanetMaps_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaps?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaps? value)
			{
				owner.PlanetMaps = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaps? value)
			{
				value = owner.PlanetMaps;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EHasAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in bool? value)
			{
				owner.HasAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out bool? value)
			{
				value = owner.HasAtmosphere;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ECloudLayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<MyCloudLayerSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in List<MyCloudLayerSettings> value)
			{
				owner.CloudLayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out List<MyCloudLayerSettings> value)
			{
				value = owner.CloudLayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EHillParams_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in SerializableRange? value)
			{
				owner.HillParams = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out SerializableRange? value)
			{
				value = owner.HillParams;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EGravityFalloffPower_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in float? value)
			{
				owner.GravityFalloffPower = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out float? value)
			{
				value = owner.GravityFalloffPower;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMaterialsMaxDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in SerializableRange? value)
			{
				owner.MaterialsMaxDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out SerializableRange? value)
			{
				value = owner.MaterialsMaxDepth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMaterialsMinDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableRange?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in SerializableRange? value)
			{
				owner.MaterialsMinDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out SerializableRange? value)
			{
				value = owner.MaterialsMinDepth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EHostileAtmosphereColorShift_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyAtmosphereColorShift>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyAtmosphereColorShift value)
			{
				owner.HostileAtmosphereColorShift = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyAtmosphereColorShift value)
			{
				value = owner.HostileAtmosphereColorShift;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ECustomMaterialTable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaterialDefinition[] value)
			{
				owner.CustomMaterialTable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaterialDefinition[] value)
			{
				value = owner.CustomMaterialTable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDistortionTable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetDistortionDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetDistortionDefinition[] value)
			{
				owner.DistortionTable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetDistortionDefinition[] value)
			{
				value = owner.DistortionTable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDefaultSurfaceMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaterialDefinition value)
			{
				owner.DefaultSurfaceMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaterialDefinition value)
			{
				value = owner.DefaultSurfaceMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDefaultSubSurfaceMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaterialDefinition value)
			{
				owner.DefaultSubSurfaceMaterial = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaterialDefinition value)
			{
				value = owner.DefaultSubSurfaceMaterial;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ESurfaceGravity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in float? value)
			{
				owner.SurfaceGravity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out float? value)
			{
				value = owner.SurfaceGravity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAtmosphere>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetAtmosphere value)
			{
				owner.Atmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetAtmosphere value)
			{
				value = owner.Atmosphere;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EAtmosphereSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyAtmosphereSettings?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyAtmosphereSettings? value)
			{
				owner.AtmosphereSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyAtmosphereSettings? value)
			{
				value = owner.AtmosphereSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EFolderName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				owner.FolderName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				value = owner.FolderName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EComplexMaterials_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialGroup[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaterialGroup[] value)
			{
				owner.ComplexMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaterialGroup[] value)
			{
				value = owner.ComplexMaterials;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ESoundRules_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MySerializablePlanetEnvironmentalSoundRule[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MySerializablePlanetEnvironmentalSoundRule[] value)
			{
				owner.SoundRules = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MySerializablePlanetEnvironmentalSoundRule[] value)
			{
				value = owner.SoundRules;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMusicCategories_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<MyMusicCategory>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in List<MyMusicCategory> value)
			{
				owner.MusicCategories = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out List<MyMusicCategory> value)
			{
				value = owner.MusicCategories;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EOreMappings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetOreMapping[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetOreMapping[] value)
			{
				owner.OreMappings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetOreMapping[] value)
			{
				value = owner.OreMappings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EEnvironmentItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, PlanetEnvironmentItemMapping[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in PlanetEnvironmentItemMapping[] value)
			{
				owner.EnvironmentItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out PlanetEnvironmentItemMapping[] value)
			{
				value = owner.EnvironmentItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMaterialBlending_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetMaterialBlendSettings?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetMaterialBlendSettings? value)
			{
				owner.MaterialBlending = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetMaterialBlendSettings? value)
			{
				value = owner.MaterialBlending;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ESurfaceDetail_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetSurfaceDetail>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetSurfaceDetail value)
			{
				owner.SurfaceDetail = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetSurfaceDetail value)
			{
				value = owner.SurfaceDetail;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EAnimalSpawnInfo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAnimalSpawnInfo>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetAnimalSpawnInfo value)
			{
				owner.AnimalSpawnInfo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetAnimalSpawnInfo value)
			{
				value = owner.AnimalSpawnInfo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ENightAnimalSpawnInfo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyPlanetAnimalSpawnInfo>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyPlanetAnimalSpawnInfo value)
			{
				owner.NightAnimalSpawnInfo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyPlanetAnimalSpawnInfo value)
			{
				value = owner.NightAnimalSpawnInfo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ESectorDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in float? value)
			{
				owner.SectorDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out float? value)
			{
				value = owner.SectorDensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EInheritFrom_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				owner.InheritFrom = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				value = owner.InheritFrom;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EEnvironment_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in SerializableDefinitionId? value)
			{
				owner.Environment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out SerializableDefinitionId? value)
			{
				value = owner.Environment;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMapProvider_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyObjectBuilder_PlanetMapProvider>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyObjectBuilder_PlanetMapProvider value)
			{
				owner.MapProvider = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyObjectBuilder_PlanetMapProvider value)
			{
				value = owner.MapProvider;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMesherPostprocessing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyObjectBuilder_VoxelMesherComponentDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyObjectBuilder_VoxelMesherComponentDefinition value)
			{
				owner.MesherPostprocessing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyObjectBuilder_VoxelMesherComponentDefinition value)
			{
				value = owner.MesherPostprocessing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EMinimumSurfaceLayerDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in float value)
			{
				owner.MinimumSurfaceLayerDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out float value)
			{
				value = owner.MinimumSurfaceLayerDepth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EStationBlockingMaterials_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, List<SerializableDefinitionId>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in List<SerializableDefinitionId> value)
			{
				owner.StationBlockingMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out List<SerializableDefinitionId> value)
			{
				value = owner.StationBlockingMaterials;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PlanetGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PlanetGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_PlanetGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PlanetGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PlanetGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PlanetGeneratorDefinition CreateInstance()
			{
				return new MyObjectBuilder_PlanetGeneratorDefinition();
			}

			MyObjectBuilder_PlanetGeneratorDefinition IActivator<MyObjectBuilder_PlanetGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(79)]
		public MyPlanetMaps? PlanetMaps;

		[ProtoMember(80)]
		public bool? HasAtmosphere;

		[ProtoMember(81)]
		[XmlArrayItem("CloudLayer")]
		public List<MyCloudLayerSettings> CloudLayers;

		[ProtoMember(82)]
		public SerializableRange? HillParams;

		[ProtoMember(83)]
		public float? GravityFalloffPower;

		[ProtoMember(84)]
		public SerializableRange? MaterialsMaxDepth;

		[ProtoMember(85)]
		public SerializableRange? MaterialsMinDepth;

		[ProtoMember(86)]
		public MyAtmosphereColorShift HostileAtmosphereColorShift;

		[ProtoMember(87)]
		[XmlArrayItem("Material")]
		public MyPlanetMaterialDefinition[] CustomMaterialTable;

		[ProtoMember(88)]
		[XmlArrayItem("Distortion")]
		public MyPlanetDistortionDefinition[] DistortionTable;

		[ProtoMember(89)]
		public MyPlanetMaterialDefinition DefaultSurfaceMaterial;

		[ProtoMember(90)]
		public MyPlanetMaterialDefinition DefaultSubSurfaceMaterial;

		[ProtoMember(91)]
		public float? SurfaceGravity;

		[ProtoMember(92)]
		public MyPlanetAtmosphere Atmosphere;

		[ProtoMember(93)]
		public MyAtmosphereSettings? AtmosphereSettings;

		[ProtoMember(94)]
		public string FolderName;

		[ProtoMember(95)]
		public MyPlanetMaterialGroup[] ComplexMaterials;

		[ProtoMember(96)]
		[XmlArrayItem("SoundRule")]
		public MySerializablePlanetEnvironmentalSoundRule[] SoundRules;

		[ProtoMember(97)]
		[XmlArrayItem("MusicCategory")]
		public List<MyMusicCategory> MusicCategories;

		[ProtoMember(98)]
		[XmlArrayItem("Ore")]
		public MyPlanetOreMapping[] OreMappings;

		[ProtoMember(99)]
		[XmlArrayItem("Item")]
		public PlanetEnvironmentItemMapping[] EnvironmentItems;

		[ProtoMember(100)]
		public MyPlanetMaterialBlendSettings? MaterialBlending;

		[ProtoMember(101)]
		public MyPlanetSurfaceDetail SurfaceDetail;

		[ProtoMember(102)]
		public MyPlanetAnimalSpawnInfo AnimalSpawnInfo;

		[ProtoMember(103)]
		public MyPlanetAnimalSpawnInfo NightAnimalSpawnInfo;

		public float? SectorDensity;

		[ProtoMember(104)]
		public string InheritFrom;

		public SerializableDefinitionId? Environment;

		[XmlElement(typeof(MyAbstractXmlSerializer<MyObjectBuilder_PlanetMapProvider>))]
		public MyObjectBuilder_PlanetMapProvider MapProvider;

		[ProtoMember(105)]
		public MyObjectBuilder_VoxelMesherComponentDefinition MesherPostprocessing;

		[ProtoMember(106)]
		public float MinimumSurfaceLayerDepth = 4f;

		[ProtoMember(108)]
		public List<SerializableDefinitionId> StationBlockingMaterials;
	}
}
