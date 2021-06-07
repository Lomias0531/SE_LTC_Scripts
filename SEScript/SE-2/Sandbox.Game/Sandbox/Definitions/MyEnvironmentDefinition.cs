using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EnvironmentDefinition), typeof(Postprocessor))]
	public class MyEnvironmentDefinition : MyDefinitionBase
	{
		private class Postprocessor : MyDefinitionPostprocessor
		{
			public override void AfterLoaded(ref Bundle definitions)
			{
			}

			public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
			{
			}

			public override void OverrideBy(ref Bundle currentDefinitions, ref Bundle overrideBySet)
			{
				foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in overrideBySet.Definitions)
				{
					if (definition.Value.Enabled)
					{
						if (currentDefinitions.Definitions.TryGetValue(definition.Key, out MyDefinitionBase value))
						{
							((MyEnvironmentDefinition)value).Merge((MyEnvironmentDefinition)definition.Value);
						}
						else
						{
							currentDefinitions.Definitions.Add(definition.Key, definition.Value);
						}
					}
					else
					{
						currentDefinitions.Definitions.Remove(definition.Key);
					}
				}
			}
		}

		private class Sandbox_Definitions_MyEnvironmentDefinition_003C_003EActor : IActivator, IActivator<MyEnvironmentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentDefinition CreateInstance()
			{
				return new MyEnvironmentDefinition();
			}

			MyEnvironmentDefinition IActivator<MyEnvironmentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyPlanetProperties PlanetProperties = MyPlanetProperties.Default;

		public MyFogProperties FogProperties = MyFogProperties.Default;

		public MySunProperties SunProperties = MySunProperties.Default;

		public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;

		public MySSAOSettings SSAOSettings = MySSAOSettings.Default;

		public MyHBAOData HBAOSettings = MyHBAOData.Default;

		public float LargeShipMaxSpeed = 100f;

		public float SmallShipMaxSpeed = 100f;

		public Color ContourHighlightColor = MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColor;

		public Color ContourHighlightColorAccessDenied = MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColorAccessDenied;

		public float ContourHighlightThickness = 5f;

		public float HighlightPulseInSeconds;

		public List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings> EnvironmentalParticles = new List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>();

		private float m_largeShipMaxAngularSpeed = 18000f;

		private float m_smallShipMaxAngularSpeed = 36000f;

		private float m_largeShipMaxAngularSpeedInRadians = MathHelper.ToRadians(18000f);

		private float m_smallShipMaxAngularSpeedInRadians = MathHelper.ToRadians(36000f);

		public string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";

		public MyOrientation EnvironmentOrientation = MyObjectBuilder_EnvironmentDefinition.Defaults.EnvironmentOrientation;

		public MyShadowsSettings ShadowSettings
		{
			get;
			private set;
		}

		public MyNewLoddingSettings LowLoddingSettings
		{
			get;
			private set;
		}

		public MyNewLoddingSettings MediumLoddingSettings
		{
			get;
			private set;
		}

		public MyNewLoddingSettings HighLoddingSettings
		{
			get;
			private set;
		}

		public MyNewLoddingSettings ExtremeLoddingSettings
		{
			get;
			private set;
		}

		public float LargeShipMaxAngularSpeed
		{
			get
			{
				return m_largeShipMaxAngularSpeed;
			}
			private set
			{
				m_largeShipMaxAngularSpeed = value;
				m_largeShipMaxAngularSpeedInRadians = MathHelper.ToRadians(m_largeShipMaxAngularSpeed);
			}
		}

		public float SmallShipMaxAngularSpeed
		{
			get
			{
				return m_smallShipMaxAngularSpeed;
			}
			private set
			{
				m_smallShipMaxAngularSpeed = value;
				m_smallShipMaxAngularSpeedInRadians = MathHelper.ToRadians(m_smallShipMaxAngularSpeed);
			}
		}

		public float LargeShipMaxAngularSpeedInRadians => m_largeShipMaxAngularSpeedInRadians;

		public float SmallShipMaxAngularSpeedInRadians => m_smallShipMaxAngularSpeedInRadians;

		public MyEnvironmentDefinition()
		{
			ShadowSettings = new MyShadowsSettings();
			LowLoddingSettings = new MyNewLoddingSettings();
			MediumLoddingSettings = new MyNewLoddingSettings();
			HighLoddingSettings = new MyNewLoddingSettings();
			ExtremeLoddingSettings = new MyNewLoddingSettings();
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EnvironmentDefinition myObjectBuilder_EnvironmentDefinition = (MyObjectBuilder_EnvironmentDefinition)builder;
			FogProperties = myObjectBuilder_EnvironmentDefinition.FogProperties;
			PlanetProperties = myObjectBuilder_EnvironmentDefinition.PlanetProperties;
			SunProperties = myObjectBuilder_EnvironmentDefinition.SunProperties;
			PostProcessSettings = myObjectBuilder_EnvironmentDefinition.PostProcessSettings;
			SSAOSettings = myObjectBuilder_EnvironmentDefinition.SSAOSettings;
			HBAOSettings = myObjectBuilder_EnvironmentDefinition.HBAOSettings;
			ShadowSettings.CopyFrom(myObjectBuilder_EnvironmentDefinition.ShadowSettings);
			LowLoddingSettings.CopyFrom(myObjectBuilder_EnvironmentDefinition.LowLoddingSettings);
			MediumLoddingSettings.CopyFrom(myObjectBuilder_EnvironmentDefinition.MediumLoddingSettings);
			HighLoddingSettings.CopyFrom(myObjectBuilder_EnvironmentDefinition.HighLoddingSettings);
			ExtremeLoddingSettings.CopyFrom(myObjectBuilder_EnvironmentDefinition.ExtremeLoddingSettings);
			SmallShipMaxSpeed = myObjectBuilder_EnvironmentDefinition.SmallShipMaxSpeed;
			LargeShipMaxSpeed = myObjectBuilder_EnvironmentDefinition.LargeShipMaxSpeed;
			SmallShipMaxAngularSpeed = myObjectBuilder_EnvironmentDefinition.SmallShipMaxAngularSpeed;
			LargeShipMaxAngularSpeed = myObjectBuilder_EnvironmentDefinition.LargeShipMaxAngularSpeed;
			ContourHighlightColor = new Color(myObjectBuilder_EnvironmentDefinition.ContourHighlightColor);
			ContourHighlightThickness = myObjectBuilder_EnvironmentDefinition.ContourHighlightThickness;
			HighlightPulseInSeconds = myObjectBuilder_EnvironmentDefinition.HighlightPulseInSeconds;
			EnvironmentTexture = myObjectBuilder_EnvironmentDefinition.EnvironmentTexture;
			DateTime now = DateTime.Now;
			if (EnvironmentTexture == "Textures\\BackgroundCube\\Final\\BackgroundCube.dds" && now >= MyObjectBuilder_EnvironmentDefinition.Defaults.ScaryFaceFrom && now <= MyObjectBuilder_EnvironmentDefinition.Defaults.ScaryFaceTo)
			{
				EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_ScaryFace.dds";
			}
			if (EnvironmentTexture == "Textures\\BackgroundCube\\Final\\BackgroundCube.dds" && now >= MyObjectBuilder_EnvironmentDefinition.Defaults.Christmas2019From && now <= MyObjectBuilder_EnvironmentDefinition.Defaults.Christmas2019To)
			{
				EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_Christmas.dds";
			}
			EnvironmentOrientation = myObjectBuilder_EnvironmentDefinition.EnvironmentOrientation;
			EnvironmentalParticles = myObjectBuilder_EnvironmentDefinition.EnvironmentalParticles;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_EnvironmentDefinition myObjectBuilder_EnvironmentDefinition = new MyObjectBuilder_EnvironmentDefinition();
			myObjectBuilder_EnvironmentDefinition.Id = Id;
			myObjectBuilder_EnvironmentDefinition.FogProperties = FogProperties;
			myObjectBuilder_EnvironmentDefinition.SunProperties = SunProperties;
			myObjectBuilder_EnvironmentDefinition.PostProcessSettings = PostProcessSettings;
			myObjectBuilder_EnvironmentDefinition.SSAOSettings = SSAOSettings;
			myObjectBuilder_EnvironmentDefinition.HBAOSettings = HBAOSettings;
			myObjectBuilder_EnvironmentDefinition.ShadowSettings.CopyFrom(ShadowSettings);
			myObjectBuilder_EnvironmentDefinition.LowLoddingSettings.CopyFrom(LowLoddingSettings);
			myObjectBuilder_EnvironmentDefinition.MediumLoddingSettings.CopyFrom(MediumLoddingSettings);
			myObjectBuilder_EnvironmentDefinition.HighLoddingSettings.CopyFrom(HighLoddingSettings);
			myObjectBuilder_EnvironmentDefinition.ExtremeLoddingSettings.CopyFrom(ExtremeLoddingSettings);
			myObjectBuilder_EnvironmentDefinition.SmallShipMaxSpeed = SmallShipMaxSpeed;
			myObjectBuilder_EnvironmentDefinition.LargeShipMaxSpeed = LargeShipMaxSpeed;
			myObjectBuilder_EnvironmentDefinition.SmallShipMaxAngularSpeed = SmallShipMaxAngularSpeed;
			myObjectBuilder_EnvironmentDefinition.LargeShipMaxAngularSpeed = LargeShipMaxAngularSpeed;
			myObjectBuilder_EnvironmentDefinition.ContourHighlightColor = ContourHighlightColor.ToVector4();
			myObjectBuilder_EnvironmentDefinition.ContourHighlightThickness = ContourHighlightThickness;
			myObjectBuilder_EnvironmentDefinition.HighlightPulseInSeconds = HighlightPulseInSeconds;
			myObjectBuilder_EnvironmentDefinition.EnvironmentTexture = EnvironmentTexture;
			myObjectBuilder_EnvironmentDefinition.EnvironmentOrientation = EnvironmentOrientation;
			myObjectBuilder_EnvironmentDefinition.EnvironmentalParticles = EnvironmentalParticles;
			return myObjectBuilder_EnvironmentDefinition;
		}

		public void Merge(MyEnvironmentDefinition src)
		{
			MyEnvironmentDefinition myEnvironmentDefinition = new MyEnvironmentDefinition();
			myEnvironmentDefinition.Id = src.Id;
			myEnvironmentDefinition.DisplayNameEnum = src.DisplayNameEnum;
			myEnvironmentDefinition.DescriptionEnum = src.DescriptionEnum;
			myEnvironmentDefinition.DisplayNameString = src.DisplayNameString;
			myEnvironmentDefinition.DescriptionString = src.DescriptionString;
			myEnvironmentDefinition.Icons = src.Icons;
			myEnvironmentDefinition.Enabled = src.Enabled;
			myEnvironmentDefinition.Public = src.Public;
			myEnvironmentDefinition.AvailableInSurvival = src.AvailableInSurvival;
			myEnvironmentDefinition.Context = src.Context;
			MyMergeHelper.Merge(this, src, myEnvironmentDefinition);
		}
	}
}
