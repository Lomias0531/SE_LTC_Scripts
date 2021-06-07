using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Player Shake")]
	internal class MyGuiScreenDebugPlayerShake : MyGuiScreenDebugBase
	{
		private float m_forceShake = 5f;

		public MyGuiScreenDebugPlayerShake()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption("Player Head Shake", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_scale = 0.7f;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MySector.MainCamera != null)
			{
				MyCameraShake cameraShake = MySector.MainCamera.CameraShake;
				AddLabel("Camera shake", Color.Yellow.ToVector4(), 1f);
				AddSlider("MaxShake", 0f, 50f, () => cameraShake.MaxShake, delegate(float s)
				{
					cameraShake.MaxShake = s;
				});
				AddSlider("MaxShakePosX", 0f, 3f, () => cameraShake.MaxShakePosX, delegate(float s)
				{
					cameraShake.MaxShakePosX = s;
				});
				AddSlider("MaxShakePosY", 0f, 3f, () => cameraShake.MaxShakePosY, delegate(float s)
				{
					cameraShake.MaxShakePosY = s;
				});
				AddSlider("MaxShakePosZ", 0f, 3f, () => cameraShake.MaxShakePosZ, delegate(float s)
				{
					cameraShake.MaxShakePosZ = s;
				});
				AddSlider("MaxShakeDir", 0f, 1f, () => cameraShake.MaxShakeDir, delegate(float s)
				{
					cameraShake.MaxShakeDir = s;
				});
				AddSlider("Reduction", 0f, 1f, () => cameraShake.Reduction, delegate(float s)
				{
					cameraShake.Reduction = s;
				});
				AddSlider("Dampening", 0f, 1f, () => cameraShake.Dampening, delegate(float s)
				{
					cameraShake.Dampening = s;
				});
				AddSlider("OffConstant", 0f, 1f, () => cameraShake.OffConstant, delegate(float s)
				{
					cameraShake.OffConstant = s;
				});
				AddSlider("DirReduction", 0f, 2f, () => cameraShake.DirReduction, delegate(float s)
				{
					cameraShake.DirReduction = s;
				});
				m_currentPosition.Y += 0.01f;
				AddLabel("Maximum shakes", Color.Yellow.ToVector4(), 1f);
				AddSlider("Character damage", 0f, 5000f, () => MyCharacter.MAX_SHAKE_DAMAGE, delegate(float s)
				{
					MyCharacter.MAX_SHAKE_DAMAGE = s;
				});
				AddSlider("Grid damage", 0f, 5000f, () => MyCockpit.MAX_SHAKE_DAMAGE, delegate(float s)
				{
					MyCockpit.MAX_SHAKE_DAMAGE = s;
				});
				AddSlider("Explosion shake time", 0f, 5000f, () => MyExplosionsConstants.CAMERA_SHAKE_TIME_MS, delegate(float s)
				{
					MyExplosionsConstants.CAMERA_SHAKE_TIME_MS = s;
				});
				AddSlider("Grinder max shake", 0f, 50f, () => MyAngleGrinder.GRINDER_MAX_SHAKE, delegate(float s)
				{
					MyAngleGrinder.GRINDER_MAX_SHAKE = s;
				});
				AddSlider("Rifle max shake", 0f, 50f, () => MyAutomaticRifleGun.RIFLE_MAX_SHAKE, delegate(float s)
				{
					MyAutomaticRifleGun.RIFLE_MAX_SHAKE = s;
				});
				AddSlider("Rifle FOV shake", 0f, 1f, () => MyAutomaticRifleGun.RIFLE_FOV_SHAKE, delegate(float s)
				{
					MyAutomaticRifleGun.RIFLE_FOV_SHAKE = s;
				});
				AddSlider("Drill max shake", 0f, 50f, () => MyDrillBase.DRILL_MAX_SHAKE, delegate(float s)
				{
					MyDrillBase.DRILL_MAX_SHAKE = s;
				});
				m_currentPosition.Y += 0.01f;
				AddLabel("Testing", Color.Yellow.ToVector4(), 1f);
				AddSlider("Shake", 0f, 50f, () => m_forceShake, delegate(float s)
				{
					m_forceShake = s;
				});
				AddButton("Force shake", OnForceShakeClick);
			}
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugPlayerShake";
		}

		private void OnForceShakeClick(MyGuiControlButton button)
		{
			if (MySector.MainCamera != null)
			{
				MySector.MainCamera.CameraShake.AddShake(m_forceShake);
			}
		}
	}
}
