using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage.Game.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Player Camera")]
	internal class MyGuiScreenDebugPlayerCamera : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugPlayerCamera()
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
				MyCameraSpring cameraSpring = MySector.MainCamera.CameraSpring;
				AddLabel("Camera target spring", Color.Yellow.ToVector4(), 1f);
				AddSlider("Stiffness", 0f, 50f, () => cameraSpring.SpringStiffness, delegate(float s)
				{
					cameraSpring.SpringStiffness = s;
				});
				AddSlider("Dampening", 0f, 1f, () => cameraSpring.SpringDampening, delegate(float s)
				{
					cameraSpring.SpringDampening = s;
				});
				AddSlider("CenterMaxVelocity", 0f, 10f, () => cameraSpring.SpringMaxVelocity, delegate(float s)
				{
					cameraSpring.SpringMaxVelocity = s;
				});
				AddSlider("SpringMaxLength", 0f, 2f, () => cameraSpring.SpringMaxLength, delegate(float s)
				{
					cameraSpring.SpringMaxLength = s;
				});
			}
			m_currentPosition.Y += 0.01f;
			if (MyThirdPersonSpectator.Static != null)
			{
				AddLabel("Third person spectator", Color.Yellow.ToVector4(), 1f);
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Debug draw", () => MyThirdPersonSpectator.Static.EnableDebugDraw, delegate(bool s)
				{
					MyThirdPersonSpectator.Static.EnableDebugDraw = s;
				});
				AddLabel("Normal spring", Color.Yellow.ToVector4(), 0.7f);
				AddSlider("Stiffness", 1f, 50000f, () => MyThirdPersonSpectator.Static.NormalSpring.Stiffness, delegate(float s)
				{
					MyThirdPersonSpectator.Static.NormalSpring.Stiffness = s;
				});
				AddSlider("Damping", 1f, 5000f, () => MyThirdPersonSpectator.Static.NormalSpring.Dampening, delegate(float s)
				{
					MyThirdPersonSpectator.Static.NormalSpring.Dampening = s;
				});
				AddSlider("Mass", 0.1f, 500f, () => MyThirdPersonSpectator.Static.NormalSpring.Mass, delegate(float s)
				{
					MyThirdPersonSpectator.Static.NormalSpring.Mass = s;
				});
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugPlayerShake";
		}
	}
}
