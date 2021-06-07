using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_AirVent))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyAirVent),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent)
	})]
	public class MyAirVent : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyAirVent, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent, IMyGasBlock, IMyConveyorEndpointBlock
	{
		protected sealed class StopVentEffectImplementation_003C_003E : ICallSite<MyAirVent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAirVent @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.StopVentEffectImplementation();
			}
		}

		protected sealed class CreateVentEffectImplementation_003C_003E : ICallSite<MyAirVent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAirVent @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateVentEffectImplementation();
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyAirVent, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAirVent @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected class m_isDepressurizing_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isDepressurizing;
				ISyncType result = isDepressurizing = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyAirVent)P_0).m_isDepressurizing = (Sync<bool, SyncDirection.BothWays>)isDepressurizing;
				return result;
			}
		}

		protected class m_blockRoomInfo_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType blockRoomInfo;
				ISyncType result = blockRoomInfo = new Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer>(P_1, P_2);
				((MyAirVent)P_0).m_blockRoomInfo = (Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer>)blockRoomInfo;
				return result;
			}
		}

		private static readonly string[] m_emissiveTextureNames = new string[4]
		{
			"Emissive0",
			"Emissive1",
			"Emissive2",
			"Emissive3"
		};

		private MyStringHash m_prevColor = MyStringHash.NullOrEmpty;

		private int m_prevFillCount = -1;

		private bool m_isProducing;

		private bool m_producedSinceLastUpdate;

		private bool m_isPlayingVentEffect;

		private MyParticleEffect m_effect;

		private MyToolbarItem m_onFullAction;

		private MyToolbarItem m_onEmptyAction;

		private MyToolbar m_actionToolbar;

		private bool? m_wasRoomFull;

		private bool? m_wasRoomEmpty;

		private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		private readonly Sync<bool, SyncDirection.BothWays> m_isDepressurizing;

		private readonly Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer> m_blockRoomInfo;

		private MyResourceSourceComponent m_sourceComp;

		private MyResourceSinkInfo OxygenSinkInfo;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private bool m_syncing;

		private MyModelDummy VentDummy
		{
			get
			{
				if (base.Model == null || base.Model.Dummies == null)
				{
					return null;
				}
				base.Model.Dummies.TryGetValue("vent_001", out MyModelDummy value);
				return value;
			}
		}

		public bool CanVent
		{
			get
			{
				if (MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
				{
					return base.IsWorking;
				}
				return false;
			}
		}

		public bool CanVentToRoom
		{
			get
			{
				if (CanVent)
				{
					return !IsDepressurizing;
				}
				return false;
			}
		}

		public bool CanVentFromRoom
		{
			get
			{
				if (CanVent)
				{
					return IsDepressurizing;
				}
				return false;
			}
		}

		public float GasOutputPerSecond
		{
			get
			{
				if (!SourceComp.ProductionEnabledByType(m_oxygenGasId))
				{
					return 0f;
				}
				return SourceComp.CurrentOutputByType(m_oxygenGasId);
			}
		}

		public float GasInputPerSecond
		{
			get
			{
				if (!IsDepressurizing)
				{
					return base.ResourceSink.CurrentInputByType(m_oxygenGasId);
				}
				return 0f;
			}
		}

		public float GasOutputPerUpdate => GasOutputPerSecond * 0.0166666675f;

		public float GasInputPerUpdate => GasInputPerSecond * 0.0166666675f;

		public bool IsDepressurizing
		{
			get
			{
				return m_isDepressurizing;
			}
			set
			{
				m_isDepressurizing.Value = value;
			}
		}

		public VentStatus Status
		{
			get;
			private set;
		}

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

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool CanPressurizeRoom => true;

		private new MyAirVentDefinition BlockDefinition => (MyAirVentDefinition)base.BlockDefinition;

		public bool CanPressurize
		{
			get
			{
				MyOxygenBlock oxygenBlock = GetOxygenBlock();
				if (oxygenBlock == null || oxygenBlock.Room == null)
				{
					return false;
				}
				return oxygenBlock.Room.IsAirtight;
			}
		}

		MyResourceSinkInfo SpaceEngineers.Game.ModAPI.IMyAirVent.OxygenSinkInfo
		{
			get
			{
				return OxygenSinkInfo;
			}
			set
			{
				OxygenSinkInfo = value;
			}
		}

		MyResourceSourceComponent SpaceEngineers.Game.ModAPI.IMyAirVent.SourceComp
		{
			get
			{
				return SourceComp;
			}
			set
			{
				SourceComp = value;
			}
		}

		float SpaceEngineers.Game.ModAPI.IMyAirVent.GasOutputPerSecond => GasOutputPerSecond;

		float SpaceEngineers.Game.ModAPI.IMyAirVent.GasInputPerSecond => GasInputPerSecond;

		VentStatus SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.Status => Status;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.Depressurize
		{
			get
			{
				return IsDepressurizing;
			}
			set
			{
				IsDepressurizing = value;
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.PressurizationEnabled => MySession.Static.Settings.EnableOxygenPressurization;

		public MyAirVent()
		{
			CreateTerminalControls();
			base.ResourceSink = new MyResourceSinkComponent(2);
			SourceComp = new MyResourceSourceComponent();
			m_isDepressurizing.ValueChanged += delegate
			{
				SetDepressurizing();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyAirVent>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyAirVent> obj = new MyTerminalControlOnOffSwitch<MyAirVent>("Depressurize", MySpaceTexts.BlockPropertyTitle_Depressurize, MySpaceTexts.BlockPropertyDescription_Depressurize)
				{
					Getter = ((MyAirVent x) => x.IsDepressurizing),
					Setter = delegate(MyAirVent x, bool v)
					{
						x.IsDepressurizing = v;
						x.UpdateEmissivity();
					}
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyAirVent>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MyAirVent self)
				{
					if (self.m_onFullAction != null)
					{
						self.m_actionToolbar.SetItemAtIndex(0, self.m_onFullAction);
					}
					if (self.m_onEmptyAction != null)
					{
						self.m_actionToolbar.SetItemAtIndex(1, self.m_onEmptyAction);
					}
					self.m_actionToolbar.ItemChanged += self.Toolbar_ItemChanged;
					if (MyGuiScreenToolbarConfigBase.Static == null)
					{
						MyToolbarComponent.CurrentToolbar = self.m_actionToolbar;
						MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
						MyToolbarComponent.AutoUpdate = false;
						myGuiScreenBase.Closed += delegate
						{
							MyToolbarComponent.AutoUpdate = true;
							self.m_actionToolbar.ItemChanged -= self.Toolbar_ItemChanged;
							self.m_actionToolbar.Clear();
						};
						MyGuiSandbox.AddScreen(myGuiScreenBase);
					}
				})
				{
					SupportsMultipleBlocks = false
				});
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_AirVent myObjectBuilder_AirVent = (MyObjectBuilder_AirVent)objectBuilder;
			InitializeConveyorEndpoint();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo
			{
				ResourceTypeId = m_oxygenGasId,
				DefinedOutput = BlockDefinition.VentilationCapacityPerSecond,
				ProductionToCapacityMultiplier = 1f
			});
			SourceComp.OutputChanged += Source_OutputChanged;
			MyResourceSinkInfo myResourceSinkInfo = new MyResourceSinkInfo
			{
				ResourceTypeId = m_oxygenGasId,
				MaxRequiredInput = BlockDefinition.VentilationCapacityPerSecond,
				RequiredInputFunc = Sink_ComputeRequiredGas
			};
			OxygenSinkInfo = myResourceSinkInfo;
			List<MyResourceSinkInfo> list = new List<MyResourceSinkInfo>();
			myResourceSinkInfo = new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = BlockDefinition.OperationalPowerConsumption,
				RequiredInputFunc = ComputeRequiredPower
			};
			list.Add(myResourceSinkInfo);
			List<MyResourceSinkInfo> sinkData = list;
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, sinkData);
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink.CurrentInputChanged += Sink_CurrentInputChanged;
			m_actionToolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
			m_actionToolbar.DrawNumbers = false;
			m_actionToolbar.Init(null, this);
			if (myObjectBuilder_AirVent.OnFullAction != null)
			{
				m_onFullAction = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_AirVent.OnFullAction);
			}
			if (myObjectBuilder_AirVent.OnEmptyAction != null)
			{
				m_onEmptyAction = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_AirVent.OnEmptyAction);
			}
			UpdateEmissivity();
			UpdateStatus();
			SetDetailedInfoDirty();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyAirVent_IsWorkingChanged;
			m_isDepressurizing.SetLocalValue(myObjectBuilder_AirVent.IsDepressurizing);
			SetDepressurizing();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_AirVent myObjectBuilder_AirVent = (MyObjectBuilder_AirVent)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_AirVent.IsDepressurizing = IsDepressurizing;
			if (m_onFullAction != null)
			{
				myObjectBuilder_AirVent.OnFullAction = m_onFullAction.GetObjectBuilder();
			}
			if (m_onEmptyAction != null)
			{
				myObjectBuilder_AirVent.OnEmptyAction = m_onEmptyAction.GetObjectBuilder();
			}
			return myObjectBuilder_AirVent;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_isProducing = m_producedSinceLastUpdate;
			m_producedSinceLastUpdate = false;
			ExecuteGasTransfer();
			UpdateStatus();
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void ExecuteGasTransfer()
		{
			float num = GasInputPerUpdate - GasOutputPerUpdate;
			if (num != 0f)
			{
				Transfer(num);
				SourceComp.OnProductionEnabledChanged(m_oxygenGasId);
			}
			else
			{
				if (!base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
				StopVentEffect();
			}
			base.ResourceSink.Update();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (Sync.IsServer)
			{
				if (base.IsWorking)
				{
					UpdateActions();
				}
				bool isRoomAirtight = oxygenBlock != null && oxygenBlock.Room != null && oxygenBlock.Room.IsAirtight;
				float roomEnvironmentOxygen = (oxygenBlock != null && oxygenBlock.Room != null) ? oxygenBlock.Room.EnvironmentOxygen : 0f;
				float oxygenLevel = oxygenBlock?.OxygenLevel(base.CubeGrid.GridSize) ?? 0f;
				m_blockRoomInfo.Value = new MyAirVentBlockRoomInfo(isRoomAirtight, oxygenLevel, roomEnvironmentOxygen);
			}
			SourceComp.SetRemainingCapacityByType(m_oxygenGasId, (oxygenBlock != null && oxygenBlock.Room != null && oxygenBlock.Room.IsAirtight) ? oxygenBlock.Room.OxygenAmount : ((MyOxygenProviderSystem.GetOxygenInPoint(base.WorldMatrix.Translation) != 0f) ? BlockDefinition.VentilationCapacityPerSecond : 0f));
			UpdateStatus();
			UpdateEmissivity();
			SetDetailedInfoDirty();
			base.ResourceSink.Update();
			if (MyFakes.ENABLE_OXYGEN_SOUNDS)
			{
				UpdateSound();
			}
		}

		private void StopVentEffect()
		{
			if (Sync.IsServer && m_isPlayingVentEffect)
			{
				m_isPlayingVentEffect = false;
				MyMultiplayer.RaiseEvent(this, (MyAirVent x) => x.StopVentEffectImplementation);
				if (Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					StopVentEffectImplementation();
				}
			}
		}

		[Event(null, 336)]
		[Reliable]
		[Broadcast]
		private void StopVentEffectImplementation()
		{
			m_isPlayingVentEffect = false;
			if (m_effect != null)
			{
				m_effect.Stop(instant: false);
				m_effect = null;
			}
		}

		private void Source_OutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!(changedResourceId != m_oxygenGasId))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void Sink_CurrentInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			if (!(resourceTypeId != m_oxygenGasId))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void Transfer(float transferAmount)
		{
			if (transferAmount > 0f)
			{
				VentToRoom(transferAmount);
			}
			else if (transferAmount < 0f)
			{
				DrainFromRoom(0f - transferAmount);
			}
		}

		private void UpdateSound()
		{
			if (m_soundEmitter == null)
			{
				return;
			}
			if (base.IsWorking)
			{
				if (m_isPlayingVentEffect)
				{
					if (IsDepressurizing)
					{
						if (!m_soundEmitter.IsPlaying || !m_soundEmitter.SoundPair.Equals(BlockDefinition.DepressurizeSound))
						{
							m_soundEmitter.PlaySound(BlockDefinition.DepressurizeSound, stopPrevious: true);
						}
					}
					else if (!m_soundEmitter.IsPlaying || !m_soundEmitter.SoundPair.Equals(BlockDefinition.PressurizeSound))
					{
						m_soundEmitter.PlaySound(BlockDefinition.PressurizeSound, stopPrevious: true);
					}
				}
				else if (!m_soundEmitter.IsPlaying || !m_soundEmitter.SoundPair.Equals(BlockDefinition.IdleSound))
				{
					if (m_soundEmitter.IsPlaying && (m_soundEmitter.SoundPair.Equals(BlockDefinition.PressurizeSound) || m_soundEmitter.SoundPair.Equals(BlockDefinition.DepressurizeSound)))
					{
						m_soundEmitter.StopSound(forced: false);
					}
					m_soundEmitter.PlaySound(BlockDefinition.IdleSound, stopPrevious: true);
				}
			}
			else if (m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
			}
			m_soundEmitter.Update();
		}

		private void CreateEffect()
		{
			if (Sync.IsServer)
			{
				m_isPlayingVentEffect = true;
				MyMultiplayer.RaiseEvent(this, (MyAirVent x) => x.CreateVentEffectImplementation);
				if (Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					CreateVentEffectImplementation();
				}
			}
		}

		[Event(null, 436)]
		[Reliable]
		[Broadcast]
		private void CreateVentEffectImplementation()
		{
			StopVentEffectImplementation();
			MatrixD effectMatrix = base.PositionComp.LocalMatrix;
			if (IsDepressurizing)
			{
				effectMatrix.Left = effectMatrix.Right;
				effectMatrix.Forward = effectMatrix.Backward;
			}
			effectMatrix.Translation += base.PositionComp.LocalMatrix.Forward * ((BlockDefinition.CubeSize == MyCubeSize.Large) ? 1f : 0.1f);
			Vector3D worldPosition = Vector3D.Zero;
			if (MyParticlesManager.TryCreateParticleEffect("OxyVent", ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_effect))
			{
				if (BlockDefinition.CubeSize == MyCubeSize.Large)
				{
					m_effect.UserScale = 3f;
				}
				else
				{
					m_effect.UserScale = 0.5f;
				}
			}
			m_isPlayingVentEffect = true;
		}

		private void UpdateActions()
		{
			float oxygenLevel = GetOxygenLevel();
			if (!m_wasRoomEmpty.HasValue || !m_wasRoomFull.HasValue)
			{
				m_wasRoomEmpty = false;
				m_wasRoomFull = false;
				if (oxygenLevel > 0.99f)
				{
					m_wasRoomFull = true;
				}
				else if (oxygenLevel < 0.01f)
				{
					m_wasRoomEmpty = true;
				}
			}
			else if (oxygenLevel > 0.99f)
			{
				m_wasRoomEmpty = false;
				if (!m_wasRoomFull.Value)
				{
					ExecuteAction(m_onFullAction);
					m_wasRoomFull = true;
				}
			}
			else if (oxygenLevel < 0.01f)
			{
				m_wasRoomFull = false;
				if (!m_wasRoomEmpty.Value)
				{
					ExecuteAction(m_onEmptyAction);
					m_wasRoomEmpty = true;
				}
			}
			else
			{
				m_wasRoomFull = false;
				m_wasRoomEmpty = false;
			}
		}

		private void ExecuteAction(MyToolbarItem action)
		{
			m_actionToolbar.SetItemAtIndex(0, action);
			m_actionToolbar.UpdateItem(0);
			m_actionToolbar.ActivateItemAtSlot(0);
			m_actionToolbar.Clear();
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index)
		{
			if (!m_syncing)
			{
				ToolbarItem arg = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
				MyMultiplayer.RaiseEvent(this, (MyAirVent x) => x.SendToolbarItemChanged, arg, index.ItemIndex);
			}
		}

		private float ComputeRequiredPower()
		{
			if (!MySession.Static.Settings.EnableOxygen || !base.Enabled || !base.IsFunctional || !MySession.Static.Settings.EnableOxygenPressurization)
			{
				return 0f;
			}
			if (!m_isProducing)
			{
				return BlockDefinition.StandbyPowerConsumption;
			}
			return BlockDefinition.OperationalPowerConsumption;
		}

		private float Sink_ComputeRequiredGas()
		{
			if (!CanVentToRoom)
			{
				return 0f;
			}
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight)
			{
				return 0f;
			}
			float num = oxygenBlock.Room.MissingOxygen(base.CubeGrid.GridSize);
			if (num < 0.0001f)
			{
				oxygenBlock.Room.OxygenAmount = oxygenBlock.Room.MaxOxygen(base.CubeGrid.GridSize);
				num = 0f;
			}
			else
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			float num2 = num * 60f * SourceComp.ProductionToCapacityMultiplierByType(m_oxygenGasId);
			float num3 = SourceComp.CurrentOutputByType(m_oxygenGasId);
			return Math.Min(num2 + num3, BlockDefinition.VentilationCapacityPerSecond);
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			if (base.CubeGrid != null && SourceComp != null && base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.ResourceDistributor != null && !base.CubeGrid.Closed)
			{
				SourceComp.Enabled = base.IsWorking;
				base.ResourceSink.Update();
				base.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
				UpdateEmissivity();
				UpdateStatus();
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = base.IsWorking;
			base.ResourceSink.Update();
			UpdateEmissivity();
			UpdateStatus();
		}

		private void MyAirVent_IsWorkingChanged(MyCubeBlock obj)
		{
			SourceComp.Enabled = base.IsWorking;
			UpdateEmissivity();
			UpdateStatus();
		}

		private bool SetEmissiveStateForVent(MyStringHash state, float fillLevel)
		{
			int num = (int)(fillLevel * (float)m_emissiveTextureNames.Length);
			bool flag = false;
			if (base.Render.RenderObjectIDs[0] != uint.MaxValue && (state != m_prevColor || num != m_prevFillCount))
			{
				for (int i = 0; i < m_emissiveTextureNames.Length; i++)
				{
					flag |= SetEmissiveState((i <= num) ? state : MyCubeBlock.m_emissiveNames.Damaged, base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i]);
				}
				m_prevColor = state;
				m_prevFillCount = num;
			}
			return flag;
		}

		public override bool SetEmissiveStateWorking()
		{
			return UpdateEmissivity();
		}

		public override bool SetEmissiveStateDamaged()
		{
			return SetEmissiveStateForVent(MyCubeBlock.m_emissiveNames.Damaged, 1f);
		}

		public override bool SetEmissiveStateDisabled()
		{
			return SetEmissiveStateForVent(MyCubeBlock.m_emissiveNames.Disabled, 1f);
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_prevFillCount = -1;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateStatus();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (oxygenBlock == null || oxygenBlock.Room == null)
			{
				Status = VentStatus.Depressurized;
			}
			else if (oxygenBlock.Room.IsAirtight)
			{
				if (oxygenBlock.Room.OxygenLevel(base.CubeGrid.GridSize) >= 1f)
				{
					if (Sync.IsServer && MyVisualScriptLogicProvider.RoomFullyPressurized != null && Status != VentStatus.Pressurized)
					{
						MyVisualScriptLogicProvider.RoomFullyPressurized(base.EntityId, base.CubeGrid.EntityId, Name, base.CubeGrid.Name);
					}
					Status = VentStatus.Pressurized;
				}
				else
				{
					Status = (IsDepressurizing ? VentStatus.Depressurizing : VentStatus.Pressurizing);
				}
			}
			else if (oxygenBlock.Room.OxygenLevel(base.CubeGrid.GridSize) > 0.01f)
			{
				Status = VentStatus.Depressurizing;
			}
			else
			{
				Status = VentStatus.Depressurized;
			}
			CheckEmissiveState();
		}

		public bool IsRoomAirtight()
		{
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (oxygenBlock != null && oxygenBlock.Room != null)
			{
				return oxygenBlock.Room.IsAirtight;
			}
			return false;
		}

		private bool UpdateEmissivity()
		{
			if (base.IsWorking)
			{
				MyOxygenBlock oxygenBlock = GetOxygenBlock();
				bool flag = oxygenBlock != null && oxygenBlock.Room != null;
				bool flag2 = flag && oxygenBlock.Room.IsAirtight;
				float val = flag ? oxygenBlock.Room.EnvironmentOxygen : 0f;
				float num = flag ? oxygenBlock.OxygenLevel(base.CubeGrid.GridSize) : 0f;
				if (!Sync.IsServer)
				{
					flag2 = m_blockRoomInfo.Value.IsRoomAirtight;
					val = m_blockRoomInfo.Value.RoomEnvironmentOxygen;
					num = m_blockRoomInfo.Value.OxygenLevel;
					flag = true;
				}
				if (flag)
				{
					if (flag2)
					{
						return SetEmissiveStateForVent(IsDepressurizing ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working, num);
					}
					MyStringHash state = MyCubeBlock.m_emissiveNames.Warning;
					if (IsDepressurizing && MyOxygenProviderSystem.GetOxygenInPoint(base.WorldMatrix.Translation) > 0f)
					{
						state = MyCubeBlock.m_emissiveNames.Alternative;
					}
					return SetEmissiveStateForVent(state, Math.Max(num, val));
				}
				float num2 = (int)(MyOxygenProviderSystem.GetOxygenInPoint(base.WorldMatrix.Translation) * (float)m_emissiveTextureNames.Length);
				return SetEmissiveStateForVent((num2 == 0f) ? MyCubeBlock.m_emissiveNames.Warning : (IsDepressurizing ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working), num2);
			}
			return false;
		}

		private void SetDepressurizing()
		{
			StopVentEffect();
			if (IsDepressurizing)
			{
				MyDefinitionId resourceType = m_oxygenGasId;
				base.ResourceSink.RemoveType(ref resourceType);
			}
			else
			{
				base.ResourceSink.AddType(ref OxygenSinkInfo);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			SourceComp.SetProductionEnabledByType(m_oxygenGasId, IsDepressurizing);
			base.ResourceSink.Update();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			if (!MySession.Static.Settings.EnableOxygen || !MySession.Static.Settings.EnableOxygenPressurization)
			{
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.Oxygen_Disabled));
				return;
			}
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			bool flag = oxygenBlock != null && oxygenBlock.Room != null;
			bool flag2 = flag && oxygenBlock.Room.IsAirtight;
			float num = flag ? oxygenBlock.OxygenLevel(base.CubeGrid.GridSize) : 0f;
			if (!Sync.IsServer)
			{
				flag2 = m_blockRoomInfo.Value.IsRoomAirtight;
				num = m_blockRoomInfo.Value.OxygenLevel;
				flag = true;
			}
			if (!flag || !flag2)
			{
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.Oxygen_NotPressurized));
			}
			else
			{
				detailedInfo.Append(string.Concat(MyTexts.Get(MySpaceTexts.Oxygen_Pressure), (num * 100f).ToString("F"), "%"));
			}
		}

		protected override void Closing()
		{
			base.Closing();
			StopVentEffect();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		private MyOxygenBlock GetOxygenBlock()
		{
			if (!MySession.Static.Settings.EnableOxygen || !MySession.Static.Settings.EnableOxygenPressurization || VentDummy == null || base.CubeGrid.GridSystems.GasSystem == null)
			{
				return new MyOxygenBlock();
			}
			MatrixD matrixD = MatrixD.Multiply(MatrixD.Normalize(VentDummy.Matrix), base.WorldMatrix);
			return base.CubeGrid.GridSystems.GasSystem.GetOxygenBlock(matrixD.Translation);
		}

		bool IMyGasBlock.IsWorking()
		{
			return CanVentToRoom;
		}

		private void VentToRoom(float amount)
		{
			if (amount == 0f || IsDepressurizing)
			{
				return;
			}
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (oxygenBlock != null && oxygenBlock.Room != null && oxygenBlock.Room.IsAirtight)
			{
				oxygenBlock.Room.OxygenAmount += amount;
				if (oxygenBlock.Room.OxygenLevel(base.CubeGrid.GridSize) > 1f)
				{
					oxygenBlock.Room.OxygenAmount = oxygenBlock.Room.MaxOxygen(base.CubeGrid.GridSize);
				}
				base.ResourceSink.Update();
				SourceComp.SetRemainingCapacityByType(m_oxygenGasId, oxygenBlock.Room.OxygenAmount);
				CheckForVentEffect(amount);
			}
		}

		private void DrainFromRoom(float amount)
		{
			if (amount == 0f || !IsDepressurizing)
			{
				return;
			}
			MyOxygenBlock oxygenBlock = GetOxygenBlock();
			if (oxygenBlock == null || oxygenBlock.Room == null)
			{
				return;
			}
			_ = oxygenBlock.Room.OxygenAmount;
			if (oxygenBlock.Room.IsAirtight)
			{
				oxygenBlock.Room.OxygenAmount -= amount;
				if (oxygenBlock.Room.OxygenAmount < 0f)
				{
					oxygenBlock.Room.OxygenAmount = 0f;
				}
				SourceComp.SetRemainingCapacityByType(m_oxygenGasId, oxygenBlock.Room.OxygenAmount);
			}
			else
			{
				float newRemainingCapacity = (MyOxygenProviderSystem.GetOxygenInPoint(base.WorldMatrix.Translation) != 0f) ? (BlockDefinition.VentilationCapacityPerSecond * 100f) : 0f;
				SourceComp.SetRemainingCapacityByType(m_oxygenGasId, newRemainingCapacity);
				m_producedSinceLastUpdate = true;
			}
			base.ResourceSink.Update();
			CheckForVentEffect(amount);
		}

		private void CheckForVentEffect(float amount)
		{
			if (amount > 0f)
			{
				m_producedSinceLastUpdate = true;
				if (amount > 1f && !m_isPlayingVentEffect)
				{
					CreateEffect();
				}
			}
		}

		/// <summary>
		/// Compatibility method
		/// </summary>
		public bool IsPressurized()
		{
			return CanPressurize;
		}

		public float GetOxygenLevel()
		{
			if (base.IsWorking)
			{
				MyOxygenBlock oxygenBlock = GetOxygenBlock();
				if (oxygenBlock != null && oxygenBlock.Room != null)
				{
					float result = oxygenBlock.OxygenLevel(base.CubeGrid.GridSize);
					if (oxygenBlock.Room.IsAirtight)
					{
						return result;
					}
					return oxygenBlock.Room.EnvironmentOxygen;
				}
			}
			return 0f;
		}

		[Event(null, 981)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem myToolbarItem = null;
			if (sentItem.EntityID != 0L)
			{
				if (string.IsNullOrEmpty(sentItem.GroupName))
				{
					if (MyEntities.TryGetEntityById(sentItem.EntityID, out MyTerminalBlock entity))
					{
						MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
						myObjectBuilder_ToolbarItemTerminalBlock._Action = sentItem.Action;
						myObjectBuilder_ToolbarItemTerminalBlock.Parameters = sentItem.Parameters;
						myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalBlock);
					}
				}
				else
				{
					MyCubeGrid cubeGrid = base.CubeGrid;
					string groupName = sentItem.GroupName;
					MyBlockGroup myBlockGroup = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((MyBlockGroup x) => x.Name.ToString() == groupName);
					if (myBlockGroup != null)
					{
						MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(myBlockGroup);
						myObjectBuilder_ToolbarItemTerminalGroup._Action = sentItem.Action;
						myObjectBuilder_ToolbarItemTerminalGroup.BlockEntityId = sentItem.EntityID;
						myObjectBuilder_ToolbarItemTerminalGroup.Parameters = sentItem.Parameters;
						myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItemTerminalGroup);
					}
				}
			}
			if (index == 0)
			{
				m_onFullAction = myToolbarItem;
			}
			else
			{
				m_onEmptyAction = myToolbarItem;
			}
			RaisePropertiesChanged();
			m_syncing = false;
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
