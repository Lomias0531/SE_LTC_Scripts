using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_RadioAntenna))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyRadioAntenna),
		typeof(Sandbox.ModAPI.Ingame.IMyRadioAntenna)
	})]
	public class MyRadioAntenna : MyFunctionalBlock, IMyGizmoDrawableObject, Sandbox.ModAPI.IMyRadioAntenna, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyRadioAntenna
	{
		protected sealed class SetHudTextEvent_003C_003ESystem_String : ICallSite<MyRadioAntenna, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyRadioAntenna @this, in string text, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetHudTextEvent(text);
			}
		}

		protected class m_ignoreOtherBroadcast_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType ignoreOtherBroadcast;
				ISyncType result = ignoreOtherBroadcast = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRadioAntenna)P_0).m_ignoreOtherBroadcast = (Sync<bool, SyncDirection.BothWays>)ignoreOtherBroadcast;
				return result;
			}
		}

		protected class m_ignoreAlliedBroadcast_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType ignoreAlliedBroadcast;
				ISyncType result = ignoreAlliedBroadcast = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRadioAntenna)P_0).m_ignoreAlliedBroadcast = (Sync<bool, SyncDirection.BothWays>)ignoreAlliedBroadcast;
				return result;
			}
		}

		protected class m_radius_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType radius;
				ISyncType result = radius = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyRadioAntenna)P_0).m_radius = (Sync<float, SyncDirection.BothWays>)radius;
				return result;
			}
		}

		protected class EnableBroadcasting_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enableBroadcasting;
				ISyncType result = enableBroadcasting = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRadioAntenna)P_0).EnableBroadcasting = (Sync<bool, SyncDirection.BothWays>)enableBroadcasting;
				return result;
			}
		}

		protected class m_showShipName_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showShipName;
				ISyncType result = showShipName = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyRadioAntenna)P_0).m_showShipName = (Sync<bool, SyncDirection.BothWays>)showShipName;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyRadioAntenna_003C_003EActor : IActivator, IActivator<MyRadioAntenna>
		{
			private sealed override object CreateInstance()
			{
				return new MyRadioAntenna();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRadioAntenna CreateInstance()
			{
				return new MyRadioAntenna();
			}

			MyRadioAntenna IActivator<MyRadioAntenna>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected Color m_gizmoColor;

		protected const float m_maxGizmoDrawDistance = 10000f;

		private MyTuple<string, MyTransmitTarget>? m_nextBroadcast;

		private Sync<bool, SyncDirection.BothWays> m_ignoreOtherBroadcast;

		private Sync<bool, SyncDirection.BothWays> m_ignoreAlliedBroadcast;

		private static MyTerminalControlCheckbox<MyRadioAntenna> m_ignoreOtherCheckbox;

		private static MyTerminalControlCheckbox<MyRadioAntenna> m_ignoreAllyCheckbox;

		private readonly Sync<float, SyncDirection.BothWays> m_radius;

		private bool onceUpdated;

		public readonly Sync<bool, SyncDirection.BothWays> EnableBroadcasting;

		private Sync<bool, SyncDirection.BothWays> m_showShipName;

		private MyRadioBroadcaster RadioBroadcaster
		{
			get
			{
				return (MyRadioBroadcaster)base.Components.Get<MyDataBroadcaster>();
			}
			set
			{
				base.Components.Add((MyDataBroadcaster)value);
			}
		}

		private MyRadioReceiver RadioReceiver
		{
			get
			{
				return (MyRadioReceiver)base.Components.Get<MyDataReceiver>();
			}
			set
			{
				base.Components.Add((MyDataReceiver)value);
			}
		}

		public bool ShowShipName
		{
			get
			{
				return m_showShipName;
			}
			set
			{
				m_showShipName.Value = value;
			}
		}

		public StringBuilder HudText
		{
			get;
			private set;
		}

		protected override bool CanShowOnHud => false;

		float Sandbox.ModAPI.Ingame.IMyRadioAntenna.Radius
		{
			get
			{
				return GetRadius();
			}
			set
			{
				m_radius.Value = MathHelper.Clamp(value, 0f, ((MyRadioAntennaDefinition)base.BlockDefinition).MaxBroadcastRadius);
			}
		}

		string Sandbox.ModAPI.Ingame.IMyRadioAntenna.HudText
		{
			get
			{
				return HudText.ToString();
			}
			set
			{
				SetHudText(value);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyRadioAntenna.IsBroadcasting => IsBroadcasting();

		bool Sandbox.ModAPI.Ingame.IMyRadioAntenna.EnableBroadcasting
		{
			get
			{
				return EnableBroadcasting.Value;
			}
			set
			{
				EnableBroadcasting.Value = value;
			}
		}

		private static event Action<MyRadioAntenna, string, MyTransmitTarget> m_messageRequest;

		private void SetHudText(StringBuilder text)
		{
			SetHudText(text.ToString());
		}

		private void SetHudText(string text)
		{
			if (HudText.CompareUpdate(text))
			{
				MyMultiplayer.RaiseEvent(this, (MyRadioAntenna x) => x.SetHudTextEvent, text);
			}
		}

		[Event(null, 101)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		protected void SetHudTextEvent(string text)
		{
			HudText.CompareUpdate(text);
		}

		public Color GetGizmoColor()
		{
			return m_gizmoColor;
		}

		public Vector3 GetPositionInGrid()
		{
			return base.Position;
		}

		public bool CanBeDrawn()
		{
			if (!MyCubeGrid.ShowAntennaGizmos || !base.IsWorking || !HasLocalPlayerAccess() || GetDistanceBetweenCameraAndBoundingSphere() > 10000.0)
			{
				return false;
			}
			return IsRecievedByPlayer(this);
		}

		public BoundingBox? GetBoundingBox()
		{
			return null;
		}

		public float GetRadius()
		{
			return RadioBroadcaster.BroadcastRadius;
		}

		public MatrixD GetWorldMatrix()
		{
			return base.PositionComp.WorldMatrix;
		}

		public bool EnableLongDrawDistance()
		{
			return true;
		}

		public static bool IsRecievedByPlayer(MyCubeBlock cubeBlock)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return false;
			}
			MyRadioReceiver radioReceiver = localCharacter.RadioReceiver;
			return MyAntennaSystem.Static.CheckConnection(radioReceiver, cubeBlock, localCharacter.GetPlayerIdentityId(), mutual: false);
		}

		public MyRadioAntenna()
		{
			CreateTerminalControls();
			HudText = new StringBuilder();
			base.NeedsWorldMatrix = true;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyRadioAntenna>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.RemoveBaseClass<MyRadioAntenna, MyTerminalBlock>();
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyRadioAntenna>("ShowInTerminal", MySpaceTexts.Terminal_ShowInTerminal, MySpaceTexts.Terminal_ShowInTerminalToolTip)
				{
					Getter = ((MyRadioAntenna x) => x.ShowInTerminal),
					Setter = delegate(MyRadioAntenna x, bool v)
					{
						x.ShowInTerminal = v;
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyRadioAntenna>("ShowInToolbarConfig", MySpaceTexts.Terminal_ShowInToolbarConfig, MySpaceTexts.Terminal_ShowInToolbarConfigToolTip)
				{
					Getter = ((MyRadioAntenna x) => x.ShowInToolbarConfig),
					Setter = delegate(MyRadioAntenna x, bool v)
					{
						x.ShowInToolbarConfig = v;
					}
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyRadioAntenna>("CustomData", MySpaceTexts.Terminal_CustomData, MySpaceTexts.Terminal_CustomDataTooltip, base.CustomDataClicked)
				{
					Enabled = ((MyRadioAntenna x) => !x.m_textboxOpen),
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyRadioAntenna>("CustomName", MyCommonTexts.Name, MySpaceTexts.Blank)
				{
					Getter = ((MyRadioAntenna x) => x.CustomName),
					Setter = delegate(MyRadioAntenna x, StringBuilder v)
					{
						x.SetCustomName(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyRadioAntenna>());
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyRadioAntenna>("HudText", MySpaceTexts.BlockPropertiesTitle_HudText, MySpaceTexts.Antenna_HudTextToolTip)
				{
					Getter = ((MyRadioAntenna x) => x.HudText),
					Setter = delegate(MyRadioAntenna x, StringBuilder v)
					{
						x.SetHudText(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyRadioAntenna>());
				MyTerminalControlSlider<MyRadioAntenna> myTerminalControlSlider = new MyTerminalControlSlider<MyRadioAntenna>("Radius", MySpaceTexts.BlockPropertyTitle_BroadcastRadius, MySpaceTexts.BlockPropertyDescription_BroadcastRadius);
				myTerminalControlSlider.SetLogLimits((MyRadioAntenna block) => 1f, (MyRadioAntenna block) => (block.BlockDefinition as MyRadioAntennaDefinition).MaxBroadcastRadius);
				myTerminalControlSlider.DefaultValueGetter = ((MyRadioAntenna x) => (x.BlockDefinition as MyRadioAntennaDefinition).MaxBroadcastRadius / 10f);
				myTerminalControlSlider.Getter = ((MyRadioAntenna x) => x.RadioBroadcaster.BroadcastRadius);
				myTerminalControlSlider.Setter = delegate(MyRadioAntenna x, float v)
				{
					x.m_radius.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyRadioAntenna x, StringBuilder result)
				{
					result.Append((object)new StringBuilder().AppendDecimal(x.RadioBroadcaster.BroadcastRadius, 0).Append(" m"));
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlCheckbox<MyRadioAntenna> obj = new MyTerminalControlCheckbox<MyRadioAntenna>("EnableBroadCast", MySpaceTexts.Antenna_EnableBroadcast, MySpaceTexts.Antenna_EnableBroadcast)
				{
					Getter = ((MyRadioAntenna x) => x.EnableBroadcasting.Value),
					Setter = delegate(MyRadioAntenna x, bool v)
					{
						x.EnableBroadcasting.Value = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlCheckbox<MyRadioAntenna> obj2 = new MyTerminalControlCheckbox<MyRadioAntenna>("ShowShipName", MySpaceTexts.BlockPropertyTitle_ShowShipName, MySpaceTexts.BlockPropertyDescription_ShowShipName)
				{
					Getter = ((MyRadioAntenna x) => x.ShowShipName),
					Setter = delegate(MyRadioAntenna x, bool v)
					{
						x.ShowShipName = v;
					}
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyRadioAntenna>());
			}
		}

		private void ChangeRadius()
		{
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.BroadcastRadius = m_radius;
				RadioBroadcaster.RaiseBroadcastRadiusChanged();
			}
		}

		private void ChangeEnableBroadcast()
		{
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.Enabled = ((bool)EnableBroadcasting && base.IsWorking);
				RadioBroadcaster.WantsToBeEnabled = EnableBroadcasting;
				base.ResourceSink.Update();
				RaisePropertiesChanged();
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			}
		}

		private void OnShowShipNameChanged()
		{
			RadioBroadcaster.RaiseAntennaNameChanged(this);
			if ((bool)m_showShipName)
			{
				base.CubeGrid.OnNameChanged += OnShipNameChanged;
			}
			else
			{
				base.CubeGrid.OnNameChanged -= OnShipNameChanged;
			}
		}

		private void OnShipNameChanged(MyCubeGrid grid)
		{
			RadioBroadcaster.RaiseAntennaNameChanged(this);
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			if ((bool)m_showShipName)
			{
				oldGrid.OnNameChanged -= OnShipNameChanged;
				base.CubeGrid.OnNameChanged += OnShipNameChanged;
				RadioBroadcaster.RaiseAntennaNameChanged(this);
			}
			base.OnCubeGridChanged(oldGrid);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			RadioBroadcaster = new MyRadioBroadcaster();
			RadioReceiver = new MyRadioReceiver();
			MyRadioAntennaDefinition myRadioAntennaDefinition = base.BlockDefinition as MyRadioAntennaDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(myRadioAntennaDefinition.ResourceSinkGroup, 0.002f, UpdatePowerInput);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.IsWorkingChanged += OnIsWorkingChanged;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_RadioAntenna myObjectBuilder_RadioAntenna = (MyObjectBuilder_RadioAntenna)objectBuilder;
			if (myObjectBuilder_RadioAntenna.BroadcastRadius > 0f)
			{
				RadioBroadcaster.BroadcastRadius = myObjectBuilder_RadioAntenna.BroadcastRadius;
			}
			else
			{
				RadioBroadcaster.BroadcastRadius = myRadioAntennaDefinition.MaxBroadcastRadius / 10f;
			}
			HudText.Clear();
			if (myObjectBuilder_RadioAntenna.HudText != null)
			{
				HudText.Append(myObjectBuilder_RadioAntenna.HudText);
			}
			RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(RadioBroadcaster.BroadcastRadius, 1f, myRadioAntennaDefinition.MaxBroadcastRadius);
			base.ResourceSink.Update();
			RadioBroadcaster.WantsToBeEnabled = myObjectBuilder_RadioAntenna.EnableBroadcasting;
			m_showShipName.SetLocalValue(myObjectBuilder_RadioAntenna.ShowShipName);
			m_ignoreOtherBroadcast.SetLocalValue(myObjectBuilder_RadioAntenna.IgnoreOther);
			m_ignoreAlliedBroadcast.SetLocalValue(myObjectBuilder_RadioAntenna.IgnoreAllied);
			base.ShowOnHUD = false;
			m_gizmoColor = new Vector4(0.2f, 0.2f, 0f, 0.5f);
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			m_radius.ValueChanged += delegate
			{
				ChangeRadius();
			};
			EnableBroadcasting.ValueChanged += delegate
			{
				ChangeEnableBroadcast();
			};
			m_showShipName.ValueChanged += delegate
			{
				OnShowShipNameChanged();
			};
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (Sync.IsServer)
			{
				EnableBroadcasting.Value = RadioBroadcaster.WantsToBeEnabled;
			}
			MyRadioBroadcaster radioBroadcaster = RadioBroadcaster;
			radioBroadcaster.OnBroadcastRadiusChanged = (Action)Delegate.Combine(radioBroadcaster.OnBroadcastRadiusChanged, new Action(OnBroadcastRadiusChanged));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if ((bool)m_showShipName)
			{
				base.CubeGrid.OnNameChanged += OnShipNameChanged;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			MyRadioBroadcaster radioBroadcaster = RadioBroadcaster;
			radioBroadcaster.OnBroadcastRadiusChanged = (Action)Delegate.Remove(radioBroadcaster.OnBroadcastRadiusChanged, new Action(OnBroadcastRadiusChanged));
			SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
			if ((bool)m_showShipName)
			{
				base.CubeGrid.OnNameChanged -= OnShipNameChanged;
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (Sync.IsServer && !onceUpdated)
			{
				base.IsWorkingChanged += UpdatePirateAntenna;
				base.CustomNameChanged += UpdatePirateAntenna;
				base.OwnershipChanged += UpdatePirateAntenna;
				UpdatePirateAntenna(this);
			}
			onceUpdated = true;
			if (m_nextBroadcast.HasValue)
			{
				MyRadioAntenna.m_messageRequest?.Invoke(this, m_nextBroadcast.Value.Item1, m_nextBroadcast.Value.Item2);
				m_nextBroadcast = null;
			}
		}

		protected override void Closing()
		{
			if (Sync.IsServer)
			{
				UpdatePirateAntenna(remove: true);
			}
			base.Closing();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_RadioAntenna obj = (MyObjectBuilder_RadioAntenna)base.GetObjectBuilderCubeBlock(copy);
			obj.BroadcastRadius = RadioBroadcaster.BroadcastRadius;
			obj.ShowShipName = ShowShipName;
			obj.EnableBroadcasting = RadioBroadcaster.WantsToBeEnabled;
			obj.HudText = HudText.ToString();
			obj.IgnoreAllied = m_ignoreAlliedBroadcast.Value;
			obj.IgnoreOther = m_ignoreOtherBroadcast.Value;
			return obj;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			RadioReceiver.UpdateBroadcastersInRange();
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.MoveBroadcaster();
			}
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			m_hudParams.Clear();
			if (base.CubeGrid == null || base.CubeGrid.MarkedForClose || base.CubeGrid.Closed)
			{
				return m_hudParams;
			}
			if (base.IsWorking)
			{
				List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink && HasLocalPlayerAccess());
				StringBuilder hudText = HudText;
				if (ShowShipName || hudText.Length > 0)
				{
					StringBuilder text = hudParams[0].Text;
					text.Clear();
					if (!string.IsNullOrEmpty(GetOwnerFactionTag()))
					{
						text.Append(GetOwnerFactionTag());
						text.Append(".");
					}
					if (ShowShipName)
					{
						text.Append(base.CubeGrid.DisplayName);
						text.Append(" - ");
					}
					text.Append((object)hudText);
				}
				m_hudParams.AddRange(hudParams);
				if (HasLocalPlayerAccess() && SlimBlock.CubeGrid.GridSystems.TerminalSystem != null)
				{
					SlimBlock.CubeGrid.GridSystems.TerminalSystem.NeedsHudUpdate = true;
					foreach (MyTerminalBlock hudBlock in SlimBlock.CubeGrid.GridSystems.TerminalSystem.HudBlocks)
					{
						if (hudBlock != this)
						{
							m_hudParams.AddRange(hudBlock.GetHudParams(allowBlink: true));
						}
					}
				}
				MyEntityController entityController = MySession.Static.Players.GetEntityController(base.CubeGrid);
				if (entityController != null)
				{
					MyCockpit myCockpit = entityController.ControlledEntity as MyCockpit;
					if (myCockpit != null && myCockpit.Pilot != null)
					{
						m_hudParams.AddRange(myCockpit.GetHudParams(allowBlink: true));
					}
				}
			}
			return m_hudParams;
		}

		private void UpdatePirateAntenna(MyCubeBlock obj)
		{
			UpdatePirateAntenna();
		}

		public void UpdatePirateAntenna(bool remove = false)
		{
			bool activeState = base.IsWorking && Sync.Players.GetNPCIdentities().Contains(base.OwnerId);
			MyPirateAntennas.UpdatePirateAntenna(base.EntityId, remove, activeState, (HudText.Length > 0) ? HudText : base.CustomName);
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			UpdateEnabled();
		}

		protected void OnIsWorkingChanged(MyCubeBlock block)
		{
			UpdateEnabled();
		}

		protected void UpdateEnabled()
		{
			base.ResourceSink.Update();
			if (onceUpdated)
			{
				RadioReceiver.Enabled = base.IsWorking;
				RadioBroadcaster.Enabled = ((bool)EnableBroadcasting && base.IsWorking);
				RadioBroadcaster.WantsToBeEnabled = EnableBroadcasting;
				RadioReceiver.UpdateBroadcastersInRange();
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			if (RadioBroadcaster != null)
			{
				if (base.IsWorking)
				{
					RadioBroadcaster.Enabled = EnableBroadcasting;
				}
				else
				{
					RadioBroadcaster.Enabled = false;
				}
			}
			if (RadioReceiver != null)
			{
				RadioReceiver.Enabled = base.IsWorking;
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void OnBroadcastRadiusChanged()
		{
			base.ResourceSink.Update();
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private float UpdatePowerInput()
		{
			float num = (EnableBroadcasting ? RadioBroadcaster.BroadcastRadius : 1f) / 500f;
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return num * 0.002f;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
		}

		private bool IsBroadcasting()
		{
			if (RadioBroadcaster == null)
			{
				return false;
			}
			return RadioBroadcaster.WantsToBeEnabled;
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			RadioBroadcaster.RaiseOwnerChanged();
		}
	}
}
