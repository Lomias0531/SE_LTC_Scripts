using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlDPad : MyGuiControlBase
	{
		private enum MyDPadVisualLayouts
		{
			Classic,
			ColorPicker
		}

		private static readonly string DEFAULT_SKIN = "Textures\\GUI\\Icons\\Skins\\Armor\\DefaultArmor.DDS";

		private MyObjectBuilder_DPadControlVisualStyle m_style;

		private MyStringId m_lastContext;

		private MyGuiControlImage m_upImage;

		private MyGuiControlImage m_leftImage;

		private MyGuiControlImage m_rightImage;

		private MyGuiControlImage m_downImage;

		private MyGuiControlImage m_upImageTop;

		private MyGuiControlImage m_leftImageTop;

		private MyGuiControlImage m_rightImageTop;

		private MyGuiControlImage m_downImageTop;

		private MyGuiControlImage m_arrows;

		private MyGuiControlLabel m_bottomHintLeft;

		private MyGuiControlLabel m_bottomHintRight;

		private Vector2 m_subiconOffset;

		private MyGuiControlImageRotatable m_upBackground;

		private MyGuiControlImageRotatable m_leftBackground;

		private MyGuiControlImageRotatable m_rightBackground;

		private MyGuiControlImageRotatable m_downBackground;

		private List<MyGuiControlImage> m_images;

		private List<MyGuiControlImageRotatable> m_backgrounds;

		private MyDPadVisualLayouts m_visuals;

		private MyGuiControlImageRotatable m_upBackgroundColor;

		private MyGuiControlImageRotatable m_leftBackgroundColor;

		private MyGuiControlImageRotatable m_rightBackgroundColor;

		private MyGuiControlImageRotatable m_downBackgroundColor;

		private MyGuiControlImage m_centerImageInner;

		private MyGuiControlImage m_centerImageOuter;

		private List<MyGuiControlImage> m_imagesColor;

		private List<MyGuiControlImageRotatable> m_backgroundsColor;

		private MyGuiControlLabel m_upLabel;

		private MyGuiControlLabel m_leftLabel;

		private MyGuiControlLabel m_rightLabel;

		private MyGuiControlLabel m_downLabel;

		private Func<string> m_upFunc;

		private Func<string> m_leftFunc;

		private Func<string> m_rightFunc;

		private Func<string> m_downFunc;

		private MyDefinitionBase m_handWeaponDefinition;

		private bool m_keepHandWeaponAmmoCount;

		private MyDPadVisualLayouts Visuals
		{
			get
			{
				return m_visuals;
			}
			set
			{
				m_visuals = value;
			}
		}

		public MyGuiControlDPad(MyObjectBuilder_DPadControlVisualStyle style)
		{
			m_style = style;
			if (m_style.VisibleCondition != null)
			{
				InitStatConditions(m_style.VisibleCondition);
			}
			m_images = new List<MyGuiControlImage>();
			m_backgrounds = new List<MyGuiControlImageRotatable>();
			m_imagesColor = new List<MyGuiControlImage>();
			m_backgroundsColor = new List<MyGuiControlImageRotatable>();
			m_backgrounds.Add(m_upBackground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_leftBackground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_rightBackground = new MyGuiControlImageRotatable());
			m_backgrounds.Add(m_downBackground = new MyGuiControlImageRotatable());
			Vector2 vector = new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_upBackground.Size = (m_leftBackground.Size = (m_rightBackground.Size = (m_downBackground.Size = vector)));
			m_upBackground.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
			m_leftBackground.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
			m_rightBackground.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
			m_downBackground.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector.png");
			m_leftBackground.Rotation = MathF.E * -449f / 777f;
			m_rightBackground.Rotation = MathF.E * 449f / 777f;
			m_downBackground.Rotation = MathF.PI;
			Vector2 vector5 = new Vector2(48f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_subiconOffset = new Vector2(vector5.X * 0.33f, vector5.Y * -0.33f);
			m_images.Add(m_upImage = new MyGuiControlImage(null, vector5));
			m_images.Add(m_leftImage = new MyGuiControlImage(null, vector5));
			m_images.Add(m_rightImage = new MyGuiControlImage(null, vector5));
			m_images.Add(m_downImage = new MyGuiControlImage(null, vector5));
			m_images.Add(m_upImageTop = new MyGuiControlImage(null, vector5 / 3f));
			m_images.Add(m_leftImageTop = new MyGuiControlImage(null, vector5 / 3f));
			m_images.Add(m_rightImageTop = new MyGuiControlImage(null, vector5 / 3f));
			m_images.Add(m_downImageTop = new MyGuiControlImage(null, vector5 / 3f));
			Vector2 value = new Vector2(200f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_arrows = new MyGuiControlImage(null, value);
			m_arrows.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialSector_arrows.png");
			m_bottomHintLeft = new MyGuiControlLabel(Vector2.Zero, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_bottomHintRight = new MyGuiControlLabel(Vector2.Zero, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			m_backgroundsColor.Add(m_upBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_downBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_leftBackgroundColor = new MyGuiControlImageRotatable());
			m_backgroundsColor.Add(m_rightBackgroundColor = new MyGuiControlImageRotatable());
			m_upBackgroundColor.Size = (m_leftBackgroundColor.Size = (m_rightBackgroundColor.Size = (m_downBackgroundColor.Size = vector)));
			m_leftBackgroundColor.Rotation = MathF.E * -449f / 777f;
			m_rightBackgroundColor.Rotation = MathF.E * 449f / 777f;
			m_downBackgroundColor.Rotation = MathF.PI;
			m_imagesColor.Add(m_centerImageInner = new MyGuiControlImage(null, vector));
			m_imagesColor.Add(m_centerImageOuter = new MyGuiControlImage(null, vector));
			m_upBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_downBackgroundColor.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_centerImageOuter.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\WhiteSquare.png", "Textures\\GUI\\Icons\\HUD 2017\\BCTMiddleCircle.dds");
			float textScale = 0.45f;
			m_upLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_leftLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_rightLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_downLabel = new MyGuiControlLabel(Vector2.Zero, null, "", null, textScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			MyCubeBuilder.Static.OnActivated += RefreshIcons;
			MyCubeBuilder.Static.OnDeactivated += RefreshIcons;
			MyCubeBuilder.Static.OnBlockVariantChanged += RefreshIcons;
			MyCubeBuilder.Static.OnSymmetrySetupModeChanged += RefreshIcons;
			MyCockpit.OnPilotAttached += RefreshIcons;
			MySession @static = MySession.Static;
			@static.OnLocalPlayerSkinOrColorChanged = (Action)Delegate.Combine(@static.OnLocalPlayerSkinOrColorChanged, new Action(RefreshIcons));
			MyCubeBuilder static2 = MyCubeBuilder.Static;
			static2.OnToolTypeChanged = (Action)Delegate.Combine(static2.OnToolTypeChanged, new Action(RefreshIcons));
			MySessionComponentVoxelHand.Static.OnEnabledChanged += RefreshIcons;
			MySessionComponentVoxelHand.Static.OnBrushChanged += RefreshIcons;
			MySession.Static.GetComponent<MyToolSwitcher>().ToolsRefreshed += RefreshIcons;
			MySession.Static.GetComponent<MyEmoteSwitcher>().OnActiveStateChanged += RefreshIcons;
			MySession.Static.GetComponent<MyEmoteSwitcher>().OnPageChanged += RefreshIcons;
			MyGuiControlRadialMenuBlock.OnSelectionConfirmed = (Action<MyGuiControlRadialMenuBlock>)Delegate.Combine(MyGuiControlRadialMenuBlock.OnSelectionConfirmed, new Action<MyGuiControlRadialMenuBlock>(RefreshIconsRadialBlock));
		}

		public void RefreshIcons()
		{
			CleanUp(full: true);
			if (MySession.Static.GetComponent<MyEmoteSwitcher>().IsActive)
			{
				RefreshEmoteIcons();
				return;
			}
			MyStringId lhs = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.PRESSED))
			{
				if (lhs == MySpaceBindingCreator.AX_ACTIONS)
				{
					RefreshShipToolbarIcons();
				}
				else if (lhs == MySpaceBindingCreator.AX_BUILD)
				{
					RefreshBuildingShortcutIcons();
				}
				else if (lhs == MySpaceBindingCreator.AX_VOXEL)
				{
					RefreshVoxelShortcutIcons();
				}
				else if (lhs == MySpaceBindingCreator.AX_COLOR_PICKER)
				{
					RefreshColorShortcutIcons();
				}
				else if (lhs == MySpaceBindingCreator.AX_CLIPBOARD)
				{
					RefreshClipboardShortcutIcons();
				}
				else
				{
					RefreshEmptyIcons();
				}
			}
			else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.PRESSED))
			{
				if ((MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty) == MySpaceBindingCreator.CX_SPACESHIP)
				{
					RefreshShipShortcutIcons();
				}
				else
				{
					RefreshCharacterShortcutIcons();
				}
			}
			else if (lhs == MySpaceBindingCreator.AX_TOOLS)
			{
				RefreshToolIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_ACTIONS)
			{
				RefreshShipToolbarIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_BUILD)
			{
				RefreshBuildingIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_SYMMETRY)
			{
				RefreshSymmetryIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_COLOR_PICKER)
			{
				RefreshColorIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_CLIPBOARD)
			{
				RefreshClipboardIcons();
			}
			else if (lhs == MySpaceBindingCreator.AX_VOXEL)
			{
				RefreshVoxelIcons();
			}
		}

		private void CleanUp(bool full)
		{
			m_upImage.ColorMask = Vector4.One;
			m_leftImage.ColorMask = Vector4.One;
			m_rightImage.ColorMask = Vector4.One;
			m_downImage.ColorMask = Vector4.One;
			m_upImageTop.SetTexture(string.Empty);
			m_leftImageTop.SetTexture(string.Empty);
			m_rightImageTop.SetTexture(string.Empty);
			m_downImageTop.SetTexture(string.Empty);
			m_upImageTop.ColorMask = Vector4.One;
			m_leftImageTop.ColorMask = Vector4.One;
			m_rightImageTop.ColorMask = Vector4.One;
			m_downImageTop.ColorMask = Vector4.One;
			m_bottomHintLeft.Text = string.Empty;
			m_bottomHintRight.Text = string.Empty;
			Visuals = MyDPadVisualLayouts.Classic;
			if (full)
			{
				m_upLabel.Text = string.Empty;
				m_leftLabel.Text = string.Empty;
				m_rightLabel.Text = string.Empty;
				m_downLabel.Text = string.Empty;
				m_upLabel.ColorMask = Vector4.One;
				m_leftLabel.ColorMask = Vector4.One;
				m_rightLabel.ColorMask = Vector4.One;
				m_downLabel.ColorMask = Vector4.One;
				m_upFunc = null;
				m_leftFunc = null;
				m_rightFunc = null;
				m_downFunc = null;
				m_handWeaponDefinition = null;
				m_keepHandWeaponAmmoCount = false;
			}
		}

		private void RefreshShipShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridPowerOnCenter.png");
		}

		private void RefreshCharacterShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchCamera.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\PlayerHelmetOn.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\LightCenter.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ColorPicker.png");
		}

		private void RefreshToolIcons()
		{
			string[] nextToolImage = GetNextToolImage(MyToolSwitcher.ToolType.Drill);
			m_upImage.SetTextures(nextToolImage);
			string[] nextToolImage2 = GetNextToolImage(MyToolSwitcher.ToolType.Welder);
			m_rightImage.SetTextures(nextToolImage2);
			string[] nextToolImage3 = GetNextToolImage(MyToolSwitcher.ToolType.Grinder);
			m_leftImage.SetTextures(nextToolImage3);
			string[] nextToolImage4 = GetNextToolImage(MyToolSwitcher.ToolType.Weapon);
			m_downImage.SetTextures(nextToolImage4);
			m_upLabel.Text = string.Empty;
			m_leftLabel.Text = string.Empty;
			m_rightLabel.Text = string.Empty;
			m_upFunc = null;
			m_leftFunc = null;
			m_rightFunc = null;
			m_keepHandWeaponAmmoCount = (m_handWeaponDefinition != null);
			m_handWeaponDefinition = GetWeaponDefinition();
			if (m_handWeaponDefinition != null)
			{
				m_downFunc = GetAmmoCount;
			}
		}

		private void RefreshIconsRadialBlock(MyGuiControlRadialMenuBlock menu)
		{
			RefreshIcons();
		}

		private string GetAmmoCount()
		{
			if (m_handWeaponDefinition == null)
			{
				return null;
			}
			bool flag = false;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && (localCharacter.FindWeaponItemByDefinition(m_handWeaponDefinition.Id).HasValue || !localCharacter.WeaponTakesBuilderFromInventory(m_handWeaponDefinition.Id)))
			{
				IMyGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
				if (currentWeapon != null)
				{
					flag = (MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == m_handWeaponDefinition.Id);
				}
				if (localCharacter.LeftHandItem != null)
				{
					flag |= (m_handWeaponDefinition == localCharacter.LeftHandItem.PhysicalItemDefinition);
				}
				if (flag && currentWeapon != null)
				{
					MyWeaponItemDefinition myWeaponItemDefinition = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) as MyWeaponItemDefinition;
					if (myWeaponItemDefinition != null && myWeaponItemDefinition.ShowAmmoCount)
					{
						return localCharacter.CurrentWeapon.GetAmmunitionAmount().ToString();
					}
				}
			}
			if (m_keepHandWeaponAmmoCount)
			{
				return null;
			}
			return "0";
		}

		private void RefreshBuildingShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ToggleSymmetry.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
		}

		private void RefreshBuildingIcons()
		{
			MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CurrentBlockDefinition;
			if (currentBlockDefinition != null)
			{
				m_upImage.SetTextures(currentBlockDefinition.Icons);
			}
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
		}

		private void RefreshSymmetryIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\CloseSymmetrySetup.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RemoveSymmetryPlane.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\PlaceSymmetryPlane.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SwitchSymmetryAxis.png");
		}

		private void RefreshEmptyIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture(string.Empty);
		}

		private void RefreshVoxelShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
		}

		private void RefreshVoxelIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RadialMenu.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleDown.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ScaleUp.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\SetupVoxelHand.png");
		}

		private void RefreshColorShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\ColorPicker.png");
			m_leftImage.SetTexture(string.Empty);
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture(string.Empty);
		}

		private void RefreshColorIcons()
		{
			GetColors(out Vector4 colPrev, out Vector4 colCur, out Vector4 colNext);
			GetSkins(out string skinPrev, out string skinCur, out string skinNext);
			m_upBackgroundColor.ColorMask = colNext;
			m_centerImageOuter.ColorMask = colCur;
			m_downBackgroundColor.ColorMask = colPrev;
			m_leftBackgroundColor.SetTexture(skinPrev, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_rightBackgroundColor.SetTexture(skinNext, "Textures\\GUI\\Icons\\HUD 2017\\BCTPeripheralCircle.dds");
			m_centerImageInner.SetTexture(skinCur, "Textures\\GUI\\Icons\\HUD 2017\\BCTCentralCircle.dds");
			Visuals = MyDPadVisualLayouts.ColorPicker;
		}

		private void RefreshClipboardShortcutIcons()
		{
			m_upImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveFurther.png");
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\Autorotate.png");
			m_rightImage.SetTexture(string.Empty);
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\MoveCloser.png");
		}

		private void RefreshClipboardIcons()
		{
			m_upImage.SetTexture(string.Empty);
			m_leftImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateCounterClockwise.png");
			m_rightImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotateClockwise.png");
			m_downImage.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\RotationPlane.png");
		}

		private void GetSkins(out string skinPrev, out string skinCur, out string skinNext)
		{
			skinPrev = (skinCur = (skinNext = string.Empty));
			List<MyAssetModifierDefinition> list = new List<MyAssetModifierDefinition>();
			List<MyGameInventoryItemDefinition> list2 = (from e in MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Armor)
				orderby e.Name
				select e).ToList();
			for (int i = 0; i < list2.Count; i++)
			{
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), list2[i].AssetModifierId));
				if (assetModifierDefinition != null)
				{
					list.Add(assetModifierDefinition);
				}
			}
			string buildArmorSkin = MySession.Static.LocalHumanPlayer.BuildArmorSkin;
			_ = string.Empty;
			int num = -1;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Id.SubtypeName == buildArmorSkin)
				{
					num = j;
					break;
				}
			}
			int num2 = list.Count + 1;
			int num3 = (num + num2) % num2 - 1;
			int num4 = (num + 2) % num2 - 1;
			skinPrev = ((num3 == -1) ? DEFAULT_SKIN : list[num3].Icons[0]);
			skinCur = ((num == -1) ? DEFAULT_SKIN : list[num].Icons[0]);
			skinNext = ((num4 == -1) ? DEFAULT_SKIN : list[num4].Icons[0]);
		}

		private void GetColors(out Vector4 colPrev, out Vector4 colCur, out Vector4 colNext)
		{
			MySession.Static.LocalHumanPlayer.GetColorPreviousCurrentNext(out Vector3 prev, out Vector3 cur, out Vector3 next);
			colPrev = MyColorPickerConstants.HSVOffsetToHSV(prev).HSVtoColor();
			colCur = MyColorPickerConstants.HSVOffsetToHSV(cur).HSVtoColor();
			colNext = MyColorPickerConstants.HSVOffsetToHSV(next).HSVtoColor();
		}

		private void RefreshShipToolbarIcons()
		{
			m_upImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(0));
			m_leftImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(1));
			m_rightImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(2));
			m_downImage.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(3));
			MyGuiControlLabel upLabel = m_upLabel;
			MyGuiControlImage upImageTop = m_upImageTop;
			Vector4 vector = m_upImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(0);
			Vector4 vector4 = upLabel.ColorMask = (upImageTop.ColorMask = vector);
			MyGuiControlLabel leftLabel = m_leftLabel;
			MyGuiControlImage leftImageTop = m_leftImageTop;
			vector = (m_leftImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(1));
			vector4 = (leftLabel.ColorMask = (leftImageTop.ColorMask = vector));
			MyGuiControlLabel rightLabel = m_rightLabel;
			MyGuiControlImage rightImageTop = m_rightImageTop;
			vector = (m_rightImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(2));
			vector4 = (rightLabel.ColorMask = (rightImageTop.ColorMask = vector));
			MyGuiControlLabel downLabel = m_downLabel;
			MyGuiControlImage downImageTop = m_downImageTop;
			vector = (m_downImage.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(3));
			vector4 = (downLabel.ColorMask = (downImageTop.ColorMask = vector));
			m_upImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(0));
			m_leftImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(1));
			m_rightImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(2));
			m_downImageTop.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(3));
			m_bottomHintLeft.Text = "\ue005+\ue001";
			m_bottomHintRight.Text = "\ue005+\ue003";
			Visuals = MyDPadVisualLayouts.Classic;
			m_upLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(0);
			m_leftLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(1);
			m_rightLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(2);
			m_downLabel.Text = MyToolbarComponent.CurrentToolbar.GetItemAction(3);
			m_upFunc = null;
			m_leftFunc = null;
			m_rightFunc = null;
			m_downFunc = null;
			m_handWeaponDefinition = null;
			m_keepHandWeaponAmmoCount = false;
		}

		private void RefreshEmoteIcons()
		{
			MyEmoteSwitcher component = MySession.Static.GetComponent<MyEmoteSwitcher>();
			if (component != null)
			{
				m_upImage.SetTexture(component.GetIconUp());
				m_leftImage.SetTexture(component.GetIconLeft());
				m_rightImage.SetTexture(component.GetIconRight());
				m_downImage.SetTexture(component.GetIconDown());
				m_upImageTop.SetTexture(string.Empty);
				m_leftImageTop.SetTexture(string.Empty);
				m_rightImageTop.SetTexture(string.Empty);
				m_downImageTop.SetTexture(string.Empty);
				m_bottomHintLeft.Text = '\ue002'.ToString();
				m_bottomHintRight.Text = '\ue003'.ToString();
				Visuals = MyDPadVisualLayouts.Classic;
				m_upLabel.Text = string.Empty;
				m_leftLabel.Text = string.Empty;
				m_rightLabel.Text = string.Empty;
				m_downLabel.Text = string.Empty;
				m_upFunc = null;
				m_leftFunc = null;
				m_rightFunc = null;
				m_downFunc = null;
				m_handWeaponDefinition = null;
				m_keepHandWeaponAmmoCount = false;
			}
		}

		private string[] GetNextToolImage(MyToolSwitcher.ToolType type)
		{
			MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(type);
			if (!currentOrNextTool.HasValue)
			{
				return null;
			}
			if (type != MyToolSwitcher.ToolType.Weapon)
			{
				return MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentOrNextTool.Value).Icons;
			}
			return MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value).Icons;
		}

		private MyDefinitionBase GetWeaponDefinition()
		{
			MyDefinitionId? currentOrNextTool = MySession.Static.GetComponent<MyToolSwitcher>().GetCurrentOrNextTool(MyToolSwitcher.ToolType.Weapon);
			if (!currentOrNextTool.HasValue)
			{
				return null;
			}
			return MyDefinitionManager.Static.GetPhysicalItemDefinition(currentOrNextTool.Value);
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			Vector2 positionAbsoluteCenter = GetPositionAbsoluteCenter();
			float scaleFactor = 0.65f;
			MyGuiControlImage arrows = m_arrows;
			MyGuiControlImageRotatable upBackground = m_upBackground;
			MyGuiControlImageRotatable leftBackground = m_leftBackground;
			MyGuiControlImageRotatable rightBackground = m_rightBackground;
			MyGuiControlImageRotatable downBackground = m_downBackground;
			MyGuiControlImageRotatable upBackgroundColor = m_upBackgroundColor;
			MyGuiControlImageRotatable leftBackgroundColor = m_leftBackgroundColor;
			MyGuiControlImageRotatable rightBackgroundColor = m_rightBackgroundColor;
			MyGuiControlImageRotatable downBackgroundColor = m_downBackgroundColor;
			MyGuiControlImage centerImageInner = m_centerImageInner;
			Vector2 vector2 = m_centerImageOuter.Position = positionAbsoluteCenter;
			Vector2 vector4 = centerImageInner.Position = vector2;
			Vector2 vector6 = downBackgroundColor.Position = vector4;
			Vector2 vector8 = rightBackgroundColor.Position = vector6;
			Vector2 vector10 = leftBackgroundColor.Position = vector8;
			Vector2 vector12 = upBackgroundColor.Position = vector10;
			Vector2 vector14 = downBackground.Position = vector12;
			Vector2 vector16 = rightBackground.Position = vector14;
			Vector2 vector18 = leftBackground.Position = vector16;
			Vector2 vector21 = arrows.Position = (upBackground.Position = vector18);
			MyGuiControlImage upImageTop = m_upImageTop;
			vector21 = (m_upImage.Position = positionAbsoluteCenter + new Vector2(0f, -65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * scaleFactor);
			upImageTop.Position = vector21 + m_subiconOffset;
			MyGuiControlImage leftImageTop = m_leftImageTop;
			vector21 = (m_leftImage.Position = positionAbsoluteCenter + new Vector2(-65f, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * scaleFactor);
			leftImageTop.Position = vector21 + m_subiconOffset;
			MyGuiControlImage rightImageTop = m_rightImageTop;
			vector21 = (m_rightImage.Position = positionAbsoluteCenter + new Vector2(65f, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE * scaleFactor);
			rightImageTop.Position = vector21 + m_subiconOffset;
			MyGuiControlImage downImageTop = m_downImageTop;
			vector21 = (m_downImage.Position = positionAbsoluteCenter + new Vector2(0f, 65f) / MyGuiConstants.GUI_OPTIMAL_SIZE * scaleFactor);
			downImageTop.Position = vector21 + m_subiconOffset;
			float num = 80f;
			m_bottomHintLeft.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -0.001f) + new Vector2(0f - num, num) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_bottomHintRight.Position = positionAbsoluteCenter + new Vector2(-0.0035f, -0.001f) + new Vector2(num, num) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 value = new Vector2(25f) / MyGuiConstants.GUI_OPTIMAL_SIZE * new Vector2(0.3f, 1.3f);
			m_upLabel.Position = m_upImageTop.Position + value;
			m_leftLabel.Position = m_leftImageTop.Position + value;
			m_rightLabel.Position = m_rightImageTop.Position + value;
			m_downLabel.Position = m_downImageTop.Position + value;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (!base.Visible || (m_style.VisibleCondition != null && !m_style.VisibleCondition.Eval()))
			{
				return;
			}
			if (m_upFunc != null)
			{
				string text = m_upFunc();
				if (!string.IsNullOrEmpty(text))
				{
					m_upLabel.Text = text;
				}
			}
			if (m_leftFunc != null)
			{
				string text2 = m_leftFunc();
				if (!string.IsNullOrEmpty(text2))
				{
					m_upLabel.Text = text2;
				}
			}
			if (m_rightFunc != null)
			{
				string text3 = m_rightFunc();
				if (!string.IsNullOrEmpty(text3))
				{
					m_rightLabel.Text = text3;
				}
			}
			if (m_downFunc != null)
			{
				string text4 = m_downFunc();
				if (!string.IsNullOrEmpty(text4))
				{
					m_downLabel.Text = text4;
				}
			}
			switch (m_visuals)
			{
			case MyDPadVisualLayouts.Classic:
				foreach (MyGuiControlImageRotatable background in m_backgrounds)
				{
					background.Draw(transitionAlpha * MySandboxGame.Config.HUDBkOpacity, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
				}
				foreach (MyGuiControlImage image in m_images)
				{
					image.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				break;
			case MyDPadVisualLayouts.ColorPicker:
				foreach (MyGuiControlImageRotatable item in m_backgroundsColor)
				{
					item.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				foreach (MyGuiControlImage item2 in m_imagesColor)
				{
					item2.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
				}
				break;
			}
			m_arrows.Draw(transitionAlpha * MySandboxGame.Config.UIOpacity, backgroundTransitionAlpha);
			m_bottomHintLeft.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_bottomHintRight.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_upLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_leftLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_rightLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_downLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		private void InitStatConditions(ConditionBase conditionBase)
		{
			ConditionBase[] terms = (conditionBase as Condition).Terms;
			for (int i = 0; i < terms.Length; i++)
			{
				StatCondition statCondition = terms[i] as StatCondition;
				if (statCondition != null)
				{
					IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
					statCondition.SetStat(stat);
				}
			}
		}

		public override void Update()
		{
			MyStringId myStringId = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (myStringId != m_lastContext)
			{
				RefreshIcons();
				m_lastContext = myStringId;
			}
			else if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyControlStateType.NEW_RELEASED))
			{
				RefreshIcons();
			}
			base.Update();
		}
	}
}
