#define VRAGE
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenLoadSubscribedWorld : MyGuiScreenBase
	{
		private class LoadListResult : IMyAsyncResult
		{
			public List<MyWorkshopItem> SubscribedWorlds;

			public bool IsCompleted => Task.IsComplete;

			public Task Task
			{
				get;
				private set;
			}

			public LoadListResult()
			{
				Task = Parallel.Start(delegate
				{
					LoadListAsync(out SubscribedWorlds);
				});
			}

			private void LoadListAsync(out List<MyWorkshopItem> list)
			{
				List<MyWorkshopItem> list2 = new List<MyWorkshopItem>();
				if (MyWorkshop.GetSubscribedWorldsBlocking(list2))
				{
					list = list2;
					List<MyWorkshopItem> list3 = new List<MyWorkshopItem>();
					if (MyWorkshop.GetSubscribedScenariosBlocking(list3) && list3.Count > 0)
					{
						list.InsertRange(list.Count, list3);
					}
				}
				else
				{
					list = null;
				}
			}
		}

		private MyGuiControlTable m_worldsTable;

		private MyGuiControlButton m_loadButton;

		private MyGuiControlButton m_openInWorkshopButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_browseWorkshopButton;

		private MyGuiControlButton m_copyButton;

		private MyGuiControlButton m_currentButton;

		private int m_selectedRow;

		private bool m_listNeedsReload;

		private List<MyWorkshopItem> m_subscribedWorlds;

		private MyGuiControlTextbox m_searchBox;

		private MyGuiControlLabel m_searchBoxLabel;

		private MyGuiControlButton m_searchClear;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		public MyGuiScreenLoadSubscribedWorld(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.878f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_displayTabScenario = displayTabScenario;
			m_displayTabWorkshop = displayTabWorkshop;
			m_displayTabCustom = displayTabCustom;
			base.EnabledBackgroundFade = true;
			m_listNeedsReload = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorkshopScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.872f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.872f);
			Controls.Add(myGuiControlSeparatorList);
			float num = 0.216f;
			float num2 = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
			float num3 = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num4 = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num5 = 93f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector2 value = -m_size.Value / 2f + new Vector2(num5, num + 0.199f);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			Vector2 vector = -m_size.Value / 2f + new Vector2(num5 + minSizeGui.X + num3, num);
			Vector2 vector2 = m_size.Value / 2f - vector;
			vector2.X -= num4;
			vector2.Y -= num2;
			m_searchBoxLabel = new MyGuiControlLabel();
			m_searchBoxLabel.Text = MyTexts.Get(MyCommonTexts.Search).ToString() + ":";
			m_searchBoxLabel.Position = new Vector2(-0.188f, -0.244f);
			Controls.Add(m_searchBoxLabel);
			m_searchBox = new MyGuiControlTextbox(new Vector2(0.382f, -0.247f));
			m_searchBox.TextChanged += OnSearchTextChange;
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_searchBox.Size = new Vector2(0.56f - m_searchBoxLabel.Size.X, 1f);
			Controls.Add(m_searchBox);
			m_searchClear = new MyGuiControlButton
			{
				Position = m_searchBox.Position + new Vector2(-0.027f, 0.004f),
				Size = new Vector2(0.045f, 17f / 300f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.Close,
				ActivateOnMouseRelease = true
			};
			m_searchClear.ButtonClicked += OnSearchClear;
			Controls.Add(m_searchClear);
			m_worldsTable = new MyGuiControlTable();
			m_worldsTable.Position = vector + new Vector2(0.0055f, 0.065f);
			m_worldsTable.Size = new Vector2(1075f / MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.852f, 0.15f);
			m_worldsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_worldsTable.ColumnsCount = 1;
			m_worldsTable.VisibleRowsCount = 15;
			m_worldsTable.ItemSelected += OnTableItemSelected;
			m_worldsTable.ItemDoubleClicked += OnTableItemConfirmedOrDoubleClick;
			m_worldsTable.ItemConfirmed += OnTableItemConfirmedOrDoubleClick;
			m_worldsTable.SetCustomColumnWidths(new float[1]
			{
				1f
			});
			m_worldsTable.SetColumnComparison(0, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => ((StringBuilder)a.UserData).CompareToIgnoreCase((StringBuilder)b.UserData));
			Controls.Add(m_worldsTable);
			Vector2 value2 = value + minSizeGui * 0.5f;
			Vector2 mENU_BUTTONS_POSITION_DELTA = MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
			Controls.Add(m_copyButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * -3.21f, MyCommonTexts.ScreenLoadSubscribedWorldCopyWorld, MyCommonTexts.ToolTipWorkshopCopyWorld, OnCopyClick));
			Controls.Add(m_openInWorkshopButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * -2.21f, MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop, MyCommonTexts.ToolTipWorkshopOpenInWorkshop, OnOpenInWorkshopClick));
			Controls.Add(m_browseWorkshopButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * -1.21f, MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop, string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopBrowseWorkshop), MySession.GameServiceName), OnBrowseWorkshopClick));
			Controls.Add(m_refreshButton = MakeButton(value2 + mENU_BUTTONS_POSITION_DELTA * -0.210000038f, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ToolTipWorkshopRefresh, OnRefreshClick));
			Controls.Add(m_loadButton = MakeButton(new Vector2(0f, 0f) - new Vector2(-0.109f, (0f - m_size.Value.Y) / 2f + 0.071f), MyCommonTexts.ScreenLoadSubscribedWorldCopyAndLoad, MyCommonTexts.ToolTipWorkshopCopyAndLoad, OnLoadClick));
			m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			m_loadButton.DrawCrossTextureWhenDisabled = false;
			m_openInWorkshopButton.DrawCrossTextureWhenDisabled = false;
			base.CloseButtonEnabled = true;
		}

		private bool SearchFilterTest(string testString)
		{
			if (m_searchBox.Text != null && m_searchBox.Text.Length != 0)
			{
				string[] array = m_searchBox.Text.Split(new char[1]
				{
					' '
				});
				string text = testString.ToLower();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text.Contains(text2.ToLower()))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoadSubscribedWorld";
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		private void OnSearchTextChange(MyGuiControlTextbox box)
		{
			RefreshGameList();
		}

		private void OnSearchClear(MyGuiControlButton sender)
		{
			m_searchBox.Text = "";
			RefreshGameList();
		}

		private void OnOpenInWorkshopClick(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_worldsTable.SelectedRow;
			if (selectedRow != null)
			{
				MyWorkshopItem myWorkshopItem = selectedRow.UserData as MyWorkshopItem;
				if (myWorkshopItem != null)
				{
					MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemUrl(myWorkshopItem.Id), MyGameService.WorkshopService.ServiceName + " Workshop");
				}
			}
		}

		private void OnBrowseWorkshopClick(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_WORLDS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		private void OnRefreshClick(MyGuiControlButton obj)
		{
			if (!m_listNeedsReload)
			{
				m_listNeedsReload = true;
				FillList();
			}
		}

		private void OnBackClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			m_selectedRow = eventArgs.RowIndex;
		}

		private void OnLoadClick(MyGuiControlButton sender)
		{
			m_currentButton = m_loadButton;
			CreateAndLoadFromSubscribedWorld();
		}

		private void OnTableItemConfirmedOrDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			m_currentButton = m_loadButton;
			CreateAndLoadFromSubscribedWorld();
		}

		private void OnCopyClick(MyGuiControlButton sender)
		{
			m_currentButton = m_copyButton;
			CopyWorldAndGoToLoadScreen();
		}

		private void CreateAndLoadFromSubscribedWorld()
		{
			MyGuiControlTable.Row selectedRow = m_worldsTable.SelectedRow;
			if (selectedRow != null && selectedRow.UserData is MyWorkshopItem)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginActionLoadSaves, endActionLoadSaves));
			}
		}

		private void CopyWorldAndGoToLoadScreen()
		{
			MyGuiControlTable.Row selectedRow = m_worldsTable.SelectedRow;
			if (selectedRow != null && selectedRow.UserData is MyWorkshopItem)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginActionLoadSaves, endActionLoadSaves));
			}
		}

		private void OnSuccess(string sessionPath)
		{
			if (m_currentButton == m_copyButton)
			{
				new MyGuiScreenLoadSandbox();
				MyGuiSandbox.AddScreen(new MyGuiScreenLoadSandbox());
			}
			else if (m_currentButton == m_loadButton)
			{
				MySessionLoader.LoadSingleplayerSession(sessionPath);
			}
			m_currentButton = null;
		}

		private void OverwriteWorldDialog()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get((m_currentButton == m_loadButton) ? MyCommonTexts.MessageBoxTextWorldExistsDownloadOverwrite : MyCommonTexts.MessageBoxTextWorldExistsOverwrite), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnOverwriteWorld));
		}

		private void OnOverwriteWorld(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyWorkshop.CreateWorldInstanceAsync(m_worldsTable.SelectedRow.UserData as MyWorkshopItem, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: true, delegate(bool success, string sessionPath)
				{
					if (success)
					{
						OnSuccess(sessionPath);
					}
				});
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (m_worldsTable.SelectedRow != null)
			{
				m_loadButton.Enabled = true;
				m_copyButton.Enabled = true;
				m_openInWorkshopButton.Enabled = true;
			}
			else
			{
				m_loadButton.Enabled = false;
				m_copyButton.Enabled = false;
				m_openInWorkshopButton.Enabled = false;
			}
			return base.Update(hasFocus);
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyAnalyticsHelper.ReportActivityEnd(null, "show_workshop");
		}

		protected override void OnShow()
		{
			base.OnShow();
			MyAnalyticsHelper.ReportActivityStart(null, "show_workshop", string.Empty, "GUI", string.Empty);
			if (m_listNeedsReload)
			{
				FillList();
			}
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(text), toolTip: MyTexts.GetString(toolTip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: onClick);
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, string toolTip, Action<MyGuiControlButton> onClick)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		private void FillList()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginAction, endAction));
		}

		private void AddHeaders()
		{
			m_worldsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
		}

		private void RefreshGameList()
		{
			m_worldsTable.Clear();
			AddHeaders();
			if (m_subscribedWorlds != null)
			{
				for (int i = 0; i < m_subscribedWorlds.Count; i++)
				{
					MyWorkshopItem myWorkshopItem = m_subscribedWorlds[i];
					MyGuiControlTable.Row row = new MyGuiControlTable.Row(myWorkshopItem);
					StringBuilder stringBuilder = new StringBuilder(myWorkshopItem.Title);
					if (SearchFilterTest(stringBuilder.ToString()))
					{
						row.AddCell(new MyGuiControlTable.Cell(stringBuilder.ToString(), stringBuilder));
						row.AddCell(new MyGuiControlTable.Cell());
						m_worldsTable.Add(row);
					}
				}
			}
			m_worldsTable.SelectedRowIndex = null;
		}

		private IMyAsyncResult beginAction()
		{
			return new LoadListResult();
		}

		private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			m_listNeedsReload = false;
			LoadListResult loadListResult = (LoadListResult)result;
			m_subscribedWorlds = loadListResult.SubscribedWorlds;
			RefreshGameList();
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
		}

		private IMyAsyncResult beginActionLoadSaves()
		{
			return new MyLoadWorldInfoListResult();
		}

		private void endActionLoadSaves(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			MyWorkshopItem myWorkshopItem = m_worldsTable.SelectedRow.UserData as MyWorkshopItem;
			if (Directory.Exists(MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(myWorkshopItem.Title), contentFolder: false, createIfNotExists: false)))
			{
				OverwriteWorldDialog();
			}
			else
			{
				MyWorkshop.CreateWorldInstanceAsync(myWorkshopItem, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: false, delegate(bool success, string sessionPath)
				{
					if (success)
					{
						OnSuccess(sessionPath);
					}
				});
			}
		}
	}
}
