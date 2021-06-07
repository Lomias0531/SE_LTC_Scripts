using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using Sandbox.RenderDirect.ActorComponents;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Cosmetics", "Thrusts visual")]
	internal class MyGuiScreenDebugThrusts : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugThrusts";
		}

		public MyGuiScreenDebugThrusts()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Thrusts visual", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition.Y += 0.01f;
			AddLabel("Thrust Light", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity const", MyThrustFlameAnimator.LIGHT_INTENSITY_BASE, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.LIGHT_INTENSITY_BASE = slider.Value;
			});
			AddSlider("Intensity from thrust length", MyThrustFlameAnimator.LIGHT_INTENSITY_LENGTH, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.LIGHT_INTENSITY_LENGTH = slider.Value;
			});
			AddSlider("Range from thrust radius", MyThrustFlameAnimator.LIGHT_RANGE_RADIUS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.LIGHT_RANGE_RADIUS = slider.Value;
			});
			AddSlider("Range from thrust length", MyThrustFlameAnimator.LIGHT_RANGE_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.LIGHT_RANGE_LENGTH = slider.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Thrust Glare", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity const", MyThrustFlameAnimator.GLARE_INTENSITY_BASE, 0f, 3f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.GLARE_INTENSITY_BASE = slider.Value;
			});
			AddSlider("Intensity from thrust length", MyThrustFlameAnimator.GLARE_INTENSITY_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.GLARE_INTENSITY_LENGTH = slider.Value;
			});
			AddSlider("Size from thrust radius", MyThrustFlameAnimator.GLARE_SIZE_RADIUS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.GLARE_SIZE_RADIUS = slider.Value;
			});
			AddSlider("Size from thrust length", MyThrustFlameAnimator.GLARE_SIZE_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.GLARE_SIZE_LENGTH = slider.Value;
			});
			AddLabel("Thrust", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity", MyThrustFlameAnimator.THRUST_INTENSITY, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.THRUST_INTENSITY = slider.Value;
			});
			AddSlider("Intensity from thrust length", MyThrustFlameAnimator.THRUST_LENGTH_INTENSITY, 0f, 2f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.THRUST_LENGTH_INTENSITY = slider.Value;
			});
			AddSlider("Radius", MyThrustFlameAnimator.THRUST_THICKNESS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyThrustFlameAnimator.THRUST_THICKNESS = slider.Value;
			});
			m_currentPosition.Y += 0.02f;
		}
	}
}
