using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Character : MyObjectBuilder_EntityBase
	{
		[ProtoContract]
		public struct StoredGas
		{
			protected class VRage_Game_MyObjectBuilder_Character_003C_003EStoredGas_003C_003EId_003C_003EAccessor : IMemberAccessor<StoredGas, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StoredGas owner, in SerializableDefinitionId value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StoredGas owner, out SerializableDefinitionId value)
				{
					value = owner.Id;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Character_003C_003EStoredGas_003C_003EFillLevel_003C_003EAccessor : IMemberAccessor<StoredGas, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StoredGas owner, in float value)
				{
					owner.FillLevel = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StoredGas owner, out float value)
				{
					value = owner.FillLevel;
				}
			}

			private class VRage_Game_MyObjectBuilder_Character_003C_003EStoredGas_003C_003EActor : IActivator, IActivator<StoredGas>
			{
				private sealed override object CreateInstance()
				{
					return default(StoredGas);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StoredGas CreateInstance()
				{
					return (StoredGas)(object)default(StoredGas);
				}

				StoredGas IActivator<StoredGas>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public SerializableDefinitionId Id;

			[ProtoMember(4)]
			public float FillLevel;
		}

		[ProtoContract]
		public struct ComponentItem
		{
			protected class VRage_Game_MyObjectBuilder_Character_003C_003EComponentItem_003C_003EComponentId_003C_003EAccessor : IMemberAccessor<ComponentItem, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ComponentItem owner, in SerializableDefinitionId value)
				{
					owner.ComponentId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ComponentItem owner, out SerializableDefinitionId value)
				{
					value = owner.ComponentId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Character_003C_003EComponentItem_003C_003ECount_003C_003EAccessor : IMemberAccessor<ComponentItem, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ComponentItem owner, in int value)
				{
					owner.Count = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ComponentItem owner, out int value)
				{
					value = owner.Count;
				}
			}

			private class VRage_Game_MyObjectBuilder_Character_003C_003EComponentItem_003C_003EActor : IActivator, IActivator<ComponentItem>
			{
				private sealed override object CreateInstance()
				{
					return default(ComponentItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ComponentItem CreateInstance()
				{
					return (ComponentItem)(object)default(ComponentItem);
				}

				ComponentItem IActivator<ComponentItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(197)]
			public SerializableDefinitionId ComponentId;

			[ProtoMember(199)]
			public int Count;
		}

		[ProtoContract]
		public struct BuildPlanItem
		{
			protected class VRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanItem_003C_003EBlockId_003C_003EAccessor : IMemberAccessor<BuildPlanItem, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildPlanItem owner, in SerializableDefinitionId value)
				{
					owner.BlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildPlanItem owner, out SerializableDefinitionId value)
				{
					value = owner.BlockId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanItem_003C_003EIsInProgress_003C_003EAccessor : IMemberAccessor<BuildPlanItem, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildPlanItem owner, in bool value)
				{
					owner.IsInProgress = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildPlanItem owner, out bool value)
				{
					value = owner.IsInProgress;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanItem_003C_003EComponents_003C_003EAccessor : IMemberAccessor<BuildPlanItem, List<ComponentItem>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BuildPlanItem owner, in List<ComponentItem> value)
				{
					owner.Components = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BuildPlanItem owner, out List<ComponentItem> value)
				{
					value = owner.Components;
				}
			}

			private class VRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanItem_003C_003EActor : IActivator, IActivator<BuildPlanItem>
			{
				private sealed override object CreateInstance()
				{
					return default(BuildPlanItem);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BuildPlanItem CreateInstance()
				{
					return (BuildPlanItem)(object)default(BuildPlanItem);
				}

				BuildPlanItem IActivator<BuildPlanItem>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(185)]
			public SerializableDefinitionId BlockId;

			[ProtoMember(188)]
			public bool IsInProgress;

			[ProtoMember(190)]
			public List<ComponentItem> Components;
		}

		[ProtoContract]
		public struct LadderInfo
		{
			protected class VRage_Game_MyObjectBuilder_Character_003C_003ELadderInfo_003C_003EBaseMatrix_003C_003EAccessor : IMemberAccessor<LadderInfo, MyPositionAndOrientation>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LadderInfo owner, in MyPositionAndOrientation value)
				{
					owner.BaseMatrix = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LadderInfo owner, out MyPositionAndOrientation value)
				{
					value = owner.BaseMatrix;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Character_003C_003ELadderInfo_003C_003EIncrementToBase_003C_003EAccessor : IMemberAccessor<LadderInfo, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LadderInfo owner, in SerializableVector3 value)
				{
					owner.IncrementToBase = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LadderInfo owner, out SerializableVector3 value)
				{
					value = owner.IncrementToBase;
				}
			}

			private class VRage_Game_MyObjectBuilder_Character_003C_003ELadderInfo_003C_003EActor : IActivator, IActivator<LadderInfo>
			{
				private sealed override object CreateInstance()
				{
					return default(LadderInfo);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override LadderInfo CreateInstance()
				{
					return (LadderInfo)(object)default(LadderInfo);
				}

				LadderInfo IActivator<LadderInfo>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(200)]
			public MyPositionAndOrientation BaseMatrix;

			[ProtoMember(205)]
			public SerializableVector3 IncrementToBase;
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ECharacterModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in string value)
			{
				owner.CharacterModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out string value)
			{
				value = owner.CharacterModel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EInventory_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_Inventory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in MyObjectBuilder_Inventory value)
			{
				owner.Inventory = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out MyObjectBuilder_Inventory value)
			{
				value = owner.Inventory;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EHandWeapon_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_EntityBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in MyObjectBuilder_EntityBase value)
			{
				owner.HandWeapon = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out MyObjectBuilder_EntityBase value)
			{
				value = owner.HandWeapon;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EBattery_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_Battery>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in MyObjectBuilder_Battery value)
			{
				owner.Battery = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out MyObjectBuilder_Battery value)
			{
				value = owner.Battery;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ELightEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.LightEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.LightEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EDampenersEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.DampenersEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.DampenersEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ECharacterGeneralDamageModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float value)
			{
				owner.CharacterGeneralDamageModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float value)
			{
				value = owner.CharacterGeneralDamageModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EUsingLadder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in long? value)
			{
				owner.UsingLadder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out long? value)
			{
				value = owner.UsingLadder;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EHeadAngle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in SerializableVector2 value)
			{
				owner.HeadAngle = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out SerializableVector2 value)
			{
				value = owner.HeadAngle;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ELinearVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in SerializableVector3 value)
			{
				owner.LinearVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out SerializableVector3 value)
			{
				value = owner.LinearVelocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EAutoenableJetpackDelay_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float value)
			{
				owner.AutoenableJetpackDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float value)
			{
				value = owner.AutoenableJetpackDelay;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EJetpackEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.JetpackEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.JetpackEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EHealth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float? value)
			{
				owner.Health = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float? value)
			{
				value = owner.Health;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EAIMode_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.AIMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.AIMode;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in SerializableVector3 value)
			{
				owner.ColorMaskHSV = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out SerializableVector3 value)
			{
				value = owner.ColorMaskHSV;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ELootingCounter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float value)
			{
				owner.LootingCounter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float value)
			{
				value = owner.LootingCounter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EIsInFirstPersonView_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.IsInFirstPersonView = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.IsInFirstPersonView;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EEnableBroadcasting_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.EnableBroadcasting = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.EnableBroadcasting;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EOxygenLevel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float value)
			{
				owner.OxygenLevel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float value)
			{
				value = owner.OxygenLevel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EEnvironmentOxygenLevel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in float value)
			{
				owner.EnvironmentOxygenLevel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out float value)
			{
				value = owner.EnvironmentOxygenLevel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EStoredGases_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<StoredGas>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in List<StoredGas> value)
			{
				owner.StoredGases = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out List<StoredGas> value)
			{
				value = owner.StoredGases;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EMovementState_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyCharacterMovementEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in MyCharacterMovementEnum value)
			{
				owner.MovementState = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out MyCharacterMovementEnum value)
			{
				value = owner.MovementState;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EEnabledComponents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in List<string> value)
			{
				owner.EnabledComponents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out List<string> value)
			{
				value = owner.EnabledComponents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EPlayerSteamId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in ulong value)
			{
				owner.PlayerSteamId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out ulong value)
			{
				value = owner.PlayerSteamId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EPlayerSerialId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in int value)
			{
				owner.PlayerSerialId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out int value)
			{
				value = owner.PlayerSerialId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ENeedsOxygenFromSuit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.NeedsOxygenFromSuit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.NeedsOxygenFromSuit;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EOwningPlayerIdentityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in long? value)
			{
				owner.OwningPlayerIdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out long? value)
			{
				value = owner.OwningPlayerIdentityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EIsPersistenceCharacter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in bool value)
			{
				owner.IsPersistenceCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out bool value)
			{
				value = owner.IsPersistenceCharacter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ERelativeDampeningEntity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in long value)
			{
				owner.RelativeDampeningEntity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out long value)
			{
				value = owner.RelativeDampeningEntity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<BuildPlanItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in List<BuildPlanItem> value)
			{
				owner.BuildPlanner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out List<BuildPlanItem> value)
			{
				value = owner.BuildPlanner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EUsingLadderInfo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Character, LadderInfo?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Character owner, in LadderInfo? value)
			{
				owner.UsingLadderInfo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Character owner, out LadderInfo? value)
			{
				value = owner.UsingLadderInfo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Character_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Character owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Character owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Character_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Character>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Character();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Character CreateInstance()
			{
				return new MyObjectBuilder_Character();
			}

			MyObjectBuilder_Character IActivator<MyObjectBuilder_Character>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static Dictionary<string, SerializableVector3> CharacterModels = new Dictionary<string, SerializableVector3>
		{
			{
				"Soldier",
				new SerializableVector3(0f, 0f, 0.05f)
			},
			{
				"Astronaut",
				new SerializableVector3(0f, -1f, 0f)
			},
			{
				"Astronaut_Black",
				new SerializableVector3(0f, -0.96f, -0.5f)
			},
			{
				"Astronaut_Blue",
				new SerializableVector3(0.575f, 0.15f, 0.2f)
			},
			{
				"Astronaut_Green",
				new SerializableVector3(0.333f, -0.33f, -0.05f)
			},
			{
				"Astronaut_Red",
				new SerializableVector3(0f, 0f, 0.05f)
			},
			{
				"Astronaut_White",
				new SerializableVector3(0f, -0.8f, 0.6f)
			},
			{
				"Astronaut_Yellow",
				new SerializableVector3(0.122f, 0.05f, 0.46f)
			},
			{
				"Engineer_suit_no_helmet",
				new SerializableVector3(-100f, -100f, -100f)
			}
		};

		[ProtoMember(7)]
		public string CharacterModel;

		[ProtoMember(10)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_Inventory Inventory;

		[ProtoMember(13)]
		[XmlElement("HandWeapon", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
		[Nullable]
		[DynamicObjectBuilder(false)]
		public MyObjectBuilder_EntityBase HandWeapon;

		[ProtoMember(16)]
		public MyObjectBuilder_Battery Battery;

		[ProtoMember(19)]
		public bool LightEnabled;

		[ProtoMember(22)]
		[DefaultValue(true)]
		public bool DampenersEnabled = true;

		[ProtoMember(25)]
		[DefaultValue(1f)]
		public float CharacterGeneralDamageModifier = 1f;

		[ProtoMember(28)]
		public long? UsingLadder;

		[ProtoMember(31)]
		public SerializableVector2 HeadAngle;

		[ProtoMember(34)]
		public SerializableVector3 LinearVelocity;

		[ProtoMember(37)]
		public float AutoenableJetpackDelay;

		[ProtoMember(40)]
		public bool JetpackEnabled;

		[ProtoMember(43)]
		[NoSerialize]
		public float? Health;

		[ProtoMember(46)]
		[DefaultValue(false)]
		public bool AIMode;

		[ProtoMember(49)]
		public SerializableVector3 ColorMaskHSV;

		[ProtoMember(52)]
		public float LootingCounter;

		[ProtoMember(55)]
		public string DisplayName;

		[ProtoMember(58)]
		public bool IsInFirstPersonView = true;

		[ProtoMember(61)]
		public bool EnableBroadcasting = true;

		[ProtoMember(64)]
		public float OxygenLevel = 1f;

		[ProtoMember(67)]
		public float EnvironmentOxygenLevel = 1f;

		[ProtoMember(70)]
		[Nullable]
		public List<StoredGas> StoredGases;

		[ProtoMember(73)]
		public MyCharacterMovementEnum MovementState;

		[ProtoMember(76)]
		[Nullable]
		public List<string> EnabledComponents;

		[ProtoMember(79)]
		public ulong PlayerSteamId;

		[ProtoMember(82)]
		public int PlayerSerialId;

		[ProtoMember(85)]
		public bool NeedsOxygenFromSuit;

		[ProtoMember(88, IsRequired = false)]
		public long? OwningPlayerIdentityId;

		[ProtoMember(91, IsRequired = false)]
		public bool IsPersistenceCharacter;

		[ProtoMember(94)]
		public long RelativeDampeningEntity;

		[ProtoMember(195)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<BuildPlanItem> BuildPlanner;

		[ProtoMember(210)]
		public LadderInfo? UsingLadderInfo;

		public bool ShouldSerializeHealth()
		{
			return false;
		}

		public bool ShouldSerializeMovementState()
		{
			return MovementState != MyCharacterMovementEnum.Standing;
		}

		public MyObjectBuilder_Character()
		{
			StoredGases = new List<StoredGas>();
			EnabledComponents = new List<string>();
		}
	}
}
