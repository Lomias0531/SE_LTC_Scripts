using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemSystem))]
	internal class MyRadialMenuItemSystem : MyRadialMenuItem
	{
		public enum SystemAction
		{
			ToggleLights,
			ToggleBroadcasting,
			TogglePower,
			ToggleConnectors,
			SwitchCamera,
			ToggleHud,
			BlueprintsScreen,
			AdminMenu,
			SpawnMenu,
			SymmetrySetup,
			ToggleSymmetry,
			ColorPicker,
			OpenInventory,
			ShowProgressionTree,
			ShowHelpScreen,
			ToggleVisor,
			ReloadSession,
			VoxelHand,
			ColorTool,
			ToggleDampeners,
			Respawn,
			ShowPlayers,
			Chat,
			Unequip,
			PlacementMode,
			ToggleAutoRotation,
			CreateBlueprint,
			ToggleMultiBlock,
			CopyGrid,
			CutGrid,
			PasteGrid,
			ViewMode,
			ToggleSignals
		}

		private SystemAction m_systemAction;

		public override string Label
		{
			get
			{
				if (m_systemAction == SystemAction.AdminMenu && !MySession.Static.IsAdminMenuEnabled)
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_AdminOnly);
				}
				if (m_systemAction == SystemAction.SpawnMenu && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
				}
				if (m_systemAction == SystemAction.ShowPlayers && !Sync.MultiplayerActive)
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_MultiplayerOnly);
				}
				if (m_systemAction == SystemAction.Respawn && !(MySession.Static.ControlledEntity is MyCharacter))
				{
					return base.Label + "\n" + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CharacterOnly);
				}
				return base.Label;
			}
			set
			{
				base.Label = value;
			}
		}

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenuItemSystem myObjectBuilder_RadialMenuItemSystem = (MyObjectBuilder_RadialMenuItemSystem)builder;
			m_systemAction = (SystemAction)myObjectBuilder_RadialMenuItemSystem.SystemAction;
		}

		public override void Activate(params object[] parameters)
		{
			if (Enabled())
			{
				ActivateSystemAction(m_systemAction);
			}
		}

		public override bool Enabled()
		{
			if (m_systemAction == SystemAction.AdminMenu && !MySession.Static.IsAdminMenuEnabled)
			{
				return false;
			}
			if ((m_systemAction == SystemAction.SpawnMenu || m_systemAction == SystemAction.CopyGrid || m_systemAction == SystemAction.CutGrid || m_systemAction == SystemAction.PasteGrid) && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
			{
				return false;
			}
			if (m_systemAction == SystemAction.ShowPlayers && !Sync.MultiplayerActive)
			{
				return false;
			}
			if (m_systemAction == SystemAction.Respawn && !(MySession.Static.ControlledEntity is MyCharacter))
			{
				return false;
			}
			return base.Enabled();
		}

		public static void ActivateSystemAction(SystemAction action)
		{
			switch (action)
			{
			case SystemAction.VoxelHand:
			case SystemAction.ToggleMultiBlock:
			case SystemAction.ViewMode:
				break;
			case SystemAction.ToggleLights:
				MySession.Static.ControlledEntity?.SwitchLights();
				break;
			case SystemAction.ToggleBroadcasting:
				MySession.Static.ControlledEntity?.SwitchBroadcasting();
				break;
			case SystemAction.TogglePower:
				MySession.Static.ControlledEntity?.SwitchReactors();
				break;
			case SystemAction.ToggleConnectors:
				MySession.Static.ControlledEntity?.SwitchLandingGears();
				break;
			case SystemAction.SwitchCamera:
				MyGuiScreenGamePlay.Static.SwitchCamera();
				break;
			case SystemAction.ToggleHud:
				MyHud.ToggleGamepadHud();
				break;
			case SystemAction.BlueprintsScreen:
				MyGuiSandbox.AddScreen(MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
				break;
			case SystemAction.AdminMenu:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
				break;
			case SystemAction.SpawnMenu:
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.VoxelMapEditingScreen));
				break;
			case SystemAction.SymmetrySetup:
				MyCubeBuilder.Static?.ToggleSymmetrySetup();
				break;
			case SystemAction.ToggleSymmetry:
				MyCubeBuilder.Static?.ToggleSymmetry();
				break;
			case SystemAction.ColorPicker:
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenColorPicker());
				break;
			case SystemAction.OpenInventory:
				MySession.Static.ControlledEntity?.ShowInventory();
				break;
			case SystemAction.ShowProgressionTree:
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, MySession.Static.ControlledEntity as MyShipController, "ResearchPage", true, null));
				break;
			case SystemAction.ShowHelpScreen:
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
				break;
			case SystemAction.ToggleVisor:
				MySession.Static.ControlledEntity?.SwitchHelmet();
				break;
			case SystemAction.ReloadSession:
				MyGuiScreenGamePlay.Static.RequestSessionReload();
				break;
			case SystemAction.ColorTool:
				MyCubeBuilder.Static.ActivateColorTool();
				break;
			case SystemAction.ToggleDampeners:
				MySession.Static.ControlledEntity?.SwitchDamping();
				break;
			case SystemAction.Respawn:
				MySession.Static.ControlledEntity?.Die();
				break;
			case SystemAction.ShowPlayers:
				if (Sync.MultiplayerActive)
				{
					MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
				}
				break;
			case SystemAction.Chat:
				if (MyGuiScreenChat.Static == null)
				{
					Vector2 hudPos = new Vector2(0.029f, 0.8f);
					hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
					MyGuiSandbox.AddScreen(new MyGuiScreenChat(hudPos));
				}
				break;
			case SystemAction.Unequip:
				(MySession.Static.ControlledEntity as MyCharacter)?.UnequipWeapon();
				break;
			case SystemAction.PlacementMode:
				MyClipboardComponent.Static.ChangeStationRotation();
				MyCubeBuilder.Static.CycleCubePlacementMode();
				break;
			case SystemAction.ToggleAutoRotation:
				MyCubeBuilder.Static.AlignToDefault = !MyCubeBuilder.Static.AlignToDefault;
				break;
			case SystemAction.CreateBlueprint:
				MyClipboardComponent.Static.CreateBlueprint();
				break;
			case SystemAction.CopyGrid:
				MyClipboardComponent.Static.Copy();
				break;
			case SystemAction.CutGrid:
				MyClipboardComponent.Static.Cut();
				break;
			case SystemAction.PasteGrid:
				MyClipboardComponent.Static.Paste();
				break;
			case SystemAction.ToggleSignals:
				MyHudMarkerRender.ChangeSignalMode();
				break;
			}
		}
	}
}
