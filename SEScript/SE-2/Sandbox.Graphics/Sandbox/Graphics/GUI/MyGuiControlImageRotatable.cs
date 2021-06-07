using System;
using VRage.Utils;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlImageRotatable : MyGuiControlImage
	{
		public float Rotation
		{
			get;
			set;
		}

		public float Scale => Math.Max(base.Size.X, base.Size.Y);

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			DrawBackground(backgroundTransitionAlpha);
			if (base.Textures != null)
			{
				for (int i = 0; i < base.Textures.Length; i++)
				{
					MyGuiManager.DrawSpriteBatch(base.Textures[i].Texture, GetPositionAbsoluteTopLeft() + m_padding.TopLeftOffset / MyGuiConstants.GUI_OPTIMAL_SIZE, base.Size - m_padding.SizeChange / MyGuiConstants.GUI_OPTIMAL_SIZE, MyGuiControlBase.ApplyColorMaskModifiers(base.Textures[i].ColorMask.HasValue ? base.Textures[i].ColorMask.Value : base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false, base.Textures[i].MaskTexture, Rotation);
				}
			}
			DrawElements(transitionAlpha, backgroundTransitionAlpha);
			DrawBorder(transitionAlpha);
		}
	}
}
