using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Graphics.Gui
{
	public sealed class MyGuiControlSolidColor : MyGuiControlBase
	{
		public Color Color
		{
			get;
			set;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (base.Visible)
			{
				Color color = MyGuiControlBase.ApplyColorMaskModifiers(Color.ToVector4(), base.Enabled, transitionAlpha);
				MyGuiManager.DrawRectangle(GetPositionAbsoluteTopLeft(), base.Size, color);
			}
		}
	}
}
