using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	internal class MyCutsceneCamera : MyEntity, IMyCameraController
	{
		private class Sandbox_Engine_Utils_MyCutsceneCamera_003C_003EActor : IActivator, IActivator<MyCutsceneCamera>
		{
			private sealed override object CreateInstance()
			{
				return new MyCutsceneCamera();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCutsceneCamera CreateInstance()
			{
				return new MyCutsceneCamera();
			}

			MyCutsceneCamera IActivator<MyCutsceneCamera>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float FOV = 70f;

		public MyEntity Entity => this;

		public bool IsInFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool ForceFirstPersonCamera
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool AllowCubeBuilding => false;

		public MyCutsceneCamera()
		{
			base.Init(null);
		}

		public void ControlCamera(MyCamera currentCamera)
		{
			currentCamera.FieldOfViewDegrees = FOV;
			currentCamera.SetViewMatrix(MatrixD.Invert(base.WorldMatrix));
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
		}

		public void RotateStopped()
		{
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
		}

		public bool HandleUse()
		{
			return false;
		}

		public bool HandlePickUp()
		{
			return false;
		}
	}
}
