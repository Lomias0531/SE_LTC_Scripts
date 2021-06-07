using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	internal class MyThrusterBlockThrustComponent : MyEntityThrustComponent
	{
		private class Sandbox_Game_EntityComponents_MyThrusterBlockThrustComponent_003C_003EActor : IActivator, IActivator<MyThrusterBlockThrustComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyThrusterBlockThrustComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyThrusterBlockThrustComponent CreateInstance()
			{
				return new MyThrusterBlockThrustComponent();
			}

			MyThrusterBlockThrustComponent IActivator<MyThrusterBlockThrustComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_levitationPeriodLength = 1.3f;

		private float m_levitationTorqueCoeficient = 0.25f;

		private new MyCubeGrid Entity => base.Entity as MyCubeGrid;

		private MyCubeGrid CubeGrid => Entity;

		protected override void UpdateThrusts(bool enableDampeners, Vector3 dampeningVelocity)
		{
			base.UpdateThrusts(enableDampeners, dampeningVelocity);
			MyCubeGrid cubeGrid = CubeGrid;
			if (cubeGrid == null)
			{
				return;
			}
			MyGridPhysics physics = cubeGrid.Physics;
			if (physics == null || physics.IsStatic || !physics.Enabled)
			{
				return;
			}
			Vector3 vector = base.FinalThrust;
			if (!(vector.LengthSquared() > 0.0001f))
			{
				return;
			}
			if (physics.IsWelded)
			{
				vector = Vector3.TransformNormal(vector, CubeGrid.WorldMatrix);
				vector = Vector3.TransformNormal(vector, Matrix.Invert(CubeGrid.Physics.RigidBody.GetRigidBodyMatrix()));
			}
			MyGridPhysicalGroupData.GroupSharedPxProperties groupSharedProperties = MyGridPhysicalGroupData.GetGroupSharedProperties(cubeGrid);
			float? maxSpeed = null;
			if (groupSharedProperties.GridCount == 1)
			{
				maxSpeed = MyGridPhysics.GetShipMaxLinearVelocity(cubeGrid.GridSizeEnum);
			}
			else
			{
				MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(cubeGrid);
				MyGridPhysics physics2 = root.Physics;
				if (physics2 != null && !physics2.IsStatic)
				{
					Vector3D vector3D = Vector3D.TransformNormal(vector, cubeGrid.WorldMatrix);
					Vector3 linearVelocity = physics2.LinearVelocity;
					Vector3D vector3D2 = linearVelocity + vector3D * 0.01666666753590107 / groupSharedProperties.Mass;
					float shipMaxLinearVelocity = MyGridPhysics.GetShipMaxLinearVelocity(root.GridSizeEnum);
					if (vector3D2.LengthSquared() > (double)(shipMaxLinearVelocity * shipMaxLinearVelocity))
					{
						float num = Vector3.Dot(vector3D, linearVelocity) / linearVelocity.LengthSquared();
						if (num > 0f)
						{
							Vector3 vector2 = num * linearVelocity;
							vector3D -= vector2;
							vector = Vector3D.TransformNormal(vector3D, cubeGrid.PositionComp.WorldMatrixNormalizedInv);
						}
					}
				}
			}
			physics.AddForce(position: (groupSharedProperties.ReferenceGrid != cubeGrid) ? Vector3D.Transform(groupSharedProperties.CoMWorld, cubeGrid.PositionComp.WorldMatrixNormalizedInv) : ((Vector3D)groupSharedProperties.PxProperties.CenterOfMass), type: MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, force: vector, torque: null, maxSpeed: maxSpeed, applyImmediately: false, activeOnly: true);
		}

		public override void Register(MyEntity entity, Vector3I forwardVector, Func<bool> onRegisteredCallback)
		{
			if (entity is MyThrust)
			{
				m_thrustEntitiesPending.Enqueue(new MyTuple<MyEntity, Vector3I, Func<bool>>(entity, forwardVector, onRegisteredCallback));
			}
		}

		protected override bool RegisterLazy(MyEntity entity, Vector3I forwardVector, Func<bool> onRegisteredCallback)
		{
			base.RegisterLazy(entity, forwardVector, onRegisteredCallback);
			base.Register(entity, forwardVector, onRegisteredCallback);
			MyThrust myThrust = entity as MyThrust;
			MyDefinitionId fuelType = FuelType(entity);
			m_lastFuelTypeData.EnergyDensity = myThrust.FuelDefinition.EnergyDensity;
			m_lastFuelTypeData.Efficiency = myThrust.BlockDefinition.FuelConverter.Efficiency;
			m_lastSink.SetMaxRequiredInputByType(fuelType, m_lastSink.MaxRequiredInputByType(fuelType) + PowerAmountToFuel(ref fuelType, myThrust.MaxPowerConsumption, m_lastGroup));
			myThrust.EnabledChanged += thrust_EnabledChanged;
			myThrust.ThrustOverrideChanged += MyThrust_ThrustOverrideChanged;
			myThrust.SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.SlowdownFactor = Math.Max(myThrust.BlockDefinition.SlowdownFactor, base.SlowdownFactor);
			onRegisteredCallback?.Invoke();
			return true;
		}

		public override void Unregister(MyEntity entity, Vector3I forwardVector)
		{
			base.Unregister(entity, forwardVector);
			MyThrust myThrust = entity as MyThrust;
			if (myThrust != null)
			{
				myThrust.SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
				myThrust.ThrustOverrideChanged -= MyThrust_ThrustOverrideChanged;
				myThrust.EnabledChanged -= thrust_EnabledChanged;
				base.SlowdownFactor = 0f;
				Vector3I[] intDirections = Base6Directions.IntDirections;
				foreach (Vector3I key in intDirections)
				{
					foreach (FuelTypeData item in m_dataByFuelType)
					{
						foreach (MyEntity item2 in item.ThrustsByDirection[key])
						{
							MyThrust myThrust2 = item2 as MyThrust;
							if (myThrust2 != null)
							{
								base.SlowdownFactor = Math.Max(myThrust2.BlockDefinition.SlowdownFactor, base.SlowdownFactor);
							}
						}
					}
					foreach (MyConveyorConnectedGroup connectedGroup in base.ConnectedGroups)
					{
						foreach (FuelTypeData item3 in connectedGroup.DataByFuelType)
						{
							foreach (MyEntity item4 in item3.ThrustsByDirection[key])
							{
								MyThrust myThrust3 = item4 as MyThrust;
								if (myThrust3 != null)
								{
									base.SlowdownFactor = Math.Max(myThrust3.BlockDefinition.SlowdownFactor, base.SlowdownFactor);
								}
							}
						}
					}
				}
			}
		}

		protected override void UpdateThrustStrength(HashSet<MyEntity> thrusters, float thrustForce)
		{
			foreach (MyEntity thruster in thrusters)
			{
				MyThrust myThrust = thruster as MyThrust;
				if (myThrust != null)
				{
					if (thrustForce == 0f && !IsOverridden(myThrust))
					{
						myThrust.CurrentStrength = 0f;
					}
					else
					{
						float num = CalculateForceMultiplier(myThrust, m_lastPlanetaryInfluence, m_lastPlanetaryInfluenceHasAtmosphere);
						MyResourceSinkComponent myResourceSinkComponent = ResourceSink(myThrust);
						if (IsOverridden(myThrust))
						{
							if (MySession.Static.CreativeMode && myThrust.IsWorking)
							{
								myThrust.CurrentStrength = num * myThrust.ThrustOverrideOverForceLen;
							}
							else
							{
								myThrust.CurrentStrength = num * myThrust.ThrustOverride * myResourceSinkComponent.SuppliedRatioByType(myThrust.FuelDefinition.Id) / myThrust.ThrustForce.Length();
							}
						}
						else if (IsUsed(myThrust))
						{
							myThrust.CurrentStrength = num * thrustForce * myResourceSinkComponent.SuppliedRatioByType(myThrust.FuelDefinition.Id);
						}
						else
						{
							myThrust.CurrentStrength = 0f;
						}
					}
				}
			}
		}

		private void MyThrust_ThrustOverrideChanged(MyThrust block, float newValue)
		{
			MarkDirty();
		}

		private void thrust_EnabledChanged(MyTerminalBlock obj)
		{
			MarkDirty();
			if (CubeGrid != null && CubeGrid.Physics != null && !CubeGrid.Physics.RigidBody.IsActive)
			{
				CubeGrid.ActivatePhysics();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			MarkDirty();
			if (CubeGrid != null && CubeGrid.Physics != null && !CubeGrid.Physics.RigidBody.IsActive)
			{
				CubeGrid.ActivatePhysics();
			}
		}

		private static bool IsOverridden(MyThrust thrust)
		{
			if (thrust != null && thrust.ThrustOverride > 0f && thrust.Enabled && thrust.IsFunctional && (!thrust.CubeGrid.Components.TryGet(out MyEntityThrustComponent component) || !component.AutopilotEnabled))
			{
				return true;
			}
			return false;
		}

		protected override bool RecomputeOverriddenParameters(MyEntity thrustEntity, FuelTypeData fuelData)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust == null)
			{
				return false;
			}
			if (!IsOverridden(myThrust))
			{
				return false;
			}
			Vector3 vector = myThrust.ThrustOverride * -myThrust.ThrustForwardVector * CalculateForceMultiplier(thrustEntity, m_lastPlanetaryInfluence, m_lastPlanetaryInfluenceHasAtmosphere);
			float num = myThrust.ThrustOverride / myThrust.ThrustForce.Length() * myThrust.MaxPowerConsumption;
			if (fuelData.ThrustsByDirection[myThrust.ThrustForwardVector].Contains(thrustEntity))
			{
				fuelData.ThrustOverride += vector;
				fuelData.ThrustOverridePower += num;
			}
			return true;
		}

		protected override bool IsUsed(MyEntity thrustEntity)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust == null)
			{
				return false;
			}
			if (myThrust.Enabled && myThrust.IsFunctional)
			{
				return myThrust.ThrustOverride == 0f;
			}
			return false;
		}

		protected override float CalculateForceMultiplier(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere)
		{
			MyThrust obj = thrustEntity as MyThrust;
			float result = 1f;
			MyThrustDefinition blockDefinition = obj.BlockDefinition;
			if (blockDefinition.NeedsAtmosphereForInfluence && !inAtmosphere)
			{
				result = blockDefinition.EffectivenessAtMinInfluence;
			}
			else if (blockDefinition.MaxPlanetaryInfluence != blockDefinition.MinPlanetaryInfluence)
			{
				float value = (planetaryInfluence - blockDefinition.MinPlanetaryInfluence) * blockDefinition.InvDiffMinMaxPlanetaryInfluence;
				result = MathHelper.Lerp(blockDefinition.EffectivenessAtMinInfluence, blockDefinition.EffectivenessAtMaxInfluence, MathHelper.Clamp(value, 0f, 1f));
			}
			return result;
		}

		protected override float CalculateConsumptionMultiplier(MyEntity thrustEntity, float naturalGravityStrength)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust == null)
			{
				return 1f;
			}
			return 1f + myThrust.BlockDefinition.ConsumptionFactorPerG * (naturalGravityStrength / 9.81f);
		}

		protected override float ForceMagnitude(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust == null)
			{
				return 0f;
			}
			float num = (thrustEntity is IMyThrust) ? (thrustEntity as IMyThrust).ThrustMultiplier : 1f;
			return myThrust.BlockDefinition.ForceMagnitude * num * CalculateForceMultiplier(myThrust, planetaryInfluence, inAtmosphere);
		}

		protected override float MaxPowerConsumption(MyEntity thrustEntity)
		{
			return (thrustEntity as MyThrust).MaxPowerConsumption;
		}

		protected override float MinPowerConsumption(MyEntity thrustEntity)
		{
			return (thrustEntity as MyThrust).MinPowerConsumption;
		}

		protected override MyDefinitionId FuelType(MyEntity thrustEntity)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust.FuelDefinition == null)
			{
				return MyResourceDistributorComponent.ElectricityId;
			}
			return myThrust.FuelDefinition.Id;
		}

		protected override bool IsThrustEntityType(MyEntity thrustEntity)
		{
			return thrustEntity is MyThrust;
		}

		protected override void AddToGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust != null)
			{
				group.ResourceSink.IsPoweredChanged += myThrust.Sink_IsPoweredChanged;
			}
		}

		protected override void RemoveFromGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group)
		{
			MyThrust myThrust = thrustEntity as MyThrust;
			if (myThrust != null)
			{
				group.ResourceSink.IsPoweredChanged -= myThrust.Sink_IsPoweredChanged;
			}
		}

		protected override float CalculateMass()
		{
			MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>.Group group = MyCubeGridGroups.Static.PhysicalDynamic.GetGroup(Entity);
			MyGridPhysics physics = Entity.Physics;
			float num = (physics.WeldedRigidBody != null) ? physics.WeldedRigidBody.Mass : GetGridMass(CubeGrid);
			MyCubeGrid myCubeGrid = null;
			float num2 = 0f;
			if (group != null)
			{
				float num3 = 0f;
				foreach (MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>.Node node in group.Nodes)
				{
					MyCubeGrid nodeData = node.NodeData;
					if (!nodeData.IsStatic && nodeData.Physics != null && !MyFixedGrids.IsRooted(nodeData))
					{
						MyEntityThrustComponent myEntityThrustComponent = nodeData.Components.Get<MyEntityThrustComponent>();
						if (myEntityThrustComponent == null || !myEntityThrustComponent.Enabled || !myEntityThrustComponent.HasPower)
						{
							num2 += ((nodeData.Physics.WeldedRigidBody != null) ? nodeData.Physics.WeldedRigidBody.Mass : GetGridMass(nodeData));
						}
						else
						{
							float radius = nodeData.PositionComp.LocalVolume.Radius;
							if (radius > num3 || (radius == num3 && (myCubeGrid == null || nodeData.EntityId > myCubeGrid.EntityId)))
							{
								num3 = radius;
								myCubeGrid = nodeData;
							}
						}
					}
				}
			}
			if (myCubeGrid == CubeGrid)
			{
				num += num2;
			}
			return num;
		}

		private float GetGridMass(MyCubeGrid grid)
		{
			if (!Sync.IsServer)
			{
				if (MyFixedGrids.IsRooted(grid))
				{
					return 0f;
				}
				HkMassProperties? massProperties = grid.Physics.Shape.MassProperties;
				if (massProperties.HasValue)
				{
					return massProperties.Value.Mass;
				}
			}
			return grid.Physics.Mass;
		}
	}
}
