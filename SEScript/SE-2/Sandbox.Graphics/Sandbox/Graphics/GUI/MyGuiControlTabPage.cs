using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlTabPage))]
	public class MyGuiControlTabPage : MyGuiControlParent
	{
		private MyStringId m_textEnum;

		private StringBuilder m_text;

		private int m_pageKey;

		public float TextScale;

		public int PageKey => m_pageKey;

		public MyStringId TextEnum
		{
			get
			{
				return m_textEnum;
			}
			set
			{
				m_textEnum = value;
				m_text = MyTexts.Get(m_textEnum);
			}
		}

		public StringBuilder Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_text = value;
			}
		}

		public bool IsTabVisible
		{
			get;
			set;
		}

		public MyGuiControlTabPage()
			: this(0)
		{
		}

		public MyGuiControlTabPage(int pageKey, Vector2? position = null, Vector2? size = null, Vector4? color = null, float captionTextScale = 1f)
			: base(position, size, color)
		{
			base.Name = "TabPage";
			m_pageKey = pageKey;
			TextScale = captionTextScale;
			IsTabVisible = true;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlTabPage myObjectBuilder_GuiControlTabPage = builder as MyObjectBuilder_GuiControlTabPage;
			m_pageKey = myObjectBuilder_GuiControlTabPage.PageKey;
			TextEnum = MyStringId.GetOrCompute(myObjectBuilder_GuiControlTabPage.TextEnum);
			TextScale = myObjectBuilder_GuiControlTabPage.TextScale;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlTabPage obj = base.GetObjectBuilder() as MyObjectBuilder_GuiControlTabPage;
			obj.PageKey = PageKey;
			obj.TextEnum = TextEnum.ToString();
			obj.TextScale = TextScale;
			return obj;
		}
	}
}
