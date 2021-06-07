using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyCharacterOxygenComponent : MyCharacterComponent
	{
		private class GasData
		{
			public MyDefinitionId Id;

			public float FillLevel;

			public float MaxCapacity;

			public float Throughput;

			public float NextGasTransfer;

			public int LastOutputTime;

			public int LastInputTime;

			public int NextGasRefill = -1;

			public override string ToString()
			{
				return $"Subtype: {Id.SubtypeName}, FillLevel: {FillLevel}, CurrentCapacity: {FillLevel * MaxCapacity}, MaxCapacity: {MaxCapacity}";
			}
		}

		private class Sandbox_Game_Entities_Character_Components_MyCharacterOxygenComponent_003C_003EActor : IActivator, IActivator<MyCharacterOxygenComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterOxygenComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterOxygenComponent CreateInstance()
			{
				return new MyCharacterOxygenComponent();
			}

			MyCharacterOxygenComponent IActivator<MyCharacterOxygenComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static readonly float LOW_OXYGEN_RATIO = 0.2f;

		public static readonly float GAS_REFILL_RATION = 0.3f;

		private Dictionary<MyDefinitionId, int> m_gasIdToIndex;

		private GasData[] m_storedGases;

		private float m_oldSuitOxygenLevel;

		private const int m_gasRefillInterval = 5;

		private int m_lastOxygenUpdateTime;

		private const int m_updateInterval = 100;

		private MyResourceSinkComponent m_characterGasSink;

		private MyResourceSourceComponent m_characterGasSource;

		public static readonly MyDefinitionId OxygenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		public static readonly MyDefinitionId HydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");

		private MyEntity3DSoundEmitter m_soundEmitter;

		private MySoundPair m_helmetOpenSound = new MySoundPair("PlayHelmetOn");

		private MySoundPair m_helmetCloseSound = new MySoundPair("PlayHelmetOff");

		private MySoundPair m_helmetAirEscapeSound = new MySoundPair("PlayChokeInitiate");

		public MyResourceSinkComponent CharacterGasSink
		{
			get
			{
				return m_characterGasSink;
			}
			set
			{
				SetGasSink(value);
			}
		}

		public MyResourceSourceComponent CharacterGasSource
		{
			get
			{
				return m_characterGasSource;
			}
			set
			{
				SetGasSource(value);
			}
		}

		private MyCharacterDefinition Definition => base.Character.Definition;

		public float OxygenCapacity
		{
			get
			{
				int typeIndex = -1;
				MyDefinitionId gasId = OxygenId;
				if (!TryGetTypeIndex(ref gasId, out typeIndex))
				{
					return 0f;
				}
				return m_storedGases[typeIndex].MaxCapacity;
			}
		}

		public float SuitOxygenAmount
		{
			get
			{
				return GetGasFillLevel(OxygenId) * OxygenCapacity;
			}
			set
			{
				MyDefinitionId gasId = OxygenId;
				UpdateStoredGasLevel(ref gasId, MyMath.Clamp(value / OxygenCapacity, 0f, 1f));
			}
		}

		public float SuitOxygenAmountMissing => OxygenCapacity - GetGasFillLevel(OxygenId) * OxygenCapacity;

		public float SuitOxygenLevel
		{
			get
			{
				if (OxygenCapacity == 0f)
				{
					return 0f;
				}
				return GetGasFillLevel(OxygenId);
			}
			set
			{
				MyDefinitionId gasId = OxygenId;
				UpdateStoredGasLevel(ref gasId, value);
			}
		}

		public bool HelmetEnabled => !NeedsOxygenFromSuit;

		public bool NeedsOxygenFromSuit
		{
			get;
			set;
		}

		public override string ComponentTypeDebugString => "Oxygen Component";

		public float EnvironmentOxygenLevel => base.Character.EnvironmentOxygenLevel;

		public float OxygenLevelAtCharacterLocation => base.Character.OxygenLevel;

		public virtual void Init(MyObjectBuilder_Character characterOb)
		{
			m_lastOxygenUpdateTime = MySession.Static.GameplayFrameCounter;
			m_gasIdToIndex = new Dictionary<MyDefinitionId, int>();
			if (MyFakes.ENABLE_HYDROGEN_FUEL && Definition.SuitResourceStorage != null)
			{
				m_storedGases = new GasData[Definition.SuitResourceStorage.Count];
				for (int i = 0; i < m_storedGases.Length; i++)
				{
					SuitResourceDefinition suitResourceDefinition = Definition.SuitResourceStorage[i];
					m_storedGases[i] = new GasData
					{
						Id = suitResourceDefinition.Id,
						FillLevel = 1f,
						MaxCapacity = suitResourceDefinition.MaxCapacity,
						Throughput = suitResourceDefinition.Throughput,
						LastOutputTime = MySession.Static.GameplayFrameCounter,
						LastInputTime = MySession.Static.GameplayFrameCounter
					};
					m_gasIdToIndex.Add(suitResourceDefinition.Id, i);
				}
				if (characterOb.StoredGases != null && !MySession.Static.CreativeMode)
				{
					foreach (MyObjectBuilder_Character.StoredGas storedGase in characterOb.StoredGases)
					{
						if (m_gasIdToIndex.TryGetValue(storedGase.Id, out int value))
						{
							m_storedGases[value].FillLevel = storedGase.FillLevel;
						}
					}
				}
			}
			if (m_storedGases == null)
			{
				m_storedGases = new GasData[0];
			}
			if (MySession.Static.Settings.EnableOxygen)
			{
				float gasFillLevel = GetGasFillLevel(OxygenId);
				m_oldSuitOxygenLevel = ((gasFillLevel == 0f) ? OxygenCapacity : gasFillLevel);
			}
			if (Sync.IsServer)
			{
				base.Character.EnvironmentOxygenLevelSync.Value = characterOb.EnvironmentOxygenLevel;
				base.Character.OxygenLevelAtCharacterLocation.Value = 0f;
			}
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out string value2);
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out string value3);
			if ((value2 != null && value3 != null) || (base.Character.UseNewAnimationSystem && base.Character.AnimationController.Controller.GetLayerByName("Helmet") != null))
			{
				NeedsOxygenFromSuit = characterOb.NeedsOxygenFromSuit;
			}
			else
			{
				NeedsOxygenFromSuit = Definition.NeedsOxygen;
			}
			base.NeedsUpdateBeforeSimulation = true;
			base.NeedsUpdateBeforeSimulation100 = true;
			if (m_soundEmitter == null)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(base.Character);
			}
			if (!HelmetEnabled)
			{
				AnimateHelmet();
			}
		}

		public virtual void GetObjectBuilder(MyObjectBuilder_Character objectBuilder)
		{
			objectBuilder.OxygenLevel = SuitOxygenLevel;
			objectBuilder.EnvironmentOxygenLevel = base.Character.EnvironmentOxygenLevel;
			objectBuilder.NeedsOxygenFromSuit = NeedsOxygenFromSuit;
			if (m_storedGases == null || m_storedGases.Length == 0)
			{
				return;
			}
			if (objectBuilder.StoredGases == null)
			{
				objectBuilder.StoredGases = new List<MyObjectBuilder_Character.StoredGas>();
			}
			GasData[] storedGases = m_storedGases;
			foreach (GasData storedGas in storedGases)
			{
				if (objectBuilder.StoredGases.TrueForAll((MyObjectBuilder_Character.StoredGas obGas) => obGas.Id != storedGas.Id))
				{
					objectBuilder.StoredGases.Add(new MyObjectBuilder_Character.StoredGas
					{
						Id = storedGas.Id,
						FillLevel = storedGas.FillLevel
					});
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			UpdateOxygen();
		}

		private void UpdateOxygen()
		{
			List<MyEntity> list = new List<MyEntity>();
			BoundingBoxD box = base.Character.PositionComp.WorldAABB;
			bool lowOxygenDamage = MySession.Static.Settings.EnableOxygen;
			bool noOxygenDamage = MySession.Static.Settings.EnableOxygen;
			bool isInEnvironment = true;
			bool flag = false;
			if (Sync.IsServer)
			{
				base.Character.EnvironmentOxygenLevelSync.Value = MyOxygenProviderSystem.GetOxygenInPoint(base.Character.PositionComp.GetPosition());
				base.Character.OxygenLevelAtCharacterLocation.Value = base.Character.EnvironmentOxygenLevel;
				MyOxygenRoom myOxygenRoom = null;
				if (MySession.Static.Settings.EnableOxygen)
				{
					if (TryGetGasData(OxygenId, out GasData data))
					{
						float num = (float)(MySession.Static.GameplayFrameCounter - data.LastOutputTime) * 0.0166666675f;
						flag = (CharacterGasSink.CurrentInputByType(OxygenId) * num > Definition.OxygenConsumption);
						if (flag)
						{
							noOxygenDamage = false;
							lowOxygenDamage = false;
						}
					}
					MyCockpit myCockpit = base.Character.Parent as MyCockpit;
					bool flag2 = false;
					if (myCockpit != null && myCockpit.BlockDefinition.IsPressurized)
					{
						if (!HelmetEnabled && MySession.Static.SurvivalMode && !flag)
						{
							if (myCockpit.OxygenAmount >= Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier)
							{
								myCockpit.OxygenAmount -= Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier;
								noOxygenDamage = false;
								lowOxygenDamage = false;
							}
							else
							{
								myCockpit.OxygenAmount = 0f;
							}
						}
						base.Character.EnvironmentOxygenLevelSync.Value = myCockpit.OxygenFillLevel;
						isInEnvironment = false;
						flag2 = true;
					}
					if (!flag2 || (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound))
					{
						base.Character.OxygenSourceGridEntityId.Value = 0L;
						Vector3D center = base.Character.PositionComp.WorldAABB.Center;
						MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, list);
						foreach (MyEntity item in list)
						{
							MyCubeGrid myCubeGrid = item as MyCubeGrid;
							if (myCubeGrid != null && myCubeGrid.GridSystems.GasSystem != null)
							{
								MyOxygenBlock safeOxygenBlock = myCubeGrid.GridSystems.GasSystem.GetSafeOxygenBlock(center);
								if (safeOxygenBlock != null && safeOxygenBlock.Room != null)
								{
									myOxygenRoom = safeOxygenBlock.Room;
									if (myOxygenRoom.OxygenLevel(myCubeGrid.GridSize) > Definition.PressureLevelForLowDamage && !HelmetEnabled)
									{
										lowOxygenDamage = false;
									}
									if (myOxygenRoom.IsAirtight)
									{
										float value = myOxygenRoom.OxygenLevel(myCubeGrid.GridSize);
										if (!flag2)
										{
											base.Character.EnvironmentOxygenLevelSync.Value = value;
										}
										base.Character.OxygenLevelAtCharacterLocation.Value = value;
										base.Character.OxygenSourceGridEntityId.Value = myCubeGrid.EntityId;
										if (myOxygenRoom.OxygenAmount > Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier)
										{
											if (!HelmetEnabled)
											{
												noOxygenDamage = false;
												safeOxygenBlock.PreviousOxygenAmount = safeOxygenBlock.OxygenAmount() - Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier;
												safeOxygenBlock.OxygenChangeTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
												if (!flag)
												{
													myOxygenRoom.OxygenAmount -= Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier;
												}
											}
											break;
										}
									}
									else
									{
										float environmentOxygen = myOxygenRoom.EnvironmentOxygen;
										base.Character.OxygenLevelAtCharacterLocation.Value = environmentOxygen;
										if (!flag2)
										{
											base.Character.EnvironmentOxygenLevelSync.Value = environmentOxygen;
											if (!HelmetEnabled && base.Character.EnvironmentOxygenLevelSync.Value > Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier)
											{
												noOxygenDamage = false;
												break;
											}
										}
									}
									isInEnvironment = false;
								}
							}
						}
					}
					m_oldSuitOxygenLevel = SuitOxygenLevel;
				}
				UpdateGassesFillLevelsAndAmounts(myOxygenRoom);
			}
			CharacterGasSink.Update();
			if (Sync.IsServer && !MySession.Static.CreativeMode)
			{
				RefillSuitGassesFromBottles();
				if (MySession.Static.Settings.EnableOxygen)
				{
					UpdateSuitOxygen(lowOxygenDamage, noOxygenDamage, isInEnvironment);
				}
				GasData[] storedGases = m_storedGases;
				foreach (GasData gasData in storedGases)
				{
					base.Character.UpdateStoredGas(gasData.Id, gasData.FillLevel);
				}
			}
		}

		private void UpdateSuitOxygen(bool lowOxygenDamage, bool noOxygenDamage, bool isInEnvironment)
		{
			if (noOxygenDamage || lowOxygenDamage)
			{
				if (HelmetEnabled && SuitOxygenAmount > Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier)
				{
					noOxygenDamage = false;
					lowOxygenDamage = false;
				}
				if (isInEnvironment && !HelmetEnabled)
				{
					if (base.Character.EnvironmentOxygenLevelSync.Value > Definition.PressureLevelForLowDamage)
					{
						lowOxygenDamage = false;
					}
					if (base.Character.EnvironmentOxygenLevelSync.Value > 0f)
					{
						noOxygenDamage = false;
					}
				}
			}
			m_oldSuitOxygenLevel = SuitOxygenLevel;
			if (noOxygenDamage)
			{
				base.Character.DoDamage(Definition.DamageAmountAtZeroPressure, MyDamageType.LowPressure, updateSync: true, 0L);
			}
			else if (lowOxygenDamage)
			{
				base.Character.DoDamage(1f, MyDamageType.Asphyxia, updateSync: true, 0L);
			}
			base.Character.UpdateOxygen(SuitOxygenAmount);
		}

		private void RefillSuitGassesFromBottles()
		{
			GasData[] storedGases = m_storedGases;
			foreach (GasData gasData in storedGases)
			{
				if (gasData.FillLevel < GAS_REFILL_RATION)
				{
					if (gasData.NextGasRefill == -1)
					{
						gasData.NextGasRefill = MySandboxGame.TotalGamePlayTimeInMilliseconds + 5000;
					}
					if (MySandboxGame.TotalGamePlayTimeInMilliseconds < gasData.NextGasRefill)
					{
						continue;
					}
					gasData.NextGasRefill = -1;
					List<MyPhysicalInventoryItem> items = base.Character.GetInventory().GetItems();
					bool flag = false;
					foreach (MyPhysicalInventoryItem item in items)
					{
						MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
						if (myObjectBuilder_GasContainerObject != null && myObjectBuilder_GasContainerObject.GasLevel != 0f)
						{
							MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
							if (!(myOxygenContainerDefinition.StoredGasId != gasData.Id))
							{
								float num = myObjectBuilder_GasContainerObject.GasLevel * myOxygenContainerDefinition.Capacity;
								float num2 = Math.Min(num, (1f - gasData.FillLevel) * gasData.MaxCapacity);
								myObjectBuilder_GasContainerObject.GasLevel = Math.Max((num - num2) / myOxygenContainerDefinition.Capacity, 0f);
								_ = myObjectBuilder_GasContainerObject.GasLevel;
								_ = 1f;
								base.Character.GetInventory().UpdateGasAmount();
								flag = true;
								TransferSuitGas(ref gasData.Id, num2, 0f);
								if (gasData.FillLevel == 1f)
								{
									break;
								}
							}
						}
					}
					if (flag && MySession.Static.LocalCharacter != base.Character)
					{
						base.Character.SendRefillFromBottle(gasData.Id);
					}
					MyCharacterJetpackComponent jetpackComp = base.Character.JetpackComp;
					if (jetpackComp != null && jetpackComp.TurnedOn && jetpackComp.FuelDefinition != null && jetpackComp.FuelDefinition.Id == gasData.Id && gasData.FillLevel <= 0f && ((base.Character.ControllerInfo.Controller != null && !MySession.Static.CreativeToolsEnabled(base.Character.ControllerInfo.Controller.Player.Id.SteamId)) || (MySession.Static.LocalCharacter != base.Character && !Sync.IsServer)))
					{
						if (Sync.IsServer && MySession.Static.LocalCharacter != base.Character)
						{
							MyMultiplayer.RaiseEvent(base.Character, (MyCharacter x) => x.SwitchJetpack, new EndpointId(base.Character.ControllerInfo.Controller.Player.Id.SteamId));
						}
						jetpackComp.SwitchThrusts();
					}
				}
				else
				{
					gasData.NextGasRefill = -1;
				}
			}
		}

		private void UpdateGassesFillLevelsAndAmounts(MyOxygenRoom room)
		{
			GasData[] storedGases = m_storedGases;
			foreach (GasData gasData in storedGases)
			{
				float num = (float)(MySession.Static.GameplayFrameCounter - gasData.LastOutputTime) * 0.0166666675f;
				float num2 = (float)(MySession.Static.GameplayFrameCounter - gasData.LastInputTime) * 0.0166666675f;
				gasData.LastOutputTime = MySession.Static.GameplayFrameCounter;
				gasData.LastInputTime = MySession.Static.GameplayFrameCounter;
				float num3 = CharacterGasSource.CurrentOutputByType(gasData.Id) * num;
				float num4 = CharacterGasSink.CurrentInputByType(gasData.Id) * num2;
				if (gasData.Id == OxygenId && MySession.Static.Settings.EnableOxygen && Definition.OxygenSuitRefillTime > 0f && gasData.FillLevel < 1f)
				{
					float num5 = MySession.Static.Settings.EnableOxygenPressurization ? Math.Max(base.Character.EnvironmentOxygenLevel, base.Character.OxygenLevel) : base.Character.EnvironmentOxygenLevel;
					if (num5 >= Definition.MinOxygenLevelForSuitRefill)
					{
						float num6 = gasData.MaxCapacity / Definition.OxygenSuitRefillTime * (num2 * 1000f);
						float value = gasData.MaxCapacity - gasData.FillLevel * gasData.MaxCapacity;
						float num7 = MathHelper.Min(num5 * num6, value);
						num4 += num7;
						if (MySession.Static.Settings.EnableOxygenPressurization && room != null && room.IsAirtight)
						{
							if (room.OxygenAmount >= num4)
							{
								room.OxygenAmount -= num4;
							}
							else
							{
								num4 = room.OxygenAmount;
								room.OxygenAmount = 0f;
							}
						}
					}
				}
				float num8 = 0f - MathHelper.Clamp(gasData.NextGasTransfer, float.NegativeInfinity, 0f);
				float num9 = MathHelper.Clamp(gasData.NextGasTransfer, 0f, float.PositiveInfinity);
				gasData.NextGasTransfer = 0f;
				TransferSuitGas(ref gasData.Id, num4 + num9, num3 + num8);
			}
		}

		public void SwitchHelmet()
		{
			if (MySession.Static == null || base.Character == null || base.Character.IsDead || base.Character.AnimationController == null || base.Character.AtmosphereDetectorComp == null)
			{
				return;
			}
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out string value);
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out string value2);
			if ((value != null && value2 != null) || (base.Character.UseNewAnimationSystem && base.Character.AnimationController.Controller.GetLayerByName("Helmet") != null))
			{
				NeedsOxygenFromSuit = !NeedsOxygenFromSuit;
				AnimateHelmet();
			}
			base.Character.SinkComp.Update();
			if (m_soundEmitter != null)
			{
				bool force2D = false;
				if (NeedsOxygenFromSuit)
				{
					m_soundEmitter.PlaySound(m_helmetOpenSound, stopPrevious: true, skipIntro: false, force2D, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
				}
				else
				{
					m_soundEmitter.PlaySound(m_helmetCloseSound, stopPrevious: true, skipIntro: false, force2D, alwaysHearOnRealistic: false, skipToEnd: false, !MyFakes.FORCE_CHARACTER_2D_SOUND);
				}
				if (!MySession.Static.CreativeMode && NeedsOxygenFromSuit && base.Character.AtmosphereDetectorComp != null && !base.Character.AtmosphereDetectorComp.InAtmosphere && !base.Character.AtmosphereDetectorComp.InShipOrStation && SuitOxygenAmount >= 0.5f)
				{
					m_soundEmitter.PlaySound(m_helmetAirEscapeSound, stopPrevious: false, skipIntro: false, force2D);
				}
			}
			if (MyFakes.ENABLE_NEW_SOUNDS && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE && MySession.Static.Settings.RealisticSound)
			{
				MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: true, updateNotPlaying: false);
			}
		}

		private void AnimateHelmet()
		{
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetOpen", out string value);
			base.Character.Definition.AnimationNameToSubtypeName.TryGetValue("HelmetClose", out string value2);
			if (base.Character.Definition != null)
			{
				if (NeedsOxygenFromSuit && value != null)
				{
					base.Character.PlayCharacterAnimation(value, MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0.2f, 1f, sync: true);
				}
				else if (!NeedsOxygenFromSuit && value2 != null)
				{
					base.Character.PlayCharacterAnimation(value2, MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0.2f, 1f, sync: true);
				}
			}
		}

		public bool ContainsGasStorage(MyDefinitionId gasId)
		{
			return m_gasIdToIndex.ContainsKey(gasId);
		}

		private bool TryGetGasData(MyDefinitionId gasId, out GasData data)
		{
			int typeIndex = -1;
			data = null;
			if (TryGetTypeIndex(ref gasId, out typeIndex))
			{
				data = m_storedGases[typeIndex];
				return true;
			}
			return false;
		}

		public float GetGasFillLevel(MyDefinitionId gasId)
		{
			int typeIndex = -1;
			if (!TryGetTypeIndex(ref gasId, out typeIndex))
			{
				return 0f;
			}
			return m_storedGases[typeIndex].FillLevel;
		}

		public void UpdateStoredGasLevel(ref MyDefinitionId gasId, float fillLevel)
		{
			int typeIndex = -1;
			if (TryGetTypeIndex(ref gasId, out typeIndex))
			{
				m_storedGases[typeIndex].FillLevel = fillLevel;
				CharacterGasSource.SetRemainingCapacityByType(gasId, fillLevel * m_storedGases[typeIndex].MaxCapacity);
				CharacterGasSource.SetProductionEnabledByType(gasId, fillLevel > 0f);
			}
		}

		internal void TransferSuitGas(ref MyDefinitionId gasId, float gasInput, float gasOutput)
		{
			int typeIndex = GetTypeIndex(ref gasId);
			float num = gasInput - gasOutput;
			if (MySession.Static.CreativeMode)
			{
				num = Math.Max(num, 0f);
			}
			if (num != 0f)
			{
				GasData gasData = m_storedGases[typeIndex];
				gasData.FillLevel = MathHelper.Clamp(gasData.FillLevel + num / gasData.MaxCapacity, 0f, 1f);
				CharacterGasSource.SetRemainingCapacityByType(gasData.Id, gasData.FillLevel * gasData.MaxCapacity);
				CharacterGasSource.SetProductionEnabledByType(gasData.Id, gasData.FillLevel > 0f);
			}
		}

		private void Source_CurrentOutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (TryGetTypeIndex(ref changedResourceId, out int typeIndex))
			{
				float num = (float)(MySession.Static.GameplayFrameCounter - m_storedGases[typeIndex].LastOutputTime) * 0.0166666675f;
				m_storedGases[typeIndex].LastOutputTime = MySession.Static.GameplayFrameCounter;
				float num2 = oldOutput * num;
				m_storedGases[typeIndex].NextGasTransfer -= num2;
			}
		}

		private void Sink_CurrentInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			if (TryGetTypeIndex(ref resourceTypeId, out int typeIndex))
			{
				float num = (float)(MySession.Static.GameplayFrameCounter - m_storedGases[typeIndex].LastInputTime) * 0.0166666675f;
				float num2 = oldInput * num;
				m_storedGases[typeIndex].NextGasTransfer += num2;
			}
		}

		private void SetGasSink(MyResourceSinkComponent characterSinkComponent)
		{
			GasData[] storedGases = m_storedGases;
			for (int i = 0; i < storedGases.Length; i++)
			{
				storedGases[i].LastInputTime = MySession.Static.GameplayFrameCounter;
				if (Sync.IsServer)
				{
					if (m_characterGasSink != null)
					{
						m_characterGasSink.CurrentInputChanged -= Sink_CurrentInputChanged;
					}
					if (characterSinkComponent != null)
					{
						characterSinkComponent.CurrentInputChanged += Sink_CurrentInputChanged;
					}
				}
			}
			m_characterGasSink = characterSinkComponent;
		}

		private void SetGasSource(MyResourceSourceComponent characterSourceComponent)
		{
			GasData[] storedGases = m_storedGases;
			foreach (GasData gasData in storedGases)
			{
				gasData.LastOutputTime = MySession.Static.GameplayFrameCounter;
				if (m_characterGasSource != null)
				{
					m_characterGasSource.SetRemainingCapacityByType(gasData.Id, 0f);
					if (Sync.IsServer)
					{
						m_characterGasSource.OutputChanged -= Source_CurrentOutputChanged;
					}
				}
				if (characterSourceComponent != null)
				{
					characterSourceComponent.SetRemainingCapacityByType(gasData.Id, gasData.FillLevel * gasData.MaxCapacity);
					characterSourceComponent.SetProductionEnabledByType(gasData.Id, gasData.FillLevel > 0f);
					if (Sync.IsServer)
					{
						characterSourceComponent.OutputChanged += Source_CurrentOutputChanged;
					}
				}
			}
			m_characterGasSource = characterSourceComponent;
		}

		public void AppendSinkData(List<MyResourceSinkInfo> sinkData)
		{
			for (int i = 0; i < m_storedGases.Length; i++)
			{
				int captureIndex = i;
				sinkData.Add(new MyResourceSinkInfo
				{
					ResourceTypeId = m_storedGases[i].Id,
					MaxRequiredInput = m_storedGases[i].Throughput,
					RequiredInputFunc = (() => Sink_ComputeRequiredGas(m_storedGases[captureIndex]))
				});
			}
		}

		public void AppendSourceData(List<MyResourceSourceInfo> sourceData)
		{
			for (int i = 0; i < m_storedGases.Length; i++)
			{
				sourceData.Add(new MyResourceSourceInfo
				{
					ResourceTypeId = m_storedGases[i].Id,
					DefinedOutput = m_storedGases[i].Throughput,
					ProductionToCapacityMultiplier = 1f,
					IsInfiniteCapacity = false
				});
			}
		}

		private float Sink_ComputeRequiredGas(GasData gas)
		{
			return Math.Min(((1f - gas.FillLevel) * gas.MaxCapacity + ((gas.Id == OxygenId) ? (Definition.OxygenConsumption * Definition.OxygenConsumptionMultiplier) : 0f)) / 60f * 100f, gas.Throughput);
		}

		private int GetTypeIndex(ref MyDefinitionId gasId)
		{
			int result = 0;
			if (m_gasIdToIndex.Count > 1)
			{
				result = m_gasIdToIndex[gasId];
			}
			return result;
		}

		private bool TryGetTypeIndex(ref MyDefinitionId gasId, out int typeIndex)
		{
			return m_gasIdToIndex.TryGetValue(gasId, out typeIndex);
		}
	}
}
