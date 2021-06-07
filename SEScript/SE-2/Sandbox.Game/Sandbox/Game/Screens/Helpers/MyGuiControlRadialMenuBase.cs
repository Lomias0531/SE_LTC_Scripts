using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyGuiControlRadialMenuBase : MyGuiScreenBase
	{
		protected static readonly TimeSpan MOVEMENT_SUPPRESSION = TimeSpan.FromSeconds(2.0);

		protected static readonly float RADIUS = 0.11f;

		protected static readonly Dictionary<MyDefinitionId, int> m_lastSelectedSection = new Dictionary<MyDefinitionId, int>();

		private const int ITEMS_COUNT = 8;

		protected List<MyGuiControlImageRotatable> m_buttons;

		private List<MyGuiControlImage> m_tabs;

		private List<MyGuiControlLabel> m_tabLabels;

		protected List<MyGuiControlImage> m_icons;

		protected MyRadialMenu m_data;

		protected MyGuiControlLabel m_tooltip;

		protected MyGuiControlImage m_cancelButton;

		protected int m_selectedButton = -1;

		private MyGuiControlLabel m_leftButtonHint;

		private MyGuiControlLabel m_rightButtonHint;

		private readonly Func<bool> m_handleInputCallback;

		private readonly MyStringId m_closingControl;

		protected int m_currentSection;

		protected const float HINTS_POS_Y = 0.365f;

		private float CATEGORY_POS_Y = -0.5f + MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.Y / 2f + 0.08f;

		private MyGuiControlImageRotatable m_buttonHighlight;

		public int CurrentTabIndex => m_currentSection;

		protected MyGuiControlRadialMenuBase(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
			: base(null, null, null, isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_isTopMostScreen = true;
			base.DrawMouseCursor = false;
			m_closeOnEsc = true;
			m_closingControl = closingControl;
			m_handleInputCallback = handleInputCallback;
			m_buttons = new List<MyGuiControlImageRotatable>();
			m_tabs = new List<MyGuiControlImage>();
			m_tabLabels = new List<MyGuiControlLabel>();
			m_icons = new List<MyGuiControlImage>();
			m_data = data;
			m_tooltip = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
				Font = "Blue",
				UseTextShadow = true,
				Visible = false
			};
			m_tooltip.TextScale *= 1.2f;
			AddControl(new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(-0.19f, 0.365f), size: MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui, backgroundTexture: null, textures: new string[1]
			{
				"Textures\\GUI\\Controls\\button_default_outlineless.dds"
			}));
			AddControl(new MyGuiControlLabel(new Vector2(-0.19f, 0.365f), null, string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintClose), MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL).ToString()), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			AddControl(new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(0.19f, 0.365f), size: MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui, backgroundTexture: null, textures: new string[1]
			{
				"Textures\\GUI\\Controls\\button_default_outlineless.dds"
			}));
			AddControl(new MyGuiControlLabel(new Vector2(0.19f, 0.365f), null, string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintConfirm), MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT).ToString()), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
			myGuiControlImage.SetTexture("Textures\\GUI\\Controls\\RadialMenuBackground.dds");
			myGuiControlImage.Size = new Vector2(884f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage);
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage();
			myGuiControlImage2.SetTexture("Textures\\GUI\\Controls\\RadialOuterCircle.dds");
			myGuiControlImage2.Size = new Vector2(632f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage2);
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
			myGuiControlImage3.SetTexture("Textures\\GUI\\Controls\\RadialBrackets.dds");
			myGuiControlImage3.Size = new Vector2(674f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			AddControl(myGuiControlImage3);
			foreach (MyRadialMenuSection section in m_data.Sections)
			{
				MyGuiControlImage myGuiControlImage4 = new MyGuiControlImage(null, null, new Vector4(1f, 1f, 1f, 0.8f));
				myGuiControlImage4.SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
				m_tabs.Add(myGuiControlImage4);
				AddControl(myGuiControlImage4);
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER
				};
				myGuiControlLabel.Text = MyTexts.GetString(section.Label);
				m_tabLabels.Add(myGuiControlLabel);
				AddControl(myGuiControlLabel);
			}
			for (int i = 0; i < 8; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = new MyGuiControlImageRotatable();
				myGuiControlImageRotatable.SetTexture("Textures\\GUI\\Controls\\RadialSectorUnSelected.dds");
				float num2 = myGuiControlImageRotatable.Rotation = MathF.PI / 4f * (float)i;
				myGuiControlImageRotatable.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlImageRotatable.Position = new Vector2((float)Math.Cos(num2 - MathF.E * 449f / 777f), (float)Math.Sin(num2 - MathF.E * 449f / 777f)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
				m_buttons.Add(myGuiControlImageRotatable);
				AddControl(myGuiControlImageRotatable);
			}
			GenerateIcons(8);
			m_buttonHighlight = new MyGuiControlImageRotatable();
			m_buttonHighlight.SetTexture("Textures\\GUI\\Controls\\RadialSectorSelected.dds");
			m_buttonHighlight.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonHighlight.Visible = false;
			AddControl(m_buttonHighlight);
			AddControl(m_tooltip);
			m_cancelButton = new MyGuiControlImage();
			AddControl(m_cancelButton);
			m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
			m_cancelButton.Size = new Vector2(126f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlImage myGuiControlImage5 = new MyGuiControlImage();
			AddControl(myGuiControlImage5);
			myGuiControlImage5.SetTexture("Textures\\GUI\\Icons\\HideWeapon.dds");
			myGuiControlImage5.Size = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float cATEGORY_POS_Y = CATEGORY_POS_Y;
			AddControl(m_leftButtonHint = new MyGuiControlLabel(null, null, MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT).ToString(), null, 1f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER)
			{
				Position = new Vector2(-0.045f - MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2f, cATEGORY_POS_Y)
			});
			AddControl(m_rightButtonHint = new MyGuiControlLabel(null, null, MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT).ToString(), null, 1f)
			{
				Position = new Vector2(0.04f + MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui.X / 2f, cATEGORY_POS_Y)
			});
			UpdateHighlight(-1, -1);
			MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
		}

		protected virtual void GenerateIcons(int maxSize)
		{
			for (int i = 0; i < maxSize; i++)
			{
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
				myGuiControlImage.Size = new Vector2(65f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				m_icons.Add(myGuiControlImage);
				AddControl(myGuiControlImage);
				float num = MathF.PI * 2f / (float)maxSize * (float)i - MathF.E * 449f / 777f;
				myGuiControlImage.Position = new Vector2(1f, 1.33333337f) * new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * RADIUS;
			}
		}

		protected bool Cancel()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return false;
			}
			localCharacter.UnequipWeapon();
			MySessionComponentVoxelHand.Static.Enabled = false;
			CloseScreen();
			return true;
		}

		protected override void OnClosed()
		{
			MyToolSwitcher component = MySession.Static.GetComponent<MyToolSwitcher>();
			component.SwitchingEnabled = false;
			component.ToolSwitched -= CloseScreenNow;
			MyGuiScreenGamePlay.Static.SuppressMovement = MySession.Static.ElapsedGameTime + MOVEMENT_SUPPRESSION;
			base.OnClosed();
		}

		protected bool ButtonAction(int section, int itemIndex)
		{
			List<MyRadialMenuItem> items = m_data.Sections[section].Items;
			if (items.Count <= itemIndex)
			{
				return false;
			}
			MyRadialMenuItem myRadialMenuItem = items[itemIndex];
			if (!myRadialMenuItem.CanBeActivated)
			{
				return false;
			}
			CloseScreen();
			ActivateItem(myRadialMenuItem);
			return true;
		}

		protected virtual void ActivateItem(MyRadialMenuItem item)
		{
			item.Activate();
		}

		protected void SwitchSection(int index)
		{
			m_currentSection = index;
			m_lastSelectedSection[m_data.Id] = index;
			float cATEGORY_POS_Y = CATEGORY_POS_Y;
			MyRadialMenuSection iconTextures = m_data.Sections[index];
			for (int i = 0; i < m_tabs.Count; i++)
			{
				if (i == index)
				{
					Vector2 vector3 = m_tabs[i].Position = (m_tabLabels[i].Position = new Vector2(0f, cATEGORY_POS_Y));
					m_tabs[i].Size = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * 1.25f;
					m_tabs[i].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless_highlight.dds");
				}
				else
				{
					if (Math.Abs(i - index) == 1)
					{
						m_tabs[i].Position = new Vector2((float)(i - index) * 0.21f, cATEGORY_POS_Y);
						m_tabLabels[i].Position = new Vector2((float)(i - index) * 0.22f, cATEGORY_POS_Y);
						m_tabs[i].Size = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui;
					}
					else
					{
						m_tabs[i].Position = new Vector2((float)Math.Sign(i - index) * 0.285f + (float)(i - index) * 0.01f, cATEGORY_POS_Y);
						m_tabs[i].Size = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * new Vector2(0.04f, 1f);
					}
					m_tabs[i].SetTexture("Textures\\GUI\\Controls\\button_default_outlineless.dds");
				}
				m_tabLabels[i].Visible = (Math.Abs(i - index) <= 1);
			}
			SetIconTextures(iconTextures);
			m_tooltip.Visible = false;
			m_leftButtonHint.Visible = (index != 0);
			m_rightButtonHint.Visible = (index != m_data.Sections.Count - 1);
			UpdateTooltip();
		}

		protected virtual void SetIconTextures(MyRadialMenuSection selectedSection)
		{
			for (int i = 0; i < m_buttons.Count; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = m_buttons[i];
				MyGuiControlImage myGuiControlImage = m_icons[i];
				if (i < selectedSection.Items.Count)
				{
					bool visible = myGuiControlImage.Visible = true;
					myGuiControlImageRotatable.Visible = visible;
					MyRadialMenuItem myRadialMenuItem = selectedSection.Items[i];
					myGuiControlImage.SetTexture(myRadialMenuItem.Icon);
					myGuiControlImage.ColorMask = (myRadialMenuItem.Enabled() ? Color.White : Color.Gray);
				}
				else
				{
					myGuiControlImage.Visible = false;
				}
			}
		}

		public override bool Update(bool hasFocus)
		{
			Vector3 joystickPositionForGameplay = MyInput.Static.GetJoystickPositionForGameplay(RequestedJoystickAxis.NoZ);
			Vector3 joystickRotationForGameplay = MyInput.Static.GetJoystickRotationForGameplay(RequestedJoystickAxis.NoZ);
			Vector3 value = Vector3.IsZero(joystickPositionForGameplay) ? joystickRotationForGameplay : joystickPositionForGameplay;
			if (!Vector3.IsZero(value))
			{
				int num = (int)Math.Round((6.2831854820251465 + (1.570796012878418 + Math.Atan2(value.Y, value.X))) % 6.2831854820251465 / (double)(MathF.PI * 2f / (float)m_buttons.Count)) % m_buttons.Count;
				if (num != m_selectedButton)
				{
					UpdateHighlight(m_selectedButton, num);
					m_selectedButton = num;
					UpdateTooltip();
					MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
				}
			}
			else if (m_cancelButton != null && m_selectedButton != -1)
			{
				UpdateHighlight(m_selectedButton, -1);
				m_selectedButton = -1;
				UpdateTooltip();
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			return base.Update(hasFocus);
		}

		protected abstract void UpdateTooltip();

		protected virtual void UpdateHighlight(int oldIndex, int newIndex)
		{
			if (oldIndex == -1)
			{
				m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircle.dds");
			}
			if (newIndex == -1)
			{
				m_cancelButton.SetTexture("Textures\\GUI\\Controls\\RadialCentralCircleSelected.dds");
				m_buttonHighlight.Visible = false;
				return;
			}
			float num = MathF.PI / 4f * (float)newIndex;
			m_buttonHighlight.Rotation = num;
			m_buttonHighlight.Position = new Vector2((float)Math.Cos(num - MathF.E * 449f / 777f), (float)Math.Sin(num - MathF.E * 449f / 777f)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_buttonHighlight.Visible = true;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && m_currentSection > 0)
			{
				SwitchSection(m_currentSection - 1);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) && m_data.Sections.Count > m_currentSection + 1)
			{
				SwitchSection(m_currentSection + 1);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT))
			{
				if ((m_selectedButton == -1) ? Cancel() : ButtonAction(m_currentSection, m_selectedButton))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
				else
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				}
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, m_closingControl) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))
			{
				CloseScreen();
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			else if (m_handleInputCallback?.Invoke() ?? false)
			{
				CloseScreen();
			}
		}

		public override string GetFriendlyName()
		{
			return "RadialMenu";
		}
	}
}
