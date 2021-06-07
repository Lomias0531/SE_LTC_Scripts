using Sandbox.Engine.Physics;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal class MyControllableSphere : MyEntity, IMyCameraController, IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity
	{
		private class Sandbox_Game_Entities_MyControllableSphere_003C_003EActor : IActivator, IActivator<MyControllableSphere>
		{
			private sealed override object CreateInstance()
			{
				return new MyControllableSphere();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyControllableSphere CreateInstance()
			{
				return new MyControllableSphere();
			}

			MyControllableSphere IActivator<MyControllableSphere>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyControllerInfo m_info = new MyControllerInfo();

		private MyToolbar m_toolbar;

		public MyControllerInfo ControllerInfo => m_info;

		public bool IsInFirstPersonView
		{
			get;
			set;
		}

		public bool EnabledThrusts => false;

		public bool EnabledDamping => false;

		public bool EnabledLights => false;

		public bool EnabledLeadingGears => false;

		public bool EnabledReactors => false;

		public bool EnabledBroadcasting => false;

		public bool EnabledHelmet => false;

		public bool PrimaryLookaround => false;

		public MyEntity Entity => this;

		public bool ForceFirstPersonCamera
		{
			get;
			set;
		}

		public float HeadLocalXAngle
		{
			get;
			set;
		}

		public float HeadLocalYAngle
		{
			get;
			set;
		}

		public MyToolbarType ToolbarType => MyToolbarType.Spectator;

		public MyToolbar Toolbar => m_toolbar;

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return IsInFirstPersonView;
			}
			set
			{
				IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return ForceFirstPersonCamera;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool IMyCameraController.EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		bool IMyCameraController.AllowCubeBuilding => false;

		public MyStringId ControlContext => MyStringId.NullOrEmpty;

		public MyStringId AuxiliaryContext => MyStringId.NullOrEmpty;

		public MyEntity RelativeDampeningEntity
		{
			get;
			set;
		}

		IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		public Vector3 LastMotionIndicator
		{
			get;
			set;
		}

		public Vector3 LastRotationIndicator
		{
			get;
			set;
		}

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		public MyControllableSphere()
		{
			ControllerInfo.ControlAcquired += OnControlAcquired;
			ControllerInfo.ControlReleased += OnControlReleased;
			m_toolbar = new MyToolbar(ToolbarType);
		}

		public void Init()
		{
			base.Init(null, "Models\\Debug\\Sphere", null, null);
			base.WorldMatrix = MatrixD.Identity;
			this.InitSpherePhysics(MyMaterialType.METAL, Vector3.Zero, 0.5f, 100f, MyPerGameSettings.DefaultLinearDamping, MyPerGameSettings.DefaultAngularDamping, 15, RigidBodyFlag.RBF_DEFAULT);
			base.Render.SkipIfTooSmall = false;
			base.Save = false;
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float roll)
		{
			float num = 0.1f;
			Vector3D translation = base.WorldMatrix.Translation + num * base.WorldMatrix.Right * moveIndicator.X + num * base.WorldMatrix.Up * moveIndicator.Y - num * base.WorldMatrix.Forward * moveIndicator.Z;
			MatrixD worldMatrix = GetRotation(rotationIndicator, roll);
			worldMatrix *= base.WorldMatrix;
			worldMatrix.Translation = translation;
			base.WorldMatrix = worldMatrix;
			LastMotionIndicator = moveIndicator;
			LastRotationIndicator = new Vector3(rotationIndicator, roll);
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
		}

		public void MoveAndRotateStopped()
		{
		}

		public void Rotate(Vector2 rotationIndicator, float roll)
		{
			MatrixD matrix = GetRotation(rotationIndicator, roll);
			base.WorldMatrix = matrix * base.WorldMatrix;
		}

		public void RotateStopped()
		{
		}

		private Matrix GetRotation(Vector2 rotationIndicator, float roll)
		{
			float num = 0.001f;
			return Matrix.CreateRotationY((0f - num) * rotationIndicator.Y) * Matrix.CreateRotationX((0f - num) * rotationIndicator.X) * Matrix.CreateRotationZ((0f - num) * roll * 10f);
		}

		public void BeginShoot(MyShootActionEnum action)
		{
		}

		public void OnBeginShoot(MyShootActionEnum action)
		{
		}

		private void ShootInternal()
		{
		}

		private void ShootFailedLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		private void ShootBeginFailed(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		private void ShootSuccessfulLocal(MyShootActionEnum action)
		{
		}

		public void SwitchOnEndShoot(MyDefinitionId? weaponDefinition)
		{
		}

		private void EndShootAll()
		{
		}

		public void EndShoot(MyShootActionEnum action)
		{
		}

		public void OnEndShoot(MyShootActionEnum action)
		{
		}

		public void Zoom(bool newKeyPress)
		{
		}

		private void EnableIronsight(bool enable, bool newKeyPress, bool changeCamera, bool updateSync = true)
		{
		}

		public void Use()
		{
		}

		public void UseContinues()
		{
		}

		public void UseFinished()
		{
		}

		public void PickUp()
		{
		}

		public void PickUpContinues()
		{
		}

		public void PickUpFinished()
		{
		}

		public void Crouch()
		{
		}

		public void Down()
		{
		}

		public void Up()
		{
		}

		public void Jump(Vector3 moveIndicator)
		{
		}

		public void SwitchWalk()
		{
		}

		public void Sprint(bool enabled)
		{
		}

		public void SwitchBroadcasting()
		{
		}

		public void ShowInventory()
		{
		}

		public void ShowTerminal()
		{
		}

		public void SwitchHelmet()
		{
		}

		public void EnableDampeners(bool enable, bool updateSync = true)
		{
		}

		public void EnableJetpack(bool enable, bool fromLoad = false, bool updateSync = true, bool fromInit = false)
		{
		}

		public void SwitchDamping()
		{
		}

		public void SwitchThrusts()
		{
		}

		public void SwitchLights()
		{
		}

		public void SwitchReactors()
		{
		}

		public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition)
		{
			return false;
		}

		public void OnControlAcquired(MyEntityController controller)
		{
		}

		public void OnControlReleased(MyEntityController controller)
		{
		}

		public void Die()
		{
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
		}

		public MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false)
		{
			MatrixD worldMatrix = base.WorldMatrix;
			worldMatrix.Translation -= 4.0 * base.WorldMatrix.Forward;
			return worldMatrix;
		}

		public override MatrixD GetViewMatrix()
		{
			return MatrixD.Invert(GetHeadMatrix(includeY: true));
		}

		public void SwitchToWeapon(MyDefinitionId weaponDefinition)
		{
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
		}

		public void SwitchAmmoMagazine()
		{
		}

		public bool CanSwitchAmmoMagazine()
		{
			return false;
		}

		public void SwitchLandingGears()
		{
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			OnAssumeControl(previousCameraController);
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		public MyEntityCameraSettings GetCameraEntitySettings()
		{
			return null;
		}

		public bool ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			return true;
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(IMyCameraController entity, long player)
		{
			if (entity != null)
			{
				DrawHud(entity, player);
			}
		}
	}
}
