using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenHudSpace : MyGuiScreenHudBase
	{
		public static MyGuiScreenHudSpace Static;

		private const float ALTITUDE_CHANGE_THRESHOLD = 500f;

		public const int PING_THRESHOLD_MILLISECONDS = 250;

		public const float SERVER_SIMSPEED_THRESHOLD = 0.8f;

		private MyGuiControlToolbar m_toolbarControl;

		private MyGuiControlDPad m_DPadControl;

		private MyGuiControlContextHelp m_contextHelp;

		private MyGuiControlBlockInfo m_blockInfo;

		private MyGuiControlLabel m_rotatingWheelLabel;

		private MyGuiControlRotatingWheel m_rotatingWheelControl;

		private MyGuiControlMultilineText m_cameraInfoMultilineControl;

		private MyGuiControlQuestlog m_questlogControl;

		private MyGuiControlLabel m_buildModeLabel;

		private MyGuiControlLabel m_blocksLeft;

		private MyHudControlChat m_chatControl;

		private MyHudMarkerRender m_markerRender;

		private int m_oreHudMarkerStyle;

		private int m_gpsHudMarkerStyle;

		private int m_buttonPanelHudMarkerStyle;

		private MyHudEntityParams m_tmpHudEntityParams;

		private MyTuple<Vector3D, MyEntityOreDeposit>[] m_nearestOreDeposits;

		private float[] m_nearestDistanceSquared;

		private MyHudControlGravityIndicator m_gravityIndicator;

		private MyObjectBuilder_GuiTexture m_visorOverlayTexture;

		private readonly List<MyStatControls> m_statControls = new List<MyStatControls>();

		private bool m_hiddenToolbar;

		public float m_gravityHudWidth;

		private float m_altitude;

		private List<MyStringId> m_warningNotifications = new List<MyStringId>();

		private readonly byte m_warningFrameCount = 200;

		private byte m_currentFrameCount;

		public MyGuiScreenHudSpace()
		{
			Static = this;
			RecreateControls(constructor: true);
			m_markerRender = new MyHudMarkerRender(this);
			m_oreHudMarkerStyle = m_markerRender.AllocateMarkerStyle("White", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_neutral, Color.White);
			m_gpsHudMarkerStyle = m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
			m_buttonPanelHudMarkerStyle = m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
			m_tmpHudEntityParams = new MyHudEntityParams
			{
				Text = new StringBuilder(),
				FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL
			};
			m_contextHelp.BlockInfo = MyHud.BlockInfo;
		}

		public override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			InitHudStatControls();
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			m_gravityIndicator = new MyHudControlGravityIndicator(hudDefinition.GravityIndicator);
			if (hudDefinition.VisorOverlayTexture.HasValue)
			{
				m_visorOverlayTexture = MyGuiTextures.Static.GetTexture(hudDefinition.VisorOverlayTexture.Value);
			}
			m_toolbarControl = new MyGuiControlToolbar(hudDefinition.Toolbar);
			m_toolbarControl.Position = hudDefinition.Toolbar.CenterPosition;
			m_toolbarControl.OriginAlign = hudDefinition.Toolbar.OriginAlign;
			m_toolbarControl.IsActiveControl = false;
			Elements.Add(m_toolbarControl);
			MyObjectBuilder_DPadControlVisualStyle myObjectBuilder_DPadControlVisualStyle = null;
			myObjectBuilder_DPadControlVisualStyle = ((hudDefinition.DPad == null) ? MyObjectBuilder_DPadControlVisualStyle.DefaultStyle() : hudDefinition.DPad);
			m_DPadControl = new MyGuiControlDPad(myObjectBuilder_DPadControlVisualStyle);
			m_DPadControl.Position = myObjectBuilder_DPadControlVisualStyle.CenterPosition;
			m_DPadControl.OriginAlign = myObjectBuilder_DPadControlVisualStyle.OriginAlign;
			m_DPadControl.IsActiveControl = false;
			Elements.Add(m_DPadControl);
			m_textScale = 0.8f * MyGuiManager.LanguageTextScale;
			MyGuiControlBlockInfo.MyControlBlockInfoStyle myControlBlockInfoStyle = default(MyGuiControlBlockInfo.MyControlBlockInfoStyle);
			myControlBlockInfoStyle.BackgroundColormask = new Vector4(13f / 85f, 52f / 255f, 59f / 255f, 0.9f);
			myControlBlockInfoStyle.BlockNameLabelFont = "Blue";
			myControlBlockInfoStyle.EnableBlockTypeLabel = true;
			myControlBlockInfoStyle.ComponentsLabelText = MySpaceTexts.HudBlockInfo_Components;
			myControlBlockInfoStyle.ComponentsLabelFont = "Blue";
			myControlBlockInfoStyle.InstalledRequiredLabelText = MySpaceTexts.HudBlockInfo_Installed_Required;
			myControlBlockInfoStyle.InstalledRequiredLabelFont = "Blue";
			myControlBlockInfoStyle.RequiredLabelText = MyCommonTexts.HudBlockInfo_Required;
			myControlBlockInfoStyle.IntegrityLabelFont = "White";
			myControlBlockInfoStyle.IntegrityBackgroundColor = new Vector4(4f / 15f, 77f / 255f, 86f / 255f, 0.9f);
			myControlBlockInfoStyle.IntegrityForegroundColor = new Vector4(23f / 51f, 23f / 85f, 16f / 51f, 1f);
			myControlBlockInfoStyle.IntegrityForegroundColorOverCritical = new Vector4(122f / 255f, 28f / 51f, 154f / 255f, 1f);
			myControlBlockInfoStyle.LeftColumnBackgroundColor = new Vector4(46f / 255f, 76f / 255f, 94f / 255f, 1f);
			myControlBlockInfoStyle.TitleBackgroundColor = new Vector4(53f / 255f, 4f / 15f, 76f / 255f, 0.9f);
			myControlBlockInfoStyle.ComponentLineMissingFont = "Red";
			myControlBlockInfoStyle.ComponentLineAllMountedFont = "White";
			myControlBlockInfoStyle.ComponentLineAllInstalledFont = "Blue";
			myControlBlockInfoStyle.ComponentLineDefaultFont = "Blue";
			myControlBlockInfoStyle.ComponentLineDefaultColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);
			myControlBlockInfoStyle.ShowAvailableComponents = false;
			myControlBlockInfoStyle.EnableBlockTypePanel = false;
			MyGuiControlBlockInfo.MyControlBlockInfoStyle style = myControlBlockInfoStyle;
			m_contextHelp = new MyGuiControlContextHelp(style);
			m_contextHelp.IsActiveControl = false;
			Controls.Add(m_contextHelp);
			m_blockInfo = new MyGuiControlBlockInfo(style);
			m_blockInfo.IsActiveControl = false;
			MyGuiControlBlockInfo.ShowComponentProgress = true;
			MyGuiControlBlockInfo.CriticalIntegrityColor = new Color(115, 69, 80);
			MyGuiControlBlockInfo.OwnershipIntegrityColor = new Color(56, 67, 147);
			Controls.Add(m_blockInfo);
			m_questlogControl = new MyGuiControlQuestlog(new Vector2(20f, 20f));
			m_questlogControl.IsActiveControl = false;
			m_questlogControl.RecreateControls();
			Controls.Add(m_questlogControl);
			m_chatControl = new MyHudControlChat(MyHud.Chat, Vector2.Zero, new Vector2(0.339f, 0.28f), null, "White", 0.7f);
			Elements.Add(m_chatControl);
			m_cameraInfoMultilineControl = new MyGuiControlMultilineText(Vector2.Zero, new Vector2(0.4f, 0.25f), null, "White", 0.7f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, drawScrollbarV: false, drawScrollbarH: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			m_cameraInfoMultilineControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			Elements.Add(m_cameraInfoMultilineControl);
			m_rotatingWheelControl = new MyGuiControlRotatingWheel(new Vector2(0.5f, 0.8f));
			Controls.Add(m_rotatingWheelControl);
			Controls.Add(m_rotatingWheelLabel = new MyGuiControlLabel());
			Vector2 hudPos = new Vector2(0.5f, 0.02f);
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			m_buildModeLabel = new MyGuiControlLabel(hudPos, null, MyTexts.GetString(MyCommonTexts.Hud_BuildMode), null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Controls.Add(m_buildModeLabel);
			m_blocksLeft = new MyGuiControlLabel(new Vector2(0.238f, 0.89f), null, MyHud.BlocksLeft.GetStringBuilder().ToString(), null, 0.8f, "White", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			Controls.Add(m_blocksLeft);
			RegisterAlphaMultiplier(VisualStyleCategory.Background, MySandboxGame.Config.HUDBkOpacity);
			MyHud.ReloadTexts();
		}

		private void InitHudStatControls()
		{
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			m_statControls.Clear();
			if (hudDefinition.StatControls != null)
			{
				MyObjectBuilder_StatControls[] statControls = hudDefinition.StatControls;
				foreach (MyObjectBuilder_StatControls myObjectBuilder_StatControls in statControls)
				{
					float uiScale = myObjectBuilder_StatControls.ApplyHudScale ? (MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier) : MyGuiManager.GetSafeScreenScale();
					MyStatControls myStatControls = new MyStatControls(myObjectBuilder_StatControls, uiScale);
					Vector2 coordScreen = myObjectBuilder_StatControls.Position * MySandboxGame.ScreenSize;
					myStatControls.Position = MyUtils.AlignCoord(coordScreen, MySandboxGame.ScreenSize, myObjectBuilder_StatControls.OriginAlign);
					m_statControls.Add(myStatControls);
				}
			}
		}

		private void RefreshRotatingWheel()
		{
			m_rotatingWheelLabel.Visible = MyHud.RotatingWheelVisible;
			m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible;
			if (MyHud.RotatingWheelVisible && m_rotatingWheelLabel.TextToDraw != MyHud.RotatingWheelText)
			{
				m_rotatingWheelLabel.Position = m_rotatingWheelControl.Position + new Vector2(0f, 0.05f);
				m_rotatingWheelLabel.TextToDraw = MyHud.RotatingWheelText;
				Vector2 textSize = m_rotatingWheelLabel.GetTextSize();
				m_rotatingWheelLabel.PositionX -= textSize.X / 2f;
			}
		}

		public void RegisterAlphaMultiplier(VisualStyleCategory category, float multiplier)
		{
			m_statControls.ForEach(delegate(MyStatControls c)
			{
				c.RegisterAlphaMultiplier(category, multiplier);
			});
		}

		public override bool Draw()
		{
			if (m_transitionAlpha < 1f || !MyHud.IsVisible)
			{
				return false;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.J) && MyFakes.ENABLE_OBJECTIVE_LINE)
			{
				MyHud.ObjectiveLine.AdvanceObjective();
			}
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				foreach (MyStatControls statControl in m_statControls)
				{
					statControl.Draw(m_transitionAlpha, m_backgroundTransition);
				}
				m_gravityIndicator.Draw(m_transitionAlpha);
				DrawTexts();
			}
			if ((!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud) || MyPetaInputComponent.SHOW_HUD_ALWAYS)
			{
				if (MyHud.SinkGroupInfo.Visible && MyFakes.LEGACY_HUD)
				{
					DrawPowerGroupInfo(MyHud.SinkGroupInfo);
				}
				if (MyHud.LocationMarkers.Visible)
				{
					m_markerRender.DrawLocationMarkers(MyHud.LocationMarkers);
				}
				if (MyHud.GpsMarkers.Visible && MyFakes.ENABLE_GPS)
				{
					DrawGpsMarkers(MyHud.GpsMarkers);
				}
				if (MyHud.ButtonPanelMarkers.Visible)
				{
					DrawButtonPanelMarkers(MyHud.ButtonPanelMarkers);
				}
				if (MyHud.OreMarkers.Visible)
				{
					DrawOreMarkers(MyHud.OreMarkers);
				}
				if (MyHud.LargeTurretTargets.Visible)
				{
					DrawLargeTurretTargets(MyHud.LargeTurretTargets);
				}
				DrawWorldBorderIndicator(MyHud.WorldBorderChecker);
				if (MyHud.HackingMarkers.Visible)
				{
					DrawHackingMarkers(MyHud.HackingMarkers);
				}
				m_markerRender.Draw();
			}
			MyGuiControlToolbar toolbarControl = m_toolbarControl;
			bool visible = m_DPadControl.Visible = (!m_hiddenToolbar && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			toolbarControl.Visible = visible;
			Vector2 hudPos = new Vector2(0.99f, 0.985f);
			if (MySession.Static.ControlledEntity is MyShipController)
			{
				hudPos.Y = 0.65f;
			}
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			if (MyVideoSettingsManager.IsTripleHead())
			{
				hudPos.X += 1f;
			}
			bool flag2 = MyHud.BlockInfo.Components.Count > 0;
			IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
			m_contextHelp.Visible = (MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			if (stat.CurrentValue == 1f)
			{
				m_contextHelp.Visible &= !string.IsNullOrEmpty(MyHud.BlockInfo.ContextHelp);
			}
			if (stat.CurrentValue == 2f)
			{
				m_contextHelp.Visible &= flag2;
			}
			m_contextHelp.BlockInfo = (MyHud.BlockInfo.Visible ? MyHud.BlockInfo : null);
			if (!MyHud.ShipInfo.Visible)
			{
				m_contextHelp.Position = new Vector2(hudPos.X, 0.28f);
			}
			else
			{
				m_contextHelp.Position = new Vector2(hudPos.X, 0.1f);
			}
			if (stat.CurrentValue == 2f)
			{
				m_contextHelp.Position = new Vector2(hudPos.X, 0.38f);
				m_contextHelp.ShowJustTitle = true;
			}
			else
			{
				m_contextHelp.ShowJustTitle = false;
			}
			m_contextHelp.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_contextHelp.ShowBuildInfo = flag2;
			m_blockInfo.Visible = (MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud && flag2);
			m_blockInfo.BlockInfo = (m_blockInfo.Visible ? MyHud.BlockInfo : null);
			m_blockInfo.Position = m_contextHelp.Position + new Vector2(0f, m_contextHelp.Size.Y + 0.006f);
			m_blockInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_questlogControl.Visible = (MyHud.Questlog.Visible && !MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			m_rotatingWheelControl.Visible = (MyHud.RotatingWheelVisible && !MyHud.MinimalHud && !MyHud.CutsceneHud);
			m_rotatingWheelLabel.Visible = m_rotatingWheelControl.Visible;
			m_chatControl.Visible = (!(MyScreenManager.GetScreenWithFocus() is MyGuiScreenScenarioMpBase) && (!MyHud.MinimalHud || m_chatControl.HasFocus || MyHud.CutsceneHud));
			if (!base.Draw())
			{
				return false;
			}
			Vector2 hudPos2 = new Vector2(0.014f, 0.81f);
			hudPos2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
			m_chatControl.Position = hudPos2 + new Vector2(0.002f, -0.07f);
			m_chatControl.TextScale = 0.7f;
			hudPos2 = new Vector2(0.03f, 0.1f);
			hudPos2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
			m_cameraInfoMultilineControl.Position = hudPos2;
			m_cameraInfoMultilineControl.TextScale = 0.9f;
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				bool flag3 = false;
				MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
				if (myShipController != null)
				{
					flag3 = (MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(myShipController.PositionComp.GetPosition()) != 0f);
				}
				if (flag3)
				{
					DrawArtificialHorizonAndAltitude();
				}
			}
			if (!MyHud.IsHudMinimal)
			{
				MyHud.Notifications.Draw();
			}
			if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				m_buildModeLabel.Visible = MyHud.IsBuildMode;
				if (MyHud.BlocksLeft.Visible)
				{
					StringBuilder stringBuilder = MyHud.BlocksLeft.GetStringBuilder();
					if (!m_blocksLeft.Text.EqualsStrFast(stringBuilder))
					{
						m_blocksLeft.Text = stringBuilder.ToString();
					}
					m_blocksLeft.Visible = true;
				}
				else
				{
					m_blocksLeft.Visible = false;
				}
				if (MyHud.ObjectiveLine.Visible && MyFakes.ENABLE_OBJECTIVE_LINE)
				{
					DrawObjectiveLine(MyHud.ObjectiveLine);
				}
				if (MySandboxGame.Config.EnablePerformanceWarnings)
				{
					if (MySession.Static.IsSettingsExperimental() && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode);
					}
					if (MyUnsafeGridsSessionComponent.UnsafeGrids.Count > 0 && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_UnsafeGrids))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_UnsafeGrids);
					}
					foreach (KeyValuePair<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning> currentWarning in MySimpleProfiler.CurrentWarnings)
					{
						if (m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading))
						{
							break;
						}
						if (currentWarning.Value.Time < 120)
						{
							m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading);
							break;
						}
					}
					if ((MyGeneralStats.Static.LowNetworkQuality || !MySession.Static.MultiplayerDirect || (!MySession.Static.MultiplayerAlive && !MySession.Static.ServerSaving) || (!Sync.IsServer && MySession.Static.MultiplayerPing.Milliseconds > 250.0)) && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_Connection))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_Connection);
					}
					if (!MySession.Static.HighSimulationQualityNotification && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_LowSimulationQuality))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_LowSimulationQuality);
					}
					if (!Sync.IsServer && Sync.ServerSimulationRatio < 0.8f && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_SimSpeed))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_SimSpeed);
					}
					if (MySession.Static.ServerSaving && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_Saving))
					{
						m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_Saving);
					}
				}
			}
			else
			{
				m_buildModeLabel.Visible = false;
				m_blocksLeft.Visible = false;
			}
			if (MyFakes.PUBLIC_BETA_MP_TEST && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_ExperimentalBetaBuild))
			{
				m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_ExperimentalBetaBuild);
			}
			if (!MyGameService.IsOnline && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_SteamOffline))
			{
				m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_SteamOffline);
			}
			if (MySession.Static.GetComponent<MySessionComponentDLC>().UsedUnownedDLCs.Count > 0 && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_PaidContent))
			{
				m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_PaidContent);
			}
			if (MySandboxGame.Config.EnablePerformanceWarnings && MySession.Static.MultiplayerLastMsg > 1.0 && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_Connection))
			{
				m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_Connection);
			}
			if (MyPetaInputComponent.DRAW_WARNINGS && m_warningNotifications.Count != 0)
			{
				DrawPerformanceWarning();
			}
			MyHud.BlockInfo.Visible = false;
			m_blockInfo.BlockInfo = null;
			MyGuiScreenHudBase.HandleSelectedObjectHighlight(MyHud.SelectedObjectHighlight, new MyHudObjectHighlightStyleData
			{
				AtlasTexture = m_atlas,
				TextureCoord = GetTextureCoord(MyHudTexturesEnum.corner)
			});
			DrawCameraInfo(MyHud.CameraInfo);
			if (MyHud.VoiceChat.Visible)
			{
				DrawVoiceChat(MyHud.VoiceChat);
			}
			return true;
		}

		public override bool Update(bool hasFocus)
		{
			m_markerRender.Update();
			RefreshRotatingWheel();
			return base.Update(hasFocus);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenHudSpace";
		}

		private static Vector2 GetRealPositionOnCenterScreen(Vector2 value)
		{
			Vector2 result = (!MyGuiManager.FullscreenHudEnabled) ? MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(value) : MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(value);
			if (MyVideoSettingsManager.IsTripleHead())
			{
				result.X += 1f;
			}
			return result;
		}

		public void SetToolbarVisible(bool visible)
		{
			if (m_toolbarControl != null)
			{
				m_toolbarControl.Visible = visible;
				m_hiddenToolbar = !visible;
			}
		}

		private void DrawVoiceChat(MyHudVoiceChat voiceChat)
		{
			MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			MyGuiPaddedTexture tEXTURE_VOICE_CHAT = MyGuiConstants.TEXTURE_VOICE_CHAT;
			Vector2 hudPos = new Vector2(0.01f, 0.99f);
			Vector2 normalizedCoord = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			MyGuiManager.DrawSpriteBatch(tEXTURE_VOICE_CHAT.Texture, normalizedCoord, tEXTURE_VOICE_CHAT.SizeGui, Color.White, drawAlign);
		}

		private void DrawPowerGroupInfo(MyHudSinkGroupInfo info)
		{
			Rectangle safeFullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
			float num = -0.25f / ((float)safeFullscreenRectangle.Width / (float)safeFullscreenRectangle.Height);
			Vector2 hudPos = new Vector2(0.985f, 0.65f);
			Vector2 hudPos2 = new Vector2(hudPos.X + num, hudPos.Y);
			Vector2 valuesBottomRight = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			Vector2 namesBottomLeft = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
			info.Data.DrawBottomUp(namesBottomLeft, valuesBottomRight, m_textScale);
		}

		private float FindDistanceToNearestPlanetSeaLevel(BoundingBoxD worldBB, out MyPlanet closestPlanet)
		{
			closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldBB);
			double num = double.MaxValue;
			if (closestPlanet != null)
			{
				num = (worldBB.Center - closestPlanet.PositionComp.GetPosition()).Length() - (double)closestPlanet.AverageRadius;
			}
			return (float)num;
		}

		private void DrawArtificialHorizonAndAltitude()
		{
			MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
			if (myCubeBlock == null || myCubeBlock.CubeGrid.Physics == null)
			{
				return;
			}
			Vector3D globalPos = myCubeBlock.CubeGrid.Physics.CenterOfMassWorld;
			Vector3D centerOfMassWorld = myCubeBlock.GetTopMostParent().Physics.CenterOfMassWorld;
			MyShipController myShipController = myCubeBlock as MyShipController;
			if (myShipController != null && !myShipController.HorizonIndicatorEnabled)
			{
				return;
			}
			FindDistanceToNearestPlanetSeaLevel(myCubeBlock.PositionComp.WorldAABB, out MyPlanet closestPlanet);
			if (closestPlanet != null)
			{
				float num = (float)Vector3D.Distance(closestPlanet.GetClosestSurfacePointGlobal(ref globalPos), globalPos);
				string font = "Blue";
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				float num2 = num;
				if (Math.Abs(num2 - m_altitude) > 500f && myCubeBlock.CubeGrid.GridSystems.GasSystem != null)
				{
					myCubeBlock.CubeGrid.GridSystems.GasSystem.OnAltitudeChanged();
					m_altitude = num2;
				}
				StringBuilder text = new StringBuilder().AppendDecimal(num2, 0).Append(" m");
				float num3 = 0.03f;
				int num4 = MyGuiManager.GetFullscreenRectangle().Width / MyGuiManager.GetSafeFullscreenRectangle().Width;
				int num5 = MyGuiManager.GetFullscreenRectangle().Height / MyGuiManager.GetSafeFullscreenRectangle().Height;
				Vector2 normalizedCoord = new Vector2(MyHud.Crosshair.Position.X * (float)num4 / MyGuiManager.GetHudSize().X, MyHud.Crosshair.Position.Y * (float)num5 / MyGuiManager.GetHudSize().Y + num3);
				if (MyVideoSettingsManager.IsTripleHead())
				{
					normalizedCoord.X -= 1f;
				}
				MyGuiManager.DrawString(font, text, normalizedCoord, m_textScale, null, drawAlign, useFullClientArea: true);
				Vector3 v = -closestPlanet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(centerOfMassWorld);
				v.Normalize();
				double num6 = v.Dot(myCubeBlock.WorldMatrix.Forward);
				float scaleFactor = 0.4f;
				Vector2 vector = MyHud.Crosshair.Position / MyGuiManager.GetHudSize() * new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
				MyGuiPaddedTexture tEXTURE_HUD_GRAVITY_HORIZON = MyGuiConstants.TEXTURE_HUD_GRAVITY_HORIZON;
				float num7 = (MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator) ? (0.35f * MySector.MainCamera.Viewport.Height) : (0.45f * MySector.MainCamera.Viewport.Height);
				double num8 = num6 * (double)num7;
				Vector2D vector2D = new Vector2D(myCubeBlock.WorldMatrix.Right.Dot(v), myCubeBlock.WorldMatrix.Up.Dot(v));
				float num9 = (vector2D.LengthSquared() > 9.9999997473787516E-06) ? ((float)Math.Atan2(vector2D.Y, vector2D.X)) : 0f;
				Vector2 vector2 = tEXTURE_HUD_GRAVITY_HORIZON.SizePx * scaleFactor;
				RectangleF destination = new RectangleF(vector - vector2 * 0.5f + new Vector2(0f, (float)num8), vector2);
				Rectangle? sourceRectangle = null;
				Vector2 rightVector = new Vector2((float)Math.Sin(num9), (float)Math.Cos(num9));
				Vector2 origin = vector;
				MyRenderProxy.DrawSpriteExt(tEXTURE_HUD_GRAVITY_HORIZON.Texture, ref destination, sourceRectangle, Color.White, ref rightVector, ref origin);
			}
		}

		private void DrawObjectiveLine(MyHudObjectiveLine objective)
		{
			MyGuiDrawAlignEnum myGuiDrawAlignEnum = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			Color aliceBlue = Color.AliceBlue;
			Vector2 hudPos = new Vector2(0.45f, 0.01f);
			Vector2 vector = new Vector2(0f, 0.02f);
			Vector2 vector2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			StringBuilder text = new StringBuilder(objective.Title);
			Vector2 normalizedCoord = vector2;
			MyGuiDrawAlignEnum drawAlign = myGuiDrawAlignEnum;
			MyGuiManager.DrawString("Debug", text, normalizedCoord, 1f, aliceBlue, drawAlign);
			hudPos += vector;
			vector2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			MyGuiManager.DrawString("Debug", new StringBuilder("- " + objective.CurrentObjective), vector2, 1f, null, myGuiDrawAlignEnum);
		}

		private void DrawGpsMarkers(MyHudGpsMarkers gpsMarkers)
		{
			m_tmpHudEntityParams.FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL;
			MySession.Static.Gpss.updateForHud();
			foreach (MyGps markerEntity in gpsMarkers.MarkerEntities)
			{
				m_markerRender.AddGPS(markerEntity);
			}
		}

		private void DrawButtonPanelMarkers(MyHudGpsMarkers buttonPanelMarkers)
		{
			foreach (MyGps markerEntity in buttonPanelMarkers.MarkerEntities)
			{
				m_markerRender.AddButtonMarker(markerEntity.Coords, markerEntity.Name);
			}
		}

		private void DrawOreMarkers(MyHudOreMarkers oreMarkers)
		{
			if (m_nearestOreDeposits == null || m_nearestOreDeposits.Length < MyDefinitionManager.Static.VoxelMaterialCount)
			{
				m_nearestOreDeposits = new MyTuple<Vector3D, MyEntityOreDeposit>[MyDefinitionManager.Static.VoxelMaterialCount];
				m_nearestDistanceSquared = new float[m_nearestOreDeposits.Length];
			}
			for (int i = 0; i < m_nearestOreDeposits.Length; i++)
			{
				m_nearestOreDeposits[i] = default(MyTuple<Vector3D, MyEntityOreDeposit>);
				m_nearestDistanceSquared[i] = float.MaxValue;
			}
			Vector3D value = Vector3D.Zero;
			if (MySession.Static != null && MySession.Static.ControlledEntity != null)
			{
				value = (MySession.Static.ControlledEntity as MyEntity).WorldMatrix.Translation;
			}
			foreach (MyEntityOreDeposit oreMarker in oreMarkers)
			{
				for (int j = 0; j < oreMarker.Materials.Count; j++)
				{
					MyEntityOreDeposit.Data data = oreMarker.Materials[j];
					MyVoxelMaterialDefinition material = data.Material;
					data.ComputeWorldPosition(oreMarker.VoxelMap, out Vector3D oreWorldPosition);
					float num = (float)(value - oreWorldPosition).LengthSquared();
					float num2 = m_nearestDistanceSquared[material.Index];
					if (num < num2)
					{
						m_nearestOreDeposits[material.Index] = MyTuple.Create(oreWorldPosition, oreMarker);
						m_nearestDistanceSquared[material.Index] = num;
					}
				}
			}
			for (int k = 0; k < m_nearestOreDeposits.Length; k++)
			{
				MyTuple<Vector3D, MyEntityOreDeposit> myTuple = m_nearestOreDeposits[k];
				if (myTuple.Item2 != null && myTuple.Item2.VoxelMap != null && !myTuple.Item2.VoxelMap.Closed)
				{
					string minedOre = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)k).MinedOre;
					m_markerRender.AddOre(myTuple.Item1, MyTexts.GetString(MyStringId.GetOrCompute(minedOre)));
				}
			}
		}

		private void DrawCameraInfo(MyHudCameraInfo cameraInfo)
		{
			cameraInfo.Draw(m_cameraInfoMultilineControl);
		}

		private void DrawLargeTurretTargets(MyHudLargeTurretTargets largeTurretTargets)
		{
			foreach (KeyValuePair<MyEntity, MyHudEntityParams> target in largeTurretTargets.Targets)
			{
				MyHudEntityParams value = target.Value;
				if (value.ShouldDraw == null || value.ShouldDraw())
				{
					m_markerRender.AddTarget(target.Key.PositionComp.WorldAABB.Center);
				}
			}
		}

		private void DrawWorldBorderIndicator(MyHudWorldBorderChecker checker)
		{
			if (checker.WorldCenterHintVisible)
			{
				m_markerRender.AddPOI(Vector3D.Zero, MyHudWorldBorderChecker.HudEntityParams.Text, MyRelationsBetweenPlayerAndBlock.Enemies);
			}
		}

		private void DrawHackingMarkers(MyHudHackingMarkers hackingMarkers)
		{
			try
			{
				hackingMarkers.UpdateMarkers();
				if (MySandboxGame.TotalTimeInMilliseconds % 200 <= 100)
				{
					foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in hackingMarkers.MarkerEntities)
					{
						MyHudEntityParams value = markerEntity.Value;
						if (value.ShouldDraw == null || value.ShouldDraw())
						{
							m_markerRender.AddHacking(markerEntity.Value.Position, value.Text);
						}
					}
				}
			}
			finally
			{
			}
		}

		private void DrawPerformanceWarning()
		{
			Vector2 vector = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 4, 42);
			vector -= new Vector2(MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE.SizeGui.X / 1.5f, 0f);
			MyGuiPaddedTexture tEXTURE_HUD_BG_PERFORMANCE = MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE;
			MyGuiManager.DrawSpriteBatch(tEXTURE_HUD_BG_PERFORMANCE.Texture, vector, tEXTURE_HUD_BG_PERFORMANCE.SizeGui / 1.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("White", new StringBuilder(MyTexts.GetString(m_warningNotifications[0])), vector + new Vector2(0.09f, -0.011f), 0.7f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			StringBuilder stringBuilder = new StringBuilder();
			MyGuiManager.DrawString("White", stringBuilder.AppendFormat(MyCommonTexts.PerformanceWarningCombination, MyGuiSandbox.GetKeyName(MyControlsSpace.HELP_SCREEN)), vector + new Vector2(0.09f, 0.011f), 0.7f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			stringBuilder.Clear();
			MyGuiManager.DrawString("White", stringBuilder.AppendFormat("({0})", m_warningNotifications.Count), vector + new Vector2(0.177f, -0.023f), 0.55f, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			if (m_currentFrameCount < m_warningFrameCount)
			{
				m_currentFrameCount++;
				return;
			}
			m_currentFrameCount = 0;
			m_warningNotifications.RemoveAt(0);
		}

		protected override void OnHide()
		{
			base.OnHide();
			if (MyHud.VoiceChat.Visible)
			{
				MyHud.VoiceChat.Hide();
			}
		}
	}
}
