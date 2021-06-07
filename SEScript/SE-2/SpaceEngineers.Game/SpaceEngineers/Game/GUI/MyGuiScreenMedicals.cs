using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
	[StaticEventOwner]
	public class MyGuiScreenMedicals : MyGuiScreenBase
	{
		[Serializable]
		private class MyPlanetInfo
		{
			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EPlanetId_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in long value)
				{
					owner.PlanetId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out long value)
				{
					value = owner.PlanetId;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EPlanetName_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in string value)
				{
					owner.PlanetName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out string value)
				{
					value = owner.PlanetName;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EWorldAABB_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, BoundingBoxD>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in BoundingBoxD value)
				{
					owner.WorldAABB = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out BoundingBoxD value)
				{
					value = owner.WorldAABB;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EGravity_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in float value)
				{
					owner.Gravity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out float value)
				{
					value = owner.Gravity;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EOxygenLevel_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in float value)
				{
					owner.OxygenLevel = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out float value)
				{
					value = owner.OxygenLevel;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003EDropPodForDetail_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in string value)
				{
					owner.DropPodForDetail = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out string value)
				{
					value = owner.DropPodForDetail;
				}
			}

			protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_003ERespawnShipForCooldownCheck_003C_003EAccessor : IMemberAccessor<MyPlanetInfo, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPlanetInfo owner, in string value)
				{
					owner.RespawnShipForCooldownCheck = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPlanetInfo owner, out string value)
				{
					value = owner.RespawnShipForCooldownCheck;
				}
			}

			public long PlanetId;

			public string PlanetName;

			public BoundingBoxD WorldAABB;

			public float Gravity;

			public float OxygenLevel;

			[Nullable]
			public string DropPodForDetail;

			public string RespawnShipForCooldownCheck;
		}

		protected sealed class RefreshRespawnPointsRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RefreshRespawnPointsRequest();
			}
		}

		protected sealed class RequestRespawnPointsResponse_003C_003ESystem_Collections_Generic_List_00601_003CSpaceEngineers_Game_World_MySpaceRespawnComponent_003C_003EMyRespawnPointInfo_003E_0023SpaceEngineers_Game_GUI_MyGuiScreenMedicals_003C_003EMyPlanetInfo_003C_0023_003E : ICallSite<IMyEventOwner, List<MySpaceRespawnComponent.MyRespawnPointInfo>, MyPlanetInfo[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms, in MyPlanetInfo[] planetInfos, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestRespawnPointsResponse(medicalRooms, planetInfos);
			}
		}

		private static readonly TimeSpan m_refreshInterval = TimeSpan.FromSeconds(10.0);

		private MyGuiControlLabel m_labelNoRespawn;

		private StringBuilder m_noRespawnHeader = new StringBuilder();

		private MyGuiControlTable m_respawnsTable;

		private MyGuiControlButton m_respawnButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_MotdButton;

		private MyGuiControlMultilineText m_noRespawnText;

		private MyGuiControlButton m_backToFactionsButton;

		private MyGuiControlButton m_showPlayersButton;

		private MyGuiControlTable m_factionsTable;

		private MyGuiControlButton m_selectFactionButton;

		private bool m_showFactions;

		private bool m_showMotD = true;

		private bool m_hideEmptyMotD = true;

		private bool m_isMotdOpen;

		private string m_lastMotD = string.Empty;

		private MyGuiControlMultilineText m_motdMultiline;

		private bool m_blackgroundDrawFull = true;

		private float m_blackgroundFade = 1f;

		private bool m_isMultiplayerReady;

		private bool m_paused;

		private bool m_medbaySelect_SuppressNext;

		private MyGuiControlMultilineText m_multilineRespawnWhenShipReady;

		private object m_selectedRespawn;

		private bool m_haveSelection;

		private object m_selectedRowData;

		private MyGuiControlRotatingWheel m_rotatingWheelControl;

		private MyGuiControlLabel m_rotatingWheelLabel;

		private int m_streamingTimeout = 240;

		private bool m_streamingStarted;

		private bool m_selectedRowIsStreamable;

		private DateTime m_nextRefresh;

		private long m_requestedReplicable;

		private bool m_respawning;

		private MyGuiControlTable.Row m_previouslySelected;

		private MyGuiControlParent m_descriptionControl;

		private List<string> m_preloadedTextures = new List<string>();

		private StringBuilder m_factionTooltip = new StringBuilder();

		private MyFaction m_applyingToFaction;

		private long m_restrictedRespawn;

		private bool m_waitingForRespawnShip;

		private const int SAFE_FRAME_COUNT = 5;

		private int m_blackgroundCounter;

		private int m_lastTimeSec = -1;

		private readonly List<MyPhysics.HitInfo> m_raycastList = new List<MyPhysics.HitInfo>(16);

		private float m_cameraRayLength = 20f;

		private long m_lastMedicalRoomId;

		public static MyGuiScreenMedicals Static
		{
			get;
			private set;
		}

		public static StringBuilder NoRespawnText
		{
			set
			{
				if (Static != null)
				{
					Static.m_noRespawnText.Text = value;
				}
			}
		}

		public static int ItemsInTable
		{
			get
			{
				if (Static == null || Static.m_respawnsTable == null)
				{
					return 0;
				}
				return Static.m_respawnsTable.RowsCount;
			}
		}

		public bool IsBlackgroundVisible
		{
			get
			{
				if (!m_blackgroundDrawFull)
				{
					return m_blackgroundFade > 0f;
				}
				return true;
			}
		}

		public bool IsBlackgroundFading
		{
			get
			{
				if (!m_blackgroundDrawFull)
				{
					return m_blackgroundFade > 0f;
				}
				return false;
			}
		}

		public MyGuiScreenMedicals(bool showFactions, long restrictedRespawn)
			: base(new Vector2(0.1f, 0.1f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.4f, 0.9f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_showFactions = showFactions;
			m_restrictedRespawn = restrictedRespawn;
			m_showMotD = (MySession.ShowMotD && !Sandbox.Engine.Platform.Game.IsDedicated);
			m_position = GetPossitionFromRatio();
			Static = this;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = false;
			m_closeOnEsc = false;
			m_selectedRespawn = null;
			base.CanBeHidden = false;
			RecreateControls(constructor: true);
			if (!Sync.MultiplayerActive)
			{
				MySandboxGame.PausePush();
				m_paused = true;
			}
			MySession.Static.Factions.OnPlayerJoined += OnPlayerJoinedFaction;
			MySession.Static.Factions.OnPlayerLeft += OnPlayerKickedFromFaction;
			MyCampaignManager.AfterCampaignLocalizationsLoaded = (Action)Delegate.Combine(MyCampaignManager.AfterCampaignLocalizationsLoaded, new Action(AfterLocalizationLoaded));
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMyGuiScreenMedicals";
		}

		protected override void OnShow()
		{
			MyHud.Notifications.Clear();
		}

		protected override void OnClosed()
		{
			if (m_paused)
			{
				m_paused = false;
				MySandboxGame.PausePop();
			}
			MyHud.RotatingWheelText = MyHud.Empty;
			UnrequestReplicable();
			MySession.Static.Factions.OnPlayerJoined -= OnPlayerJoinedFaction;
			MySession.Static.Factions.OnPlayerLeft -= OnPlayerKickedFromFaction;
			MyCampaignManager.AfterCampaignLocalizationsLoaded = (Action)Delegate.Remove(MyCampaignManager.AfterCampaignLocalizationsLoaded, new Action(AfterLocalizationLoaded));
			base.OnClosed();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MAIN_MENU))
			{
				if (!MyInput.Static.IsAnyShiftKeyPressed())
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(new MyGuiScreenMainMenu());
				}
				else if (MySession.Static.HasPlayerSpectatorRights(Sync.MyId))
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: MyTexts.Get(MySpaceTexts.ScreenMedicals_ActivateSpectator_Confirm), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							Close();
						}
					}, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
				}
			}
		}

		public override void RecreateControls(bool constructor)
		{
			Vector2 zero = Vector2.Zero;
			StringBuilder text;
			if (m_showMotD)
			{
				m_isMotdOpen = true;
				m_size = new Vector2(0.4f, 0.9f);
				m_position = new Vector2(0.8f, 0.5f);
				zero = new Vector2(0f, 0f);
				text = MyTexts.Get(MyCommonTexts.HideMotD);
			}
			else
			{
				m_isMotdOpen = false;
				m_size = new Vector2(0.4f, 0.9f);
				m_position = new Vector2(0.8f, 0.5f);
				zero = new Vector2(0f, 0f);
				text = MyTexts.Get(MyCommonTexts.ShowMotD);
			}
			base.RecreateControls(constructor);
			if (m_showMotD && !string.IsNullOrEmpty(m_lastMotD))
			{
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
				myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				myGuiControlImage.SetTexture(MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture);
				myGuiControlImage.Size = new Vector2(0.58f, 0.9f);
				myGuiControlImage.Position = new Vector2(-0.23f, -0.45f);
				Controls.Add(myGuiControlImage);
				MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.61f, 0f) - new Vector2(m_size.Value.X * 0.9f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 1.35f);
				Controls.Add(myGuiControlSeparatorList);
				MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage();
				myGuiControlImage2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				myGuiControlImage2.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
				myGuiControlImage2.Size = new Vector2(0.54f, 0.783f);
				myGuiControlImage2.Position = new Vector2(-0.25f, -0.358f);
				Controls.Add(myGuiControlImage2);
				m_motdMultiline = new MyGuiControlMultilineText
				{
					Position = new Vector2(-0.77f, -0.34f),
					Size = new Vector2(0.515f, 0.74f),
					Font = "Blue",
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
					TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(m_lastMotD));
				Controls.Add(m_motdMultiline);
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f, (0f - m_size.Value.Y) / 2f + MyGuiConstants.SCREEN_CAPTION_DELTA_Y) + new Vector2(-0.52f, 0.003f), null, MyTexts.GetString(MyCommonTexts.MotD_Caption), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				myGuiControlLabel.Name = "CaptionLabel";
				myGuiControlLabel.Font = "ScreenCaption";
				Controls.Add(myGuiControlLabel);
			}
			if (!m_showFactions)
			{
				RecreateControlsRespawn(zero);
			}
			else
			{
				RecreateControlsFactions(zero);
			}
			m_MotdButton = new MyGuiControlButton(new Vector2(0.003f, m_size.Value.Y / 2f - 0.045f) + zero, MyGuiControlButtonStyleEnum.Rectangular, new Vector2(0.36f, 0.033f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onMotdClick);
			Controls.Add(m_MotdButton);
			if (Sandbox.Engine.Platform.Game.IsDedicated || string.IsNullOrEmpty(m_lastMotD))
			{
				m_MotdButton.Enabled = false;
			}
			new MyGuiControlLabel(new Vector2(-0.175f, -0.34f), null, MyTexts.GetString(MyCommonTexts.MotDCaption), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			m_lastMedicalRoomId = 0L;
			m_rotatingWheelControl = new MyGuiControlRotatingWheel(new Vector2(0.5f, 0.8f) - m_position);
			Controls.Add(m_rotatingWheelControl);
			Controls.Add(m_rotatingWheelLabel = new MyGuiControlLabel());
			MyHud.RotatingWheelText = MyTexts.Get(MySpaceTexts.LoadingWheel_Streaming);
			if (MySession.Static.HasPlayerSpectatorRights(Sync.MyId))
			{
				MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(0f, 0.51f) * m_size.Value, null, MyTexts.GetString(MySpaceTexts.ScreenMedicals_ActivateSpectator), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				Controls.Add(control);
			}
		}

		private void RecreateControlsRespawn(Vector2 offsetting)
		{
			AddCaption(MyCommonTexts.Medicals_Title, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.9f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.9f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.9f / 2f, (0f - m_size.Value.Y) / 2f + 0.18f + 0.01f), m_size.Value.X * 0.9f);
			Controls.Add(myGuiControlSeparatorList);
			m_multilineRespawnWhenShipReady = new MyGuiControlMultilineText
			{
				Position = new Vector2(0f, -0.5f * base.Size.Value.Y + 80f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				Size = new Vector2(base.Size.Value.X * 0.85f, 75f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				Font = "Red",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
			};
			Controls.Add(m_multilineRespawnWhenShipReady);
			UpdateRespawnShipLabel();
			m_respawnsTable = new MyGuiControlTable();
			m_respawnsTable.Position = new Vector2(0f, (0f - m_size.Value.Y) / 2f + 0.7f) + offsetting + new Vector2(0f, -0.03f);
			m_respawnsTable.Size = new Vector2(575f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1.3f);
			m_respawnsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			m_respawnsTable.VisibleRowsCount = 16;
			Controls.Add(m_respawnsTable);
			m_respawnsTable.ColumnsCount = 2;
			m_respawnsTable.ItemSelected += OnTableItemSelected;
			m_respawnsTable.ItemDoubleClicked += OnTableItemDoubleClick;
			m_respawnsTable.ItemConfirmed += OnTableItemDoubleClick;
			m_respawnsTable.ItemMouseOver += respawnsTable_ItemMouseOver;
			m_respawnsTable.SetCustomColumnWidths(new float[2]
			{
				0.5f,
				0.5f
			});
			m_respawnsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
			m_respawnsTable.SetColumnName(1, MyTexts.Get(MySpaceTexts.ScreenMedicals_OwnerTimeoutColumn));
			m_labelNoRespawn = new MyGuiControlLabel
			{
				Position = new Vector2(0f, -0.35f) + offsetting,
				ColorMask = Color.Red,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			Controls.Add(m_labelNoRespawn);
			if (m_applyingToFaction != null)
			{
				MyGuiControlMultilineText control = new MyGuiControlMultilineText(new Vector2(-0.02f, m_size.Value.Y / 2f - 0.21f) + offsetting, new Vector2(0.32f, 0.5f), null, "Red", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyTexts.Get(MySpaceTexts.ScreenMedicals_WaitingForAcceptance));
				Controls.Add(control);
			}
			m_backToFactionsButton = new MyGuiControlButton(new Vector2(0.003f, m_size.Value.Y / 2f - 0.155f) + offsetting, MyGuiControlButtonStyleEnum.Rectangular, new Vector2(0.36f, 0.033f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenMedicals_BackToFactionSelection), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnBackToFactionsClick);
			Controls.Add(m_backToFactionsButton);
			m_backToFactionsButton.Enabled = !IsPlayerInFaction();
			m_respawnButton = new MyGuiControlButton(new Vector2(-0.09f, m_size.Value.Y / 2f - 0.1f) + offsetting, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Respawn), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onRespawnClick);
			Controls.Add(m_respawnButton);
			base.FocusedControl = m_respawnButton;
			m_refreshButton = new MyGuiControlButton(new Vector2(0.095f, m_size.Value.Y / 2f - 0.1f) + offsetting, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Refresh), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnRefreshClick);
			Controls.Add(m_refreshButton);
			m_noRespawnText = new MyGuiControlMultilineText(new Vector2(-0.02f, -0.19f) + offsetting, new Vector2(0.32f, 0.5f), null, "Red", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyTexts.Get(MySpaceTexts.ScreenMedicals_NoRespawnPossible));
			Controls.Add(m_noRespawnText);
			CreateDetailInfoControl();
			RefreshRespawnPoints();
			Controls.Add(m_descriptionControl);
		}

		private void RecreateControlsFactions(Vector2 offsetting)
		{
			AddCaption(MyCommonTexts.Factions, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.9f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.9f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.9f / 2f, (0f - m_size.Value.Y) / 2f + 0.18f + 0.01f), m_size.Value.X * 0.9f);
			Controls.Add(myGuiControlSeparatorList);
			m_factionsTable = new MyGuiControlTable();
			m_factionsTable.Position = new Vector2(0f, (0f - m_size.Value.Y) / 2f + 0.7f) + offsetting + new Vector2(0f, -0.604f);
			m_factionsTable.Size = new Vector2(575f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1.3f);
			m_factionsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			m_factionsTable.VisibleRowsCount = 16;
			Controls.Add(m_factionsTable);
			m_factionsTable.ColumnsCount = 2;
			m_factionsTable.ItemSelected += OnFactionsTableItemSelected;
			m_factionsTable.ItemDoubleClicked += OnFactionsTableItemDoubleClick;
			m_factionsTable.ItemConfirmed += OnFactionsTableItemDoubleClick;
			m_factionsTable.ItemMouseOver += OnFactionsTableItemMouseOver;
			m_factionsTable.SetCustomColumnWidths(new float[2]
			{
				0.2f,
				0.8f
			});
			m_factionsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Tag));
			m_factionsTable.SetColumnName(1, MyTexts.Get(MyCommonTexts.Name));
			m_showPlayersButton = new MyGuiControlButton(new Vector2(0.003f, m_size.Value.Y / 2f - 0.155f) + offsetting, MyGuiControlButtonStyleEnum.Rectangular, new Vector2(0.36f, 0.033f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenMenuButtonPlayers), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnShowPlayersClick);
			Controls.Add(m_showPlayersButton);
			m_showPlayersButton.Enabled = (MyMultiplayer.Static != null);
			m_selectFactionButton = new MyGuiControlButton(new Vector2(-0.09f, m_size.Value.Y / 2f - 0.1f) + offsetting, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Join), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnFactionSelectClick);
			Controls.Add(m_selectFactionButton);
			base.FocusedControl = m_selectFactionButton;
			m_refreshButton = new MyGuiControlButton(new Vector2(0.095f, m_size.Value.Y / 2f - 0.1f) + offsetting, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Refresh), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnFactionsRefreshClick);
			Controls.Add(m_refreshButton);
			RefreshFactions();
		}

		public void CreateDetailInfoControl()
		{
			float num = 0.25f;
			m_descriptionControl = new MyGuiControlParent
			{
				BorderSize = 1,
				BorderEnabled = true,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f),
				Visible = false,
				Size = new Vector2(num, 0f),
				Position = new Vector2(-0.33f, -0.224f),
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				Controls = 
				{
					(MyGuiControlBase)new MyGuiControlLabel
					{
						TextToDraw = new StringBuilder(),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
					},
					(MyGuiControlBase)new MyGuiControlImage
					{
						BorderSize = 1,
						BorderEnabled = true,
						BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f),
						Visible = false,
						Size = num * new Vector2(1f, 0.7f),
						Padding = new MyGuiBorderThickness(2f, 2f, 2f, 2f),
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM
					},
					(MyGuiControlBase)new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: false, drawScrollbarH: false)
					{
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
						Size = new Vector2(num, 0.05f)
					}
				}
			};
		}

		private Vector2 GetPossitionFromRatio()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			switch (MyVideoSettingsManager.GetClosestAspectRatio((float)fullscreenRectangle.Width / (float)fullscreenRectangle.Height))
			{
			case MyAspectRatioEnum.Normal_4_3:
				return new Vector2(0.79f, 0.52f);
			case MyAspectRatioEnum.Unsupported_5_4:
				return new Vector2(0.76f, 0.52f);
			case MyAspectRatioEnum.Normal_16_9:
				return new Vector2(0.95f, 0.52f);
			case MyAspectRatioEnum.Normal_16_10:
				return new Vector2(0.88f, 0.52f);
			default:
				return new Vector2(0.95f, 0.52f);
			}
		}

		private void RefreshRotatingWheelLabel()
		{
			bool visible = MyHud.RotatingWheelVisible || m_respawning || (m_blackgroundDrawFull && m_selectedRowIsStreamable);
			m_rotatingWheelLabel.Visible = visible;
			m_rotatingWheelControl.Visible = visible;
			if ((MyHud.RotatingWheelVisible || m_respawning) && m_rotatingWheelLabel.TextToDraw != MyHud.RotatingWheelText)
			{
				m_rotatingWheelLabel.Position = m_rotatingWheelControl.Position + new Vector2(0f, 0.05f);
				m_rotatingWheelLabel.TextToDraw = MyHud.RotatingWheelText;
				Vector2 textSize = m_rotatingWheelLabel.GetTextSize();
				m_rotatingWheelLabel.PositionX -= textSize.X / 2f;
			}
		}

		private void RefreshRespawnPoints()
		{
			m_respawnsTable.Clear();
			m_nextRefresh = DateTime.UtcNow + m_refreshInterval;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RefreshRespawnPointsRequest);
		}

		private void UpdateSpawnTimes()
		{
			for (int i = 0; i < m_respawnsTable.RowsCount; i++)
			{
				MyGuiControlTable.Row row = m_respawnsTable.GetRow(i);
				string text = (row.UserData as MyRespawnShipDefinition)?.Id.SubtypeName ?? (row.UserData as MyPlanetInfo)?.RespawnShipForCooldownCheck;
				if (text != null)
				{
					MyGuiControlTable.Cell cell = row.GetCell(0);
					MyGuiControlTable.Cell cell2 = row.GetCell(1);
					Color cooldownInfo = GetCooldownInfo(text, cell2.Text.Clear());
					cell.TextColor = cooldownInfo;
					cell2.TextColor = cooldownInfo;
				}
			}
		}

		private static Color GetCooldownInfo(string respawnShipId, StringBuilder text)
		{
			MySpaceRespawnComponent @static = MySpaceRespawnComponent.Static;
			int num = (MySession.Static.LocalHumanPlayer != null) ? @static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShipId) : 0;
			bool flag = num == 0;
			if (!@static.IsSynced)
			{
				text.Append((object)MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipNotReady));
			}
			else if (flag)
			{
				text.Append((object)MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipReady));
			}
			else
			{
				MyValueFormatter.AppendTimeExact(num, text);
			}
			return MyGuiControlBase.ApplyColorMaskModifiers(Color.White, flag, 1f);
		}

		private void UpdateRespawnShipLabel()
		{
			if (m_selectedRespawn == null)
			{
				m_multilineRespawnWhenShipReady.Visible = false;
				return;
			}
			object selectedRespawn = m_selectedRespawn;
			if (selectedRespawn != null)
			{
				MyPlanetInfo myPlanetInfo;
				string arg;
				string respawnShipId;
				if ((myPlanetInfo = (selectedRespawn as MyPlanetInfo)) == null)
				{
					MyRespawnShipDefinition myRespawnShipDefinition;
					if ((myRespawnShipDefinition = (selectedRespawn as MyRespawnShipDefinition)) == null)
					{
						goto IL_005c;
					}
					arg = myRespawnShipDefinition.DisplayNameText;
					respawnShipId = myRespawnShipDefinition.Id.SubtypeName;
				}
				else
				{
					arg = myPlanetInfo.PlanetName;
					respawnShipId = myPlanetInfo.RespawnShipForCooldownCheck;
				}
				MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShipId);
				m_multilineRespawnWhenShipReady.Text.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScreenMedicals_RespawnWhenShipReady), arg);
				m_multilineRespawnWhenShipReady.RefreshText(useEnum: false);
				m_multilineRespawnWhenShipReady.Visible = true;
				return;
			}
			goto IL_005c;
			IL_005c:
			throw new Exception("Invalid branch " + m_selectedRespawn);
		}

		private static StringBuilder GetOwnerDisplayName(long owner)
		{
			if (owner == 0L)
			{
				return MyTexts.Get(MySpaceTexts.BlockOwner_Nobody);
			}
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(owner);
			if (myIdentity != null)
			{
				return new StringBuilder(myIdentity.DisplayName);
			}
			return MyTexts.Get(MySpaceTexts.BlockOwner_Unknown);
		}

		[Event(null, 709)]
		[Reliable]
		[Server]
		private static void RefreshRespawnPointsRequest()
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num != 0L)
			{
				ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo> availableRespawnPoints = MySpaceRespawnComponent.GetAvailableRespawnPoints(num, includePublicSpawns: true);
				try
				{
					MyPlanetInfo[] arg = VRage.Library.Collections.EmptyArray<MyPlanetInfo>.Value;
					if (MySession.Static.Settings.EnableRespawnShips)
					{
						MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
						arg = (from x in MyPlanets.GetPlanets().Select(delegate(MyPlanet planet)
							{
								ClearToken<MyRespawnShipDefinition> respawnShips = MySpaceRespawnComponent.GetRespawnShips(planet);
								try
								{
									if (respawnShips.List.Count == 0)
									{
										return null;
									}
									string dropPodForDetail = null;
									if (respawnShips.List.Count == 1)
									{
										dropPodForDetail = respawnShips.List[0].Id.SubtypeId.String;
									}
									float oxygenLevel = 0f;
									if (planet.HasAtmosphere)
									{
										MyPlanetAtmosphere atmosphere = planet.Generator.Atmosphere;
										if (atmosphere.Breathable)
										{
											oxygenLevel = atmosphere.OxygenDensity;
										}
									}
									playerId = new MyPlayer.PlayerId(MyEventContext.Current.Sender.Value, 0);
									MyRespawnShipDefinition myRespawnShipDefinition = respawnShips.List.MinBy((MyRespawnShipDefinition ship) => MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(playerId, ship.Id.SubtypeName));
									return new MyPlanetInfo
									{
										PlanetName = planet.Name,
										PlanetId = planet.EntityId,
										OxygenLevel = oxygenLevel,
										DropPodForDetail = dropPodForDetail,
										WorldAABB = planet.PositionComp.WorldAABB,
										Gravity = planet.GetInitArguments.SurfaceGravity,
										RespawnShipForCooldownCheck = myRespawnShipDefinition.Id.SubtypeName
									};
								}
								finally
								{
									((IDisposable)respawnShips).Dispose();
								}
							})
							where x != null
							select x).ToArray();
					}
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestRespawnPointsResponse, availableRespawnPoints.List, arg, MyEventContext.Current.Sender);
				}
				finally
				{
					((IDisposable)availableRespawnPoints).Dispose();
				}
			}
		}

		[Event(null, 769)]
		[Reliable]
		[Client]
		private static void RequestRespawnPointsResponse(List<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms, MyPlanetInfo[] planetInfos)
		{
			Static.RefreshMedicalRooms(medicalRooms, planetInfos);
		}

		public static bool EqualRespawns(object first, object second)
		{
			if (first == null || second == null)
			{
				return first == second;
			}
			if (first.GetType() != second.GetType())
			{
				return false;
			}
			if (first is MySpaceRespawnComponent.MyRespawnPointInfo)
			{
				MySpaceRespawnComponent.MyRespawnPointInfo obj = first as MySpaceRespawnComponent.MyRespawnPointInfo;
				MySpaceRespawnComponent.MyRespawnPointInfo myRespawnPointInfo = second as MySpaceRespawnComponent.MyRespawnPointInfo;
				if (obj.MedicalRoomId != myRespawnPointInfo.MedicalRoomId)
				{
					return false;
				}
			}
			else if (first is MyRespawnShipDefinition)
			{
				MyRespawnShipDefinition myRespawnShipDefinition = first as MyRespawnShipDefinition;
				MyRespawnShipDefinition myRespawnShipDefinition2 = second as MyRespawnShipDefinition;
				if (myRespawnShipDefinition.Prefab == null || myRespawnShipDefinition2.Prefab == null)
				{
					return false;
				}
				if (myRespawnShipDefinition.Prefab.PrefabPath != myRespawnShipDefinition2.Prefab.PrefabPath)
				{
					return false;
				}
			}
			else if (first is MyPlanetInfo)
			{
				MyPlanetInfo obj2 = first as MyPlanetInfo;
				MyPlanetInfo myPlanetInfo = second as MyPlanetInfo;
				if (obj2.PlanetId != myPlanetInfo.PlanetId)
				{
					return false;
				}
			}
			return true;
		}

		private void RefreshMedicalRooms(ListReader<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms, MyPlanetInfo[] planetInfos)
		{
			m_respawnsTable.Clear();
			AddMedicalRespawnPoints();
			if (!MySession.Static.CreativeMode && MySession.Static.Settings.EnableRespawnShips && !MySession.Static.Settings.Scenario)
			{
				AddPlanetSpawns();
				AddSpaceRespawnShips();
				AddManuallyPositionedShips();
			}
			AddSuitRespawn();
			if (m_respawnsTable.RowsCount > 0)
			{
				if (m_haveSelection)
				{
					int num = m_respawnsTable.FindIndexByUserData(ref m_selectedRowData, EqualRespawns);
					if (num >= 0)
					{
						m_medbaySelect_SuppressNext = true;
						m_respawnsTable.SelectedRowIndex = num;
					}
					else
					{
						m_respawnsTable.SelectedRowIndex = 0;
					}
				}
				else
				{
					m_respawnsTable.SelectedRowIndex = 0;
				}
				OnTableItemSelected(null, default(MyGuiControlTable.EventArgs));
				m_noRespawnText.Visible = false;
			}
			else
			{
				m_noRespawnText.Visible = true;
			}
			void AddManuallyPositionedShips()
			{
				foreach (MyRespawnShipDefinition value in MyDefinitionManager.Static.GetRespawnShipDefinitions().Values)
				{
					if (value.SpawnPosition.HasValue && !value.UseForPlanetsWithAtmosphere && !value.UseForPlanetsWithoutAtmosphere && value.PlanetTypes == null)
					{
						MyGuiControlTable.Row row2 = new MyGuiControlTable.Row(value);
						row2.AddCell(new MyGuiControlTable.Cell(value.DisplayNameText));
						MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(string.Empty);
						GetCooldownInfo(value.Id.SubtypeName, cell.Text);
						row2.AddCell(cell);
						m_respawnsTable.Add(row2);
					}
				}
			}
			void AddMedicalRespawnPoints()
			{
				foreach (MySpaceRespawnComponent.MyRespawnPointInfo item in medicalRooms)
				{
					MyGuiControlTable.Row row5 = new MyGuiControlTable.Row(item);
					bool flag = m_restrictedRespawn == 0L || item.MedicalRoomId == m_restrictedRespawn;
					row5.AddCell(new MyGuiControlTable.Cell(item.MedicalRoomName, null, null, flag ? null : new Color?(Color.Gray)));
					row5.AddCell(new MyGuiControlTable.Cell(flag ? MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipReady) : MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipNotReady), null, null, flag ? null : new Color?(Color.Gray)));
					m_respawnsTable.Add(row5);
				}
			}
			void AddPlanetSpawns()
			{
				MyPlanetInfo[] array = planetInfos;
				foreach (MyPlanetInfo myPlanetInfo in array)
				{
					MyGuiControlTable.Row row4 = new MyGuiControlTable.Row(myPlanetInfo);
					row4.AddCell(new MyGuiControlTable.Cell(string.Format(MyTexts.GetString(MySpaceTexts.PlanetRespawnPod), myPlanetInfo.PlanetName)));
					MyGuiControlTable.Cell cell3 = new MyGuiControlTable.Cell(string.Empty);
					GetCooldownInfo(myPlanetInfo.RespawnShipForCooldownCheck, cell3.Text);
					row4.AddCell(cell3);
					m_respawnsTable.Add(row4);
				}
			}
			void AddSpaceRespawnShips()
			{
				foreach (MyRespawnShipDefinition value2 in MyDefinitionManager.Static.GetRespawnShipDefinitions().Values)
				{
					if (value2.UseForSpace && !value2.SpawnPosition.HasValue)
					{
						MyGuiControlTable.Row row3 = new MyGuiControlTable.Row(value2);
						row3.AddCell(new MyGuiControlTable.Cell(value2.DisplayNameText));
						MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(string.Empty);
						GetCooldownInfo(value2.Id.SubtypeName, cell2.Text);
						row3.AddCell(cell2);
						m_respawnsTable.Add(row3);
					}
				}
			}
			void AddSuitRespawn()
			{
				MyGuiControlTable.Row row = new MyGuiControlTable.Row();
				row.AddCell(new MyGuiControlTable.Cell(MyTexts.GetString(MySpaceTexts.SpawnInSpaceSuit)));
				row.AddCell(new MyGuiControlTable.Cell(MyTexts.GetString(MySpaceTexts.ScreenMedicals_RespawnShipReady)));
				m_respawnsTable.Add(row);
			}
		}

		private void RefreshFactions()
		{
			m_factionsTable.Clear();
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null)
			{
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					bool num = MySession.Static.Factions.IsNpcFaction(faction.Value.Tag);
					bool flag = MySession.Static.Factions.IsFactionDiscovered(localHumanPlayer.Id, faction.Value.FactionId);
					if (!num || flag)
					{
						bool flag2 = faction.Value.AcceptHumans && (faction.Value.AutoAcceptMember || faction.Value.IsAnyLeaderOnline);
						MyGuiControlTable.Row row = new MyGuiControlTable.Row(faction.Value);
						row.AddCell(new MyGuiControlTable.Cell(faction.Value.Tag, null, null, flag2 ? null : new Color?(Color.Red)));
						row.AddCell(new MyGuiControlTable.Cell(faction.Value.Name, null, null, flag2 ? null : new Color?(Color.Red)));
						m_factionsTable.Add(row);
					}
				}
				if (MySession.Static.Settings.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION || MySession.Static.IsUserAdmin(Sync.MyId))
				{
					MyGuiControlTable.Row row2 = new MyGuiControlTable.Row();
					row2.AddCell(new MyGuiControlTable.Cell());
					row2.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.ScreenMedicals_NoFaction)));
					m_factionsTable.Add(row2);
				}
				RefreshSelectFactionButton();
			}
		}

		private static void BuildOxygenLevelInfo(StringBuilder ownerText, float oxygenLevel)
		{
			if (MySession.Static.Settings.EnableOxygen)
			{
				ownerText.Append(MyTexts.GetString(MySpaceTexts.HudInfoOxygen));
				ownerText.Append(": ");
				ownerText.Append((oxygenLevel * 100f).ToString("F0"));
				ownerText.Append("% ");
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (!m_showFactions)
			{
				UpdateSpawnTimes();
			}
			bool result = base.Update(hasFocus);
			if (MySandboxGame.IsPaused)
			{
				MyHud.Notifications.UpdateBeforeSimulation();
			}
			if (IsBlackgroundVisible)
			{
				UpdateBlackground();
				Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
				MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background.dds", safeFullscreenRectangle, new Color(new Vector4(0f, 0f, 0f, m_blackgroundFade)));
			}
			if (!m_showFactions)
			{
				if (m_selectedRespawn != null)
				{
					MySpaceRespawnComponent @static = MySpaceRespawnComponent.Static;
					object selectedRespawn = m_selectedRespawn;
					if (selectedRespawn == null)
					{
						goto IL_00c3;
					}
					MyPlanetInfo myPlanetInfo;
					MyPlanetInfo myPlanetInfo2;
					string text;
					if ((myPlanetInfo = (selectedRespawn as MyPlanetInfo)) == null)
					{
						MyRespawnShipDefinition myRespawnShipDefinition;
						if ((myRespawnShipDefinition = (selectedRespawn as MyRespawnShipDefinition)) == null)
						{
							goto IL_00c3;
						}
						myPlanetInfo2 = null;
						text = myRespawnShipDefinition.Id.SubtypeName;
					}
					else
					{
						text = (myPlanetInfo2 = myPlanetInfo).RespawnShipForCooldownCheck;
					}
					int respawnCooldownSeconds = @static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, text);
					if (@static.IsSynced && respawnCooldownSeconds == 0)
					{
						RespawnImmediately((myPlanetInfo2 == null) ? text : null, myPlanetInfo2?.PlanetId);
					}
				}
				if (DateTime.UtcNow > m_nextRefresh)
				{
					RefreshRespawnPoints();
				}
				if (m_labelNoRespawn.Text == null)
				{
					m_labelNoRespawn.Visible = false;
				}
				else
				{
					m_labelNoRespawn.Visible = true;
				}
			}
			m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible;
			RefreshRotatingWheelLabel();
			if (m_respawning && MySession.Static.LocalCharacter != null && !MySession.Static.LocalCharacter.IsDead && !m_blackgroundDrawFull)
			{
				if (m_paused)
				{
					m_paused = false;
					MySandboxGame.PausePop();
				}
				m_blackgroundCounter--;
				if (m_blackgroundCounter <= 0)
				{
					CloseScreen();
				}
			}
			MyHud.IsVisible = false;
			return result;
			IL_00c3:
			throw new Exception("Invalid branch " + m_selectedRespawn);
		}

		private void UpdateBlackground()
		{
			if (m_blackgroundDrawFull)
			{
				if ((m_selectedRowIsStreamable || m_respawning) && MySandboxGame.IsGameReady && (Sync.IsServer || !MyFakes.ENABLE_WAIT_UNTIL_MULTIPLAYER_READY || m_isMultiplayerReady) && !m_waitingForRespawnShip && MySandboxGame.AreClipmapsReady)
				{
					m_blackgroundDrawFull = false;
				}
			}
			else if (m_blackgroundFade > 0f)
			{
				m_blackgroundFade -= 0.1f;
			}
		}

		public static void Close()
		{
			if (Static != null)
			{
				Static.CloseScreen();
			}
		}

		public override bool HandleInputAfterSimulation()
		{
			if (!m_showFactions && m_respawnsTable.SelectedRow != null && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity)
			{
				MySpaceRespawnComponent.MyRespawnPointInfo myRespawnPointInfo = m_respawnsTable.SelectedRow.UserData as MySpaceRespawnComponent.MyRespawnPointInfo;
				if (myRespawnPointInfo != null)
				{
					m_respawnButton.Enabled = false;
					if ((m_restrictedRespawn == 0L || myRespawnPointInfo.MedicalRoomId == m_restrictedRespawn) && MyEntities.TryGetEntityById(myRespawnPointInfo.MedicalRoomId, out MyEntity entity))
					{
						if (m_lastMedicalRoomId != myRespawnPointInfo.MedicalRoomId && (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.IsDead))
						{
							MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, entity);
							MyThirdPersonSpectator.Static.ResetInternalTimers();
							MyThirdPersonSpectator.Static.ResetViewerDistance(m_cameraRayLength);
							MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
							MyThirdPersonSpectator.Static.ResetSpring();
							m_lastMedicalRoomId = myRespawnPointInfo.MedicalRoomId;
						}
						m_respawnButton.Enabled = true;
					}
				}
			}
			return true;
		}

		public static void SetNoRespawnText(StringBuilder text, int timeSec)
		{
			if (Static != null)
			{
				Static.SetNoRespawnTexts(text, timeSec);
			}
		}

		public void SetNoRespawnTexts(StringBuilder text, int timeSec)
		{
			NoRespawnText = text;
			if (timeSec != m_lastTimeSec)
			{
				m_lastTimeSec = timeSec;
				int num = timeSec / 60;
				m_noRespawnHeader.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScreenMedicals_NoRespawnPlaceHeader), num, timeSec - num * 60);
				m_labelNoRespawn.Text = m_noRespawnHeader.ToString();
			}
		}

		private bool IsPlayerInFaction()
		{
			return MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId) != null;
		}

		private void respawnsTable_ItemMouseOver(MyGuiControlTable.Row row)
		{
			UpdateDetailedInfo(row);
		}

		private void UpdateDetailedInfo(MyGuiControlTable.Row row)
		{
			MyGuiControlParent descriptionControl = m_descriptionControl;
			MyGuiControlImage myGuiControlImage = (MyGuiControlImage)descriptionControl.Controls[1];
			MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)descriptionControl.Controls[0];
			MyGuiControlMultilineText myGuiControlMultilineText = (MyGuiControlMultilineText)descriptionControl.Controls[2];
			descriptionControl.Position = new Vector2(-0.33f, -0.3f);
			descriptionControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			string text = null;
			StringBuilder stringBuilder = myGuiControlLabel.TextToDraw.Clear();
			if (row != null && row.UserData != null)
			{
				string text2 = null;
				MyRespawnShipDefinition myRespawnShipDefinition;
				MyPlanetInfo myPlanetInfo;
				MySpaceRespawnComponent.MyRespawnPointInfo myRespawnPointInfo;
				if ((myRespawnShipDefinition = (row.UserData as MyRespawnShipDefinition)) != null)
				{
					if (!myRespawnShipDefinition.Icons.IsNullOrEmpty())
					{
						text = myRespawnShipDefinition.Icons[0];
					}
					text2 = myRespawnShipDefinition.HelpTextLocalizationId;
					MyStringId difficultyHard = MySpaceTexts.DifficultyHard;
					stringBuilder.Append(MyTexts.GetString(MySpaceTexts.Difficulty)).Append(": ");
					stringBuilder.Append(MyTexts.GetString(difficultyHard)).AppendLine();
				}
				else if ((myPlanetInfo = (row.UserData as MyPlanetInfo)) != null)
				{
					if (!string.IsNullOrEmpty(myPlanetInfo.DropPodForDetail))
					{
						MyRespawnShipDefinition respawnShipDefinition = MyDefinitionManager.Static.GetRespawnShipDefinition(myPlanetInfo.DropPodForDetail);
						if (respawnShipDefinition != null)
						{
							if (!respawnShipDefinition.Icons.IsNullOrEmpty())
							{
								text = respawnShipDefinition.Icons[0];
							}
							text2 = respawnShipDefinition.HelpTextLocalizationId;
							int num = 0;
							if ((double)myPlanetInfo.Gravity < 0.5)
							{
								num++;
							}
							if ((double)myPlanetInfo.OxygenLevel < 0.5)
							{
								num++;
							}
							MyStringId id = (num >= 2) ? MySpaceTexts.DifficultyHard : ((num < 1) ? MySpaceTexts.DifficultyEasy : MySpaceTexts.DifficultyNormal);
							stringBuilder.Append(MyTexts.GetString(MySpaceTexts.Difficulty)).Append(": ");
							stringBuilder.AppendLine(MyTexts.GetString(id)).AppendLine();
							BuildOxygenLevelInfo(stringBuilder, myPlanetInfo.OxygenLevel);
							stringBuilder.AppendLine();
							stringBuilder.Append(MyTexts.GetString(MySpaceTexts.HudInfoGravityNatural));
							stringBuilder.Append(' ').Append(myPlanetInfo.Gravity.ToString("F2")).AppendLine("g");
						}
					}
				}
				else if ((myRespawnPointInfo = (row.UserData as MySpaceRespawnComponent.MyRespawnPointInfo)) != null)
				{
					stringBuilder.Append(MyTexts.GetString(MySpaceTexts.ScreenMedicals_Owner));
					stringBuilder.Append(": ").Append((object)GetOwnerDisplayName(myRespawnPointInfo.OwnerId));
					stringBuilder.AppendLine();
					BuildOxygenLevelInfo(stringBuilder, myRespawnPointInfo.OxygenLevel);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					myGuiControlMultilineText.Text = new StringBuilder(MyTexts.GetString(text2));
					myGuiControlMultilineText.Visible = MySession.Static.Settings.EnableAutorespawn;
				}
				else
				{
					myGuiControlMultilineText.Visible = false;
				}
			}
			bool flag = stringBuilder.Length > 0;
			bool flag3 = myGuiControlImage.Visible = !string.IsNullOrEmpty(text);
			myGuiControlLabel.Visible = flag;
			if (!flag3 && !flag)
			{
				descriptionControl.Visible = false;
				return;
			}
			if (flag3 && (myGuiControlImage.Textures == null || myGuiControlImage.Textures.Length == 0 || myGuiControlImage.Textures[0].Texture != text))
			{
				using (MyUtils.ReuseCollection(ref m_preloadedTextures))
				{
					m_preloadedTextures.Add(text);
					MyRenderProxy.PreloadTextures(m_preloadedTextures, TextureType.GUIWithoutPremultiplyAlpha);
					myGuiControlImage.SetTexture(text);
				}
			}
			descriptionControl.Visible = true;
			myGuiControlLabel.Size = new Vector2(descriptionControl.Size.X, myGuiControlLabel.GetTextSize().Y * myGuiControlLabel.TextScale);
			float num2 = 0f;
			if (flag3)
			{
				num2 += myGuiControlImage.Size.Y;
			}
			if (flag)
			{
				num2 += myGuiControlLabel.Size.Y + 0.02f;
				if (!flag3)
				{
					num2 += 0.01f;
				}
			}
			if (myGuiControlMultilineText.Visible)
			{
				num2 += myGuiControlMultilineText.Size.Y + 0.02f;
			}
			descriptionControl.Size = new Vector2(descriptionControl.Size.X, num2);
			myGuiControlImage.PositionY = descriptionControl.Size.Y / 2f;
			myGuiControlLabel.PositionY = (0f - descriptionControl.Size.Y) / 2f + 0.01f;
			if (myGuiControlLabel.Visible)
			{
				myGuiControlMultilineText.PositionY = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + 0.04f;
			}
			else
			{
				myGuiControlMultilineText.PositionY = (0f - descriptionControl.Size.Y) / 2f + 0.04f;
			}
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			if (m_medbaySelect_SuppressNext)
			{
				m_medbaySelect_SuppressNext = false;
			}
			else
			{
				if (m_respawnsTable.SelectedRow == m_previouslySelected)
				{
					return;
				}
				base.FocusedControl = sender;
				m_previouslySelected = m_respawnsTable.SelectedRow;
				if (m_respawnsTable.SelectedRow != null)
				{
					m_respawnButton.Enabled = true;
					m_haveSelection = true;
					m_selectedRowData = m_respawnsTable.SelectedRow.UserData;
					MySpaceRespawnComponent.MyRespawnPointInfo myRespawnPointInfo = m_selectedRowData as MySpaceRespawnComponent.MyRespawnPointInfo;
					if (myRespawnPointInfo != null)
					{
						m_selectedRowIsStreamable = true;
						MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
						m_lastMedicalRoomId = 0L;
						m_isMultiplayerReady = false;
						ShowBlackground();
						MySession.RequestVicinityCache(myRespawnPointInfo.MedicalRoomGridId);
						if (!Sync.IsServer && MyEntities.EntityExists(myRespawnPointInfo.MedicalRoomGridId))
						{
							RequestConfirmation();
							return;
						}
						RequestReplicable(myRespawnPointInfo.MedicalRoomGridId);
						MyEntities.OnEntityAdd += OnEntityStreamedIn;
						return;
					}
					MyPlanetInfo myPlanetInfo = m_selectedRowData as MyPlanetInfo;
					if (myPlanetInfo != null)
					{
						m_selectedRowIsStreamable = true;
						Vector3 directionToSunNormalized = MySector.DirectionToSunNormalized;
						BoundingSphereD boundingSphereD = BoundingSphereD.CreateFromBoundingBox(myPlanetInfo.WorldAABB);
						boundingSphereD.IntersectRaySphere(new RayD(boundingSphereD.Center, directionToSunNormalized), out double _, out double tmax);
						Vector3D value = Vector3D.CalculatePerpendicularVector(directionToSunNormalized);
						Vector3D value2 = boundingSphereD.Center + (Vector3D)directionToSunNormalized * (tmax * 1.5);
						MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, value2);
						MySpectatorCameraController.Static.SetTarget(boundingSphereD.Center, value);
						m_isMultiplayerReady = true;
						return;
					}
					MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, new Vector3D(1000000.0));
				}
				else
				{
					m_haveSelection = false;
					m_selectedRowData = null;
					m_respawnButton.Enabled = false;
				}
				ShowBlackground();
				UnrequestReplicable();
				m_selectedRowIsStreamable = false;
			}
		}

		private void RequestReplicable(long replicableId)
		{
			if (m_requestedReplicable != replicableId)
			{
				UnrequestReplicable();
				m_requestedReplicable = replicableId;
				(MyMultiplayer.ReplicationLayer as MyReplicationClient)?.RequestReplicable(m_requestedReplicable, 0, add: true);
			}
		}

		private void UnrequestReplicable()
		{
			if (m_requestedReplicable != 0L)
			{
				MyReplicationClient myReplicationClient = MyMultiplayer.ReplicationLayer as MyReplicationClient;
				if (myReplicationClient != null)
				{
					myReplicationClient.RequestReplicable(m_requestedReplicable, 0, add: false);
					m_requestedReplicable = 0L;
				}
			}
		}

		private void OnTableItemDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			if (m_respawnsTable.SelectedRow == null)
			{
				return;
			}
			long? num = null;
			object userData = m_respawnsTable.SelectedRow.UserData;
			MySpaceRespawnComponent.MyRespawnPointInfo myRespawnPointInfo = userData as MySpaceRespawnComponent.MyRespawnPointInfo;
			if (myRespawnPointInfo != null)
			{
				num = myRespawnPointInfo.MedicalRoomId;
			}
			else
			{
				MyPlanetInfo myPlanetInfo = userData as MyPlanetInfo;
				if (myPlanetInfo != null)
				{
					num = myPlanetInfo.PlanetId;
				}
			}
			if (!num.HasValue || MyEntities.TryGetEntityById(num.Value, out MyEntity _))
			{
				onRespawnClick(m_respawnButton);
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionNotReady), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextNotReady), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
			}
		}

		private void OnFactionSelectClick(MyGuiControlButton sender)
		{
			if (m_factionsTable.SelectedRow == null)
			{
				return;
			}
			MyFaction myFaction = m_factionsTable.SelectedRow.UserData as MyFaction;
			if (m_applyingToFaction != null)
			{
				MyFactionCollection.CancelJoinRequest(m_applyingToFaction.FactionId, MySession.Static.LocalPlayerId);
			}
			m_applyingToFaction = myFaction;
			if (myFaction != null)
			{
				if (!myFaction.AcceptHumans || (!myFaction.AutoAcceptMember && !myFaction.IsAnyLeaderOnline))
				{
					return;
				}
				MyFactionCollection.SendJoinRequest(myFaction.FactionId, MySession.Static.LocalPlayerId);
			}
			m_showFactions = false;
			RecreateControls(constructor: true);
		}

		private void OnPlayerJoinedFaction(MyFaction faction, long identityId)
		{
			if (identityId == MySession.Static.LocalPlayerId)
			{
				m_showFactions = false;
				m_applyingToFaction = null;
				RecreateControls(constructor: true);
				if (!m_showFactions)
				{
					m_backToFactionsButton.Enabled = false;
				}
			}
		}

		private void OnPlayerKickedFromFaction(MyFaction faction, long identityId)
		{
			if (identityId == MySession.Static.LocalPlayerId)
			{
				m_showFactions = false;
				RecreateControls(constructor: true);
				if (!m_showFactions)
				{
					m_backToFactionsButton.Enabled = true;
				}
			}
		}

		private void OnPlayerRejected(MyFaction faction, long identityId)
		{
		}

		private void OnFactionsTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			RefreshSelectFactionButton();
		}

		private void RefreshSelectFactionButton()
		{
			if (m_factionsTable.SelectedRow == null)
			{
				m_selectFactionButton.Enabled = false;
				return;
			}
			MyFaction myFaction = m_factionsTable.SelectedRow.UserData as MyFaction;
			m_selectFactionButton.Enabled = (myFaction == null || (myFaction.AcceptHumans && (myFaction.AutoAcceptMember || myFaction.IsAnyLeaderOnline)));
		}

		private void OnFactionsTableItemDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			OnFactionSelectClick(null);
		}

		private void OnFactionsTableItemMouseOver(MyGuiControlTable.Row row)
		{
			m_factionsTable.SetToolTip(GetFactionTooltip(row.UserData as MyFaction));
		}

		private string GetFactionTooltip(MyFaction faction)
		{
			m_factionTooltip.Clear();
			if (faction != null)
			{
				bool flag = faction.Description != null && faction.Description != string.Empty;
				if (!faction.AcceptHumans)
				{
					m_factionTooltip.Append((object)MyTexts.Get(MySpaceTexts.ScreenMedicals_DoesNotAcceptPlayers));
					if (flag)
					{
						m_factionTooltip.Append("\n");
					}
				}
				else if (!faction.AutoAcceptMember)
				{
					m_factionTooltip.Append((object)MyTexts.Get(MySpaceTexts.ScreenMedicals_RequiresAcceptance));
					if (!faction.IsAnyLeaderOnline)
					{
						m_factionTooltip.Append("\n");
						m_factionTooltip.Append((object)MyTexts.Get(MySpaceTexts.ScreenMedicals_LeaderNotOnline));
					}
					if (flag)
					{
						m_factionTooltip.Append("\n");
					}
				}
				if (flag)
				{
					m_factionTooltip.Append(faction.Description);
				}
				if (faction.Members.Count > 0)
				{
					m_factionTooltip.Append("\n").Append("\n");
					m_factionTooltip.Append((object)MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Members));
					foreach (KeyValuePair<long, MyFactionMember> member in faction.Members)
					{
						MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(member.Key);
						if (myIdentity != null)
						{
							m_factionTooltip.Append("\n");
							m_factionTooltip.Append(myIdentity.DisplayName);
							if (member.Value.IsLeader)
							{
								m_factionTooltip.Append(" (").Append((object)MyTexts.Get(MyCommonTexts.Leader)).Append(")");
							}
						}
					}
				}
			}
			return m_factionTooltip.ToString();
		}

		private void OnShowPlayersClick(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
		}

		private void onRespawnClick(MyGuiControlButton sender)
		{
			if (m_respawnsTable.SelectedRow == null)
			{
				return;
			}
			object userData = m_respawnsTable.SelectedRow.UserData;
			MyRespawnShipDefinition respawnShip;
			MyPlanetInfo planetInfo;
			if (userData == null)
			{
				CheckPermaDeathAndRespawn(delegate
				{
					RespawnImmediately(null, null);
					if (!Sync.IsServer)
					{
						RequestConfirmation();
					}
				});
			}
			else if ((respawnShip = (userData as MyRespawnShipDefinition)) != null)
			{
				if (MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShip.Id.SubtypeName) == 0)
				{
					CheckPermaDeathAndRespawn(delegate
					{
						RespawnShip(respawnShip.Id.SubtypeName, null);
						if (!Sync.IsServer)
						{
							RequestConfirmation();
						}
					});
				}
			}
			else if ((planetInfo = (userData as MyPlanetInfo)) != null)
			{
				if (MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, planetInfo.RespawnShipForCooldownCheck) == 0)
				{
					CheckPermaDeathAndRespawn(delegate
					{
						RespawnShip(planetInfo.RespawnShipForCooldownCheck, planetInfo);
						ShowBlackground();
						if (!Sync.IsServer)
						{
							RequestConfirmation();
						}
						else
						{
							MySandboxGame.AreClipmapsReady = false;
						}
					});
				}
			}
			else if (m_restrictedRespawn == 0L || m_restrictedRespawn == ((MySpaceRespawnComponent.MyRespawnPointInfo)m_respawnsTable.SelectedRow.UserData).MedicalRoomId)
			{
				CheckPermaDeathAndRespawn(delegate
				{
					RespawnAtMedicalRoom(((MySpaceRespawnComponent.MyRespawnPointInfo)m_respawnsTable.SelectedRow.UserData).MedicalRoomId);
				});
			}
		}

		private void CheckPermaDeathAndRespawn(Action respawnAction)
		{
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
			if (myIdentity != null)
			{
				if (MySession.Static.Settings.PermanentDeath.Value && myIdentity.FirstSpawnDone)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: MyTexts.Get(MySpaceTexts.MessageBoxCaptionRespawn), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							respawnAction();
						}
					}));
				}
				else
				{
					respawnAction();
				}
			}
		}

		private void RespawnShip(string shipPrefabId, MyPlanetInfo planetInfo)
		{
			MySpaceRespawnComponent @static = MySpaceRespawnComponent.Static;
			int num = (MySession.Static.LocalHumanPlayer != null) ? @static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, shipPrefabId) : 0;
			if (@static.IsSynced && num == 0)
			{
				if (planetInfo == null)
				{
					RespawnImmediately(shipPrefabId, null);
				}
				else
				{
					RespawnImmediately(null, planetInfo.PlanetId);
				}
			}
			else
			{
				m_selectedRespawn = (((object)planetInfo) ?? ((object)MyDefinitionManager.Static.GetRespawnShipDefinition(shipPrefabId)));
				UpdateRespawnShipLabel();
			}
		}

		private void RespawnAtMedicalRoom(long medicalId)
		{
			string model = null;
			Color color = Color.Red;
			MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref color);
			MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, newIdentity: false, medicalId, null, 0, model, color);
			m_respawning = true;
			m_respawnButton.Visible = false;
			m_respawnsTable.Enabled = false;
		}

		private void RespawnImmediately(string shipPrefabId, long? planetId)
		{
			bool newIdentity = Sync.Players.TryGetIdentity(MySession.Static.LocalPlayerId)?.FirstSpawnDone ?? true;
			if (Sync.IsServer && (!string.IsNullOrEmpty(shipPrefabId) || planetId.HasValue))
			{
				m_waitingForRespawnShip = true;
				MySpaceRespawnComponent.Static.RespawnDoneEvent += RespawnShipDoneEvent;
				MyPlayerCollection.OnRespawnRequestFailureEvent += RespawnShipDoneEvent;
			}
			else
			{
				m_waitingForRespawnShip = false;
			}
			string model = null;
			Color color = Color.Red;
			MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref color);
			MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, newIdentity, planetId ?? 0, shipPrefabId, 0, model, color);
			m_respawning = true;
			m_respawnButton.Visible = false;
			m_respawnsTable.Enabled = false;
		}

		private void RespawnShipDoneEvent(ulong steamId)
		{
			m_waitingForRespawnShip = false;
			MySpaceRespawnComponent.Static.RespawnDoneEvent -= RespawnShipDoneEvent;
			MyPlayerCollection.OnRespawnRequestFailureEvent -= RespawnShipDoneEvent;
		}

		private void OnRefreshClick(MyGuiControlButton sender)
		{
			RefreshRespawnPoints();
		}

		private void OnFactionsRefreshClick(MyGuiControlButton sender)
		{
			RefreshFactions();
		}

		private void OnBackToFactionsClick(MyGuiControlButton sender)
		{
			m_showFactions = true;
			RecreateControls(constructor: true);
		}

		private void onMotdClick(MyGuiControlButton sender)
		{
			m_hideEmptyMotD = false;
			if (m_isMotdOpen)
			{
				m_showMotD = false;
			}
			else
			{
				m_showMotD = !Sandbox.Engine.Platform.Game.IsDedicated;
			}
			RecreateControls(constructor: false);
		}

		public void SetMotD(string motd)
		{
			m_lastMotD = motd;
			if (m_motdMultiline != null)
			{
				m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(m_lastMotD));
			}
			if (string.IsNullOrEmpty(m_lastMotD) || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_showMotD = false;
				m_MotdButton.Enabled = false;
			}
			else
			{
				m_MotdButton.Enabled = true;
			}
			if (m_showMotD)
			{
				RecreateControls(constructor: false);
			}
		}

		internal void HideMotdButton()
		{
			m_MotdButton.Visible = false;
		}

		public void ShowBlackground()
		{
			m_blackgroundCounter = 5;
			m_blackgroundDrawFull = true;
			m_blackgroundFade = 1f;
			m_streamingTimeout = 120;
		}

		internal static void ShowMotDUrl(string url)
		{
			if (MySession.ShowMotD)
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		private void AfterLocalizationLoaded()
		{
			if (m_motdMultiline != null)
			{
				m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(m_lastMotD));
			}
		}

		private void OnEntityStreamedIn(MyEntity entity)
		{
			if (entity.EntityId == m_requestedReplicable)
			{
				RequestConfirmation();
				MyEntities.OnEntityAdd -= OnEntityStreamedIn;
			}
		}

		private void RequestConfirmation()
		{
			m_isMultiplayerReady = false;
			(MyMultiplayer.Static as MyMultiplayerClientBase).RequestBatchConfirmation();
			MyMultiplayer.Static.PendingReplicablesDone += OnPendingReplicablesDone;
		}

		private void OnPendingReplicablesDone()
		{
			m_isMultiplayerReady = true;
			MyMultiplayer.Static.PendingReplicablesDone -= OnPendingReplicablesDone;
			if (MySession.Static.VoxelMaps.Instances.Count > 0)
			{
				MySandboxGame.AreClipmapsReady = false;
			}
		}
	}
}
