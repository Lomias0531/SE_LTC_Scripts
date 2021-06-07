using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "GBuffer Multipliers")]
	internal class MyGuiScreenDebugRenderGBufferMultipliers : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderGBufferMultipliers()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("GBuffer Multipliers", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Multipliers", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Albedo *", MySector.SunProperties.TextureMultipliers.AlbedoMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.AlbedoMultiplier = x.Value;
			});
			AddSlider("Albedo +", MySector.SunProperties.TextureMultipliers.AlbedoShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.AlbedoShift = x.Value;
			});
			AddSlider("Metalness *", MySector.SunProperties.TextureMultipliers.MetalnessMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.MetalnessMultiplier = x.Value;
			});
			AddSlider("Metalness +", MySector.SunProperties.TextureMultipliers.MetalnessShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.MetalnessShift = x.Value;
			});
			AddSlider("Gloss *", MySector.SunProperties.TextureMultipliers.GlossMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.GlossMultiplier = x.Value;
			});
			AddSlider("Gloss +", MySector.SunProperties.TextureMultipliers.GlossShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.GlossShift = x.Value;
			});
			AddSlider("AO *", MySector.SunProperties.TextureMultipliers.AoMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.AoMultiplier = x.Value;
			});
			AddSlider("AO +", MySector.SunProperties.TextureMultipliers.AoShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.AoShift = x.Value;
			});
			AddSlider("Emissive *", MySector.SunProperties.TextureMultipliers.EmissiveMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.EmissiveMultiplier = x.Value;
			});
			AddSlider("Emissive +", MySector.SunProperties.TextureMultipliers.EmissiveShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.EmissiveShift = x.Value;
			});
			AddSlider("Color Mask *", MySector.SunProperties.TextureMultipliers.ColorMaskMultiplier, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.ColorMaskMultiplier = x.Value;
			});
			AddSlider("Color Mask +", MySector.SunProperties.TextureMultipliers.ColorMaskShift, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.ColorMaskShift = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Colorize", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Color Hue", MySector.SunProperties.TextureMultipliers.ColorizeHSV.X, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.ColorizeHSV.X = x.Value;
			});
			AddSlider("Color Saturation", MySector.SunProperties.TextureMultipliers.ColorizeHSV.Y, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.ColorizeHSV.Y = x.Value;
			});
			AddSlider("Color Value", MySector.SunProperties.TextureMultipliers.ColorizeHSV.Z, -1f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.TextureMultipliers.ColorizeHSV.Z = x.Value;
			});
			AddLabel("Glass", Color.Yellow.ToVector4(), 1.2f);
			AddColor("Color", MyRenderProxy.Settings.TransparentColorMultiplier, delegate(MyGuiControlColor v)
			{
				Vector3 vector = v.Color;
				MyRenderProxy.Settings.TransparentColorMultiplier.X = vector.X;
				MyRenderProxy.Settings.TransparentColorMultiplier.Y = vector.Y;
				MyRenderProxy.Settings.TransparentColorMultiplier.Z = vector.Z;
				MyRenderProxy.SetSettingsDirty();
			});
			AddSlider("Alpha", MyRenderProxy.Settings.TransparentColorMultiplier.W, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.TransparentColorMultiplier.W = x.Value;
			});
			AddSlider("Reflectivity", MyRenderProxy.Settings.TransparentReflectivityMultiplier, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.TransparentReflectivityMultiplier = x.Value;
			});
			AddSlider("Fresnel", MyRenderProxy.Settings.TransparentFresnelMultiplier, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.TransparentFresnelMultiplier = x.Value;
			});
			AddSlider("Gloss", MyRenderProxy.Settings.TransparentGlossMultiplier, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.TransparentGlossMultiplier = x.Value;
			});
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderGBufferMultipliers";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
