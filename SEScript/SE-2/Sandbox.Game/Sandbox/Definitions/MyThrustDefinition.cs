using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ThrustDefinition), null)]
	public class MyThrustDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyThrustDefinition_003C_003EActor : IActivator, IActivator<MyThrustDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyThrustDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyThrustDefinition CreateInstance()
			{
				return new MyThrustDefinition();
			}

			MyThrustDefinition IActivator<MyThrustDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public MyStringHash ThrusterType;

		public MyFuelConverterInfo FuelConverter;

		public float SlowdownFactor;

		public float ForceMagnitude;

		public float MaxPowerConsumption;

		public float MinPowerConsumption;

		public float FlameDamageLengthScale;

		public float FlameDamage;

		public float FlameLengthScale;

		public Vector4 FlameFullColor;

		public Vector4 FlameIdleColor;

		public string FlamePointMaterial;

		public string FlameLengthMaterial;

		public string FlameFlare;

		public float FlameVisibilityDistance;

		public float FlameGlareQuerySize;

		public float MinPlanetaryInfluence;

		public float MaxPlanetaryInfluence;

		public float InvDiffMinMaxPlanetaryInfluence;

		public float EffectivenessAtMaxInfluence;

		public float EffectivenessAtMinInfluence;

		public bool NeedsAtmosphereForInfluence;

		public float ConsumptionFactorPerG;

		public bool PropellerUse;

		public string PropellerEntity;

		public float PropellerFullSpeed;

		public float PropellerIdleSpeed;

		public float PropellerAcceleration;

		public float PropellerDeceleration;

		public float PropellerMaxDistance;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ThrustDefinition myObjectBuilder_ThrustDefinition = builder as MyObjectBuilder_ThrustDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ThrustDefinition.ResourceSinkGroup);
			FuelConverter = myObjectBuilder_ThrustDefinition.FuelConverter;
			SlowdownFactor = myObjectBuilder_ThrustDefinition.SlowdownFactor;
			ForceMagnitude = myObjectBuilder_ThrustDefinition.ForceMagnitude;
			ThrusterType = MyStringHash.GetOrCompute(myObjectBuilder_ThrustDefinition.ThrusterType);
			MaxPowerConsumption = myObjectBuilder_ThrustDefinition.MaxPowerConsumption;
			MinPowerConsumption = myObjectBuilder_ThrustDefinition.MinPowerConsumption;
			FlameDamageLengthScale = myObjectBuilder_ThrustDefinition.FlameDamageLengthScale;
			FlameDamage = myObjectBuilder_ThrustDefinition.FlameDamage;
			FlameLengthScale = myObjectBuilder_ThrustDefinition.FlameLengthScale;
			FlameFullColor = myObjectBuilder_ThrustDefinition.FlameFullColor;
			FlameIdleColor = myObjectBuilder_ThrustDefinition.FlameIdleColor;
			FlamePointMaterial = myObjectBuilder_ThrustDefinition.FlamePointMaterial;
			FlameLengthMaterial = myObjectBuilder_ThrustDefinition.FlameLengthMaterial;
			FlameFlare = myObjectBuilder_ThrustDefinition.FlameFlare;
			FlameVisibilityDistance = myObjectBuilder_ThrustDefinition.FlameVisibilityDistance;
			FlameGlareQuerySize = myObjectBuilder_ThrustDefinition.FlameGlareQuerySize;
			MinPlanetaryInfluence = myObjectBuilder_ThrustDefinition.MinPlanetaryInfluence;
			MaxPlanetaryInfluence = myObjectBuilder_ThrustDefinition.MaxPlanetaryInfluence;
			EffectivenessAtMinInfluence = myObjectBuilder_ThrustDefinition.EffectivenessAtMinInfluence;
			EffectivenessAtMaxInfluence = myObjectBuilder_ThrustDefinition.EffectivenessAtMaxInfluence;
			NeedsAtmosphereForInfluence = myObjectBuilder_ThrustDefinition.NeedsAtmosphereForInfluence;
			ConsumptionFactorPerG = myObjectBuilder_ThrustDefinition.ConsumptionFactorPerG;
			PropellerUse = myObjectBuilder_ThrustDefinition.PropellerUsesPropellerSystem;
			PropellerEntity = myObjectBuilder_ThrustDefinition.PropellerSubpartEntityName;
			PropellerFullSpeed = myObjectBuilder_ThrustDefinition.PropellerRoundsPerSecondOnFullSpeed;
			PropellerIdleSpeed = myObjectBuilder_ThrustDefinition.PropellerRoundsPerSecondOnIdleSpeed;
			PropellerAcceleration = myObjectBuilder_ThrustDefinition.PropellerAccelerationTime;
			PropellerDeceleration = myObjectBuilder_ThrustDefinition.PropellerDecelerationTime;
			PropellerMaxDistance = myObjectBuilder_ThrustDefinition.PropellerMaxVisibleDistance;
			InvDiffMinMaxPlanetaryInfluence = 1f / (MaxPlanetaryInfluence - MinPlanetaryInfluence);
			if (!InvDiffMinMaxPlanetaryInfluence.IsValid())
			{
				InvDiffMinMaxPlanetaryInfluence = 0f;
			}
		}
	}
}
