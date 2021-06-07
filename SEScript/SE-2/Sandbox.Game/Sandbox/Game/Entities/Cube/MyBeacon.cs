using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Graphics;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Beacon))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyBeacon),
		typeof(Sandbox.ModAPI.Ingame.IMyBeacon)
	})]
	public class MyBeacon : MyFunctionalBlock, Sandbox.ModAPI.IMyBeacon, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyBeacon
	{
		protected sealed class SetHudTextEvent_003C_003ESystem_String : ICallSite<MyBeacon, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyBeacon @this, in string text, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetHudTextEvent(text);
			}
		}

		protected class m_radius_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType radius;
				ISyncType result = radius = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyBeacon)P_0).m_radius = (Sync<float, SyncDirection.BothWays>)radius;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyBeacon_003C_003EActor : IActivator, IActivator<MyBeacon>
		{
			private sealed override object CreateInstance()
			{
				return new MyBeacon();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBeacon CreateInstance()
			{
				return new MyBeacon();
			}

			MyBeacon IActivator<MyBeacon>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly Color COLOR_ON = new Color(255, 255, 128);

		private static readonly Color COLOR_OFF = new Color(30, 30, 30);

		private static readonly float POINT_LIGHT_RANGE_SMALL = 2f;

		private static readonly float POINT_LIGHT_RANGE_LARGE = 7.5f;

		private static readonly float POINT_LIGHT_INTENSITY_SMALL = 1f;

		private static readonly float POINT_LIGHT_INTENSITY_LARGE = 1f;

		private static readonly float GLARE_MAX_DISTANCE = 10000f;

		private const float LIGHT_TURNING_ON_TIME_IN_SECONDS = 0.5f;

		private bool m_largeLight;

		private MyLight m_light;

		private Vector3 m_lightPositionOffset;

		private float m_currentLightPower;

		private int m_lastAnimationUpdateTime;

		private bool m_restartTimeMeasure;

		private MyFlareDefinition m_flare;

		private readonly Sync<float, SyncDirection.BothWays> m_radius;

		private bool m_animationRunning;

		public StringBuilder HudText
		{
			get;
			private set;
		}

		protected override bool CanShowOnHud => false;

		internal MyRadioBroadcaster RadioBroadcaster
		{
			get
			{
				return (MyRadioBroadcaster)base.Components.Get<MyDataBroadcaster>();
			}
			private set
			{
				base.Components.Add((MyDataBroadcaster)value);
			}
		}

		internal MyBeaconDefinition Definition => (MyBeaconDefinition)base.BlockDefinition;

		internal bool AnimationRunning
		{
			get
			{
				return m_animationRunning;
			}
			private set
			{
				if (m_animationRunning != value)
				{
					m_animationRunning = value;
					if (value)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
						m_lastAnimationUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					}
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyBeacon.Radius
		{
			get
			{
				return RadioBroadcaster.BroadcastRadius;
			}
			set
			{
				RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(value, 0f, ((MyBeaconDefinition)base.BlockDefinition).MaxBroadcastRadius);
			}
		}

		string Sandbox.ModAPI.Ingame.IMyBeacon.HudText
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

		public MyBeacon()
		{
			CreateTerminalControls();
			HudText = new StringBuilder();
			m_radius.ValueChanged += delegate
			{
				ChangeRadius();
			};
			base.NeedsWorldMatrix = true;
		}

		private void ChangeRadius()
		{
			RadioBroadcaster.BroadcastRadius = m_radius;
			RadioBroadcaster.RaiseBroadcastRadiusChanged();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyBeacon>())
			{
				base.CreateTerminalControls();
				MyUniqueList<ITerminalControl> controls = MyTerminalControlFactory.GetList(typeof(MyBeacon)).Controls;
				controls.Remove(controls[5]);
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyBeacon>("CustomName", MyCommonTexts.Name, MySpaceTexts.Blank)
				{
					Getter = ((MyBeacon x) => x.CustomName),
					Setter = delegate(MyBeacon x, StringBuilder v)
					{
						x.SetCustomName(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyBeacon>());
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyBeacon>("HudText", MySpaceTexts.BlockPropertiesTitle_HudText, MySpaceTexts.BlockPropertiesTitle_HudText_Tooltip)
				{
					Getter = ((MyBeacon x) => x.HudText),
					Setter = delegate(MyBeacon x, StringBuilder v)
					{
						x.SetHudText(v);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlSlider<MyBeacon> myTerminalControlSlider = new MyTerminalControlSlider<MyBeacon>("Radius", MySpaceTexts.BlockPropertyTitle_BroadcastRadius, MySpaceTexts.BlockPropertyDescription_BroadcastRadius);
				myTerminalControlSlider.SetLogLimits((MyBeacon x) => 1f, (MyBeacon x) => x.Definition.MaxBroadcastRadius);
				myTerminalControlSlider.DefaultValueGetter = ((MyBeacon x) => x.Definition.MaxBroadcastRadius / 10f);
				myTerminalControlSlider.Getter = ((MyBeacon x) => x.RadioBroadcaster.BroadcastRadius);
				myTerminalControlSlider.Setter = delegate(MyBeacon x, float v)
				{
					x.m_radius.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyBeacon x, StringBuilder result)
				{
					result.AppendDecimal(x.RadioBroadcaster.BroadcastRadius, 0).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		private void SetHudText(StringBuilder text)
		{
			SetHudText(text.ToString());
		}

		private void SetHudText(string text)
		{
			if (HudText.CompareUpdate(text))
			{
				MyMultiplayer.RaiseEvent(this, (MyBeacon x) => x.SetHudTextEvent, text);
			}
		}

		[Event(null, 128)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		protected void SetHudTextEvent(string text)
		{
			HudText.CompareUpdate(text);
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			if (Definition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
			{
				Definition.EmissiveColorPreset = MyStringHash.GetOrCompute("Beacon");
			}
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute(Definition.ResourceSinkGroup), Definition.MaxBroadcastPowerDrainkW, UpdatePowerInput);
			base.ResourceSink = myResourceSinkComponent;
			RadioBroadcaster = new MyRadioBroadcaster(Definition.MaxBroadcastRadius / 10f);
			MyObjectBuilder_Beacon myObjectBuilder_Beacon = (MyObjectBuilder_Beacon)objectBuilder;
			if (myObjectBuilder_Beacon.BroadcastRadius > 0f)
			{
				RadioBroadcaster.BroadcastRadius = myObjectBuilder_Beacon.BroadcastRadius;
			}
			RadioBroadcaster.BroadcastRadius = MathHelper.Clamp(RadioBroadcaster.BroadcastRadius, 1f, Definition.MaxBroadcastRadius);
			HudText.Clear();
			if (myObjectBuilder_Beacon.HudText != null)
			{
				HudText.Append(myObjectBuilder_Beacon.HudText);
			}
			base.Init(objectBuilder, cubeGrid);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.Update();
			MyRadioBroadcaster radioBroadcaster = RadioBroadcaster;
			radioBroadcaster.OnBroadcastRadiusChanged = (Action)Delegate.Combine(radioBroadcaster.OnBroadcastRadiusChanged, new Action(OnBroadcastRadiusChanged));
			m_largeLight = (cubeGrid.GridSizeEnum == MyCubeSize.Large);
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start(DisplayNameText);
				m_light.Range = (m_largeLight ? 2f : 0.3f);
				m_light.GlareOn = false;
				m_light.GlareQuerySize = (m_largeLight ? 1.5f : 0.3f);
				m_light.GlareQueryShift = (m_largeLight ? 1f : 0.2f);
				m_light.GlareType = MyGlareTypeEnum.Normal;
				m_light.GlareMaxDistance = GLARE_MAX_DISTANCE;
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), Definition.Flare);
				m_flare = ((MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition) ?? new MyFlareDefinition());
				m_light.GlareIntensity = m_flare.Intensity;
				m_light.GlareSize = m_flare.Size;
				m_light.SubGlares = m_flare.SubGlares;
			}
			m_lightPositionOffset = (m_largeLight ? new Vector3(0f, base.CubeGrid.GridSize * 0.3f, 0f) : Vector3.Zero);
			UpdateLightPosition();
			m_restartTimeMeasure = false;
			AnimationRunning = true;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyBeacon_IsWorkingChanged;
			base.ShowOnHUD = false;
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
		}

		private void OnUpdatePower()
		{
			base.ResourceSink.Update();
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			base.IsWorkingChanged -= MyBeacon_IsWorkingChanged;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Beacon obj = (MyObjectBuilder_Beacon)base.GetObjectBuilderCubeBlock(copy);
			obj.HudText = HudText.ToString();
			obj.BroadcastRadius = RadioBroadcaster.BroadcastRadius;
			return obj;
		}

		private void MyBeacon_IsWorkingChanged(MyCubeBlock obj)
		{
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.Enabled = base.IsWorking;
			}
			if (!MyFakes.ENABLE_RADIO_HUD)
			{
				if (base.IsWorking)
				{
					MyHud.LocationMarkers.RegisterMarker(this, new MyHudEntityParams
					{
						FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL,
						Text = ((HudText.Length > 0) ? HudText : base.CustomName)
					});
				}
				else
				{
					MyHud.LocationMarkers.UnregisterMarker(this);
				}
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
				List<MyHudEntityParams> hudParams = base.GetHudParams(allowBlink);
				StringBuilder hudText = HudText;
				if (hudText.Length > 0)
				{
					StringBuilder text = hudParams[0].Text;
					text.Clear();
					if (!string.IsNullOrEmpty(GetOwnerFactionTag()))
					{
						text.Append(GetOwnerFactionTag());
						text.Append(".");
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
			}
			return m_hudParams;
		}

		private void Receiver_IsPoweredChanged()
		{
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.Enabled = base.IsWorking;
			}
			UpdatePower();
			UpdateLightProperties();
			UpdateIsWorking();
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdatePower();
			UpdateLightProperties();
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			if ((base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0)
			{
				m_restartTimeMeasure = true;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if ((base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0)
			{
				m_restartTimeMeasure = true;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.MoveBroadcaster();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateLightParent();
			UpdateLightPosition();
			UpdateLightProperties();
			UpdateEmissivity();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			UpdateLightProperties();
			UpdateEmissivity();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			float currentLightPower = m_currentLightPower;
			float num = 0f;
			if (!m_restartTimeMeasure)
			{
				num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastAnimationUpdateTime) / 1000f;
			}
			else
			{
				m_restartTimeMeasure = false;
			}
			float num2 = base.IsWorking ? 1f : (-1f);
			m_currentLightPower = MathHelper.Clamp(m_currentLightPower + num2 * num / 0.5f, 0f, 1f);
			m_lastAnimationUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_light != null)
			{
				if (m_currentLightPower <= 0f)
				{
					m_light.LightOn = false;
					m_light.GlareOn = false;
				}
				else
				{
					m_light.LightOn = true;
					m_light.GlareOn = true;
				}
				if (currentLightPower != m_currentLightPower)
				{
					UpdateLightPosition();
					UpdateLightParent();
					m_light.UpdateLight();
					UpdateEmissivity();
					UpdateLightProperties();
				}
			}
			if (m_currentLightPower == num2 * 0.5f + 0.5f)
			{
				AnimationRunning = false;
			}
			if (!AnimationRunning && base.IsFunctional && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		private void UpdateEmissivity()
		{
			Color value = COLOR_OFF;
			Color value2 = COLOR_ON;
			if (base.UsesEmissivePreset)
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out MyEmissiveColorStateResult result))
				{
					value2 = result.EmissiveColor;
				}
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					value = result.EmissiveColor;
				}
			}
			MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], m_currentLightPower, Color.Lerp(value, value2, m_currentLightPower), Color.White);
		}

		protected override void Closing()
		{
			MyLights.RemoveLight(m_light);
			MyRadioBroadcaster radioBroadcaster = RadioBroadcaster;
			radioBroadcaster.OnBroadcastRadiusChanged = (Action)Delegate.Remove(radioBroadcaster.OnBroadcastRadiusChanged, new Action(OnBroadcastRadiusChanged));
			base.Closing();
		}

		private void UpdateLightParent()
		{
			if (m_light != null)
			{
				uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
				m_light.ParentID = parentCullObject;
			}
		}

		private void UpdateLightPosition()
		{
			if (m_light != null)
			{
				MatrixD matrix = base.PositionComp.LocalMatrix;
				m_light.Position = Vector3D.Transform(m_lightPositionOffset, matrix);
				if (!AnimationRunning)
				{
					m_light.UpdateLight();
				}
			}
		}

		private void UpdatePower()
		{
			AnimationRunning = true;
		}

		private void UpdateLightProperties()
		{
			if (m_light != null)
			{
				Color color = Color.Lerp(COLOR_OFF, COLOR_ON, m_currentLightPower);
				float range = m_largeLight ? POINT_LIGHT_RANGE_LARGE : POINT_LIGHT_RANGE_SMALL;
				float intensity = m_currentLightPower * (m_largeLight ? POINT_LIGHT_INTENSITY_LARGE : POINT_LIGHT_INTENSITY_SMALL);
				m_light.Color = color;
				m_light.Range = range;
				m_light.Intensity = intensity;
				m_light.GlareIntensity = m_currentLightPower * m_flare.Intensity;
				m_light.UpdateLight();
			}
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			UpdatePower();
			base.OnEnabledChanged();
		}

		private void OnBroadcastRadiusChanged()
		{
			base.ResourceSink.Update();
			RaisePropertiesChanged();
			SetDetailedInfoDirty();
		}

		private float UpdatePowerInput()
		{
			float result = RadioBroadcaster.BroadcastRadius / Definition.MaxBroadcastRadius * Definition.MaxBroadcastPowerDrainkW / 1000f;
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return result;
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			RadioBroadcaster.RaiseOwnerChanged();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
			detailedInfo.Append("\n");
		}
	}
}
