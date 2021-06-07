using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.Animation;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenHelpSpace : MyGuiScreenBase
	{
		protected class MyHackyQuestLogComparer : IComparer<KeyValuePair<string, bool>>
		{
			int IComparer<KeyValuePair<string, bool>>.Compare(KeyValuePair<string, bool> x, KeyValuePair<string, bool> y)
			{
				return string.Compare(x.Key, y.Key);
			}
		}

		private enum HelpPageEnum
		{
			Tutorials,
			BasicControls,
			AdvancedControls,
			Controller,
			ControllerAdvanced,
			Chat,
			Support,
			IngameHelp,
			Welcome
		}

		private static readonly MyHackyQuestLogComparer m_hackyQuestComparer = new MyHackyQuestLogComparer();

		public MyGuiControlList contentList;

		private HelpPageEnum m_currentPage;

		public MyGuiScreenHelpSpace()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8436f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			m_currentPage = HelpPageEnum.Tutorials;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			int num = -1;
			for (int i = 0; i < Controls.Count; i++)
			{
				if (Controls[i].HasFocus)
				{
					num = i;
					_ = (Controls[i] as MyGuiControlTable)?.SelectedRowIndex;
				}
			}
			base.RecreateControls(constructor);
			AddCaption(MyTexts.GetString(MyCommonTexts.HelpScreenHeader), null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.87f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.847f), m_size.Value.X * 0.87f);
			Controls.Add(myGuiControlSeparatorList);
			StringBuilder output = new StringBuilder();
			MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).AppendBoundButtonNames(ref output, ",", MyInput.Static.GetUnassignedName(), includeSecondary: false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.HelpScreen_Description), output);
			StringBuilder contents = stringBuilder;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(-0.365f, 0.381f), new Vector2(0.4f, 0.2f), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents);
			myGuiControlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Controls.Add(myGuiControlMultilineText);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(0.281f, 0.415f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.ScreenMenuButtonBack), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close));
			myGuiControlButton.ButtonClicked += backButton_ButtonClicked;
			Controls.Add(myGuiControlButton);
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.365f, -0.39f), new Vector2(0.211f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = myGuiControlPanel.Position + new Vector2(0.01f, 0.005f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSelectCategory)
			};
			Controls.Add(myGuiControlPanel);
			Controls.Add(control);
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				Position = myGuiControlPanel.Position + new Vector2(0f, 0.033f),
				Size = new Vector2(0.211f, 0.5f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ColumnsCount = 1,
				VisibleRowsCount = 20,
				HeaderVisible = false
			};
			myGuiControlTable.SetCustomColumnWidths(new float[1]
			{
				1f
			});
			myGuiControlTable.ItemSelected += OnTableItemSelected;
			Controls.Add(myGuiControlTable);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Tutorials), HelpPageEnum.Tutorials);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), HelpPageEnum.BasicControls);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedControls), HelpPageEnum.AdvancedControls);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Gamepad), HelpPageEnum.Controller);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced), HelpPageEnum.ControllerAdvanced);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Chat), HelpPageEnum.Chat);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Support), HelpPageEnum.Support);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_IngameHelp), HelpPageEnum.IngameHelp);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Welcome), HelpPageEnum.Welcome);
			myGuiControlTable.SelectedRow = myGuiControlTable.GetRow((int)m_currentPage);
			contentList = new MyGuiControlList(myGuiControlPanel.Position + new Vector2(0.22f, 0f), new Vector2(0.511f, 0.74f));
			contentList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			contentList.VisualStyle = MyGuiControlListStyleEnum.Dark;
			Controls.Add(contentList);
			switch (m_currentPage)
			{
			case HelpPageEnum.Support:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkUserResponse), "https://support.keenswh.com/"));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkForum), "http://forums.keenswh.com/"));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContactDescription)));
				contentList.Controls.Add(AddLinkPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContact), "mailto:support@keenswh.com"));
				break;
			case HelpPageEnum.Tutorials:
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Intro.dds", "Intro", MySteamConstants.URL_TUTORIAL_PART1));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\BasicControls.dds", "Basic Controls", MySteamConstants.URL_TUTORIAL_PART2));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\GameModePossibilities.dds", "Possibilities Within The Game Modes", MySteamConstants.URL_TUTORIAL_PART3));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\DrillingRefiningAssembling.dds", "Drilling, Refining, & Assembling (Survival)", MySteamConstants.URL_TUTORIAL_PART4));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stShip.dds", "Building Your 1st Ship (Creative)", MySteamConstants.URL_TUTORIAL_PART5));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Survival.dds", "Survival", MySteamConstants.URL_TUTORIAL_PART10));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\ExperimentalMode.dds", "Experimental Mode", MySteamConstants.URL_TUTORIAL_PART6));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stVehicle.dds", "Building Your 1st Ground Vehicle (Creative)", MySteamConstants.URL_TUTORIAL_PART7));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamWorkshopBlueprints.dds", "Steam Workshop & Blueprints", MySteamConstants.URL_TUTORIAL_PART8));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\OtherAdvice.dds", "Other Advice & Closing Thoughts", MySteamConstants.URL_TUTORIAL_PART9));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamLink.dds", string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkSteam), MyGameService.Service.ServiceName), "http://steamcommunity.com/app/244850/guides"));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\WikiLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkWiki), "http://spaceengineerswiki.com/Main_Page"));
				contentList.Controls.Add(AddSeparatorPanel());
				break;
			case HelpPageEnum.BasicControls:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BasicDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeNavigation) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Navigation);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems1) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems1);
				contentList.Controls.Add(AddKeyPanel("CTRL + " + GetControlButtonName(MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.ControlName_RelativeDampening)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems2) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems2);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems3) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems3);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeToolsOrWeapons) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeView) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Spectator);
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			case HelpPageEnum.AdvancedControls:
			{
				StringBuilder output2 = null;
				MyInput.Static.GetGameControl(MyControlsSpace.CUBE_COLOR_CHANGE).AppendBoundButtonNames(ref output2, ", ", MyInput.Static.GetUnassignedName());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGeneral)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("F10", MyTexts.Get(MySpaceTexts.OpenBlueprints).ToString()));
				contentList.Controls.Add(AddKeyPanel("SHIFT + F10", MyTexts.Get(MySpaceTexts.OpenSpawnScreen).ToString()));
				contentList.Controls.Add(AddKeyPanel("ALT + F10", MyTexts.Get(MySpaceTexts.OpenAdminScreen).ToString()));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickLoad)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickSave)));
				contentList.Controls.Add(AddKeyPanel("CTRL + H", MyTexts.GetString(MySpaceTexts.ControlDescNetgraph)));
				contentList.Controls.Add(AddKeyPanel("F3", MyTexts.GetString(MyCommonTexts.ControlDescPlayersList)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGridsAndBlueprints)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + B", MyTexts.Get(MySpaceTexts.CreateManageBlueprints).ToString()));
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants)));
				contentList.Controls.Add(AddKeyPanel("Ctrl + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlDescCopyPasteMove)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + C", MyTexts.Get(MySpaceTexts.CopyObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + SHIFT + C", MyTexts.Get(MySpaceTexts.CopyObjectDetached).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + V", MyTexts.Get(MySpaceTexts.PasteObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + X", MyTexts.Get(MySpaceTexts.CutObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + Del", MyTexts.Get(MySpaceTexts.DeleteObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + ALT + E", MyTexts.GetString(MyCommonTexts.ControlDescExportModel)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedCamera)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("Alt + " + MyTexts.Get(MyCommonTexts.MouseWheel).ToString(), MyTexts.Get(MySpaceTexts.ControlDescZoom).ToString()));
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.SWITCH_LEFT), GetControlButtonDescription(MyControlsSpace.SWITCH_LEFT)));
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.SWITCH_RIGHT), GetControlButtonDescription(MyControlsSpace.SWITCH_RIGHT)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedColorPicker)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.LANDING_GEAR), GetControlButtonDescription(MyControlsSpace.LANDING_GEAR)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + P", MyTexts.GetString(MySpaceTexts.PickColorFromCube)));
				contentList.Controls.Add(AddKeyPanel(output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor)));
				contentList.Controls.Add(AddKeyPanel("CTRL + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush)));
				contentList.Controls.Add(AddKeyPanel("CTRL + SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedVoxelHands)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("H", MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings)));
				contentList.Controls.Add(AddKeyPanel("[", MyTexts.GetString(MyCommonTexts.ControlDescNextVoxelMaterial)));
				contentList.Controls.Add(AddKeyPanel("]", MyTexts.GetString(MyCommonTexts.ControlDescPreviousVoxelMaterial)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedSpectator)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + SPACE", MyTexts.GetString(MyCommonTexts.ControlDescMoveToSpectator)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MySpaceTexts.ControlDescSpectatorSpeed)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BuildPlanner)));
				contentList.Controls.Add(AddTinySpacePanel());
				StringBuilder output3 = null;
				MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER).AppendBoundButtonNames(ref output3, ", ", MyInput.Static.GetUnassignedName());
				contentList.Controls.Add(AddKeyPanel(output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw)));
				contentList.Controls.Add(AddKeyPanel("ALT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_WithdrawKeep)));
				contentList.Controls.Add(AddKeyPanel("CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw10Keep)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Put10ToProduction)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("ALT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll)));
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			}
			case HelpPageEnum.Controller:
				contentList.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerCharacterControl, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MySpaceTexts.HelpScreen_ControllerBuildMenu, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MySpaceTexts.HelpScreen_ControllerTools, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MyCommonTexts.ControlName_UpOrJump, MyCommonTexts.ControlName_DownOrCrouch, MyCommonTexts.ControlName_UseOrInteract, MySpaceTexts.ControlName_Jetpack, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation));
				contentList.Controls.Add(AddControllerSchema(MySpaceTexts.HelpScreen_ControllerShipControl, MySpaceTexts.HelpScreen_ControllerSecondaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.Inventory, MyStringId.NullOrEmpty, MySpaceTexts.HelpScreen_ControllerHorizontalMover, MySpaceTexts.HelpScreen_ControllerShipActions, MySpaceTexts.HelpScreen_ControllerPrimaryAction, MySpaceTexts.HelpScreen_ControllerModifier, MySpaceTexts.RadialMenuGroupTitle_Menu, MySpaceTexts.HelpScreen_ControllerFlyUp, MySpaceTexts.HelpScreen_ControllerFlyDown, MySpaceTexts.HelpScreen_ControllerLeaveControl, MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.HelpScreen_ControllerSystemMenu, MySpaceTexts.HelpScreen_ControllerRotation));
				break;
			case HelpPageEnum.ControllerAdvanced:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerShipControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.JUMP), MyTexts.GetString(MySpaceTexts.BlockActionTitle_Jump), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerTurretControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_ENERGY), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Powerkit), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterSurvival)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.TOGGLE_SIGNALS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ToggleSignals), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.PROGRESSION_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowProgressionTree), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterCreative)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.BuildPlanner)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.TERMINAL), MyTexts.GetString(MySpaceTexts.TerminalAccess), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.Spectator)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.TOOLBAR_RADIAL_MENU), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildMenu), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuilding)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.FREE_ROTATION), MyTexts.GetString(MySpaceTexts.StationRotation_Static), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE), MyTexts.GetString(MyCommonTexts.ControlName_CubeSizeMode), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT), MyTexts.GetString(MyCommonTexts.ControlName_CubeSizeMode), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingSurvival)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondaryBuildSurvival), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingCreative)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondayBuildCreative), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH), MyTexts.GetString(MySpaceTexts.ControlName_UseSymmetry), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlacing)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.Cancel), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCcw), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.ControlName_SymmetrySwitch)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.RadialMenuGroupTitle_VoxelHandBrushes)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.Remove), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_PAINT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPaint), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_REVERT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRevert), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleDown), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleUp), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT), MyTexts.GetString(MySpaceTexts.RadialMenu_Materials), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS), MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.PickColorFromCube), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadTips)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint1)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint2)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint3)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint4)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint5)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint6)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint7)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint8)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint9)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint10)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddSeparatorPanel());
				break;
			case HelpPageEnum.Chat:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ChatDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Name) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatColors_Name();
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Text) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatColors_Text();
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Controls) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatControls();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Commands) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatCommands();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Emotes) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddEmoteCommands();
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			case HelpPageEnum.IngameHelp:
				AddIngameHelpContent(contentList);
				break;
			case HelpPageEnum.Welcome:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ScreenCaptionWelcomeScreen)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text1)));
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text2)));
				contentList.Controls.Add(AddTextPanel(string.Format(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text3), MyGameService.Service.ServiceName)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddSignaturePanel());
				break;
			default:
				contentList.Controls.Add(AddTextPanel("Incorrect page selected"));
				break;
			}
			if (num != -1)
			{
				if (num >= Controls.Count)
				{
					num = Controls.Count - 1;
				}
				base.FocusedControl = Controls[num];
			}
			else
			{
				base.FocusedControl = myGuiControlTable;
			}
		}

		private void AddIngameHelpContent(MyGuiControlList contentList)
		{
			foreach (MyIngameHelpObjective item in MySessionComponentIngameHelp.GetFinishedObjectives().Reverse())
			{
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(item.TitleEnum)));
				contentList.Controls.Add(AddTinySpacePanel());
				MyIngameHelpDetail[] details = item.Details;
				foreach (MyIngameHelpDetail myIngameHelpDetail in details)
				{
					contentList.Controls.Add(AddTextPanel((myIngameHelpDetail.Args == null) ? MyTexts.GetString(myIngameHelpDetail.TextEnum) : string.Format(MyTexts.GetString(myIngameHelpDetail.TextEnum), myIngameHelpDetail.Args), 0.9f));
				}
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
			}
			MyDisgustingHackyQuestlogForLearningToSurviveAsWeAreRunningOutOfTime(contentList);
		}

		private void MyDisgustingHackyQuestlogForLearningToSurviveAsWeAreRunningOutOfTime(MyGuiControlList contentList)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				Regex nameRegex = new Regex("O_..x.._IsFinished");
				Regex nameRegex2 = new Regex("O_..x.._IsFailed");
				string str = "Caption";
				List<KeyValuePair<string, bool>> list = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex).ToList();
				List<KeyValuePair<string, bool>> list2 = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex2).ToList();
				list.Sort(m_hackyQuestComparer);
				list2.Sort(m_hackyQuestComparer);
				int num = -1;
				foreach (KeyValuePair<string, bool> item in list)
				{
					num++;
					if (item.Value)
					{
						string str2 = item.Key.Substring(0, 8);
						contentList.Controls.Add(AddKeyCategoryPanel(MyStatControlText.SubstituteTexts("{LOCC:" + MyTexts.GetString(str2 + str) + "}")));
						contentList.Controls.Add(AddTinySpacePanel());
						contentList.Controls.Add(AddTextPanel(MyStatControlText.SubstituteTexts("{LOCC:" + (list2[num].Value ? MyTexts.GetString("QuestlogDetail_Failed") : MyTexts.GetString("QuestlogDetail_Success")) + "}"), 0.9f));
						contentList.Controls.Add(AddTinySpacePanel());
						contentList.Controls.Add(AddSeparatorPanel());
						contentList.Controls.Add(AddTinySpacePanel());
					}
				}
			}
		}

		private MyGuiControlTable.Row AddHelpScreenCategory(MyGuiControlTable table, string rowName, HelpPageEnum pageEnum)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(pageEnum);
			StringBuilder stringBuilder = new StringBuilder(rowName);
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, null, stringBuilder.ToString(), Color.White));
			table.Add(row);
			return row;
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick)
		{
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			Vector4 bACK_BUTTON_BACKGROUND_COLOR = MyGuiConstants.BACK_BUTTON_BACKGROUND_COLOR;
			_ = MyGuiConstants.BACK_BUTTON_TEXT_COLOR;
			float textScale = 0.8f;
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, bACK_BUTTON_BACKGROUND_COLOR, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(text), textScale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		private void AddControlsByType(MyGuiControlTypeEnum type)
		{
			DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
			int num = 0;
			foreach (MyControl item in gameControlsList)
			{
				if (item.GetControlTypeEnum() == type)
				{
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
					}
					contentList.Controls.Add(AddKeyPanel(GetControlButtonName(item), GetControlButtonDescription(item)));
				}
			}
		}

		private void AddChatColors_Name()
		{
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Self), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Self), Color.CornflowerBlue));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Ally), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Ally), Color.LightGreen));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Neutral), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Neutral), Color.PaleGoldenrod));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Enemy), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Enemy), Color.Crimson));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Admin), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Admin), Color.Purple));
		}

		private void AddChatColors_Text()
		{
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Faction), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Faction), Color.LimeGreen));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Private), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Private), Color.Violet));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Global), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Global), Color.White));
		}

		private void AddChatControls()
		{
			contentList.Controls.Add(AddKeyPanel("PageUp", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageUp)));
			contentList.Controls.Add(AddKeyPanel("PageDown", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageDown)));
		}

		private void AddChatCommands()
		{
			if (MySession.Static == null)
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
				return;
			}
			contentList.Controls.Add(AddKeyPanel("/? <question>", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_Question)));
			int num = 1;
			foreach (KeyValuePair<string, IMyChatCommand> chatCommand in MySession.Static.ChatSystem.CommandSystem.ChatCommands)
			{
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.CommandText)), MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.HelpSimpleText))));
				num++;
				if (num % 5 == 0)
				{
					contentList.Controls.Add(AddTinySpacePanel());
				}
			}
		}

		private void AddEmoteCommands()
		{
			if (MySession.Static == null)
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
				return;
			}
			int num = 0;
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (!string.IsNullOrEmpty(animationDefinition.ChatCommandName))
				{
					contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommandDescription))));
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
					}
				}
			}
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem != null && inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote)
				{
					MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItem.ItemDefinition.AssetModifierId);
					if (definition != null && !string.IsNullOrWhiteSpace(definition.ChatCommandName))
					{
						contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommandDescription))));
						num++;
						if (num % 5 == 0)
						{
							contentList.Controls.Add(AddTinySpacePanel());
						}
					}
				}
			}
		}

		public string GetControlButtonName(MyStringId control)
		{
			MyControl gameControl = MyInput.Static.GetGameControl(control);
			StringBuilder output = new StringBuilder();
			gameControl.AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
			return output.ToString();
		}

		public string GetControlButtonName(MyControl control)
		{
			StringBuilder output = new StringBuilder();
			control.AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
			return output.ToString();
		}

		public string GetControlButtonDescription(MyStringId control)
		{
			return MyTexts.GetString(MyInput.Static.GetGameControl(control).GetControlName());
		}

		public string GetControlButtonDescription(MyControl control)
		{
			return MyTexts.GetString(control.GetControlName());
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenHelp";
		}

		private void OnCloseClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		private void backButton_ButtonClicked(MyGuiControlButton obj)
		{
			CloseScreen();
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (sender.SelectedRow != null)
			{
				m_currentPage = (HelpPageEnum)sender.SelectedRow.UserData;
				RecreateControls(constructor: false);
			}
		}

		private MyGuiControlParent AddTextPanel(string text, float textScaleMultiplier = 1f)
		{
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.4645f, 0.5f);
			myGuiControlMultilineText.TextScale *= textScaleMultiplier;
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.PositionX += 0.013f;
			myGuiControlMultilineText.Parse();
			return new MyGuiControlParent
			{
				Size = new Vector2(0.4645f, myGuiControlMultilineText.TextSize.Y + 0.01f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlMultilineText
				}
			};
		}

		private MyGuiControlParent AddSeparatorPanel()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.22f, 0f), 0.44f);
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.001f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlSeparatorList
				}
			};
		}

		private MyGuiControlParent AddImagePanel(string imagePath)
		{
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			myGuiControlParent.Size = new Vector2(0.437f, 0.35f);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = myGuiControlParent.Size,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.22f, 0.003f),
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
			{
				Size = myGuiControlImage.Size,
				OriginAlign = myGuiControlImage.OriginAlign,
				Position = myGuiControlImage.Position,
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage2.SetTexture(imagePath);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			myGuiControlParent.Controls.Add(myGuiControlImage2);
			return myGuiControlParent;
		}

		private MyGuiControlParent AddControllerSchema(MyStringId title, params MyStringId[] controls)
		{
			MyGuiControlParent panel = AddImagePanel("Textures\\GUI\\HelpScreen\\ControllerSchema.png");
			Span<float> span = stackalloc float[15]
			{
				122f,
				225f,
				330f,
				434f,
				538f,
				642f,
				122f,
				187f,
				252f,
				317f,
				382f,
				447f,
				512f,
				577f,
				642f
			};
			Span<char> span2 = stackalloc char[15]
			{
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZPOS", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J05", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J07", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J09", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_MOTION", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ZNEG", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J06", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J08", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J04", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J02", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J01", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J03", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("BUTTON_J10", null)[0],
				MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null)[0]
			};
			for (int i = 0; i < span.Length; i++)
			{
				bool num = i < 6;
				float y2 = span[i] / 762f;
				string text = (!(controls[i] == MyStringId.NullOrEmpty)) ? MyTexts.GetString(controls[i]) : "—";
				float x2;
				MyGuiDrawAlignEnum align2;
				if (num)
				{
					text = $"{text}  {span2[i]}";
					x2 = 83f / 264f;
					align2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				}
				else
				{
					text = $"{span2[i]}  {text}";
					x2 = 0.673606038f;
					align2 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				}
				Add(x2, y2, align2, new MyGuiControlLabel(null, null, text));
			}
			Add(0.5f, 0.05f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new MyGuiControlLabel
			{
				TextEnum = title,
				TextScale = 1.2f
			});
			return panel;
			void Add(float x, float y, MyGuiDrawAlignEnum align, MyGuiControlBase control)
			{
				control.OriginAlign = align;
				control.Position = new Vector2(base.Size.Value.Y, 0.66f) * contentList.Size * panel.Size * new Vector2(x * 2f - 1f, y * 2f - 1f);
				panel.Controls.Add(control);
			}
		}

		private MyGuiControlParent AddImageLinkPanel(string imagePath, string text, string url)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = new Vector2(0.137f, 0.108f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.22f, 0.003f),
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
			{
				Size = new Vector2(0.137f, 0.108f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.22f, 0.003f),
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage2.SetTexture(imagePath);
			myGuiControlImage2.SetTooltip(url);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.3f, 0.1f);
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.Position = new Vector2(0.08f, -0.005f);
			MyGuiControlButton myGuiControlButton = MakeButton(new Vector2(0.08f, 0f), MySpaceTexts.Blank, delegate
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
			});
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSteamOverlay), MyGameService.Service.ServiceName);
			myGuiControlButton.Alpha = 1f;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			myGuiControlButton.Size = new Vector2(0.22f, 0.13f);
			myGuiControlButton.TextScale = 0.736f;
			myGuiControlButton.CanHaveFocus = false;
			myGuiControlButton.PositionY += 0.05f;
			myGuiControlButton.PositionX += 0.113f;
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage
			{
				Size = new Vector2(0.0128f, 0.0176f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = myGuiControlButton.Position + new Vector2(0.01f, -0.01f),
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage3.SetTexture("Textures\\GUI\\link.dds");
			return new MyGuiControlParent
			{
				Size = new Vector2(0.4645f, 0.12f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlImage,
					(MyGuiControlBase)myGuiControlImage2,
					(MyGuiControlBase)myGuiControlMultilineText,
					(MyGuiControlBase)myGuiControlButton,
					(MyGuiControlBase)myGuiControlImage3
				}
			};
		}

		private MyGuiControlParent AddLinkPanel(string text, string url)
		{
			MyGuiControlButton myGuiControlButton = MakeButton(new Vector2(0.08f, 0f), MySpaceTexts.Blank, delegate
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.ExternalBrowser);
			});
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.Text = text;
			myGuiControlButton.Alpha = 1f;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			myGuiControlButton.Size = new Vector2(0.22f, 0.13f);
			myGuiControlButton.TextScale = 0.736f;
			myGuiControlButton.CanHaveFocus = false;
			myGuiControlButton.PositionY += 0.01f;
			myGuiControlButton.PositionX += 0.113f;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = new Vector2(0.0128f, 0.0176f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = myGuiControlButton.Position + new Vector2(0.01f, -0.01f),
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\link.dds");
			return new MyGuiControlParent
			{
				Size = new Vector2(0.4645f, 0.024f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlButton,
					(MyGuiControlBase)myGuiControlImage
				}
			};
		}

		private MyGuiControlParent AddKeyCategoryPanel(string text)
		{
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(null, null, null, "Textures\\GUI\\Controls\\item_highlight_dark.dds");
			myGuiControlPanel.Size = new Vector2(0.44f, 0.035f);
			myGuiControlPanel.BorderEnabled = true;
			myGuiControlPanel.BorderSize = 1;
			myGuiControlPanel.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.4645f, 0.5f);
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.PositionX += 0.02f;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, myGuiControlMultilineText.TextSize.Y + 0.01f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlPanel,
					(MyGuiControlBase)myGuiControlMultilineText
				}
			};
		}

		private MyGuiControlParent AddKeyPanel(string key, string description, Color? color = null)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
			myGuiControlLabel.Text = key;
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Font = (color.HasValue ? "White" : "Red");
			myGuiControlLabel.PositionX -= 0.2f;
			if (color.HasValue)
			{
				myGuiControlLabel.ColorMask = new Vector4((float)(int)color.Value.X / 256f, (float)(int)color.Value.Y / 256f, (float)(int)color.Value.Z / 256f, (float)(int)color.Value.A / 256f);
			}
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
			myGuiControlLabel2.Text = description;
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlLabel2.PositionX += 0.2f;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.013f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlLabel,
					(MyGuiControlBase)myGuiControlLabel2
				}
			};
		}

		private MyGuiControlParent AddTinySpacePanel()
		{
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.005f)
			};
		}

		private MyGuiControlParent AddSignaturePanel()
		{
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.08f, -0.04f), MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0.19f, -0.01f), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_SignatureTitle));
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(0.19f, 0.015f), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_Signature));
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.1f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlLabel,
					(MyGuiControlBase)myGuiControlLabel2,
					(MyGuiControlBase)myGuiControlPanel
				}
			};
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			float num = 0f - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP);
			num += MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN);
			contentList.GetScrollBar().Value = contentList.GetScrollBar().Value + num / 30f;
		}
	}
}
