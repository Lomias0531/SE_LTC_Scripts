using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	internal class MyJetpackThrustComponent : MyEntityThrustComponent
	{
		private class Sandbox_Game_EntityComponents_MyJetpackThrustComponent_003C_003EActor : IActivator, IActivator<MyJetpackThrustComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyJetpackThrustComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJetpackThrustComponent CreateInstance()
			{
				return new MyJetpackThrustComponent();
			}

			MyJetpackThrustComponent IActivator<MyJetpackThrustComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public new MyCharacter Entity => base.Entity as MyCharacter;

		public MyCharacter Character => Entity;

		public MyCharacterJetpackComponent Jetpack => Character.JetpackComp;

		protected override void UpdateThrusts(bool enableDampers, Vector3 dampeningVelocity)
		{
			base.UpdateThrusts(enableDampers, dampeningVelocity);
			if (Character == null || Character.Physics == null || Character.Physics.CharacterProxy == null || !Jetpack.TurnedOn)
			{
				return;
			}
			if (base.FinalThrust.LengthSquared() > 0.0001f && Character.Physics.IsInWorld)
			{
				Character.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, base.FinalThrust, null, null);
				Vector3 linearVelocityLocal = Character.Physics.LinearVelocityLocal;
				float num = Math.Max(Character.Definition.MaxSprintSpeed, Math.Max(Character.Definition.MaxRunSpeed, Character.Definition.MaxBackrunSpeed));
				float num2 = MyGridPhysics.ShipMaxLinearVelocity() + num;
				if (linearVelocityLocal.LengthSquared() > num2 * num2)
				{
					linearVelocityLocal.Normalize();
					linearVelocityLocal *= num2;
					Character.Physics.LinearVelocity = linearVelocityLocal;
				}
			}
			if (Character.Physics.Enabled && Character.Physics.LinearVelocity != Vector3.Zero && Character.Physics.LinearVelocity.LengthSquared() < 1.00000011E-06f)
			{
				Character.Physics.LinearVelocity = Vector3.Zero;
				base.ControlThrustChanged = true;
			}
		}

		public override void Register(MyEntity entity, Vector3I forwardVector, Func<bool> onRegisteredCallback = null)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				base.Register(entity, forwardVector, onRegisteredCallback);
				MyDefinitionId fuelType = FuelType(entity);
				float efficiency = 1f;
				if (MyFakes.ENABLE_HYDROGEN_FUEL)
				{
					efficiency = Jetpack.FuelConverterDefinition.Efficiency;
				}
				m_lastFuelTypeData.Efficiency = efficiency;
				m_lastFuelTypeData.EnergyDensity = Jetpack.FuelDefinition.EnergyDensity;
				m_lastSink.SetMaxRequiredInputByType(fuelType, m_lastSink.MaxRequiredInputByType(fuelType) + PowerAmountToFuel(ref fuelType, Jetpack.MaxPowerConsumption, m_lastGroup));
				base.SlowdownFactor = Math.Max(myCharacter.Definition.Jetpack.ThrustProperties.SlowdownFactor, base.SlowdownFactor);
			}
		}

		protected override bool RecomputeOverriddenParameters(MyEntity thrustEntity, FuelTypeData fuelData)
		{
			return false;
		}

		protected override bool IsUsed(MyEntity thrustEntity)
		{
			return base.Enabled;
		}

		protected override float ForceMagnitude(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere)
		{
			return Jetpack.ForceMagnitude * CalculateForceMultiplier(thrustEntity, planetaryInfluence, inAtmosphere);
		}

		protected override float CalculateForceMultiplier(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere)
		{
			float result = 1f;
			if (Jetpack.MaxPlanetaryInfluence != Jetpack.MinPlanetaryInfluence && ((inAtmosphere && Jetpack.NeedsAtmosphereForInfluence) || !inAtmosphere))
			{
				result = MathHelper.Lerp(Jetpack.EffectivenessAtMinInfluence, Jetpack.EffectivenessAtMaxInfluence, MathHelper.Clamp((planetaryInfluence - Jetpack.MinPlanetaryInfluence) / (Jetpack.MaxPlanetaryInfluence - Jetpack.MinPlanetaryInfluence), 0f, 1f));
			}
			else if (Jetpack.NeedsAtmosphereForInfluence && !inAtmosphere)
			{
				result = Jetpack.EffectivenessAtMinInfluence;
			}
			return result;
		}

		protected override float CalculateConsumptionMultiplier(MyEntity thrustEntity, float naturalGravityStrength)
		{
			return 1f + Jetpack.ConsumptionFactorPerG * (naturalGravityStrength / 9.81f);
		}

		protected override float MaxPowerConsumption(MyEntity thrustEntity)
		{
			return Jetpack.MaxPowerConsumption;
		}

		protected override float MinPowerConsumption(MyEntity thrustEntity)
		{
			return Jetpack.MinPowerConsumption;
		}

		protected override void UpdateThrustStrength(HashSet<MyEntity> entities, float thrustForce)
		{
			base.ControlThrust = Vector3.Zero;
		}

		protected override MyDefinitionId FuelType(MyEntity thrustEntity)
		{
			if (Jetpack == null || Jetpack.FuelDefinition == null)
			{
				return MyResourceDistributorComponent.ElectricityId;
			}
			return Jetpack.FuelDefinition.Id;
		}

		protected override bool IsThrustEntityType(MyEntity thrustEntity)
		{
			return thrustEntity is MyCharacter;
		}

		protected override void RemoveFromGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group)
		{
		}

		protected override void AddToGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group)
		{
		}

		protected override Vector3 ApplyThrustModifiers(ref MyDefinitionId fuelType, ref Vector3 thrust, ref Vector3 thrustOverride, MyResourceSinkComponentBase resourceSink)
		{
			thrust += thrustOverride;
			if (((Character.ControllerInfo.Controller == null || !MySession.Static.CreativeToolsEnabled(Character.ControllerInfo.Controller.Player.Id.SteamId)) && !MySession.Static.CreativeToolsEnabled(Character.ControlSteamId)) || (MySession.Static.LocalCharacter != Character && !Sync.IsServer))
			{
				thrust *= resourceSink.SuppliedRatioByType(fuelType);
			}
			thrust *= MyFakes.THRUST_FORCE_RATIO;
			return thrust;
		}
	}
}
