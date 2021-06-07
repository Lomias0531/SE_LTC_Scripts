using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("ScenarioDefinition")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ScenarioDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public struct AsteroidClustersSettings
		{
			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAsteroidClustersSettings_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<AsteroidClustersSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AsteroidClustersSettings owner, in bool value)
				{
					owner.Enabled = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AsteroidClustersSettings owner, out bool value)
				{
					value = owner.Enabled;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAsteroidClustersSettings_003C_003EOffset_003C_003EAccessor : IMemberAccessor<AsteroidClustersSettings, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AsteroidClustersSettings owner, in float value)
				{
					owner.Offset = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AsteroidClustersSettings owner, out float value)
				{
					value = owner.Offset;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAsteroidClustersSettings_003C_003ECentralCluster_003C_003EAccessor : IMemberAccessor<AsteroidClustersSettings, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AsteroidClustersSettings owner, in bool value)
				{
					owner.CentralCluster = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AsteroidClustersSettings owner, out bool value)
				{
					value = owner.CentralCluster;
				}
			}

			private class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAsteroidClustersSettings_003C_003EActor : IActivator, IActivator<AsteroidClustersSettings>
			{
				private sealed override object CreateInstance()
				{
					return default(AsteroidClustersSettings);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AsteroidClustersSettings CreateInstance()
				{
					return (AsteroidClustersSettings)(object)default(AsteroidClustersSettings);
				}

				AsteroidClustersSettings IActivator<AsteroidClustersSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(73)]
			[XmlAttribute]
			public bool Enabled;

			[ProtoMember(76)]
			[XmlAttribute]
			public float Offset;

			[ProtoMember(79)]
			[XmlAttribute]
			public bool CentralCluster;

			public bool ShouldSerializeOffset()
			{
				return Enabled;
			}

			public bool ShouldSerializeCentralCluster()
			{
				return Enabled;
			}
		}

		[ProtoContract]
		public struct StartingItem
		{
			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingItem_003C_003Eamount_003C_003EAccessor : IMemberAccessor<StartingItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StartingItem owner, in float value)
				{
					owner.amount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StartingItem owner, out float value)
				{
					value = owner.amount;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingItem_003C_003EitemName_003C_003EAccessor : IMemberAccessor<StartingItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StartingItem owner, in string value)
				{
					owner.itemName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StartingItem owner, out string value)
				{
					value = owner.itemName;
				}
			}

			private class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingItem_003C_003EActor : IActivator, IActivator<StartingItem>
			{
				private sealed override object CreateInstance()
				{
					return default(StartingItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StartingItem CreateInstance()
				{
					return (StartingItem)(object)default(StartingItem);
				}

				StartingItem IActivator<StartingItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(82)]
			[XmlAttribute]
			public float amount;

			[ProtoMember(85)]
			[XmlText]
			public string itemName;
		}

		[ProtoContract]
		public struct StartingPhysicalItem
		{
			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingPhysicalItem_003C_003Eamount_003C_003EAccessor : IMemberAccessor<StartingPhysicalItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StartingPhysicalItem owner, in float value)
				{
					owner.amount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StartingPhysicalItem owner, out float value)
				{
					value = owner.amount;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingPhysicalItem_003C_003EitemName_003C_003EAccessor : IMemberAccessor<StartingPhysicalItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StartingPhysicalItem owner, in string value)
				{
					owner.itemName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StartingPhysicalItem owner, out string value)
				{
					value = owner.itemName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingPhysicalItem_003C_003EitemType_003C_003EAccessor : IMemberAccessor<StartingPhysicalItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StartingPhysicalItem owner, in string value)
				{
					owner.itemType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StartingPhysicalItem owner, out string value)
				{
					value = owner.itemType;
				}
			}

			private class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EStartingPhysicalItem_003C_003EActor : IActivator, IActivator<StartingPhysicalItem>
			{
				private sealed override object CreateInstance()
				{
					return default(StartingPhysicalItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StartingPhysicalItem CreateInstance()
				{
					return (StartingPhysicalItem)(object)default(StartingPhysicalItem);
				}

				StartingPhysicalItem IActivator<StartingPhysicalItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(88)]
			[XmlAttribute]
			public float amount;

			[ProtoMember(91)]
			[XmlText]
			public string itemName;

			[ProtoMember(94)]
			[XmlAttribute]
			public string itemType;
		}

		[ProtoContract]
		public class MyOBBattleSettings
		{
			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EMyOBBattleSettings_003C_003EAttackerSlots_003C_003EAccessor : IMemberAccessor<MyOBBattleSettings, SerializableBoundingBoxD[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyOBBattleSettings owner, in SerializableBoundingBoxD[] value)
				{
					owner.AttackerSlots = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyOBBattleSettings owner, out SerializableBoundingBoxD[] value)
				{
					value = owner.AttackerSlots;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EMyOBBattleSettings_003C_003EDefenderSlot_003C_003EAccessor : IMemberAccessor<MyOBBattleSettings, SerializableBoundingBoxD>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyOBBattleSettings owner, in SerializableBoundingBoxD value)
				{
					owner.DefenderSlot = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyOBBattleSettings owner, out SerializableBoundingBoxD value)
				{
					value = owner.DefenderSlot;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EMyOBBattleSettings_003C_003EDefenderEntityId_003C_003EAccessor : IMemberAccessor<MyOBBattleSettings, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyOBBattleSettings owner, in long value)
				{
					owner.DefenderEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyOBBattleSettings owner, out long value)
				{
					value = owner.DefenderEntityId;
				}
			}

			private class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EMyOBBattleSettings_003C_003EActor : IActivator, IActivator<MyOBBattleSettings>
			{
				private sealed override object CreateInstance()
				{
					return new MyOBBattleSettings();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyOBBattleSettings CreateInstance()
				{
					return new MyOBBattleSettings();
				}

				MyOBBattleSettings IActivator<MyOBBattleSettings>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(97)]
			[XmlArrayItem("Slot")]
			public SerializableBoundingBoxD[] AttackerSlots;

			[ProtoMember(100)]
			public SerializableBoundingBoxD DefenderSlot;

			[ProtoMember(103)]
			public long DefenderEntityId;
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EGameDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in SerializableDefinitionId value)
			{
				owner.GameDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.GameDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EEnvironmentDefinition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in SerializableDefinitionId value)
			{
				owner.EnvironmentDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out SerializableDefinitionId value)
			{
				value = owner.EnvironmentDefinition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAsteroidClusters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, AsteroidClustersSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in AsteroidClustersSettings value)
			{
				owner.AsteroidClusters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out AsteroidClustersSettings value)
			{
				value = owner.AsteroidClusters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDefaultEnvironment_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyEnvironmentHostilityEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyEnvironmentHostilityEnum value)
			{
				owner.DefaultEnvironment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyEnvironmentHostilityEnum value)
			{
				value = owner.DefaultEnvironment;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EPossibleStartingStates_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_WorldGeneratorPlayerStartingState[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_WorldGeneratorPlayerStartingState[] value)
			{
				owner.PossibleStartingStates = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_WorldGeneratorPlayerStartingState[] value)
			{
				value = owner.PossibleStartingStates;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EWorldGeneratorOperations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_WorldGeneratorOperation[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_WorldGeneratorOperation[] value)
			{
				owner.WorldGeneratorOperations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_WorldGeneratorOperation[] value)
			{
				value = owner.WorldGeneratorOperations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeModeWeapons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value)
			{
				owner.CreativeModeWeapons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value)
			{
				value = owner.CreativeModeWeapons;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeModeComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingItem[] value)
			{
				owner.CreativeModeComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingItem[] value)
			{
				value = owner.CreativeModeComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeModePhysicalItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingPhysicalItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingPhysicalItem[] value)
			{
				owner.CreativeModePhysicalItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingPhysicalItem[] value)
			{
				value = owner.CreativeModePhysicalItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeModeAmmoItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingItem[] value)
			{
				owner.CreativeModeAmmoItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingItem[] value)
			{
				value = owner.CreativeModeAmmoItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalModeWeapons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value)
			{
				owner.SurvivalModeWeapons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value)
			{
				value = owner.SurvivalModeWeapons;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalModeComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingItem[] value)
			{
				owner.SurvivalModeComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingItem[] value)
			{
				value = owner.SurvivalModeComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalModePhysicalItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingPhysicalItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingPhysicalItem[] value)
			{
				owner.SurvivalModePhysicalItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingPhysicalItem[] value)
			{
				value = owner.SurvivalModePhysicalItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalModeAmmoItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, StartingItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in StartingItem[] value)
			{
				owner.SurvivalModeAmmoItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out StartingItem[] value)
			{
				value = owner.SurvivalModeAmmoItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeInventoryItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_InventoryItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_InventoryItem[] value)
			{
				owner.CreativeInventoryItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_InventoryItem[] value)
			{
				value = owner.CreativeInventoryItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalInventoryItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_InventoryItem[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_InventoryItem[] value)
			{
				owner.SurvivalInventoryItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_InventoryItem[] value)
			{
				value = owner.SurvivalInventoryItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EWorldBoundaries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableBoundingBoxD?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in SerializableBoundingBoxD? value)
			{
				owner.WorldBoundaries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out SerializableBoundingBoxD? value)
			{
				value = owner.WorldBoundaries;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003Em_creativeDefaultToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_Toolbar value)
			{
				owner.m_creativeDefaultToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.m_creativeDefaultToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESurvivalDefaultToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_Toolbar value)
			{
				owner.SurvivalDefaultToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.SurvivalDefaultToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EBattle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyOBBattleSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyOBBattleSettings value)
			{
				owner.Battle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyOBBattleSettings value)
			{
				value = owner.Battle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EMainCharacterModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				owner.MainCharacterModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				value = owner.MainCharacterModel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EGameDate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in long value)
			{
				owner.GameDate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out long value)
			{
				value = owner.GameDate;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESunDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in SerializableVector3 value)
			{
				owner.SunDirection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out SerializableVector3 value)
			{
				value = owner.SunDirection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EDefaultToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_Toolbar value)
			{
				owner.DefaultToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.DefaultToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ECreativeDefaultToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyObjectBuilder_Toolbar value)
			{
				owner.CreativeDefaultToolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.CreativeDefaultToolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ScenarioDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScenarioDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScenarioDefinition CreateInstance()
			{
				return new MyObjectBuilder_ScenarioDefinition();
			}

			MyObjectBuilder_ScenarioDefinition IActivator<MyObjectBuilder_ScenarioDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId GameDefinition = MyGameDefinition.Default;

		[ProtoMember(4)]
		public SerializableDefinitionId EnvironmentDefinition = new SerializableDefinitionId(typeof(MyObjectBuilder_EnvironmentDefinition), "Default");

		[ProtoMember(7)]
		public AsteroidClustersSettings AsteroidClusters;

		[ProtoMember(10)]
		public MyEnvironmentHostilityEnum DefaultEnvironment = MyEnvironmentHostilityEnum.NORMAL;

		[ProtoMember(13)]
		[XmlArrayItem("StartingState", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_WorldGeneratorPlayerStartingState>))]
		public MyObjectBuilder_WorldGeneratorPlayerStartingState[] PossibleStartingStates;

		[ProtoMember(16)]
		[XmlArrayItem("Operation", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_WorldGeneratorOperation>))]
		public MyObjectBuilder_WorldGeneratorOperation[] WorldGeneratorOperations;

		[ProtoMember(19)]
		[XmlArrayItem("Weapon")]
		public string[] CreativeModeWeapons;

		[ProtoMember(22)]
		[XmlArrayItem("Component")]
		public StartingItem[] CreativeModeComponents;

		[ProtoMember(25)]
		[XmlArrayItem("PhysicalItem")]
		public StartingPhysicalItem[] CreativeModePhysicalItems;

		[ProtoMember(28)]
		[XmlArrayItem("AmmoItem")]
		public StartingItem[] CreativeModeAmmoItems;

		[ProtoMember(31)]
		[XmlArrayItem("Weapon")]
		public string[] SurvivalModeWeapons;

		[ProtoMember(34)]
		[XmlArrayItem("Component")]
		public StartingItem[] SurvivalModeComponents;

		[ProtoMember(37)]
		[XmlArrayItem("PhysicalItem")]
		public StartingPhysicalItem[] SurvivalModePhysicalItems;

		[ProtoMember(40)]
		[XmlArrayItem("AmmoItem")]
		public StartingItem[] SurvivalModeAmmoItems;

		[ProtoMember(43)]
		public MyObjectBuilder_InventoryItem[] CreativeInventoryItems;

		[ProtoMember(46)]
		public MyObjectBuilder_InventoryItem[] SurvivalInventoryItems;

		[ProtoMember(49)]
		public SerializableBoundingBoxD? WorldBoundaries;

		private MyObjectBuilder_Toolbar m_creativeDefaultToolbar;

		[ProtoMember(58)]
		public MyObjectBuilder_Toolbar SurvivalDefaultToolbar;

		[ProtoMember(61)]
		public MyOBBattleSettings Battle;

		[ProtoMember(64)]
		public string MainCharacterModel;

		[ProtoMember(67)]
		public long GameDate = 656385372000000000L;

		[ProtoMember(70)]
		public SerializableVector3 SunDirection = Vector3.Invalid;

		[ProtoMember(52)]
		public MyObjectBuilder_Toolbar DefaultToolbar
		{
			get
			{
				return null;
			}
			set
			{
				CreativeDefaultToolbar = (SurvivalDefaultToolbar = value);
			}
		}

		[ProtoMember(55)]
		public MyObjectBuilder_Toolbar CreativeDefaultToolbar
		{
			get
			{
				return m_creativeDefaultToolbar;
			}
			set
			{
				m_creativeDefaultToolbar = value;
			}
		}

		public bool ShouldSerializeDefaultToolbar()
		{
			return false;
		}
	}
}
