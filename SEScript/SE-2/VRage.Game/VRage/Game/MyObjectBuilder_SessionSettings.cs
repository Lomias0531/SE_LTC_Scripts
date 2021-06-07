using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract(SkipConstructor = true)]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SessionSettings : MyObjectBuilder_Base
	{
		[Flags]
		public enum ExperimentalReason : long
		{
			ExperimentalMode = 0x2,
			MaxPlayers = 0x4,
			EnvironmentHostility = 0x8,
			ProceduralDensity = 0x10,
			FloraDensity = 0x20,
			WorldSizeKm = 0x40,
			SpawnShipTimeMultiplier = 0x80,
			SunRotationIntervalMinutes = 0x100,
			MaxFloatingObjects = 0x200,
			PhysicsIterations = 0x400,
			SyncDistance = 0x800,
			ViewDistance = 0x1000,
			BlockLimitsEnabled = 0x2000,
			TotalPCU = 0x4000,
			AutoHealing = 0x8000,
			RespawnShipDelete = 0x10000,
			EnableSpectator = 0x20000,
			EnableCopyPaste = 0x40000,
			ShowPlayerNamesOnHud = 0x80000,
			ResetOwnership = 0x100000,
			ThrusterDamage = 0x200000,
			PermanentDeath = 0x400000,
			WeaponsEnabled = 0x800000,
			CargoShipsEnabled = 0x1000000,
			DestructibleBlocks = 0x2000000,
			EnableIngameScripts = 0x4000000,
			EnableToolShake = 0x8000000,
			EnableEncounters = 0x10000000,
			Enable3rdPersonView = 0x20000000,
			EnableOxygen = 0x40000000,
			EnableOxygenPressurization = 0x80000000,
			EnableSunRotation = 0x100000000,
			EnableConvertToStation = 0x200000000,
			StationVoxelSupport = 0x400000000,
			EnableJetpack = 0x800000000,
			SpawnWithTools = 0x1000000000,
			StartInRespawnScreen = 0x2000000000,
			EnableVoxelDestruction = 0x4000000000,
			EnableDrones = 0x8000000000,
			EnableWolfs = 0x10000000000,
			EnableSpiders = 0x20000000000,
			EnableRemoteBlockRemoval = 0x40000000000,
			EnableSubgridDamage = 0x80000000000,
			EnableTurretsFriendlyFire = 0x100000000000,
			EnableContainerDrops = 0x200000000000,
			EnableRespawnShips = 0x400000000000,
			AdaptiveSimulationQuality = 0x800000000000,
			ExperimentalTurnedOnInConfiguration = 0x1000000000000,
			InsufficientHardware = 0x2000000000000,
			Mods = 0x4000000000000,
			Plugins = 0x8000000000000,
			SupergriddingEnabled = 0x10000000000000,
			ReasonMax = -2147483648L
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EGameMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyGameModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyGameModeEnum value)
			{
				owner.GameMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyGameModeEnum value)
			{
				value = owner.GameMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EInventorySizeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.InventorySizeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.InventorySizeMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EBlocksInventorySizeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.BlocksInventorySizeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.BlocksInventorySizeMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAssemblerSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.AssemblerSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.AssemblerSpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAssemblerEfficiencyMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.AssemblerEfficiencyMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.AssemblerEfficiencyMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ERefinerySpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.RefinerySpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.RefinerySpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EOnlineMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyOnlineModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyOnlineModeEnum value)
			{
				owner.OnlineMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyOnlineModeEnum value)
			{
				value = owner.OnlineMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in short value)
			{
				owner.MaxPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out short value)
			{
				value = owner.MaxPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxFloatingObjects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in short value)
			{
				owner.MaxFloatingObjects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out short value)
			{
				value = owner.MaxFloatingObjects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxBackupSaves_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in short value)
			{
				owner.MaxBackupSaves = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out short value)
			{
				value = owner.MaxBackupSaves;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxGridSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxGridSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxGridSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxBlocksPerPlayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxBlocksPerPlayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxBlocksPerPlayer;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ETotalPCU_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.TotalPCU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.TotalPCU;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPiratePCU_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.PiratePCU = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.PiratePCU;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxFactionsCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxFactionsCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxFactionsCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EBlockLimitsEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyBlockLimitsEnabledEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyBlockLimitsEnabledEnum value)
			{
				owner.BlockLimitsEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyBlockLimitsEnabledEnum value)
			{
				value = owner.BlockLimitsEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableRemoteBlockRemoval_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableRemoteBlockRemoval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableRemoteBlockRemoval;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnvironmentHostility_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyEnvironmentHostilityEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyEnvironmentHostilityEnum value)
			{
				owner.EnvironmentHostility = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyEnvironmentHostilityEnum value)
			{
				value = owner.EnvironmentHostility;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAutoHealing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.AutoHealing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.AutoHealing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableCopyPaste_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableCopyPaste = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableCopyPaste;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EWeaponsEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.WeaponsEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.WeaponsEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EShowPlayerNamesOnHud_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ShowPlayerNamesOnHud = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ShowPlayerNamesOnHud;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EThrusterDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ThrusterDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ThrusterDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ECargoShipsEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.CargoShipsEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.CargoShipsEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSpectator_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSpectator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSpectator;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EWorldSizeKm_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.WorldSizeKm = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.WorldSizeKm;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ERespawnShipDelete_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.RespawnShipDelete = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.RespawnShipDelete;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EResetOwnership_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ResetOwnership = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ResetOwnership;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EWelderSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.WelderSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.WelderSpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EGrinderSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.GrinderSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.GrinderSpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ERealisticSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.RealisticSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.RealisticSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EHackSpeedMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.HackSpeedMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.HackSpeedMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPermanentDeath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool? value)
			{
				owner.PermanentDeath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool? value)
			{
				value = owner.PermanentDeath;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAutoSaveInMinutes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in uint value)
			{
				owner.AutoSaveInMinutes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out uint value)
			{
				value = owner.AutoSaveInMinutes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSaving_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSaving = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSaving;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EInfiniteAmmo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.InfiniteAmmo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.InfiniteAmmo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableContainerDrops_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableContainerDrops = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableContainerDrops;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ESpawnShipTimeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.SpawnShipTimeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.SpawnShipTimeMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EProceduralDensity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.ProceduralDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.ProceduralDensity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EProceduralSeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.ProceduralSeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.ProceduralSeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EDestructibleBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.DestructibleBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.DestructibleBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableIngameScripts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableIngameScripts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableIngameScripts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EViewDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.ViewDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.ViewDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableToolShake_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableToolShake = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableToolShake;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EVoxelGeneratorVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.VoxelGeneratorVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.VoxelGeneratorVersion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableOxygen_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableOxygen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableOxygen;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableOxygenPressurization_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableOxygenPressurization = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableOxygenPressurization;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnable3rdPersonView_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.Enable3rdPersonView = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.Enable3rdPersonView;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableEncounters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableEncounters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableEncounters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableConvertToStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableConvertToStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableConvertToStation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStationVoxelSupport_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.StationVoxelSupport = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.StationVoxelSupport;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSunRotation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSunRotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSunRotation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableRespawnShips_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableRespawnShips = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableRespawnShips;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EScenarioEditMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ScenarioEditMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ScenarioEditMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EScenario_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.Scenario = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.Scenario;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ECanJoinRunning_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.CanJoinRunning = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.CanJoinRunning;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPhysicsIterations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.PhysicsIterations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.PhysicsIterations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ESunRotationIntervalMinutes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.SunRotationIntervalMinutes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.SunRotationIntervalMinutes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableJetpack_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableJetpack = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableJetpack;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ESpawnWithTools_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.SpawnWithTools = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.SpawnWithTools;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStartInRespawnScreen_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.StartInRespawnScreen = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.StartInRespawnScreen;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableVoxelDestruction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableVoxelDestruction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableVoxelDestruction;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxDrones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxDrones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxDrones;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableDrones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableDrones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableDrones;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableWolfs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableWolfs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableWolfs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSpiders_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSpiders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSpiders;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EFloraDensityMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.FloraDensityMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.FloraDensityMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableStructuralSimulation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableStructuralSimulation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableStructuralSimulation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxActiveFracturePieces_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxActiveFracturePieces = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxActiveFracturePieces;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EBlockTypeLimits_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, SerializableDictionary<string, short>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in SerializableDictionary<string, short> value)
			{
				owner.BlockTypeLimits = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out SerializableDictionary<string, short> value)
			{
				value = owner.BlockTypeLimits;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableScripterRole_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableScripterRole = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableScripterRole;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMinDropContainerRespawnTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MinDropContainerRespawnTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MinDropContainerRespawnTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EMaxDropContainerRespawnTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.MaxDropContainerRespawnTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.MaxDropContainerRespawnTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableTurretsFriendlyFire_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableTurretsFriendlyFire = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableTurretsFriendlyFire;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSubgridDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSubgridDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSubgridDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ESyncDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.SyncDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.SyncDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EExperimentalMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ExperimentalMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ExperimentalMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAdaptiveSimulationQuality_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.AdaptiveSimulationQuality = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.AdaptiveSimulationQuality;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableVoxelHand_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableVoxelHand = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableVoxelHand;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ERemoveOldIdentitiesH_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.RemoveOldIdentitiesH = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.RemoveOldIdentitiesH;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ETrashRemovalEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.TrashRemovalEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.TrashRemovalEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStopGridsPeriodMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.StopGridsPeriodMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.StopGridsPeriodMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ETrashFlagsValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.TrashFlagsValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.TrashFlagsValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAFKTimeountMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.AFKTimeountMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.AFKTimeountMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EBlockCountThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.BlockCountThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.BlockCountThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPlayerDistanceThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.PlayerDistanceThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.PlayerDistanceThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EOptimalGridCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.OptimalGridCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.OptimalGridCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPlayerInactivityThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.PlayerInactivityThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.PlayerInactivityThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EPlayerCharacterRemovalThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.PlayerCharacterRemovalThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.PlayerCharacterRemovalThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EVoxelTrashRemovalEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.VoxelTrashRemovalEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.VoxelTrashRemovalEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EVoxelPlayerDistanceThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.VoxelPlayerDistanceThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.VoxelPlayerDistanceThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EVoxelGridDistanceThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.VoxelGridDistanceThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.VoxelGridDistanceThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EVoxelAgeThreshold_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.VoxelAgeThreshold = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.VoxelAgeThreshold;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableResearch_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableResearch = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableResearch;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableGoodBotHints_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableGoodBotHints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableGoodBotHints;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EOptimalSpawnDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.OptimalSpawnDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.OptimalSpawnDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableAutorespawn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableAutorespawn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableAutorespawn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableBountyContracts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableBountyContracts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableBountyContracts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableSupergridding_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableSupergridding = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableSupergridding;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEnableEconomy_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.EnableEconomy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.EnableEconomy;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EDepositsCountCoefficient_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.DepositsCountCoefficient = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.DepositsCountCoefficient;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EDepositSizeDenominator_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.DepositSizeDenominator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.DepositSizeDenominator;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EHarvestRatioMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in float value)
			{
				owner.HarvestRatioMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out float value)
			{
				value = owner.HarvestRatioMultiplier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ETradeFactionsCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.TradeFactionsCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.TradeFactionsCount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStationsDistanceInnerRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in double value)
			{
				owner.StationsDistanceInnerRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out double value)
			{
				value = owner.StationsDistanceInnerRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStationsDistanceOuterRadiusStart_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in double value)
			{
				owner.StationsDistanceOuterRadiusStart = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out double value)
			{
				value = owner.StationsDistanceOuterRadiusStart;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EStationsDistanceOuterRadiusEnd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in double value)
			{
				owner.StationsDistanceOuterRadiusEnd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out double value)
			{
				value = owner.StationsDistanceOuterRadiusEnd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EEconomyTickInSeconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in int value)
			{
				owner.EconomyTickInSeconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out int value)
			{
				value = owner.EconomyTickInSeconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003Em_experimentalReasonInited_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.m_experimentalReasonInited = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.m_experimentalReasonInited;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003Em_experimentalReason_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, ExperimentalReason>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in ExperimentalReason value)
			{
				owner.m_experimentalReason = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out ExperimentalReason value)
			{
				value = owner.m_experimentalReason;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EAutoSave_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.AutoSave = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.AutoSave;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EClientCanSave_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in bool value)
			{
				owner.ClientCanSave = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out bool value)
			{
				value = owner.ClientCanSave;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ETrashFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionSettings, MyTrashRemovalFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyTrashRemovalFlags value)
			{
				owner.TrashFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyTrashRemovalFlags value)
			{
				value = owner.TrashFlags;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SessionSettings_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionSettings owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionSettings owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_SessionSettings_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionSettings>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionSettings CreateInstance()
			{
				return new MyObjectBuilder_SessionSettings();
			}

			MyObjectBuilder_SessionSettings IActivator<MyObjectBuilder_SessionSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlIgnore]
		public const uint DEFAULT_AUTOSAVE_IN_MINUTES = 5u;

		[ProtoMember(1)]
		[Display(Name = "Game Mode", Description = "The type of the game mode.")]
		[Category("Others")]
		[GameRelation(Game.Shared)]
		public MyGameModeEnum GameMode;

		[ProtoMember(3)]
		[Display(Name = "Characters Inventory Size", Description = "The multiplier for inventory size for the characters.")]
		[Category("Multipliers")]
		[GameRelation(Game.Shared)]
		[Range(1, 100)]
		public float InventorySizeMultiplier = 3f;

		[ProtoMember(5)]
		[Display(Name = "Blocks Inventory Size", Description = "The multiplier for inventory size for the blocks.")]
		[Category("Multipliers")]
		[GameRelation(Game.Shared)]
		[Range(1, 100)]
		public float BlocksInventorySizeMultiplier = 1f;

		[ProtoMember(7)]
		[Display(Name = "Assembler Speed", Description = "The multiplier for assembler speed.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(1, 100)]
		public float AssemblerSpeedMultiplier = 3f;

		[ProtoMember(9)]
		[Display(Name = "Assembler Efficiency", Description = "The multiplier for assembler efficiency.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(1, 100)]
		public float AssemblerEfficiencyMultiplier = 3f;

		[ProtoMember(11)]
		[Display(Name = "Refinery Speed", Description = "The multiplier for refinery speed.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(1, 100)]
		public float RefinerySpeedMultiplier = 3f;

		[ProtoMember(13)]
		public MyOnlineModeEnum OnlineMode;

		[ProtoMember(15)]
		[Display(Name = "Max Players", Description = "The maximum number of connected players.")]
		[GameRelation(Game.Shared)]
		[Category("Players")]
		[Range(2, 64)]
		public short MaxPlayers = 4;

		[ProtoMember(17)]
		[Display(Name = "Max Floating Objects", Description = "The maximum number of existing floating objects.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(2, 1024)]
		[Category("Environment")]
		public short MaxFloatingObjects = 100;

		[ProtoMember(19)]
		[Display(Name = "Max Backup Saves", Description = "The maximum number of backup saves.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, 1000)]
		[Category("Others")]
		public short MaxBackupSaves = 5;

		[ProtoMember(21)]
		[Display(Name = "Max Grid Blocks", Description = "The maximum number of blocks in one grid.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, int.MaxValue)]
		[Category("Block Limits")]
		public int MaxGridSize = 50000;

		[ProtoMember(23)]
		[Display(Name = "Max Blocks per Player", Description = "The maximum number of blocks per player.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, int.MaxValue)]
		[Category("Block Limits")]
		public int MaxBlocksPerPlayer = 100000;

		[ProtoMember(25)]
		[Display(Name = "World PCU", Description = "The total number of Performance Cost Units in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, int.MaxValue)]
		[Category("Block Limits")]
		public int TotalPCU = 600000;

		[ProtoMember(27)]
		[Display(Name = "Pirate PCU", Description = "Number of Performance Cost Units allocated for pirate faction.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, int.MaxValue)]
		[Category("Block Limits")]
		public int PiratePCU = 50000;

		[ProtoMember(29)]
		[Display(Name = "Max Factions Count", Description = "The maximum number of existing factions in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, int.MaxValue)]
		[Category("Block Limits")]
		public int MaxFactionsCount;

		[ProtoMember(31)]
		[Display(Name = "Block Limits Mode", Description = "Defines block limits mode.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Block Limits")]
		public MyBlockLimitsEnabledEnum BlockLimitsEnabled;

		[ProtoMember(33)]
		[Display(Name = "Enable Remote Grid Removal", Description = "Enables possibility to remove grid remotely from the world by an author.")]
		[Category("Others")]
		[GameRelation(Game.SpaceEngineers)]
		public bool EnableRemoteBlockRemoval = true;

		[ProtoMember(35)]
		[Display(Name = "Environment Hostility", Description = "Defines hostility of the environment.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public MyEnvironmentHostilityEnum EnvironmentHostility = MyEnvironmentHostilityEnum.NORMAL;

		[ProtoMember(37)]
		[Display(Name = "Auto Healing", Description = "Auto-healing heals players only in oxygen environments and during periods of not taking damage.")]
		[Category("Players")]
		[GameRelation(Game.SpaceEngineers)]
		public bool AutoHealing = true;

		[ProtoMember(39)]
		[Display(Name = "Enable Copy & Paste", Description = "Enables copy and paste feature.")]
		[GameRelation(Game.Shared)]
		[Category("Players")]
		public bool EnableCopyPaste = true;

		[ProtoMember(41)]
		[Display(Name = "Enable Weapons", Description = "Enables weapons.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool WeaponsEnabled = true;

		[ProtoMember(43)]
		[Display(Name = "Show Player Names on HUD", Description = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool ShowPlayerNamesOnHud = true;

		[ProtoMember(45)]
		[Display(Name = "Enable Thruster Damage", Description = "Enables thruster damage.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool ThrusterDamage = true;

		[ProtoMember(47)]
		[Display(Name = "Enable Cargo Ships", Description = "Enables spawning of cargo ships.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool CargoShipsEnabled = true;

		[ProtoMember(49)]
		[Display(Name = "Enable Spectator Camera", Description = "Enables spectator camera.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		public bool EnableSpectator;

		/// <summary>
		/// Size of the edge of the world area cube.
		/// Don't use directly, as it is error-prone (it's km instead of m and edge size instead of half-extent)
		/// Rather use MyEntities.WorldHalfExtent()
		/// </summary>
		[ProtoMember(51)]
		[Display(Name = "World Size [km]", Description = "Defines the size of the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(0, int.MaxValue)]
		public int WorldSizeKm;

		[ProtoMember(53)]
		[Display(Name = "Remove Respawn Ships on Logoff", Description = "When enabled respawn ship is removed after player logout.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool RespawnShipDelete;

		[ProtoMember(55)]
		[Display(Name = "Reset Ownership", Description = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool ResetOwnership;

		[ProtoMember(57)]
		[Display(Name = "Welder Speed", Description = "The multiplier for welder speed.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, 100)]
		public float WelderSpeedMultiplier = 2f;

		[ProtoMember(59)]
		[Display(Name = "Grinder Speed", Description = "The multiplier for grinder speed.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, 100)]
		public float GrinderSpeedMultiplier = 2f;

		[ProtoMember(61)]
		[Display(Name = "Enable Realistic Sound", Description = "Enables realistic sounds.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool RealisticSound;

		[ProtoMember(63)]
		[Display(Name = "Hacking Speed", Description = "The multiplier for hacking speed.")]
		[Category("Multipliers")]
		[GameRelation(Game.SpaceEngineers)]
		[Range(0, 100)]
		public float HackSpeedMultiplier = 0.33f;

		[ProtoMember(65)]
		[Display(Name = "Permanent Death", Description = "Enables permanent death.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool? PermanentDeath = false;

		[ProtoMember(67)]
		[Display(Name = "Autosave Interval [mins]", Description = "Defines autosave interval.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		[Range(0.0, 4294967295.0)]
		public uint AutoSaveInMinutes = 5u;

		[ProtoMember(69)]
		[Display(Name = "Enable Saving from Menu", Description = "Enables saving from the menu.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		public bool EnableSaving = true;

		[ProtoMember(71)]
		[Display(Name = "Enable Infinite Ammunition in Survival", Description = "Enables infinite ammunition in survival game mode.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		public bool InfiniteAmmo;

		[ProtoMember(73)]
		[Display(Name = "Enable Drop Containers", Description = "Enables drop containers (unknown signals).")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		public bool EnableContainerDrops = true;

		[ProtoMember(75)]
		[Display(Name = "Respawn Ship Time Multiplier", Description = "The multiplier for respawn ship timer.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		[Range(0, 100)]
		public float SpawnShipTimeMultiplier;

		[ProtoMember(77)]
		[Display(Name = "Procedural Density", Description = "Defines density of the procedurally generated content.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(0, 1)]
		public float ProceduralDensity;

		[ProtoMember(79)]
		[Display(Name = "Procedural Seed", Description = "Defines unique starting seed for the procedurally generated content.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(int.MinValue, int.MaxValue)]
		public int ProceduralSeed;

		[ProtoMember(81)]
		[Display(Name = "Enable Destructible Blocks", Description = "Enables destruction feature for the blocks.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool DestructibleBlocks = true;

		[ProtoMember(83)]
		[Display(Name = "Enable Ingame Scripts", Description = "Enables in game scripts.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool EnableIngameScripts = true;

		[ProtoMember(85)]
		[Display(Name = "View Distance")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(5, 50000)]
		[Browsable(false)]
		public int ViewDistance = 15000;

		[ProtoMember(87)]
		[DefaultValue(false)]
		[Display(Name = "Enable Tool Shake", Description = "Enables tool shake feature.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool EnableToolShake;

		[ProtoMember(89)]
		[Display(Name = "Voxel Generator Version", Description = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(0, 100)]
		public int VoxelGeneratorVersion = 4;

		[ProtoMember(91)]
		[Display(Name = "Enable Oxygen", Description = "Enables oxygen in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableOxygen;

		[ProtoMember(93)]
		[Display(Name = "Enable Airtightness", Description = "Enables airtightness in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableOxygenPressurization;

		[ProtoMember(95)]
		[Display(Name = "Enable 3rd Person Camera", Description = "Enables 3rd person camera.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool Enable3rdPersonView = true;

		[ProtoMember(97)]
		[Display(Name = "Enable Encounters", Description = "Enables random encounters in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool EnableEncounters = true;

		[ProtoMember(99)]
		[Display(Name = "Enable Convert to Station", Description = "Enables possibility of converting grid to station.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool EnableConvertToStation = true;

		[ProtoMember(101)]
		[Display(Name = "Unsupported stations", Description = "By enabling this option grids will no longer turn dynamic when disconnected from static grids.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool StationVoxelSupport;

		[ProtoMember(103)]
		[Display(Name = "Enable Sun Rotation", Description = "Enables sun rotation.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableSunRotation = true;

		[ProtoMember(105)]
		[Display(Name = "Enable Respawn Ships", Description = "Enables respawn ships.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		public bool EnableRespawnShips = true;

		[ProtoMember(107)]
		[Display(Name = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Browsable(false)]
		public bool ScenarioEditMode;

		[ProtoMember(109)]
		[Display(Name = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Browsable(false)]
		public bool Scenario;

		[ProtoMember(111)]
		[Display(Name = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Browsable(false)]
		public bool CanJoinRunning;

		[ProtoMember(113)]
		[Display(Name = "Physics Iterations", Description = "")]
		[Category("Environment")]
		[Range(2, 32)]
		public int PhysicsIterations = 8;

		[ProtoMember(115)]
		[Display(Name = "Sun Rotation Interval", Description = "Defines interval of one rotation of the sun.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(0, 1440)]
		public float SunRotationIntervalMinutes = 120f;

		[ProtoMember(117)]
		[Display(Name = "Enable Jetpack", Description = "Enables jetpack.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool EnableJetpack = true;

		[ProtoMember(119)]
		[Display(Name = "Spawn with Tools", Description = "Enables spawning with tools in the inventory.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool SpawnWithTools = true;

		[ProtoMember(121)]
		[Display(Name = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Browsable(false)]
		public bool StartInRespawnScreen;

		[ProtoMember(123)]
		[Display(Name = "Enable Voxel Destruction", Description = "Enables voxel destructions.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableVoxelDestruction = true;

		[ProtoMember(125)]
		[Display(Name = "")]
		[GameRelation(Game.SpaceEngineers)]
		[Browsable(false)]
		[Range(0, int.MaxValue)]
		public int MaxDrones = 5;

		[ProtoMember(127)]
		[Display(Name = "Enable Drones", Description = "Enables spawning of drones in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool EnableDrones = true;

		[ProtoMember(129)]
		[Display(Name = "Enable Wolves", Description = "Enables spawning of wolves in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool EnableWolfs = true;

		[ProtoMember(131)]
		[Display(Name = "Enable Spiders", Description = "Enables spawning of spiders in the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool EnableSpiders;

		[ProtoMember(133)]
		[Display(Name = "Flora Density Multiplier", Description = "")]
		[GameRelation(Game.Shared)]
		[Category("Environment")]
		[Range(0, 100)]
		[Browsable(false)]
		public float FloraDensityMultiplier = 1f;

		[ProtoMember(135)]
		[Display(Name = "Enable Structural Simulation")]
		[GameRelation(Game.MedievalEngineers)]
		public bool EnableStructuralSimulation;

		[ProtoMember(137)]
		[Display(Name = "Max Active Fracture Pieces")]
		[GameRelation(Game.MedievalEngineers)]
		[Range(0, int.MaxValue)]
		public int MaxActiveFracturePieces = 50;

		[ProtoMember(139)]
		[Display(Name = "Block Type World Limits")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Block Limits")]
		public SerializableDictionary<string, short> BlockTypeLimits = new SerializableDictionary<string, short>(new Dictionary<string, short>
		{
			{
				"Assembler",
				24
			},
			{
				"Refinery",
				24
			},
			{
				"Blast Furnace",
				24
			},
			{
				"Antenna",
				30
			},
			{
				"Drill",
				30
			},
			{
				"InteriorTurret",
				50
			},
			{
				"GatlingTurret",
				50
			},
			{
				"MissileTurret",
				50
			},
			{
				"ExtendedPistonBase",
				50
			},
			{
				"MotorStator",
				50
			},
			{
				"MotorAdvancedStator",
				50
			},
			{
				"ShipWelder",
				100
			},
			{
				"ShipGrinder",
				150
			}
		});

		[ProtoMember(141)]
		[Display(Name = "Enable Scripter Role", Description = "Enables scripter role for administration.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool EnableScripterRole;

		[ProtoMember(143, IsRequired = false)]
		[Display(Name = "Min Drop Container Respawn Time", Description = "Defines minimum respawn time for drop containers.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		[Range(0, 100)]
		public int MinDropContainerRespawnTime = 5;

		[ProtoMember(145, IsRequired = false)]
		[Display(Name = "Max Drop Container Respawn Time", Description = "Defines maximum respawn time for drop containers.")]
		[GameRelation(Game.Shared)]
		[Category("Others")]
		[Range(0, 100)]
		public int MaxDropContainerRespawnTime = 20;

		[ProtoMember(147, IsRequired = false)]
		[Display(Name = "Enable Turrets Friendly Fire", Description = "Enables friendly fire for turrets.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableTurretsFriendlyFire;

		[ProtoMember(149, IsRequired = false)]
		[Display(Name = "Enable Sub-Grid Damage", Description = "Enables sub-grid damage.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		public bool EnableSubgridDamage;

		[ProtoMember(151, IsRequired = false)]
		[Display(Name = "Sync Distance", Description = "Defines synchronization distance in multiplayer. High distance can slow down server drastically. Use with caution.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(1000, 20000)]
		public int SyncDistance = 3000;

		[ProtoMember(153, IsRequired = false)]
		[Display(Name = "Experimental Mode", Description = "Enables experimental mode.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool ExperimentalMode;

		[ProtoMember(155, IsRequired = false)]
		[Display(Name = "Adaptive Simulation Quality", Description = "Enables adaptive simulation quality system. This system is useful if you have a lot of voxel deformations in the world and low simulation speed.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool AdaptiveSimulationQuality = true;

		[ProtoMember(157, IsRequired = false)]
		[Display(Name = "Enable voxel hand", Description = "Enables voxel hand.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool EnableVoxelHand;

		[ProtoMember(158, IsRequired = false)]
		[Display(Name = "Remove Old Identities (h)", Description = "Defines time in hours after which inactive identities that do not own any grids will be removed. Set 0 to disable.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int RemoveOldIdentitiesH;

		[ProtoMember(159, IsRequired = false)]
		[Display(Name = "Trash Removal Enabled", Description = "Enables trash removal system.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public bool TrashRemovalEnabled = true;

		[ProtoMember(160, IsRequired = false)]
		[Display(Name = "Stop Grids Period (m)", Description = "Defines time in minutes after which grids will be stopped if far from player. Set 0 to disable.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int StopGridsPeriodMin = 15;

		[ProtoMember(161, IsRequired = false)]
		[Display(Name = "Trash Removal Flags", Description = "Defines flags for trash removal system.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[MyFlagEnum(typeof(MyTrashRemovalFlags))]
		public int TrashFlagsValue = 7706;

		[ProtoMember(162, IsRequired = false)]
		[Display(Name = "AFK Timeout", Description = "Defines time in minutes after which inactive players will be kicked. 0 is off.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int AFKTimeountMin;

		[ProtoMember(163, IsRequired = false)]
		[Display(Name = "Block Count Threshold", Description = "Defines block count threshold for trash removal system.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int BlockCountThreshold = 20;

		[ProtoMember(165, IsRequired = false)]
		[Display(Name = "Player Distance Threshold [m]", Description = "Defines player distance threshold for trash removal system.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public float PlayerDistanceThreshold = 500f;

		[ProtoMember(167, IsRequired = false)]
		[Display(Name = "Optimal Grid Count", Description = "By setting this, server will keep number of grids around this value. \n !WARNING! It ignores Powered and Fixed flags, Block Count and lowers Distance from player.\n Set to 0 to disable.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int OptimalGridCount;

		[ProtoMember(169, IsRequired = false)]
		[Display(Name = "Player Inactivity Threshold [hours]", Description = "Defines player inactivity (time from logout) threshold for trash removal system. \n !WARNING! This will remove all grids of the player.\n Set to 0 to disable.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public float PlayerInactivityThreshold;

		[ProtoMember(171, IsRequired = false)]
		[Display(Name = "Character Removal Threshold [mins]", Description = "Defines character removal threshold for trash removal system. If player disconnects it will remove his character after this time.\n Set to 0 to disable.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int PlayerCharacterRemovalThreshold = 15;

		[ProtoMember(173, IsRequired = false)]
		[Display(Name = "Voxel Reverting Enabled", Description = "Enables system for voxel reverting.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public bool VoxelTrashRemovalEnabled;

		[ProtoMember(175, IsRequired = false)]
		[Display(Name = "Distance voxel from player (m)", Description = "Only voxel chunks that are further from player will be reverted.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public float VoxelPlayerDistanceThreshold = 5000f;

		[ProtoMember(177, IsRequired = false)]
		[Display(Name = "Distance voxel from grid (m)", Description = "Only voxel chunks that are further from any grid will be reverted.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		public float VoxelGridDistanceThreshold = 5000f;

		[ProtoMember(179, IsRequired = false)]
		[Display(Name = "Voxel age (min)", Description = "Only voxel chunks that have been modified longer time age may be reverted.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Trash Removal")]
		[Range(0, int.MaxValue)]
		public int VoxelAgeThreshold = 24;

		[ProtoMember(181, IsRequired = false)]
		[Display(Name = "Enable Progression", Description = "Enables research progression.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Others")]
		public bool EnableResearch;

		[ProtoMember(183, IsRequired = false)]
		[GameRelation(Game.SpaceEngineers)]
		[Display(Name = "Enable Good.bot Hints", Description = "Enables Good.bot hints in the world. If user has disabled hints, this will not override that.")]
		[Category("Others")]
		public bool EnableGoodBotHints = true;

		[ProtoMember(185, IsRequired = false)]
		[GameRelation(Game.SpaceEngineers)]
		[Display(Name = "Optimal respawn distance", Description = "Sets optimal distance in meters the game should take into consideration when spawning new player near others.")]
		[Category("Players")]
		public float OptimalSpawnDistance = 16000f;

		[ProtoMember(187, IsRequired = false)]
		[GameRelation(Game.SpaceEngineers)]
		[Display(Name = "Enable Autorespawn", Description = "Enables automatic respawn at nearest available respawn point")]
		[Category("Players")]
		public bool EnableAutorespawn = true;

		[ProtoMember(188)]
		[Display(Name = "Enable Bounty Contracts", Description = "If enabled bounty contracts will be available on stations.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Players")]
		public bool EnableBountyContracts = true;

		[ProtoMember(189, IsRequired = false)]
		[GameRelation(Game.SpaceEngineers)]
		[Display(Name = "Enable Supergridding", Description = "Allows super gridding exploit to be used")]
		[Category("Others")]
		public bool EnableSupergridding;

		[ProtoMember(191)]
		[Display(Name = "Enable Economy", Description = "Enables economy features.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		public bool EnableEconomy;

		[ProtoMember(194)]
		[Display(Name = "Deposits Count Coefficient", Description = "Resource deposits count coefficient for generated world content (voxel generator version > 2).")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(0, 10)]
		public float DepositsCountCoefficient = 2f;

		[ProtoMember(197)]
		[Display(Name = "Deposit Size Denominator", Description = "Resource deposit size denominator for generated world content (voxel generator version > 2).")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Environment")]
		[Range(1.0, 100.0)]
		public float DepositSizeDenominator = 30f;

		[ProtoMember(200)]
		[Display(Name = "Harvest Ratio Multiplier", Description = "Harvest ratio multiplier for drills.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("Multipliers")]
		[Range(0.0, 100.0)]
		public float HarvestRatioMultiplier = 1f;

		[ProtoMember(203)]
		[Display(Name = "NPC Factions Count", Description = "The number of NPC factions generated on the start of the world.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		[Range(10.0, 100.0)]
		public int TradeFactionsCount = 15;

		[ProtoMember(206)]
		[Display(Name = "Stations Inner Radius", Description = "The inner radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		[Range(200000.0, 9.2233720368547758E+18)]
		public double StationsDistanceInnerRadius = 10000000.0;

		[ProtoMember(209)]
		[Display(Name = "Stations Outer Radius Start", Description = "The outer radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		[Range(500000.0, 9.2233720368547758E+18)]
		public double StationsDistanceOuterRadiusStart = 10000000.0;

		[ProtoMember(212)]
		[Display(Name = "Stations Outer Radius End", Description = "The outer radius [m] (center is in 0,0,0), where stations can spawn. Does not affect planet-bound stations (surface Outposts and Orbital stations).")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		[Range(1000000.0, 9.2233720368547758E+18)]
		public double StationsDistanceOuterRadiusEnd = 30000000.0;

		[ProtoMember(215)]
		[Display(Name = "Economy tick time", Description = "Time period between two economy updates.")]
		[GameRelation(Game.SpaceEngineers)]
		[Category("NPCs")]
		[Range(300, 3600)]
		public int EconomyTickInSeconds = 1200;

		private bool m_experimentalReasonInited;

		private ExperimentalReason m_experimentalReason;

		public bool AutoSave
		{
			get
			{
				return AutoSaveInMinutes != 0;
			}
			set
			{
				AutoSaveInMinutes = (value ? 5u : 0u);
			}
		}

		[Display(Name = "Client can save")]
		[GameRelation(Game.Shared)]
		[XmlIgnore]
		[NoSerialize]
		[Browsable(false)]
		public bool ClientCanSave
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[XmlIgnore]
		[ProtoIgnore]
		[Browsable(false)]
		public MyTrashRemovalFlags TrashFlags
		{
			get
			{
				return (MyTrashRemovalFlags)TrashFlagsValue;
			}
			set
			{
				TrashFlagsValue = (int)value;
			}
		}

		public bool ShouldSerializeAutoSave()
		{
			return false;
		}

		public bool ShouldSerializeProceduralDensity()
		{
			return ProceduralDensity > 0f;
		}

		public bool ShouldSerializeProceduralSeed()
		{
			return ProceduralDensity > 0f;
		}

		public bool ShouldSerializeTrashFlags()
		{
			return false;
		}

		public ExperimentalReason GetExperimentalReason(bool update = true)
		{
			if (update || !m_experimentalReasonInited)
			{
				UpdateExperimentalReason();
			}
			return m_experimentalReason;
		}

		public void UpdateExperimentalReason()
		{
			m_experimentalReasonInited = true;
			m_experimentalReason = (ExperimentalReason)0L;
			if (ExperimentalMode)
			{
				m_experimentalReason |= ExperimentalReason.ExperimentalMode;
			}
			if (MaxPlayers > 16 || MaxPlayers == 0)
			{
				m_experimentalReason |= ExperimentalReason.MaxPlayers;
			}
			if (EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM || EnvironmentHostility == MyEnvironmentHostilityEnum.CATACLYSM_UNREAL)
			{
				m_experimentalReason |= ExperimentalReason.EnvironmentHostility;
			}
			if (ProceduralDensity > 0.35f)
			{
				m_experimentalReason |= ExperimentalReason.ProceduralDensity;
			}
			if (SunRotationIntervalMinutes <= 29f)
			{
				m_experimentalReason |= ExperimentalReason.SunRotationIntervalMinutes;
			}
			if (MaxFloatingObjects > 100)
			{
				m_experimentalReason |= ExperimentalReason.MaxFloatingObjects;
			}
			if (PhysicsIterations != 8)
			{
				m_experimentalReason |= ExperimentalReason.PhysicsIterations;
			}
			if (SyncDistance != 3000)
			{
				m_experimentalReason |= ExperimentalReason.SyncDistance;
			}
			if (BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				m_experimentalReason |= ExperimentalReason.BlockLimitsEnabled;
			}
			if (TotalPCU > 600000)
			{
				m_experimentalReason |= ExperimentalReason.TotalPCU;
			}
			if (EnableSpectator)
			{
				m_experimentalReason |= ExperimentalReason.EnableSpectator;
			}
			if (ResetOwnership)
			{
				m_experimentalReason |= ExperimentalReason.ResetOwnership;
			}
			if (PermanentDeath == true)
			{
				m_experimentalReason |= ExperimentalReason.PermanentDeath;
			}
			if (EnableIngameScripts)
			{
				m_experimentalReason |= ExperimentalReason.EnableIngameScripts;
			}
			if (StationVoxelSupport)
			{
				m_experimentalReason |= ExperimentalReason.StationVoxelSupport;
			}
			if (EnableWolfs)
			{
				m_experimentalReason |= ExperimentalReason.EnableWolfs;
			}
			if (EnableSpiders)
			{
				m_experimentalReason |= ExperimentalReason.EnableSpiders;
			}
			if (EnableSubgridDamage)
			{
				m_experimentalReason |= ExperimentalReason.EnableSubgridDamage;
			}
			if (!AdaptiveSimulationQuality)
			{
				m_experimentalReason |= ExperimentalReason.AdaptiveSimulationQuality;
			}
			if (EnableSupergridding)
			{
				m_experimentalReason |= ExperimentalReason.SupergriddingEnabled;
			}
		}

		public bool IsSettingsExperimental()
		{
			if (!m_experimentalReasonInited)
			{
				UpdateExperimentalReason();
			}
			return m_experimentalReason != (ExperimentalReason)0L;
		}

		public void LogMembers(MyLog log, LoggingOptions options)
		{
			log.WriteLine("Settings:");
			using (log.IndentUsing(options))
			{
				log.WriteLine("GameMode = " + GameMode);
				log.WriteLine("MaxPlayers = " + MaxPlayers);
				log.WriteLine("OnlineMode = " + OnlineMode);
				log.WriteLine("AutoHealing = " + AutoHealing);
				log.WriteLine("WeaponsEnabled = " + WeaponsEnabled);
				log.WriteLine("ThrusterDamage = " + ThrusterDamage);
				log.WriteLine("EnableSpectator = " + EnableSpectator);
				log.WriteLine("EnableCopyPaste = " + EnableCopyPaste);
				log.WriteLine("MaxFloatingObjects = " + MaxFloatingObjects);
				log.WriteLine("MaxGridSize = " + (int)MaxGridSize);
				log.WriteLine("MaxBlocksPerPlayer = " + (int)MaxBlocksPerPlayer);
				log.WriteLine("CargoShipsEnabled = " + CargoShipsEnabled);
				log.WriteLine("EnvironmentHostility = " + EnvironmentHostility);
				log.WriteLine("ShowPlayerNamesOnHud = " + ShowPlayerNamesOnHud);
				log.WriteLine("InventorySizeMultiplier = " + InventorySizeMultiplier);
				log.WriteLine("RefinerySpeedMultiplier = " + RefinerySpeedMultiplier);
				log.WriteLine("AssemblerSpeedMultiplier = " + AssemblerSpeedMultiplier);
				log.WriteLine("AssemblerEfficiencyMultiplier = " + AssemblerEfficiencyMultiplier);
				log.WriteLine("WelderSpeedMultiplier = " + WelderSpeedMultiplier);
				log.WriteLine("GrinderSpeedMultiplier = " + GrinderSpeedMultiplier);
				log.WriteLine("ClientCanSave = " + ClientCanSave);
				log.WriteLine("HackSpeedMultiplier = " + HackSpeedMultiplier);
				log.WriteLine("PermanentDeath = " + PermanentDeath);
				log.WriteLine("DestructibleBlocks =  " + DestructibleBlocks);
				log.WriteLine("EnableScripts =  " + EnableIngameScripts);
				log.WriteLine("AutoSaveInMinutes = " + AutoSaveInMinutes);
				log.WriteLine("SpawnShipTimeMultiplier = " + SpawnShipTimeMultiplier);
				log.WriteLine("ProceduralDensity = " + ProceduralDensity);
				log.WriteLine("ProceduralSeed = " + (int)ProceduralSeed);
				log.WriteLine("DestructibleBlocks = " + DestructibleBlocks);
				log.WriteLine("EnableIngameScripts = " + EnableIngameScripts);
				log.WriteLine("ViewDistance = " + (int)ViewDistance);
				log.WriteLine("Voxel destruction = " + EnableVoxelDestruction);
				log.WriteLine("EnableStructuralSimulation = " + EnableStructuralSimulation);
				log.WriteLine("MaxActiveFracturePieces = " + (int)MaxActiveFracturePieces);
				log.WriteLine("EnableContainerDrops = " + EnableContainerDrops);
				log.WriteLine("MinDropContainerRespawnTime = " + (int)MinDropContainerRespawnTime);
				log.WriteLine("MaxDropContainerRespawnTime = " + (int)MaxDropContainerRespawnTime);
				log.WriteLine("EnableTurretsFriendlyFire = " + EnableTurretsFriendlyFire);
				log.WriteLine("EnableSubgridDamage = " + EnableSubgridDamage);
				log.WriteLine("SyncDistance = " + (int)SyncDistance);
				log.WriteLine("BlockLimitsEnabled = " + BlockLimitsEnabled);
				log.WriteLine("AFKTimeoutMin = " + (int)AFKTimeountMin);
				log.WriteLine("StopGridsPeriodMin = " + (int)StopGridsPeriodMin);
				log.WriteLine("ExperimentalMode = " + ExperimentalMode);
				log.WriteLine("ExperimentalModeReason = " + GetExperimentalReason());
			}
		}

		/// <summary>
		/// If you are modifying this function, also modify MyBlockLimits.GetInitialPCU 
		/// (This function cannot be moved into MyBlockLimits as using MyBlockLimits while MySession.Static == null will result in crash during intialization of statics of MyBlockLimits)
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		public static int GetInitialPCU(MyObjectBuilder_SessionSettings settings)
		{
			switch (settings.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
				return int.MaxValue;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				return settings.TotalPCU / settings.MaxPlayers;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				if (settings.MaxFactionsCount == 0)
				{
					return settings.TotalPCU;
				}
				return settings.TotalPCU / settings.MaxFactionsCount;
			default:
				return settings.TotalPCU;
			}
		}
	}
}
