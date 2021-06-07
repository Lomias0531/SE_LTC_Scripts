#define VRAGE
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyGuiBlueprintScreen_Reworked : MyGuiScreenDebugBase
	{
		private enum Tab
		{
			Info,
			Edit
		}

		public enum SortOption
		{
			None,
			Alphabetical,
			CreationDate,
			UpdateDate
		}

		private enum WaitForScreenshotOptions
		{
			None,
			TakeScreenshotLocal,
			CreateNewBlueprintCloud,
			TakeScreenshotCloud
		}

		private class MyWaitForScreenshotData
		{
			private bool m_isSet;

			public WaitForScreenshotOptions Option
			{
				get;
				private set;
			}

			public MyGuiControlContentButton UsedButton
			{
				get;
				private set;
			}

			public MyObjectBuilder_Definitions Prefab
			{
				get;
				private set;
			}

			public string PathRel
			{
				get;
				private set;
			}

			public string PathFull
			{
				get;
				private set;
			}

			public MyWaitForScreenshotData()
			{
				Clear();
			}

			public bool SetData_TakeScreenshotLocal(MyGuiControlContentButton button)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.TakeScreenshotLocal;
				UsedButton = button;
				return true;
			}

			public bool SetData_CreateNewBlueprintCloud(string pathRel, string pathFull)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.CreateNewBlueprintCloud;
				PathRel = pathRel;
				PathFull = pathFull;
				return true;
			}

			public bool SetData_TakeScreenshotCloud(string pathRel, string pathFull, MyGuiControlContentButton button)
			{
				if (m_isSet)
				{
					return false;
				}
				m_isSet = true;
				Option = WaitForScreenshotOptions.TakeScreenshotCloud;
				PathRel = pathRel;
				PathFull = pathFull;
				UsedButton = button;
				return true;
			}

			public bool IsWaiting()
			{
				return m_isSet;
			}

			public void Clear()
			{
				m_isSet = false;
				Option = WaitForScreenshotOptions.None;
				UsedButton = null;
				PathRel = string.Empty;
				PathFull = string.Empty;
				UsedButton = null;
			}
		}

		private enum ScrollTestResult
		{
			Ok,
			Higher,
			Lower
		}

		protected sealed class ShareBlueprintRequest_003C_003ESystem_UInt64_0023System_String_0023System_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, ulong, string, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong workshopId, in string name, in ulong sendToId, in string senderName, in DBNull arg5, in DBNull arg6)
			{
				ShareBlueprintRequest(workshopId, name, sendToId, senderName);
			}
		}

		protected sealed class ShareBlueprintRequestClient_003C_003ESystem_UInt64_0023System_String_0023System_UInt64_0023System_String : ICallSite<IMyEventOwner, ulong, string, ulong, string, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong workshopId, in string name, in ulong sendToId, in string senderName, in DBNull arg5, in DBNull arg6)
			{
				ShareBlueprintRequestClient(workshopId, name, sendToId, senderName);
			}
		}

		public static readonly float MAGIC_SPACING_BIG = 0.00535f;

		public static readonly float MAGIC_SPACING_SMALL = 0.00888f;

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.878f, 0.97f);

		private static HashSet<ulong> m_downloadQueued = new HashSet<ulong>();

		private static MyConcurrentHashSet<ulong> m_downloadFinished = new MyConcurrentHashSet<ulong>();

		private static MyWaitForScreenshotData m_waitingForScreenshot = new MyWaitForScreenshotData();

		private static bool m_downloadFromSteam = true;

		private static bool m_needsExtract = false;

		private static bool m_showDlcIcons = false;

		private static List<MyBlueprintItemInfo> m_recievedBlueprints = new List<MyBlueprintItemInfo>();

		private readonly List<MyGuiControlImage> m_dlcIcons = new List<MyGuiControlImage>();

		private static LoadPrefabData m_LoadPrefabData;

		private static Dictionary<Content, List<MyWorkshopItem>> m_subscribedItemsListDict = new Dictionary<Content, List<MyWorkshopItem>>();

		private static Dictionary<Content, string> m_currentLocalDirectoryDict = new Dictionary<Content, string>();

		private static Dictionary<Content, SortOption> m_selectedSortDict = new Dictionary<Content, SortOption>();

		private static Dictionary<Content, MyBlueprintTypeEnum> m_selectedBlueprintTypeDict = new Dictionary<Content, MyBlueprintTypeEnum>();

		private static Dictionary<Content, bool> m_thumbnailsVisibleDict = new Dictionary<Content, bool>();

		public static Task Task;

		public static readonly FastResourceLock SubscribedItemsLock = new FastResourceLock();

		private Tab m_selectedTab;

		private float m_guiMultilineHeight;

		private float m_guiAdditionalInfoOffset;

		private MyGridClipboard m_clipboard;

		private MyBlueprintAccessType m_accessType;

		private bool m_allowCopyToClipboard;

		private MyObjectBuilder_Definitions m_loadedPrefab;

		private ulong? m_publishedItemId;

		private bool m_blueprintBeingLoaded;

		private Action<string> m_onScriptOpened;

		private Func<string> m_getCodeFromEditor;

		private Action m_onCloseAction;

		private MyBlueprintItemInfo m_selectedBlueprint;

		private MyGuiControlContentButton m_selectedButton;

		private MyGuiControlRadioButtonGroup m_BPTypesGroup;

		private MyGuiControlList m_BPList;

		private List<string> m_preloadedTextures = new List<string>();

		private Content m_content;

		private bool m_wasPublished;

		private MyGuiControlSeparatorList m_separator;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlMultilineText m_multiline;

		private MyGuiControlPanel m_detailsBackground;

		private MyGuiControlLabel m_detailName;

		private MyGuiControlLabel m_detailBlockCount;

		private MyGuiControlLabel m_detailBlockCountValue;

		private MyGuiControlLabel m_detailSizeValue;

		private MyGuiControlLabel m_detailAuthor;

		private MyGuiControlLabel m_detailAuthorName;

		private MyGuiControlLabel m_detailDLC;

		private MyGuiControlLabel m_detailSize;

		private MyGuiControlLabel m_detailSendTo;

		private MyGuiControlButton m_button_Refresh;

		private MyGuiControlButton m_button_GroupSelection;

		private MyGuiControlButton m_button_Sorting;

		private MyGuiControlButton m_button_OpenWorkshop;

		private MyGuiControlButton m_button_NewBlueprint;

		private MyGuiControlButton m_button_DirectorySelection;

		private MyGuiControlButton m_button_HideThumbnails;

		private MyGuiControlButton m_button_TabInfo;

		private MyGuiControlButton m_button_TabEdit;

		private MyGuiControlButton m_button_OpenInWorkshop;

		private MyGuiControlButton m_button_CopyToClipboard;

		private MyGuiControlButton m_button_Rename;

		private MyGuiControlButton m_button_Replace;

		private MyGuiControlButton m_button_Delete;

		private MyGuiControlButton m_button_TakeScreenshot;

		private MyGuiControlButton m_button_Publish;

		private MyGuiControlCombobox m_sendToCombo;

		private MyGuiControlImage m_icon_Refresh;

		private MyGuiControlImage m_icon_GroupSelection;

		private MyGuiControlImage m_icon_Sorting;

		private MyGuiControlImage m_icon_OpenWorkshop;

		private MyGuiControlImage m_icon_DirectorySelection;

		private MyGuiControlImage m_icon_NewBlueprint;

		private MyGuiControlImage m_icon_HideThumbnails;

		private MyGuiControlImage m_thumbnailImage;

		public List<MyWorkshopItem> GetSubscribedItemsList()
		{
			return GetSubscribedItemsList(m_content);
		}

		public static List<MyWorkshopItem> GetSubscribedItemsList(Content content)
		{
			if (!m_subscribedItemsListDict.ContainsKey(content))
			{
				m_subscribedItemsListDict.Add(content, new List<MyWorkshopItem>());
			}
			return m_subscribedItemsListDict[content];
		}

		public void SetSubscriveItemList(ref List<MyWorkshopItem> list)
		{
			SetSubscriveItemList(ref list, m_content);
		}

		public static void SetSubscriveItemList(ref List<MyWorkshopItem> list, Content content)
		{
			if (m_subscribedItemsListDict.ContainsKey(content))
			{
				m_subscribedItemsListDict[content] = list;
			}
			else
			{
				m_subscribedItemsListDict.Add(content, list);
			}
		}

		public string GetCurrentLocalDirectory()
		{
			return GetCurrentLocalDirectory(m_content);
		}

		public static string GetCurrentLocalDirectory(Content content)
		{
			if (!m_currentLocalDirectoryDict.ContainsKey(content))
			{
				m_currentLocalDirectoryDict.Add(content, string.Empty);
			}
			return m_currentLocalDirectoryDict[content];
		}

		public void SetCurrentLocalDirectory(string path)
		{
			SetCurrentLocalDirectory(m_content, path);
		}

		public static void SetCurrentLocalDirectory(Content content, string path)
		{
			if (m_currentLocalDirectoryDict.ContainsKey(content))
			{
				m_currentLocalDirectoryDict[content] = path;
			}
			else
			{
				m_currentLocalDirectoryDict.Add(content, path);
			}
		}

		private SortOption GetSelectedSort()
		{
			return GetSelectedSort(m_content);
		}

		private SortOption GetSelectedSort(Content content)
		{
			if (!m_selectedSortDict.ContainsKey(content))
			{
				m_selectedSortDict.Add(content, SortOption.None);
			}
			return m_selectedSortDict[content];
		}

		public MyBlueprintTypeEnum GetSelectedBlueprintType()
		{
			return GetSelectedBlueprintType(m_content);
		}

		public MyBlueprintTypeEnum GetSelectedBlueprintType(Content content)
		{
			if (!m_selectedBlueprintTypeDict.ContainsKey(content))
			{
				m_selectedBlueprintTypeDict.Add(content, MyBlueprintTypeEnum.MIXED);
			}
			return m_selectedBlueprintTypeDict[content];
		}

		public bool GetThumbnailVisibility()
		{
			return GetThumbnailVisibility(m_content);
		}

		public bool GetThumbnailVisibility(Content content)
		{
			if (!m_thumbnailsVisibleDict.ContainsKey(content))
			{
				m_thumbnailsVisibleDict.Add(content, value: true);
			}
			return m_thumbnailsVisibleDict[content];
		}

		private void SetSelectedSort(SortOption option)
		{
			SetSelectedSort(m_content, option);
		}

		private static void SetSelectedSort(Content content, SortOption option)
		{
			if (m_selectedSortDict.ContainsKey(content))
			{
				m_selectedSortDict[content] = option;
			}
			else
			{
				m_selectedSortDict.Add(content, option);
			}
		}

		public void SetSelectedBlueprintType(MyBlueprintTypeEnum option)
		{
			SetSelectedBlueprintType(m_content, option);
		}

		public static void SetSelectedBlueprintType(Content content, MyBlueprintTypeEnum option)
		{
			if (m_selectedBlueprintTypeDict.ContainsKey(content))
			{
				m_selectedBlueprintTypeDict[content] = option;
			}
			else
			{
				m_selectedBlueprintTypeDict.Add(content, option);
			}
		}

		public void SetThumbnailVisibility(bool option)
		{
			SetThumbnailVisibility(m_content, option);
		}

		public static void SetThumbnailVisibility(Content content, bool option)
		{
			if (m_thumbnailsVisibleDict.ContainsKey(content))
			{
				m_thumbnailsVisibleDict[content] = option;
			}
			else
			{
				m_thumbnailsVisibleDict.Add(content, option);
			}
		}

		public static MyGuiBlueprintScreen_Reworked CreateBlueprintScreen(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType)
		{
			MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = new MyGuiBlueprintScreen_Reworked();
			myGuiBlueprintScreen_Reworked.SetBlueprintInitData(clipboard, allowCopyToClipboard, accessType);
			myGuiBlueprintScreen_Reworked.FinishInitialization();
			return myGuiBlueprintScreen_Reworked;
		}

		public static MyGuiBlueprintScreen_Reworked CreateScriptScreen(Action<string> onScriptOpened, Func<string> getCodeFromEditor, Action onCloseAction)
		{
			MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = new MyGuiBlueprintScreen_Reworked();
			myGuiBlueprintScreen_Reworked.SetScriptInitData(onScriptOpened, getCodeFromEditor, onCloseAction);
			myGuiBlueprintScreen_Reworked.FinishInitialization();
			return myGuiBlueprintScreen_Reworked;
		}

		private MyGuiBlueprintScreen_Reworked()
			: base(new Vector2(0.5f, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: false)
		{
			base.CanHideOthers = true;
			m_canShareInput = false;
			base.CanBeHidden = true;
			m_canCloseInCloseAllScreenCalls = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			InitializeBPList();
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
		}

		private void SetBlueprintInitData(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType)
		{
			m_content = Content.Blueprint;
			m_accessType = accessType;
			m_clipboard = clipboard;
			m_allowCopyToClipboard = allowCopyToClipboard;
			CheckCurrentLocalDirectory_Blueprint();
			GetLocalNames_Blueprints(m_downloadFromSteam);
			ApplyFiltering();
		}

		private void SetScriptInitData(Action<string> onScriptOpened, Func<string> getCodeFromEditor, Action onCloseAction)
		{
			m_content = Content.Script;
			m_onScriptOpened = onScriptOpened;
			m_getCodeFromEditor = getCodeFromEditor;
			m_onCloseAction = onCloseAction;
			CheckCurrentLocalDirectory_Blueprint();
			using (SubscribedItemsLock.AcquireSharedUsing())
			{
				GetLocalNames_Scripts(GetSubscribedItemsList(m_content).Count == 0);
			}
		}

		private void FinishInitialization()
		{
			if (m_downloadFromSteam)
			{
				m_downloadFromSteam = false;
			}
			RecreateControls(constructor: true);
			TrySelectFirstBlueprint();
		}

		private void InitializeBPList()
		{
			float num = 0.307f;
			float x = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector2 position = -m_size.Value / 2f + new Vector2(x, num);
			m_BPTypesGroup = new MyGuiControlRadioButtonGroup();
			m_BPTypesGroup.SelectedChanged += OnSelectItem;
			m_BPTypesGroup.MouseDoubleClick += OnMouseDoubleClickItem;
			m_BPList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(0.2f, m_size.Value.Y - num - 0.048f)
			};
		}

		private void OnMouseDoubleClickItem(MyGuiControlRadioButton obj)
		{
			CopyToClipboard();
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float y = 0.22f;
			float num = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			_ = -m_size.Value / 2f + new Vector2(num, y);
			RecomputeDetailOffsets();
			MyGuiControlMultilineText control = new MyGuiControlMultilineText
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.5f * m_size.Value.X + num, -0.345f),
				Size = new Vector2(m_size.Value.X - 0.1f, 0.05f),
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Text = new StringBuilder(string.Format(MyTexts.GetString((m_content == Content.Blueprint) ? MyCommonTexts.BlueprintsScreen_Description : MyCommonTexts.ScriptsScreen_Description), MyGameService.Service.ServiceName)),
				Font = "Blue"
			};
			Controls.Add(control);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.5f * m_size.Value.X + num, -0.39f), m_size.Value.X - 2f * num);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.5f * m_size.Value.X + num, -0.299999982f), m_size.Value.X - 2f * num);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.1715f, -0.225000009f), m_size.Value.X - 0.3245f);
			m_separator = new MyGuiControlSeparatorList();
			m_separator.AddHorizontal(new Vector2(-0.1715f, 0.232f), m_size.Value.X - 0.3245f);
			m_separator.Visible = (m_selectedTab == Tab.Edit);
			Controls.Add(m_separator);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.1715f, 0.374f), m_size.Value.X - 0.3245f);
			Controls.Add(myGuiControlSeparatorList);
			MyStringId id = MySpaceTexts.ScreenBlueprintsRew_Caption;
			switch (m_content)
			{
			case Content.Blueprint:
				id = MySpaceTexts.ScreenBlueprintsRew_Caption_Blueprint;
				break;
			case Content.Script:
				id = MySpaceTexts.ScreenBlueprintsRew_Caption_Script;
				break;
			}
			AddCaption(MyTexts.GetString(id), Color.White.ToVector4(), new Vector2(0f, 0.02f));
			m_detailName = AddCaption("Blueprint Name", Color.White.ToVector4(), new Vector2(0.1035f, 0.233f));
			m_multiline = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable: false, showTextShadow: false, MyGuiConstants.TEXTURE_RECTANGLE_DARK, new MyGuiBorderThickness(0.005f, 0f, 0f, 0f));
			m_multiline.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			Controls.Add(m_multiline);
			m_detailsBackground = new MyGuiControlPanel();
			m_detailsBackground.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			Controls.Add(m_detailsBackground);
			m_searchBox = new MyGuiControlSearchBox(new Vector2(-0.382f, -0.21f), new Vector2(m_BPList.Size.X, 0.032f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_searchBox.OnTextChanged += OnSearchTextChange;
			Controls.Add(m_searchBox);
			m_detailBlockCount = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NumOfBlocks), string.Empty));
			Controls.Add(m_detailBlockCount);
			m_detailBlockCountValue = new MyGuiControlLabel(null, null, "0");
			Controls.Add(m_detailBlockCountValue);
			m_detailSize = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_GridType), string.Empty));
			Controls.Add(m_detailSize);
			m_detailSizeValue = new MyGuiControlLabel(null, null, "Unknown");
			Controls.Add(m_detailSizeValue);
			m_detailAuthor = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Author), string.Empty));
			Controls.Add(m_detailAuthor);
			m_detailAuthorName = new MyGuiControlLabel(null, null, "N/A");
			Controls.Add(m_detailAuthorName);
			m_detailDLC = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Dlc), string.Empty));
			Controls.Add(m_detailDLC);
			m_detailSendTo = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_PCU), string.Empty));
			Controls.Add(m_detailSendTo);
			UpdatePrefab(null, loadPrefab: false);
			UpdateInfo(null, null);
			m_sendToCombo = AddCombo(null, null, new Vector2(0.16f, 0.1f));
			m_sendToCombo.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Tooltip_SendToPlayer), MyGameService.Service.ServiceName));
			m_sendToCombo.AddItem(0L, new StringBuilder("   "));
			foreach (MyNetworkClient client in Sync.Clients.GetClients())
			{
				if (client.SteamUserId != Sync.MyId)
				{
					m_sendToCombo.AddItem(Convert.ToInt64(client.SteamUserId), new StringBuilder(client.DisplayName));
				}
			}
			m_sendToCombo.ItemSelected += OnSendToPlayer;
			CreateButtons();
			RecomputeTabSelection();
			Controls.Add(m_BPList);
			switch (m_content)
			{
			case Content.Blueprint:
				m_detailSendTo.Visible = true;
				break;
			case Content.Script:
				m_sendToCombo.Visible = false;
				m_detailAuthor.Visible = false;
				m_detailAuthorName.Visible = false;
				m_detailBlockCount.Visible = false;
				m_detailBlockCountValue.Visible = false;
				m_detailSize.Visible = false;
				m_detailSizeValue.Visible = false;
				m_detailDLC.Visible = false;
				m_detailSendTo.Visible = false;
				break;
			}
			m_searchBox.Position = new Vector2(m_button_Refresh.Position.X - m_button_Refresh.Size.X * 0.5f - 0.002f, m_searchBox.Position.Y);
			m_searchBox.Size = new Vector2(m_button_OpenWorkshop.Position.X + m_button_OpenWorkshop.Size.X - m_button_Refresh.Position.X, m_searchBox.Size.Y);
			m_BPList.Position = new Vector2(m_searchBox.Position.X, m_BPList.Position.Y);
			m_BPList.Size = new Vector2(m_searchBox.Size.X, m_BPList.Size.Y);
			RefreshThumbnail();
			Controls.Add(m_thumbnailImage);
			RepositionDetailedPage();
			SetDetailPageTexts();
			UpdateDetailKeyEnable();
			base.FocusedControl = m_searchBox.TextBox;
			SwitchTabToInfo();
			RecomputeTabSelection();
		}

		private void CreateButtons()
		{
			Vector2 value = new Vector2(-0.37f, -0.27f);
			Vector2 value2 = new Vector2(-0.0955f, -0.27f);
			Vector2 value3 = new Vector2(-0.0812f, 0.255f);
			Vector2 value4 = new Vector2(-0.0812f, 0.395f);
			new Vector2(0.144f, 0.035f);
			float num = 0.029f;
			float num2 = 0.178f;
			float textScale = 0.8f;
			m_button_Refresh = MyBlueprintUtils.CreateButton(this, num, null, OnButton_Refresh, enabled: true, null, textScale);
			m_button_Refresh.Position = value + new Vector2(num, 0f) * 0f;
			m_button_Refresh.ShowTooltipWhenDisabled = true;
			m_button_GroupSelection = MyBlueprintUtils.CreateButton(this, num, null, OnButton_GroupSelection, enabled: true, null, textScale);
			m_button_GroupSelection.Position = value + new Vector2(num, 0f) * 1f;
			m_button_GroupSelection.ShowTooltipWhenDisabled = true;
			m_button_Sorting = MyBlueprintUtils.CreateButton(this, num, null, OnButton_Sorting, enabled: true, null, textScale);
			m_button_Sorting.Position = value + new Vector2(num, 0f) * 2f;
			m_button_Sorting.ShowTooltipWhenDisabled = true;
			m_button_NewBlueprint = MyBlueprintUtils.CreateButton(this, num, null, OnButton_NewBlueprint, enabled: true, null, textScale);
			m_button_NewBlueprint.Position = value + new Vector2(num, 0f) * 3f;
			m_button_NewBlueprint.ShowTooltipWhenDisabled = true;
			m_button_DirectorySelection = MyBlueprintUtils.CreateButton(this, num, null, OnButton_DirectorySelection, enabled: true, null, textScale);
			m_button_DirectorySelection.Position = value + new Vector2(num, 0f) * 4f;
			m_button_DirectorySelection.ShowTooltipWhenDisabled = true;
			m_button_HideThumbnails = MyBlueprintUtils.CreateButton(this, num, null, OnButton_HideThumbnails, enabled: true, null, textScale);
			m_button_HideThumbnails.Position = value + new Vector2(num, 0f) * 5f;
			m_button_HideThumbnails.ShowTooltipWhenDisabled = true;
			m_button_OpenWorkshop = MyBlueprintUtils.CreateButton(this, num, null, OnButton_OpenWorkshop, enabled: true, null, textScale);
			m_button_OpenWorkshop.Position = value + new Vector2(num, 0f) * 6f;
			m_button_OpenWorkshop.ShowTooltipWhenDisabled = true;
			float num3 = 0.1502911f;
			m_button_TabInfo = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_TabInfo, enabled: true, null, textScale);
			m_button_TabInfo.Position = value2 + new Vector2(num3, 0f) * 0f;
			m_button_TabInfo.Size = new Vector2(num3, num * 4f / 3f);
			m_button_TabInfo.ShowTooltipWhenDisabled = true;
			m_button_TabInfo.CanHaveFocus = false;
			m_button_TabEdit = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_TabEdit, enabled: true, null, textScale);
			m_button_TabEdit.Position = value2 + new Vector2(num3 + MyGuiConstants.GENERIC_BUTTON_SPACING.X, 0f) * 1f;
			m_button_TabEdit.Size = new Vector2(num3, num * 4f / 3f);
			m_button_TabEdit.ShowTooltipWhenDisabled = true;
			m_button_TabEdit.CanHaveFocus = false;
			float num4 = 0.01f;
			m_button_Rename = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Rename, enabled: true, null, textScale);
			m_button_Rename.Position = value3 + new Vector2(num2 + num4, 0f) * 0f;
			m_button_Rename.Size = new Vector2(m_button_Rename.Size.X, m_button_Rename.Size.Y * 1.3f);
			m_button_Rename.ShowTooltipWhenDisabled = true;
			m_button_Replace = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Replace, enabled: true, null, textScale);
			m_button_Replace.Position = value3 + new Vector2(num2 + num4, 0f) * 1f;
			m_button_Replace.Size = new Vector2(m_button_Replace.Size.X, m_button_Replace.Size.Y * 1.3f);
			m_button_Replace.ShowTooltipWhenDisabled = true;
			m_button_Delete = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Delete, enabled: true, null, textScale);
			m_button_Delete.Position = value3 + new Vector2(num2 + num4, 0f) * 2f;
			m_button_Delete.Size = new Vector2(m_button_Delete.Size.X, m_button_Delete.Size.Y * 1.3f);
			m_button_Delete.ShowTooltipWhenDisabled = true;
			m_button_TakeScreenshot = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_TakeScreenshot, enabled: true, null, textScale);
			m_button_TakeScreenshot.Position = value3 + new Vector2(num2 + num4, 0f) * 1f + new Vector2(0f, 0.055f);
			m_button_TakeScreenshot.Size = new Vector2(m_button_TakeScreenshot.Size.X, m_button_TakeScreenshot.Size.Y * 1.3f);
			m_button_TakeScreenshot.ShowTooltipWhenDisabled = true;
			m_button_Publish = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_Publish, enabled: true, null, textScale);
			m_button_Publish.Position = value3 + new Vector2(num2 + num4, 0f) * 2f + new Vector2(0f, 0.055f);
			m_button_Publish.Size = new Vector2(m_button_Publish.Size.X, m_button_Publish.Size.Y * 1.3f);
			m_button_Publish.ShowTooltipWhenDisabled = true;
			m_button_OpenInWorkshop = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_OpenInWorkshop, enabled: true, null, textScale);
			m_button_OpenInWorkshop.Position = value4 + new Vector2(num2 + num4, 0f) * 1f;
			m_button_OpenInWorkshop.Size = new Vector2(m_button_OpenInWorkshop.Size.X, m_button_OpenInWorkshop.Size.Y * 1.3f);
			m_button_OpenInWorkshop.ShowTooltipWhenDisabled = true;
			m_button_CopyToClipboard = MyBlueprintUtils.CreateButton(this, num2, null, OnButton_CopyToClipboard, enabled: true, null, textScale);
			m_button_CopyToClipboard.Position = value4 + new Vector2(num2 + num4, 0f) * 2f;
			m_button_CopyToClipboard.Size = new Vector2(m_button_CopyToClipboard.Size.X, m_button_CopyToClipboard.Size.Y * 1.3f);
			m_button_CopyToClipboard.ShowTooltipWhenDisabled = true;
			base.CloseButtonEnabled = true;
			m_icon_Refresh = CreateButtonIcon(m_button_Refresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			m_icon_GroupSelection = CreateButtonIcon(m_button_GroupSelection, "");
			SetIconForGroupSelection();
			m_icon_Sorting = CreateButtonIcon(m_button_Sorting, "");
			SetIconForSorting();
			m_icon_OpenWorkshop = CreateButtonIcon(m_button_OpenWorkshop, "Textures\\GUI\\Icons\\Blueprints\\Steam.png");
			m_icon_DirectorySelection = CreateButtonIcon(m_button_NewBlueprint, "Textures\\GUI\\Icons\\Blueprints\\BP_New.png");
			m_icon_NewBlueprint = CreateButtonIcon(m_button_DirectorySelection, "Textures\\GUI\\Icons\\Blueprints\\FolderIcon.png");
			m_icon_HideThumbnails = CreateButtonIcon(m_button_HideThumbnails, "");
			SetIconForHideThubnails();
		}

		private MyGuiControlImage CreateButtonIcon(MyGuiControlButton butt, string texture)
		{
			butt.Size = new Vector2(butt.Size.X, butt.Size.X * 4f / 3f);
			float num = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2(num * 0.75f, num), position: butt.Position + new Vector2(-0.0016f, 0.018f), backgroundColor: null, backgroundTexture: null, textures: new string[1]
			{
				texture
			});
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		private void RecomputeTabSelection()
		{
			if (m_button_TabInfo != null)
			{
				m_button_TabInfo.HasHighlight = (m_selectedTab == Tab.Info);
				m_button_TabInfo.Selected = (m_selectedTab == Tab.Info);
			}
			if (m_button_TabEdit != null)
			{
				m_button_TabEdit.HasHighlight = (m_selectedTab == Tab.Edit);
				m_button_TabEdit.Selected = (m_selectedTab == Tab.Edit);
			}
		}

		private void RecomputeDetailOffsets()
		{
			switch (m_selectedTab)
			{
			case Tab.Info:
				m_guiMultilineHeight = 0.412f;
				m_guiAdditionalInfoOffset = 0.271f;
				break;
			case Tab.Edit:
				m_guiMultilineHeight = 0.272f;
				m_guiAdditionalInfoOffset = 0.131f;
				break;
			default:
				m_guiMultilineHeight = 0.382f;
				m_guiAdditionalInfoOffset = 0.285f;
				break;
			}
		}

		private void RepositionDetailedPage()
		{
			Vector2 vector = new Vector2(-0.168f, m_guiAdditionalInfoOffset);
			Vector2 value = new Vector2(-0.173f, -0.2655f);
			Vector2 vector2 = new Vector2(m_size.Value.X - 0.3245f, m_guiMultilineHeight);
			Vector2 value2 = Vector2.Zero;
			switch (m_selectedTab)
			{
			case Tab.Info:
				m_button_Rename.Visible = false;
				m_button_Replace.Visible = false;
				m_button_Delete.Visible = false;
				m_button_TakeScreenshot.Visible = false;
				m_button_Publish.Visible = false;
				m_separator.Visible = false;
				value2 = new Vector2(0.394f, 0f) + new Vector2(-0.024f, 0.04f);
				break;
			case Tab.Edit:
				m_button_Rename.Visible = true;
				m_button_Replace.Visible = true;
				m_button_Delete.Visible = true;
				m_button_TakeScreenshot.Visible = (m_content == Content.Blueprint);
				m_button_Publish.Visible = true;
				m_separator.Visible = true;
				value2 = new Vector2(0.394f, 0f) + new Vector2(-0.024f, 0.04f);
				break;
			}
			m_multiline.Position = value + 0.5f * vector2 + new Vector2(0f, 0.09f);
			m_multiline.Size = vector2;
			float x = m_detailBlockCount.Position.X + Math.Max(Math.Max(m_detailBlockCount.Size.X, m_detailSize.Size.X), m_detailAuthor.Size.X) + 0.001f;
			m_detailAuthor.Position = vector + new Vector2(0f, 0f);
			m_detailBlockCount.Position = vector + new Vector2(0f, 0.03f);
			m_detailSize.Position = vector + new Vector2(0f, 0.06f);
			m_detailBlockCountValue.Position = new Vector2(x, m_detailBlockCount.Position.Y);
			m_detailSizeValue.Position = new Vector2(x, m_detailSize.Position.Y);
			m_detailAuthorName.Position = new Vector2(x, m_detailAuthor.Position.Y);
			m_detailDLC.Position = vector + new Vector2(0.27f, 0f);
			m_detailSendTo.Position = vector + new Vector2(0.27f, 0.06f);
			m_sendToCombo.Position = vector + value2;
			m_sendToCombo.Size = m_button_CopyToClipboard.Size * 0.978f;
			vector2 = m_sendToCombo.Position - vector;
			m_detailsBackground.Position = m_multiline.Position + new Vector2(0f, m_multiline.Size.Y / 2f + 0.0715f);
			m_detailsBackground.Size = new Vector2(m_multiline.Size.X, vector2.Y + m_sendToCombo.Size.Y + 0.02f);
			foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
			{
				Vector2 position = dlcIcon.Position;
				position.Y = vector.Y;
				dlcIcon.Position = position;
			}
		}

		private void SetDetailPageTexts()
		{
			m_button_Refresh.Text = null;
			m_button_Refresh.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRefresh);
			m_button_GroupSelection.Text = null;
			m_button_GroupSelection.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping), MyGameService.Service.ServiceName));
			m_button_Sorting.Text = null;
			m_button_Sorting.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort);
			m_button_OpenWorkshop.Text = null;
			m_button_OpenWorkshop.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop);
			m_button_DirectorySelection.Text = null;
			m_button_DirectorySelection.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButFolders);
			m_button_HideThumbnails.Text = null;
			m_button_HideThumbnails.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButVisibility);
			m_button_TabInfo.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButInfo);
			m_button_TabInfo.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButInfo);
			m_button_TabEdit.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButEdit);
			m_button_TabEdit.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButEdit);
			m_button_Rename.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButRename);
			m_button_Rename.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRename);
			m_button_Replace.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButReplace);
			m_button_Replace.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButReplace);
			m_button_Delete.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButDelete);
			m_button_Delete.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButDelete);
			m_button_TakeScreenshot.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButScreenshot);
			m_button_TakeScreenshot.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButScreenshot);
			m_button_Publish.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButPublish);
			m_button_Publish.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButPublish), MyGameService.Service.ServiceName));
			m_button_OpenInWorkshop.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButOpenInWorkshop);
			m_button_OpenInWorkshop.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenInWorkshop), MyGameService.Service.ServiceName));
			switch (m_content)
			{
			case Content.Blueprint:
				m_button_NewBlueprint.Text = null;
				m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewBlueprint);
				m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToClipboard);
				m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToClipboard);
				break;
			case Content.Script:
				m_button_NewBlueprint.Text = null;
				m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewScript);
				m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToEditor);
				m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToEditor);
				break;
			}
		}

		private void SetIconForGroupSelection()
		{
			switch (GetSelectedBlueprintType())
			{
			case MyBlueprintTypeEnum.MIXED:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Mixed.png");
				break;
			case MyBlueprintTypeEnum.LOCAL:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Local.png");
				break;
			case MyBlueprintTypeEnum.STEAM:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Steam.png");
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Cloud.png");
				break;
			default:
				m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Mixed.png");
				break;
			}
		}

		private void SetIconForSorting()
		{
			switch (GetSelectedSort())
			{
			case SortOption.None:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			case SortOption.Alphabetical:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
				break;
			case SortOption.CreationDate:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
				break;
			case SortOption.UpdateDate:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
				break;
			default:
				m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			}
		}

		private void SetIconForHideThubnails()
		{
			if (GetThumbnailVisibility())
			{
				m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png");
			}
			else
			{
				m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");
			}
		}

		private void UpdateInfo(Stream sbcStream, MyBlueprintItemInfo data)
		{
			int num = 0;
			string text = string.Empty;
			string text2 = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
			MyBlueprintItemInfo selectedBlueprint = m_selectedBlueprint;
			MyGuiControlContentButton selectedButton = m_selectedButton;
			if (data != null && m_selectedBlueprint != null && data.Equals(m_selectedBlueprint))
			{
				switch (m_content)
				{
				case Content.Blueprint:
				{
					XDocument xDocument = XDocument.Load(sbcStream);
					IEnumerable<XElement> enumerable = xDocument.Descendants("GridSizeEnum");
					IEnumerable<XElement> enumerable2 = xDocument.Descendants("DisplayName");
					IEnumerable<XElement> enumerable3 = xDocument.Descendants("CubeBlocks");
					IEnumerable<XElement> enumerable4 = xDocument.Descendants("DLC");
					text2 = ((enumerable != null && enumerable.Count() > 0) ? ((string)enumerable.First()) : "N/A");
					text = ((enumerable2 != null && enumerable2.Count() > 0) ? ((string)enumerable2.First()) : "N/A");
					num = 0;
					if (enumerable3 != null && enumerable3.Count() > 0)
					{
						foreach (XElement item in enumerable3)
						{
							num += item.Elements().Count();
						}
					}
					if (enumerable4 != null)
					{
						HashSet<uint> hashSet = new HashSet<uint>();
						foreach (XElement item2 in enumerable4)
						{
							if (!string.IsNullOrEmpty(item2.Value) && MyDLCs.TryGetDLC(item2.Value, out MyDLCs.MyDLC dlc))
							{
								hashSet.Add(dlc.AppId);
							}
						}
						if (hashSet.Count > 0)
						{
							selectedBlueprint.Data.DLCs = hashSet.ToArray();
						}
					}
					break;
				}
				case Content.Script:
					text = ((data.Item != null) ? data.Item.OwnerId.ToString() : "N/A");
					break;
				}
			}
			if (selectedBlueprint == m_selectedBlueprint && selectedButton == m_selectedButton)
			{
				m_detailDLC.Visible = false;
				foreach (MyGuiControlImage dlcIcon in m_dlcIcons)
				{
					Controls.Remove(dlcIcon);
				}
				m_dlcIcons.Clear();
				if (selectedBlueprint != null && !selectedBlueprint.Data.DLCs.IsNullOrEmpty())
				{
					m_detailDLC.Visible = true;
					Vector2 position = new Vector2(m_sendToCombo.Position.X, m_detailDLC.Position.Y);
					uint[] dLCs = selectedBlueprint.Data.DLCs;
					foreach (uint id in dLCs)
					{
						if (MyDLCs.TryGetDLC(id, out MyDLCs.MyDLC dlc2))
						{
							MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1]
							{
								dlc2.Icon
							}, MyDLCs.GetRequiredDLCTooltip(id))
							{
								OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
								Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE
							};
							myGuiControlImage.Position = position;
							position.X += myGuiControlImage.Size.X + 0.002f;
							m_dlcIcons.Add(myGuiControlImage);
							Controls.Add(myGuiControlImage);
						}
					}
				}
				m_detailBlockCountValue.Text = num.ToString();
				m_detailSizeValue.Text = text2;
				m_detailAuthorName.Text = text;
				m_detailSendTo.Text = MyTexts.GetString(MySpaceTexts.BlueprintInfo_SendTo);
				float x = m_detailBlockCount.Position.X + Math.Max(Math.Max(m_detailBlockCount.Size.X, m_detailSize.Size.X), m_detailAuthor.Size.X) + 0.001f;
				m_detailBlockCountValue.Position = new Vector2(x, m_detailBlockCount.Position.Y);
				m_detailSizeValue.Position = new Vector2(x, m_detailSize.Position.Y);
				m_detailAuthorName.Position = new Vector2(x, m_detailAuthor.Position.Y);
			}
			if (m_loadedPrefab != null)
			{
				UpdateDetailKeyEnable();
			}
		}

		public void UpdateDetailKeyEnable()
		{
			if (m_selectedBlueprint == null)
			{
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = false;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				return;
			}
			switch (m_selectedBlueprint.Type)
			{
			case MyBlueprintTypeEnum.STEAM:
				m_button_OpenInWorkshop.Enabled = true;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = true;
				break;
			case MyBlueprintTypeEnum.SHARED:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.LOCAL:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = true;
				m_button_Replace.Enabled = true;
				m_button_Delete.Enabled = true;
				m_button_TakeScreenshot.Enabled = true;
				m_button_Publish.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = true;
				m_button_Delete.Enabled = true;
				m_button_TakeScreenshot.Enabled = true;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			case MyBlueprintTypeEnum.DEFAULT:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = true;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			default:
				m_button_OpenInWorkshop.Enabled = false;
				m_button_CopyToClipboard.Enabled = false;
				m_button_Rename.Enabled = false;
				m_button_Replace.Enabled = false;
				m_button_Delete.Enabled = false;
				m_button_TakeScreenshot.Enabled = false;
				m_button_Publish.Enabled = false;
				m_sendToCombo.Enabled = false;
				break;
			}
		}

		private void TogglePreviewVisibility()
		{
			foreach (MyGuiControlBase control in m_BPList.Controls)
			{
				(control as MyGuiControlContentButton)?.SetPreviewVisibility(GetThumbnailVisibility());
			}
			m_BPList.Recalculate();
		}

		private void AddBlueprintButton(MyBlueprintItemInfo data, bool isLocalMod = false, bool isWorkshopMod = false, bool filter = false)
		{
			string blueprintName = data.BlueprintName;
			string imagePath = GetImagePath(data);
			if (File.Exists(imagePath))
			{
				MyRenderProxy.PreloadTextures(new List<string>
				{
					imagePath
				}, TextureType.GUIWithoutPremultiplyAlpha);
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(blueprintName, imagePath)
			{
				UserData = data,
				IsLocalMod = isLocalMod,
				IsWorkshopMod = isWorkshopMod,
				Key = m_BPTypesGroup.Count
			};
			myGuiControlContentButton.MouseOverChanged += OnMouseOverItem;
			myGuiControlContentButton.FocusChanged += OnFocusedItem;
			myGuiControlContentButton.SetTooltip(blueprintName);
			myGuiControlContentButton.SetPreviewVisibility(GetThumbnailVisibility());
			m_BPTypesGroup.Add(myGuiControlContentButton);
			m_BPList.Controls.Add(myGuiControlContentButton);
			if (filter)
			{
				ApplyFiltering(myGuiControlContentButton);
			}
		}

		private void AddBlueprintButtons(ref List<MyBlueprintItemInfo> data, bool isLocalMod = false, bool isWorkshopMod = false, bool filter = false)
		{
			m_preloadedTextures.Clear();
			List<string> list = new List<string>();
			for (int i = 0; i < data.Count; i++)
			{
				string imagePath = GetImagePath(data[i]);
				list.Add(imagePath);
				if (File.Exists(imagePath))
				{
					m_preloadedTextures.Add(imagePath);
					MyRenderProxy.PreloadTextures(m_preloadedTextures, TextureType.GUIWithoutPremultiplyAlpha);
				}
			}
			for (int j = 0; j < data.Count; j++)
			{
				string name = data[j].Data.Name;
				MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(name, File.Exists(list[j]) ? list[j] : "")
				{
					UserData = data[j],
					IsLocalMod = isLocalMod,
					IsWorkshopMod = isWorkshopMod,
					Key = m_BPTypesGroup.Count
				};
				if (m_showDlcIcons)
				{
					if (data[j].Item != null && data[j].Item.DLCs.Count > 0)
					{
						foreach (uint dLC in data[j].Item.DLCs)
						{
							string dLCIcon = MyDLCs.GetDLCIcon(dLC);
							if (!string.IsNullOrEmpty(dLCIcon))
							{
								myGuiControlContentButton.AddDlcIcon(dLCIcon);
							}
						}
					}
					else if (data[j].Data.DLCs != null && data[j].Data.DLCs.Length != 0)
					{
						uint[] dLCs = data[j].Data.DLCs;
						for (int k = 0; k < dLCs.Length; k++)
						{
							string dLCIcon2 = MyDLCs.GetDLCIcon(dLCs[k]);
							if (!string.IsNullOrEmpty(dLCIcon2))
							{
								myGuiControlContentButton.AddDlcIcon(dLCIcon2);
							}
						}
					}
				}
				myGuiControlContentButton.MouseOverChanged += OnMouseOverItem;
				myGuiControlContentButton.FocusChanged += OnFocusedItem;
				myGuiControlContentButton.SetTooltip(name);
				myGuiControlContentButton.SetPreviewVisibility(GetThumbnailVisibility());
				m_BPTypesGroup.Add(myGuiControlContentButton);
				m_BPList.Controls.Add(myGuiControlContentButton);
				if (filter)
				{
					ApplyFiltering(myGuiControlContentButton);
				}
			}
		}

		private void TrySelectFirstBlueprint()
		{
			if (m_BPTypesGroup.Count > 0)
			{
				if (!m_BPTypesGroup.SelectedIndex.HasValue)
				{
					m_BPTypesGroup.SelectByIndex(0);
				}
				return;
			}
			m_multiline.Clear();
			MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
			if (selectedBlueprintType == MyBlueprintTypeEnum.STEAM || selectedBlueprintType == MyBlueprintTypeEnum.MIXED)
			{
				m_multiline.AppendText(string.Format(MyTexts.GetString((m_content == Content.Blueprint) ? MySpaceTexts.ScreenBlueprintsRew_NoWorkshopBlueprints : MySpaceTexts.ScreenBlueprintsRew_NoWorkshopScripts), MyGameService.Service.ServiceName), "Blue", m_multiline.TextScale, Vector4.One);
				m_multiline.AppendLine();
				m_multiline.AppendLink(MyGameService.WorkshopService.GetItemListUrl((m_content == Content.Blueprint) ? MySteamConstants.TAG_BLUEPRINTS : MySteamConstants.TAG_SCRIPTS), "Space Engineers " + MyGameService.WorkshopService.ServiceName + " Workshop");
				m_multiline.AppendLine();
				m_multiline.OnLinkClicked += OnLinkClicked;
			}
			else
			{
				m_multiline.AppendText(MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_NoBlueprints), "Blue", m_multiline.TextScale, Vector4.One);
			}
			m_multiline.ScrollbarOffsetV = 1f;
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Steam Workshop");
		}

		private void OnButton_Refresh(MyGuiControlButton button)
		{
			bool flag = false;
			MyBlueprintItemInfo itemInfo = null;
			if (m_selectedBlueprint != null)
			{
				flag = true;
				itemInfo = m_selectedBlueprint;
			}
			m_selectedButton = null;
			m_selectedBlueprint = null;
			UpdateDetailKeyEnable();
			m_downloadFinished.Clear();
			m_downloadQueued.Clear();
			RefreshAndReloadItemList();
			TrySelectFirstBlueprint();
			if (flag)
			{
				SelectBlueprint(itemInfo);
			}
			UpdateDetailKeyEnable();
		}

		private void OnButton_GroupSelection(MyGuiControlButton button)
		{
			MyBlueprintTypeEnum groupSelection = MyBlueprintTypeEnum.MIXED;
			switch (GetSelectedBlueprintType())
			{
			case MyBlueprintTypeEnum.MIXED:
				groupSelection = MyBlueprintTypeEnum.LOCAL;
				break;
			case MyBlueprintTypeEnum.LOCAL:
				groupSelection = MyBlueprintTypeEnum.STEAM;
				break;
			case MyBlueprintTypeEnum.STEAM:
				groupSelection = MyBlueprintTypeEnum.CLOUD;
				break;
			case MyBlueprintTypeEnum.CLOUD:
				groupSelection = MyBlueprintTypeEnum.MIXED;
				break;
			}
			SetGroupSelection(groupSelection);
		}

		private void OnButton_Sorting(MyGuiControlButton button)
		{
			switch (GetSelectedSort())
			{
			case SortOption.None:
				SetSelectedSort(SortOption.Alphabetical);
				break;
			case SortOption.Alphabetical:
				SetSelectedSort(SortOption.CreationDate);
				break;
			case SortOption.CreationDate:
				SetSelectedSort(SortOption.UpdateDate);
				break;
			case SortOption.UpdateDate:
				SetSelectedSort(SortOption.None);
				break;
			}
			SetIconForSorting();
			OnReload(null);
		}

		private void OnButton_OpenWorkshop(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_BLUEPRINTS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}

		private void OnButton_NewBlueprint(MyGuiControlButton button)
		{
			switch (m_content)
			{
			case Content.Blueprint:
				CreateBlueprintFromClipboard(withScreenshot: true);
				break;
			case Content.Script:
				CreateScriptFromEditor();
				break;
			}
		}

		private void OnButton_DirectorySelection(MyGuiControlButton button)
		{
			string rootPath = string.Empty;
			Func<string, bool> isItem = null;
			switch (m_content)
			{
			case Content.Blueprint:
				rootPath = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
				isItem = MyBlueprintUtils.IsItem_Blueprint;
				break;
			case Content.Script:
				rootPath = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
				isItem = MyBlueprintUtils.IsItem_Script;
				break;
			}
			MyGuiSandbox.AddScreen(new MyGuiFolderScreen(hideOthers: false, OnPathSelected, rootPath, GetCurrentLocalDirectory(), isItem));
		}

		private void OnButton_HideThumbnails(MyGuiControlButton button)
		{
			SetThumbnailVisibility(!GetThumbnailVisibility());
			SetIconForHideThubnails();
			TogglePreviewVisibility();
		}

		private void OnButton_TabInfo(MyGuiControlButton button)
		{
			SwitchTabToInfo();
		}

		private void SwitchTabToInfo()
		{
			m_button_TabInfo.HighlightType = MyGuiControlHighlightType.FORCED;
			m_button_TabEdit.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
			m_selectedTab = Tab.Info;
			RecomputeDetailOffsets();
			RecomputeTabSelection();
			RepositionDetailedPage();
		}

		private void OnButton_TabEdit(MyGuiControlButton button)
		{
			SwitchTabToEdit();
		}

		private void SwitchTabToEdit()
		{
			m_button_TabInfo.HighlightType = MyGuiControlHighlightType.WHEN_ACTIVE;
			m_button_TabEdit.HighlightType = MyGuiControlHighlightType.FORCED;
			m_selectedTab = Tab.Edit;
			RecomputeDetailOffsets();
			RecomputeTabSelection();
			RepositionDetailedPage();
		}

		private void OnButton_OpenInWorkshop(MyGuiControlButton button)
		{
			if (m_publishedItemId.HasValue)
			{
				MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemUrl(m_publishedItemId.Value), MyGameService.WorkshopService.ServiceName + " Workshop");
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Invalid workshop id"), messageText: new StringBuilder("")));
			}
		}

		private void OnButton_CopyToClipboard(MyGuiControlButton button)
		{
			CopyToClipboard();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				if (m_selectedTab == Tab.Info)
				{
					SwitchTabToEdit();
				}
				else if (m_selectedTab == Tab.Edit)
				{
					SwitchTabToInfo();
				}
			}
		}

		private void CopyToClipboard()
		{
			if (m_selectedBlueprint == null)
			{
				return;
			}
			switch (m_content)
			{
			case Content.Blueprint:
				if (m_blueprintBeingLoaded)
				{
					break;
				}
				if (m_selectedBlueprint.IsDirectory)
				{
					if (string.IsNullOrEmpty(m_selectedBlueprint.BlueprintName))
					{
						string[] array = GetCurrentLocalDirectory().Split(new char[1]
						{
							Path.DirectorySeparatorChar
						});
						if (array.Length > 1)
						{
							array[array.Length - 1] = string.Empty;
							SetCurrentLocalDirectory(Path.Combine(array));
						}
						else
						{
							SetCurrentLocalDirectory(string.Empty);
						}
					}
					else
					{
						SetCurrentLocalDirectory(Path.Combine(GetCurrentLocalDirectory(), m_selectedBlueprint.BlueprintName));
					}
					CheckCurrentLocalDirectory_Blueprint();
					RefreshAndReloadItemList();
					break;
				}
				m_blueprintBeingLoaded = true;
				switch (m_selectedBlueprint.Type)
				{
				case MyBlueprintTypeEnum.STEAM:
					m_blueprintBeingLoaded = true;
					Task = Parallel.Start(delegate
					{
						if (!MyWorkshop.IsUpToDate(m_selectedBlueprint.Item))
						{
							DownloadBlueprintFromSteam(m_selectedBlueprint.Item);
						}
					}, delegate
					{
						CopyBlueprintAndClose();
					});
					break;
				case MyBlueprintTypeEnum.LOCAL:
				case MyBlueprintTypeEnum.DEFAULT:
				case MyBlueprintTypeEnum.CLOUD:
					m_blueprintBeingLoaded = true;
					CopyBlueprintAndClose();
					break;
				case MyBlueprintTypeEnum.SHARED:
					OpenSharedBlueprint(m_selectedBlueprint);
					break;
				}
				break;
			case Content.Script:
				OpenSelectedSript();
				break;
			}
		}

		private void OnButton_Rename(MyGuiControlButton button)
		{
			if (m_selectedBlueprint != null)
			{
				MyScreenManager.AddScreen(new MyGuiBlueprintTextDialog(m_position, delegate(string result)
				{
					if (result != null)
					{
						ChangeName(result);
					}
				}, caption: MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), defaultName: m_selectedBlueprint.Data.Name, maxLenght: 40, textBoxWidth: 0.3f));
			}
		}

		private void OnButton_Replace(MyGuiControlButton button)
		{
			if (m_selectedBlueprint == null)
			{
				return;
			}
			if (m_selectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD && !MySandboxGame.Config.EnableSteamCloud)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff), MyGameService.Service.ServiceName)), new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff_Caption), MyGameService.Service.ServiceName))));
				return;
			}
			if (m_selectedBlueprint.Type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn), MyGameService.Service.ServiceName)), new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn_Caption), MyGameService.Service.ServiceName))));
				return;
			}
			switch (m_content)
			{
			case Content.Blueprint:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && m_clipboard != null && m_clipboard.CopiedGrids != null && m_clipboard.CopiedGrids.Count != 0)
					{
						string name = m_selectedBlueprint.Data.Name;
						string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), name, "bp.sbc");
						if (File.Exists(text))
						{
							MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefab(text);
							m_clipboard.CopiedGrids[0].DisplayName = name;
							myObjectBuilder_Definitions.ShipBlueprints[0].CubeGrids = m_clipboard.CopiedGrids.ToArray();
							myObjectBuilder_Definitions.ShipBlueprints[0].DLCs = GetNecessaryDLCs(myObjectBuilder_Definitions.ShipBlueprints[0].CubeGrids);
							MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, GetCurrentLocalDirectory(), replace: true);
							RefreshBlueprintList();
						}
					}
				}));
				break;
			case Content.Script:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, m_selectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
						if (File.Exists(path))
						{
							string contents = m_getCodeFromEditor();
							File.WriteAllText(path, contents, Encoding.UTF8);
						}
					}
				}));
				break;
			}
		}

		private void OnButton_Delete(MyGuiControlButton button)
		{
			if (m_selectedBlueprint != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Delete), messageText: MyTexts.Get(MySpaceTexts.DeleteBlueprintQuestion), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && m_selectedBlueprint != null)
					{
						switch (m_content)
						{
						case Content.Blueprint:
							switch (m_selectedBlueprint.Type)
							{
							case MyBlueprintTypeEnum.LOCAL:
							case MyBlueprintTypeEnum.DEFAULT:
							{
								string path2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), m_selectedBlueprint.BlueprintName);
								if (DeleteItem(path2))
								{
									m_selectedBlueprint = null;
									ResetBlueprintUI();
								}
								break;
							}
							case MyBlueprintTypeEnum.CLOUD:
							{
								string text = MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY + "/" + m_selectedBlueprint.BlueprintName + "/" + MyBlueprintUtils.BLUEPRINT_LOCAL_NAME;
								if (MyGameService.DeleteFromCloud(text))
								{
									m_selectedBlueprint = null;
									ResetBlueprintUI();
									text += "B5";
									MyGameService.DeleteFromCloud(text);
								}
								break;
							}
							}
							break;
						case Content.Script:
						{
							string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), m_selectedBlueprint.Data.Name);
							if (DeleteItem(path))
							{
								m_selectedBlueprint = null;
								ResetBlueprintUI();
							}
							break;
						}
						}
						RefreshBlueprintList();
					}
				}));
			}
		}

		private void OnButton_TakeScreenshot(MyGuiControlButton button)
		{
			if (m_selectedBlueprint != null)
			{
				switch (m_selectedBlueprint.Type)
				{
				case MyBlueprintTypeEnum.LOCAL:
					TakeScreenshotLocalBP(m_selectedBlueprint.Data.Name, m_selectedButton);
					break;
				case MyBlueprintTypeEnum.CLOUD:
				{
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY, m_selectedBlueprint.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
					string pathFull = Path.Combine(MyFileSystem.UserDataPath, text);
					TakeScreenshotCloud(text, pathFull, m_selectedButton);
					break;
				}
				}
			}
		}

		private void OnButton_Publish(MyGuiControlButton button)
		{
			string localDirectory = GetCurrentLocalDirectory();
			switch (m_content)
			{
			case Content.Blueprint:
				if (m_selectedBlueprint != null)
				{
					string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, localDirectory, m_selectedBlueprint.Data.Name, "bp.sbc");
					if (File.Exists(path))
					{
						m_LoadPrefabData = new LoadPrefabData(null, path, null);
						Action<WorkData> completionCallback = delegate(WorkData workData)
						{
							LoadPrefabData loadPrefabData = workData as LoadPrefabData;
							if (loadPrefabData.Prefab != null)
							{
								MyBlueprintUtils.PublishBlueprint(loadPrefabData.Prefab, m_selectedBlueprint.Data.Name, localDirectory);
							}
						};
						Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefab, completionCallback, m_LoadPrefabData);
					}
				}
				break;
			case Content.Script:
				if (m_selectedBlueprint != null)
				{
					MyBlueprintUtils.PublishScript(button, localDirectory, m_selectedBlueprint, delegate
					{
						m_wasPublished = true;
					});
				}
				break;
			}
		}

		private void OnSendToPlayer()
		{
			if (m_sendToCombo.GetSelectedIndex() != 0)
			{
				if (m_selectedBlueprint == null)
				{
					m_sendToCombo.SelectItemByIndex(0);
					return;
				}
				ulong selectedKey = (ulong)m_sendToCombo.GetSelectedKey();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShareBlueprintRequest, m_publishedItemId.Value, m_selectedBlueprint.Data.Name, selectedKey, MySession.Static.LocalHumanPlayer.DisplayName);
			}
		}

		private void OnSelectItem(MyGuiControlRadioButtonGroup args)
		{
			switch (m_content)
			{
			case Content.Blueprint:
			{
				m_selectedBlueprint = null;
				m_selectedButton = null;
				m_loadedPrefab = null;
				m_LoadPrefabData = null;
				UpdateDetailKeyEnable();
				MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					MyBlueprintItemInfo myBlueprintItemInfo2 = myGuiControlContentButton.UserData as MyBlueprintItemInfo;
					if (myBlueprintItemInfo2 == null)
					{
						break;
					}
					m_selectedButton = myGuiControlContentButton;
					m_selectedBlueprint = myBlueprintItemInfo2;
				}
				UpdatePrefab(m_selectedBlueprint, loadPrefab: false);
				break;
			}
			case Content.Script:
			{
				m_selectedBlueprint = null;
				m_selectedButton = null;
				MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					MyBlueprintItemInfo myBlueprintItemInfo = myGuiControlContentButton.UserData as MyBlueprintItemInfo;
					if (myBlueprintItemInfo == null)
					{
						break;
					}
					m_selectedButton = myGuiControlContentButton;
					m_selectedBlueprint = myBlueprintItemInfo;
					m_publishedItemId = m_selectedBlueprint.PublishedItemId;
				}
				UpdateNameAndDescription();
				UpdateInfo(null, m_selectedBlueprint);
				UpdateDetailKeyEnable();
				break;
			}
			}
		}

		private void OnSearchTextChange(string message)
		{
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void OnReload(MyGuiControlButton button)
		{
			m_selectedButton = null;
			m_selectedBlueprint = null;
			UpdateDetailKeyEnable();
			m_downloadFinished.Clear();
			m_downloadQueued.Clear();
			RefreshAndReloadItemList();
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void ResetBlueprintUI()
		{
			m_selectedBlueprint = null;
			UpdateDetailKeyEnable();
		}

		public void RefreshBlueprintList(bool fromTask = false)
		{
			bool flag = false;
			MyBlueprintItemInfo itemInfo = null;
			if (m_selectedBlueprint != null)
			{
				flag = true;
				itemInfo = m_selectedBlueprint;
			}
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
			switch (m_content)
			{
			case Content.Blueprint:
				GetLocalNames_Blueprints(fromTask);
				break;
			case Content.Script:
				GetLocalNames_Scripts(fromTask);
				break;
			}
			ApplyFiltering();
			m_selectedButton = null;
			m_selectedBlueprint = null;
			TrySelectFirstBlueprint();
			if (flag)
			{
				SelectBlueprint(itemInfo);
			}
			UpdateDetailKeyEnable();
		}

		public void RefreshAndReloadItemList()
		{
			m_BPList.Clear();
			m_BPTypesGroup.Clear();
			switch (m_content)
			{
			case Content.Blueprint:
				GetLocalNames_Blueprints(reload: true);
				break;
			case Content.Script:
				GetLocalNames_Scripts(reload: true);
				break;
			}
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		private void SetGroupSelection(MyBlueprintTypeEnum option)
		{
			SetSelectedBlueprintType(option);
			SetIconForGroupSelection();
			ApplyFiltering();
			TrySelectFirstBlueprint();
		}

		public override bool CloseScreen()
		{
			switch (m_content)
			{
			case Content.Blueprint:
				if (m_blueprintBeingLoaded)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_StillLoading), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_StillLoading), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							m_blueprintBeingLoaded = false;
							Task.valid = false;
							CloseScreen();
						}
					}));
					return false;
				}
				return base.CloseScreen();
			case Content.Script:
				if (m_onCloseAction != null)
				{
					m_onCloseAction();
				}
				return base.CloseScreen();
			default:
				return base.CloseScreen();
			}
		}

		private void OpenSelectedSript()
		{
			if (m_selectedBlueprint.Type == MyBlueprintTypeEnum.STEAM)
			{
				OpenSharedScript(m_selectedBlueprint);
			}
			else if (m_onScriptOpened != null)
			{
				m_onScriptOpened(Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), m_selectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION));
			}
			CloseScreen();
		}

		private void OpenSharedScript(MyBlueprintItemInfo itemInfo)
		{
			m_BPList.Enabled = false;
			Task = Parallel.Start(DownloadScriptFromSteam, OnScriptDownloaded);
		}

		private void DownloadScriptFromSteam()
		{
			if (m_selectedBlueprint != null)
			{
				MyWorkshop.DownloadScriptBlocking(m_selectedBlueprint.Item);
			}
		}

		private void OnScriptDownloaded()
		{
			if (m_onScriptOpened != null && m_selectedBlueprint != null)
			{
				m_onScriptOpened(m_selectedBlueprint.Item.Folder);
			}
			m_BPList.Enabled = true;
		}

		private bool DeleteItem(string path)
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
				return true;
			}
			return false;
		}

		private bool ValidateSelecteditem()
		{
			if (m_selectedBlueprint == null)
			{
				return false;
			}
			if (m_selectedBlueprint.Data.Name == null)
			{
				return false;
			}
			return true;
		}

		internal void OnPrefabLoaded(MyObjectBuilder_Definitions prefab)
		{
			m_blueprintBeingLoaded = false;
			if (prefab != null)
			{
				if (MySandboxGame.Static.SessionCompatHelper != null)
				{
					MySandboxGame.Static.SessionCompatHelper.CheckAndFixPrefab(prefab);
				}
				if (!CheckBlueprintForModsAndModifiedBlocks(prefab))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							CloseScreen();
							if (CopyBlueprintPrefabToClipboard(prefab, m_clipboard) && m_accessType == MyBlueprintAccessType.NORMAL)
							{
								if (MySession.Static.IsCopyPastingEnabled)
								{
									MySandboxGame.Static.Invoke(delegate
									{
										MyClipboardComponent.Static.Paste();
									}, "BlueprintSelectionAutospawn2");
								}
								else
								{
									MyClipboardComponent.ShowCannotPasteWarning();
								}
							}
						}
						_ = 1;
					}));
					return;
				}
				CloseScreen();
				if (CopyBlueprintPrefabToClipboard(prefab, m_clipboard) && m_accessType == MyBlueprintAccessType.NORMAL)
				{
					if (MySession.Static.IsCopyPastingEnabled)
					{
						MySandboxGame.Static.Invoke(delegate
						{
							MyClipboardComponent.Static.Paste();
						}, "BlueprintSelectionAutospawn1");
					}
					else
					{
						MyClipboardComponent.ShowCannotPasteWarning();
					}
				}
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Error), messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint)));
			}
		}

		private static bool CheckBlueprintForModsAndModifiedBlocks(MyObjectBuilder_Definitions prefab)
		{
			MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = prefab.ShipBlueprints;
			if (shipBlueprints != null)
			{
				return MyGridClipboard.CheckPastedBlocks(shipBlueprints[0].CubeGrids);
			}
			return true;
		}

		private bool IsExtracted(MyWorkshopItem subItem)
		{
			switch (m_content)
			{
			case Content.Blueprint:
				return Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.Id.ToString()));
			case Content.Script:
				return true;
			default:
				return false;
			}
		}

		private string GetImagePath(MyBlueprintItemInfo data)
		{
			string result = string.Empty;
			if (data.Type == MyBlueprintTypeEnum.LOCAL)
			{
				switch (m_content)
				{
				case Content.Blueprint:
					result = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
					break;
				case Content.Script:
					result = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), data.Data.Name, MyBlueprintUtils.THUMB_IMAGE_NAME);
					break;
				}
			}
			else if (data.Type == MyBlueprintTypeEnum.CLOUD)
			{
				result = data.Data.CloudImagePath;
			}
			else if (data.Type == MyBlueprintTypeEnum.STEAM)
			{
				if (m_content == Content.Script)
				{
					return result;
				}
				ulong? publishedItemId = data.PublishedItemId;
				if (publishedItemId.HasValue && data.Item != null)
				{
					bool flag = false;
					if (data.Item.Folder != null && MyFileSystem.IsDirectory(data.Item.Folder))
					{
						result = Path.Combine(data.Item.Folder, MyBlueprintUtils.THUMB_IMAGE_NAME);
						flag = true;
					}
					else
					{
						result = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, publishedItemId.ToString(), MyBlueprintUtils.THUMB_IMAGE_NAME);
					}
					bool num = m_downloadQueued.Contains(data.Item.Id);
					bool flag2 = m_downloadFinished.Contains(data.Item.Id);
					MyWorkshopItem worshopData = data.Item;
					if (flag2 && !IsExtracted(worshopData) && !flag)
					{
						m_BPList.Enabled = false;
						ExtractWorkshopItem(worshopData);
						m_BPList.Enabled = true;
					}
					if (!num && !flag2)
					{
						m_BPList.Enabled = false;
						m_downloadQueued.Add(data.Item.Id);
						Task = Parallel.Start(delegate
						{
							DownloadBlueprintFromSteam(worshopData);
						}, delegate
						{
							OnBlueprintDownloadedThumbnail(worshopData);
						});
						result = string.Empty;
					}
				}
			}
			else if (data.Type == MyBlueprintTypeEnum.DEFAULT)
			{
				result = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
			}
			return result;
		}

		private void ExtractWorkshopItem(MyWorkshopItem subItem)
		{
			if (!MyFileSystem.IsDirectory(subItem.Folder))
			{
				try
				{
					string folder = subItem.Folder;
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.Id.ToString());
					if (Directory.Exists(text))
					{
						Directory.Delete(text, recursive: true);
					}
					Directory.CreateDirectory(text);
					MyObjectBuilder_ModInfo myObjectBuilder_ModInfo = new MyObjectBuilder_ModInfo();
					myObjectBuilder_ModInfo.SubtypeName = subItem.Title;
					myObjectBuilder_ModInfo.WorkshopId = subItem.Id;
					myObjectBuilder_ModInfo.SteamIDOwner = subItem.OwnerId;
					string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.Id.ToString(), "info.temp");
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_ModInfo);
					if (!string.IsNullOrEmpty(folder))
					{
						MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(folder);
						if (myZipArchive != null && myZipArchive.FileExists(MyBlueprintUtils.THUMB_IMAGE_NAME))
						{
							Stream stream = myZipArchive.GetFile(MyBlueprintUtils.THUMB_IMAGE_NAME).GetStream();
							if (stream != null)
							{
								using (FileStream destination = File.Create(Path.Combine(text, MyBlueprintUtils.THUMB_IMAGE_NAME)))
								{
									stream.CopyTo(destination);
								}
							}
							stream.Close();
						}
						myZipArchive.Dispose();
					}
					else
					{
						MyLog.Default.Critical(new StringBuilder("Path in Folder directory of blueprint \"" + subItem.Title + "\" " + subItem.Id + " is null, it shouldn't be and who knows what problems it causes. "));
					}
				}
				catch (IOException ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
			MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, subItem.Id)
			{
				BlueprintName = subItem.Title
			};
			MyGuiControlListbox.Item listItem = new MyGuiControlListbox.Item(null, null, null, info);
			if (m_BPList.Controls.FindIndex((MyGuiControlBase item) => (item.UserData as MyBlueprintItemInfo).PublishedItemId == (listItem.UserData as MyBlueprintItemInfo).PublishedItemId && (item.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.STEAM) == -1)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					AddBlueprintButton(info, isLocalMod: false, isWorkshopMod: false, filter: true);
				}, "AddBlueprintButton");
			}
		}

		private void DownloadBlueprintFromSteam(MyWorkshopItem item)
		{
			if (!MyWorkshop.IsUpToDate(item))
			{
				MyWorkshop.DownloadBlueprintBlockingUGC(item, check: false);
				ExtractWorkshopItem(item);
			}
		}

		private void OnBlueprintDownloadedThumbnail(MyWorkshopItem item)
		{
			Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp", item.Id.ToString(), MyBlueprintUtils.THUMB_IMAGE_NAME);
			m_downloadQueued.Remove(item.Id);
			m_downloadFinished.Add(item.Id);
		}

		private void GetBlueprints(string directory, MyBlueprintTypeEnum type)
		{
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			if (!Directory.Exists(directory))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(directory);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array = directories;
			foreach (string text in array)
			{
				list.Add(text + "\\bp.sbc");
				string[] array2 = text.Split(new char[1]
				{
					'\\'
				});
				list2.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				string text2 = list2[j];
				string path = list[j];
				if (File.Exists(path))
				{
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(type)
					{
						TimeCreated = File.GetCreationTimeUtc(path),
						TimeUpdated = File.GetLastWriteTimeUtc(path),
						BlueprintName = text2
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(text2, text2);
					data.Add(myBlueprintItemInfo);
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref data);
		}

		private void GetLocalNames_Blueprints(bool reload = false)
		{
			string directory = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory());
			GetBlueprints(directory, MyBlueprintTypeEnum.LOCAL);
			if (MySandboxGame.Config.EnableSteamCloud)
			{
				GetBlueprintsFromCloud();
			}
			if (Task.IsComplete)
			{
				if (reload)
				{
					GetWorkshopItems();
				}
				else
				{
					GetWorkshopItemsSteam();
				}
			}
			SortBlueprints(m_recievedBlueprints, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref m_recievedBlueprints);
			if (MyFakes.ENABLE_DEFAULT_BLUEPRINTS)
			{
				GetBlueprints(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyBlueprintTypeEnum.DEFAULT);
			}
		}

		private void GetLocalNames_Scripts(bool reload = false)
		{
			string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory());
			if (!Directory.Exists(path))
			{
				MyFileSystem.CreateDirectoryRecursive(path);
			}
			string[] directories = Directory.GetDirectories(path);
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			string[] array = directories;
			foreach (string path2 in array)
			{
				if (MyBlueprintUtils.IsItem_Script(path2))
				{
					string fileName = Path.GetFileName(path2);
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL)
					{
						BlueprintName = fileName
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(fileName);
					data.Add(myBlueprintItemInfo);
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
			AddBlueprintButtons(ref data);
			if (Task.IsComplete && reload)
			{
				GetWorkshopItems();
			}
			else
			{
				AddWorkshopItemsToList();
			}
		}

		private void GetWorkshopItems()
		{
			switch (m_content)
			{
			case Content.Blueprint:
				Task = Parallel.Start(DownloadBlueprints);
				break;
			case Content.Script:
				Task = Parallel.Start(GetScriptsInfo);
				break;
			}
		}

		private void GetScriptsInfo()
		{
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			bool subscribedIngameScriptsBlocking = MyWorkshop.GetSubscribedIngameScriptsBlocking(list);
			if (subscribedIngameScriptsBlocking)
			{
				if (Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP))
				{
					try
					{
						Directory.Delete(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP, recursive: true);
					}
					catch (IOException)
					{
					}
				}
				Directory.CreateDirectory(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP);
			}
			using (SubscribedItemsLock.AcquireExclusiveUsing())
			{
				SetSubscriveItemList(ref list);
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

		private void AddWorkshopItemsToList()
		{
			List<MyBlueprintItemInfo> list = new List<MyBlueprintItemInfo>();
			using (SubscribedItemsLock.AcquireSharedUsing())
			{
				foreach (MyWorkshopItem subscribedItems in GetSubscribedItemsList(Content.Script))
				{
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, subscribedItems.Id)
					{
						BlueprintName = subscribedItems.Title,
						Item = subscribedItems
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(subscribedItems.Title, subscribedItems.Description, subscribedItems.DLCs.ToArray());
					list.Add(myBlueprintItemInfo);
				}
			}
			MySandboxGame.Static.Invoke(delegate
			{
				SortBlueprints(list, MyBlueprintTypeEnum.STEAM);
				AddBlueprintButtons(ref list);
			}, "string");
		}

		private void DownloadBlueprints()
		{
			m_downloadFromSteam = true;
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			bool subscribedBlueprintsBlocking = MyWorkshop.GetSubscribedBlueprintsBlocking(list);
			if (subscribedBlueprintsBlocking)
			{
				Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
				foreach (MyWorkshopItem item in list)
				{
					DownloadBlueprintFromSteam(item);
					m_downloadFinished.Add(item.Id);
				}
			}
			using (SubscribedItemsLock.AcquireExclusiveUsing())
			{
				SetSubscriveItemList(ref list);
			}
			if (subscribedBlueprintsBlocking)
			{
				m_needsExtract = true;
				m_downloadFromSteam = false;
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.CannotFindBlueprintSteam), MySession.GameServiceName), MyTexts.Get(MyCommonTexts.Error)));
			}
		}

		private void GetBlueprintsFromCloud()
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY);
			if (cloudFiles != null)
			{
				List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
				Dictionary<string, MyBlueprintItemInfo> dictionary = new Dictionary<string, MyBlueprintItemInfo>();
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				foreach (MyCloudFileInfo item in cloudFiles)
				{
					string[] array = item.Name.Split(new char[1]
					{
						'/'
					});
					string text = array[array.Length - 2];
					string text2 = array[array.Length - 1];
					if (text2.Equals(MyBlueprintUtils.THUMB_IMAGE_NAME))
					{
						if (dictionary2.ContainsKey(text))
						{
							MyBlueprintItemInfo myBlueprintItemInfo = dictionary2[text] as MyBlueprintItemInfo;
							if (myBlueprintItemInfo != null)
							{
								myBlueprintItemInfo.Data.CloudImagePath = item.LocalPath;
							}
						}
						else
						{
							dictionary2.Add(text, item.LocalPath);
						}
					}
					else if (text2.Equals(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
					{
						MyBlueprintItemInfo value = null;
						if (!dictionary.TryGetValue(text, out value))
						{
							value = new MyBlueprintItemInfo(MyBlueprintTypeEnum.CLOUD)
							{
								TimeCreated = DateTime.FromFileTimeUtc(item.Timestamp),
								TimeUpdated = DateTime.FromFileTimeUtc(item.Timestamp),
								BlueprintName = text,
								CloudInfo = item
							};
							value.SetAdditionalBlueprintInformation(text, item.Name);
							dictionary.Add(text, value);
							data.Add(value);
						}
						if (item.Name.EndsWith("B5"))
						{
							value.CloudPathPB = item.Name;
						}
						else if (item.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
						{
							value.CloudPathXML = item.Name;
						}
						if (dictionary2.ContainsKey(text))
						{
							string text3 = dictionary2[text] as string;
							if (!string.IsNullOrEmpty(text3))
							{
								value.Data.CloudImagePath = text3;
							}
						}
						else
						{
							dictionary2.Add(text, value);
						}
					}
				}
				SortBlueprints(data, MyBlueprintTypeEnum.CLOUD);
				AddBlueprintButtons(ref data);
			}
		}

		private void GetWorkshopItemsSteam()
		{
			List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
			using (SubscribedItemsLock.AcquireSharedUsing())
			{
				List<MyWorkshopItem> subscribedItemsList = GetSubscribedItemsList();
				for (int i = 0; i < subscribedItemsList.Count; i++)
				{
					MyWorkshopItem myWorkshopItem = subscribedItemsList[i];
					MyAnalyticsHelper.ReportActivityStart(null, "show_blueprints", string.Empty, "gui", string.Empty);
					string title = myWorkshopItem.Title;
					MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, myWorkshopItem.Id)
					{
						BlueprintName = title,
						Item = myWorkshopItem
					};
					myBlueprintItemInfo.SetAdditionalBlueprintInformation(title, title, myWorkshopItem.DLCs.ToArray());
					data.Add(myBlueprintItemInfo);
				}
			}
			SortBlueprints(data, MyBlueprintTypeEnum.STEAM);
			AddBlueprintButtons(ref data);
		}

		private bool ValidateModInfo(MyObjectBuilder_ModInfo info)
		{
			if (info == null || info.SubtypeName == null)
			{
				return false;
			}
			return true;
		}

		private static void CheckCurrentLocalDirectory_Blueprint()
		{
			if (!Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(Content.Blueprint))))
			{
				SetCurrentLocalDirectory(Content.Blueprint, string.Empty);
			}
		}

		private bool UpdatePrefab(MyBlueprintItemInfo data, bool loadPrefab)
		{
			bool result = true;
			m_loadedPrefab = null;
			if (data != null)
			{
				switch (data.Type)
				{
				case MyBlueprintTypeEnum.LOCAL:
				{
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(Content.Blueprint), data.Data.Name, "bp.sbc");
					if (File.Exists(text))
					{
						if (loadPrefab)
						{
							m_loadedPrefab = MyBlueprintUtils.LoadPrefab(text);
						}
						try
						{
							using (FileStream sbcStream2 = new FileStream(text, FileMode.Open))
							{
								UpdateNameAndDescription();
								UpdateInfo(sbcStream2, data);
								UpdateDetailKeyEnable();
								return result;
							}
						}
						catch (Exception ex)
						{
							MyLog.Default.WriteLine($"Failed to open {text}.\nException: {ex.Message}");
							return result;
						}
					}
					break;
				}
				case MyBlueprintTypeEnum.STEAM:
					if (data.PublishedItemId.HasValue && data.Item != null)
					{
						result = false;
						MyWorkshopItem workshopData = data.Item;
						Task = Parallel.Start(delegate
						{
							if (!MyWorkshop.IsUpToDate(workshopData))
							{
								DownloadBlueprintFromSteam(workshopData);
							}
							OnBlueprintDownloadedDetails(workshopData, loadPrefab);
						}, delegate
						{
						});
					}
					break;
				case MyBlueprintTypeEnum.CLOUD:
				{
					if (loadPrefab)
					{
						m_loadedPrefab = MyBlueprintUtils.LoadPrefabFromCloud(data);
					}
					byte[] array = MyGameService.LoadFromCloud(data.CloudPathXML);
					if (array != null)
					{
						using (MemoryStream stream = new MemoryStream(array))
						{
							UpdateNameAndDescription();
							UpdateInfo(stream.UnwrapGZip(), data);
							UpdateDetailKeyEnable();
							return result;
						}
					}
					break;
				}
				case MyBlueprintTypeEnum.DEFAULT:
				{
					string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.Data.Name, "bp.sbc");
					if (File.Exists(text))
					{
						if (loadPrefab)
						{
							m_loadedPrefab = MyBlueprintUtils.LoadPrefab(text);
						}
						using (FileStream sbcStream = new FileStream(text, FileMode.Open))
						{
							UpdateNameAndDescription();
							UpdateInfo(sbcStream, data);
							UpdateDetailKeyEnable();
							return result;
						}
					}
					break;
				}
				}
			}
			return result;
		}

		private void OnBlueprintDownloadedDetails(MyWorkshopItem workshopDetails, bool loadPrefab = true)
		{
			if (m_selectedBlueprint == null || m_selectedBlueprint.Item == null || workshopDetails.Id != m_selectedBlueprint.Item.Id)
			{
				return;
			}
			UpdateDetailKeyEnable();
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_selectedBlueprint != null && m_selectedBlueprint.Item != null)
				{
					UpdateNameAndDescription();
				}
			}, "OnBlueprintDownloadedDetails");
			ulong? publishedItemId = m_selectedBlueprint.PublishedItemId;
			string folder = workshopDetails.Folder;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
			if (loadPrefab)
			{
				myObjectBuilder_Definitions = MyBlueprintUtils.LoadWorkshopPrefab(folder, publishedItemId, isOldBlueprintScreen: false);
				if (myObjectBuilder_Definitions == null)
				{
					return;
				}
			}
			if (m_selectedBlueprint == null || m_selectedBlueprint.Item == null || workshopDetails.Id != m_selectedBlueprint.Item.Id)
			{
				return;
			}
			m_publishedItemId = publishedItemId;
			if (loadPrefab)
			{
				m_loadedPrefab = myObjectBuilder_Definitions;
			}
			if (folder != null)
			{
				string path = Path.Combine(folder, "bp.sbc");
				if (MyFileSystem.FileExists(path))
				{
					using (Stream sbcStream = MyFileSystem.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						UpdateInfo(sbcStream, m_selectedBlueprint);
					}
				}
			}
		}

		private void UpdateNameAndDescription()
		{
			if (m_selectedBlueprint.Item == null)
			{
				m_detailName.Text = m_selectedBlueprint.Data.Name;
				StringBuilder text = new StringBuilder(m_selectedBlueprint.Data.Description);
				if (m_selectedBlueprint.Data.DLCs == null || m_selectedBlueprint.Data.DLCs.Length == 0)
				{
					m_multiline.Text = text;
				}
				else
				{
					m_multiline.Text = text;
				}
				return;
			}
			StringBuilder text2 = new StringBuilder(m_selectedBlueprint.Item.Description);
			m_multiline.Text = text2;
			string text3 = m_selectedBlueprint.Item.Title;
			if (text3.Length > 80)
			{
				text3 = text3.Substring(0, 80);
			}
			m_detailName.Text = text3;
		}

		public void OnPathSelected(bool confirmed, string pathNew)
		{
			if (confirmed)
			{
				SetCurrentLocalDirectory(m_content, pathNew);
				RefreshAndReloadItemList();
			}
		}

		public void CreateBlueprintFromClipboard(bool withScreenshot = false, bool replace = false)
		{
			if (m_clipboard.CopiedGridsName == null)
			{
				return;
			}
			string text = MyUtils.StripInvalidChars(m_clipboard.CopiedGridsName);
			string text2 = text;
			string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), text);
			int num = 1;
			while (MyFileSystem.DirectoryExists(path))
			{
				text2 = text + "_" + num;
				path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), text2);
				num++;
			}
			MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
			myObjectBuilder_ShipBlueprintDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(text));
			myObjectBuilder_ShipBlueprintDefinition.CubeGrids = m_clipboard.CopiedGrids.ToArray();
			myObjectBuilder_ShipBlueprintDefinition.DLCs = GetNecessaryDLCs(myObjectBuilder_ShipBlueprintDefinition.CubeGrids);
			myObjectBuilder_ShipBlueprintDefinition.RespawnShip = false;
			myObjectBuilder_ShipBlueprintDefinition.DisplayName = MyGameService.UserName;
			myObjectBuilder_ShipBlueprintDefinition.OwnerSteamId = Sync.MyId;
			myObjectBuilder_ShipBlueprintDefinition.CubeGrids[0].DisplayName = text;
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			myObjectBuilder_Definitions.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
			myObjectBuilder_Definitions.ShipBlueprints[0] = myObjectBuilder_ShipBlueprintDefinition;
			if (MySandboxGame.Config.EnableSteamCloud && withScreenshot)
			{
				SavePrefabToCloudWithScreenshot(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, GetCurrentLocalDirectory(), replace);
				return;
			}
			MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, GetCurrentLocalDirectory(), replace);
			SetGroupSelection(MyBlueprintTypeEnum.MIXED);
			RefreshBlueprintList();
			SelectNewBlueprint(text2, (!MySandboxGame.Config.EnableSteamCloud) ? MyBlueprintTypeEnum.LOCAL : MyBlueprintTypeEnum.CLOUD);
			if (withScreenshot)
			{
				TakeScreenshotLocalBP(text2, m_selectedButton);
			}
		}

		private string[] GetNecessaryDLCs(MyObjectBuilder_CubeGrid[] cubeGrids)
		{
			if (cubeGrids.IsNullOrEmpty())
			{
				return null;
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < cubeGrids.Length; i++)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrids[i].CubeBlocks)
				{
					MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cubeBlock);
					if (cubeBlockDefinition != null && cubeBlockDefinition.DLCs != null && cubeBlockDefinition.DLCs.Length != 0)
					{
						for (int j = 0; j < cubeBlockDefinition.DLCs.Length; j++)
						{
							hashSet.Add(cubeBlockDefinition.DLCs[j]);
						}
					}
				}
			}
			return hashSet.ToArray();
		}

		public void SelectBlueprint(MyBlueprintItemInfo itemInfo)
		{
			int num = -1;
			MyGuiControlContentButton myGuiControlContentButton = null;
			foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
			{
				num++;
				MyBlueprintItemInfo myBlueprintItemInfo = item.UserData as MyBlueprintItemInfo;
				if (myBlueprintItemInfo != null && myBlueprintItemInfo.Equals(itemInfo))
				{
					myGuiControlContentButton = (item as MyGuiControlContentButton);
					break;
				}
			}
			if (myGuiControlContentButton != null)
			{
				SelectButton(myGuiControlContentButton, num);
			}
		}

		public void SelectNewBlueprint(string name, MyBlueprintTypeEnum type)
		{
			int num = -1;
			MyGuiControlContentButton myGuiControlContentButton = null;
			foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
			{
				num++;
				MyBlueprintItemInfo myBlueprintItemInfo = item.UserData as MyBlueprintItemInfo;
				if (myBlueprintItemInfo != null && myBlueprintItemInfo.Type == type && myBlueprintItemInfo.Data.Name.Equals(name))
				{
					myGuiControlContentButton = (item as MyGuiControlContentButton);
					break;
				}
			}
			if (myGuiControlContentButton != null)
			{
				SelectButton(myGuiControlContentButton, num);
			}
		}

		public void SelectButton(MyGuiControlContentButton butt, int idx = -1, bool forceToTop = true, bool alwaysScroll = true)
		{
			if (idx < 0)
			{
				bool flag = false;
				int num = -1;
				foreach (MyGuiControlRadioButton item in m_BPTypesGroup)
				{
					num++;
					if (butt == item)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
				idx = num;
			}
			if (m_selectedButton != butt)
			{
				m_BPTypesGroup.SelectByIndex(idx);
			}
			float min;
			float max;
			ScrollTestResult scrollTestResult = ShouldScroll(butt, idx, out min, out max);
			if (!alwaysScroll && scrollTestResult == ScrollTestResult.Ok)
			{
				return;
			}
			float scrollBarPage;
			if (forceToTop || scrollTestResult == ScrollTestResult.Lower)
			{
				scrollBarPage = min;
			}
			else
			{
				if (scrollTestResult != ScrollTestResult.Higher)
				{
					return;
				}
				scrollBarPage = max - 1f;
			}
			m_BPList.SetScrollBarPage(scrollBarPage);
		}

		private ScrollTestResult ShouldScroll(MyGuiControlContentButton butt, int idx, out float min, out float max)
		{
			float num = (!GetThumbnailVisibility()) ? MAGIC_SPACING_SMALL : MAGIC_SPACING_BIG;
			int num2 = 0;
			for (int i = 0; i < idx; i++)
			{
				if (!m_BPList.Controls[i].Visible)
				{
					num2++;
				}
			}
			float num3 = butt.Size.Y + m_BPList.GetItemMargins().SizeChange.Y - num;
			float y = m_BPList.Size.Y;
			float num4 = (float)(idx - num2) * num3 / y;
			float num5 = ((float)(idx - num2) + 1f) * num3 / y;
			float page = m_BPList.GetScrollBar().GetPage();
			min = num4;
			max = num5;
			if (num4 < page)
			{
				return ScrollTestResult.Lower;
			}
			if (num5 > page + 1f)
			{
				return ScrollTestResult.Higher;
			}
			return ScrollTestResult.Ok;
		}

		public void SavePrefabToCloudWithScreenshot(MyObjectBuilder_Definitions prefab, string name, string currentDirectory, bool replace = false)
		{
			string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY, name);
			string filePath = Path.Combine(path, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
			string text = Path.Combine(path, MyBlueprintUtils.THUMB_IMAGE_NAME);
			if (!m_waitingForScreenshot.IsWaiting())
			{
				string text2 = Path.Combine(MyFileSystem.UserDataPath, text);
				m_waitingForScreenshot.SetData_CreateNewBlueprintCloud(text, text2);
				MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), text2, debug: false, ignoreSprites: true, showNotification: false);
				MyBlueprintUtils.SaveToCloud(prefab, filePath, replace);
				SetGroupSelection(MyBlueprintTypeEnum.MIXED);
				RefreshBlueprintList();
				SelectNewBlueprint(name, MyBlueprintTypeEnum.CLOUD);
			}
		}

		public void CreateScriptFromEditor()
		{
			if (m_getCodeFromEditor != null && Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL))
			{
				int num = 0;
				while (Directory.Exists(Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num)))
				{
					num++;
				}
				string text = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + num;
				string text2 = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, GetCurrentLocalDirectory(), text);
				Directory.CreateDirectory(text2);
				File.Copy(Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME), Path.Combine(text2, MyBlueprintUtils.THUMB_IMAGE_NAME), overwrite: true);
				string contents = m_getCodeFromEditor();
				File.WriteAllText(Path.Combine(text2, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), contents, Encoding.UTF8);
				RefreshAndReloadItemList();
				SelectNewBlueprint(text, MyBlueprintTypeEnum.LOCAL);
			}
		}

		private void ChangeName(string name)
		{
			name = MyUtils.StripInvalidChars(name);
			string path = string.Empty;
			switch (m_content)
			{
			case Content.Blueprint:
				path = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
				break;
			case Content.Script:
				path = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
				break;
			}
			string name2 = m_selectedBlueprint.Data.Name;
			string file = Path.Combine(path, GetCurrentLocalDirectory(), name2);
			string newFile = Path.Combine(path, GetCurrentLocalDirectory(), name);
			if (file == newFile || !Directory.Exists(file))
			{
				return;
			}
			if (Directory.Exists(newFile))
			{
				if (file.ToLower() == newFile.ToLower())
				{
					switch (m_content)
					{
					case Content.Blueprint:
					{
						UpdatePrefab(m_selectedBlueprint, loadPrefab: true);
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
						m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
						string text = Path.Combine(path, "temp");
						if (Directory.Exists(text))
						{
							Directory.Delete(text, recursive: true);
						}
						Directory.Move(file, text);
						Directory.Move(text, newFile);
						MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
						break;
					}
					case Content.Script:
						if (Directory.Exists(file))
						{
							Directory.Move(file, newFile);
						}
						if (Directory.Exists(file))
						{
							Directory.Delete(file, recursive: true);
						}
						break;
					}
					RefreshBlueprintList();
					UpdatePrefab(m_selectedBlueprint, loadPrefab: false);
					using (FileStream sbcStream = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
					{
						UpdateInfo(sbcStream, m_selectedBlueprint);
					}
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Replace), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage1).Append(name).Append((object)MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage2)), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
					{
						if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							switch (m_content)
							{
							case Content.Blueprint:
								DeleteItem(newFile);
								UpdatePrefab(m_selectedBlueprint, loadPrefab: true);
								m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
								m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
								m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
								Directory.Move(file, newFile);
								MyRenderProxy.UnloadTexture(Path.Combine(newFile, "thumb.png"));
								MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
								break;
							case Content.Script:
								Directory.Delete(newFile, recursive: true);
								if (Directory.Exists(file))
								{
									Directory.Move(file, newFile);
								}
								if (Directory.Exists(file))
								{
									Directory.Delete(file, recursive: true);
								}
								break;
							}
							RefreshBlueprintList();
							UpdatePrefab(m_selectedBlueprint, loadPrefab: false);
							using (FileStream sbcStream3 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
							{
								UpdateInfo(sbcStream3, m_selectedBlueprint);
							}
						}
					}));
				}
			}
			else
			{
				try
				{
					switch (m_content)
					{
					case Content.Blueprint:
						UpdatePrefab(m_selectedBlueprint, loadPrefab: true);
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
						m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
						m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
						Directory.Move(file, newFile);
						MyRenderProxy.UnloadTexture(Path.Combine(newFile, "thumb.png"));
						MyBlueprintUtils.SavePrefabToFile(m_loadedPrefab, name, GetCurrentLocalDirectory(), replace: true);
						break;
					case Content.Script:
						if (Directory.Exists(file))
						{
							Directory.Move(file, newFile);
						}
						if (Directory.Exists(file))
						{
							Directory.Delete(file, recursive: true);
						}
						break;
					}
					RefreshBlueprintList();
					UpdatePrefab(m_selectedBlueprint, loadPrefab: false);
					using (FileStream sbcStream2 = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
					{
						UpdateInfo(sbcStream2, m_selectedBlueprint);
					}
				}
				catch (IOException)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Delete), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_DeleteMessage)));
				}
			}
		}

		public static bool CopyBlueprintPrefabToClipboard(MyObjectBuilder_Definitions prefab, MyGridClipboard clipboard, bool setOwner = true)
		{
			if (prefab.ShipBlueprints == null)
			{
				return false;
			}
			MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
			if (cubeGrids == null || cubeGrids.Length == 0)
			{
				return false;
			}
			BoundingSphere boundingSphere = cubeGrids[0].CalculateBoundingSphere();
			MyPositionAndOrientation value = cubeGrids[0].PositionAndOrientation.Value;
			MatrixD m = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
			Matrix matrix = Matrix.Normalize(Matrix.Invert(m));
			BoundingSphere boundingSphere2 = boundingSphere.Transform(m);
			Vector3 dragPointDelta = Vector3.TransformNormal((Vector3)(Vector3D)value.Position - boundingSphere2.Center, matrix);
			float dragVectorLength = boundingSphere.Radius + 10f;
			if ((!MySession.Static.IsUserAdmin(Sync.MyId) || !MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.KeepOriginalOwnershipOnPaste)) && setOwner)
			{
				MyObjectBuilder_CubeGrid[] array = cubeGrids;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (MyObjectBuilder_CubeBlock cubeBlock in array[i].CubeBlocks)
					{
						if (cubeBlock.Owner != 0L)
						{
							cubeBlock.Owner = MySession.Static.LocalPlayerId;
						}
					}
				}
			}
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				for (int j = 0; j < cubeGrids.Length; j++)
				{
					cubeGrids[j] = MyFracturedBlock.ConvertFracturedBlocksToComponents(cubeGrids[j]);
				}
			}
			clipboard.SetGridFromBuilders(cubeGrids, dragPointDelta, dragVectorLength);
			clipboard.ShowModdedBlocksWarning = false;
			return true;
		}

		private void CopyBlueprintAndClose()
		{
			if (CopySelectedItemToClipboard())
			{
				CloseScreen();
			}
		}

		private bool CopySelectedItemToClipboard()
		{
			if (!ValidateSelecteditem())
			{
				return false;
			}
			string text = "";
			MyObjectBuilder_Definitions prefab = null;
			m_clipboard.Deactivate();
			switch (m_selectedBlueprint.Type)
			{
			case MyBlueprintTypeEnum.STEAM:
			{
				ulong? publishedItemId = m_selectedBlueprint.PublishedItemId;
				text = m_selectedBlueprint.Item.Folder;
				if (File.Exists(text) || MyFileSystem.IsDirectory(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this, publishedItemId);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadWorkshopPrefab, null, m_LoadPrefabData);
				}
				break;
			}
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), m_selectedBlueprint.Data.Name, "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefab, null, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.SHARED:
				return false;
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_selectedBlueprint.Data.Name, "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefab, null, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_LoadPrefabData = new LoadPrefabData(prefab, m_selectedBlueprint, this);
				Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefabFromCloud, null, m_LoadPrefabData);
				break;
			}
			return false;
		}

		private void SortBlueprints(List<MyBlueprintItemInfo> list, MyBlueprintTypeEnum type)
		{
			MyItemComparer_Rew myItemComparer_Rew = null;
			switch (type)
			{
			case MyBlueprintTypeEnum.LOCAL:
			case MyBlueprintTypeEnum.CLOUD:
				switch (GetSelectedSort())
				{
				case SortOption.Alphabetical:
					myItemComparer_Rew = new MyItemComparer_Rew((MyBlueprintItemInfo x, MyBlueprintItemInfo y) => x.BlueprintName.CompareTo(y.BlueprintName));
					break;
				case SortOption.UpdateDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime? timeUpdated = x.TimeUpdated;
						DateTime? timeUpdated2 = y.TimeUpdated;
						return (timeUpdated.HasValue && timeUpdated2.HasValue) ? (-1 * DateTime.Compare(timeUpdated.Value, timeUpdated2.Value)) : 0;
					});
					break;
				case SortOption.CreationDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime? timeCreated = x.TimeCreated;
						DateTime? timeCreated2 = y.TimeCreated;
						return (timeCreated.HasValue && timeCreated2.HasValue) ? (-1 * DateTime.Compare(timeCreated.Value, timeCreated2.Value)) : 0;
					});
					break;
				}
				break;
			case MyBlueprintTypeEnum.STEAM:
				switch (GetSelectedSort())
				{
				case SortOption.Alphabetical:
					myItemComparer_Rew = new MyItemComparer_Rew((MyBlueprintItemInfo x, MyBlueprintItemInfo y) => x.BlueprintName.CompareTo(y.BlueprintName));
					break;
				case SortOption.CreationDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime timeCreated3 = x.Item.TimeCreated;
						DateTime timeCreated4 = y.Item.TimeCreated;
						if (timeCreated3 < timeCreated4)
						{
							return 1;
						}
						return (timeCreated3 > timeCreated4) ? (-1) : 0;
					});
					break;
				case SortOption.UpdateDate:
					myItemComparer_Rew = new MyItemComparer_Rew(delegate(MyBlueprintItemInfo x, MyBlueprintItemInfo y)
					{
						DateTime timeUpdated3 = x.Item.TimeUpdated;
						DateTime timeUpdated4 = y.Item.TimeUpdated;
						if (timeUpdated3 < timeUpdated4)
						{
							return 1;
						}
						return (timeUpdated3 > timeUpdated4) ? (-1) : 0;
					});
					break;
				}
				break;
			}
			if (myItemComparer_Rew != null)
			{
				list.Sort(myItemComparer_Rew);
			}
		}

		[Event(null, 3737)]
		[Reliable]
		[Server]
		public static void ShareBlueprintRequest(ulong workshopId, string name, ulong sendToId, string senderName)
		{
			if (Sync.IsServer && sendToId != Sync.MyId)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShareBlueprintRequestClient, workshopId, name, sendToId, senderName, new EndpointId(sendToId));
			}
			else
			{
				ShareBlueprintRequestClient(workshopId, name, sendToId, senderName);
			}
		}

		[Event(null, 3750)]
		[Reliable]
		[Client]
		private static void ShareBlueprintRequestClient(ulong workshopId, string name, ulong sendToId, string senderName)
		{
			MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.SHARED, workshopId)
			{
				BlueprintName = name
			};
			info.SetAdditionalBlueprintInformation(name);
			if (!m_recievedBlueprints.Any((MyBlueprintItemInfo item2) => item2.PublishedItemId == info.PublishedItemId))
			{
				m_recievedBlueprints.Add(info);
				MyHudNotificationDebug notification = new MyHudNotificationDebug(string.Format(MyTexts.Get(MySpaceTexts.SharedBlueprintNotify).ToString(), senderName));
				MyHud.Notifications.Add(notification);
			}
		}

		private void OpenSharedBlueprint(MyBlueprintItemInfo itemInfo)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MySpaceTexts.SharedBlueprint), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), MySession.GameServiceName), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				switch (callbackReturn)
				{
				case MyGuiScreenMessageBox.ResultEnum.YES:
					MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(itemInfo.PublishedItemId.Value));
					m_recievedBlueprints.Remove(m_selectedBlueprint);
					m_selectedBlueprint = null;
					UpdateDetailKeyEnable();
					RefreshBlueprintList();
					break;
				case MyGuiScreenMessageBox.ResultEnum.NO:
					m_recievedBlueprints.Remove(m_selectedBlueprint);
					m_selectedBlueprint = null;
					UpdateDetailKeyEnable();
					RefreshBlueprintList();
					break;
				default:
					_ = 2;
					break;
				}
			}));
		}

		private void ApplyFiltering(MyGuiControlContentButton button)
		{
			if (button == null)
			{
				return;
			}
			bool flag = m_searchBox != null && m_searchBox.SearchText != "";
			string[] array = new string[0];
			if (flag)
			{
				array = m_searchBox.SearchText.Split(new char[1]
				{
					' '
				});
			}
			bool flag2 = true;
			MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
			if (selectedBlueprintType != MyBlueprintTypeEnum.MIXED && selectedBlueprintType != ((MyBlueprintItemInfo)button.UserData).Type)
			{
				flag2 = false;
			}
			if (flag2 && flag)
			{
				string text = button.Title.ToString().ToLower();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text.Contains(text2.ToLower()))
					{
						flag2 = false;
						break;
					}
				}
			}
			if (flag2)
			{
				button.Visible = true;
			}
			else
			{
				button.Visible = false;
			}
		}

		private void ApplyFiltering()
		{
			bool flag = false;
			string[] array = new string[0];
			if (m_searchBox != null)
			{
				flag = (m_searchBox.SearchText != "");
				array = m_searchBox.SearchText.Split(new char[1]
				{
					' '
				});
			}
			foreach (MyGuiControlBase control in m_BPList.Controls)
			{
				MyGuiControlContentButton myGuiControlContentButton = control as MyGuiControlContentButton;
				if (myGuiControlContentButton != null)
				{
					bool flag2 = true;
					MyBlueprintTypeEnum selectedBlueprintType = GetSelectedBlueprintType();
					if (selectedBlueprintType != MyBlueprintTypeEnum.MIXED && selectedBlueprintType != ((MyBlueprintItemInfo)myGuiControlContentButton.UserData).Type && (selectedBlueprintType != 0 || ((MyBlueprintItemInfo)myGuiControlContentButton.UserData).Type != MyBlueprintTypeEnum.SHARED))
					{
						flag2 = false;
					}
					if (flag2 && flag)
					{
						string text = myGuiControlContentButton.Title.ToString().ToLower();
						string[] array2 = array;
						foreach (string text2 in array2)
						{
							if (!text.Contains(text2.ToLower()))
							{
								flag2 = false;
								break;
							}
						}
					}
					if (flag2)
					{
						control.Visible = true;
					}
					else
					{
						control.Visible = false;
					}
				}
			}
			m_BPList.SetScrollBarPage();
		}

		public void TakeScreenshotLocalBP(string name, MyGuiControlContentButton button)
		{
			if (m_waitingForScreenshot.IsWaiting())
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken)));
				return;
			}
			m_waitingForScreenshot.SetData_TakeScreenshotLocal(button);
			string pathToSave = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, GetCurrentLocalDirectory(), name, MyBlueprintUtils.THUMB_IMAGE_NAME);
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), pathToSave, debug: false, ignoreSprites: true, showNotification: false);
		}

		public void TakeScreenshotCloud(string pathRel, string pathFull, MyGuiControlContentButton button)
		{
			if (m_waitingForScreenshot.IsWaiting())
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption), messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken)));
				return;
			}
			m_waitingForScreenshot.SetData_TakeScreenshotCloud(pathRel, pathFull, button);
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), pathFull, debug: false, ignoreSprites: true, showNotification: false);
		}

		public static void ScreenshotTaken(bool success, string filename)
		{
			if (!m_waitingForScreenshot.IsWaiting())
			{
				return;
			}
			switch (m_waitingForScreenshot.Option)
			{
			case WaitForScreenshotOptions.TakeScreenshotLocal:
				if (success)
				{
					MyRenderProxy.UnloadTexture(filename);
					if (m_waitingForScreenshot.UsedButton != null)
					{
						m_waitingForScreenshot.UsedButton.CreatePreview(filename);
					}
				}
				break;
			case WaitForScreenshotOptions.CreateNewBlueprintCloud:
				if (success && File.Exists(m_waitingForScreenshot.PathFull))
				{
					MyBlueprintUtils.SaveToCloudFile(m_waitingForScreenshot.PathFull, m_waitingForScreenshot.PathRel);
				}
				break;
			case WaitForScreenshotOptions.TakeScreenshotCloud:
				if (!success)
				{
					break;
				}
				if (File.Exists(m_waitingForScreenshot.PathFull))
				{
					MyBlueprintUtils.SaveToCloudFile(m_waitingForScreenshot.PathFull, m_waitingForScreenshot.PathRel);
				}
				MyRenderProxy.UnloadTexture(filename);
				if (m_waitingForScreenshot.UsedButton != null)
				{
					if (string.IsNullOrEmpty(m_waitingForScreenshot.UsedButton.PreviewImagePath))
					{
						m_waitingForScreenshot.UsedButton.CreatePreview(filename);
					}
					else
					{
						MyRenderProxy.UnloadTexture(m_waitingForScreenshot.UsedButton.PreviewImagePath);
					}
				}
				break;
			}
			m_waitingForScreenshot.Clear();
		}

		public override bool Update(bool hasFocus)
		{
			if (m_thumbnailImage.Visible)
			{
				UpdateThumbnailPosition();
			}
			if (Task.IsComplete && m_needsExtract)
			{
				GetWorkshopItemsSteam();
				m_needsExtract = false;
				RefreshBlueprintList();
			}
			if (m_wasPublished)
			{
				m_wasPublished = false;
				RefreshBlueprintList(fromTask: true);
			}
			return base.Update(hasFocus);
		}

		private void UpdateThumbnailPosition()
		{
			Vector2 vector = MyGuiManager.MouseCursorPosition + new Vector2(0.02f, 0.055f) + m_thumbnailImage.Size;
			if (vector.X <= 1f && vector.Y <= 1f)
			{
				m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + 0.5f * m_thumbnailImage.Size + new Vector2(-0.48f, -0.445f);
			}
			else
			{
				m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + new Vector2(0.5f * m_thumbnailImage.Size.X - 0.48f, -0.5f * m_thumbnailImage.Size.Y - 0.47f);
			}
		}

		private void OnMouseOverItem(MyGuiControlRadioButton butt, bool isMouseOver)
		{
			if (GetThumbnailVisibility())
			{
				return;
			}
			if (!isMouseOver)
			{
				m_thumbnailImage.SetTexture();
				m_thumbnailImage.Visible = false;
				return;
			}
			MyBlueprintItemInfo myBlueprintItemInfo = butt.UserData as MyBlueprintItemInfo;
			if (myBlueprintItemInfo == null)
			{
				m_thumbnailImage.SetTexture();
				m_thumbnailImage.Visible = false;
				return;
			}
			string imagePath = GetImagePath(myBlueprintItemInfo);
			if (File.Exists(imagePath))
			{
				MyRenderProxy.PreloadTextures(new List<string>
				{
					imagePath
				}, TextureType.GUIWithoutPremultiplyAlpha);
				m_thumbnailImage.SetTexture(imagePath);
				if (m_thumbnailImage.IsAnyTextureValid())
				{
					m_thumbnailImage.Visible = true;
					UpdateThumbnailPosition();
				}
			}
		}

		private void OnFocusedItem(MyGuiControlBase control, bool state)
		{
			if (state)
			{
				MyGuiControlContentButton butt = control as MyGuiControlContentButton;
				SelectButton(butt, -1, forceToTop: false, alwaysScroll: false);
			}
		}

		public void RefreshThumbnail()
		{
			m_thumbnailImage = new MyGuiControlImage();
			m_thumbnailImage.Position = new Vector2(-0.31f, -0.224f);
			m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
			m_thumbnailImage.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
			m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
			m_thumbnailImage.Visible = false;
			m_thumbnailImage.BorderEnabled = true;
			m_thumbnailImage.BorderSize = 1;
			m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
		}
	}
}
