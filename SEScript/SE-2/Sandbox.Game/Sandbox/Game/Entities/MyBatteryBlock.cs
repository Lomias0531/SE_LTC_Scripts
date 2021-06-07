using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_BatteryBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyBatteryBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyBatteryBlock)
	})]
	public class MyBatteryBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyBatteryBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyBatteryBlock
	{
		protected class m_chargeMode_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType chargeMode;
				ISyncType result = chargeMode = new Sync<ChargeMode, SyncDirection.BothWays>(P_1, P_2);
				((MyBatteryBlock)P_0).m_chargeMode = (Sync<ChargeMode, SyncDirection.BothWays>)chargeMode;
				return result;
			}
		}

		protected class m_isFull_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isFull;
				ISyncType result = isFull = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyBatteryBlock)P_0).m_isFull = (Sync<bool, SyncDirection.FromServer>)isFull;
				return result;
			}
		}

		protected class m_storedPower_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType storedPower;
				ISyncType result = storedPower = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyBatteryBlock)P_0).m_storedPower = (Sync<float, SyncDirection.FromServer>)storedPower;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyBatteryBlock_003C_003EActor : IActivator, IActivator<MyBatteryBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyBatteryBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBatteryBlock CreateInstance()
			{
				return new MyBatteryBlock();
			}

			MyBatteryBlock IActivator<MyBatteryBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly string[] m_emissiveTextureNames = new string[4]
		{
			"Emissive0",
			"Emissive1",
			"Emissive2",
			"Emissive3"
		};

		private bool m_hasRemainingCapacity;

		private float m_maxOutput;

		private float m_currentOutput;

		private float m_currentStoredPower;

		private float m_maxStoredPower;

		private int m_lastUpdateTime;

		private float m_timeRemaining;

		private bool m_sourceDirty;

		private const int m_productionUpdateInterval = 100;

		private readonly Sync<ChargeMode, SyncDirection.BothWays> m_chargeMode;

		private readonly Sync<bool, SyncDirection.FromServer> m_isFull;

		private readonly Sync<float, SyncDirection.FromServer> m_storedPower;

		private Color m_prevEmissiveColor = Color.Black;

		private int m_prevFillCount = -1;

		private MyResourceSourceComponent m_sourceComp;

		public new MyBatteryBlockDefinition BlockDefinition => base.BlockDefinition as MyBatteryBlockDefinition;

		public MyResourceSourceComponent SourceComp
		{
			get
			{
				return m_sourceComp;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceSourceComponent)))
				{
					base.Components.Remove<MyResourceSourceComponent>();
				}
				base.Components.Add(value);
				m_sourceComp = value;
			}
		}

		public float TimeRemaining
		{
			get
			{
				return m_timeRemaining;
			}
			set
			{
				m_timeRemaining = value;
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			}
		}

		public bool HasCapacityRemaining => SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId);

		public float MaxStoredPower
		{
			get
			{
				return m_maxStoredPower;
			}
			private set
			{
				if (m_maxStoredPower != value)
				{
					m_maxStoredPower = value;
				}
			}
		}

		private bool ProducerEnabled => (ChargeMode)m_chargeMode != ChargeMode.Recharge;

		public float CurrentStoredPower
		{
			get
			{
				return SourceComp.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
			}
			set
			{
				SourceComp.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(value, 0f, MaxStoredPower));
				UpdateMaxOutputAndEmissivity();
			}
		}

		public float CurrentOutput
		{
			get
			{
				if (SourceComp != null)
				{
					return SourceComp.CurrentOutput;
				}
				return 0f;
			}
		}

		public float MaxOutput
		{
			get
			{
				if (SourceComp != null)
				{
					return SourceComp.MaxOutput;
				}
				return 0f;
			}
		}

		public float CurrentInput
		{
			get
			{
				if (base.ResourceSink != null)
				{
					return base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId);
				}
				return 0f;
			}
		}

		public float MaxInput
		{
			get
			{
				if (base.ResourceSink != null)
				{
					return base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
				}
				return 0f;
			}
		}

		public bool IsCharging
		{
			get
			{
				if (CurrentInput > CurrentOutput)
				{
					return CurrentInput > 0f;
				}
				return false;
			}
		}

		public bool SemiautoEnabled
		{
			get
			{
				return (ChargeMode)m_chargeMode == ChargeMode.Auto;
			}
			set
			{
				if (value)
				{
					m_chargeMode.Value = ChargeMode.Auto;
				}
			}
		}

		public bool OnlyRecharge
		{
			get
			{
				return (ChargeMode)m_chargeMode == ChargeMode.Recharge;
			}
			set
			{
				m_chargeMode.Value = (value ? ChargeMode.Recharge : ChargeMode.Auto);
			}
		}

		public bool OnlyDischarge
		{
			get
			{
				return (ChargeMode)m_chargeMode == ChargeMode.Discharge;
			}
			set
			{
				m_chargeMode.Value = (value ? ChargeMode.Discharge : ChargeMode.Auto);
			}
		}

		public ChargeMode ChargeMode
		{
			get
			{
				return m_chargeMode.Value;
			}
			set
			{
				if (m_chargeMode.Value != value)
				{
					m_chargeMode.Value = value;
				}
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.Enabled && SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public MyBatteryBlock()
		{
			CreateTerminalControls();
			SourceComp = new MyResourceSourceComponent();
			base.ResourceSink = new MyResourceSinkComponent();
			SourceComp.OutputChanged += delegate
			{
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			};
			m_chargeMode.ValueChanged += delegate
			{
				SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, (ChargeMode)m_chargeMode != ChargeMode.Recharge);
				UpdateMaxOutputAndEmissivity();
				m_sourceDirty = true;
			};
			m_storedPower.ValueChanged += delegate
			{
				CapacityChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyBatteryBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCombobox<MyBatteryBlock> myTerminalControlCombobox = new MyTerminalControlCombobox<MyBatteryBlock>("ChargeMode", MySpaceTexts.BlockPropertyTitle_ChargeMode, MySpaceTexts.Blank);
				myTerminalControlCombobox.ComboBoxContent = FillChargeModeCombo;
				myTerminalControlCombobox.Getter = ((MyBatteryBlock x) => (long)x.ChargeMode);
				myTerminalControlCombobox.Setter = delegate(MyBatteryBlock x, long v)
				{
					x.ChargeMode = (ChargeMode)v;
				};
				myTerminalControlCombobox.SetSerializerRange((int)MyEnum<ChargeMode>.Range.Min, (int)MyEnum<ChargeMode>.Range.Max);
				MyTerminalControlFactory.AddControl(myTerminalControlCombobox);
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyBatteryBlock>("Recharge", MyTexts.Get(MySpaceTexts.BlockActionTitle_RechargeToggle), OnRechargeToggle, WriteChargeModeValue, MyTerminalActionIcons.TOGGLE));
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyBatteryBlock>("Discharge", MyTexts.Get(MySpaceTexts.BlockActionTitle_DischargeToggle), OnDischargeToggle, WriteChargeModeValue, MyTerminalActionIcons.TOGGLE));
				MyTerminalControlFactory.AddAction(new MyTerminalAction<MyBatteryBlock>("Auto", MyTexts.Get(MySpaceTexts.BlockActionTitle_AutoEnable), OnAutoEnabled, WriteChargeModeValue, MyTerminalActionIcons.TOGGLE));
			}
		}

		private static void OnRechargeToggle(MyBatteryBlock block)
		{
			block.OnlyRecharge = !block.OnlyRecharge;
		}

		private static void OnDischargeToggle(MyBatteryBlock block)
		{
			block.OnlyDischarge = !block.OnlyDischarge;
		}

		private static void OnAutoEnabled(MyBatteryBlock block)
		{
			block.ChargeMode = ChargeMode.Auto;
		}

		private static void WriteChargeModeValue(MyBatteryBlock block, StringBuilder writeTo)
		{
			switch (block.ChargeMode)
			{
			case ChargeMode.Auto:
				writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Auto));
				break;
			case ChargeMode.Recharge:
				writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Recharge));
				break;
			case ChargeMode.Discharge:
				writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Discharge));
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static void FillChargeModeCombo(List<MyTerminalControlComboBoxItem> list)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MySpaceTexts.BlockPropertyTitle_Auto
			};
			list.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 1L,
				Value = MySpaceTexts.BlockPropertyTitle_Recharge
			};
			list.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 2L,
				Value = MySpaceTexts.BlockPropertyTitle_Discharge
			};
			list.Add(item);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			List<MyResourceSourceInfo> sourceResourceData = new List<MyResourceSourceInfo>
			{
				new MyResourceSourceInfo
				{
					ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
					DefinedOutput = BlockDefinition.MaxPowerOutput,
					ProductionToCapacityMultiplier = 3600f
				}
			};
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, sourceResourceData);
			SourceComp.HasCapacityRemainingChanged += delegate
			{
				UpdateIsWorking();
			};
			SourceComp.ProductionEnabledChanged += Source_ProductionEnabledChanged;
			MyObjectBuilder_BatteryBlock myObjectBuilder_BatteryBlock = (MyObjectBuilder_BatteryBlock)objectBuilder;
			SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, myObjectBuilder_BatteryBlock.ProducerEnabled);
			MaxStoredPower = BlockDefinition.MaxStoredPower;
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, Sink_ComputeRequiredPower);
			base.Init(objectBuilder, cubeGrid);
			if (myObjectBuilder_BatteryBlock.CurrentStoredPower >= 0f)
			{
				CurrentStoredPower = myObjectBuilder_BatteryBlock.CurrentStoredPower;
			}
			else
			{
				CurrentStoredPower = BlockDefinition.InitialStoredPowerRatio * BlockDefinition.MaxStoredPower;
			}
			if (Sync.IsServer)
			{
				m_storedPower.Value = CurrentStoredPower;
			}
			if (myObjectBuilder_BatteryBlock.OnlyDischargeEnabled)
			{
				m_chargeMode.SetLocalValue(ChargeMode.Discharge);
			}
			else
			{
				m_chargeMode.SetLocalValue((ChargeMode)myObjectBuilder_BatteryBlock.ChargeMode);
			}
			UpdateMaxOutputAndEmissivity();
			SetDetailedInfoDirty();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyBatteryBlock_IsWorkingChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			base.ResourceSink.Update();
		}

		private void MyBatteryBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			UpdateMaxOutputAndEmissivity();
			base.ResourceSink.Update();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_BatteryBlock obj = (MyObjectBuilder_BatteryBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.CurrentStoredPower = CurrentStoredPower;
			obj.ProducerEnabled = SourceComp.ProductionEnabled;
			obj.SemiautoEnabled = false;
			obj.OnlyDischargeEnabled = false;
			obj.ChargeMode = (int)ChargeMode;
			return obj;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			m_prevEmissiveColor = Color.White;
			UpdateEmissivity();
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		private float Sink_ComputeRequiredPower()
		{
			bool num = base.Enabled && base.IsFunctional && !m_isFull;
			bool flag = ChargeMode != ChargeMode.Discharge;
			float num2 = (MaxStoredPower - CurrentStoredPower) * 60f / 100f * SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId);
			float num3 = SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId);
			float result = 0f;
			if (num && flag)
			{
				float val = base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
				result = Math.Min(num2 + num3, val);
			}
			return result;
		}

		private float ComputeMaxPowerOutput()
		{
			if (!CheckIsWorking() || !SourceComp.ProductionEnabledByType(MyResourceDistributorComponent.ElectricityId))
			{
				return 0f;
			}
			return BlockDefinition.MaxPowerOutput;
		}

		private void CalculateOutputTimeRemaining()
		{
			if (CurrentStoredPower != 0f && SourceComp.CurrentOutput != 0f)
			{
				TimeRemaining = CurrentStoredPower / ((SourceComp.CurrentOutput - base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId)) / SourceComp.ProductionToCapacityMultiplier);
			}
			else
			{
				TimeRemaining = 0f;
			}
		}

		private void CalculateInputTimeRemaining()
		{
			if (base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) != 0f)
			{
				TimeRemaining = (MaxStoredPower - CurrentStoredPower) / ((base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) - SourceComp.CurrentOutput) / SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId));
			}
			else
			{
				TimeRemaining = 0f;
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			float num = m_lastUpdateTime;
			m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
			if (!base.IsFunctional)
			{
				return;
			}
			UpdateMaxOutputAndEmissivity();
			float timeDeltaMs = ((float)MySession.Static.GameplayFrameCounter - num) * 0.0166666675f * 1000f;
			if (!MySession.Static.CreativeMode)
			{
				if (Sync.IsServer)
				{
					switch (ChargeMode)
					{
					case ChargeMode.Auto:
						TransferPower(timeDeltaMs, base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId));
						break;
					case ChargeMode.Recharge:
						StorePower(timeDeltaMs, base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
						break;
					case ChargeMode.Discharge:
						ConsumePower(timeDeltaMs, SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId));
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
			else if (Sync.IsServer && base.IsFunctional)
			{
				if (ChargeMode != ChargeMode.Discharge)
				{
					float num2 = SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId) * MaxStoredPower / 8f;
					num2 *= ((base.Enabled && base.IsFunctional) ? 1f : 0f);
					StorePower(timeDeltaMs, num2);
				}
				else
				{
					UpdateIsWorking();
					if (!SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
					{
						return;
					}
					CalculateOutputTimeRemaining();
				}
			}
			base.ResourceSink.Update();
			if (m_sourceDirty)
			{
				SourceComp.OnProductionEnabledChanged(MyResourceDistributorComponent.ElectricityId);
			}
			m_sourceDirty = false;
			switch (ChargeMode)
			{
			case ChargeMode.Auto:
				if (base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId))
				{
					CalculateInputTimeRemaining();
				}
				else
				{
					CalculateOutputTimeRemaining();
				}
				break;
			case ChargeMode.Recharge:
				CalculateInputTimeRemaining();
				break;
			case ChargeMode.Discharge:
				CalculateOutputTimeRemaining();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		protected override void OnEnabledChanged()
		{
			SourceComp.Enabled = base.Enabled;
			UpdateMaxOutputAndEmissivity();
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void UpdateMaxOutputAndEmissivity()
		{
			base.ResourceSink.Update();
			SourceComp.SetMaxOutputByType(MyResourceDistributorComponent.ElectricityId, ComputeMaxPowerOutput());
			UpdateEmissivity();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BatteryBlock));
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.MaxPowerOutput, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(BlockDefinition.RequiredPowerInput, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxStoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(MaxStoredPower, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
			MyValueFormatter.AppendWorkInBestUnit(SourceComp.CurrentOutput, detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
			MyValueFormatter.AppendWorkHoursInBestUnit(CurrentStoredPower, detailedInfo);
			detailedInfo.Append("\n");
			float num = base.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId);
			float num2 = SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId);
			if (num > num2)
			{
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
				MyValueFormatter.AppendTimeInBestUnit(m_timeRemaining, detailedInfo);
			}
			else if (num == num2)
			{
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_DepletedIn));
				MyValueFormatter.AppendTimeInBestUnit(float.PositiveInfinity, detailedInfo);
			}
			else
			{
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_DepletedIn));
				MyValueFormatter.AppendTimeInBestUnit(m_timeRemaining, detailedInfo);
			}
		}

		private void TransferPower(float timeDeltaMs, float input, float output)
		{
			float num = input - output;
			if (num < 0f)
			{
				ConsumePower(timeDeltaMs, 0f - num);
			}
			else if (num > 0f)
			{
				StorePower(timeDeltaMs, num);
			}
		}

		private void StorePower(float timeDeltaMs, float input)
		{
			float num = input / (SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId) * 1000f);
			float num2 = timeDeltaMs * num * 0.8f;
			if (num2 > 0f)
			{
				if (CurrentStoredPower + num2 < MaxStoredPower)
				{
					CurrentStoredPower += num2;
				}
				else
				{
					CurrentStoredPower = MaxStoredPower;
					TimeRemaining = 0f;
					if (Sync.IsServer && !m_isFull)
					{
						m_isFull.Value = true;
					}
				}
			}
			if (Sync.IsServer)
			{
				m_storedPower.Value = CurrentStoredPower;
			}
		}

		private void ConsumePower(float timeDeltaMs, float output)
		{
			if (!SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
			{
				return;
			}
			float num = output / (SourceComp.ProductionToCapacityMultiplier * 1000f);
			float num2 = timeDeltaMs * num;
			if (num2 == 0f)
			{
				return;
			}
			if (CurrentStoredPower - num2 <= 0f)
			{
				SourceComp.SetOutput(0f);
				CurrentStoredPower = 0f;
				TimeRemaining = 0f;
			}
			else
			{
				CurrentStoredPower -= num2;
				if (Sync.IsServer && (bool)m_isFull)
				{
					m_isFull.Value = false;
				}
			}
			if (Sync.IsServer)
			{
				m_storedPower.Value = CurrentStoredPower;
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		internal void UpdateEmissivity()
		{
			if (!base.InScene)
			{
				return;
			}
			float fill = 1f;
			Color color = Color.Red;
			MyEmissiveColorStateResult result;
			if (base.IsFunctional && base.Enabled)
			{
				if (base.IsWorking)
				{
					fill = CurrentStoredPower / MaxStoredPower;
					if (ChargeMode == ChargeMode.Auto)
					{
						color = Color.Green;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
						{
							color = result.EmissiveColor;
						}
					}
					else if (ChargeMode == ChargeMode.Discharge)
					{
						color = Color.SteelBlue;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result))
						{
							color = result.EmissiveColor;
						}
					}
					else
					{
						color = Color.Yellow;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
						{
							color = result.EmissiveColor;
						}
					}
				}
				else
				{
					fill = 0.25f;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						color = result.EmissiveColor;
					}
				}
			}
			else if (base.IsFunctional)
			{
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					color = result.EmissiveColor;
				}
			}
			else if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
			{
				color = result.EmissiveColor;
			}
			if (BlockDefinition.Id.SubtypeName == "SmallBlockSmallBatteryBlock")
			{
				MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[0], color, 1f);
			}
			else
			{
				SetEmissive(color, fill);
			}
		}

		private void SetEmissive(Color color, float fill)
		{
			int num = (int)(fill * (float)m_emissiveTextureNames.Length);
			if (base.Render.RenderObjectIDs[0] == uint.MaxValue || (!(color != m_prevEmissiveColor) && num == m_prevFillCount))
			{
				return;
			}
			for (int i = 0; i < m_emissiveTextureNames.Length; i++)
			{
				if (i < num)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], color, 1f);
				}
				else
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], Color.Black, 0f);
				}
			}
			m_prevEmissiveColor = color;
			m_prevFillCount = num;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_prevFillCount = -1;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			UpdateMaxOutputAndEmissivity();
		}

		private void ProducerEnadChanged()
		{
			SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, ProducerEnabled);
		}

		private void Source_ProductionEnabledChanged(MyDefinitionId changedResourceId, MyResourceSourceComponent source)
		{
			UpdateIsWorking();
		}

		private void CapacityChanged()
		{
			CurrentStoredPower = m_storedPower.Value;
		}
	}
}
