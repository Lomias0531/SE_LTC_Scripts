using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiDetailScreenDefault : MyGuiDetailScreenBase
	{
		public MyGuiDetailScreenDefault(Action<MyGuiControlListbox.Item> callBack, MyGuiControlListbox.Item selectedItem, MyGuiBlueprintScreen parent, string thumbnailTexture, float textScale)
			: base(isTopMostScreen: false, parent, thumbnailTexture, selectedItem, textScale)
		{
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_blueprintName, "bp.sbc");
			base.callBack = callBack;
			if (File.Exists(text))
			{
				m_loadedPrefab = MyBlueprintUtils.LoadPrefab(text);
				if (m_loadedPrefab == null)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("Failed to load the blueprint file.")));
					m_killScreen = true;
				}
				else
				{
					RecreateControls(constructor: true);
				}
			}
			else
			{
				m_killScreen = true;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiDetailScreenDefault";
		}

		protected override void CreateButtons()
		{
			_ = new Vector2(0.215f, -0.173f) + m_offset;
			new Vector2(0.13f, 0f);
		}
	}
}
