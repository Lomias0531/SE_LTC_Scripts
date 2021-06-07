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
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
	public class MyGuiBlueprintScreen : MyGuiBlueprintScreenBase
	{
		private class LoadPrefabData : WorkData
		{
			private MyObjectBuilder_Definitions m_prefab;

			private string m_path;

			private MyGuiBlueprintScreen m_blueprintScreen;

			private ulong? m_id;

			private MyBlueprintItemInfo m_info;

			public LoadPrefabData(MyObjectBuilder_Definitions prefab, string path, MyGuiBlueprintScreen blueprintScreen, ulong? id = null)
			{
				m_prefab = prefab;
				m_path = path;
				m_blueprintScreen = blueprintScreen;
				m_id = id;
			}

			public LoadPrefabData(MyObjectBuilder_Definitions prefab, MyBlueprintItemInfo info, MyGuiBlueprintScreen blueprintScreen)
			{
				m_prefab = prefab;
				m_blueprintScreen = blueprintScreen;
				m_info = info;
			}

			public void CallLoadPrefab(WorkData workData)
			{
				m_prefab = MyBlueprintUtils.LoadPrefab(m_path);
				CallOnPrefabLoaded();
			}

			public void CallLoadWorkshopPrefab(WorkData workData)
			{
				m_prefab = MyBlueprintUtils.LoadWorkshopPrefab(m_path, m_id, isOldBlueprintScreen: true);
				CallOnPrefabLoaded();
			}

			public void CallLoadPrefabFromCloud(WorkData workData)
			{
				m_prefab = MyBlueprintUtils.LoadPrefabFromCloud(m_info);
				CallOnPrefabLoaded();
			}

			public void CallOnPrefabLoaded()
			{
				if (m_blueprintScreen.State == MyGuiScreenState.OPENED)
				{
					m_blueprintScreen.OnPrefabLoaded(m_prefab);
				}
			}
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

		private readonly string m_thumbImageName = "thumb.png";

		public static Task Task;

		private static bool m_downloadFromSteam;

		private static readonly Vector2 SCREEN_SIZE;

		private static readonly float HIDDEN_PART_RIGHT;

		private static List<MyGuiControlListbox.Item> m_recievedBlueprints;

		private static bool m_needsExtract;

		public static List<MyWorkshopItem> m_subscribedItemsList;

		private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private float m_textScale = 0.8f;

		private MyGuiControlButton m_detailsButton;

		private MyGuiControlButton m_screenshotButton;

		private MyGuiControlButton m_replaceButton;

		private MyGuiControlButton m_deleteButton;

		private MyGuiControlButton m_okButton;

		private MyGuiControlCombobox m_sortCombobox;

		private MyGuiControlTextbox m_searchBox;

		private MyGuiControlButton m_searchClear;

		private static MyBlueprintSortingOptions m_sortBy;

		private static MyGuiControlListbox m_blueprintList;

		private MyGuiDetailScreenBase m_detailScreen;

		private MyGuiControlImage m_thumbnailImage;

		private bool m_activeDetail;

		private MyGuiControlListbox.Item m_selectedItem;

		private MyGuiControlRotatingWheel m_wheel;

		private MyGridClipboard m_clipboard;

		private bool m_allowCopyToClipboard;

		private string m_selectedThumbnailPath;

		private bool m_blueprintBeingLoaded;

		private MyBlueprintAccessType m_accessType;

		private static string m_currentLocalDirectory;

		private static HashSet<ulong> m_downloadQueued;

		private static MyConcurrentHashSet<ulong> m_downloadFinished;

		private static string TEMP_PATH;

		private string[] filenames;

		private static LoadPrefabData m_LoadPrefabData;

		private List<string> m_preloadedTextures = new List<string>();

		private MyGuiControlListbox.Item m_previousItem;

		public static bool FirstTime
		{
			get
			{
				return m_downloadFromSteam;
			}
			set
			{
				m_downloadFromSteam = value;
			}
		}

		static MyGuiBlueprintScreen()
		{
			m_downloadFromSteam = true;
			SCREEN_SIZE = new Vector2(0.4f, 1.2f);
			HIDDEN_PART_RIGHT = 0.04f;
			m_recievedBlueprints = new List<MyGuiControlListbox.Item>();
			m_needsExtract = false;
			m_subscribedItemsList = new List<MyWorkshopItem>();
			m_sortBy = MyBlueprintSortingOptions.SortBy_None;
			m_blueprintList = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.Blueprints);
			m_currentLocalDirectory = string.Empty;
			m_downloadQueued = new HashSet<ulong>();
			m_downloadFinished = new MyConcurrentHashSet<ulong>();
			TEMP_PATH = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp");
		}

		[Event(null, 210)]
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

		[Event(null, 223)]
		[Reliable]
		[Client]
		private static void ShareBlueprintRequestClient(ulong workshopId, string name, ulong sendToId, string senderName)
		{
			MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.SHARED, workshopId);
			StringBuilder text = new StringBuilder(name.ToString());
			object userData = myBlueprintItemInfo;
			MyGuiControlListbox.Item item3 = new MyGuiControlListbox.Item(text, null, MyGuiConstants.TEXTURE_BLUEPRINTS_ARROW.Normal, userData);
			item3.ColorMask = new Vector4(0.7f);
			if (!m_recievedBlueprints.Any((MyGuiControlListbox.Item item2) => (item2.UserData as MyBlueprintItemInfo).PublishedItemId == (item3.UserData as MyBlueprintItemInfo).PublishedItemId))
			{
				m_recievedBlueprints.Add(item3);
				m_blueprintList.Add(item3);
				MyHudNotificationDebug notification = new MyHudNotificationDebug(string.Format(MyTexts.Get(MySpaceTexts.SharedBlueprintNotify).ToString(), senderName));
				MyHud.Notifications.Add(notification);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyBlueprintScreen";
		}

		public MyGuiBlueprintScreen(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType)
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: false)
		{
			m_accessType = accessType;
			m_clipboard = clipboard;
			m_allowCopyToClipboard = allowCopyToClipboard;
			if (!Directory.Exists(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL))
			{
				Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL);
			}
			if (!Directory.Exists(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP))
			{
				Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
			}
			m_blueprintList.Items.Clear();
			CheckCurrentLocalDirectory();
			GetLocalBlueprintNames(m_downloadFromSteam);
			if (m_downloadFromSteam)
			{
				m_downloadFromSteam = false;
			}
			CreateTempDirectory();
			RecreateControls(constructor: true);
			m_blueprintList.ItemsSelected += OnSelectItem;
			m_blueprintList.ItemDoubleClicked += OnItemDoubleClick;
			m_blueprintList.ItemMouseOver += OnMouseOverItem;
			OnEnterCallback = (Action)Delegate.Combine(OnEnterCallback, new Action(Ok));
			m_searchBox.TextChanged += OnSearchTextChange;
			if (MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.HideScreen();
			}
		}

		private void CreateButtons()
		{
			Vector2 vector = new Vector2(-0.091f, 0.194f);
			Vector2 value = new Vector2(0.144f, 0.035f);
			float num = 0.143f;
			float num2 = 0.287f;
			StringBuilder text = MyTexts.Get(MyCommonTexts.Ok);
			Action<MyGuiControlButton> onClick = OnOk;
			float textScale = m_textScale;
			MyStringId? tooltip = (m_allowCopyToClipboard && m_selectedItem != null) ? MyCommonTexts.Blueprints_OkTooltip : MyCommonTexts.Blueprints_OkTooltipDisabled;
			m_okButton = MyBlueprintUtils.CreateButton(this, num, text, onClick, m_allowCopyToClipboard && m_selectedItem != null, tooltip, textScale);
			m_okButton.Position = vector;
			m_okButton.ShowTooltipWhenDisabled = true;
			m_detailsButton = MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.Details), OnDetails, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_detailsButton.Position = vector + new Vector2(1f, 0f) * value;
			m_detailsButton.ShowTooltipWhenDisabled = true;
			m_screenshotButton = MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.TakeScreenshot), OnScreenshot, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
			m_screenshotButton.Position = vector + new Vector2(0f, 1f) * value;
			m_screenshotButton.ShowTooltipWhenDisabled = true;
			m_deleteButton = MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.Delete), OnDelete, enabled: false, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_DeleteTooltipDisabled);
			m_deleteButton.Position = vector + new Vector2(1f, 1f) * value;
			m_deleteButton.ShowTooltipWhenDisabled = true;
			m_replaceButton = MyBlueprintUtils.CreateButton(this, num2, MyTexts.Get(MySpaceTexts.ReplaceWithClipboard), OnReplace, m_clipboard != null && m_clipboard.HasCopiedGrids() && m_selectedItem != null, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_replaceButton.Position = vector + new Vector2(0f, 2f) * value;
			m_replaceButton.PositionX += value.X / 2f;
			m_replaceButton.ShowTooltipWhenDisabled = true;
			vector = new Vector2(-0.091f, 0.343f);
			MyGuiControlButton obj = MyBlueprintUtils.CreateButton(this, num2, MyTexts.Get(MySpaceTexts.CreateFromClipboard), OnCreate, m_clipboard != null && m_clipboard.HasCopiedGrids(), textScale: m_textScale, tooltip: (m_clipboard != null && m_clipboard.HasCopiedGrids()) ? MyCommonTexts.Blueprints_CreateTooltip : MyCommonTexts.Blueprints_CreateTooltipDisabled);
			obj.ShowTooltipWhenDisabled = true;
			obj.Position = vector + new Vector2(0f, 0f) * value;
			obj.PositionX += value.X / 2f;
			MyGuiControlButton obj2 = MyBlueprintUtils.CreateButton(this, num2, MyTexts.Get(MySpaceTexts.RefreshBlueprints), OnReload, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Blueprints_RefreshTooltip);
			obj2.Position = vector + new Vector2(0f, 1f) * value;
			obj2.PositionX += value.X / 2f;
			MyGuiControlButton myGuiControlButton = MyBlueprintUtils.CreateButton(this, num2, MyTexts.Get(MyCommonTexts.Close), OnCancel, enabled: true, MySpaceTexts.ToolTipNewsletter_Close, m_textScale);
			myGuiControlButton.Position = vector + new Vector2(0f, 2f) * value;
			myGuiControlButton.PositionX += value.X / 2f;
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

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyAnalyticsHelper.ReportActivityStart(null, "show_blueprints", string.Empty, "gui", string.Empty);
			Vector2 value = new Vector2(0.02f, SCREEN_SIZE.Y - 1.076f);
			float num = (SCREEN_SIZE.Y - 1f) / 2f;
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MyTexts.Get(MyCommonTexts.Search).ToString() + ":", value + new Vector2(-0.175f, -0.015f), m_textScale);
			myGuiControlLabel.Position = new Vector2(-0.164f, -0.406f);
			m_searchBox = new MyGuiControlTextbox();
			m_searchBox.Position = new Vector2(0.123f, -0.401f);
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_searchBox.Size = new Vector2(0.279f - myGuiControlLabel.Size.X, 0.2f);
			m_searchBox.SetToolTip(MyCommonTexts.Blueprints_SearchTooltip);
			m_searchClear = new MyGuiControlButton
			{
				Position = value + new Vector2(0.076f, -0.521f),
				Size = new Vector2(0.045f, 17f / 300f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.Close,
				ActivateOnMouseRelease = true
			};
			m_searchClear.ButtonClicked += OnSearchClear;
			m_sortCombobox = new MyGuiControlCombobox();
			foreach (object value2 in Enum.GetValues(typeof(MyBlueprintSortingOptions)))
			{
				m_sortCombobox.AddItem((int)value2, new StringBuilder(MyTexts.TrySubstitute(value2.ToString())));
			}
			m_sortCombobox.SelectItemByIndex((int)m_sortBy);
			m_sortCombobox.ItemSelected += delegate
			{
				SortOptionChanged((MyBlueprintSortingOptions)m_sortCombobox.GetSelectedKey());
			};
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.Blueprint_Sort_Label));
			myGuiControlLabel2.Position = new Vector2(-0.164f, -0.348f);
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_sortCombobox.Position = new Vector2(0.123f, -0.348f);
			m_sortCombobox.Size = new Vector2(0.28f - myGuiControlLabel2.Size.X, 0.04f);
			m_sortCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_sortCombobox.SetToolTip(MyCommonTexts.Blueprints_SortByTooltip);
			AddCaption(MyTexts.Get(MySpaceTexts.BlueprintsScreen).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.17f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.318f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(-0.155f, -0.307f),
				Name = "ControlLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.Blueprints_ListOfBlueprints)
			};
			MyGuiControlPanel control2 = new MyGuiControlPanel(new Vector2(-0.1635f, -0.312f), new Vector2(0.2865f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_blueprintList.Position = new Vector2(-0.02f, -0.066f);
			m_blueprintList.VisibleRowsCount = 12;
			m_blueprintList.MultiSelect = false;
			Controls.Add(control2);
			Controls.Add(control);
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_searchBox);
			Controls.Add(m_searchClear);
			Controls.Add(m_blueprintList);
			Controls.Add(m_sortCombobox);
			Controls.Add(myGuiControlLabel2);
			RefreshThumbnail();
			Controls.Add(m_thumbnailImage);
			CreateButtons();
			string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds";
			m_wheel = new MyGuiControlRotatingWheel(new Vector2(-0.02f, -0.12f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.28f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, texture, manualRotationUpdate: true, MyPerGameSettings.GUI.MultipleSpinningWheels);
			Controls.Add(m_wheel);
			m_wheel.Visible = false;
		}

		public void SortOptionChanged(MyBlueprintSortingOptions option)
		{
			m_sortBy = option;
			OnReload(null);
		}

		private void GetLocalBlueprintNames(bool reload = false)
		{
			string directory = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory);
			GetBlueprints(directory, MyBlueprintTypeEnum.LOCAL);
			if (MySandboxGame.Config.EnableSteamCloud)
			{
				GetBlueprintsFromCloud();
			}
			if (Task.IsComplete)
			{
				if (reload)
				{
					GetWorkshopBlueprints();
				}
				else
				{
					GetWorkshopItemsSteam();
				}
			}
			foreach (MyGuiControlListbox.Item recievedBlueprint in m_recievedBlueprints)
			{
				m_blueprintList.Add(recievedBlueprint);
			}
			if (MyFakes.ENABLE_DEFAULT_BLUEPRINTS)
			{
				GetBlueprints(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyBlueprintTypeEnum.DEFAULT);
			}
		}

		private void SortBlueprints(List<MyGuiControlListbox.Item> list, MyBlueprintTypeEnum type)
		{
			MyItemComparer myItemComparer = null;
			switch (type)
			{
			case MyBlueprintTypeEnum.LOCAL:
			case MyBlueprintTypeEnum.CLOUD:
				switch (m_sortBy)
				{
				case MyBlueprintSortingOptions.SortBy_Name:
					myItemComparer = new MyItemComparer((MyGuiControlListbox.Item x, MyGuiControlListbox.Item y) => ((MyBlueprintItemInfo)x.UserData).BlueprintName.CompareTo(((MyBlueprintItemInfo)y.UserData).BlueprintName));
					break;
				case MyBlueprintSortingOptions.SortBy_UpdateDate:
					myItemComparer = new MyItemComparer(delegate(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y)
					{
						DateTime? timeUpdated = ((MyBlueprintItemInfo)x.UserData).TimeUpdated;
						DateTime? timeUpdated2 = ((MyBlueprintItemInfo)y.UserData).TimeUpdated;
						return (timeUpdated.HasValue && timeUpdated2.HasValue) ? (-1 * DateTime.Compare(timeUpdated.Value, timeUpdated2.Value)) : 0;
					});
					break;
				case MyBlueprintSortingOptions.SortBy_CreationDate:
					myItemComparer = new MyItemComparer(delegate(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y)
					{
						DateTime? timeCreated = ((MyBlueprintItemInfo)x.UserData).TimeCreated;
						DateTime? timeCreated2 = ((MyBlueprintItemInfo)y.UserData).TimeCreated;
						return (timeCreated.HasValue && timeCreated2.HasValue) ? (-1 * DateTime.Compare(timeCreated.Value, timeCreated2.Value)) : 0;
					});
					break;
				}
				break;
			case MyBlueprintTypeEnum.STEAM:
				switch (m_sortBy)
				{
				case MyBlueprintSortingOptions.SortBy_Name:
					myItemComparer = new MyItemComparer((MyGuiControlListbox.Item x, MyGuiControlListbox.Item y) => ((MyBlueprintItemInfo)x.UserData).BlueprintName.CompareTo(((MyBlueprintItemInfo)y.UserData).BlueprintName));
					break;
				case MyBlueprintSortingOptions.SortBy_CreationDate:
					myItemComparer = new MyItemComparer(delegate(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y)
					{
						DateTime timeCreated3 = ((MyBlueprintItemInfo)x.UserData).Item.TimeCreated;
						DateTime timeCreated4 = ((MyBlueprintItemInfo)y.UserData).Item.TimeCreated;
						if (timeCreated3 < timeCreated4)
						{
							return 1;
						}
						return (timeCreated3 > timeCreated4) ? (-1) : 0;
					});
					break;
				case MyBlueprintSortingOptions.SortBy_UpdateDate:
					myItemComparer = new MyItemComparer(delegate(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y)
					{
						DateTime timeUpdated3 = ((MyBlueprintItemInfo)x.UserData).Item.TimeUpdated;
						DateTime timeUpdated4 = ((MyBlueprintItemInfo)y.UserData).Item.TimeUpdated;
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
			if (myItemComparer != null)
			{
				list.Sort(myItemComparer);
			}
		}

		private void GetBlueprintsFromCloud()
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY);
			if (cloudFiles != null)
			{
				List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
				Dictionary<string, MyBlueprintItemInfo> dictionary = new Dictionary<string, MyBlueprintItemInfo>();
				foreach (MyCloudFileInfo item2 in cloudFiles)
				{
					string[] array = item2.Name.Split(new char[1]
					{
						'/'
					});
					string text = array[array.Length - 2];
					MyBlueprintItemInfo value = null;
					if (!dictionary.TryGetValue(text, out value))
					{
						value = new MyBlueprintItemInfo(MyBlueprintTypeEnum.CLOUD)
						{
							TimeCreated = DateTime.FromFileTimeUtc(item2.Timestamp),
							TimeUpdated = DateTime.FromFileTimeUtc(item2.Timestamp),
							BlueprintName = text,
							CloudInfo = item2
						};
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(text), item2.Name, userData: value, icon: MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_CLOUD.Normal);
						dictionary.Add(text, value);
						list.Add(item);
					}
					if (item2.Name.EndsWith("B5"))
					{
						value.CloudPathPB = item2.Name;
					}
					else if (item2.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
					{
						value.CloudPathXML = item2.Name;
					}
				}
				SortBlueprints(list, MyBlueprintTypeEnum.CLOUD);
				foreach (MyGuiControlListbox.Item item3 in list)
				{
					m_blueprintList.Add(item3);
				}
			}
		}

		private void GetBlueprints(string directory, MyBlueprintTypeEnum type)
		{
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			if (!Directory.Exists(directory))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(directory);
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			string[] array = directories;
			foreach (string text in array)
			{
				list3.Add(text + "\\bp.sbc");
				string[] array2 = text.Split(new char[1]
				{
					'\\'
				});
				list4.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list4.Count; j++)
			{
				string text2 = list4[j];
				string path = list3[j];
				MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(type)
				{
					TimeCreated = File.GetCreationTimeUtc(path),
					TimeUpdated = File.GetLastWriteTimeUtc(path),
					BlueprintName = text2
				};
				string empty = string.Empty;
				if (!File.Exists(path))
				{
					myBlueprintItemInfo.IsDirectory = true;
					empty = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal;
				}
				else
				{
					empty = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal;
				}
				StringBuilder text3 = new StringBuilder(text2);
				object userData = myBlueprintItemInfo;
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text3, text2, empty, userData);
				if (myBlueprintItemInfo.IsDirectory)
				{
					list2.Add(item);
				}
				else
				{
					list.Add(item);
				}
			}
			if (!string.IsNullOrEmpty(m_currentLocalDirectory))
			{
				MyBlueprintItemInfo myBlueprintItemInfo2 = new MyBlueprintItemInfo(type)
				{
					TimeCreated = DateTime.Now,
					TimeUpdated = DateTime.Now,
					BlueprintName = string.Empty,
					IsDirectory = true
				};
				StringBuilder text4 = new StringBuilder("[..]");
				string currentLocalDirectory = m_currentLocalDirectory;
				object userData = myBlueprintItemInfo2;
				MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(text4, currentLocalDirectory, MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal, userData);
				m_blueprintList.Add(item2);
			}
			SortBlueprints(list2, MyBlueprintTypeEnum.LOCAL);
			foreach (MyGuiControlListbox.Item item3 in list2)
			{
				m_blueprintList.Add(item3);
			}
			SortBlueprints(list, MyBlueprintTypeEnum.LOCAL);
			foreach (MyGuiControlListbox.Item item4 in list)
			{
				m_blueprintList.Add(item4);
			}
		}

		private bool ValidateModInfo(MyObjectBuilder_ModInfo info)
		{
			if (info == null || info.SubtypeName == null)
			{
				return false;
			}
			return true;
		}

		private void GetWorkshopItemsSteam()
		{
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			for (int i = 0; i < m_subscribedItemsList.Count; i++)
			{
				MyWorkshopItem myWorkshopItem = m_subscribedItemsList[i];
				MyAnalyticsHelper.ReportActivityStart(null, "show_blueprints", string.Empty, "gui", string.Empty);
				string title = myWorkshopItem.Title;
				MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, myWorkshopItem.Id)
				{
					Item = myWorkshopItem,
					BlueprintName = title
				};
				StringBuilder text = new StringBuilder(title);
				object userData = myBlueprintItemInfo;
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, title, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Normal, userData);
				list.Add(item);
			}
			SortBlueprints(list, MyBlueprintTypeEnum.STEAM);
			foreach (MyGuiControlListbox.Item item2 in list)
			{
				m_blueprintList.Add(item2);
			}
		}

		private void GetWorkshopItemsLocal()
		{
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp");
			if (!Directory.Exists(text))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(text);
			List<string> list = new List<string>();
			new List<string>();
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1]
				{
					'\\'
				});
				list.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list.Count; j++)
			{
				string path = Path.Combine(text, list[j], "info.temp");
				MyObjectBuilder_ModInfo objectBuilder = null;
				if (File.Exists(path))
				{
					MyAnalyticsHelper.ReportActivityStart(null, "show_blueprints", string.Empty, "gui", string.Empty);
					bool flag = MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder);
					if (ValidateModInfo(objectBuilder) && flag)
					{
						string subtypeName = objectBuilder.SubtypeName;
						MyBlueprintItemInfo myBlueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, objectBuilder.WorkshopId);
						StringBuilder text2 = new StringBuilder(subtypeName);
						object userData = myBlueprintItemInfo;
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text2, subtypeName, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Normal, userData);
						m_blueprintList.Add(item);
					}
				}
			}
		}

		private bool IsExtracted(MyWorkshopItem subItem)
		{
			return Directory.Exists(Path.Combine(TEMP_PATH, subItem.Id.ToString()));
		}

		private void ExtractWorkshopItem(MyWorkshopItem subItem)
		{
			if (MyFileSystem.IsDirectory(subItem.Folder))
			{
				new MyObjectBuilder_ModInfo
				{
					SubtypeName = subItem.Title,
					WorkshopId = subItem.Id,
					SteamIDOwner = subItem.OwnerId
				};
			}
			else
			{
				try
				{
					string folder = subItem.Folder;
					string text = Path.Combine(TEMP_PATH, subItem.Id.ToString());
					if (Directory.Exists(text))
					{
						Directory.Delete(text, recursive: true);
					}
					Directory.CreateDirectory(text);
					MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(folder);
					MyObjectBuilder_ModInfo myObjectBuilder_ModInfo = new MyObjectBuilder_ModInfo();
					myObjectBuilder_ModInfo.SubtypeName = subItem.Title;
					myObjectBuilder_ModInfo.WorkshopId = subItem.Id;
					myObjectBuilder_ModInfo.SteamIDOwner = subItem.OwnerId;
					string path = Path.Combine(TEMP_PATH, subItem.Id.ToString(), "info.temp");
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					MyObjectBuilderSerializer.SerializeXML(path, compress: false, myObjectBuilder_ModInfo);
					if (myZipArchive.FileExists(m_thumbImageName))
					{
						Stream stream = myZipArchive.GetFile(m_thumbImageName).GetStream();
						if (stream != null)
						{
							using (FileStream destination = File.Create(Path.Combine(text, m_thumbImageName)))
							{
								stream.CopyTo(destination);
							}
						}
						stream.Close();
					}
					myZipArchive.Dispose();
				}
				catch (IOException ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
			MyBlueprintItemInfo userData = new MyBlueprintItemInfo(MyBlueprintTypeEnum.STEAM, subItem.Id);
			MyGuiControlListbox.Item listItem = new MyGuiControlListbox.Item(new StringBuilder(subItem.Title), subItem.Title, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP.Normal, userData);
			if (m_blueprintList.Items.FindIndex((MyGuiControlListbox.Item item) => (item.UserData as MyBlueprintItemInfo).PublishedItemId == (listItem.UserData as MyBlueprintItemInfo).PublishedItemId && (item.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.STEAM) == -1)
			{
				m_blueprintList.Add(listItem);
			}
		}

		private DirectoryInfo CreateTempDirectory()
		{
			return Directory.CreateDirectory(TEMP_PATH);
		}

		private void DownloadBlueprints()
		{
			m_downloadFromSteam = true;
			m_subscribedItemsList.Clear();
			bool subscribedBlueprintsBlocking = MyWorkshop.GetSubscribedBlueprintsBlocking(m_subscribedItemsList);
			if (subscribedBlueprintsBlocking)
			{
				Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
				foreach (MyWorkshopItem subscribedItems in m_subscribedItemsList)
				{
					if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, subscribedItems.Id + MyBlueprintUtils.BLUEPRINT_WORKSHOP_EXTENSION)))
					{
						m_downloadFinished.Add(subscribedItems.Id);
					}
					else
					{
						DownloadBlueprintFromSteam(subscribedItems);
						m_downloadFinished.Add(subscribedItems.Id);
					}
				}
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

		private void GetWorkshopBlueprints()
		{
			Task = Parallel.Start(DownloadBlueprints);
		}

		public override void RefreshBlueprintList(bool fromTask = false)
		{
			m_blueprintList.StoreSituation();
			m_blueprintList.Items.Clear();
			GetLocalBlueprintNames(fromTask);
			m_selectedItem = null;
			m_screenshotButton.Enabled = false;
			m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_detailsButton.Enabled = false;
			m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_replaceButton.Enabled = false;
			m_replaceButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_deleteButton.Enabled = false;
			m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_okButton.Enabled = false;
			m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_blueprintList.RestoreSituation(compareUserData: false, compareText: true);
			OnSearchTextChange(m_searchBox);
		}

		private void ReloadTextures()
		{
			string bLUEPRINT_FOLDER_LOCAL = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
			string[] directories = Directory.GetDirectories(bLUEPRINT_FOLDER_LOCAL);
			string text = "";
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				text = Path.Combine(array[i], m_thumbImageName);
				if (File.Exists(text))
				{
					MyRenderProxy.UnloadTexture(text);
				}
			}
			bLUEPRINT_FOLDER_LOCAL = MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY;
			if (Directory.Exists(bLUEPRINT_FOLDER_LOCAL))
			{
				array = Directory.GetDirectories(bLUEPRINT_FOLDER_LOCAL);
				for (int i = 0; i < array.Length; i++)
				{
					text = Path.Combine(array[i], m_thumbImageName);
					if (File.Exists(text))
					{
						MyRenderProxy.UnloadTexture(text);
					}
				}
			}
			bLUEPRINT_FOLDER_LOCAL = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp");
			if (!Directory.Exists(bLUEPRINT_FOLDER_LOCAL))
			{
				return;
			}
			array = Directory.GetDirectories(bLUEPRINT_FOLDER_LOCAL);
			for (int i = 0; i < array.Length; i++)
			{
				text = Path.Combine(array[i], m_thumbImageName);
				if (File.Exists(text))
				{
					MyRenderProxy.UnloadTexture(text);
				}
			}
		}

		public void RefreshAndReloadBlueprintList()
		{
			m_blueprintList.StoreSituation();
			m_blueprintList.Items.Clear();
			GetLocalBlueprintNames(reload: true);
			ReloadTextures();
			m_blueprintList.RestoreSituation(compareUserData: false, compareText: true);
			OnSearchTextChange(m_searchBox);
		}

		private void OnSearchClear(MyGuiControlButton button)
		{
			m_searchBox.Text = "";
		}

		private void OnMouseOverItem(MyGuiControlListbox listBox)
		{
			MyGuiControlListbox.Item mouseOverItem = listBox.MouseOverItem;
			if (m_previousItem == mouseOverItem)
			{
				return;
			}
			m_previousItem = mouseOverItem;
			if (mouseOverItem == null)
			{
				m_thumbnailImage.Visible = false;
				return;
			}
			string text = string.Empty;
			MyBlueprintItemInfo myBlueprintItemInfo = mouseOverItem.UserData as MyBlueprintItemInfo;
			if (myBlueprintItemInfo.Type == MyBlueprintTypeEnum.LOCAL)
			{
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, mouseOverItem.Text.ToString(), m_thumbImageName);
			}
			else if (myBlueprintItemInfo.Type == MyBlueprintTypeEnum.STEAM)
			{
				ulong? publishedItemId = myBlueprintItemInfo.PublishedItemId;
				if (publishedItemId.HasValue && myBlueprintItemInfo.Item != null)
				{
					bool flag = false;
					if (MyFileSystem.IsDirectory(myBlueprintItemInfo.Item.Folder))
					{
						text = Path.Combine(myBlueprintItemInfo.Item.Folder, m_thumbImageName);
						flag = true;
					}
					else
					{
						text = Path.Combine(TEMP_PATH, publishedItemId.ToString(), m_thumbImageName);
					}
					bool num = m_downloadQueued.Contains(myBlueprintItemInfo.Item.Id);
					bool flag2 = m_downloadFinished.Contains(myBlueprintItemInfo.Item.Id);
					MyWorkshopItem worshopData = myBlueprintItemInfo.Item;
					if (flag2 && !IsExtracted(worshopData) && !flag)
					{
						m_blueprintList.Enabled = false;
						m_okButton.Enabled = false;
						m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
						ExtractWorkshopItem(worshopData);
						m_blueprintList.Enabled = true;
						m_okButton.Enabled = true;
						m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
					}
					if (!num && !flag2)
					{
						m_blueprintList.Enabled = false;
						m_okButton.Enabled = false;
						m_downloadQueued.Add(myBlueprintItemInfo.Item.Id);
						Task = Parallel.Start(delegate
						{
							DownloadBlueprintFromSteam(worshopData);
						}, delegate
						{
							OnBlueprintDownloadedThumbnail(worshopData);
						});
						text = string.Empty;
					}
				}
			}
			else if (myBlueprintItemInfo.Type == MyBlueprintTypeEnum.DEFAULT)
			{
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, mouseOverItem.Text.ToString(), m_thumbImageName);
			}
			if (File.Exists(text))
			{
				m_preloadedTextures.Clear();
				m_preloadedTextures.Add(text);
				MyRenderProxy.PreloadTextures(m_preloadedTextures, TextureType.GUIWithoutPremultiplyAlpha);
				m_thumbnailImage.SetTexture(text);
				if (!m_activeDetail && m_thumbnailImage.IsAnyTextureValid())
				{
					m_thumbnailImage.Visible = true;
				}
			}
			else
			{
				m_thumbnailImage.Visible = false;
				m_thumbnailImage.SetTexture();
			}
		}

		private void OnSelectItem(MyGuiControlListbox list)
		{
			if (list.SelectedItems.Count == 0)
			{
				return;
			}
			m_selectedItem = list.SelectedItems[0];
			m_okButton.Enabled = true;
			m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
			m_detailsButton.Enabled = true;
			m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_DetailsTooltip);
			m_screenshotButton.Enabled = true;
			m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltip);
			m_replaceButton.Enabled = m_clipboard.HasCopiedGrids();
			m_replaceButton.SetToolTip(m_replaceButton.Enabled ? MyCommonTexts.Blueprints_ReplaceBlueprintTooltip : MyCommonTexts.Blueprints_CreateTooltipDisabled);
			MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
			MyBlueprintTypeEnum type = myBlueprintItemInfo.Type;
			ulong? publishedItemId = myBlueprintItemInfo.PublishedItemId;
			string text = "";
			switch (type)
			{
			case MyBlueprintTypeEnum.STEAM:
				if (myBlueprintItemInfo.Item == null)
				{
					return;
				}
				text = ((!MyFileSystem.IsDirectory(myBlueprintItemInfo.Item.Folder)) ? Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp", publishedItemId.ToString(), m_thumbImageName) : Path.Combine(myBlueprintItemInfo.Item.Folder, m_thumbImageName));
				m_screenshotButton.Enabled = false;
				m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
				m_replaceButton.Enabled = false;
				m_deleteButton.Enabled = false;
				m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
				break;
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, m_selectedItem.Text.ToString(), m_thumbImageName);
				m_deleteButton.Enabled = true;
				m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltip);
				if (myBlueprintItemInfo.IsDirectory)
				{
					m_detailsButton.Enabled = false;
					m_screenshotButton.Enabled = false;
					m_replaceButton.Enabled = false;
				}
				break;
			case MyBlueprintTypeEnum.SHARED:
				m_replaceButton.Enabled = false;
				m_screenshotButton.Enabled = false;
				m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
				m_detailsButton.Enabled = false;
				m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
				m_deleteButton.Enabled = false;
				m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
				break;
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_selectedItem.Text.ToString(), m_thumbImageName);
				m_replaceButton.Enabled = false;
				m_screenshotButton.Enabled = false;
				m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
				m_deleteButton.Enabled = false;
				m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_deleteButton.Enabled = true;
				m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltip);
				m_detailsButton.Enabled = true;
				m_screenshotButton.Enabled = false;
				break;
			}
			if (File.Exists(text))
			{
				m_selectedThumbnailPath = text;
			}
			else
			{
				m_selectedThumbnailPath = null;
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

		private bool CopySelectedItemToClipboard()
		{
			if (!ValidateSelecteditem())
			{
				return false;
			}
			string text = "";
			MyObjectBuilder_Definitions prefab = null;
			MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
			switch (myBlueprintItemInfo.Type)
			{
			case MyBlueprintTypeEnum.STEAM:
			{
				ulong? publishedItemId = myBlueprintItemInfo.PublishedItemId;
				text = myBlueprintItemInfo.Item.Folder;
				if (File.Exists(text) || MyFileSystem.IsDirectory(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this, publishedItemId);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadWorkshopPrefab, null, m_LoadPrefabData);
				}
				break;
			}
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, m_selectedItem.Text.ToString(), "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefab, null, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.SHARED:
				return false;
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_selectedItem.Text.ToString(), "bp.sbc");
				if (File.Exists(text))
				{
					m_LoadPrefabData = new LoadPrefabData(prefab, text, this);
					Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefab, null, m_LoadPrefabData);
				}
				break;
			case MyBlueprintTypeEnum.CLOUD:
				m_LoadPrefabData = new LoadPrefabData(prefab, myBlueprintItemInfo, this);
				Task = Parallel.Start(m_LoadPrefabData.CallLoadPrefabFromCloud, null, m_LoadPrefabData);
				break;
			}
			return false;
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
				if (!CheckBlueprintForMods(prefab))
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
						if (result == MyGuiScreenMessageBox.ResultEnum.NO)
						{
							m_selectedItem = m_blueprintList.SelectedItems[0];
							m_okButton.Enabled = true;
							m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
						}
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

		private static bool CheckBlueprintForMods(MyObjectBuilder_Definitions prefab)
		{
			if (prefab.ShipBlueprints == null)
			{
				return true;
			}
			MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
			if (cubeGrids == null || cubeGrids.Length == 0)
			{
				return true;
			}
			MyObjectBuilder_CubeGrid[] array = cubeGrids;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (MyObjectBuilder_CubeBlock cubeBlock in array[i].CubeBlocks)
				{
					MyDefinitionId id = cubeBlock.GetId();
					MyCubeBlockDefinition blockDefinition = null;
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition))
					{
						return false;
					}
				}
			}
			return true;
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
			if (setOwner)
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

		private void OnSearchTextChange(MyGuiControlTextbox box)
		{
			if (box.Text != "")
			{
				string[] array = box.Text.Split(new char[1]
				{
					' '
				});
				foreach (MyGuiControlListbox.Item item in m_blueprintList.Items)
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
				foreach (MyGuiControlListbox.Item item2 in m_blueprintList.Items)
				{
					item2.Visible = true;
				}
			}
			m_blueprintList.ScrollToolbarToTop();
		}

		private void OpenSharedBlueprint(MyBlueprintItemInfo itemInfo)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MySpaceTexts.SharedBlueprint), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), MySession.GameServiceName), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				switch (callbackReturn)
				{
				case MyGuiScreenMessageBox.ResultEnum.YES:
					MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(itemInfo.PublishedItemId.Value));
					m_recievedBlueprints.Remove(m_selectedItem);
					m_selectedItem = null;
					RefreshBlueprintList();
					break;
				case MyGuiScreenMessageBox.ResultEnum.NO:
					m_recievedBlueprints.Remove(m_selectedItem);
					m_selectedItem = null;
					RefreshBlueprintList();
					break;
				case MyGuiScreenMessageBox.ResultEnum.CANCEL:
					m_okButton.Enabled = true;
					m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
					break;
				}
			}));
		}

		private void OnItemDoubleClick(MyGuiControlListbox list)
		{
			m_selectedItem = list.SelectedItems[0];
			Ok();
		}

		private void CopyBlueprintAndClose()
		{
			if (CopySelectedItemToClipboard())
			{
				CloseScreen();
			}
		}

		private void Ok()
		{
			if (m_selectedItem == null)
			{
				CloseScreen();
				return;
			}
			MyBlueprintItemInfo itemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
			if (itemInfo.IsDirectory)
			{
				if (string.IsNullOrEmpty(itemInfo.BlueprintName))
				{
					string[] array = m_currentLocalDirectory.Split(new char[1]
					{
						Path.DirectorySeparatorChar
					});
					if (array.Length > 1)
					{
						array[array.Length - 1] = string.Empty;
						m_currentLocalDirectory = Path.Combine(array);
					}
					else
					{
						m_currentLocalDirectory = string.Empty;
					}
				}
				else
				{
					m_currentLocalDirectory = Path.Combine(m_currentLocalDirectory, itemInfo.BlueprintName);
				}
				CheckCurrentLocalDirectory();
				RefreshAndReloadBlueprintList();
				return;
			}
			m_blueprintBeingLoaded = true;
			switch (itemInfo.Type)
			{
			case MyBlueprintTypeEnum.STEAM:
				Task = Parallel.Start(delegate
				{
					if (!MyWorkshop.IsUpToDate(itemInfo.Item))
					{
						DownloadBlueprintFromSteam(itemInfo.Item);
					}
				}, delegate
				{
					CopyBlueprintAndClose();
				});
				break;
			case MyBlueprintTypeEnum.LOCAL:
			case MyBlueprintTypeEnum.DEFAULT:
			case MyBlueprintTypeEnum.CLOUD:
				CopyBlueprintAndClose();
				break;
			case MyBlueprintTypeEnum.SHARED:
				OpenSharedBlueprint(itemInfo);
				break;
			}
		}

		private static void CheckCurrentLocalDirectory()
		{
			if (!Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory)))
			{
				m_currentLocalDirectory = string.Empty;
			}
		}

		private void OnOk(MyGuiControlButton button)
		{
			button.Enabled = false;
			Ok();
		}

		private void OnCancel(MyGuiControlButton button)
		{
			CloseScreen();
		}

		private void OnReload(MyGuiControlButton button)
		{
			m_selectedItem = null;
			m_detailsButton.Enabled = false;
			m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_screenshotButton.Enabled = false;
			m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
			m_downloadFinished.Clear();
			m_downloadQueued.Clear();
			RefreshAndReloadBlueprintList();
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
				MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
				if (myBlueprintItemInfo != null)
				{
					switch (myBlueprintItemInfo.Type)
					{
					case MyBlueprintTypeEnum.SHARED:
						break;
					case MyBlueprintTypeEnum.STEAM:
						OpenSteamWorkshopDetail(myBlueprintItemInfo);
						break;
					case MyBlueprintTypeEnum.LOCAL:
						OpenLocalBlueprintDetail();
						break;
					case MyBlueprintTypeEnum.DEFAULT:
						OpenDefaultBlueprintDetail();
						break;
					case MyBlueprintTypeEnum.CLOUD:
						OpenCloudBlueprintDetail();
						break;
					}
				}
			}
		}

		private void OpenCloudBlueprintDetail()
		{
			m_thumbnailImage.Visible = false;
			m_detailScreen = new MyGuiDetailScreenCloud(delegate(MyGuiControlListbox.Item item)
			{
				if (item == null)
				{
					m_screenshotButton.Enabled = false;
					m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
					m_detailsButton.Enabled = false;
					m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
					m_replaceButton.Enabled = false;
					m_deleteButton.Enabled = false;
					m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
				}
				m_selectedItem = item;
				m_activeDetail = false;
				m_detailScreen = null;
				if (Task.IsComplete)
				{
					RefreshBlueprintList();
				}
			}, m_selectedItem, this, m_selectedThumbnailPath, m_textScale);
			m_activeDetail = true;
			MyScreenManager.InputToNonFocusedScreens = true;
			MyScreenManager.AddScreen(m_detailScreen);
		}

		private void OpenDefaultBlueprintDetail()
		{
			if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_selectedItem.Text.ToString(), "bp.sbc")))
			{
				m_thumbnailImage.Visible = false;
				m_detailScreen = new MyGuiDetailScreenDefault(delegate(MyGuiControlListbox.Item item)
				{
					if (item == null)
					{
						m_screenshotButton.Enabled = false;
						m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
						m_detailsButton.Enabled = false;
						m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
						m_replaceButton.Enabled = false;
						m_deleteButton.Enabled = false;
						m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
					}
					m_selectedItem = item;
					m_activeDetail = false;
					m_detailScreen = null;
					if (Task.IsComplete)
					{
						RefreshBlueprintList();
					}
				}, m_selectedItem, this, m_selectedThumbnailPath, m_textScale);
				m_activeDetail = true;
				MyScreenManager.InputToNonFocusedScreens = true;
				MyScreenManager.AddScreen(m_detailScreen);
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Error), messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint)));
			}
		}

		private void OpenLocalBlueprintDetail()
		{
			if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, m_selectedItem.Text.ToString(), "bp.sbc")))
			{
				m_thumbnailImage.Visible = false;
				m_detailScreen = new MyGuiDetailScreenLocal(delegate(MyGuiControlListbox.Item item)
				{
					if (item == null)
					{
						m_screenshotButton.Enabled = false;
						m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
						m_detailsButton.Enabled = false;
						m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
						m_replaceButton.Enabled = false;
						m_deleteButton.Enabled = false;
						m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
					}
					m_selectedItem = item;
					m_activeDetail = false;
					m_detailScreen = null;
					if (Task.IsComplete)
					{
						RefreshBlueprintList();
					}
				}, m_selectedItem, this, m_selectedThumbnailPath, m_textScale, m_currentLocalDirectory);
				m_activeDetail = true;
				MyScreenManager.InputToNonFocusedScreens = true;
				MyScreenManager.AddScreen(m_detailScreen);
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Error), messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint)));
			}
		}

		private void OpenSteamWorkshopDetail(MyBlueprintItemInfo blueprintInfo)
		{
			MyWorkshopItem workshopData = blueprintInfo.Item;
			Task = Parallel.Start(delegate
			{
				if (!MyWorkshop.IsUpToDate(workshopData))
				{
					DownloadBlueprintFromSteam(workshopData);
				}
			}, delegate
			{
				OnBlueprintDownloadedDetails(workshopData);
			});
		}

		private void DownloadBlueprintFromSteam(MyWorkshopItem item)
		{
			if (!MyWorkshop.IsUpToDate(item))
			{
				MyWorkshop.DownloadBlueprintBlockingUGC(item, check: false);
				ExtractWorkshopItem(item);
			}
		}

		private void OnBlueprintDownloadedDetails(MyWorkshopItem workshopDetails)
		{
			if (File.Exists(workshopDetails.Folder))
			{
				m_thumbnailImage.Visible = false;
				m_detailScreen = new MyGuiDetailScreenSteam(delegate(MyGuiControlListbox.Item item)
				{
					m_selectedItem = item;
					m_activeDetail = false;
					m_detailScreen = null;
					if (Task.IsComplete)
					{
						RefreshBlueprintList();
					}
				}, m_selectedItem, this, m_selectedThumbnailPath, m_textScale);
				m_activeDetail = true;
				MyScreenManager.InputToNonFocusedScreens = true;
				MyScreenManager.AddScreen(m_detailScreen);
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.Error), messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint)));
			}
		}

		private void OnBlueprintDownloadedThumbnail(MyWorkshopItem item)
		{
			m_okButton.Enabled = true;
			m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
			m_blueprintList.Enabled = true;
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp", item.Id.ToString(), m_thumbImageName);
			if (File.Exists(text))
			{
				m_thumbnailImage.SetTexture(text);
				if (!m_activeDetail && m_thumbnailImage.IsAnyTextureValid())
				{
					m_thumbnailImage.Visible = true;
				}
			}
			else
			{
				m_thumbnailImage.Visible = false;
				m_thumbnailImage.SetTexture();
			}
			m_downloadQueued.Remove(item.Id);
			m_downloadFinished.Add(item.Id);
		}

		public void TakeScreenshot(string name)
		{
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, name, m_thumbImageName);
			MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), text, debug: false, ignoreSprites: true, showNotification: false);
			MyRenderProxy.UnloadTexture(text);
			m_thumbnailImage.Visible = true;
		}

		private void OnScreenshot(MyGuiControlButton button)
		{
			if (m_selectedItem != null)
			{
				TakeScreenshot(m_selectedItem.Text.ToString());
			}
		}

		public void CreateFromClipboard(bool withScreenshot = false, bool replace = false)
		{
			if (m_clipboard.CopiedGridsName != null)
			{
				string text = MyUtils.StripInvalidChars(m_clipboard.CopiedGridsName);
				string text2 = text;
				string text3 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, text);
				int num = 1;
				while (MyFileSystem.DirectoryExists(text3))
				{
					text2 = text + "_" + num;
					text3 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, text2);
					num++;
				}
				Path.Combine(text3, m_thumbImageName);
				if (withScreenshot && !MySandboxGame.Config.EnableSteamCloud)
				{
					TakeScreenshot(text2);
				}
				MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
				myObjectBuilder_ShipBlueprintDefinition.Id = new MyDefinitionId(new MyObjectBuilderType(typeof(MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(text));
				myObjectBuilder_ShipBlueprintDefinition.CubeGrids = m_clipboard.CopiedGrids.ToArray();
				myObjectBuilder_ShipBlueprintDefinition.RespawnShip = false;
				myObjectBuilder_ShipBlueprintDefinition.DisplayName = MyGameService.UserName;
				myObjectBuilder_ShipBlueprintDefinition.OwnerSteamId = Sync.MyId;
				myObjectBuilder_ShipBlueprintDefinition.CubeGrids[0].DisplayName = text;
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
				myObjectBuilder_Definitions.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
				myObjectBuilder_Definitions.ShipBlueprints[0] = myObjectBuilder_ShipBlueprintDefinition;
				MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, m_currentLocalDirectory, replace);
				RefreshBlueprintList();
			}
		}

		private void OnDelete(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Delete), messageText: MyTexts.Get(MySpaceTexts.DeleteBlueprintQuestion), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES && m_selectedItem != null)
				{
					MyBlueprintItemInfo myBlueprintItemInfo = m_selectedItem.UserData as MyBlueprintItemInfo;
					if (myBlueprintItemInfo != null)
					{
						switch (myBlueprintItemInfo.Type)
						{
						case MyBlueprintTypeEnum.LOCAL:
						case MyBlueprintTypeEnum.DEFAULT:
						{
							string file = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, m_currentLocalDirectory, myBlueprintItemInfo.BlueprintName);
							if (DeleteItem(file))
							{
								ResetBlueprintUI();
							}
							break;
						}
						case MyBlueprintTypeEnum.CLOUD:
						{
							string text = MyBlueprintUtils.BLUEPRINT_CLOUD_DIRECTORY + "/" + myBlueprintItemInfo.BlueprintName + "/" + MyBlueprintUtils.BLUEPRINT_LOCAL_NAME;
							if (MyGameService.DeleteFromCloud(text))
							{
								ResetBlueprintUI();
								text += "B5";
								MyGameService.DeleteFromCloud(text);
							}
							break;
						}
						}
						RefreshBlueprintList();
					}
				}
			}));
		}

		private void ResetBlueprintUI()
		{
			m_deleteButton.Enabled = false;
			m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
			m_detailsButton.Enabled = false;
			m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
			m_screenshotButton.Enabled = false;
			m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
			m_replaceButton.Enabled = false;
			m_selectedItem = null;
		}

		private void OnCreate(MyGuiControlButton button)
		{
			CreateFromClipboard();
		}

		private void OnReplace(MyGuiControlButton button)
		{
			if (m_selectedItem != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace), messageText: MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
				{
					if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						string text = m_selectedItem.Text.ToString();
						string text2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_currentLocalDirectory, text, "bp.sbc");
						if (File.Exists(text2))
						{
							MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyBlueprintUtils.LoadPrefab(text2);
							m_clipboard.CopiedGrids[0].DisplayName = text;
							myObjectBuilder_Definitions.ShipBlueprints[0].CubeGrids = m_clipboard.CopiedGrids.ToArray();
							MyBlueprintUtils.SavePrefabToFile(myObjectBuilder_Definitions, m_clipboard.CopiedGridsName, m_currentLocalDirectory, replace: true);
							RefreshBlueprintList();
						}
					}
				}));
			}
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyAnalyticsHelper.ReportActivityEnd(null, "show_blueprints");
			if (m_activeDetail)
			{
				m_detailScreen.CloseScreen();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!m_blueprintList.IsMouseOver)
			{
				m_thumbnailImage.Visible = false;
			}
			if (!Task.IsComplete)
			{
				m_wheel.Visible = true;
			}
			if (Task.IsComplete)
			{
				m_wheel.Visible = false;
				if (m_needsExtract)
				{
					GetWorkshopItemsSteam();
					m_needsExtract = false;
					RefreshBlueprintList();
				}
			}
			return base.Update(hasFocus);
		}

		public override bool CloseScreen()
		{
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
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10))
			{
				CloseScreen();
			}
		}
	}
}
