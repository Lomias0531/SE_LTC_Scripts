using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlPanel))]
	public class MyGuiControlPanel : MyGuiControlBase
	{
		public MyGuiControlPanel()
			: this(null, null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
		{
		}

		public MyGuiControlPanel(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string texture = null, string toolTip = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			: base(position, size, backgroundColor, toolTip, new MyGuiCompositeTexture
			{
				Center = new MyGuiSizedTexture
				{
					Texture = texture
				}
			}, isActiveControl: false, canHaveFocus: false, MyGuiControlHighlightType.NEVER, originAlign)
		{
			base.Visible = true;
		}
	}
}
