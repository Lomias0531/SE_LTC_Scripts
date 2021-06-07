using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[XmlRoot("Definitions")]
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Definitions : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DefinitionBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DefinitionBase[] value)
			{
				owner.Definitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DefinitionBase[] value)
			{
				value = owner.Definitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EGridCreators_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_GridCreateToolDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_GridCreateToolDefinition[] value)
			{
				owner.GridCreators = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_GridCreateToolDefinition[] value)
			{
				value = owner.GridCreators;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAmmoMagazines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AmmoMagazineDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AmmoMagazineDefinition[] value)
			{
				owner.AmmoMagazines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AmmoMagazineDefinition[] value)
			{
				value = owner.AmmoMagazines;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlueprints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BlueprintDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BlueprintDefinition[] value)
			{
				owner.Blueprints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BlueprintDefinition[] value)
			{
				value = owner.Blueprints;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ComponentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ComponentDefinition[] value)
			{
				owner.Components = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ComponentDefinition[] value)
			{
				value = owner.Components;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EContainerTypes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ContainerTypeDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ContainerTypeDefinition[] value)
			{
				owner.ContainerTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ContainerTypeDefinition[] value)
			{
				value = owner.ContainerTypes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECubeBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_CubeBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_CubeBlockDefinition[] value)
			{
				owner.CubeBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_CubeBlockDefinition[] value)
			{
				value = owner.CubeBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlockPositions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyBlockPosition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyBlockPosition[] value)
			{
				owner.BlockPositions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyBlockPosition[] value)
			{
				value = owner.BlockPositions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EConfiguration_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_Configuration>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_Configuration value)
			{
				owner.Configuration = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_Configuration value)
			{
				value = owner.Configuration;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEnvironments_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EnvironmentDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EnvironmentDefinition[] value)
			{
				owner.Environments = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EnvironmentDefinition[] value)
			{
				value = owner.Environments;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EGlobalEvents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_GlobalEventDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_GlobalEventDefinition[] value)
			{
				owner.GlobalEvents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_GlobalEventDefinition[] value)
			{
				value = owner.GlobalEvents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EHandItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_HandItemDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_HandItemDefinition[] value)
			{
				owner.HandItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_HandItemDefinition[] value)
			{
				value = owner.HandItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPhysicalItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PhysicalItemDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PhysicalItemDefinition[] value)
			{
				owner.PhysicalItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PhysicalItemDefinition[] value)
			{
				value = owner.PhysicalItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ESpawnGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_SpawnGroupDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_SpawnGroupDefinition[] value)
			{
				owner.SpawnGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_SpawnGroupDefinition[] value)
			{
				value = owner.SpawnGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ETransparentMaterials_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_TransparentMaterialDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_TransparentMaterialDefinition[] value)
			{
				owner.TransparentMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_TransparentMaterialDefinition[] value)
			{
				value = owner.TransparentMaterials;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EVoxelMaterials_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_VoxelMaterialDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_VoxelMaterialDefinition[] value)
			{
				owner.VoxelMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_VoxelMaterialDefinition[] value)
			{
				value = owner.VoxelMaterials;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECharacters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_CharacterDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_CharacterDefinition[] value)
			{
				owner.Characters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_CharacterDefinition[] value)
			{
				value = owner.Characters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAnimations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AnimationDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AnimationDefinition[] value)
			{
				owner.Animations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AnimationDefinition[] value)
			{
				value = owner.Animations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDebris_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DebrisDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DebrisDefinition[] value)
			{
				owner.Debris = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DebrisDefinition[] value)
			{
				value = owner.Debris;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEdges_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EdgesDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EdgesDefinition[] value)
			{
				owner.Edges = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EdgesDefinition[] value)
			{
				value = owner.Edges;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EFactions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_FactionDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_FactionDefinition[] value)
			{
				owner.Factions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_FactionDefinition[] value)
			{
				value = owner.Factions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPrefabs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PrefabDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PrefabDefinition[] value)
			{
				owner.Prefabs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PrefabDefinition[] value)
			{
				value = owner.Prefabs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlueprintClasses_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BlueprintClassDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BlueprintClassDefinition[] value)
			{
				owner.BlueprintClasses = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BlueprintClassDefinition[] value)
			{
				value = owner.BlueprintClasses;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlueprintClassEntries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, BlueprintClassEntry[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in BlueprintClassEntry[] value)
			{
				owner.BlueprintClassEntries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out BlueprintClassEntry[] value)
			{
				value = owner.BlueprintClassEntries;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEnvironmentItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EnvironmentItemDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EnvironmentItemDefinition[] value)
			{
				owner.EnvironmentItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EnvironmentItemDefinition[] value)
			{
				value = owner.EnvironmentItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECompoundBlockTemplates_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_CompoundBlockTemplateDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_CompoundBlockTemplateDefinition[] value)
			{
				owner.CompoundBlockTemplates = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_CompoundBlockTemplateDefinition[] value)
			{
				value = owner.CompoundBlockTemplates;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ERespawnShips_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_RespawnShipDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_RespawnShipDefinition[] value)
			{
				owner.RespawnShips = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_RespawnShipDefinition[] value)
			{
				value = owner.RespawnShips;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDropContainers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DropContainerDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DropContainerDefinition[] value)
			{
				owner.DropContainers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DropContainerDefinition[] value)
			{
				value = owner.DropContainers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EWheelModels_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_WheelModelsDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_WheelModelsDefinition[] value)
			{
				owner.WheelModels = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_WheelModelsDefinition[] value)
			{
				value = owner.WheelModels;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAsteroidGenerators_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AsteroidGeneratorDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AsteroidGeneratorDefinition[] value)
			{
				owner.AsteroidGenerators = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AsteroidGeneratorDefinition[] value)
			{
				value = owner.AsteroidGenerators;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECategoryClasses_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_GuiBlockCategoryDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_GuiBlockCategoryDefinition[] value)
			{
				owner.CategoryClasses = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_GuiBlockCategoryDefinition[] value)
			{
				value = owner.CategoryClasses;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EShipBlueprints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ShipBlueprintDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ShipBlueprintDefinition[] value)
			{
				owner.ShipBlueprints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ShipBlueprintDefinition[] value)
			{
				value = owner.ShipBlueprints;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EWeapons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_WeaponDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_WeaponDefinition[] value)
			{
				owner.Weapons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_WeaponDefinition[] value)
			{
				value = owner.Weapons;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAmmos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AmmoDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AmmoDefinition[] value)
			{
				owner.Ammos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AmmoDefinition[] value)
			{
				value = owner.Ammos;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ESounds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AudioDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AudioDefinition[] value)
			{
				owner.Sounds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AudioDefinition[] value)
			{
				value = owner.Sounds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAssetModifiers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AssetModifierDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AssetModifierDefinition[] value)
			{
				owner.AssetModifiers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AssetModifierDefinition[] value)
			{
				value = owner.AssetModifiers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EMainMenuInventoryScenes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_MainMenuInventorySceneDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_MainMenuInventorySceneDefinition[] value)
			{
				owner.MainMenuInventoryScenes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_MainMenuInventorySceneDefinition[] value)
			{
				value = owner.MainMenuInventoryScenes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EVoxelHands_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_VoxelHandDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_VoxelHandDefinition[] value)
			{
				owner.VoxelHands = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_VoxelHandDefinition[] value)
			{
				value = owner.VoxelHands;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EMultiBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_MultiBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_MultiBlockDefinition[] value)
			{
				owner.MultiBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_MultiBlockDefinition[] value)
			{
				value = owner.MultiBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPrefabThrowers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PrefabThrowerDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PrefabThrowerDefinition[] value)
			{
				owner.PrefabThrowers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PrefabThrowerDefinition[] value)
			{
				value = owner.PrefabThrowers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ESoundCategories_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_SoundCategoryDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_SoundCategoryDefinition[] value)
			{
				owner.SoundCategories = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_SoundCategoryDefinition[] value)
			{
				value = owner.SoundCategories;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EShipSoundGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ShipSoundsDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ShipSoundsDefinition[] value)
			{
				owner.ShipSoundGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ShipSoundsDefinition[] value)
			{
				value = owner.ShipSoundGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDroneBehaviors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DroneBehaviorDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DroneBehaviorDefinition[] value)
			{
				owner.DroneBehaviors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DroneBehaviorDefinition[] value)
			{
				value = owner.DroneBehaviors;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EShipSoundSystem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ShipSoundSystemDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ShipSoundSystemDefinition value)
			{
				owner.ShipSoundSystem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ShipSoundSystemDefinition value)
			{
				value = owner.ShipSoundSystem;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EParticleEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ParticleEffect[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ParticleEffect[] value)
			{
				owner.ParticleEffects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ParticleEffect[] value)
			{
				value = owner.ParticleEffects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAIBehaviors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BehaviorTreeDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BehaviorTreeDefinition[] value)
			{
				owner.AIBehaviors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BehaviorTreeDefinition[] value)
			{
				value = owner.AIBehaviors;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EVoxelMapStorages_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_VoxelMapStorageDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_VoxelMapStorageDefinition[] value)
			{
				owner.VoxelMapStorages = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_VoxelMapStorageDefinition[] value)
			{
				value = owner.VoxelMapStorages;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ELCDTextures_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_LCDTextureDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_LCDTextureDefinition[] value)
			{
				owner.LCDTextures = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_LCDTextureDefinition[] value)
			{
				value = owner.LCDTextures;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBots_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BotDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BotDefinition[] value)
			{
				owner.Bots = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BotDefinition[] value)
			{
				value = owner.Bots;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPhysicalMaterials_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PhysicalMaterialDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PhysicalMaterialDefinition[] value)
			{
				owner.PhysicalMaterials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PhysicalMaterialDefinition[] value)
			{
				value = owner.PhysicalMaterials;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAiCommands_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AiCommandDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AiCommandDefinition[] value)
			{
				owner.AiCommands = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AiCommandDefinition[] value)
			{
				value = owner.AiCommands;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlockNavigationDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BlockNavigationDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BlockNavigationDefinition[] value)
			{
				owner.BlockNavigationDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BlockNavigationDefinition[] value)
			{
				value = owner.BlockNavigationDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECuttings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_CuttingDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_CuttingDefinition[] value)
			{
				owner.Cuttings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_CuttingDefinition[] value)
			{
				value = owner.Cuttings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EMaterialProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_MaterialPropertiesDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_MaterialPropertiesDefinition[] value)
			{
				owner.MaterialProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_MaterialPropertiesDefinition[] value)
			{
				value = owner.MaterialProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EControllerSchemas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ControllerSchemaDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ControllerSchemaDefinition[] value)
			{
				owner.ControllerSchemas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ControllerSchemaDefinition[] value)
			{
				value = owner.ControllerSchemas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECurveDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_CurveDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_CurveDefinition[] value)
			{
				owner.CurveDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_CurveDefinition[] value)
			{
				value = owner.CurveDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EAudioEffects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_AudioEffectDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_AudioEffectDefinition[] value)
			{
				owner.AudioEffects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_AudioEffectDefinition[] value)
			{
				value = owner.AudioEffects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEnvironmentItemsDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EnvironmentItemsDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EnvironmentItemsDefinition[] value)
			{
				owner.EnvironmentItemsDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EnvironmentItemsDefinition[] value)
			{
				value = owner.EnvironmentItemsDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEnvironmentItemsEntries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, EnvironmentItemsEntry[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in EnvironmentItemsEntry[] value)
			{
				owner.EnvironmentItemsEntries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out EnvironmentItemsEntry[] value)
			{
				value = owner.EnvironmentItemsEntries;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ECharacterNames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyCharacterName[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyCharacterName[] value)
			{
				owner.CharacterNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyCharacterName[] value)
			{
				value = owner.CharacterNames;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDecalGlobals_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DecalGlobalsDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DecalGlobalsDefinition value)
			{
				owner.DecalGlobals = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DecalGlobalsDefinition value)
			{
				value = owner.DecalGlobals;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDecals_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DecalDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DecalDefinition[] value)
			{
				owner.Decals = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DecalDefinition[] value)
			{
				value = owner.Decals;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEmissiveColors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EmissiveColorDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EmissiveColorDefinition[] value)
			{
				owner.EmissiveColors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EmissiveColorDefinition[] value)
			{
				value = owner.EmissiveColors;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEmissiveColorStatePresets_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EmissiveColorStatePresetDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EmissiveColorStatePresetDefinition[] value)
			{
				owner.EmissiveColorStatePresets = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EmissiveColorStatePresetDefinition[] value)
			{
				value = owner.EmissiveColorStatePresets;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPlanetGeneratorDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PlanetGeneratorDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PlanetGeneratorDefinition[] value)
			{
				owner.PlanetGeneratorDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PlanetGeneratorDefinition[] value)
			{
				value = owner.PlanetGeneratorDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EStatDefinitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_EntityStatDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_EntityStatDefinition[] value)
			{
				owner.StatDefinitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_EntityStatDefinition[] value)
			{
				value = owner.StatDefinitions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EGasProperties_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_GasProperties[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_GasProperties[] value)
			{
				owner.GasProperties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_GasProperties[] value)
			{
				value = owner.GasProperties;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EResourceDistributionGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ResourceDistributionGroup[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ResourceDistributionGroup[] value)
			{
				owner.ResourceDistributionGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ResourceDistributionGroup[] value)
			{
				value = owner.ResourceDistributionGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EComponentGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ComponentGroupDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ComponentGroupDefinition[] value)
			{
				owner.ComponentGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ComponentGroupDefinition[] value)
			{
				value = owner.ComponentGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EComponentBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyComponentBlockEntry[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyComponentBlockEntry[] value)
			{
				owner.ComponentBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyComponentBlockEntry[] value)
			{
				value = owner.ComponentBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPlanetPrefabs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PlanetPrefabDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PlanetPrefabDefinition[] value)
			{
				owner.PlanetPrefabs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PlanetPrefabDefinition[] value)
			{
				value = owner.PlanetPrefabs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEnvironmentGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyGroupedIds[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyGroupedIds[] value)
			{
				owner.EnvironmentGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyGroupedIds[] value)
			{
				value = owner.EnvironmentGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPirateAntennas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PirateAntennaDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PirateAntennaDefinition[] value)
			{
				owner.PirateAntennas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PirateAntennaDefinition[] value)
			{
				value = owner.PirateAntennas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EDestruction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_DestructionDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_DestructionDefinition value)
			{
				owner.Destruction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_DestructionDefinition value)
			{
				value = owner.Destruction;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEntityComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ComponentDefinitionBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ComponentDefinitionBase[] value)
			{
				owner.EntityComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ComponentDefinitionBase[] value)
			{
				value = owner.EntityComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EEntityContainers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ContainerDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ContainerDefinition[] value)
			{
				owner.EntityContainers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ContainerDefinition[] value)
			{
				value = owner.EntityContainers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EShadowTextureSets_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ShadowTextureSetDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ShadowTextureSetDefinition[] value)
			{
				owner.ShadowTextureSets = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ShadowTextureSetDefinition[] value)
			{
				value = owner.ShadowTextureSets;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EFonts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_FontDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_FontDefinition[] value)
			{
				owner.Fonts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_FontDefinition[] value)
			{
				value = owner.Fonts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EFlares_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_FlareDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_FlareDefinition[] value)
			{
				owner.Flares = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_FlareDefinition[] value)
			{
				value = owner.Flares;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EResearchBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ResearchBlockDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ResearchBlockDefinition[] value)
			{
				owner.ResearchBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ResearchBlockDefinition[] value)
			{
				value = owner.ResearchBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EResearchGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ResearchGroupDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ResearchGroupDefinition[] value)
			{
				owner.ResearchGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ResearchGroupDefinition[] value)
			{
				value = owner.ResearchGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EContractTypes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_ContractTypeDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_ContractTypeDefinition[] value)
			{
				owner.ContractTypes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_ContractTypeDefinition[] value)
			{
				value = owner.ContractTypes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EFactionNames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_FactionNameDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_FactionNameDefinition[] value)
			{
				owner.FactionNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_FactionNameDefinition[] value)
			{
				value = owner.FactionNames;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EPlanetWhitelists_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_PlanetWhitelistDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_PlanetWhitelistDefinition[] value)
			{
				owner.PlanetWhitelists = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_PlanetWhitelistDefinition[] value)
			{
				value = owner.PlanetWhitelists;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ERadialMenus_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_RadialMenu[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_RadialMenu[] value)
			{
				owner.RadialMenus = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_RadialMenu[] value)
			{
				value = owner.RadialMenus;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003EBlockVariantGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Definitions, MyObjectBuilder_BlockVariantGroup[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyObjectBuilder_BlockVariantGroup[] value)
			{
				owner.BlockVariantGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyObjectBuilder_BlockVariantGroup[] value)
			{
				value = owner.BlockVariantGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Definitions, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Definitions, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Definitions owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Definitions owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Definitions, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Definitions owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Definitions owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Definitions_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Definitions, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Definitions owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Definitions owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Definitions_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Definitions>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Definitions();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Definitions CreateInstance()
			{
				return new MyObjectBuilder_Definitions();
			}

			MyObjectBuilder_Definitions IActivator<MyObjectBuilder_Definitions>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlElement("Definition", Type = typeof(MyDefinitionXmlSerializer))]
		public MyObjectBuilder_DefinitionBase[] Definitions;

		[XmlArrayItem("GridCreator")]
		[ProtoMember(1)]
		public MyObjectBuilder_GridCreateToolDefinition[] GridCreators;

		[XmlArrayItem("AmmoMagazine", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AmmoMagazineDefinition>))]
		[ProtoMember(3)]
		public MyObjectBuilder_AmmoMagazineDefinition[] AmmoMagazines;

		[XmlArrayItem("Blueprint", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BlueprintDefinition>))]
		[ProtoMember(5)]
		public MyObjectBuilder_BlueprintDefinition[] Blueprints;

		[XmlArrayItem("Component", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ComponentDefinition>))]
		[ProtoMember(7)]
		public MyObjectBuilder_ComponentDefinition[] Components;

		[XmlArrayItem("ContainerType", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ContainerTypeDefinition>))]
		[ProtoMember(9)]
		public MyObjectBuilder_ContainerTypeDefinition[] ContainerTypes;

		[XmlArrayItem("Definition", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CubeBlockDefinition>))]
		[ProtoMember(11)]
		public MyObjectBuilder_CubeBlockDefinition[] CubeBlocks;

		[XmlArrayItem("BlockPosition")]
		[ProtoMember(13)]
		public MyBlockPosition[] BlockPositions;

		[ProtoMember(15)]
		[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_Configuration>))]
		public MyObjectBuilder_Configuration Configuration;

		[ProtoMember(17)]
		[XmlElement("Environment", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentDefinition>))]
		public MyObjectBuilder_EnvironmentDefinition[] Environments;

		[XmlArrayItem("GlobalEvent", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_GlobalEventDefinition>))]
		[ProtoMember(19)]
		public MyObjectBuilder_GlobalEventDefinition[] GlobalEvents;

		[XmlArrayItem("HandItem", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_HandItemDefinition>))]
		[ProtoMember(21)]
		public MyObjectBuilder_HandItemDefinition[] HandItems;

		[XmlArrayItem("PhysicalItem", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PhysicalItemDefinition>))]
		[ProtoMember(23)]
		public MyObjectBuilder_PhysicalItemDefinition[] PhysicalItems;

		[XmlArrayItem("SpawnGroup", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_SpawnGroupDefinition>))]
		[ProtoMember(25)]
		public MyObjectBuilder_SpawnGroupDefinition[] SpawnGroups;

		[XmlArrayItem("TransparentMaterial", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_TransparentMaterialDefinition>))]
		[ProtoMember(27)]
		public MyObjectBuilder_TransparentMaterialDefinition[] TransparentMaterials;

		[XmlArrayItem("VoxelMaterial", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_VoxelMaterialDefinition>))]
		[ProtoMember(29)]
		public MyObjectBuilder_VoxelMaterialDefinition[] VoxelMaterials;

		[XmlArrayItem("Character", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CharacterDefinition>))]
		[ProtoMember(31)]
		public MyObjectBuilder_CharacterDefinition[] Characters;

		[XmlArrayItem("Animation", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AnimationDefinition>))]
		[ProtoMember(33)]
		public MyObjectBuilder_AnimationDefinition[] Animations;

		[XmlArrayItem("Debris", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_DebrisDefinition>))]
		[ProtoMember(35)]
		public MyObjectBuilder_DebrisDefinition[] Debris;

		[XmlArrayItem("Edges", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EdgesDefinition>))]
		[ProtoMember(37)]
		public MyObjectBuilder_EdgesDefinition[] Edges;

		[XmlArrayItem("Faction", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_FactionDefinition>))]
		[ProtoMember(39)]
		public MyObjectBuilder_FactionDefinition[] Factions;

		[XmlArrayItem("Prefab", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PrefabDefinition>))]
		[ProtoMember(41)]
		public MyObjectBuilder_PrefabDefinition[] Prefabs;

		[XmlArrayItem("Class")]
		[ProtoMember(43)]
		public MyObjectBuilder_BlueprintClassDefinition[] BlueprintClasses;

		[XmlArrayItem("Entry")]
		[ProtoMember(45)]
		public BlueprintClassEntry[] BlueprintClassEntries;

		[XmlArrayItem("EnvironmentItem", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentItemDefinition>))]
		[ProtoMember(47)]
		public MyObjectBuilder_EnvironmentItemDefinition[] EnvironmentItems;

		[XmlArrayItem("Template", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CompoundBlockTemplateDefinition>))]
		[ProtoMember(49)]
		public MyObjectBuilder_CompoundBlockTemplateDefinition[] CompoundBlockTemplates;

		[XmlArrayItem("Ship", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_RespawnShipDefinition>))]
		[ProtoMember(51)]
		public MyObjectBuilder_RespawnShipDefinition[] RespawnShips;

		[XmlArrayItem("DropContainer", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_DropContainerDefinition>))]
		[ProtoMember(53)]
		public MyObjectBuilder_DropContainerDefinition[] DropContainers;

		[XmlArrayItem("WheelModel", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_WheelModelsDefinition>))]
		[ProtoMember(55)]
		public MyObjectBuilder_WheelModelsDefinition[] WheelModels;

		[XmlArrayItem("AsteroidGenerator", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AsteroidGeneratorDefinition>))]
		[ProtoMember(57)]
		public MyObjectBuilder_AsteroidGeneratorDefinition[] AsteroidGenerators;

		[XmlArrayItem("Category", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_GuiBlockCategoryDefinition>))]
		[ProtoMember(59)]
		public MyObjectBuilder_GuiBlockCategoryDefinition[] CategoryClasses;

		[XmlArrayItem("ShipBlueprint", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ShipBlueprintDefinition>))]
		[ProtoMember(61)]
		public MyObjectBuilder_ShipBlueprintDefinition[] ShipBlueprints;

		[XmlArrayItem("Weapon", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_WeaponDefinition>))]
		[ProtoMember(63)]
		public MyObjectBuilder_WeaponDefinition[] Weapons;

		[XmlArrayItem("Ammo", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AmmoDefinition>))]
		[ProtoMember(65)]
		public MyObjectBuilder_AmmoDefinition[] Ammos;

		[XmlArrayItem("Sound", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AudioDefinition>))]
		[ProtoMember(67)]
		public MyObjectBuilder_AudioDefinition[] Sounds;

		[XmlArrayItem("AssetModifier", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AssetModifierDefinition>))]
		[ProtoMember(69)]
		public MyObjectBuilder_AssetModifierDefinition[] AssetModifiers;

		[XmlArrayItem("MainMenuInventoryScene", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_MainMenuInventorySceneDefinition>))]
		[ProtoMember(71)]
		public MyObjectBuilder_MainMenuInventorySceneDefinition[] MainMenuInventoryScenes;

		[XmlArrayItem("VoxelHand", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_VoxelHandDefinition>))]
		[ProtoMember(73)]
		public MyObjectBuilder_VoxelHandDefinition[] VoxelHands;

		[XmlArrayItem("MultiBlock", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_MultiBlockDefinition>))]
		[ProtoMember(75)]
		public MyObjectBuilder_MultiBlockDefinition[] MultiBlocks;

		[XmlArrayItem("PrefabThrower", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PrefabThrowerDefinition>))]
		[ProtoMember(77)]
		public MyObjectBuilder_PrefabThrowerDefinition[] PrefabThrowers;

		[XmlArrayItem("SoundCategory", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_SoundCategoryDefinition>))]
		[ProtoMember(79)]
		public MyObjectBuilder_SoundCategoryDefinition[] SoundCategories;

		[XmlArrayItem("ShipSoundGroup", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ShipSoundsDefinition>))]
		[ProtoMember(81)]
		public MyObjectBuilder_ShipSoundsDefinition[] ShipSoundGroups;

		[ProtoMember(83)]
		[XmlArrayItem("DroneBehavior", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_DroneBehaviorDefinition>))]
		public MyObjectBuilder_DroneBehaviorDefinition[] DroneBehaviors;

		[XmlElement("ShipSoundSystem", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ShipSoundSystemDefinition>))]
		[ProtoMember(85)]
		public MyObjectBuilder_ShipSoundSystemDefinition ShipSoundSystem;

		[XmlArrayItem("ParticleEffect", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ParticleEffect>))]
		[ProtoMember(87)]
		public MyObjectBuilder_ParticleEffect[] ParticleEffects;

		[XmlArrayItem("AIBehavior", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BehaviorTreeDefinition>))]
		[ProtoMember(89)]
		public MyObjectBuilder_BehaviorTreeDefinition[] AIBehaviors;

		[XmlArrayItem("VoxelMapStorage", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_VoxelMapStorageDefinition>))]
		[ProtoMember(91)]
		public MyObjectBuilder_VoxelMapStorageDefinition[] VoxelMapStorages;

		[XmlArrayItem("LCDTextureDefinition", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_LCDTextureDefinition>))]
		[ProtoMember(93)]
		public MyObjectBuilder_LCDTextureDefinition[] LCDTextures;

		[XmlArrayItem("Bot", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BotDefinition>))]
		[ProtoMember(95)]
		public MyObjectBuilder_BotDefinition[] Bots;

		[XmlArrayItem("PhysicalMaterial", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PhysicalMaterialDefinition>))]
		[ProtoMember(99)]
		public MyObjectBuilder_PhysicalMaterialDefinition[] PhysicalMaterials;

		[XmlArrayItem("AiCommand", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AiCommandDefinition>))]
		[ProtoMember(101)]
		public MyObjectBuilder_AiCommandDefinition[] AiCommands;

		[XmlArrayItem("NavDef", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BlockNavigationDefinition>))]
		[ProtoMember(103)]
		public MyObjectBuilder_BlockNavigationDefinition[] BlockNavigationDefinitions;

		[XmlArrayItem("Cutting", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CuttingDefinition>))]
		[ProtoMember(105)]
		public MyObjectBuilder_CuttingDefinition[] Cuttings;

		[XmlArrayItem("Properties", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_MaterialPropertiesDefinition>))]
		[ProtoMember(107)]
		public MyObjectBuilder_MaterialPropertiesDefinition[] MaterialProperties;

		[XmlArrayItem("ControllerSchema", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ControllerSchemaDefinition>))]
		[ProtoMember(109)]
		public MyObjectBuilder_ControllerSchemaDefinition[] ControllerSchemas;

		[XmlArrayItem("SoundCurve", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CurveDefinition>))]
		[ProtoMember(111)]
		public MyObjectBuilder_CurveDefinition[] CurveDefinitions;

		[XmlArrayItem("Effect", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_AudioEffectDefinition>))]
		[ProtoMember(113)]
		public MyObjectBuilder_AudioEffectDefinition[] AudioEffects;

		[XmlArrayItem("Definition", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentItemsDefinition>))]
		[ProtoMember(115)]
		public MyObjectBuilder_EnvironmentItemsDefinition[] EnvironmentItemsDefinitions;

		[XmlArrayItem("Entry")]
		[ProtoMember(117)]
		public EnvironmentItemsEntry[] EnvironmentItemsEntries;

		[XmlArrayItem("Entry")]
		[ProtoMember(121)]
		public MyCharacterName[] CharacterNames;

		[ProtoMember(125)]
		public MyObjectBuilder_DecalGlobalsDefinition DecalGlobals;

		[XmlArrayItem("Decal")]
		[ProtoMember(127)]
		public MyObjectBuilder_DecalDefinition[] Decals;

		[XmlArrayItem("EmissiveColor")]
		[ProtoMember(129)]
		public MyObjectBuilder_EmissiveColorDefinition[] EmissiveColors;

		[XmlArrayItem("EmissiveColorStatePreset")]
		[ProtoMember(131)]
		public MyObjectBuilder_EmissiveColorStatePresetDefinition[] EmissiveColorStatePresets;

		[XmlArrayItem("PlanetGeneratorDefinition", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PlanetGeneratorDefinition>))]
		[ProtoMember(133)]
		public MyObjectBuilder_PlanetGeneratorDefinition[] PlanetGeneratorDefinitions;

		[XmlArrayItem("Stat")]
		[ProtoMember(137)]
		public MyObjectBuilder_EntityStatDefinition[] StatDefinitions;

		[XmlArrayItem("Gas")]
		[ProtoMember(139)]
		public MyObjectBuilder_GasProperties[] GasProperties;

		[XmlArrayItem("DistributionGroup")]
		[ProtoMember(141)]
		public MyObjectBuilder_ResourceDistributionGroup[] ResourceDistributionGroups;

		[XmlArrayItem("Group", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ComponentGroupDefinition>))]
		[ProtoMember(143)]
		public MyObjectBuilder_ComponentGroupDefinition[] ComponentGroups;

		[XmlArrayItem("Block")]
		[ProtoMember(147)]
		public MyComponentBlockEntry[] ComponentBlocks;

		[XmlArrayItem("PlanetPrefab", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PlanetPrefabDefinition>))]
		[ProtoMember(149)]
		public MyObjectBuilder_PlanetPrefabDefinition[] PlanetPrefabs;

		[XmlArrayItem("Group")]
		[ProtoMember(151)]
		public MyGroupedIds[] EnvironmentGroups;

		[XmlArrayItem("Antenna", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PirateAntennaDefinition>))]
		[ProtoMember(157)]
		public MyObjectBuilder_PirateAntennaDefinition[] PirateAntennas;

		[ProtoMember(159)]
		public MyObjectBuilder_DestructionDefinition Destruction;

		[XmlArrayItem("EntityComponent", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ComponentDefinitionBase>))]
		[ProtoMember(161)]
		public MyObjectBuilder_ComponentDefinitionBase[] EntityComponents;

		[XmlArrayItem("Container", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ContainerDefinition>))]
		[ProtoMember(163)]
		public MyObjectBuilder_ContainerDefinition[] EntityContainers;

		[ProtoMember(165)]
		[XmlArrayItem("ShadowTextureSet")]
		public MyObjectBuilder_ShadowTextureSetDefinition[] ShadowTextureSets;

		[XmlArrayItem("Font", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_FontDefinition>))]
		[ProtoMember(167)]
		public MyObjectBuilder_FontDefinition[] Fonts;

		[ProtoMember(169)]
		[XmlArrayItem("Definition", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_FlareDefinition>))]
		public MyObjectBuilder_FlareDefinition[] Flares;

		[XmlArrayItem("ResearchBlock", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ResearchBlockDefinition>))]
		[ProtoMember(171)]
		public MyObjectBuilder_ResearchBlockDefinition[] ResearchBlocks;

		[XmlArrayItem("ResearchGroup", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ResearchGroupDefinition>))]
		[ProtoMember(173)]
		public MyObjectBuilder_ResearchGroupDefinition[] ResearchGroups;

		[XmlArrayItem("ContractType", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ContractTypeDefinition>))]
		[ProtoMember(175)]
		public MyObjectBuilder_ContractTypeDefinition[] ContractTypes;

		[XmlArrayItem("FactionName", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_FactionNameDefinition>))]
		[ProtoMember(177)]
		public MyObjectBuilder_FactionNameDefinition[] FactionNames;

		[XmlArrayItem("PlanetWhitelist", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_PlanetWhitelistDefinition>))]
		[ProtoMember(179)]
		public MyObjectBuilder_PlanetWhitelistDefinition[] PlanetWhitelists;

		[XmlArrayItem("RadialMenu", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_RadialMenu>))]
		[ProtoMember(182)]
		public MyObjectBuilder_RadialMenu[] RadialMenus;

		[XmlArrayItem("BlockVariantGroup", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_BlockVariantGroup>))]
		[ProtoMember(185)]
		public MyObjectBuilder_BlockVariantGroup[] BlockVariantGroups;
	}
}
