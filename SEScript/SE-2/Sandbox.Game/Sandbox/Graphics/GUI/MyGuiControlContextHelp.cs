using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlContextHelp : MyGuiControlBase
	{
		private MyGuiControlLabel m_blockTypeLabel;

		private MyGuiControlLabel m_blockNameLabel;

		private MyGuiControlImage m_blockIconImage;

		private MyGuiControlPanel m_blockTypePanelLarge;

		private MyGuiControlPanel m_blockTypePanelSmall;

		private MyGuiControlLabel m_blockBuiltByLabel;

		private MyGuiControlPanel m_titleBackground;

		private MyGuiControlPanel m_pcuBackground;

		private MyGuiControlImage m_PCUIcon;

		private MyGuiControlLabel m_PCULabel;

		private MyGuiControlMultilineText m_helpText;

		private bool m_progressMode;

		private MyGuiControlBlockInfo.MyControlBlockInfoStyle m_style;

		private float m_smallerFontSize = 0.83f;

		private bool m_needsRecalculateSize;

		private MyHudBlockInfo m_blockInfo;

		public bool ShowBuildInfo = true;

		private float baseScale => 0.83f;

		private float itemHeight => 0.037f * baseScale;

		public bool ShowJustTitle
		{
			set
			{
				if (value)
				{
					base.Size = new Vector2(0.225f, 0.1f);
					m_helpText.Visible = false;
				}
				else
				{
					base.Size = new Vector2(0.225f, 0.32f);
					m_helpText.Visible = true;
				}
			}
		}

		public MyHudBlockInfo BlockInfo
		{
			get
			{
				return m_blockInfo;
			}
			set
			{
				if (m_blockInfo != value)
				{
					if (m_blockInfo != null)
					{
						m_blockInfo.ContextHelpChanged -= BlockInfoOnContextHelpChanged;
					}
					m_blockInfo = value;
					if (m_blockInfo != null)
					{
						m_blockInfo.ContextHelpChanged += BlockInfoOnContextHelpChanged;
					}
					m_needsRecalculateSize = true;
				}
			}
		}

		public MyGuiControlContextHelp(MyGuiControlBlockInfo.MyControlBlockInfoStyle style, bool progressMode = true, bool largeBlockInfo = true)
			: base(null, null, null, null, new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds"))
		{
			m_style = style;
			m_progressMode = true;
			base.ColorMask = m_style.BackgroundColormask;
			m_titleBackground = new MyGuiControlPanel(null, null, Color.Red.ToVector4());
			m_titleBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_titleBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
			Elements.Add(m_titleBackground);
			m_blockIconImage = new MyGuiControlImage();
			m_blockIconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockIconImage.BackgroundTexture = (m_progressMode ? null : new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_DEFAULT.Texture));
			m_blockIconImage.Size = (m_progressMode ? new Vector2(0.066f) : new Vector2(0.04f));
			m_blockIconImage.Size *= new Vector2(0.75f, 1f);
			Elements.Add(m_blockIconImage);
			m_blockTypePanelLarge = new MyGuiControlPanel();
			m_blockTypePanelLarge.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Elements.Add(m_blockTypePanelLarge);
			m_blockTypePanelSmall = new MyGuiControlPanel();
			m_blockTypePanelSmall.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Elements.Add(m_blockTypePanelSmall);
			m_blockNameLabel = new MyGuiControlLabel(null, null, string.Empty);
			m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockNameLabel.TextScale = 1f * baseScale;
			m_blockNameLabel.Font = m_style.BlockNameLabelFont;
			m_blockNameLabel.AutoEllipsis = true;
			Elements.Add(m_blockNameLabel);
			string text = string.Empty;
			if (style.EnableBlockTypeLabel)
			{
				text = MyTexts.GetString(largeBlockInfo ? MySpaceTexts.HudBlockInfo_LargeShip_Station : MySpaceTexts.HudBlockInfo_SmallShip);
			}
			m_blockTypeLabel = new MyGuiControlLabel(null, null, text);
			m_blockTypeLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockTypeLabel.TextScale = 1f * baseScale;
			m_blockTypeLabel.Font = "White";
			Elements.Add(m_blockTypeLabel);
			m_blockBuiltByLabel = new MyGuiControlLabel(null, null, string.Empty);
			m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_blockBuiltByLabel.TextScale = m_smallerFontSize * baseScale;
			m_blockBuiltByLabel.Font = m_style.InstalledRequiredLabelFont;
			Elements.Add(m_blockBuiltByLabel);
			m_pcuBackground = new MyGuiControlPanel(null, null, new Color(0.21f, 0.26f, 0.3f));
			m_pcuBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_pcuBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
			m_pcuBackground.Size = new Vector2(0.225f, 0.03f);
			Elements.Add(m_pcuBackground);
			m_PCUIcon = new MyGuiControlImage(null, new Vector2(0.022f, 0.029f), null, null, new string[1]
			{
				"Textures\\GUI\\PCU.png"
			});
			m_PCUIcon.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Elements.Add(m_PCUIcon);
			m_PCULabel = new MyGuiControlLabel();
			m_PCULabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Elements.Add(m_PCULabel);
			m_helpText = new MyGuiControlMultilineText(null, null, null, "Blue", 0.68f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "HelpText",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			Elements.Add(m_helpText);
			base.Size = new Vector2(0.225f, 0.32f);
		}

		public void RecalculateSize()
		{
			m_helpText.Position = -base.Size / 2f + new Vector2(0f, m_titleBackground.Size.Y) + new Vector2(0.01f, 0.01f);
			m_helpText.Size = new Vector2(base.Size.X, base.Size.Y - m_titleBackground.Size.Y - 0.006f);
			m_helpText.RefreshText(useEnum: false);
			m_helpText.Text.Clear();
			m_helpText.Text.Append(m_blockInfo.ContextHelp);
			m_helpText.Parse();
			m_needsRecalculateSize = false;
		}

		private void Reposition()
		{
			Vector2 vector = -base.Size / 2f;
			Vector2 value = new Vector2(base.Size.X / 2f, (0f - base.Size.Y) / 2f);
			Vector2 value2 = new Vector2((0f - base.Size.X) / 2f, base.Size.Y / 2f);
			Vector2 vector2 = vector + (m_progressMode ? new Vector2(0.06f, 0f) : new Vector2(0.036f, 0f));
			float num = 0.072f * baseScale;
			m_blockIconImage.Position = vector + new Vector2(0.005f, 0.005f);
			Vector2 vector3 = new Vector2(0.0035f) * new Vector2(0.75f, 1f) * baseScale;
			if (!m_progressMode)
			{
				vector3.Y *= 1f;
			}
			m_titleBackground.Position = vector;
			m_titleBackground.ColorMask = m_style.TitleBackgroundColor;
			num = ((!m_progressMode) ? (Math.Abs(vector.Y - m_blockIconImage.Position.Y) + m_blockIconImage.Size.Y + 0.003f) : (Math.Abs(vector.Y - m_blockIconImage.Position.Y) + m_blockIconImage.Size.Y));
			m_titleBackground.Size = new Vector2(value.X - m_titleBackground.Position.X, num + 0.003f);
			if (m_needsRecalculateSize)
			{
				RecalculateSize();
			}
			if (m_progressMode)
			{
				m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				float num2 = 0.16f;
				float num3 = 0.81f;
				Vector2 vector4 = MyGuiManager.MeasureString(m_blockNameLabel.Font, m_blockNameLabel.TextToDraw, num3);
				if (vector4.X > num2)
				{
					m_blockNameLabel.TextScale = num3 * (num2 / vector4.X);
				}
				else
				{
					m_blockNameLabel.TextScale = num3;
				}
				m_blockNameLabel.Size = new Vector2(value.X - (m_blockIconImage.Position.X + m_blockIconImage.Size.X + 0.004f), m_blockNameLabel.Size.Y);
				m_blockNameLabel.Position = new Vector2(vector2.X, m_blockIconImage.Position.Y + 0.022f);
				m_blockTypeLabel.Visible = false;
				if (ShowBuildInfo)
				{
					m_blockTypePanelLarge.Position = value + new Vector2(-0.005f, 0.032f);
					m_blockTypePanelLarge.Size = (m_progressMode ? new Vector2(0.05f) : new Vector2(0.04f));
					m_blockTypePanelLarge.Size *= new Vector2(0.75f, 1f);
					m_blockTypePanelLarge.BackgroundTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE;
					m_blockTypePanelLarge.Visible = true;
					m_blockTypePanelLarge.Enabled = (m_blockInfo.GridSize == MyCubeSize.Large);
					m_blockTypePanelSmall.Position = value + new Vector2(-0.005f, 0.032f);
					m_blockTypePanelSmall.Size = (m_progressMode ? new Vector2(0.05f) : new Vector2(0.04f));
					m_blockTypePanelSmall.Size *= new Vector2(0.75f, 1f);
					m_blockTypePanelSmall.BackgroundTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL;
					m_blockTypePanelSmall.Visible = true;
					m_blockTypePanelSmall.Enabled = (m_blockInfo.GridSize == MyCubeSize.Small);
				}
				else
				{
					m_blockTypePanelLarge.Visible = false;
					m_blockTypePanelSmall.Visible = false;
				}
			}
			if (ShowBuildInfo)
			{
				m_pcuBackground.Visible = true;
				m_PCUIcon.Visible = true;
				m_PCULabel.Visible = true;
				m_pcuBackground.Position = value2 + new Vector2(0f, -0.03f);
				m_PCUIcon.Position = value2 + new Vector2(0.0085f, -0.03f);
				m_PCULabel.Position = m_PCUIcon.Position + new Vector2(0.035f, 0.003f);
				m_PCULabel.Text = "PCU: " + m_blockInfo.PCUCost;
			}
			else
			{
				m_pcuBackground.Visible = false;
				m_PCUIcon.Visible = false;
				m_PCULabel.Visible = false;
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			m_pcuBackground.Visible = false;
			m_PCUIcon.Visible = false;
			m_PCULabel.Visible = false;
			if (m_blockInfo != null)
			{
				Reposition();
				m_blockNameLabel.TextToDraw.Clear();
				if (m_blockInfo.BlockName != null)
				{
					m_blockNameLabel.TextToDraw.Append(m_blockInfo.BlockName);
				}
				m_blockNameLabel.TextToDraw.ToUpper();
				m_blockNameLabel.Autowrap(0.25f);
				m_blockIconImage.SetTextures(m_blockInfo.BlockIcons);
				if (ShowBuildInfo)
				{
					m_blockBuiltByLabel.Visible = true;
					m_blockBuiltByLabel.Position = m_blockNameLabel.Position + new Vector2(0f, m_blockNameLabel.Size.Y + 0f);
					m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					m_blockBuiltByLabel.TextScale = 0.6f;
					m_blockBuiltByLabel.TextToDraw.Clear();
					MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(m_blockInfo.BlockBuiltBy);
					if (myIdentity != null)
					{
						m_blockBuiltByLabel.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.BuiltBy));
						m_blockBuiltByLabel.TextToDraw.Append(": ");
						m_blockBuiltByLabel.TextToDraw.Append(myIdentity.DisplayName);
					}
				}
				else
				{
					m_blockBuiltByLabel.Visible = false;
				}
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
		}

		private void BlockInfoOnContextHelpChanged(string obj)
		{
			m_needsRecalculateSize = true;
		}
	}
}
