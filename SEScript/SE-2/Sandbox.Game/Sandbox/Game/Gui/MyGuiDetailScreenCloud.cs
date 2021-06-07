using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiDetailScreenCloud : MyGuiDetailScreenBase
	{
		private MyBlueprintItemInfo m_info;

		public MyGuiDetailScreenCloud(Action<MyGuiControlListbox.Item> callBack, MyGuiControlListbox.Item selectedItem, MyGuiBlueprintScreen parent, string thumbnailTexture, float textScale)
			: base(isTopMostScreen: false, parent, thumbnailTexture, selectedItem, textScale)
		{
			base.callBack = callBack;
			m_info = (selectedItem.UserData as MyBlueprintItemInfo);
			if (m_info == null)
			{
				m_killScreen = true;
				return;
			}
			m_loadedPrefab = MyBlueprintUtils.LoadPrefabFromCloud(m_info);
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

		public override string GetFriendlyName()
		{
			return "MyGuiDetailScreenCloud";
		}

		protected override void CreateButtons()
		{
			Vector2 position = new Vector2(0.148f, -0.197f) + m_offset;
			new Vector2(0.132f, 0.045f);
			float num = 0.13f;
			MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Delete), OnDelete, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_DeleteTooltip).Position = position;
		}

		private void OnDelete(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder("Delete"), messageText: new StringBuilder("Are you sure you want to delete this blueprint?"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					string text = MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY + "/" + m_info.BlueprintName + "/" + MyBlueprintUtils.BLUEPRINT_LOCAL_NAME;
					if (MyGameService.DeleteFromCloud(text))
					{
						text += "B5";
						MyGameService.DeleteFromCloud(text);
					}
					CallResultCallback(null);
					CloseScreen();
				}
			}));
		}

		private void OnPublish(MyGuiControlButton obj)
		{
		}
	}
}
