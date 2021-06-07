using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
	public interface IMyControllableEntity
	{
		IMyControllerInfo ControllerInfo
		{
			get;
		}

		IMyEntity Entity
		{
			get;
		}

		bool ForceFirstPersonCamera
		{
			get;
			set;
		}

		Vector3 LastMotionIndicator
		{
			get;
		}

		Vector3 LastRotationIndicator
		{
			get;
		}

		bool EnabledThrusts
		{
			get;
		}

		bool EnabledDamping
		{
			get;
		}

		bool EnabledLights
		{
			get;
		}

		bool EnabledLeadingGears
		{
			get;
		}

		bool EnabledReactors
		{
			get;
		}

		bool EnabledHelmet
		{
			get;
		}

		bool PrimaryLookaround
		{
			get;
		}

		MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false);

		void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator);

		void MoveAndRotateStopped();

		void Use();

		void UseContinues();

		void PickUp();

		void PickUpContinues();

		void Jump(Vector3 moveindicator = default(Vector3));

		void SwitchWalk();

		void Up();

		void Crouch();

		void Down();

		void ShowInventory();

		void ShowTerminal();

		void SwitchThrusts();

		void SwitchDamping();

		void SwitchLights();

		void SwitchLandingGears();

		void SwitchReactors();

		void SwitchHelmet();

		void DrawHud(IMyCameraController camera, long playerId);

		void Die();
	}
}
