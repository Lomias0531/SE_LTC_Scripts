using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Factions.Definitions;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenCreateOrEditFactionSpace : MyGuiScreenCreateOrEditFaction
	{
		private const string FALLBACK_DEFINITION_FACTIONICON = "Textures\\FactionLogo\\Empty.dds";

		public MyGuiScreenCreateOrEditFactionSpace(ref IMyFaction editData)
			: base(ref editData)
		{
			base.CloseButtonEnabled = true;
		}

		public MyGuiScreenCreateOrEditFactionSpace()
		{
			base.CloseButtonEnabled = true;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenCreateOrEditFactionSpace";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.TerminalTab_Factions_EditFaction, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			float num = -0.2285f;
			float num2 = -0.273f;
			float num3 = 0.07f;
			Vector2 value = new Vector2(0.29f, 0.052f);
			MyGuiControlLabel control = new MyGuiControlLabel(size: value, position: new Vector2(num, num2 + num3), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionTag), colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlLabel control2 = new MyGuiControlLabel(size: value, position: new Vector2(num, num2 + 2f * num3), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionName), colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlLabel control3 = new MyGuiControlLabel(size: value, position: new Vector2(num, num2 + 3f * num3), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionDescription), colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlLabel control4 = new MyGuiControlLabel(size: value, position: new Vector2(num, num2 + 4.8f * num3), text: MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionPrivateInfo), colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Controls.Add(control);
			Controls.Add(control2);
			Controls.Add(control3);
			Controls.Add(control4);
			num += 0.1165f;
			num2 += 0.065f;
			Vector2 size5 = new Vector2(0.2485f, 0.1f);
			m_shortcut = new MyGuiControlTextbox(new Vector2(num, num2), (m_editFaction != null) ? m_editFaction.Tag : "", 3);
			m_shortcut.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_shortcut.Enabled = (m_editFaction == null || (m_editFaction != null && !MySession.Static.Factions.IsNpcFaction(m_editFaction.Tag)));
			m_name = new MyGuiControlTextbox(new Vector2(num, num2 + num3), (m_editFaction != null) ? MyStatControlText.SubstituteTexts(m_editFaction.Name) : "", 64);
			m_name.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			float val = 0.0075f;
			m_desc = new MyGuiControlMultilineEditableText(new Vector2(num, num2 + 2f * num3));
			m_desc.TextWrap = true;
			m_desc.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_desc.Text = ((m_editFaction != null) ? new StringBuilder(m_editFaction.Description) : new StringBuilder(""));
			m_desc.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER;
			m_desc.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_desc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_desc.TextPadding = new MyGuiBorderThickness(val);
			m_desc.TextChanged += OnTextChanged;
			m_privInfo = new MyGuiControlMultilineEditableText(new Vector2(num, num2 + 3.8f * num3));
			m_privInfo.TextWrap = true;
			m_privInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_privInfo.Text = ((m_editFaction != null) ? new StringBuilder(m_editFaction.PrivateInfo) : new StringBuilder(""));
			m_privInfo.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER;
			m_privInfo.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_privInfo.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_privInfo.TextPadding = new MyGuiBorderThickness(val);
			m_privInfo.TextChanged += OnTextChanged;
			m_shortcut.Size = size5;
			m_name.Size = size5;
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			float num4 = (float)safeGuiRectangle.Height / (float)safeGuiRectangle.Width;
			float num5 = 0.03f;
			Vector2 vector = new Vector2(num5 * num4, num5);
			float num6 = 0.11f;
			float num7 = num6 * num4;
			float num8 = 0.01f;
			Vector2 size6 = new Vector2(size5.X + num7 + num8, 0.1f);
			m_desc.Size = size6;
			m_privInfo.Size = size6;
			m_factionIcon = new MyGuiControlImage
			{
				Position = m_shortcut.Position + new Vector2(m_shortcut.Size.X + num8, 0f),
				Size = new Vector2(num7, num6),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Padding = new MyGuiBorderThickness(1f)
			};
			if (m_editFaction != null)
			{
				string iconPath = m_editFaction.FactionIcon.HasValue ? m_editFaction.FactionIcon.Value.ToString() : string.Empty;
				GetFactionIconTextures(m_editFaction.CustomColor, m_editFaction.IconColor, iconPath, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction);
				m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
				{
					backgroundTexture,
					iconFaction
				});
				if (MyFactionCollection.GetDefinitionIdsByIconName(iconFaction.Texture, out SerializableDefinitionId? factionIconGroupId, out m_factionIconId))
				{
					m_factionIconGroupId = factionIconGroupId.Value;
				}
				else
				{
					MyFactionCollection.GetDefinitionIdsByIconName("Textures\\FactionLogo\\Empty.dds", out factionIconGroupId, out m_factionIconId);
					m_factionIconGroupId = factionIconGroupId.Value;
				}
				m_factionColor = m_editFaction.CustomColor;
				m_factionIconColor = m_editFaction.IconColor;
			}
			else
			{
				GetRandomIcon(out MyGuiControlImage.MyDrawTexture backgroundTexture2, out MyGuiControlImage.MyDrawTexture iconFaction2);
				m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
				{
					backgroundTexture2,
					iconFaction2
				});
				if (MyFactionCollection.GetDefinitionIdsByIconName(iconFaction2.Texture, out SerializableDefinitionId? factionIconGroupId2, out m_factionIconId))
				{
					m_factionIconGroupId = factionIconGroupId2.Value;
				}
				Color rgb = new Color(backgroundTexture2.ColorMask.Value);
				m_factionColor = MyColorPickerConstants.HSVToHSVOffset(rgb.ColorToHSV());
				m_factionIconColor = MyColorPickerConstants.HSVToHSVOffset(Color.White.ColorToHSV());
			}
			m_editFactionIconBtn = new MyGuiControlImageButton("", m_factionIcon.Position + new Vector2(m_factionIcon.Size.X, 0f), vector);
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Active = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
				},
				Disabled = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
				},
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER
				},
				Padding = new MyGuiBorderThickness(0.005f * num4, 0.005f)
			};
			m_editFactionIconBtn.ApplyStyle(style);
			m_editFactionIconBtn.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_editFactionIconBtn.Size = vector;
			MyGuiControlImageButton.ButtonIcon buttonIcon = default(MyGuiControlImageButton.ButtonIcon);
			buttonIcon.Normal = "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png";
			MyGuiControlImageButton.ButtonIcon icon = buttonIcon;
			m_editFactionIconBtn.Icon = icon;
			m_editFactionIconBtn.ButtonClicked += OnEditIconButtonPressed;
			m_shortcut.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionTagToolTip);
			m_privInfo.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionPrivateInfoToolTip);
			m_name.SetToolTip(MyCommonTexts.MessageBoxErrorFactionsNameTooShort);
			m_desc.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionPublicInfoToolTip);
			Controls.Add(m_shortcut);
			Controls.Add(m_name);
			Controls.Add(m_desc);
			Controls.Add(m_privInfo);
			Controls.Add(m_factionIcon);
			Controls.Add(m_editFactionIconBtn);
			num2 -= 0.003f;
			Vector2 value2 = new Vector2(0.002f, m_size.Value.Y / 2f - 0.041f);
			Vector2 value3 = new Vector2(0.229f, 0f);
			MyGuiControls controls = Controls;
			Vector2? vector2 = value;
			controls.Add(new MyGuiControlButton(value2 + value3, MyGuiControlButtonStyleEnum.Default, vector2, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOkClick, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok)));
		}

		private void OnTextChanged(MyGuiControlMultilineEditableText obj)
		{
			if (obj.GetTextLength() > 512)
			{
				obj.Text = new StringBuilder(obj.Text.ToString().Substring(0, 512));
			}
		}

		private void OnEditIconButtonPressed(MyGuiControlImageButton obj)
		{
			Vector3 hSV = MyColorPickerConstants.HSVOffsetToHSV(m_factionColor);
			Vector3 hSV2 = MyColorPickerConstants.HSVOffsetToHSV(m_factionIconColor);
			string factionIcon = MyFactionCollection.GetFactionIcon(m_factionIconGroupId, m_factionIconId);
			MyEditFactionIconViewModel myEditFactionIconViewModel = new MyEditFactionIconViewModel(m_factionIconGroupId, m_factionIconId, factionIcon, hSV.HSVtoColor(), hSV2.HSVtoColor());
			myEditFactionIconViewModel.OnFactionEditorOk += OnFactionEditorOk;
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(myEditFactionIconViewModel);
		}

		private void OnFactionEditorOk(MyEditFactionIconViewModel viewModel)
		{
			m_factionColor = MyColorPickerConstants.HSVToHSVOffset(viewModel.BackgroundColor.ColorToHSV());
			m_factionIconColor = MyColorPickerConstants.HSVToHSVOffset(viewModel.IconColor.ColorToHSV());
			GetFactionIconTextures(m_factionColor, m_factionIconColor, viewModel.ImageIconPath, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction);
			m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
			{
				backgroundTexture,
				iconFaction
			});
			m_factionIconGroupId = viewModel.FactionIconGroupId;
			m_factionIconId = viewModel.FactionIconId;
		}

		private void GetFactionIconTextures(Vector3 hsvOffsetColor, string iconPath, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction)
		{
			Vector3 hSV = MyColorPickerConstants.HSVOffsetToHSV(hsvOffsetColor);
			MyGuiControlImage.MyDrawTexture myDrawTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = "Textures\\GUI\\Blank.dds",
				ColorMask = hSV.HSVtoColor().ToVector4()
			};
			backgroundTexture = myDrawTexture;
			myDrawTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = iconPath,
				ColorMask = new Vector4(1f)
			};
			iconFaction = myDrawTexture;
		}

		private void GetFactionIconTextures(Vector3 hsvOffsetColor, Vector3 hsvIconColor, string iconPath, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction)
		{
			Vector3 hSV = MyColorPickerConstants.HSVOffsetToHSV(hsvOffsetColor);
			MyGuiControlImage.MyDrawTexture myDrawTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = "Textures\\GUI\\Blank.dds",
				ColorMask = hSV.HSVtoColor().ToVector4()
			};
			backgroundTexture = myDrawTexture;
			Vector3 hSV2 = MyColorPickerConstants.HSVOffsetToHSV(hsvIconColor);
			myDrawTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = iconPath,
				ColorMask = hSV2.HSVtoColor().ToVector4()
			};
			iconFaction = myDrawTexture;
		}

		private void GetRandomIcon(out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction)
		{
			IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
			List<string> list = new List<string>();
			MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
			foreach (MyFactionIconsDefinition item3 in allDefinitions)
			{
				if (item3.Id.SubtypeId.String == "Other")
				{
					if (component.HasDLC(MyDLCs.MyDLC.EconomyExpansion.Name, Sync.MyId))
					{
						string[] icons = item3.Icons;
						foreach (string item in icons)
						{
							list.Add(item);
						}
					}
				}
				else
				{
					string[] icons = item3.Icons;
					foreach (string item2 in icons)
					{
						list.Add(item2);
					}
				}
			}
			int index = MyRandom.Instance.Next(0, list.Count);
			MyGuiControlImage.MyDrawTexture myDrawTexture = iconFaction = new MyGuiControlImage.MyDrawTexture
			{
				Texture = list[index],
				ColorMask = Vector4.One
			};
			float x = MyRandom.Instance.NextFloat(0f, 1f);
			float y = MyRandom.Instance.NextFloat(0f, 1f);
			float z = MyRandom.Instance.NextFloat(0f, 1f);
			myDrawTexture = (backgroundTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = "Textures\\GUI\\Blank.dds",
				ColorMask = new Vector4(x, y, z, 1f)
			});
		}
	}
}
