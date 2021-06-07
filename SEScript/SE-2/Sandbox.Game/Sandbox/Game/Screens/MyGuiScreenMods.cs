using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenMods : MyGuiScreenBase
	{
		private class ModDependenciesWorkData : WorkData
		{
			public ulong ParentId;

			public MyGuiControlTable.Row ParentModRow;

			public List<MyWorkshopItem> Dependencies;

			public bool HasReferenceIssue;
		}

		private MyGuiControlTable m_modsTableEnabled;

		private MyGuiControlTable m_modsTableDisabled;

		private MyGuiControlButton m_moveUpButton;

		private MyGuiControlButton m_moveDownButton;

		private MyGuiControlButton m_moveTopButton;

		private MyGuiControlButton m_moveBottomButton;

		private MyGuiControlButton m_moveLeftButton;

		private MyGuiControlButton m_moveLeftAllButton;

		private MyGuiControlButton m_moveRightButton;

		private MyGuiControlButton m_moveRightAllButton;

		private MyGuiControlButton m_openInWorkshopButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_browseWorkshopButton;

		private MyGuiControlButton m_publishModButton;

		private MyGuiControlButton m_okButton;

		private MyGuiControlTable.Row m_selectedRow;

		private MyGuiControlTable m_selectedTable;

		private bool m_listNeedsReload;

		private bool m_keepActiveMods;

		private List<MyWorkshopItem> m_subscribedMods;

		private List<MyWorkshopItem> m_worldMods;

		private List<MyObjectBuilder_Checkpoint.ModItem> m_modListToEdit;

		private MyObjectBuilder_Checkpoint.ModItem m_selectedMod;

		private HashSet<string> m_worldLocalMods = new HashSet<string>();

		private HashSet<ulong> m_worldWorkshopMods = new HashSet<ulong>();

		private MyGuiControlButton m_categoryCategorySelectButton;

		private List<MyGuiControlButton> m_categoryButtonList = new List<MyGuiControlButton>();

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlButton m_searchClear;

		private List<string> m_tmpSearch = new List<string>();

		private List<string> m_selectedCategories = new List<string>();

		private Dictionary<ulong, StringBuilder> m_modsToolTips = new Dictionary<ulong, StringBuilder>();

		public MyGuiScreenMods(List<MyObjectBuilder_Checkpoint.ModItem> modListToEdit)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1.015f, 0.934f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_modListToEdit = modListToEdit;
			if (m_modListToEdit == null)
			{
				m_modListToEdit = new List<MyObjectBuilder_Checkpoint.ModItem>();
			}
			base.EnabledBackgroundFade = true;
			RefreshWorldMods(m_modListToEdit);
			m_listNeedsReload = true;
			RecreateControls(constructor: true);
		}

		private void RefreshWorldMods(ListReader<MyObjectBuilder_Checkpoint.ModItem> mods)
		{
			m_worldLocalMods.Clear();
			m_worldWorkshopMods.Clear();
			foreach (MyObjectBuilder_Checkpoint.ModItem item in mods)
			{
				if (item.PublishedFileId == 0L)
				{
					m_worldLocalMods.Add(item.Name);
				}
				else
				{
					m_worldWorkshopMods.Add(item.PublishedFileId);
				}
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionWorkshop, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList2);
			Vector2 vector = new Vector2(-0.454f, -0.417f);
			Vector2 value = new Vector2(-0f, -4.75f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA.Y);
			m_modsTableDisabled = new MyGuiControlTable();
			m_modsTableEnabled = new MyGuiControlTable();
			if (MyFakes.ENABLE_MOD_CATEGORIES)
			{
				m_modsTableDisabled.Position = vector + new Vector2(0f, 0.1f);
				m_modsTableDisabled.VisibleRowsCount = 18;
				m_modsTableEnabled.VisibleRowsCount = 18;
			}
			else
			{
				m_modsTableDisabled.Position = vector;
				m_modsTableDisabled.VisibleRowsCount = 20;
				m_modsTableEnabled.VisibleRowsCount = 20;
			}
			m_modsTableDisabled.Size = new Vector2(m_size.Value.X * 0.428f, 1f);
			m_modsTableDisabled.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_modsTableDisabled.ColumnsCount = 2;
			m_modsTableDisabled.ItemSelected += OnTableItemSelected;
			m_modsTableDisabled.ItemDoubleClicked += OnTableItemConfirmedOrDoubleClick;
			m_modsTableDisabled.ItemConfirmed += OnTableItemConfirmedOrDoubleClick;
			m_modsTableDisabled.SetCustomColumnWidths(new float[2]
			{
				0.085f,
				0.905f
			});
			m_modsTableDisabled.SetColumnComparison(1, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			Controls.Add(m_modsTableDisabled);
			m_modsTableEnabled.Position = vector + new Vector2(m_modsTableDisabled.Size.X + 0.04f, 0.1f);
			m_modsTableEnabled.Size = new Vector2(m_size.Value.X * 0.428f, 1f);
			m_modsTableEnabled.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_modsTableEnabled.ColumnsCount = 2;
			m_modsTableEnabled.ItemSelected += OnTableItemSelected;
			m_modsTableEnabled.ItemDoubleClicked += OnTableItemConfirmedOrDoubleClick;
			m_modsTableEnabled.ItemConfirmed += OnTableItemConfirmedOrDoubleClick;
			m_modsTableEnabled.SetCustomColumnWidths(new float[2]
			{
				0.085f,
				0.905f
			});
			m_modsTableEnabled.SetColumnComparison(1, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			Controls.Add(m_modsTableEnabled);
			Controls.Add(m_moveRightAllButton = MakeButtonTiny(value + 0f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0f, MyCommonTexts.ToolTipScreenMods_MoveRightAll, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnMoveRightAllClick));
			Controls.Add(m_moveRightButton = MakeButtonTiny(value + 1f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0f, MyCommonTexts.ToolTipScreenMods_MoveRight, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnMoveRightClick));
			Controls.Add(m_moveUpButton = MakeButtonTiny(value + 2.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MathF.E * -449f / 777f, MyCommonTexts.ToolTipScreenMods_MoveUp, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnMoveUpClick));
			Controls.Add(m_moveTopButton = MakeButtonTiny(value + 3.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MathF.E * -449f / 777f, MyCommonTexts.ToolTipScreenMods_MoveTop, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnMoveTopClick));
			Controls.Add(m_moveBottomButton = MakeButtonTiny(value + 4.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MathF.E * 449f / 777f, MyCommonTexts.ToolTipScreenMods_MoveBottom, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnMoveBottomClick));
			Controls.Add(m_moveDownButton = MakeButtonTiny(value + 5.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MathF.E * 449f / 777f, MyCommonTexts.ToolTipScreenMods_MoveDown, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnMoveDownClick));
			Controls.Add(m_moveLeftButton = MakeButtonTiny(value + 7f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyCommonTexts.ToolTipScreenMods_MoveLeft, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, OnMoveLeftClick));
			Controls.Add(m_moveLeftAllButton = MakeButtonTiny(value + 8f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyCommonTexts.ToolTipScreenMods_MoveLeftAll, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, OnMoveLeftAllClick));
			float num = 0.0075f;
			Controls.Add(m_okButton = MakeButton(new Vector2(m_modsTableDisabled.Position.X + 0.002f, 0f) - new Vector2(0f, (0f - m_size.Value.Y) / 2f + 0.097f), MyCommonTexts.Ok, MyCommonTexts.Ok, OnOkClick));
			Controls.Add(m_refreshButton = MakeButton(m_okButton.Position + new Vector2(m_okButton.Size.X + num, 0f), MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ToolTipWorkshopRefreshMod, OnRefreshClick));
			Controls.Add(m_browseWorkshopButton = MakeButton(m_okButton.Position + new Vector2(m_okButton.Size.X * 2f + num * 2f, 0f), MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop, string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopBrowseWorkshop), MySession.GameServiceName), OnBrowseWorkshopClick));
			Controls.Add(m_openInWorkshopButton = MakeButton(m_okButton.Position + new Vector2(m_okButton.Size.X * 3f + num * 3f, 0f), MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop, string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopPublish), MyGameService.Service.ServiceName), OnOpenInWorkshopClick));
			Controls.Add(m_publishModButton = MakeButton(m_okButton.Position + new Vector2(m_okButton.Size.X * 4f + num * 4f, 0f), MyCommonTexts.LoadScreenButtonPublish, MyCommonTexts.LoadScreenButtonPublish, OnPublishModClick));
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipMods_Ok));
			if (MyFakes.ENABLE_MOD_CATEGORIES)
			{
				Vector2 value2 = m_modsTableDisabled.Position + new Vector2(0.015f, -0.036f);
				Vector2 value3 = new Vector2(0.0335f, 0f);
				MyWorkshop.Category[] modCategories = MyWorkshop.ModCategories;
				int i;
				for (i = 0; i < modCategories.Length; i++)
				{
					if (modCategories[i].IsVisibleForFilter)
					{
						Controls.Add(MakeButtonCategory(value2 + value3 * i, modCategories[i]));
					}
				}
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton
				{
					Position = value2 + value3 * i + new Vector2(-0.013f, -0.02f),
					Size = new Vector2(0.03f, 0.05f),
					Name = "SelectCategory",
					Text = "...",
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					VisualStyle = MyGuiControlButtonStyleEnum.Square48
				};
				myGuiControlButton.SetToolTip(MyCommonTexts.TooltipScreenMods_SelectCategories);
				myGuiControlButton.ButtonClicked += OnSelectCategoryClicked;
				Controls.Add(myGuiControlButton);
				Vector2 value4 = new Vector2(m_modsTableEnabled.Position.X, 0f) - new Vector2(0f, m_size.Value.Y / 2f - 0.099f);
				m_searchBox = new MyGuiControlSearchBox(value4 + new Vector2(0f, 0.013f));
				m_searchBox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipMods_Search));
				m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_searchBox.Size = new Vector2(m_modsTableEnabled.Size.X, 0.2f);
				m_searchBox.OnTextChanged += OnSearchTextChanged;
				Vector2 vector2 = new Vector2(0f, 0.05f);
				m_moveUpButton.Position += vector2;
				m_moveTopButton.Position += vector2;
				m_moveBottomButton.Position += vector2;
				m_moveDownButton.Position += vector2;
				m_moveLeftButton.Position += vector2;
				m_moveLeftAllButton.Position += vector2;
				m_moveRightAllButton.Position += vector2;
				m_moveRightButton.Position += vector2;
				Controls.Add(m_searchBox);
			}
			base.CloseButtonEnabled = true;
			if ((float)MySandboxGame.ScreenSize.X / (float)MySandboxGame.ScreenSize.Y == 1.25f)
			{
				SetCloseButtonOffset_5_to_4();
			}
			else
			{
				SetDefaultCloseButtonOffset();
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMods";
		}

		private MyGuiControlLabel MakeLabel(Vector2 position, string text)
		{
			return new MyGuiControlLabel(position, null, MyTexts.GetString(text), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			string @string = MyTexts.GetString(toolTip);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, @string, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, string toolTip, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, toolTip, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		private MyGuiControlButton MakeButtonTiny(Vector2 position, float rotation, MyStringId toolTip, MyGuiHighlightTexture icon, Action<MyGuiControlButton> onClick, Vector2? size = null)
		{
			Vector2? position2 = position;
			string @string = MyTexts.GetString(toolTip);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Square, size, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, @string, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			icon.SizePx = new Vector2(64f, 64f);
			myGuiControlButton.Icon = icon;
			myGuiControlButton.IconRotation = rotation;
			myGuiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			return myGuiControlButton;
		}

		private MyGuiControlButton MakeButtonCategoryTiny(Vector2 position, float rotation, MyStringId toolTip, MyGuiHighlightTexture icon, Action<MyGuiControlButton> onClick, Vector2? size = null)
		{
			Vector2? position2 = position;
			string @string = MyTexts.GetString(toolTip);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Square48, size, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, @string, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			icon.SizePx = new Vector2(48f, 48f);
			myGuiControlButton.Icon = icon;
			myGuiControlButton.IconRotation = rotation;
			myGuiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			return myGuiControlButton;
		}

		private MyGuiControlButton MakeButtonCategory(Vector2 position, MyWorkshop.Category category)
		{
			string arg = category.Id.Replace(" ", "");
			MyGuiControlButton myGuiControlButton = MakeButtonCategoryTiny(position, 0f, category.LocalizableName, new MyGuiHighlightTexture
			{
				Normal = $"Textures\\GUI\\Icons\\buttons\\small_variant\\{arg}.dds",
				Highlight = $"Textures\\GUI\\Icons\\buttons\\small_variant\\{arg}Highlight.dds",
				SizePx = new Vector2(48f, 48f)
			}, OnCategoryButtonClick);
			myGuiControlButton.UserData = category.Id;
			myGuiControlButton.HighlightType = MyGuiControlHighlightType.FORCED;
			m_categoryButtonList.Add(myGuiControlButton);
			myGuiControlButton.Size = new Vector2(0.005f, 0.005f);
			return myGuiControlButton;
		}

		private void MoveSelectedItem(MyGuiControlTable from, MyGuiControlTable to)
		{
			to.Add(from.SelectedRow);
			from.RemoveSelectedRow();
			m_selectedRow = from.SelectedRow;
		}

		private void GetActiveMods(List<MyObjectBuilder_Checkpoint.ModItem> outputList)
		{
			for (int num = m_modsTableEnabled.RowsCount - 1; num >= 0; num--)
			{
				outputList.Add((MyObjectBuilder_Checkpoint.ModItem)m_modsTableEnabled.GetRow(num).UserData);
			}
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			sender.CanHaveFocus = true;
			base.FocusedControl = sender;
			m_selectedRow = sender.SelectedRow;
			m_selectedTable = sender;
			if (sender == m_modsTableEnabled)
			{
				m_modsTableDisabled.SelectedRowIndex = null;
			}
			if (sender == m_modsTableDisabled)
			{
				m_modsTableEnabled.SelectedRowIndex = null;
			}
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				OnTableItemConfirmedOrDoubleClick(sender, eventArgs);
			}
		}

		private void OnTableItemConfirmedOrDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			if (sender.SelectedRow != null)
			{
				MyGuiControlTable.Row selectedRow = sender.SelectedRow;
				MyObjectBuilder_Checkpoint.ModItem selectedMod = (MyObjectBuilder_Checkpoint.ModItem)selectedRow.UserData;
				MyGuiControlTable myGuiControlTable = (sender == m_modsTableEnabled) ? m_modsTableDisabled : m_modsTableEnabled;
				MoveSelectedItem(sender, myGuiControlTable);
				if (myGuiControlTable == m_modsTableEnabled && selectedMod.PublishedFileId != 0L)
				{
					GetModDependenciesAsync(selectedRow, selectedMod);
				}
			}
		}

		private void GetModDependenciesAsync(MyGuiControlTable.Row parentRow, MyObjectBuilder_Checkpoint.ModItem selectedMod)
		{
			ModDependenciesWorkData workData2 = new ModDependenciesWorkData
			{
				ParentId = selectedMod.PublishedFileId,
				ParentModRow = parentRow
			};
			Parallel.Start(delegate(WorkData workData)
			{
				ModDependenciesWorkData modDependenciesWorkData = workData as ModDependenciesWorkData;
				modDependenciesWorkData.Dependencies = MyWorkshop.GetModsDependencyHiearchy(new HashSet<ulong>
				{
					modDependenciesWorkData.ParentId
				}, out bool hasReferenceIssue);
				modDependenciesWorkData.HasReferenceIssue = hasReferenceIssue;
			}, OnGetModDependencyHiearchyCompleted, workData2);
		}

		private void OnGetModDependencyHiearchyCompleted(WorkData workData)
		{
			if (base.State != MyGuiScreenState.OPENED)
			{
				return;
			}
			ModDependenciesWorkData modDependenciesWorkData = workData as ModDependenciesWorkData;
			if (modDependenciesWorkData == null || modDependenciesWorkData.Dependencies == null || modDependenciesWorkData.Dependencies.Count <= 1)
			{
				return;
			}
			modDependenciesWorkData.Dependencies.RemoveAt(modDependenciesWorkData.Dependencies.Count - 1);
			MyGuiControlTable.Row parentModRow = modDependenciesWorkData.ParentModRow;
			if (parentModRow != null)
			{
				MyGuiControlTable.Cell cell = parentModRow.GetCell(1);
				string @string = MyTexts.GetString(MyCommonTexts.ScreenMods_ModDependencies);
				cell.ToolTip.ToolTips.Clear();
				StringBuilder value = null;
				if (m_modsToolTips.TryGetValue(modDependenciesWorkData.ParentId, out value))
				{
					cell.ToolTip.AddToolTip(value.ToString());
				}
				cell.ToolTip.AddToolTip(@string);
				foreach (MyWorkshopItem dependency in modDependenciesWorkData.Dependencies)
				{
					cell.ToolTip.AddToolTip(dependency.Title);
				}
			}
		}

		private void OnMoveUpClick(MyGuiControlButton sender)
		{
			m_selectedTable.MoveSelectedRowUp();
		}

		private void OnMoveDownClick(MyGuiControlButton sender)
		{
			m_selectedTable.MoveSelectedRowDown();
		}

		private void OnMoveTopClick(MyGuiControlButton sender)
		{
			m_selectedTable.MoveSelectedRowTop();
		}

		private void OnMoveBottomClick(MyGuiControlButton sender)
		{
			m_selectedTable.MoveSelectedRowBottom();
		}

		private void OnMoveLeftClick(MyGuiControlButton sender)
		{
			MoveSelectedItem(m_modsTableEnabled, m_modsTableDisabled);
		}

		private void OnMoveRightClick(MyGuiControlButton sender)
		{
			MoveSelectedItem(m_modsTableDisabled, m_modsTableEnabled);
		}

		private void OnMoveLeftAllClick(MyGuiControlButton sender)
		{
			while (m_modsTableEnabled.RowsCount > 0)
			{
				m_modsTableEnabled.SelectedRowIndex = 0;
				MoveSelectedItem(m_modsTableEnabled, m_modsTableDisabled);
			}
		}

		private void OnMoveRightAllClick(MyGuiControlButton sender)
		{
			while (m_modsTableDisabled.RowsCount > 0)
			{
				m_modsTableDisabled.SelectedRowIndex = 0;
				MoveSelectedItem(m_modsTableDisabled, m_modsTableEnabled);
			}
		}

		private void OnCategoryButtonClick(MyGuiControlButton sender)
		{
			if (sender.UserData != null && sender.UserData is string)
			{
				string item = (string)sender.UserData;
				if (m_selectedCategories.Contains(item))
				{
					m_selectedCategories.Remove(item);
					sender.Selected = false;
				}
				else
				{
					m_selectedCategories.Add(item);
					sender.Selected = true;
				}
				RefreshModList();
			}
		}

		private void OnSelectCategoryClicked(MyGuiControlButton sender)
		{
			string[] tags = m_selectedCategories.ToArray();
			MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("mod", MyWorkshop.ModCategories, tags, delegate(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags)
			{
				if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					List<string> list = outTags.ToList();
					list.Remove("mod");
					m_selectedCategories = list;
					RefreshCategoryButtons();
					RefreshModList();
				}
			}));
		}

		private void OnSearchTextChanged(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] source = text.Split(new char[1]
				{
					' '
				});
				m_tmpSearch = source.ToList();
			}
			else
			{
				m_tmpSearch.Clear();
			}
			RefreshModList();
		}

		private void OnPublishModClick(MyGuiControlButton sender)
		{
			if (m_selectedRow != null && m_selectedRow.UserData != null)
			{
				m_selectedMod = (MyObjectBuilder_Checkpoint.ModItem)m_selectedRow.UserData;
				string localModFolder = Path.Combine(MyFileSystem.ModsPath, m_selectedMod.Name);
				StringBuilder text = m_selectedRow.GetCell(1).Text;
				m_selectedMod.FriendlyName = text.ToString();
				m_selectedMod.PublishedFileId = MyWorkshop.GetWorkshopIdFromLocalMod(localModFolder);
				StringBuilder messageText;
				MyStringId id;
				if (m_selectedMod.PublishedFileId != 0L)
				{
					messageText = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToUpdateMod), MySession.GameServiceName));
					id = MyCommonTexts.MessageBoxCaptionDoYouWishToUpdateMod;
				}
				else
				{
					messageText = new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToPublishMod), MySession.GameServiceName, MySession.PlatformLinkAgreement));
					id = MyCommonTexts.MessageBoxCaptionDoYouWishToPublishMod;
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageText, MyTexts.Get(id), null, null, null, null, OnPublishModQuestionAnswer));
			}
		}

		private void OnPublishModQuestionAnswer(MyGuiScreenMessageBox.ResultEnum val)
		{
			if (val != 0)
			{
				return;
			}
			string[] tags = null;
			MyWorkshopItem subscribedItem = GetSubscribedItem(m_selectedMod.PublishedFileId);
			if (subscribedItem != null)
			{
				tags = subscribedItem.Tags.ToArray();
				if (subscribedItem.OwnerId != Sync.MyId)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_OwnerMismatchMod), MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublishFailed)));
					return;
				}
			}
			MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("mod", MyWorkshop.ModCategories, tags, OnPublishWorkshopTagsResult));
		}

		private void OnPublishWorkshopTagsResult(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags)
		{
			if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				string modFullPath = Path.Combine(MyFileSystem.ModsPath, m_selectedMod.Name);
				MyWorkshop.PublishModAsync(modFullPath, m_selectedMod.FriendlyName, null, m_selectedMod.PublishedFileId, outTags, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, MyWorkshopItemPublisher publishedFile)
				{
					if (success)
					{
						MyWorkshop.GenerateModInfo(modFullPath, publishedFile.Id, Sync.MyId);
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MessageBoxTextModPublished), MySession.GameServiceName)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublished), null, null, null, null, delegate
						{
							MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedFile.Id));
							FillList();
						}));
					}
					else
					{
						StringBuilder messageText = (result != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishFailed), MySession.WorkshopServiceName, MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublishFailed)));
					}
				});
			}
		}

		private void OnOpenInWorkshopClick(MyGuiControlButton obj)
		{
			if (m_selectedRow != null)
			{
				object userData = m_selectedRow.UserData;
				if (userData != null)
				{
					MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemUrl(((MyObjectBuilder_Checkpoint.ModItem)userData).PublishedFileId), MyGameService.WorkshopService.ServiceName + " Workshop");
				}
			}
		}

		private void OnBrowseWorkshopClick(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_MODS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		private void OnRefreshClick(MyGuiControlButton obj)
		{
			if (!m_listNeedsReload)
			{
				m_listNeedsReload = true;
				FillList();
			}
		}

		private void OnOkClick(MyGuiControlButton obj)
		{
			m_modListToEdit.Clear();
			GetActiveMods(m_modListToEdit);
			CloseScreen();
		}

		private void OnCancelClick(MyGuiControlButton obj)
		{
			CloseScreen();
		}

		public override bool Update(bool hasFocus)
		{
			bool flag = m_selectedRow != null;
			bool num = flag && m_selectedRow.UserData != null;
			bool flag2 = num && ((MyObjectBuilder_Checkpoint.ModItem)m_selectedRow.UserData).PublishedFileId == 0;
			bool flag3 = num && ((MyObjectBuilder_Checkpoint.ModItem)m_selectedRow.UserData).PublishedFileId != 0;
			bool flag4 = num && ((MyObjectBuilder_Checkpoint.ModItem)m_selectedRow.UserData).IsDependency;
			m_openInWorkshopButton.Enabled = (flag && flag3);
			m_publishModButton.Enabled = (flag && flag2 && MyFakes.ENABLE_WORKSHOP_PUBLISH);
			MyGuiControlButton moveUpButton = m_moveUpButton;
			bool enabled = m_moveTopButton.Enabled = (flag && m_selectedTable.SelectedRowIndex.HasValue && m_selectedTable.SelectedRowIndex.Value > 0 && !flag4);
			moveUpButton.Enabled = enabled;
			MyGuiControlButton moveDownButton = m_moveDownButton;
			enabled = (m_moveBottomButton.Enabled = (flag && m_selectedTable.SelectedRowIndex.HasValue && m_selectedTable.SelectedRowIndex.Value < m_selectedTable.RowsCount - 1 && !flag4));
			moveDownButton.Enabled = enabled;
			m_moveLeftButton.Enabled = (flag && m_selectedTable == m_modsTableEnabled && !flag4);
			m_moveRightButton.Enabled = (flag && m_selectedTable == m_modsTableDisabled && !flag4);
			m_moveLeftAllButton.Enabled = (m_modsTableEnabled.RowsCount > 0 && !flag4);
			m_moveRightAllButton.Enabled = (m_modsTableDisabled.RowsCount > 0 && !flag4);
			if (MySession.Static == null)
			{
				Parallel.RunCallbacks();
			}
			return base.Update(hasFocus);
		}

		public override bool Draw()
		{
			if (m_listNeedsReload)
			{
				return false;
			}
			return base.Draw();
		}

		protected override void OnShow()
		{
			base.OnShow();
			if (m_listNeedsReload)
			{
				FillList();
			}
		}

		private void FillList()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginAction, endAction));
		}

		private void AddHeaders()
		{
			m_modsTableEnabled.SetColumnName(1, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenMods_ActiveMods) + "     "));
			m_modsTableEnabled.SetHeaderColumnAlign(1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_modsTableDisabled.SetColumnName(1, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenMods_AvailableMods) + "     "));
			m_modsTableDisabled.SetHeaderColumnAlign(1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}

		private MyGuiControlTable.Row AddMod(bool active, StringBuilder title, StringBuilder toolTip, StringBuilder modState, MyGuiHighlightTexture? icon, MyObjectBuilder_Checkpoint.ModItem mod, Color? textColor = null)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(mod);
			row.AddCell(new MyGuiControlTable.Cell(string.Empty, null, modState.ToString(), null, icon));
			row.AddCell(new MyGuiControlTable.Cell(title, null, toolTip.ToString(), textColor));
			if (active)
			{
				m_modsTableEnabled.Insert(0, row);
			}
			else
			{
				m_modsTableDisabled.Add(row);
			}
			if (mod.PublishedFileId != 0L)
			{
				m_modsToolTips[mod.PublishedFileId] = toolTip;
			}
			return row;
		}

		private MyWorkshopItem GetSubscribedItem(ulong publishedFileId)
		{
			foreach (MyWorkshopItem subscribedMod in m_subscribedMods)
			{
				if (subscribedMod.Id == publishedFileId)
				{
					return subscribedMod;
				}
			}
			foreach (MyWorkshopItem worldMod in m_worldMods)
			{
				if (worldMod.Id == publishedFileId)
				{
					return worldMod;
				}
			}
			return null;
		}

		private void RefreshCategoryButtons()
		{
			foreach (MyGuiControlButton categoryButton in m_categoryButtonList)
			{
				if (categoryButton.UserData != null)
				{
					string item = (categoryButton.UserData as string).ToLower();
					categoryButton.Selected = m_selectedCategories.Contains(item);
				}
			}
		}

		private void RefreshModList()
		{
			m_selectedRow = null;
			m_selectedTable = null;
			if (m_modsTableEnabled != null)
			{
				ListReader<MyObjectBuilder_Checkpoint.ModItem> mods;
				if (m_keepActiveMods)
				{
					List<MyObjectBuilder_Checkpoint.ModItem> list = new List<MyObjectBuilder_Checkpoint.ModItem>(m_modsTableEnabled.RowsCount);
					GetActiveMods(list);
					mods = list;
				}
				else
				{
					mods = m_modListToEdit;
				}
				m_keepActiveMods = true;
				RefreshWorldMods(mods);
				m_modsTableEnabled.Clear();
				m_modsTableDisabled.Clear();
				m_modsToolTips.Clear();
				AddHeaders();
				foreach (MyObjectBuilder_Checkpoint.ModItem item in mods)
				{
					if (!item.IsDependency)
					{
						if (item.PublishedFileId == 0L)
						{
							StringBuilder title = new StringBuilder(item.Name);
							string text = Path.Combine(MyFileSystem.ModsPath, item.Name);
							StringBuilder stringBuilder = new StringBuilder(text);
							StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_LocalMod);
							Color? textColor = null;
							MyGuiHighlightTexture tEXTURE_ICON_MODS_LOCAL = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
							if (!Directory.Exists(text) && !File.Exists(text))
							{
								stringBuilder = MyTexts.Get(MyCommonTexts.ScreenMods_MissingLocalMod);
								modState = stringBuilder;
								textColor = MyHudConstants.MARKER_COLOR_RED;
							}
							AddMod(active: true, title, stringBuilder, modState, tEXTURE_ICON_MODS_LOCAL, item, textColor);
						}
						else
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							StringBuilder stringBuilder3 = new StringBuilder();
							StringBuilder modState2 = MyTexts.Get(MyCommonTexts.ScreenMods_WorkshopMod);
							Color? textColor2 = null;
							MyWorkshopItem subscribedItem = GetSubscribedItem(item.PublishedFileId);
							if (subscribedItem != null)
							{
								if (!string.IsNullOrEmpty(subscribedItem.Title))
								{
									stringBuilder2.Append(subscribedItem.Title);
								}
								else
								{
									stringBuilder2.Append(string.Format(MyTexts.GetString(MyCommonTexts.ModNotReceived), item.PublishedFileId));
								}
								if (!string.IsNullOrEmpty(subscribedItem.Description))
								{
									int num = Math.Min(subscribedItem.Description.Length, 128);
									int num2 = subscribedItem.Description.IndexOf("\n");
									if (num2 > 0)
									{
										num = Math.Min(num, num2 - 1);
									}
									stringBuilder3.Append(subscribedItem.Description.Substring(0, num));
								}
								else
								{
									stringBuilder3.Append(string.Format(MyTexts.GetString(MyCommonTexts.ModNotReceived_ToolTip), MyGameService.Service.ServiceName));
								}
							}
							else
							{
								ulong publishedFileId = item.PublishedFileId;
								stringBuilder2.Append(publishedFileId.ToString());
								stringBuilder3 = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenMods_MissingDetails), MySession.GameServiceName);
								textColor2 = MyHudConstants.MARKER_COLOR_RED;
							}
							MyGuiHighlightTexture tEXTURE_ICON_MODS_WORKSHOP = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP;
							AddMod(active: true, stringBuilder2, stringBuilder3, modState2, tEXTURE_ICON_MODS_WORKSHOP, item, textColor2);
						}
					}
				}
				FillLocalMods();
				if (m_subscribedMods != null)
				{
					FillSubscribedMods();
				}
			}
		}

		private void FillSubscribedMods()
		{
			foreach (MyWorkshopItem subscribedMod in m_subscribedMods)
			{
				if (subscribedMod != null && !m_worldWorkshopMods.Contains(subscribedMod.Id))
				{
					if (MyFakes.ENABLE_MOD_CATEGORIES)
					{
						bool flag = false;
						foreach (string tag in subscribedMod.Tags)
						{
							if (m_selectedCategories.Contains(tag.ToLower()) || m_selectedCategories.Count == 0)
							{
								flag = true;
								break;
							}
						}
						if (!CheckSearch(subscribedMod.Title) || !flag)
						{
							continue;
						}
					}
					StringBuilder title = new StringBuilder(subscribedMod.Title);
					int num = Math.Min(subscribedMod.Description.Length, 128);
					int num2 = subscribedMod.Description.IndexOf("\n");
					if (num2 > 0)
					{
						num = Math.Min(num, num2 - 1);
					}
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_WorkshopMod);
					stringBuilder.Append(subscribedMod.Description.Substring(0, num));
					MyGuiHighlightTexture tEXTURE_ICON_MODS_WORKSHOP = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP;
					AddMod(mod: new MyObjectBuilder_Checkpoint.ModItem(null, subscribedMod.Id, subscribedMod.Title), active: false, title: title, toolTip: stringBuilder, modState: modState, icon: tEXTURE_ICON_MODS_WORKSHOP);
				}
			}
		}

		private void FillLocalMods()
		{
			if (!Directory.Exists(MyFileSystem.ModsPath))
			{
				Directory.CreateDirectory(MyFileSystem.ModsPath);
			}
			string[] directories = Directory.GetDirectories(MyFileSystem.ModsPath, "*", SearchOption.TopDirectoryOnly);
			foreach (string text in directories)
			{
				string fileName = Path.GetFileName(text);
				if (!m_worldLocalMods.Contains(fileName) && Directory.GetFileSystemEntries(text).Length != 0 && (!MyFakes.ENABLE_MOD_CATEGORIES || CheckSearch(fileName)))
				{
					StringBuilder title = new StringBuilder(fileName);
					StringBuilder toolTip = new StringBuilder(text);
					StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_LocalMod);
					MyWorkshop.GetWorkshopIdFromLocalMod(text);
					MyGuiHighlightTexture tEXTURE_ICON_MODS_LOCAL = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
					AddMod(active: false, title, toolTip, modState, tEXTURE_ICON_MODS_LOCAL, new MyObjectBuilder_Checkpoint.ModItem(fileName, 0uL));
				}
			}
		}

		private bool CheckSearch(string name)
		{
			bool result = true;
			string text = name.ToLower();
			foreach (string item in m_tmpSearch)
			{
				if (!text.Contains(item.ToLower()))
				{
					return false;
				}
			}
			return result;
		}

		private IMyAsyncResult beginAction()
		{
			return new MyModsLoadListResult(m_worldWorkshopMods);
		}

		private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			m_listNeedsReload = false;
			MyModsLoadListResult myModsLoadListResult = result as MyModsLoadListResult;
			if (myModsLoadListResult != null)
			{
				m_subscribedMods = myModsLoadListResult.SubscribedMods;
				m_worldMods = myModsLoadListResult.SetMods;
				RefreshModList();
				screen.CloseScreen();
			}
		}
	}
}
