using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[PreloadRequired]
	public class MyGuiIngameScriptsPage : MyGuiScreenDebugBase
	{
		public const string STEAM_THUMBNAIL_NAME = "Textures\\GUI\\Icons\\IngameProgrammingIcon.png";

		public const string THUMBNAIL_NAME = "thumb.png";

		public const string DEFAULT_SCRIPT_NAME = "Script";

		public const string SCRIPTS_DIRECTORY = "IngameScripts";

		public const string SCRIPT_EXTENSION = ".cs";

		public const string WORKSHOP_SCRIPT_EXTENSION = ".bin";

		private static readonly Vector2 SCREEN_SIZE;

		private static readonly float HIDDEN_PART_RIGHT;

		private static Task m_task;

		private static List<MyWorkshopItem> m_subscribedItemsList;

		private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private float m_textScale = 0.8f;

		private MyGuiControlButton m_createFromEditorButton;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_detailsButton;

		private MyGuiControlButton m_deleteButton;

		private MyGuiControlButton m_replaceButton;

		private MyGuiControlTextbox m_searchBox;

		private MyGuiControlButton m_searchClear;

		private static MyGuiControlListbox m_scriptList;

		private MyGuiDetailScreenScriptLocal m_detailScreen;

		private bool m_activeDetail;

		private MyGuiControlListbox.Item m_selectedItem;

		private MyGuiControlRotatingWheel m_wheel;

		private string m_localScriptFolder;

		private string m_workshopFolder;

		private Action OnClose;

		private Action<string> OnScriptOpened;

		private Func<string> GetCodeFromEditor;

		static MyGuiIngameScriptsPage()
		{
			SCREEN_SIZE = new Vector2(0.37f, 1.2f);
			HIDDEN_PART_RIGHT = 0.04f;
			m_subscribedItemsList = new List<MyWorkshopItem>();
			m_scriptList = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.IngameScipts);
		}

		public override string GetFriendlyName()
		{
			return "MyIngameScriptScreen";
		}

		public MyGuiIngameScriptsPage(Action<string> onScriptOpened, Func<string> getCodeFromEditor, Action close)
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			base.EnabledBackgroundFade = true;
			OnClose = close;
			GetCodeFromEditor = getCodeFromEditor;
			OnScriptOpened = onScriptOpened;
			m_localScriptFolder = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local");
			m_workshopFolder = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "workshop");
			if (!Directory.Exists(m_localScriptFolder))
			{
				Directory.CreateDirectory(m_localScriptFolder);
			}
			if (!Directory.Exists(m_workshopFolder))
			{
				Directory.CreateDirectory(m_workshopFolder);
			}
			m_scriptList.Items.Clear();
			GetLocalScriptNames(m_subscribedItemsList.Count == 0);
			RecreateControls(constructor: true);
			m_scriptList.ItemsSelected += OnSelectItem;
			m_scriptList.ItemDoubleClicked += OnItemDoubleClick;
			OnEnterCallback = (Action)Delegate.Combine(OnEnterCallback, new Action(Ok));
			m_canShareInput = false;
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			m_searchBox.TextChanged += OnSearchTextChange;
		}

		private void CreateButtons()
		{
			Vector2 vector = new Vector2(-0.083f, 0.15f);
			Vector2 value = new Vector2(0.134f, 0.038f);
			float num = 0.131f;
			float num2 = 0.265f;
			m_okButton = CreateButton(num, MyTexts.Get(MyCommonTexts.Ok), OnOk, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_NoSelectedScript);
			m_okButton.Position = vector;
			m_okButton.ShowTooltipWhenDisabled = true;
			m_detailsButton = CreateButton(num, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonDetails), OnDetails, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_NoSelectedScript);
			m_detailsButton.Position = vector + new Vector2(1f, 0f) * value;
			m_detailsButton.ShowTooltipWhenDisabled = true;
			m_replaceButton = CreateButton(num2, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonReplaceFromEditor), OnReplaceFromEditor, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_NoSelectedScript);
			m_replaceButton.Position = vector + new Vector2(0f, 1f) * value;
			m_replaceButton.PositionX += value.X / 2f;
			m_replaceButton.ShowTooltipWhenDisabled = true;
			m_deleteButton = CreateButton(num2, MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete), OnDelete, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_NoSelectedScript);
			m_deleteButton.Position = vector + new Vector2(0f, 2f) * value;
			m_deleteButton.PositionX += value.X / 2f;
			m_deleteButton.ShowTooltipWhenDisabled = true;
			vector = new Vector2(-0.083f, 0.305f);
			MyGuiControlButton obj = CreateButton(num2, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonCreateFromEditor), OnCreateFromEditor, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_NewFromEditorTooltip);
			obj.ShowTooltipWhenDisabled = true;
			obj.Position = vector + new Vector2(0f, 0f) * value;
			obj.PositionX += value.X / 2f;
			MyGuiControlButton obj2 = CreateButton(num2, MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenInWorkshop), OnOpenWorkshop, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop);
			obj2.Position = vector + new Vector2(0f, 1f) * value;
			obj2.PositionX += value.X / 2f;
			MyGuiControlButton obj3 = CreateButton(num2, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonRefreshScripts), OnReload, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_RefreshTooltip);
			obj3.Position = vector + new Vector2(0f, 2f) * value;
			obj3.PositionX += value.X / 2f;
			MyGuiControlButton myGuiControlButton = CreateButton(num2, MyTexts.Get(MyCommonTexts.Close), OnCancel, enabled: true, MySpaceTexts.ToolTipNewsletter_Close, m_textScale);
			myGuiControlButton.Position = vector + new Vector2(0f, 3f) * value;
			myGuiControlButton.PositionX += value.X / 2f;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 value = new Vector2(0.02f, SCREEN_SIZE.Y - 1.076f);
			float num = (SCREEN_SIZE.Y - 1f) / 2f;
			AddCaption(MyTexts.Get(MySpaceTexts.ProgrammableBlock_ScriptsScreenTitle).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.123f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.278f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MyTexts.GetString(MyCommonTexts.ScreenCubeBuilderBlockSearch), value + new Vector2(-0.129f, -0.015f), m_textScale);
			myGuiControlLabel.Position = new Vector2(-0.15f, -0.406f);
			m_searchBox = new MyGuiControlTextbox();
			m_searchBox.Position = new Vector2(0.115f, -0.401f);
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_searchBox.Size = new Vector2(0.257f - myGuiControlLabel.Size.X, 0.2f);
			m_searchBox.SetToolTip(MyCommonTexts.Scripts_SearchTooltip);
			m_searchClear = new MyGuiControlButton
			{
				Position = value + new Vector2(0.068f, -0.521f),
				Size = new Vector2(0.045f, 17f / 300f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.Close,
				ActivateOnMouseRelease = true
			};
			m_searchClear.ButtonClicked += OnSearchClear;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(-0.145f, -0.357000023f),
				Name = "ControlLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.Scripts_ListOfScripts)
			};
			MyGuiControlPanel control2 = new MyGuiControlPanel(new Vector2(-0.153499991f, -0.362000018f), new Vector2(0.2685f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_scriptList.Size -= new Vector2(0.5f, 0f);
			m_scriptList.Position = new Vector2(-0.019f, -0.115f);
			m_scriptList.VisibleRowsCount = 12;
			m_scriptList.MultiSelect = false;
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_searchBox);
			Controls.Add(m_searchClear);
			Controls.Add(m_scriptList);
			Controls.Add(control2);
			Controls.Add(control);
			CreateButtons();
			string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds";
			m_wheel = new MyGuiControlRotatingWheel(new Vector2(-0.02f, -0.1f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.28f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, texture, manualRotationUpdate: true, MyPerGameSettings.GUI.MultipleSpinningWheels);
			Controls.Add(m_wheel);
			m_wheel.Visible = false;
		}

		private void GetLocalScriptNames(bool reload = false)
		{
			if (Directory.Exists(m_localScriptFolder))
			{
				string[] directories = Directory.GetDirectories(m_localScriptFolder);
				for (int i = 0; i < directories.Length; i++)
				{
					string fileName = Path.GetFileName(directories[i]);
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL);
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(fileName);
					StringBuilder text = new StringBuilder(fileName);
					object userData = myBlueprintItemInfo;
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, fileName, MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal, userData);
					m_scriptList.Add(item);
				}
				if (m_task.IsComplete && reload)
				{
					GetWorkshopScripts();
				}
				else
				{
					AddWorkshopItemsToList();
				}
			}
		}

		private static void AddWorkshopItemsToList()
		{
			foreach (MyWorkshopItem subscribedItems in m_subscribedItemsList)
			{
				MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, subscribedItems.Id)
				{
					Item = subscribedItems
				};
				myBlueprintItemInfo.SetAdditionalBlueprintInformation(subscribedItems.Title, subscribedItems.Description, subscribedItems.DLCs.ToArray());
				StringBuilder text = new StringBuilder(subscribedItems.Title);
				string title = subscribedItems.Title;
				object userData = myBlueprintItemInfo;
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, title, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Normal, userData);
				m_scriptList.Add(item);
			}
		}

		private void GetScriptsInfo()
		{
			m_subscribedItemsList.Clear();
			bool subscribedIngameScriptsBlocking = MyWorkshop.GetSubscribedIngameScriptsBlocking(m_subscribedItemsList);
			if (subscribedIngameScriptsBlocking)
			{
				if (Directory.Exists(m_workshopFolder))
				{
					try
					{
						Directory.Delete(m_workshopFolder, recursive: true);
					}
					catch (IOException)
					{
					}
				}
				Directory.CreateDirectory(m_workshopFolder);
			}
			if (subscribedIngameScriptsBlocking)
			{
				AddWorkshopItemsToList();
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("Couldn't load scripts from steam workshop"), new StringBuilder("Error")));
			}
		}

		private void GetWorkshopScripts()
		{
			m_task = Parallel.Start(GetScriptsInfo);
		}

		public void RefreshBlueprintList(bool fromTask = false)
		{
			m_scriptList.Items.Clear();
			GetLocalScriptNames(fromTask);
		}

		public void RefreshAndReloadScriptsList(bool refreshWorkshopList = false)
		{
			m_scriptList.Items.Clear();
			GetLocalScriptNames(refreshWorkshopList);
		}

		private void OnSearchClear(MyGuiControlButton button)
		{
			m_searchBox.Text = "";
		}

		private void OnSelectItem(MyGuiControlListbox list)
		{
			if (list.SelectedItems.Count != 0)
			{
				m_selectedItem = list.SelectedItems[0];
				m_detailsButton.Enabled = true;
				MyBlueprintTypeEnum type = (m_selectedItem.UserData as MyBlueprintItemInfo).Type;
				_ = (m_selectedItem.UserData as MyBlueprintItemInfo).PublishedItemId;
				switch (type)
				{
				case MyBlueprintTypeEnum.LOCAL:
					m_okButton.Enabled = true;
					m_detailsButton.Enabled = true;
					m_replaceButton.Enabled = true;
					m_deleteButton.Enabled = true;
					m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_OkTooltip));
					m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_DetailsTooltip));
					m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_ReplaceTooltip));
					m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_DeleteTooltip));
					break;
				case MyBlueprintTypeEnum.STEAM:
					m_okButton.Enabled = true;
					m_detailsButton.Enabled = true;
					m_deleteButton.Enabled = false;
					m_replaceButton.Enabled = false;
					m_deleteButton.SetToolTip(MyTexts.GetString(MyCommonTexts.Scripts_LocalScriptsOnly));
					m_replaceButton.SetToolTip(MyTexts.GetString(MyCommonTexts.Scripts_LocalScriptsOnly));
					break;
				case MyBlueprintTypeEnum.SHARED:
					m_detailsButton.Enabled = false;
					m_deleteButton.Enabled = false;
					break;
				}
			}
		}

		private bool ValidateSelecteditem()
		{
			if (m_selectedItem == null)
			{
				return false;
			}
			if (m_selectedItem.UserData == null)
			{
				return false;
			}
			if (m_selectedItem.Text == null)
			{
				return false;
			}
			return true;
		}

		private void OnSearchTextChange(MyGuiControlTextbox box)
		{
			if (box.Text != "")
			{
				string[] array = box.Text.Split(new char[1]
				{
					' '
				});
				foreach (MyGuiControlListbox.Item item in m_scriptList.Items)
				{
					string text = item.Text.ToString().ToLower();
					bool flag = true;
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!text.Contains(text2.ToLower()))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						item.Visible = true;
					}
					else
					{
						item.Visible = false;
					}
				}
			}
			else
			{
				foreach (MyGuiControlListbox.Item item2 in m_scriptList.Items)
				{
					item2.Visible = true;
				}
			}
		}

		private void OpenSharedScript(MyBlueprintItemInfo itemInfo)
		{
			m_scriptList.Enabled = false;
			m_task = Parallel.Start(DownloadScriptFromSteam, OnScriptDownloaded);
		}

		private void DownloadScriptFromSteam()
		{
			if (m_selectedItem != null)
			{
				MyWorkshop.DownloadScriptBlocking((m_selectedItem.UserData as MyBlueprintItemInfo).Item);
			}
		}

		private void OnScriptDownloaded()
		{
			if (OnScriptOpened != null && m_selectedItem != null)
			{
				MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
				OnScriptOpened(myBlueprintItemInfo.Item.Folder);
			}
			m_scriptList.Enabled = true;
		}

		private void OnItemDoubleClick(MyGuiControlListbox list)
		{
			m_selectedItem = list.SelectedItems[0];
			_ = m_selectedItem.UserData;
			OpenSelectedSript();
		}

		private void Ok()
		{
			if (m_selectedItem == null)
			{
				CloseScreen();
			}
			else
			{
				OpenSelectedSript();
			}
		}

		private void OpenSelectedSript()
		{
			MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
			if (myBlueprintItemInfo.Type == MyBlueprintTypeEnum.STEAM)
			{
				OpenSharedScript(myBlueprintItemInfo);
			}
			else if (OnScriptOpened != null)
			{
				OnScriptOpened(Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local", myBlueprintItemInfo.Data.Name, "Script.cs"));
			}
			CloseScreen();
		}

		private void OnOk(MyGuiControlButton button)
		{
			Ok();
		}

		private void OnCancel(MyGuiControlButton button)
		{
			CloseScreen();
		}

		private void OnReload(MyGuiControlButton button)
		{
			m_selectedItem = null;
			m_okButton.Enabled = false;
			m_detailsButton.Enabled = false;
			m_deleteButton.Enabled = false;
			m_replaceButton.Enabled = false;
			m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
			m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
			m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
			m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
			RefreshAndReloadScriptsList(refreshWorkshopList: true);
		}

		private void OnDetails(MyGuiControlButton button)
		{
			if (m_selectedItem == null)
			{
				if (m_activeDetail)
				{
					MyScreenManager.RemoveScreen(m_detailScreen);
				}
			}
			else if (m_activeDetail)
			{
				MyScreenManager.RemoveScreen(m_detailScreen);
			}
			else
			{
				if (m_activeDetail)
				{
					return;
				}
				if ((m_selectedItem.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.LOCAL)
				{
					if (Directory.Exists(Path.Combine(m_localScriptFolder, m_selectedItem.Text.ToString())))
					{
						m_detailScreen = new MyGuiDetailScreenScriptLocal(delegate(MyBlueprintItemInfo item)
						{
							if (item == null)
							{
								m_okButton.Enabled = false;
								m_detailsButton.Enabled = false;
								m_deleteButton.Enabled = false;
								m_replaceButton.Enabled = false;
							}
							m_activeDetail = false;
							if (m_task.IsComplete)
							{
								RefreshBlueprintList(m_detailScreen.WasPublished);
							}
						}, m_selectedItem.UserData as MyBlueprintItemInfo, this, m_textScale);
						m_activeDetail = true;
						MyScreenManager.InputToNonFocusedScreens = true;
						MyScreenManager.AddScreen(m_detailScreen);
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ScriptNotFound)));
					}
				}
				else if ((m_selectedItem.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.STEAM)
				{
					m_detailScreen = new MyGuiDetailScreenScriptLocal(delegate
					{
						m_activeDetail = false;
						if (m_task.IsComplete)
						{
							RefreshBlueprintList();
						}
					}, m_selectedItem.UserData as MyBlueprintItemInfo, this, m_textScale);
					m_activeDetail = true;
					MyScreenManager.InputToNonFocusedScreens = true;
					MyScreenManager.AddScreen(m_detailScreen);
				}
			}
		}

		private void OnRename(MyGuiControlButton button)
		{
			if (m_selectedItem != null)
			{
				MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), delegate(string result)
				{
					if (result != null)
					{
						ChangeName(result);
					}
				}, caption: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_NewScriptName), defaultName: m_selectedItem.Text.ToString(), maxLenght: 50, textBoxWidth: 0.3f));
			}
		}

		public void ChangeName(string newName)
		{
			newName = MyUtils.StripInvalidChars(newName);
			string oldName = m_selectedItem.Text.ToString();
			string text = Path.Combine(m_localScriptFolder, oldName);
			string newFile = Path.Combine(m_localScriptFolder, newName);
			if (text == newFile || !Directory.Exists(text))
			{
				return;
			}
			if (Directory.Exists(newFile))
			{
				if (text.ToLower() == newFile.ToLower())
				{
					RenameScript(oldName, newName);
					RefreshAndReloadScriptsList();
					return;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogText, newName);
				StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, stringBuilder, messageCaption, null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						Directory.Delete(newFile, recursive: true);
						RenameScript(oldName, newName);
						RefreshAndReloadScriptsList();
					}
				}));
			}
			else
			{
				try
				{
					RenameScript(oldName, newName);
					RefreshAndReloadScriptsList();
				}
				catch (IOException)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameUsed)));
				}
			}
		}

		public void OnDelete(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_DeleteScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && m_selectedItem != null)
				{
					if (DeleteScript(m_selectedItem.Text.ToString()))
					{
						m_okButton.Enabled = false;
						m_detailsButton.Enabled = false;
						m_deleteButton.Enabled = false;
						m_replaceButton.Enabled = false;
						m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
						m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
						m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
						m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
						m_selectedItem = null;
					}
					RefreshBlueprintList();
				}
			}));
		}

		private void RenameScript(string oldName, string newName)
		{
			string text = Path.Combine(m_localScriptFolder, oldName);
			if (Directory.Exists(text))
			{
				string destDirName = Path.Combine(m_localScriptFolder, newName);
				Directory.Move(text, destDirName);
			}
			DeleteScript(oldName);
		}

		private bool DeleteScript(string p)
		{
			string path = Path.Combine(m_localScriptFolder, p);
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
				return true;
			}
			return false;
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			if (m_activeDetail)
			{
				m_detailScreen.CloseScreen();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!m_task.IsComplete)
			{
				m_wheel.Visible = true;
			}
			if (m_task.IsComplete)
			{
				m_wheel.Visible = false;
			}
			return base.Update(hasFocus);
		}

		public override bool CloseScreen()
		{
			if (OnClose != null)
			{
				OnClose();
			}
			return base.CloseScreen();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				CloseScreen();
			}
		}

		public void OnCreateFromEditor(MyGuiControlButton button)
		{
			if (GetCodeFromEditor != null && Directory.Exists(m_localScriptFolder))
			{
				int num = 0;
				while (Directory.Exists(Path.Combine(m_localScriptFolder, "Script_" + num)))
				{
					num++;
				}
				string text = Path.Combine(m_localScriptFolder, "Script_" + num);
				Directory.CreateDirectory(text);
				File.Copy(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Icons\\IngameProgrammingIcon.png"), Path.Combine(text, "thumb.png"), overwrite: true);
				string contents = GetCodeFromEditor();
				File.WriteAllText(Path.Combine(text, "Script.cs"), contents, Encoding.UTF8);
				RefreshAndReloadScriptsList();
			}
		}

		public void OnReplaceFromEditor(MyGuiControlButton button)
		{
			if (m_selectedItem != null && GetCodeFromEditor != null && Directory.Exists(m_localScriptFolder))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
						string path = Path.Combine(m_localScriptFolder, myBlueprintItemInfo.Data.Name, "Script.cs");
						if (File.Exists(path))
						{
							string contents = GetCodeFromEditor();
							File.WriteAllText(path, contents, Encoding.UTF8);
						}
					}
				}));
			}
		}

		private void OnOpenWorkshop(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_SCRIPTS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		protected MyGuiControlButton CreateButton(float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = AddButton(text, onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		protected MyGuiControlLabel MakeLabel(string text, Vector2 position, float textScale = 1f)
		{
			return new MyGuiControlLabel(position, null, text, null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}
	}
}
