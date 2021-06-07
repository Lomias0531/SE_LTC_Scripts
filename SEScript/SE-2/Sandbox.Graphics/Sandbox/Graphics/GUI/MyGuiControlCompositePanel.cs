using VRage.Game;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlCompositePanel))]
	public class MyGuiControlCompositePanel : MyGuiControlPanel
	{
		private float m_innerHeight;

		public float InnerHeight
		{
			get
			{
				return m_innerHeight;
			}
			set
			{
				m_innerHeight = value;
				RefreshInternals();
			}
		}

		public MyGuiControlCompositePanel()
		{
			BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlCompositePanel myObjectBuilder_GuiControlCompositePanel = (MyObjectBuilder_GuiControlCompositePanel)objectBuilder;
			InnerHeight = myObjectBuilder_GuiControlCompositePanel.InnerHeight;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlCompositePanel obj = (MyObjectBuilder_GuiControlCompositePanel)base.GetObjectBuilder();
			obj.InnerHeight = InnerHeight;
			return obj;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			BackgroundTexture.Draw(GetPositionAbsoluteTopLeft(), base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			DrawBorder(transitionAlpha);
		}

		private void RefreshInternals()
		{
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
			base.Size = new Vector2(base.Size.X, base.MinSize.Y + InnerHeight);
		}
	}
}
