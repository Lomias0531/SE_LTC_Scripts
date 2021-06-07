using Sandbox.Game.Components;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Cosmetics", "Jetpack visual")]
	internal class MyGuiScreenDebugJetpack : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugJetpack";
		}

		public MyGuiScreenDebugJetpack()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Jetpack visual", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition.Y += 0.01f;
			AddLabel("Light", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity const", MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_BASE, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_BASE = slider.Value;
			});
			AddSlider("Intensity from thrust length", MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_LENGTH, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_LENGTH = slider.Value;
			});
			AddSlider("Range from thrust radius", MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_RADIUS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_RADIUS = slider.Value;
			});
			AddSlider("Range from thrust length", MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_LENGTH = slider.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Glare", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity const", MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_BASE, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_BASE = slider.Value;
			});
			AddSlider("Intensity from thrust length", MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_LENGTH, 0f, 100f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_LENGTH = slider.Value;
			});
			AddSlider("Size from thrust radius", MyRenderComponentCharacter.JETPACK_GLARE_SIZE_RADIUS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_GLARE_SIZE_RADIUS = slider.Value;
			});
			AddSlider("Size from thrust length", MyRenderComponentCharacter.JETPACK_GLARE_SIZE_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_GLARE_SIZE_LENGTH = slider.Value;
			});
			m_currentPosition.Y += 0.02f;
			AddLabel("Thrust", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Intensity base", MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE = slider.Value;
			});
			AddSlider("Intensity", MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY = slider.Value;
			});
			AddSlider("Radius", MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS = slider.Value;
			});
			AddSlider("Length", MyRenderComponentCharacter.JETPACK_THRUST_LENGTH, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_THRUST_LENGTH = slider.Value;
			});
			AddSlider("Offset", MyRenderComponentCharacter.JETPACK_THRUST_OFFSET, -1f, 1f, delegate(MyGuiControlSlider slider)
			{
				MyRenderComponentCharacter.JETPACK_THRUST_OFFSET = slider.Value;
			});
			m_currentPosition.Y += 0.02f;
		}
	}
}
