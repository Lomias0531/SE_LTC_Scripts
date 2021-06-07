using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	internal class MyGuiDetailScreenLocal : MyGuiDetailScreenBase
	{
		private string m_currentLocalDirectory;

		public MyGuiDetailScreenLocal(Action<MyGuiControlListbox.Item> callBack, MyGuiControlListbox.Item selectedItem, MyGuiBlueprintScreenBase parent, string thumbnailTexture, float textScale, string currentLocalDirectory)
			: base(isTopMostScreen: false, parent, thumbnailTexture, selectedItem, textScale)
		{
			m_currentLocalDirectory = currentLocalDirectory;
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, currentLocalDirectory, m_blueprintName, "bp.sbc");
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

		protected override void CreateButtons()
		{
			Vector2 vector = new Vector2(0.148f, -0.197f) + m_offset;
			Vector2 value = new Vector2(0.132f, 0.045f);
			float num = 0.13f;
			MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Rename), OnRename, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_RenameTooltip).Position = vector;
			MyBlueprintUtils.CreateButtonString(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Publish), OnPublish, enabled: true, textScale: m_textScale, tooltip: string.Format(MyTexts.GetString(MyCommonTexts.ToolTipBlueprintPublish), MyGameService.Service.ServiceName)).Position = vector + new Vector2(1f, 0f) * value;
			MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Delete), OnDelete, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_DeleteTooltip).Position = vector + new Vector2(0f, 1f) * value;
			MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenWorkshop), OnOpenWorkshop, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop).Position = vector + new Vector2(1f, 1f) * value;
		}

		public override string GetFriendlyName()
		{
			return "MyDetailScreen";
		}

		private void ChangeDescription(string newDescription)
		{
			if (Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, m_blueprintName)))
			{
				m_loadedPrefab.ShipBlueprints[0].Description = newDescription;
				MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, m_blueprintName, m_currentLocalDirectory, replace: true);
				RefreshDescriptionField();
			}
		}

		private void OnEditDescription(MyGuiControlButton button)
		{
			m_dialog = new MyGuiBlueprintTextDialog(m_position, delegate(string result)
			{
				if (result != null)
				{
					ChangeDescription(result);
				}
			}, m_loadedPrefab.ShipBlueprints[0].Description, "Enter new description", 8000);
			MyScreenManager.AddScreen(m_dialog);
		}

		private void OnDeleteDescription(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder("Delete description"), messageText: new StringBuilder("Are you sure you want to delete the description of this blueprint?"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					ChangeDescription("");
				}
			}));
		}

		private void ChangeName(string name)
		{
			name = MyUtils.StripInvalidChars(name);
			string blueprintName = m_blueprintName;
			string file = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, blueprintName);
			string newFile = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, name);
			if (file == newFile || !Directory.Exists(file))
			{
				return;
			}
			if (Directory.Exists(newFile))
			{
				if (file.ToLower() == newFile.ToLower())
				{
					m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
					m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
					m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, "temp");
					if (Directory.Exists(text))
					{
						Directory.Delete(text, recursive: true);
					}
					Directory.Move(file, text);
					Directory.Move(text, newFile);
					m_thumbnailImage.SetTexture(Path.Combine(newFile, "thumb.png"));
					MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, m_currentLocalDirectory, replace: true);
					m_blueprintName = name;
					RefreshTextField();
					m_parent.RefreshBlueprintList();
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder("Replace"), messageText: new StringBuilder("Blueprint with the name \"" + name + "\" already exists. Do you want to replace it?"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
					{
						if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							string file2 = Path.Combine(m_localRoot, m_currentLocalDirectory, name);
							DeleteItem(file2);
							m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
							m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
							m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
							Directory.Move(file, newFile);
							MyRenderProxy.UnloadTexture(Path.Combine(newFile, "thumb.png"));
							m_thumbnailImage.SetTexture(Path.Combine(newFile, "thumb.png"));
							MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, m_currentLocalDirectory, replace: true);
							m_blueprintName = name;
							RefreshTextField();
							m_parent.RefreshBlueprintList();
						}
					}));
				}
			}
			else
			{
				m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
				m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
				m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
				try
				{
					Directory.Move(file, newFile);
				}
				catch (IOException)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Delete"), messageText: new StringBuilder("Cannot rename blueprint because it is used by another process.")));
					return;
				}
				MyRenderProxy.UnloadTexture(Path.Combine(newFile, "thumb.png"));
				m_thumbnailImage.SetTexture(Path.Combine(newFile, "thumb.png"));
				MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, m_currentLocalDirectory, replace: true);
				m_blueprintName = name;
				RefreshTextField();
				m_parent.RefreshBlueprintList();
			}
		}

		private void OnRename(MyGuiControlButton button)
		{
			m_dialog = new MyGuiBlueprintTextDialog(m_position, delegate(string result)
			{
				if (result != null)
				{
					ChangeName(result);
				}
			}, caption: MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), defaultName: m_blueprintName, maxLenght: maxNameLenght, textBoxWidth: 0.3f);
			MyScreenManager.AddScreen(m_dialog);
		}

		private void OnDelete(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: new StringBuilder("Delete"), messageText: new StringBuilder("Are you sure you want to delete this blueprint?"), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					DeleteItem(Path.Combine(m_localRoot, m_currentLocalDirectory, m_blueprintName));
					CallResultCallback(null);
					CloseScreen();
				}
			}));
		}

		private void OnPublish(MyGuiControlButton button)
		{
			MyBlueprintUtils.PublishBlueprint(m_loadedPrefab, m_blueprintName, m_currentLocalDirectory);
		}

		private void OnOpenWorkshop(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_BLUEPRINTS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			CallResultCallback(m_selectedItem);
			if (m_dialog != null)
			{
				m_dialog.CloseScreen();
			}
		}
	}
}
