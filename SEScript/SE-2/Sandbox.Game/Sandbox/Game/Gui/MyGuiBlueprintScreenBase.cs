using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System.IO;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public abstract class MyGuiBlueprintScreenBase : MyGuiScreenDebugBase
	{
		protected string m_localRoot = string.Empty;

		public MyGuiBlueprintScreenBase(Vector2 position, Vector2 size, Vector4 backgroundColor, bool isTopMostScreen)
			: base(position, size, backgroundColor, isTopMostScreen)
		{
			m_localRoot = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
			m_canShareInput = false;
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
		}

		protected MyGuiControlCompositePanel AddCompositePanel(MyGuiCompositeTexture texture, Vector2 position, Vector2 size, MyGuiDrawAlignEnum panelAlign)
		{
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = texture
			};
			myGuiControlCompositePanel.Position = position;
			myGuiControlCompositePanel.Size = size;
			myGuiControlCompositePanel.OriginAlign = panelAlign;
			Controls.Add(myGuiControlCompositePanel);
			return myGuiControlCompositePanel;
		}

		protected MyGuiControlLabel MakeLabel(string text, Vector2 position, float textScale = 1f)
		{
			return new MyGuiControlLabel(position, null, text, null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		protected bool DeleteItem(string file)
		{
			if (Directory.Exists(file))
			{
				Directory.Delete(file, recursive: true);
				return true;
			}
			return false;
		}

		public override bool CloseScreen()
		{
			return base.CloseScreen();
		}

		public virtual void RefreshBlueprintList(bool fromTask = false)
		{
		}
	}
}
