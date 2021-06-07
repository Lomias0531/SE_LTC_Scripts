using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public abstract class MyEntityRespawnComponentBase : MyEntityComponentBase, IMyCameraController, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity
	{
		private static List<MyPhysics.HitInfo> m_raycastList;

		public new MyEntity Entity => (MyEntity)base.Entity;

		public Vector3 LastMotionIndicator => Vector3D.Zero;

		public Vector3 LastRotationIndicator => Vector3.Zero;

		bool IMyCameraController.AllowCubeBuilding => false;

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyCameraController.EnableFirstPersonView
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		MyEntity Sandbox.Game.Entities.IMyControllableEntity.Entity => Entity;

		float Sandbox.Game.Entities.IMyControllableEntity.HeadLocalXAngle
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		float Sandbox.Game.Entities.IMyControllableEntity.HeadLocalYAngle
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		bool Sandbox.Game.Entities.IMyControllableEntity.EnabledBroadcasting => false;

		MyToolbarType Sandbox.Game.Entities.IMyControllableEntity.ToolbarType => MyToolbarType.None;

		MyStringId Sandbox.Game.Entities.IMyControllableEntity.ControlContext => MyStringId.NullOrEmpty;

		public MyStringId AuxiliaryContext => MyStringId.NullOrEmpty;

		MyToolbar Sandbox.Game.Entities.IMyControllableEntity.Toolbar => null;

		MyEntity Sandbox.Game.Entities.IMyControllableEntity.RelativeDampeningEntity
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		MyControllerInfo Sandbox.Game.Entities.IMyControllableEntity.ControllerInfo => null;

		IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => null;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PrimaryLookaround => false;

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			if (MySession.Static.ControlledEntity == null)
			{
				MyThirdPersonSpectator.Static.Update();
				MyThirdPersonSpectator.Static.UpdateZoom();
			}
			if (!MyThirdPersonSpectator.Static.IsCameraForced())
			{
				MyPhysicsComponentBase physics = Entity.Physics;
				MatrixD viewMatrix = MyThirdPersonSpectator.Static.GetViewMatrix();
				currentCamera.SetViewMatrix(viewMatrix);
				currentCamera.CameraSpring.Enabled = false;
				currentCamera.CameraSpring.SetCurrentCameraControllerVelocity(physics?.LinearVelocity ?? Vector3.Zero);
			}
			else
			{
				MatrixD worldMatrix = Entity.PositionComp.WorldMatrix;
				Vector3D translation = worldMatrix.Translation;
				Vector3D vector3D = translation + (worldMatrix.Up + worldMatrix.Right + worldMatrix.Forward) * 20.0;
				using (MyUtils.ReuseCollection(ref m_raycastList))
				{
					MyPhysics.CastRay(translation, vector3D, m_raycastList);
					float num = 1f;
					foreach (MyPhysics.HitInfo raycast in m_raycastList)
					{
						IMyEntity hitEntity = raycast.HkHitInfo.GetHitEntity();
						if (hitEntity != Entity && !(hitEntity is MyFloatingObject) && !(hitEntity is MyCharacter) && raycast.HkHitInfo.HitFraction < num)
						{
							num = Math.Max(0.1f, raycast.HkHitInfo.HitFraction - 0.1f);
						}
					}
					vector3D = translation + (vector3D - translation) * num;
				}
				MatrixD viewMatrix2 = MatrixD.CreateLookAt(vector3D, translation, worldMatrix.Up);
				currentCamera.SetViewMatrix(viewMatrix2);
			}
		}

		MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(bool includeY, bool includeX, bool forceHeadAnim, bool forceHeadBone)
		{
			MatrixD worldMatrix = Entity.PositionComp.WorldMatrix;
			Vector3D translation = worldMatrix.Translation;
			return MatrixD.Invert(MatrixD.Normalize(MatrixD.CreateLookAt(translation + worldMatrix.Right + worldMatrix.Forward + worldMatrix.Up, translation, worldMatrix.Up)));
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
		}

		void IMyCameraController.RotateStopped()
		{
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		void Sandbox.Game.Entities.IMyControllableEntity.BeginShoot(MyShootActionEnum action)
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.EndShoot(MyShootActionEnum action)
		{
		}

		bool Sandbox.Game.Entities.IMyControllableEntity.ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			return true;
		}

		void Sandbox.Game.Entities.IMyControllableEntity.OnBeginShoot(MyShootActionEnum action)
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.OnEndShoot(MyShootActionEnum action)
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.UseFinished()
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.PickUpFinished()
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.Sprint(bool enabled)
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.SwitchToWeapon(MyDefinitionId weaponDefinition)
		{
		}

		void Sandbox.Game.Entities.IMyControllableEntity.SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
		}

		bool Sandbox.Game.Entities.IMyControllableEntity.CanSwitchToWeapon(MyDefinitionId? weaponDefinition)
		{
			return false;
		}

		void Sandbox.Game.Entities.IMyControllableEntity.SwitchAmmoMagazine()
		{
		}

		bool Sandbox.Game.Entities.IMyControllableEntity.CanSwitchAmmoMagazine()
		{
			return false;
		}

		void Sandbox.Game.Entities.IMyControllableEntity.SwitchBroadcasting()
		{
		}

		MyEntityCameraSettings Sandbox.Game.Entities.IMyControllableEntity.GetCameraEntitySettings()
		{
			return new MyEntityCameraSettings();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(Vector3 moveIndicator)
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchWalk()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(IMyCameraController camera, long playerId)
		{
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die()
		{
		}
	}
}
