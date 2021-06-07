using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyLightingBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyLightingBlock)
	})]
	public abstract class MyLightingBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock
	{
		protected class LightLocalData
		{
			public Matrix LocalMatrix;

			public MyEntitySubpart Subpart;
		}

		protected class m_blinkIntervalSeconds_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType blinkIntervalSeconds;
				ISyncType result = blinkIntervalSeconds = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_blinkIntervalSeconds = (Sync<float, SyncDirection.BothWays>)blinkIntervalSeconds;
				return result;
			}
		}

		protected class m_blinkLength_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType blinkLength;
				ISyncType result = blinkLength = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_blinkLength = (Sync<float, SyncDirection.BothWays>)blinkLength;
				return result;
			}
		}

		protected class m_blinkOffset_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType blinkOffset;
				ISyncType result = blinkOffset = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_blinkOffset = (Sync<float, SyncDirection.BothWays>)blinkOffset;
				return result;
			}
		}

		protected class m_intensity_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType intensity;
				ISyncType result = intensity = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_intensity = (Sync<float, SyncDirection.BothWays>)intensity;
				return result;
			}
		}

		protected class m_lightColor_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightColor;
				ISyncType result = lightColor = new Sync<Color, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_lightColor = (Sync<Color, SyncDirection.BothWays>)lightColor;
				return result;
			}
		}

		protected class m_lightRadius_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightRadius;
				ISyncType result = lightRadius = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_lightRadius = (Sync<float, SyncDirection.BothWays>)lightRadius;
				return result;
			}
		}

		protected class m_lightFalloff_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightFalloff;
				ISyncType result = lightFalloff = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_lightFalloff = (Sync<float, SyncDirection.BothWays>)lightFalloff;
				return result;
			}
		}

		protected class m_lightOffset_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lightOffset;
				ISyncType result = lightOffset = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLightingBlock)P_0).m_lightOffset = (Sync<float, SyncDirection.BothWays>)lightOffset;
				return result;
			}
		}

		private const double MIN_MOVEMENT_SQUARED_FOR_UPDATE = 0.0001;

		private const int NUM_DECIMALS = 1;

		private readonly Sync<float, SyncDirection.BothWays> m_blinkIntervalSeconds;

		private readonly Sync<float, SyncDirection.BothWays> m_blinkLength;

		private readonly Sync<float, SyncDirection.BothWays> m_blinkOffset;

		protected List<MyLight> m_lights = new List<MyLight>();

		private readonly Sync<float, SyncDirection.BothWays> m_intensity;

		private readonly Sync<Color, SyncDirection.BothWays> m_lightColor;

		private readonly Sync<float, SyncDirection.BothWays> m_lightRadius;

		private readonly Sync<float, SyncDirection.BothWays> m_lightFalloff;

		private readonly Sync<float, SyncDirection.BothWays> m_lightOffset;

		protected List<LightLocalData> m_lightLocalData = new List<LightLocalData>();

		private readonly float m_lightTurningOnSpeed = 0.05f;

		protected bool m_positionDirty = true;

		protected bool m_needsRecreateLights;

		private const int MaxLightUpdateDistance = 5000;

		private bool m_emissiveMaterialDirty;

		private Color m_bulbColor = Color.Black;

		private float m_currentLightPower;

		private bool m_blinkOn = true;

		private float m_radius;

		private float m_reflectorRadius;

		private Color m_color;

		private float m_falloff;

		public new MyLightingBlockDefinition BlockDefinition => (MyLightingBlockDefinition)base.BlockDefinition;

		public MyBounds BlinkIntervalSecondsBounds => BlockDefinition.BlinkIntervalSeconds;

		public MyBounds BlinkLenghtBounds => BlockDefinition.BlinkLenght;

		public MyBounds BlinkOffsetBounds => BlockDefinition.BlinkOffset;

		public MyBounds FalloffBounds => BlockDefinition.LightFalloff;

		public MyBounds OffsetBounds => BlockDefinition.LightOffset;

		public MyBounds RadiusBounds => BlockDefinition.LightRadius;

		public MyBounds ReflectorRadiusBounds => BlockDefinition.LightReflectorRadius;

		public MyBounds IntensityBounds => BlockDefinition.LightIntensity;

		public float ReflectorConeDegrees => BlockDefinition.ReflectorConeDegrees;

		public Vector4 LightColorDef => (IsLargeLight ? new Color(255, 255, 222) : new Color(206, 235, 255)).ToVector4();

		public bool IsLargeLight
		{
			get;
			private set;
		}

		public abstract bool IsReflector
		{
			get;
		}

		protected abstract bool SupportsFalloff
		{
			get;
		}

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				if (m_color != value)
				{
					m_color = value;
					BulbColor = ComputeBulbColor();
					UpdateEmissivity(force: true);
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		public float Radius
		{
			get
			{
				return m_radius;
			}
			set
			{
				if (m_radius != value)
				{
					m_radius = value;
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		public float ReflectorRadius
		{
			get
			{
				return m_reflectorRadius;
			}
			set
			{
				if (m_reflectorRadius != value)
				{
					m_reflectorRadius = value;
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		public float BlinkLength
		{
			get
			{
				return m_blinkLength;
			}
			set
			{
				if ((float)m_blinkLength != value)
				{
					m_blinkLength.Value = (float)Math.Round(value, 1);
					RaisePropertiesChanged();
				}
			}
		}

		public float BlinkOffset
		{
			get
			{
				return m_blinkOffset;
			}
			set
			{
				if ((float)m_blinkOffset != value)
				{
					m_blinkOffset.Value = (float)Math.Round(value, 1);
					RaisePropertiesChanged();
				}
			}
		}

		public float BlinkIntervalSeconds
		{
			get
			{
				return m_blinkIntervalSeconds;
			}
			set
			{
				if ((float)m_blinkIntervalSeconds != value)
				{
					if (value > (float)m_blinkIntervalSeconds)
					{
						m_blinkIntervalSeconds.Value = (float)Math.Round(value + 0.04999f, 1);
					}
					else
					{
						m_blinkIntervalSeconds.Value = (float)Math.Round(value - 0.04999f, 1);
					}
					if ((float)m_blinkIntervalSeconds == 0f && base.Enabled)
					{
						UpdateEnabled();
					}
					RaisePropertiesChanged();
				}
			}
		}

		public virtual float Falloff
		{
			get
			{
				return m_falloff;
			}
			set
			{
				if (m_falloff != value)
				{
					m_falloff = value;
					UpdateIntensity();
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		public float Intensity
		{
			get
			{
				return m_intensity;
			}
			set
			{
				if ((float)m_intensity != value)
				{
					m_intensity.Value = value;
					UpdateIntensity();
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		public float Offset
		{
			get
			{
				return m_lightOffset;
			}
			set
			{
				if ((float)m_lightOffset != value)
				{
					m_lightOffset.Value = value;
					UpdateLightProperties();
					RaisePropertiesChanged();
				}
			}
		}

		protected virtual bool NeedPerFrameUpdate => base.HasDamageEffect | ((float)m_blinkIntervalSeconds > 0f) | (GetNewLightPower() != CurrentLightPower);

		public float CurrentLightPower
		{
			get
			{
				return m_currentLightPower;
			}
			set
			{
				if (m_currentLightPower != value)
				{
					m_currentLightPower = value;
					m_emissiveMaterialDirty = true;
				}
			}
		}

		public Color BulbColor
		{
			get
			{
				return m_bulbColor;
			}
			set
			{
				if (m_bulbColor != value)
				{
					m_bulbColor = value;
					m_emissiveMaterialDirty = true;
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.ReflectorRadius => ReflectorRadius;

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLenght => BlinkLength;

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Radius
		{
			get
			{
				if (!IsReflector)
				{
					return Radius;
				}
				return ReflectorRadius;
			}
			set
			{
				value = (IsReflector ? MathHelper.Clamp(value, ReflectorRadiusBounds.Min, ReflectorRadiusBounds.Max) : MathHelper.Clamp(value, RadiusBounds.Min, RadiusBounds.Max));
				m_lightRadius.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Intensity
		{
			get
			{
				return m_intensity;
			}
			set
			{
				value = MathHelper.Clamp(value, IntensityBounds.Min, IntensityBounds.Max);
				m_intensity.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.Falloff
		{
			get
			{
				return m_lightFalloff;
			}
			set
			{
				value = MathHelper.Clamp(value, FalloffBounds.Min, FalloffBounds.Max);
				m_lightFalloff.Value = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkIntervalSeconds
		{
			get
			{
				return BlinkIntervalSeconds;
			}
			set
			{
				value = MathHelper.Clamp(value, BlinkIntervalSecondsBounds.Min, BlinkIntervalSecondsBounds.Max);
				BlinkIntervalSeconds = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkLength
		{
			get
			{
				return BlinkLength;
			}
			set
			{
				value = MathHelper.Clamp(value, BlinkLenghtBounds.Min, BlinkLenghtBounds.Max);
				BlinkLength = value;
			}
		}

		float Sandbox.ModAPI.Ingame.IMyLightingBlock.BlinkOffset
		{
			get
			{
				return BlinkOffset;
			}
			set
			{
				value = MathHelper.Clamp(value, BlinkOffsetBounds.Min, BlinkOffsetBounds.Max);
				BlinkOffset = value;
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyLightingBlock.Color
		{
			get
			{
				return Color;
			}
			set
			{
				m_lightColor.Value = value;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLightingBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlColor<MyLightingBlock>("Color", MySpaceTexts.BlockPropertyTitle_LightColor)
				{
					Getter = ((MyLightingBlock x) => x.Color),
					Setter = delegate(MyLightingBlock x, Color v)
					{
						x.m_lightColor.Value = v;
					}
				});
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyLightingBlock>("Radius", MySpaceTexts.BlockPropertyTitle_LightRadius, MySpaceTexts.BlockPropertyDescription_LightRadius);
				myTerminalControlSlider.SetLimits((MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Min : x.ReflectorRadiusBounds.Min, (MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Max : x.ReflectorRadiusBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = ((MyLightingBlock x) => (!x.IsReflector) ? x.RadiusBounds.Default : x.ReflectorRadiusBounds.Default);
				myTerminalControlSlider.Getter = ((MyLightingBlock x) => (!x.IsReflector) ? x.Radius : x.ReflectorRadius);
				myTerminalControlSlider.Setter = delegate(MyLightingBlock x, float v)
				{
					x.m_lightRadius.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.IsReflector ? x.m_reflectorRadius : x.m_radius, 1)).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MyLightingBlock>("Falloff", MySpaceTexts.BlockPropertyTitle_LightFalloff, MySpaceTexts.BlockPropertyDescription_LightFalloff);
				myTerminalControlSlider2.SetLimits((MyLightingBlock x) => x.FalloffBounds.Min, (MyLightingBlock x) => x.FalloffBounds.Max);
				myTerminalControlSlider2.DefaultValueGetter = ((MyLightingBlock x) => x.FalloffBounds.Default);
				myTerminalControlSlider2.Getter = ((MyLightingBlock x) => x.Falloff);
				myTerminalControlSlider2.Setter = delegate(MyLightingBlock x, float v)
				{
					x.m_lightFalloff.Value = v;
				};
				myTerminalControlSlider2.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Falloff, 1));
				};
				myTerminalControlSlider2.Visible = ((MyLightingBlock x) => x.SupportsFalloff);
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MyLightingBlock>("Intensity", MySpaceTexts.BlockPropertyTitle_LightIntensity, MySpaceTexts.BlockPropertyDescription_LightIntensity);
				myTerminalControlSlider3.SetLimits((MyLightingBlock x) => x.IntensityBounds.Min, (MyLightingBlock x) => x.IntensityBounds.Max);
				myTerminalControlSlider3.DefaultValueGetter = ((MyLightingBlock x) => x.IntensityBounds.Default);
				myTerminalControlSlider3.Getter = ((MyLightingBlock x) => x.Intensity);
				myTerminalControlSlider3.Setter = delegate(MyLightingBlock x, float v)
				{
					x.Intensity = v;
				};
				myTerminalControlSlider3.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Intensity, 1));
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MyLightingBlock>("Offset", MySpaceTexts.BlockPropertyTitle_LightOffset, MySpaceTexts.BlockPropertyDescription_LightOffset);
				myTerminalControlSlider4.SetLimits((MyLightingBlock x) => x.OffsetBounds.Min, (MyLightingBlock x) => x.OffsetBounds.Max);
				myTerminalControlSlider4.DefaultValueGetter = ((MyLightingBlock x) => x.OffsetBounds.Default);
				myTerminalControlSlider4.Getter = ((MyLightingBlock x) => x.Offset);
				myTerminalControlSlider4.Setter = delegate(MyLightingBlock x, float v)
				{
					x.m_lightOffset.Value = v;
				};
				myTerminalControlSlider4.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.Offset, 1));
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider5 = new MyTerminalControlSlider<MyLightingBlock>("Blink Interval", MySpaceTexts.BlockPropertyTitle_LightBlinkInterval, MySpaceTexts.BlockPropertyDescription_LightBlinkInterval);
				myTerminalControlSlider5.SetLimits((MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Min, (MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Max);
				myTerminalControlSlider5.DefaultValueGetter = ((MyLightingBlock x) => x.BlinkIntervalSecondsBounds.Default);
				myTerminalControlSlider5.Getter = ((MyLightingBlock x) => x.BlinkIntervalSeconds);
				myTerminalControlSlider5.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkIntervalSeconds = v;
				};
				myTerminalControlSlider5.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkIntervalSeconds, 1)).Append(" s");
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider6 = new MyTerminalControlSlider<MyLightingBlock>("Blink Lenght", MySpaceTexts.BlockPropertyTitle_LightBlinkLenght, MySpaceTexts.BlockPropertyDescription_LightBlinkLenght);
				myTerminalControlSlider6.SetLimits((MyLightingBlock x) => x.BlinkLenghtBounds.Min, (MyLightingBlock x) => x.BlinkLenghtBounds.Max);
				myTerminalControlSlider6.DefaultValueGetter = ((MyLightingBlock x) => x.BlinkLenghtBounds.Default);
				myTerminalControlSlider6.Getter = ((MyLightingBlock x) => x.BlinkLength);
				myTerminalControlSlider6.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkLength = v;
				};
				myTerminalControlSlider6.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkLength, 1)).Append(" %");
				};
				myTerminalControlSlider6.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
				MyTerminalControlSlider<MyLightingBlock> myTerminalControlSlider7 = new MyTerminalControlSlider<MyLightingBlock>("Blink Offset", MySpaceTexts.BlockPropertyTitle_LightBlinkOffset, MySpaceTexts.BlockPropertyDescription_LightBlinkOffset);
				myTerminalControlSlider7.SetLimits((MyLightingBlock x) => x.BlinkOffsetBounds.Min, (MyLightingBlock x) => x.BlinkOffsetBounds.Max);
				myTerminalControlSlider7.DefaultValueGetter = ((MyLightingBlock x) => x.BlinkOffsetBounds.Default);
				myTerminalControlSlider7.Getter = ((MyLightingBlock x) => x.BlinkOffset);
				myTerminalControlSlider7.Setter = delegate(MyLightingBlock x, float v)
				{
					x.BlinkOffset = v;
				};
				myTerminalControlSlider7.Writer = delegate(MyLightingBlock x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.BlinkOffset, 1)).Append(" %");
				};
				myTerminalControlSlider7.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider7);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			IsLargeLight = (cubeGrid.GridSizeEnum == MyCubeSize.Large);
			MyObjectBuilder_LightingBlock myObjectBuilder_LightingBlock = (MyObjectBuilder_LightingBlock)objectBuilder;
			m_color = ((myObjectBuilder_LightingBlock.ColorAlpha == -1f) ? LightColorDef : new Vector4(myObjectBuilder_LightingBlock.ColorRed, myObjectBuilder_LightingBlock.ColorGreen, myObjectBuilder_LightingBlock.ColorBlue, myObjectBuilder_LightingBlock.ColorAlpha));
			m_radius = RadiusBounds.Clamp((myObjectBuilder_LightingBlock.Radius == -1f) ? RadiusBounds.Default : myObjectBuilder_LightingBlock.Radius);
			m_reflectorRadius = ReflectorRadiusBounds.Clamp((myObjectBuilder_LightingBlock.ReflectorRadius == -1f) ? ReflectorRadiusBounds.Default : myObjectBuilder_LightingBlock.ReflectorRadius);
			m_falloff = FalloffBounds.Clamp((myObjectBuilder_LightingBlock.Falloff == -1f) ? FalloffBounds.Default : myObjectBuilder_LightingBlock.Falloff);
			m_blinkIntervalSeconds.SetLocalValue(BlinkIntervalSecondsBounds.Clamp((myObjectBuilder_LightingBlock.BlinkIntervalSeconds == -1f) ? BlinkIntervalSecondsBounds.Default : myObjectBuilder_LightingBlock.BlinkIntervalSeconds));
			m_blinkLength.SetLocalValue(BlinkLenghtBounds.Clamp((myObjectBuilder_LightingBlock.BlinkLenght == -1f) ? BlinkLenghtBounds.Default : myObjectBuilder_LightingBlock.BlinkLenght));
			m_blinkOffset.SetLocalValue(BlinkOffsetBounds.Clamp((myObjectBuilder_LightingBlock.BlinkOffset == -1f) ? BlinkOffsetBounds.Default : myObjectBuilder_LightingBlock.BlinkOffset));
			m_intensity.SetLocalValue(IntensityBounds.Clamp((myObjectBuilder_LightingBlock.Intensity == -1f) ? IntensityBounds.Default : myObjectBuilder_LightingBlock.Intensity));
			m_lightOffset.SetLocalValue(OffsetBounds.Clamp((myObjectBuilder_LightingBlock.Offset == -1f) ? OffsetBounds.Default : myObjectBuilder_LightingBlock.Offset));
			UpdateLightData();
			m_positionDirty = true;
			CreateLights();
			UpdateIntensity();
			UpdateLightPosition();
			UpdateLightBlink();
			UpdateEnabled();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += CubeBlock_OnWorkingChanged;
		}

		private void UpdateLightData()
		{
			m_lightLocalData.Clear();
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies)
			{
				string text = dummy.Key.ToLower();
				if (!text.Contains("subpart") && text.Contains("light"))
				{
					m_lightLocalData.Add(new LightLocalData
					{
						LocalMatrix = Matrix.Normalize(dummy.Value.Matrix),
						Subpart = null
					});
				}
			}
			foreach (KeyValuePair<string, MyEntitySubpart> subpart in base.Subparts)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy2 in subpart.Value.Model.Dummies)
				{
					if (dummy2.Key.ToLower().Contains("light"))
					{
						m_lightLocalData.Add(new LightLocalData
						{
							LocalMatrix = Matrix.Normalize(dummy2.Value.Matrix),
							Subpart = subpart.Value
						});
					}
				}
			}
		}

		private void CreateLights()
		{
			CloseLights();
			foreach (LightLocalData lightLocalDatum in m_lightLocalData)
			{
				_ = lightLocalDatum;
				MyLight myLight = MyLights.AddLight();
				if (myLight != null)
				{
					m_lights.Add(myLight);
					InitLight(myLight, m_color, m_radius, m_falloff);
					myLight.ReflectorColor = m_color;
					myLight.ReflectorRange = m_reflectorRadius;
					myLight.Range = m_radius;
					myLight.ReflectorConeDegrees = ReflectorConeDegrees;
					UpdateRadius(IsReflector ? m_reflectorRadius : m_radius);
				}
			}
			m_positionDirty = true;
		}

		private void UpdateParents()
		{
			uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in m_lights)
			{
				light.ParentID = parentCullObject;
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected abstract void InitLight(MyLight light, Vector4 color, float radius, float falloff);

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_LightingBlock obj = (MyObjectBuilder_LightingBlock)base.GetObjectBuilderCubeBlock(copy);
			Vector4 vector = m_color.ToVector4();
			obj.ColorRed = vector.X;
			obj.ColorGreen = vector.Y;
			obj.ColorBlue = vector.Z;
			obj.ColorAlpha = vector.W;
			obj.Radius = m_radius;
			obj.ReflectorRadius = m_reflectorRadius;
			obj.Falloff = Falloff;
			obj.Intensity = m_intensity;
			obj.BlinkIntervalSeconds = m_blinkIntervalSeconds;
			obj.BlinkLenght = m_blinkLength;
			obj.BlinkOffset = m_blinkOffset;
			obj.Offset = m_lightOffset;
			return obj;
		}

		private void CloseLights()
		{
			foreach (MyLight light in m_lights)
			{
				MyLights.RemoveLight(light);
			}
			m_lights.Clear();
		}

		protected override void Closing()
		{
			CloseLights();
			base.Closing();
		}

		public MyLightingBlock()
		{
			CreateTerminalControls();
			m_lightColor.ValueChanged += delegate
			{
				LightColorChanged();
			};
			m_lightRadius.ValueChanged += delegate
			{
				LightRadiusChanged();
			};
			m_lightFalloff.ValueChanged += delegate
			{
				LightFalloffChanged();
			};
			m_lightOffset.ValueChanged += delegate
			{
				LightOffsetChanged();
			};
		}

		private void LightFalloffChanged()
		{
			Falloff = m_lightFalloff.Value;
		}

		private void LightOffsetChanged()
		{
			UpdateLightProperties();
		}

		protected virtual void UpdateRadius(float value)
		{
			if (IsReflector)
			{
				ReflectorRadius = value;
			}
			else
			{
				Radius = value;
			}
		}

		private void LightRadiusChanged()
		{
			UpdateRadius(m_lightRadius.Value);
		}

		private void LightColorChanged()
		{
			Color = m_lightColor.Value;
		}

		private float GetNewLightPower()
		{
			return MathHelper.Clamp(CurrentLightPower + (float)(base.IsWorking ? 1 : (-1)) * m_lightTurningOnSpeed, 0f, 1f);
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			m_emissiveMaterialDirty = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			m_emissiveMaterialDirty = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_needsRecreateLights)
			{
				m_needsRecreateLights = false;
				CreateLights();
				UpdateLightPosition();
				UpdateIntensity();
				UpdateLightBlink();
				UpdateEnabled();
			}
			UpdateParents();
			UpdateLightProperties();
		}

		public override void UpdateAfterSimulation100()
		{
			if ((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0 && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				return;
			}
			if (NeedPerFrameUpdate)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			else
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			UpdateLightProperties();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
			foreach (MyLight light in m_lights)
			{
				light.ParentID = parentCullObject;
			}
			UpdateLightPosition();
			UpdateLightProperties();
			UpdateEmissivity(force: true);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0))
			{
				uint parentCullObject = base.CubeGrid.Render.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize).ParentCullObject;
				foreach (MyLight light in m_lights)
				{
					light.ParentID = parentCullObject;
				}
				float newLightPower = GetNewLightPower();
				if (newLightPower != CurrentLightPower)
				{
					CurrentLightPower = newLightPower;
					UpdateIntensity();
				}
				UpdateLightBlink();
				UpdateEnabled();
				UpdateLightPosition();
				UpdateLightProperties();
				UpdateEmissivity();
				UpdateEmissiveMaterial();
			}
		}

		private void UpdateEnabled()
		{
			UpdateEnabled(CurrentLightPower * Intensity > 0f && m_blinkOn);
		}

		protected abstract void UpdateEnabled(bool state);

		protected abstract void UpdateIntensity();

		private void UpdateLightBlink()
		{
			if ((float)m_blinkIntervalSeconds > 0.00099f)
			{
				ulong num = (ulong)((float)m_blinkIntervalSeconds * 1000f);
				float num2 = (float)num * (float)m_blinkOffset * 0.01f;
				ulong num3 = (ulong)(MySession.Static.ElapsedGameTime.TotalMilliseconds - (double)num2) % num;
				ulong num4 = (ulong)((float)num * (float)m_blinkLength * 0.01f);
				m_blinkOn = (num4 > num3);
			}
			else
			{
				m_blinkOn = true;
			}
		}

		protected virtual void UpdateEmissivity(bool force = false)
		{
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void CubeBlock_OnWorkingChanged(MyCubeBlock block)
		{
			m_positionDirty = true;
		}

		protected Color ComputeBulbColor()
		{
			float num = IntensityBounds.Normalize(Intensity);
			float num2 = 0.125f + num * 0.25f;
			return new Color((float)(int)Color.R * 0.5f + num2, (float)(int)Color.G * 0.5f + num2, (float)(int)Color.B * 0.5f + num2);
		}

		private void UpdateLightProperties()
		{
			foreach (MyLight light in m_lights)
			{
				light.Range = m_radius;
				light.ReflectorRange = m_reflectorRadius;
				light.Color = m_color;
				light.ReflectorColor = m_color;
				light.Falloff = m_falloff;
				light.PointLightOffset = Offset;
				light.UpdateLight();
			}
		}

		private void UpdateLightPosition()
		{
			if (m_lights == null || m_lights.Count == 0 || !m_positionDirty)
			{
				return;
			}
			m_positionDirty = false;
			_ = (MatrixD)base.PositionComp.LocalMatrix;
			for (int i = 0; i < m_lightLocalData.Count; i++)
			{
				MatrixD matrixD = base.PositionComp.LocalMatrix;
				if (m_lightLocalData[i].Subpart != null)
				{
					matrixD = m_lightLocalData[i].Subpart.PositionComp.LocalMatrix * matrixD;
				}
				MyLight myLight = m_lights[i];
				myLight.Position = Vector3D.Transform(m_lightLocalData[i].LocalMatrix.Translation, matrixD);
				myLight.ReflectorDirection = Vector3D.TransformNormal(m_lightLocalData[i].LocalMatrix.Forward, matrixD);
				myLight.ReflectorUp = Vector3D.TransformNormal(m_lightLocalData[i].LocalMatrix.Right, matrixD);
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			m_positionDirty = true;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			UpdateLightData();
			m_needsRecreateLights = true;
			m_emissiveMaterialDirty = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateParents();
			m_positionDirty = true;
			m_emissiveMaterialDirty = true;
			UpdateLightPosition();
			UpdateIntensity();
			UpdateLightBlink();
			UpdateEnabled();
		}

		private void UpdateEmissiveMaterial()
		{
			if (m_emissiveMaterialDirty)
			{
				uint[] renderObjectIDs = base.Render.RenderObjectIDs;
				foreach (uint renderId in renderObjectIDs)
				{
					UpdateEmissiveMaterial(renderId);
				}
				foreach (LightLocalData lightLocalDatum in m_lightLocalData)
				{
					if (lightLocalDatum.Subpart != null && lightLocalDatum.Subpart.Render != null)
					{
						renderObjectIDs = lightLocalDatum.Subpart.Render.RenderObjectIDs;
						foreach (uint renderId2 in renderObjectIDs)
						{
							UpdateEmissiveMaterial(renderId2);
						}
					}
				}
				m_emissiveMaterialDirty = false;
			}
		}

		private void UpdateEmissiveMaterial(uint renderId)
		{
			MyRenderProxy.UpdateModelProperties(renderId, "Emissive", (RenderFlags)0, (RenderFlags)0, BulbColor, CurrentLightPower);
			MyRenderProxy.UpdateModelProperties(renderId, "EmissiveSpotlight", (RenderFlags)0, (RenderFlags)0, BulbColor, CurrentLightPower);
		}
	}
}
