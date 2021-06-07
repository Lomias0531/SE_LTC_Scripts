using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
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
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TimerBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyTimerBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock)
	})]
	public class MyTimerBlock : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyTimerBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock, IMyTriggerableBlock
	{
		protected sealed class Stop_003C_003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTimerBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.Stop();
			}
		}

		protected sealed class Start_003C_003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTimerBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.Start();
			}
		}

		protected sealed class Trigger_003C_003E : ICallSite<MyTimerBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTimerBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.Trigger();
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyTimerBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTimerBlock @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected class m_silent_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType silent;
				ISyncType result = silent = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyTimerBlock)P_0).m_silent = (Sync<bool, SyncDirection.BothWays>)silent;
				return result;
			}
		}

		protected class m_timerSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType timerSync;
				ISyncType result = timerSync = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyTimerBlock)P_0).m_timerSync = (Sync<float, SyncDirection.BothWays>)timerSync;
				return result;
			}
		}

		private int m_countdownMsCurrent;

		private int m_countdownMsStart;

		private MySoundPair m_beepStart = MySoundPair.Empty;

		private MySoundPair m_beepMid = MySoundPair.Empty;

		private MySoundPair m_beepEnd = MySoundPair.Empty;

		private MyEntity3DSoundEmitter m_beepEmitter;

		private readonly Sync<bool, SyncDirection.BothWays> m_silent;

		private static List<MyToolbar> m_openedToolbars;

		private static bool m_shouldSetOtherToolbars;

		private bool m_syncing;

		private readonly Sync<float, SyncDirection.BothWays> m_timerSync;

		public MyToolbar Toolbar
		{
			get;
			set;
		}

		public bool IsCountingDown
		{
			get;
			set;
		}

		public bool Silent
		{
			get
			{
				return m_silent;
			}
			private set
			{
				m_silent.Value = value;
			}
		}

		public float TriggerDelay
		{
			get
			{
				return m_timerSync.Value / 1000f;
			}
			set
			{
				m_timerSync.Value = value * 1000f;
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.IsCountingDown => IsCountingDown;

		float SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.TriggerDelay
		{
			get
			{
				return TriggerDelay;
			}
			set
			{
				TriggerDelay = value;
			}
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.Silent
		{
			get
			{
				return Silent;
			}
			set
			{
				Silent = value;
			}
		}

		public MyTimerBlock()
		{
			CreateTerminalControls();
			m_openedToolbars = new List<MyToolbar>();
			m_timerSync.ValueChanged += delegate
			{
				TimerChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyTimerBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyTimerBlock> obj = new MyTerminalControlCheckbox<MyTimerBlock>("Silent", MySpaceTexts.BlockPropertyTitle_Silent, MySpaceTexts.ToolTipTimerBlock_Silent)
				{
					Getter = ((MyTimerBlock x) => x.Silent),
					Setter = delegate(MyTimerBlock x, bool v)
					{
						x.Silent = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlSlider<MyTimerBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyTimerBlock>("TriggerDelay", MySpaceTexts.TerminalControlPanel_TimerDelay, MySpaceTexts.TerminalControlPanel_TimerDelay);
				myTerminalControlSlider.SetLogLimits(1f, 3600f);
				myTerminalControlSlider.DefaultValue = 10f;
				myTerminalControlSlider.Enabled = ((MyTimerBlock x) => !x.IsCountingDown);
				myTerminalControlSlider.Getter = ((MyTimerBlock x) => x.TriggerDelay);
				myTerminalControlSlider.Setter = delegate(MyTimerBlock x, float v)
				{
					x.TriggerDelay = v;
				};
				myTerminalControlSlider.Writer = delegate(MyTimerBlock x, StringBuilder sb)
				{
					MyValueFormatter.AppendTimeExact(Math.Max((int)x.TriggerDelay, 1), sb);
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTimerBlock>("OpenToolbar", MySpaceTexts.BlockPropertyTitle_TimerToolbarOpen, MySpaceTexts.BlockPropertyTitle_TimerToolbarOpen, delegate(MyTimerBlock self)
				{
					m_openedToolbars.Add(self.Toolbar);
					if (MyGuiScreenToolbarConfigBase.Static == null)
					{
						m_shouldSetOtherToolbars = true;
						MyToolbarComponent.CurrentToolbar = self.Toolbar;
						MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
						MyToolbarComponent.AutoUpdate = false;
						myGuiScreenBase.Closed += delegate
						{
							MyToolbarComponent.AutoUpdate = true;
							m_openedToolbars.Clear();
						};
						MyGuiSandbox.AddScreen(myGuiScreenBase);
					}
				}));
				MyTerminalControlButton<MyTimerBlock> myTerminalControlButton = new MyTerminalControlButton<MyTimerBlock>("TriggerNow", MySpaceTexts.BlockPropertyTitle_TimerTrigger, MySpaceTexts.BlockPropertyTitle_TimerTrigger, delegate(MyTimerBlock x)
				{
					x.OnTrigger();
				});
				myTerminalControlButton.EnableAction();
				MyTerminalControlFactory.AddControl(myTerminalControlButton);
				MyTerminalControlButton<MyTimerBlock> myTerminalControlButton2 = new MyTerminalControlButton<MyTimerBlock>("Start", MySpaceTexts.BlockPropertyTitle_TimerStart, MySpaceTexts.BlockPropertyTitle_TimerStart, delegate(MyTimerBlock x)
				{
					x.StartBtn();
				});
				myTerminalControlButton2.EnableAction();
				MyTerminalControlFactory.AddControl(myTerminalControlButton2);
				MyTerminalControlButton<MyTimerBlock> myTerminalControlButton3 = new MyTerminalControlButton<MyTimerBlock>("Stop", MySpaceTexts.BlockPropertyTitle_TimerStop, MySpaceTexts.BlockPropertyTitle_TimerStop, delegate(MyTimerBlock x)
				{
					x.StopBtn();
				});
				myTerminalControlButton3.EnableAction();
				MyTerminalControlFactory.AddControl(myTerminalControlButton3);
			}
		}

		private void TimerChanged()
		{
			SetTimer((int)TriggerDelay * 1000);
		}

		private void StopBtn()
		{
			MyMultiplayer.RaiseEvent(this, (MyTimerBlock x) => x.Stop);
		}

		private void StartBtn()
		{
			MyMultiplayer.RaiseEvent(this, (MyTimerBlock x) => x.Start);
		}

		[Event(null, 155)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void Stop()
		{
			IsCountingDown = false;
			base.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			ClearMemory();
			UpdateEmissivity();
		}

		private void ClearMemory()
		{
			m_countdownMsCurrent = 0;
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		[Event(null, 171)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void Start()
		{
			IsCountingDown = true;
			m_countdownMsCurrent = m_countdownMsStart;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			if (m_beepEmitter != null && !Silent)
			{
				m_beepEmitter.PlaySound(m_beepStart);
			}
			UpdateEmissivity();
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index)
		{
			if (!m_syncing)
			{
				ToolbarItem arg = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
				MyMultiplayer.RaiseEvent(this, (MyTimerBlock x) => x.SendToolbarItemChanged, arg, index.ItemIndex);
				if (m_shouldSetOtherToolbars)
				{
					m_shouldSetOtherToolbars = false;
					foreach (MyToolbar openedToolbar in m_openedToolbars)
					{
						if (openedToolbar != self)
						{
							openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
						}
					}
					m_shouldSetOtherToolbars = true;
				}
			}
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			if (m_countdownMsCurrent != 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyTimerBlockDefinition myTimerBlockDefinition = base.BlockDefinition as MyTimerBlockDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(myTimerBlockDefinition.ResourceSinkGroup, 1E-07f, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			if (myTimerBlockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
			{
				myTimerBlockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("Timer");
			}
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_TimerBlock myObjectBuilder_TimerBlock = objectBuilder as MyObjectBuilder_TimerBlock;
			Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, 9, 10);
			Toolbar.Init(myObjectBuilder_TimerBlock.Toolbar, this);
			Toolbar.ItemChanged += Toolbar_ItemChanged;
			if (myObjectBuilder_TimerBlock.JustTriggered)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			IsCountingDown = myObjectBuilder_TimerBlock.IsCountingDown;
			if (Sync.IsServer)
			{
				Silent = myObjectBuilder_TimerBlock.Silent;
				TriggerDelay = MathHelper.Clamp(myObjectBuilder_TimerBlock.Delay, myTimerBlockDefinition.MinDelay, myTimerBlockDefinition.MaxDelay) / 1000;
			}
			m_countdownMsStart = MathHelper.Clamp(myObjectBuilder_TimerBlock.Delay, myTimerBlockDefinition.MinDelay, myTimerBlockDefinition.MaxDelay);
			m_countdownMsCurrent = MathHelper.Clamp(myObjectBuilder_TimerBlock.CurrentTime, 0, myTimerBlockDefinition.MaxDelay);
			if (IsCountingDown)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			m_beepStart = new MySoundPair(myTimerBlockDefinition.TimerSoundStart);
			m_beepMid = new MySoundPair(myTimerBlockDefinition.TimerSoundMid);
			m_beepEnd = new MySoundPair(myTimerBlockDefinition.TimerSoundEnd);
			m_beepEmitter = new MyEntity3DSoundEmitter(this);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TimerBlock obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_TimerBlock;
			obj.Toolbar = Toolbar.GetObjectBuilder();
			obj.JustTriggered = base.NeedsUpdate.HasFlag(MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			obj.Delay = m_countdownMsStart;
			obj.CurrentTime = m_countdownMsCurrent;
			obj.IsCountingDown = IsCountingDown;
			obj.Silent = Silent;
			return obj;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			IsCountingDown = false;
			if (Sync.IsServer)
			{
				for (int i = 0; i < Toolbar.ItemCount; i++)
				{
					Toolbar.UpdateItem(i);
					Toolbar.ActivateItemAtIndex(i);
				}
				if (base.CubeGrid.Physics != null && MyVisualScriptLogicProvider.TimerBlockTriggered != null)
				{
					MyVisualScriptLogicProvider.TimerBlockTriggered(base.CustomName.ToString());
				}
				if (base.CubeGrid.Physics != null && !string.IsNullOrEmpty(Name) && MyVisualScriptLogicProvider.TimerBlockTriggeredEntityName != null)
				{
					MyVisualScriptLogicProvider.TimerBlockTriggeredEntityName(Name);
				}
			}
			UpdateEmissivity();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public void SetTimer(int p)
		{
			m_countdownMsStart = p;
			RaisePropertiesChanged();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (!base.IsWorking)
			{
				return;
			}
			int num = m_countdownMsCurrent % 1000;
			if (m_countdownMsCurrent > 0)
			{
				m_countdownMsCurrent -= 166;
			}
			int num2 = m_countdownMsCurrent % 1000;
			if ((num > 800 && num2 <= 800) || (num <= 800 && num2 > 800))
			{
				UpdateEmissivity();
			}
			if (m_countdownMsCurrent <= 0)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_countdownMsCurrent = 0;
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				if (m_beepEmitter != null && !Silent)
				{
					m_beepEmitter.PlaySound(m_beepEnd, stopPrevious: true);
				}
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_TimerToTrigger));
			MyValueFormatter.AppendTimeExact(m_countdownMsCurrent / 1000, detailedInfo);
		}

		public override void UpdateSoundEmitters()
		{
			base.UpdateSoundEmitters();
			if (m_beepEmitter != null)
			{
				m_beepEmitter.Update();
			}
		}

		public void StopCountdown()
		{
			base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
			IsCountingDown = false;
			ClearMemory();
		}

		protected void OnTrigger()
		{
			if (base.IsWorking)
			{
				MyMultiplayer.RaiseEvent(this, (MyTimerBlock x) => x.Trigger);
			}
		}

		[Event(null, 383)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		protected void Trigger()
		{
			if (base.IsWorking)
			{
				StopCountdown();
				UpdateEmissivity();
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		void IMyTriggerableBlock.Trigger()
		{
			OnTrigger();
		}

		public override bool SetEmissiveStateWorking()
		{
			return UpdateEmissivity();
		}

		private bool UpdateEmissivity()
		{
			if (!base.InScene)
			{
				return false;
			}
			if (base.IsWorking)
			{
				if (IsCountingDown)
				{
					if (m_countdownMsCurrent % 1000 > 800)
					{
						if (m_beepEmitter != null && !Silent)
						{
							m_beepEmitter.PlaySound(m_beepMid);
						}
						return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
					}
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.Trigger()
		{
			OnTrigger();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.StartCountdown()
		{
			StartBtn();
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyTimerBlock.StopCountdown()
		{
			StopBtn();
		}

		[Event(null, 480)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem.EntityID != 0L)
			{
				item = ToolbarItem.ToItem(sentItem);
			}
			Toolbar.SetItemAtIndex(index, item);
			m_syncing = false;
		}
	}
}
