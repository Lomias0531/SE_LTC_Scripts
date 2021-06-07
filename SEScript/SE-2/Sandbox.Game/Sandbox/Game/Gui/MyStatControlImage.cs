using Sandbox.Graphics.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControlImage : MyStatControlBase
	{
		public MyObjectBuilder_GuiTexture Texture
		{
			get;
			set;
		}

		public override void Draw(float transitionAlpha)
		{
			Vector4 colorMask = base.ColorMask;
			bool flag = false;
			base.BlinkBehavior.UpdateBlink();
			if (base.BlinkBehavior.Blink)
			{
				transitionAlpha = MathHelper.Min(transitionAlpha, base.BlinkBehavior.CurrentBlinkAlpha);
				if (base.BlinkBehavior.ColorMask.HasValue)
				{
					base.ColorMask = base.BlinkBehavior.ColorMask.Value;
					flag = true;
				}
			}
			RectangleF destination = new RectangleF(base.Position, base.Size);
			Rectangle? sourceRectangle = null;
			_ = Vector2.Zero;
			MyRenderProxy.DrawSprite(Texture.Path, ref destination, sourceRectangle, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, enabled: true, transitionAlpha), 0f);
			if (flag)
			{
				base.ColorMask = colorMask;
			}
		}

		public MyStatControlImage(MyStatControls parent)
			: base(parent)
		{
		}
	}
}
