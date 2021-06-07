using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Cosmetics", "Headlamp")]
	internal class MyGuiScreenDebugHeadlamp : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugHeadlamp";
		}

		public MyGuiScreenDebugHeadlamp()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Headlamp", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition.Y += 0.01f;
			AddLabel("Spot", Color.Yellow.ToVector4(), 1.2f);
			AddColor("Color", MyCharacter.REFLECTOR_COLOR, delegate(MyGuiControlColor v)
			{
				MyCharacter.REFLECTOR_COLOR = v.Color;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Falloff", MyCharacter.REFLECTOR_FALLOFF, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.REFLECTOR_FALLOFF = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Gloss factor", MyCharacter.REFLECTOR_GLOSS_FACTOR, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.REFLECTOR_GLOSS_FACTOR = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Diffuse factor", MyCharacter.REFLECTOR_DIFFUSE_FACTOR, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.REFLECTOR_DIFFUSE_FACTOR = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Intensity", MyCharacter.REFLECTOR_INTENSITY, 0f, 200f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.REFLECTOR_INTENSITY = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Point", Color.Yellow.ToVector4(), 1.2f);
			AddColor("Color", MyCharacter.POINT_COLOR, delegate(MyGuiControlColor v)
			{
				MyCharacter.POINT_COLOR = v.Color;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Falloff", MyCharacter.POINT_FALLOFF, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.POINT_FALLOFF = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Gloss factor", MyCharacter.POINT_GLOSS_FACTOR, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.POINT_GLOSS_FACTOR = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Diffuse factor", MyCharacter.POINT_DIFFUSE_FACTOR, 0f, 5f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.POINT_DIFFUSE_FACTOR = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Intensity", MyCharacter.POINT_LIGHT_INTENSITY, 0f, 50f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.POINT_LIGHT_INTENSITY = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
			AddSlider("Range", MyCharacter.POINT_LIGHT_RANGE, 0f, 10f, delegate(MyGuiControlSlider slider)
			{
				MyCharacter.POINT_LIGHT_RANGE = slider.Value;
				MyCharacter.LIGHT_PARAMETERS_CHANGED = true;
			});
		}
	}
}
