#define VRAGE
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Audio;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyCharacterJetpackComponent : MyCharacterComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterJetpackComponent_003C_003EActor : IActivator, IActivator<MyCharacterJetpackComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterJetpackComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterJetpackComponent CreateInstance()
			{
				return new MyCharacterJetpackComponent();
			}

			MyCharacterJetpackComponent IActivator<MyCharacterJetpackComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const float FuelLowThresholdPlayer = 0.1f;

		public const float FuelCriticalThresholdPlayer = 0.05f;

		public const float MinimumInputRequirement = 1E-06f;

		public const float ROTATION_FACTOR = 0.02f;

		private const float AUTO_ENABLE_JETPACK_INTERVAL = 1f;

		private bool m_isOnPlanetSurface;

		private int m_planetSurfaceRaycastCounter;

		private MyJetpackThrustComponent ThrustComp => base.Character.Components.Get<MyEntityThrustComponent>() as MyJetpackThrustComponent;

		public float CurrentAutoEnableDelay
		{
			get;
			set;
		}

		public float ForceMagnitude
		{
			get;
			private set;
		}

		public float MinPowerConsumption
		{
			get;
			private set;
		}

		public float MaxPowerConsumption
		{
			get;
			private set;
		}

		public Vector3 FinalThrust => ThrustComp.FinalThrust;

		public bool CanDrawThrusts => base.Character.ActualUpdateFrame >= 2;

		public bool DampenersTurnedOn => ThrustComp.DampenersEnabled;

		public MyGasProperties FuelDefinition
		{
			get;
			private set;
		}

		public MyFuelConverterInfo FuelConverterDefinition
		{
			get;
			private set;
		}

		public bool IsPowered
		{
			get
			{
				if (((MySession.Static.LocalCharacter != base.Character && !Sync.IsServer) || base.Character.ControllerInfo.Controller == null || !MySession.Static.CreativeToolsEnabled(base.Character.ControllerInfo.Controller.Player.Id.SteamId)) && !MySession.Static.CreativeToolsEnabled(base.Character.ControlSteamId))
				{
					if (ThrustComp != null)
					{
						return ThrustComp.IsThrustPoweredByType(base.Character, ref FuelDefinition.Id);
					}
					return false;
				}
				return true;
			}
		}

		public bool DampenersEnabled
		{
			get
			{
				if (ThrustComp != null)
				{
					return ThrustComp.DampenersEnabled;
				}
				return false;
			}
		}

		public bool Running
		{
			get
			{
				if (TurnedOn && IsPowered)
				{
					return !base.Character.IsDead;
				}
				return false;
			}
		}

		public bool TurnedOn
		{
			get;
			private set;
		}

		public float MinPlanetaryInfluence
		{
			get;
			private set;
		}

		public float MaxPlanetaryInfluence
		{
			get;
			private set;
		}

		public float EffectivenessAtMaxInfluence
		{
			get;
			private set;
		}

		public float EffectivenessAtMinInfluence
		{
			get;
			private set;
		}

		public bool NeedsAtmosphereForInfluence
		{
			get;
			private set;
		}

		public float ConsumptionFactorPerG
		{
			get;
			private set;
		}

		public bool IsFlying
		{
			get;
			private set;
		}

		public override string ComponentTypeDebugString => "Jetpack Component";

		public MyCharacterJetpackComponent()
		{
			CurrentAutoEnableDelay = 0f;
			TurnedOn = false;
		}

		public virtual void Init(MyObjectBuilder_Character characterBuilder)
		{
			if (characterBuilder != null)
			{
				CurrentAutoEnableDelay = characterBuilder.AutoenableJetpackDelay;
				if (ThrustComp != null)
				{
					base.Character.Components.Remove<MyJetpackThrustComponent>();
				}
				MyObjectBuilder_ThrustDefinition thrustProperties = base.Character.Definition.Jetpack.ThrustProperties;
				FuelConverterDefinition = null;
				FuelConverterDefinition = ((!MyFakes.ENABLE_HYDROGEN_FUEL) ? new MyFuelConverterInfo
				{
					Efficiency = 1f
				} : base.Character.Definition.Jetpack.ThrustProperties.FuelConverter);
				MyDefinitionId defId = default(MyDefinitionId);
				if (!FuelConverterDefinition.FuelId.IsNull())
				{
					defId = thrustProperties.FuelConverter.FuelId;
				}
				MyGasProperties definition = null;
				if (MyFakes.ENABLE_HYDROGEN_FUEL)
				{
					MyDefinitionManager.Static.TryGetDefinition(defId, out definition);
				}
				FuelDefinition = (definition ?? new MyGasProperties
				{
					Id = MyResourceDistributorComponent.ElectricityId,
					EnergyDensity = 1f
				});
				ForceMagnitude = thrustProperties.ForceMagnitude;
				MinPowerConsumption = thrustProperties.MinPowerConsumption;
				MaxPowerConsumption = thrustProperties.MaxPowerConsumption;
				MinPlanetaryInfluence = thrustProperties.MinPlanetaryInfluence;
				MaxPlanetaryInfluence = thrustProperties.MaxPlanetaryInfluence;
				EffectivenessAtMinInfluence = thrustProperties.EffectivenessAtMinInfluence;
				EffectivenessAtMaxInfluence = thrustProperties.EffectivenessAtMaxInfluence;
				NeedsAtmosphereForInfluence = thrustProperties.NeedsAtmosphereForInfluence;
				ConsumptionFactorPerG = thrustProperties.ConsumptionFactorPerG;
				MyEntityThrustComponent myEntityThrustComponent = new MyJetpackThrustComponent();
				myEntityThrustComponent.Init();
				base.Character.Components.Add(myEntityThrustComponent);
				ThrustComp.DampenersEnabled = characterBuilder.DampenersEnabled;
				Vector3I[] intDirections = Base6Directions.IntDirections;
				foreach (Vector3I forwardVector in intDirections)
				{
					ThrustComp.Register(base.Character, forwardVector);
				}
				myEntityThrustComponent.ResourceSink(base.Character).TemporaryConnectedEntity = base.Character;
				base.Character.SuitRechargeDistributor.AddSink(myEntityThrustComponent.ResourceSink(base.Character));
				TurnOnJetpack(characterBuilder.JetpackEnabled, fromInit: true, fromLoad: true);
			}
		}

		public virtual void GetObjectBuilder(MyObjectBuilder_Character characterBuilder)
		{
			characterBuilder.DampenersEnabled = DampenersTurnedOn;
			bool jetpackEnabled = TurnedOn;
			if (MySession.Static.ControlledEntity is MyCockpit)
			{
				jetpackEnabled = (MySession.Static.ControlledEntity as MyCockpit).PilotJetpackEnabledBackup;
			}
			characterBuilder.JetpackEnabled = jetpackEnabled;
			characterBuilder.AutoenableJetpackDelay = CurrentAutoEnableDelay;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			if (!base.Entity.MarkedForClose)
			{
				base.Character.SuitRechargeDistributor.RemoveSink(ThrustComp.ResourceSink(base.Character), resetSinkInput: true, base.Entity.MarkedForClose);
				base.OnBeforeRemovedFromContainer();
			}
		}

		public override void Simulate()
		{
			ThrustComp.UpdateBeforeSimulation(Sync.IsServer || base.Character == MySession.Static.LocalCharacter, base.Character.RelativeDampeningEntity);
		}

		public override void OnCharacterDead()
		{
			base.OnCharacterDead();
			TurnOnJetpack(newState: false);
		}

		public void TurnOnJetpack(bool newState, bool fromInit = false, bool fromLoad = false)
		{
			MyEntityController controller = base.Character.ControllerInfo.Controller;
			newState = (newState && MySession.Static.Settings.EnableJetpack);
			newState = (newState && base.Character.Definition.Jetpack != null);
			newState = (newState && (!MySession.Static.SurvivalMode || MyFakes.ENABLE_JETPACK_IN_SURVIVAL || controller == null || MySession.Static.CreativeToolsEnabled(controller.Player.Id.SteamId)));
			bool flag = TurnedOn != newState;
			TurnedOn = newState;
			ThrustComp.Enabled = newState;
			ThrustComp.ControlThrust = Vector3.Zero;
			ThrustComp.MarkDirty();
			ThrustComp.UpdateBeforeSimulation(updateDampeners: true, base.Character.RelativeDampeningEntity);
			if (!ThrustComp.Enabled)
			{
				ThrustComp.SetRequiredFuelInput(ref FuelDefinition.Id, 1E-06f, null);
			}
			ThrustComp.ResourceSink(base.Character).Update();
			if (!base.Character.ControllerInfo.IsLocallyControlled() && !fromInit && !Sync.IsServer)
			{
				return;
			}
			MyCharacterMovementEnum currentMovementState = base.Character.GetCurrentMovementState();
			if (currentMovementState == MyCharacterMovementEnum.Sitting)
			{
				return;
			}
			if (TurnedOn)
			{
				base.Character.StopFalling();
			}
			bool flag2 = false;
			bool flag3 = newState;
			if (!IsPowered && flag3 && ((base.Character.ControllerInfo.Controller != null && !MySession.Static.CreativeToolsEnabled(base.Character.ControllerInfo.Controller.Player.Id.SteamId)) || (MySession.Static.LocalCharacter != base.Character && !Sync.IsServer)))
			{
				flag3 = false;
				flag2 = true;
			}
			if (flag3)
			{
				if (base.Character.IsOnLadder)
				{
					base.Character.GetOffLadder();
				}
				base.Character.IsUsing = null;
			}
			if (flag && !base.Character.IsDead)
			{
				base.Character.UpdateCharacterPhysics();
			}
			if (MySession.Static.ControlledEntity == base.Character && flag && !fromLoad)
			{
				if (flag3)
				{
					MyAnalyticsHelper.ReportActivityStart(base.Character, "jetpack", "character", string.Empty, string.Empty);
				}
				else
				{
					MyAnalyticsHelper.ReportActivityEnd(base.Character, "jetpack");
				}
				if (flag2)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					TurnedOn = false;
					ThrustComp.Enabled = false;
					ThrustComp.ControlThrust = Vector3.Zero;
					ThrustComp.MarkDirty();
					ThrustComp.UpdateBeforeSimulation(updateDampeners: true, base.Character.RelativeDampeningEntity);
					ThrustComp.SetRequiredFuelInput(ref FuelDefinition.Id, 1E-06f, null);
					ThrustComp.ResourceSink(base.Character).Update();
				}
			}
			MyCharacterProxy characterProxy = base.Character.Physics.CharacterProxy;
			if (characterProxy != null)
			{
				MatrixD worldMatrix = base.Character.WorldMatrix;
				characterProxy.SetForwardAndUp(worldMatrix.Forward, worldMatrix.Up);
				characterProxy.EnableFlyingState(Running);
				if (currentMovementState != MyCharacterMovementEnum.Died && !base.Character.IsOnLadder)
				{
					if (!Running && (characterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR || characterProxy.GetState() == (HkCharacterStateType)5))
					{
						base.Character.StartFalling();
					}
					else if (currentMovementState != 0 && !newState)
					{
						base.Character.PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
						base.Character.SetCurrentMovementState(MyCharacterMovementEnum.Standing);
						currentMovementState = base.Character.GetCurrentMovementState();
					}
				}
				if (Running && currentMovementState != MyCharacterMovementEnum.Died)
				{
					base.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
					base.Character.SetCurrentMovementState(MyCharacterMovementEnum.Flying);
					base.Character.SetLocalHeadAnimation(0f, 0f, 0.3f);
					characterProxy.PosX = 0f;
					characterProxy.PosY = 0f;
				}
				if (!fromLoad && !newState && base.Character.Physics.Gravity.LengthSquared() <= 0.1f)
				{
					CurrentAutoEnableDelay = -1f;
				}
			}
			else if (Running && currentMovementState != MyCharacterMovementEnum.Died)
			{
				base.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
				base.Character.SetLocalHeadAnimation(0f, 0f, 0.3f);
			}
		}

		public void UpdateFall()
		{
			if (!(CurrentAutoEnableDelay < 1f))
			{
				ThrustComp.DampenersEnabled = true;
				TurnOnJetpack(newState: true);
				CurrentAutoEnableDelay = -1f;
			}
		}

		public void MoveAndRotate(ref Vector3 moveIndicator, ref Vector2 rotationIndicator, float roll, bool canRotate)
		{
			MyCharacterProxy characterProxy = base.Character.Physics.CharacterProxy;
			ThrustComp.ControlThrust = Vector3.Zero;
			base.Character.SwitchAnimation(MyCharacterMovementEnum.Flying);
			base.Character.SetCurrentMovementState(MyCharacterMovementEnum.Flying);
			IsFlying = (moveIndicator.LengthSquared() != 0f);
			HkCharacterStateType hkCharacterStateType = characterProxy?.GetState() ?? HkCharacterStateType.HK_CHARACTER_ON_GROUND;
			if (hkCharacterStateType == HkCharacterStateType.HK_CHARACTER_IN_AIR || hkCharacterStateType == (HkCharacterStateType)5)
			{
				base.Character.PlayCharacterAnimation("Jetpack", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
				base.Character.CanJump = true;
			}
			MatrixD worldMatrix = base.Character.WorldMatrix;
			if (canRotate)
			{
				MatrixD matrix = MatrixD.Identity;
				MatrixD matrix2 = MatrixD.Identity;
				MatrixD matrix3 = MatrixD.Identity;
				if (Math.Abs(rotationIndicator.X) > float.Epsilon)
				{
					if (base.Character.Definition.VerticalPositionFlyingOnly)
					{
						base.Character.SetHeadLocalXAngle(base.Character.HeadLocalXAngle - rotationIndicator.X * base.Character.RotationSpeed);
					}
					else
					{
						matrix = MatrixD.CreateFromAxisAngle(worldMatrix.Right, (0f - rotationIndicator.X) * base.Character.RotationSpeed * 0.02f);
					}
				}
				if (Math.Abs(rotationIndicator.Y) > float.Epsilon)
				{
					matrix2 = MatrixD.CreateFromAxisAngle(worldMatrix.Up, (0f - rotationIndicator.Y) * base.Character.RotationSpeed * 0.02f);
				}
				if (!base.Character.Definition.VerticalPositionFlyingOnly && Math.Abs(roll) > float.Epsilon)
				{
					matrix3 = MatrixD.CreateFromAxisAngle(worldMatrix.Forward, roll * 0.02f);
				}
				float y = base.Character.ModelCollision.BoundingBoxSizeHalf.Y;
				Vector3D value = base.Character.Physics.GetWorldMatrix().Translation + worldMatrix.Up * y;
				MatrixD matrixD = matrix * matrix2 * matrix3;
				MatrixD orientation = worldMatrix.GetOrientation();
				orientation *= matrixD;
				orientation.Translation = value - orientation.Up * y;
				base.Character.WorldMatrix = orientation;
				base.Character.ClearShapeContactPoints();
			}
			Vector3 vector = moveIndicator;
			if (base.Character.Definition.VerticalPositionFlyingOnly)
			{
				float num = Math.Sign(base.Character.HeadLocalXAngle);
				double num2 = Math.Abs(MathHelper.ToRadians(base.Character.HeadLocalXAngle));
				double y2 = 1.95;
				double num3 = Math.Pow(num2, y2);
				num3 *= num2 / Math.Pow(MathHelper.ToRadians(89f), y2);
				MatrixD matrix4 = MatrixD.CreateFromAxisAngle(Vector3D.Right, (double)num * num3);
				vector = Vector3D.Transform(vector, matrix4);
			}
			ThrustComp.ControlThrust += vector;
		}

		public bool UpdatePhysicalMovement()
		{
			if (!Running)
			{
				return false;
			}
			MyPhysicsBody physics = base.Character.Physics;
			MyCharacterProxy characterProxy = physics.CharacterProxy;
			if (characterProxy != null && characterProxy.LinearVelocity.Length() < 0.001f)
			{
				characterProxy.LinearVelocity = Vector3.Zero;
			}
			float scaleFactor = 1f;
			HkRigidBody rigidBody = physics.RigidBody;
			if (rigidBody != null)
			{
				rigidBody.Gravity = Vector3.Zero;
				if (MySession.Static.SurvivalMode || MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE)
				{
					Vector3 vector = scaleFactor * MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.Character.PositionComp.WorldAABB.Center);
					if (vector != Vector3.Zero)
					{
						rigidBody.Gravity = vector * MyPerGameSettings.CharacterGravityMultiplier;
					}
				}
				return true;
			}
			if (characterProxy != null)
			{
				characterProxy.Gravity = Vector3.Zero;
				if (MySession.Static.SurvivalMode || MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE)
				{
					Vector3 vector2 = scaleFactor * MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.Character.PositionComp.WorldAABB.Center);
					if (vector2 != Vector3.Zero)
					{
						characterProxy.Gravity = vector2 * MyPerGameSettings.CharacterGravityMultiplier;
					}
				}
				return true;
			}
			return false;
		}

		public void EnableDampeners(bool enable)
		{
			if (DampenersTurnedOn != enable)
			{
				ThrustComp.DampenersEnabled = enable;
			}
		}

		public void SwitchDamping()
		{
			if (base.Character.GetCurrentMovementState() != MyCharacterMovementEnum.Died)
			{
				EnableDampeners(!DampenersTurnedOn);
			}
		}

		public void SwitchThrusts()
		{
			if (base.Character.GetCurrentMovementState() != MyCharacterMovementEnum.Died)
			{
				TurnOnJetpack(!TurnedOn);
			}
		}

		public void ClearMovement()
		{
			ThrustComp.ControlThrust = Vector3.Zero;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			base.NeedsUpdateSimulation = true;
		}
	}
}
