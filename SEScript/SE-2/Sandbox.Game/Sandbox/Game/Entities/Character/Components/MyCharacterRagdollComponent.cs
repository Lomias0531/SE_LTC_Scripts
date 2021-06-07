using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyCharacterRagdollComponent : MyCharacterComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterRagdollComponent_003C_003EActor : IActivator, IActivator<MyCharacterRagdollComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterRagdollComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterRagdollComponent CreateInstance()
			{
				return new MyCharacterRagdollComponent();
			}

			MyCharacterRagdollComponent IActivator<MyCharacterRagdollComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyRagdollMapper RagdollMapper;

		private IMyGunObject<MyDeviceBase> m_previousWeapon;

		private MyPhysicsBody m_previousPhysics;

		private Vector3D m_lastPosition;

		private int m_gravityTimer;

		private const int GRAVITY_DELAY = 300;

		public float Distance;

		public bool IsRagdollMoving
		{
			get;
			set;
		}

		public bool IsRagdollActivated
		{
			get
			{
				if (base.Character.Physics == null)
				{
					return false;
				}
				return base.Character.Physics.IsRagdollModeActive;
			}
		}

		public override string ComponentTypeDebugString => "Character Ragdoll Component";

		public bool InitRagdoll()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("RagdollComponent.InitRagdoll");
			}
			if (base.Character.Physics.Ragdoll != null)
			{
				base.Character.Physics.CloseRagdollMode();
				base.Character.Physics.Ragdoll.ResetToRigPose();
				base.Character.Physics.Ragdoll.SetToKeyframed();
				return true;
			}
			base.Character.Physics.Ragdoll = new HkRagdoll();
			bool flag = false;
			if (base.Character.Model.HavokData != null && base.Character.Model.HavokData.Length != 0)
			{
				try
				{
					flag = base.Character.Physics.Ragdoll.LoadRagdollFromBuffer(base.Character.Model.HavokData);
				}
				catch (Exception)
				{
					base.Character.Physics.CloseRagdoll();
					base.Character.Physics.Ragdoll = null;
				}
			}
			else if (base.Character.Definition.RagdollDataFile != null)
			{
				string text = Path.Combine(MyFileSystem.ContentPath, base.Character.Definition.RagdollDataFile);
				if (File.Exists(text))
				{
					flag = base.Character.Physics.Ragdoll.LoadRagdollFromFile(text);
				}
			}
			if (base.Character.Definition.RagdollRootBody != string.Empty)
			{
				base.Character.Physics.Ragdoll.SetRootBody(base.Character.Definition.RagdollRootBody);
			}
			if (!flag)
			{
				base.Character.Physics.Ragdoll.Dispose();
				base.Character.Physics.Ragdoll = null;
			}
			foreach (HkRigidBody rigidBody in base.Character.Physics.Ragdoll.RigidBodies)
			{
				rigidBody.UserObject = base.Character;
			}
			if (base.Character.Physics.Ragdoll != null && MyPerGameSettings.Destruction)
			{
				base.Character.Physics.Ragdoll.SetToDynamic();
				HkMassProperties properties = default(HkMassProperties);
				foreach (HkRigidBody rigidBody2 in base.Character.Physics.Ragdoll.RigidBodies)
				{
					properties.Mass = MyDestructionHelper.MassToHavok(rigidBody2.Mass);
					properties.InertiaTensor = Matrix.CreateScale(0.04f) * rigidBody2.InertiaTensor;
					rigidBody2.SetMassProperties(ref properties);
				}
				base.Character.Physics.Ragdoll.SetToKeyframed();
			}
			if (base.Character.Physics.Ragdoll != null && MyFakes.ENABLE_RAGDOLL_DEFAULT_PROPERTIES)
			{
				base.Character.Physics.SetRagdollDefaults();
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("RagdollComponent.InitRagdoll - FINISHED");
			}
			return flag;
		}

		public void InitRagdollMapper()
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (base.Character.AnimationController.CharacterBones.Length != 0 && base.Character.Physics != null && base.Character.Physics.Ragdoll != null)
			{
				RagdollMapper = new MyRagdollMapper(base.Character, base.Character.AnimationController);
				RagdollMapper.Init(base.Character.Definition.RagdollBonesMappings);
			}
		}

		private void UpdateRagdoll()
		{
			if (base.Character.Physics != null && base.Character.Physics.Ragdoll != null && RagdollMapper != null && MyPerGameSettings.EnableRagdollModels && !(Distance > MyFakes.ANIMATION_UPDATE_DISTANCE) && RagdollMapper.IsActive && base.Character.Physics.IsRagdollModeActive && (RagdollMapper.IsKeyFramed || RagdollMapper.IsPartiallySimulated))
			{
				RagdollMapper.UpdateRagdollPosition();
				RagdollMapper.SetVelocities(onlyKeyframed: true, onlyIfChanged: true);
				RagdollMapper.SetLimitedVelocities();
				RagdollMapper.DebugDraw(base.Character.WorldMatrix);
			}
		}

		private void ActivateJetpackRagdoll()
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (RagdollMapper == null || base.Character.Physics == null || base.Character.Physics.Ragdoll == null || !MyPerGameSettings.EnableRagdollModels || !MyPerGameSettings.EnableRagdollInJetpack || base.Character.GetPhysicsBody().HavokWorld == null)
			{
				return;
			}
			List<string> list = new List<string>();
			string[] value;
			if (base.Character.CurrentWeapon == null)
			{
				if (base.Character.Definition.RagdollPartialSimulations.TryGetValue("Jetpack", out value))
				{
					list.AddRange(value);
				}
				else
				{
					list.Add("Ragdoll_SE_rig_LUpperarm001");
					list.Add("Ragdoll_SE_rig_LForearm001");
					list.Add("Ragdoll_SE_rig_LPalm001");
					list.Add("Ragdoll_SE_rig_RUpperarm001");
					list.Add("Ragdoll_SE_rig_RForearm001");
					list.Add("Ragdoll_SE_rig_RPalm001");
					list.Add("Ragdoll_SE_rig_LThigh001");
					list.Add("Ragdoll_SE_rig_LCalf001");
					list.Add("Ragdoll_SE_rig_LFoot001");
					list.Add("Ragdoll_SE_rig_RThigh001");
					list.Add("Ragdoll_SE_rig_RCalf001");
					list.Add("Ragdoll_SE_rig_RFoot001");
				}
			}
			else if (base.Character.Definition.RagdollPartialSimulations.TryGetValue("Jetpack_Weapon", out value))
			{
				list.AddRange(value);
			}
			else
			{
				list.Add("Ragdoll_SE_rig_LThigh001");
				list.Add("Ragdoll_SE_rig_LCalf001");
				list.Add("Ragdoll_SE_rig_LFoot001");
				list.Add("Ragdoll_SE_rig_RThigh001");
				list.Add("Ragdoll_SE_rig_RCalf001");
				list.Add("Ragdoll_SE_rig_RFoot001");
			}
			if (base.Character.Physics.Enabled)
			{
				List<int> list2 = new List<int>();
				foreach (string item in list)
				{
					list2.Add(RagdollMapper.BodyIndex(item));
				}
				if (!base.Character.Physics.IsRagdollModeActive)
				{
					base.Character.Physics.SwitchToRagdollMode(deadMode: false);
				}
				if (base.Character.Physics.IsRagdollModeActive)
				{
					RagdollMapper.ActivatePartialSimulation(list2);
				}
				RagdollMapper.SetVelocities();
				if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
				{
					base.Character.Physics.DisableRagdollBodiesCollisions();
				}
			}
		}

		private void DeactivateJetpackRagdoll()
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (RagdollMapper != null && base.Character.Physics != null && base.Character.Physics.Ragdoll != null && MyPerGameSettings.EnableRagdollModels && MyPerGameSettings.EnableRagdollInJetpack)
			{
				if (RagdollMapper.IsPartiallySimulated)
				{
					RagdollMapper.DeactivatePartialSimulation();
				}
				if (base.Character.Physics.IsRagdollModeActive)
				{
					base.Character.Physics.CloseRagdollMode();
				}
			}
		}

		private void SimulateRagdoll()
		{
			if (MyPerGameSettings.EnableRagdollModels && base.Character.Physics != null && RagdollMapper != null && base.Character.Physics.Ragdoll != null && base.Character.Physics.Ragdoll.InWorld && RagdollMapper.IsActive)
			{
				try
				{
					RagdollMapper.UpdateRagdollAfterSimulation();
					if (!base.Character.IsCameraNear)
					{
						_ = MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION;
					}
				}
				finally
				{
				}
			}
		}

		public void InitDeadBodyPhysics()
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			MyPhysicsBody physics = base.Character.Physics;
			if (physics.IsRagdollModeActive)
			{
				physics.CloseRagdollMode();
			}
			MyRagdollMapper ragdollMapper = RagdollMapper;
			if (ragdollMapper.IsActive)
			{
				ragdollMapper.Deactivate();
			}
			physics.SwitchToRagdollMode();
			ragdollMapper.Activate();
			ragdollMapper.SetRagdollToKeyframed();
			ragdollMapper.UpdateRagdollPose();
			ragdollMapper.SetRagdollToDynamic();
		}

		public void UpdateCharacterPhysics()
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			InitRagdoll();
			if (base.Character.Definition.RagdollBonesMappings.Count > 1 && base.Character.Physics.Ragdoll != null)
			{
				InitRagdollMapper();
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && base.Character.IsDead && MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
			{
				RagdollMapper.SyncRigidBodiesTransforms(base.Character.WorldMatrix);
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			UpdateRagdoll();
			if (base.Character.Physics != null && base.Character.Physics.Ragdoll != null && base.Character.Physics.Ragdoll.InWorld && (!base.Character.Physics.Ragdoll.IsKeyframed || RagdollMapper.IsPartiallySimulated) && (IsRagdollMoving || m_gravityTimer > 0))
			{
				Vector3 vector = MyGravityProviderSystem.CalculateTotalGravityInPoint(base.Character.PositionComp.WorldAABB.Center) + base.Character.GetPhysicsBody().HavokWorld.Gravity * MyPerGameSettings.CharacterGravityMultiplier;
				bool isDead = base.Character.IsDead;
				if (isDead)
				{
					foreach (HkRigidBody rigidBody in base.Character.Physics.Ragdoll.RigidBodies)
					{
						if (!rigidBody.IsFixedOrKeyframed)
						{
							rigidBody.ApplyForce(0.0166666675f, vector * rigidBody.Mass);
						}
					}
				}
				else
				{
					vector *= MyFakes.RAGDOLL_GRAVITY_MULTIPLIER;
					Vector3.ClampToSphere(ref vector, 500f);
					foreach (HkRigidBody rigidBody2 in base.Character.Physics.Ragdoll.RigidBodies)
					{
						if (!rigidBody2.IsFixedOrKeyframed)
						{
							rigidBody2.ApplyForce(0.0166666675f, vector);
						}
					}
				}
				if (IsRagdollMoving)
				{
					m_gravityTimer = 300;
					if (isDead)
					{
						m_gravityTimer /= 5;
					}
				}
				else
				{
					m_gravityTimer--;
				}
			}
			if (base.Character.Physics != null && base.Character.Physics.Ragdoll != null && IsRagdollMoving)
			{
				m_lastPosition = base.Character.Physics.Ragdoll.WorldMatrix.Translation;
			}
		}

		public override void Simulate()
		{
			if (!(Distance > MyFakes.ANIMATION_UPDATE_DISTANCE))
			{
				base.Simulate();
				if (!base.Character.IsDead || (RagdollMapper.Ragdoll != null && RagdollMapper.Ragdoll.IsSimulationActive))
				{
					SimulateRagdoll();
				}
				UpdateCharacterBones();
				if (base.Character.Physics != null && base.Character.Physics.Ragdoll != null)
				{
					double num = Vector3D.DistanceSquared(m_lastPosition, base.Character.Physics.Ragdoll.WorldMatrix.Translation);
					IsRagdollMoving = (num > 9.9999997473787516E-05);
				}
				else
				{
					IsRagdollMoving = true;
				}
				CheckChangesOnCharacter();
			}
		}

		private void UpdateCharacterBones()
		{
			if (RagdollMapper != null && RagdollMapper.Ragdoll != null && RagdollMapper.Ragdoll.InWorld)
			{
				RagdollMapper.UpdateCharacterPose(1f, 0f);
				RagdollMapper.DebugDraw(base.Character.WorldMatrix);
				MyCharacterBone[] characterBones = base.Character.AnimationController.CharacterBones;
				for (int i = 0; i < characterBones.Length; i++)
				{
					MyCharacterBone myCharacterBone = characterBones[i];
					myCharacterBone.ComputeBoneTransform();
					base.Character.BoneRelativeTransforms[i] = myCharacterBone.RelativeTransform;
				}
			}
		}

		private void CheckChangesOnCharacter()
		{
			MyCharacter character = base.Character;
			if (MyPerGameSettings.EnableRagdollInJetpack)
			{
				if (character.Physics != m_previousPhysics)
				{
					UpdateCharacterPhysics();
					m_previousPhysics = character.Physics;
				}
				if (!Sync.IsServer && character.ClosestParentId != 0L)
				{
					DeactivateJetpackRagdoll();
				}
				else
				{
					if (character.CurrentWeapon != m_previousWeapon)
					{
						DeactivateJetpackRagdoll();
						ActivateJetpackRagdoll();
						m_previousWeapon = character.CurrentWeapon;
					}
					MyCharacterJetpackComponent jetpackComp = character.JetpackComp;
					MyCharacterMovementEnum currentMovementState = character.GetCurrentMovementState();
					if ((jetpackComp != null && jetpackComp.TurnedOn && currentMovementState == MyCharacterMovementEnum.Flying) || (currentMovementState == MyCharacterMovementEnum.Falling && character.Physics.Enabled))
					{
						if (!IsRagdollActivated || !RagdollMapper.IsActive)
						{
							DeactivateJetpackRagdoll();
							ActivateJetpackRagdoll();
						}
					}
					else if (RagdollMapper != null && RagdollMapper.IsPartiallySimulated)
					{
						DeactivateJetpackRagdoll();
					}
					if (IsRagdollActivated && character.Physics.Ragdoll != null)
					{
						bool isDead = character.IsDead;
						foreach (HkRigidBody rigidBody in character.Physics.Ragdoll.RigidBodies)
						{
							rigidBody.EnableDeactivation = isDead;
						}
					}
				}
			}
			if (character.IsDead && !IsRagdollActivated && character.Physics.Enabled)
			{
				InitDeadBodyPhysics();
			}
		}

		public override void OnAddedToContainer()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || !MyFakes.ENABLE_RAGDOLL)
			{
				base.Container.Remove<MyCharacterRagdollComponent>();
				return;
			}
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			base.OnAddedToContainer();
			base.NeedsUpdateSimulation = true;
			base.NeedsUpdateBeforeSimulation = true;
			base.NeedsUpdateBeforeSimulation100 = true;
			if (base.Character.Physics != null && MyPerGameSettings.EnableRagdollModels && base.Character.Model.HavokData != null && base.Character.Model.HavokData.Length != 0)
			{
				if (InitRagdoll() && base.Character.Definition.RagdollBonesMappings.Count > 1)
				{
					InitRagdollMapper();
				}
			}
			else
			{
				base.Container.Remove<MyCharacterRagdollComponent>();
			}
		}
	}
}
