using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenLoadSandbox : MyGuiScreenBase
	{
		public static readonly string CONST_THUMB = "//thumb.jpg";

		public static readonly string CONST_BACKUP = "//Backup";

		private MyGuiControlSaveBrowser m_saveBrowser;

		private MyGuiControlButton m_continueLastSave;

		private MyGuiControlButton m_loadButton;

		private MyGuiControlButton m_editButton;

		private MyGuiControlButton m_saveButton;

		private MyGuiControlButton m_deleteButton;

		private MyGuiControlButton m_publishButton;

		private MyGuiControlButton m_subscribedWorldsButton;

		private MyGuiControlButton m_backupsButton;

		private MyGuiControlButton m_backButton;

		private int m_selectedRow;

		private int m_lastSelectedRow;

		private bool m_rowAutoSelect = true;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private MyGuiControlImage m_levelImage;

		private MyGuiControlSearchBox m_searchBox;

		public MyGuiScreenLoadSandbox()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.874f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenMenuButtonLoadGame, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.872f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.872f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.872f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.872f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.25f), m_size.Value.X * 0.2f);
			Controls.Add(myGuiControlSeparatorList3);
			Vector2 value = new Vector2(-0.378f, -0.39f);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			m_searchBox = new MyGuiControlSearchBox(value + new Vector2(minSizeGui.X * 1.1f - 0.004f, 0.017f));
			m_searchBox.OnTextChanged += OnSearchTextChange;
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_searchBox.Size = new Vector2(1075f / MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.848f, 1f);
			Controls.Add(m_searchBox);
			m_saveBrowser = new MyGuiControlSaveBrowser();
			m_saveBrowser.Position = value + new Vector2(minSizeGui.X * 1.1f - 0.004f, 0.055f);
			m_saveBrowser.Size = new Vector2(1075f / MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.848f, 0.15f);
			m_saveBrowser.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_saveBrowser.VisibleRowsCount = 19;
			m_saveBrowser.ItemSelected += OnTableItemSelected;
			m_saveBrowser.ItemDoubleClicked += OnTableItemConfirmedOrDoubleClick;
			m_saveBrowser.ItemConfirmed += OnTableItemConfirmedOrDoubleClick;
			m_saveBrowser.SelectedRowIndex = 0;
			Controls.Add(m_saveBrowser);
			base.FocusedControl = m_saveBrowser;
			Vector2 value2 = value + minSizeGui * 0.5f;
			value2.Y += 0.002f;
			Vector2 mENU_BUTTONS_POSITION_DELTA = MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
			value2.Y += 0.192f;
			Controls.Add(m_editButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * -0.25f, MyCommonTexts.LoadScreenButtonEditSettings, OnEditClick));
			Controls.Add(m_publishButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * 0.75f, MyCommonTexts.LoadScreenButtonPublish, OnPublishClick));
			Controls.Add(m_backupsButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * 1.75f, MyCommonTexts.LoadScreenButtonBackups, OnBackupsButtonClick));
			Controls.Add(m_saveButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * 2.75f, MyCommonTexts.LoadScreenButtonSaveAs, OnSaveAsClick));
			Controls.Add(m_deleteButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * 3.75f, MyCommonTexts.LoadScreenButtonDelete, OnDeleteClick));
			m_backupsButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Backups));
			m_saveButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_SaveAs));
			m_deleteButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Delete));
			m_editButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_EditSettings));
			Vector2 position = value2 + mENU_BUTTONS_POSITION_DELTA * -3.65f;
			position.X -= m_publishButton.Size.X / 2f + 0.0025f;
			m_levelImage = new MyGuiControlImage
			{
				Size = new Vector2(m_publishButton.Size.X, m_publishButton.Size.X / 4f * 3f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			Controls.Add(m_levelImage);
			Controls.Add(m_loadButton = MakeButton(new Vector2(0f, 0f) - new Vector2(-0.295f, (0f - m_size.Value.Y) / 2f + 0.071f), MyCommonTexts.LoadScreenButtonLoad, OnLoadClick));
			m_loadButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Load));
			m_backButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_loadingWheel = new MyGuiControlRotatingWheel(m_loadButton.Position + new Vector2(0.273f, -0.008f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.22f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			m_publishButton.SetToolTip(MyTexts.GetString(MyCommonTexts.LoadScreenButtonTooltipPublish));
			m_loadButton.DrawCrossTextureWhenDisabled = false;
			m_editButton.DrawCrossTextureWhenDisabled = false;
			m_deleteButton.DrawCrossTextureWhenDisabled = false;
			m_saveButton.DrawCrossTextureWhenDisabled = false;
			m_publishButton.DrawCrossTextureWhenDisabled = false;
			base.CloseButtonEnabled = true;
		}

		private void DebugOverrideAutosaveCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
		{
			MySandboxGame.Config.DebugOverrideAutosave = checkbox.IsChecked;
			MySandboxGame.Config.Save();
		}

		private void DebugWorldCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
		{
			string topMostAndCurrentDir = checkbox.IsChecked ? Path.Combine(MyFileSystem.ContentPath, "Worlds") : MyFileSystem.SavesPath;
			m_saveBrowser.SetTopMostAndCurrentDir(topMostAndCurrentDir);
			m_saveBrowser.Refresh();
		}

		private void OnBackupsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			m_saveBrowser.AccessBackups();
		}

		private void OnTableItemConfirmedOrDoubleClick(MyGuiControlTable table, MyGuiControlTable.EventArgs args)
		{
			LoadSandbox();
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(text), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoadSandbox";
		}

		private void OnSearchTextChange(string text)
		{
			m_saveBrowser.SearchTextFilter = text;
			m_saveBrowser.Refresh();
		}

		private void OnLoadClick(MyGuiControlButton sender)
		{
			LoadSandbox();
		}

		private void OnBackClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void OnWorkshopClick(MyGuiControlButton sender)
		{
			MyScreenManager.AddScreen(new MyGuiScreenLoadSubscribedWorld());
		}

		private void OnEditClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(save.Item1, out sizeInBytes);
				if (save != null && !save.Item2.IsCorrupted && myObjectBuilder_Checkpoint != null)
				{
					MySession.FixIncorrectSettings(myObjectBuilder_Checkpoint.Settings);
					MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.EditWorldSettingsScreen, myObjectBuilder_Checkpoint, save.Item1, true, true, true);
					myGuiScreenBase.Closed += delegate
					{
						m_saveBrowser.ForceRefresh();
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase);
					m_rowAutoSelect = true;
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
				}
			}
		}

		private void OnSaveAsClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
				if (save != null)
				{
					MyGuiScreenSaveAs myGuiScreenSaveAs = new MyGuiScreenSaveAs(save.Item2, save.Item1, null);
					myGuiScreenSaveAs.SaveAsConfirm += OnSaveAsConfirm;
					MyGuiSandbox.AddScreen(myGuiScreenSaveAs);
				}
			}
		}

		private void OnSaveAsConfirm()
		{
			m_saveBrowser.ForceRefresh();
		}

		private void OnDeleteClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
			if (save != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, save.Item2.SessionName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnDeleteConfirm));
				return;
			}
			DirectoryInfo directory = m_saveBrowser.GetDirectory(selectedRow);
			if (directory != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, directory.Name), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnDeleteConfirm));
			}
		}

		private void OnDeleteConfirm(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
				if (selectedRow == null)
				{
					return;
				}
				Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
				if (save != null)
				{
					try
					{
						Directory.Delete(save.Item1, recursive: true);
						m_saveBrowser.RemoveSelectedRow();
						m_saveBrowser.SelectedRowIndex = m_selectedRow;
						m_saveBrowser.Refresh();
						m_levelImage.SetTexture();
					}
					catch (Exception)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
					}
				}
				else
				{
					try
					{
						DirectoryInfo directory = m_saveBrowser.GetDirectory(selectedRow);
						if (directory != null)
						{
							directory.Delete(recursive: true);
							m_saveBrowser.Refresh();
						}
					}
					catch (Exception)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
					}
				}
			}
			m_rowAutoSelect = true;
		}

		private void OnContinueLastGameClick(MyGuiControlButton sender)
		{
			MySessionLoader.LoadLastSession();
			m_continueLastSave.Enabled = false;
		}

		private void OnPublishClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
				if (save != null)
				{
					Publish(save.Item1, save.Item2);
				}
			}
		}

		public static void Publish(string sessionPath, MyWorldInfo worlInfo)
		{
			StringBuilder messageText;
			MyStringId id;
			if (worlInfo.WorkshopId.HasValue)
			{
				messageText = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToUpdateWorld), MySession.GameServiceName));
				id = MyCommonTexts.MessageBoxCaptionDoYouWishToUpdateWorld;
			}
			else
			{
				messageText = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToPublishWorld), MySession.GameServiceName, MySession.PlatformLinkAgreement));
				id = MyCommonTexts.MessageBoxCaptionDoYouWishToPublishWorld;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageText, MyTexts.Get(id), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					Action<MyGuiScreenMessageBox.ResultEnum, string[]> action = delegate(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags)
					{
						if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							MyWorkshop.PublishWorldAsync(sessionPath, worlInfo.SessionName, worlInfo.Description, worlInfo.WorkshopId, outTags, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, MyWorkshopItemPublisher publishedFile)
							{
								if (success)
								{
									ulong sizeInBytes;
									MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
									worlInfo.WorkshopId = publishedFile.Id;
									myObjectBuilder_Checkpoint.WorkshopId = publishedFile.Id;
									MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, sessionPath);
									MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublished), MySession.GameServiceName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublished), null, null, null, null, delegate
									{
										MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedFile.Id));
									}));
								}
								else
								{
									StringBuilder messageText2 = (result != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishFailed), MySession.WorkshopServiceName, MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
									MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText2, MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
								}
							});
						}
					};
					if (MyWorkshop.WorldCategories.Length != 0)
					{
						MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("world", MyWorkshop.WorldCategories, null, action));
					}
					else
					{
						action(MyGuiScreenMessageBox.ResultEnum.YES, new string[1]
						{
							"world"
						});
					}
				}
			}));
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			sender.CanHaveFocus = true;
			base.FocusedControl = sender;
			m_selectedRow = eventArgs.RowIndex;
			m_lastSelectedRow = m_selectedRow;
			LoadImagePreview();
		}

		private void LoadImagePreview()
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
			if (save == null || save.Item2.IsCorrupted)
			{
				m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
				return;
			}
			string item = save.Item1;
			if (Directory.Exists(item + CONST_BACKUP))
			{
				string[] directories = Directory.GetDirectories(item + CONST_BACKUP);
				if (directories.Any())
				{
					string text = directories.Last().ToString() + CONST_THUMB;
					if (File.Exists(text) && new FileInfo(text).Length > 0)
					{
						m_levelImage.SetTexture(Directory.GetDirectories(item + CONST_BACKUP).Last().ToString() + CONST_THUMB);
						return;
					}
				}
			}
			string text2 = item + CONST_THUMB;
			if (File.Exists(text2) && new FileInfo(text2).Length > 0)
			{
				m_levelImage.SetTexture();
				m_levelImage.SetTexture(item + CONST_THUMB);
			}
			else
			{
				m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			}
		}

		private void LoadSandbox()
		{
			MyLog.Default.WriteLine("LoadSandbox() - Start");
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> save = m_saveBrowser.GetSave(selectedRow);
				if (m_saveBrowser.GetDirectory(selectedRow) != null)
				{
					return;
				}
				if (save == null || save.Item2.IsCorrupted)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
					return;
				}
				string saveFilePath = save.Item1;
				if (m_saveBrowser.InBackupsFolder)
				{
					CopyBackupUpALevel(ref saveFilePath, save.Item2);
				}
				MySessionLoader.LoadSingleplayerSession(saveFilePath);
			}
			MyLog.Default.WriteLine("LoadSandbox() - End");
		}

		private void CopyBackupUpALevel(ref string saveFilePath, MyWorldInfo worldInfo)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(saveFilePath);
			DirectoryInfo targetDirectory = directoryInfo.Parent.Parent;
			targetDirectory.GetFiles().ForEach(delegate(FileInfo file)
			{
				file.Delete();
			});
			directoryInfo.GetFiles().ForEach(delegate(FileInfo file)
			{
				file.CopyTo(Path.Combine(targetDirectory.FullName, file.Name));
			});
			saveFilePath = targetDirectory.FullName;
		}

		public override bool Update(bool hasFocus)
		{
			if (m_saveBrowser != null && hasFocus && m_saveBrowser.RowsCount != 0 && m_rowAutoSelect)
			{
				if (m_lastSelectedRow < m_saveBrowser.RowsCount)
				{
					m_saveBrowser.SelectedRow = m_saveBrowser.GetRow(m_lastSelectedRow);
					m_selectedRow = m_lastSelectedRow;
				}
				else
				{
					m_saveBrowser.SelectedRow = m_saveBrowser.GetRow(0);
					m_selectedRow = (m_lastSelectedRow = 0);
				}
				m_rowAutoSelect = false;
				m_saveBrowser.ScrollToSelection();
				LoadImagePreview();
			}
			if (m_saveBrowser.GetSave(m_saveBrowser.SelectedRow) != null)
			{
				m_loadButton.Enabled = true;
				m_editButton.Enabled = true;
				m_saveButton.Enabled = true;
				m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
				m_backupsButton.Enabled = true;
			}
			else
			{
				m_loadButton.Enabled = false;
				m_editButton.Enabled = false;
				m_saveButton.Enabled = false;
				m_publishButton.Enabled = false;
				m_backupsButton.Enabled = false;
			}
			m_deleteButton.Enabled = (m_saveBrowser.SelectedRow != null);
			return base.Update(hasFocus);
		}
	}
}
