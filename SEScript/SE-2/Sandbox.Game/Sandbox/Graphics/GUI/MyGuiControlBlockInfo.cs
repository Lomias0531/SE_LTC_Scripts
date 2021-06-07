using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlBlockInfo : MyGuiControlBase
	{
		public struct MyControlBlockInfoStyle
		{
			public Vector4 BackgroundColormask;

			public string BlockNameLabelFont;

			public MyStringId ComponentsLabelText;

			public string ComponentsLabelFont;

			public MyStringId InstalledRequiredLabelText;

			public string InstalledRequiredLabelFont;

			public MyStringId RequiredAvailableLabelText;

			public MyStringId RequiredLabelText;

			public string IntegrityLabelFont;

			public Vector4 IntegrityBackgroundColor;

			public Vector4 IntegrityForegroundColor;

			public Vector4 IntegrityForegroundColorOverCritical;

			public Vector4 LeftColumnBackgroundColor;

			public Vector4 TitleBackgroundColor;

			public Vector4 TitleSeparatorColor;

			public string ComponentLineMissingFont;

			public string ComponentLineAllMountedFont;

			public string ComponentLineAllInstalledFont;

			public string ComponentLineDefaultFont;

			public Vector4 ComponentLineDefaultColor;

			public bool EnableBlockTypeLabel;

			public bool ShowAvailableComponents;

			public bool EnableBlockTypePanel;

			public bool HiddenPCU;

			public bool HiddenHeader;
		}

		private class ComponentLineControl : MyGuiControlBase
		{
			public MyGuiControlImage IconImage;

			public MyGuiControlPanel IconPanelProgress;

			public MyGuiControlLabel NameLabel;

			public MyGuiControlLabel NumbersLabel;

			public ComponentLineControl(Vector2 size, float iconSize)
				: base(null, size, null, null, null, isActiveControl: true, canHaveFocus: false, MyGuiControlHighlightType.WHEN_ACTIVE, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Vector2 size2 = new Vector2(iconSize) * new Vector2(0.75f, 1f);
				Vector2 value = new Vector2((0f - base.Size.X) / 2f, 0f);
				Vector2 value2 = new Vector2(base.Size.X / 2f, 0f);
				Vector2 position = value - new Vector2(0f, size2.Y / 2f);
				IconImage = new MyGuiControlImage();
				IconPanelProgress = new MyGuiControlPanel();
				NameLabel = new MyGuiControlLabel(null, null, string.Empty);
				NumbersLabel = new MyGuiControlLabel(null, null, string.Empty);
				IconImage.Size = size2;
				IconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				IconImage.Position = position;
				IconPanelProgress.Size = size2;
				IconPanelProgress.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				IconPanelProgress.Position = position;
				IconPanelProgress.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
				float num = 0.1f;
				IconPanelProgress.ColorMask = new Vector4(num, num, num, 0.5f);
				NameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				NameLabel.Position = value + new Vector2(size2.X + 0.01225f, 0f);
				NameLabel.AutoEllipsis = true;
				NumbersLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				NumbersLabel.Position = value2 + new Vector2(-0.033f, 0f);
				Elements.Add(IconImage);
				Elements.Add(IconPanelProgress);
				Elements.Add(NameLabel);
				Elements.Add(NumbersLabel);
			}

			public void RecalcTextSize(bool progressMode)
			{
				NameLabel.BorderEnabled = false;
				NumbersLabel.BorderEnabled = false;
				float num = 0.002f;
				if (progressMode)
				{
					Vector2 value = new Vector2(base.Size.X / 2f, 0f);
					NumbersLabel.Position = value + new Vector2(-0.133f, 0f);
				}
				float num2 = NumbersLabel.Position.X - NameLabel.Position.X - NumbersLabel.Size.X - 2f * num;
				int num3 = 3;
				Vector2 vector = MyGuiManager.MeasureString(NameLabel.Font, NameLabel.TextToDraw, NameLabel.TextScale);
				while (num3 > 0 && vector.X > num2)
				{
					NameLabel.TextScale *= 0.9f;
					vector = MyGuiManager.MeasureString(NameLabel.Font, NameLabel.TextToDraw, NameLabel.TextScale);
					num3--;
				}
				NameLabel.Size = new Vector2(num2 + num, NameLabel.Size.Y);
			}

			public void SetProgress(float val)
			{
				IconPanelProgress.Size = IconImage.Size * new Vector2(1f, 1f - val);
			}
		}

		public static bool ShowComponentProgress = true;

		public static bool ShowCriticalComponent = false;

		public static bool ShowCriticalIntegrity = true;

		public static bool ShowOwnershipIntegrity = MyFakes.SHOW_FACTIONS_GUI;

		public static Vector4 CriticalIntegrityColor = Color.Red.ToVector4();

		public static Vector4 CriticalComponentColor = CriticalIntegrityColor * new Vector4(1f, 1f, 1f, 0.7f);

		public static Vector4 OwnershipIntegrityColor = Color.Blue.ToVector4();

		private MyGuiControlLabel m_blockTypeLabel;

		private MyGuiControlLabel m_blockNameLabel;

		private MyGuiControlLabel m_componentsLabel;

		private MyGuiControlLabel m_installedRequiredLabel;

		private MyGuiControlLabel m_blockBuiltByLabel;

		private MyGuiControlLabel m_integrityLabel;

		private MyGuiControlLabel m_PCULabel;

		private MyGuiControlImage m_blockIconImage;

		private MyGuiControlImage m_PCUIcon;

		private MyGuiControlPanel m_blockTypePanel;

		private MyGuiControlPanel m_pcuBackground;

		private MyGuiControlPanel m_titleBackground;

		private MyGuiControlPanel m_integrityBackground;

		private MyGuiProgressCompositeTextureAdvanced m_integrityForeground;

		private Color m_integrityForegroundColorMask = Color.White;

		private MyGuiControlLabel m_criticalIntegrityLabel;

		private MyGuiControlLabel m_ownershipIntegrityLabel;

		private MyGuiControlSeparatorList m_separator;

		private List<ComponentLineControl> m_componentLines = new List<ComponentLineControl>(15);

		public MyHudBlockInfo BlockInfo;

		private bool m_progressMode;

		private MyControlBlockInfoStyle m_style;

		private float m_smallerFontSize = 0.83f;

		private float baseScale
		{
			get
			{
				if (!m_progressMode)
				{
					return 0.83f;
				}
				return 1f;
			}
		}

		private float itemHeight => 0.037f * baseScale;

		public MyGuiControlBlockInfo(MyControlBlockInfoStyle style, bool progressMode = true, bool largeBlockInfo = true)
			: base(null, null, null, null, new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds"))
		{
			m_style = style;
			m_progressMode = progressMode;
			if (m_progressMode)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM_30;
			}
			base.ColorMask = m_style.BackgroundColormask;
			m_titleBackground = new MyGuiControlPanel(null, null, Color.Red.ToVector4());
			m_titleBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_titleBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
			Elements.Add(m_titleBackground);
			if (m_progressMode)
			{
				m_integrityLabel = new MyGuiControlLabel(null, null, string.Empty);
				m_integrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
				m_integrityLabel.Font = m_style.IntegrityLabelFont;
				Elements.Add(m_integrityLabel);
				m_integrityBackground = new MyGuiControlPanel();
				m_integrityBackground.BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR;
				m_integrityBackground.ColorMask = m_style.IntegrityBackgroundColor;
				m_integrityBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
				Elements.Add(m_integrityBackground);
				m_integrityForeground = new MyGuiProgressCompositeTextureAdvanced(MyGuiConstants.TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR);
				m_integrityForeground.IsInverted = true;
				m_integrityForeground.Orientation = MyGuiProgressCompositeTexture.BarOrientation.VERTICAL;
				m_criticalIntegrityLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.Functional));
				m_criticalIntegrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
				m_criticalIntegrityLabel.TextScale = 0.4f * baseScale;
				m_criticalIntegrityLabel.Font = "Blue";
				m_ownershipIntegrityLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.Hack));
				m_ownershipIntegrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
				m_ownershipIntegrityLabel.TextScale = 0.4f * baseScale;
				m_ownershipIntegrityLabel.Font = "Blue";
			}
			m_blockIconImage = new MyGuiControlImage();
			m_blockIconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockIconImage.BackgroundTexture = (m_progressMode ? null : new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_DEFAULT.Texture));
			m_blockIconImage.Size = (m_progressMode ? new Vector2(0.066f) : new Vector2(0.04f));
			m_blockIconImage.Size *= new Vector2(0.75f, 1f);
			Elements.Add(m_blockIconImage);
			m_blockTypePanel = new MyGuiControlPanel();
			m_blockTypePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockTypePanel.Size = (m_progressMode ? new Vector2(0.088f) : new Vector2(0.02f));
			m_blockTypePanel.Size *= new Vector2(0.75f, 1f);
			m_blockTypePanel.BackgroundTexture = new MyGuiCompositeTexture(largeBlockInfo ? "Textures\\GUI\\Icons\\HUD 2017\\GridSizeLargeFit.png" : "Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmallFit.png");
			Elements.Add(m_blockTypePanel);
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
			m_componentsLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(m_style.ComponentsLabelText));
			m_componentsLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_componentsLabel.TextScale = m_smallerFontSize * baseScale;
			m_componentsLabel.Font = m_style.ComponentsLabelFont;
			Elements.Add(m_componentsLabel);
			m_installedRequiredLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(m_style.InstalledRequiredLabelText));
			m_installedRequiredLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_installedRequiredLabel.TextScale = m_smallerFontSize * baseScale;
			m_installedRequiredLabel.Font = m_style.InstalledRequiredLabelFont;
			Elements.Add(m_installedRequiredLabel);
			m_blockBuiltByLabel = new MyGuiControlLabel(null, null, string.Empty);
			m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			m_blockBuiltByLabel.TextScale = m_smallerFontSize * baseScale;
			m_blockBuiltByLabel.Font = m_style.InstalledRequiredLabelFont;
			Elements.Add(m_blockBuiltByLabel);
			if (!m_progressMode && !m_style.HiddenPCU)
			{
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
			}
			m_separator = new MyGuiControlSeparatorList();
			EnsureLineControls(m_componentLines.Capacity);
			base.Size = (m_progressMode ? new Vector2(0.225f, 0.4f) : new Vector2(0.225f, 0.4f));
		}

		private void EnsureLineControls(int count)
		{
			while (m_componentLines.Count < count)
			{
				ComponentLineControl componentLineControl = new ComponentLineControl((m_progressMode ? new Vector2(0.288f, 0.05f) : new Vector2(0.24f, 0.05f)) * new Vector2(1f, baseScale), 0.035f * baseScale);
				m_componentLines.Add(componentLineControl);
				Elements.Add(componentLineControl);
			}
		}

		public void RecalculateSize()
		{
			if (m_progressMode)
			{
				base.Size = new Vector2(base.Size.X, 0.12f * baseScale + itemHeight * (float)(BlockInfo.Components.Count - 2));
				return;
			}
			base.Size = new Vector2(base.Size.X, 0.1f * baseScale + itemHeight * (float)(BlockInfo.Components.Count + 1));
			if (m_style.HiddenPCU)
			{
				base.Size -= new Vector2(0f, itemHeight);
			}
			if (m_style.HiddenHeader)
			{
				base.Size -= new Vector2(0f, itemHeight);
			}
		}

		private void Reposition()
		{
			RecalculateSize();
			Vector2 vector = -base.Size / 2f;
			Vector2 value = new Vector2(base.Size.X / 2f, (0f - base.Size.Y) / 2f);
			Vector2 value2 = new Vector2((0f - base.Size.X) / 2f, base.Size.Y / 2f);
			Vector2 value3 = vector + (m_progressMode ? new Vector2(0.06f, 0f) : new Vector2(0.036f, 0f));
			float num = 0.072f * baseScale;
			Vector2 vector2 = new Vector2(0.0035f) * new Vector2(0.75f, 1f) * baseScale;
			if (!m_progressMode)
			{
				vector2.Y *= 1f;
			}
			if (BlockInfo.BlockIntegrity > 0f)
			{
				m_installedRequiredLabel.TextToDraw = MyTexts.Get(m_style.RequiredLabelText);
			}
			else if (BlockInfo.ShowAvailable)
			{
				m_installedRequiredLabel.TextToDraw = MyTexts.Get(m_style.RequiredAvailableLabelText);
			}
			else
			{
				m_installedRequiredLabel.TextToDraw = MyTexts.Get(m_style.RequiredLabelText);
			}
			m_titleBackground.Position = vector;
			m_titleBackground.ColorMask = m_style.TitleBackgroundColor;
			if (m_progressMode || m_style.HiddenHeader)
			{
				num = 0f;
				m_blockIconImage.Visible = false;
				m_blockTypeLabel.Visible = false;
				m_blockNameLabel.Visible = false;
				m_titleBackground.Visible = false;
			}
			else
			{
				num = Math.Abs(vector.Y - m_blockIconImage.Position.Y) + m_blockIconImage.Size.Y + 0.003f;
			}
			m_titleBackground.Size = new Vector2(value.X - m_titleBackground.Position.X, num + 0.003f);
			m_separator.Clear();
			if (m_progressMode)
			{
				m_ownershipIntegrityLabel.Visible = false;
				m_criticalIntegrityLabel.Visible = false;
				float num2 = itemHeight * (float)BlockInfo.Components.Count;
				float num3 = 0.05f;
				Vector2 value4 = new Vector2(0.006f, 0.04f);
				Vector2 vector3 = GetPositionAbsoluteTopLeft() + value4;
				Vector2 position = vector + value4 + new Vector2(0f, num2);
				Vector2 vector4 = new Vector2(num3, num2);
				m_integrityBackground.Position = position;
				m_integrityBackground.Size = vector4;
				m_integrityLabel.Position = vector + value4 + new Vector2(num3 / 2f, -0.005f);
				m_integrityLabel.TextToDraw.Clear();
				m_integrityLabel.TextToDraw.AppendInt32((int)Math.Floor(BlockInfo.BlockIntegrity * 100f)).Append("%");
				if (BlockInfo.BlockIntegrity > 0f)
				{
					Vector4 v = (BlockInfo.BlockIntegrity > BlockInfo.CriticalIntegrity) ? m_style.IntegrityForegroundColorOverCritical : m_style.IntegrityForegroundColor;
					m_integrityForegroundColorMask = v;
					m_integrityForeground.Position = new Vector2I(MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(vector3));
					m_integrityForeground.Size = new Vector2I(MyGuiManager.GetScreenSizeFromNormalizedSize(vector4));
					float width = 0.004f;
					if (ShowCriticalIntegrity)
					{
						m_separator.AddHorizontal(vector3 + new Vector2(0f, num2 * (1f - BlockInfo.CriticalIntegrity)), num3, width, CriticalIntegrityColor);
						m_criticalIntegrityLabel.Position = vector3 + new Vector2(num3 / 2f, num2 * (1f - BlockInfo.CriticalIntegrity));
						m_criticalIntegrityLabel.Visible = true;
					}
					if (ShowOwnershipIntegrity && BlockInfo.OwnershipIntegrity > 0f)
					{
						m_separator.AddHorizontal(vector3 + new Vector2(0f, num2 * (1f - BlockInfo.OwnershipIntegrity)), num3, width, OwnershipIntegrityColor);
						m_ownershipIntegrityLabel.Position = vector3 + new Vector2(num3 / 2f, num2 * (1f - BlockInfo.OwnershipIntegrity) + 0.002f);
						m_ownershipIntegrityLabel.Visible = true;
					}
				}
			}
			else if (!m_style.HiddenPCU)
			{
				m_pcuBackground.Position = value2 + new Vector2(0f, -0.03f);
				m_pcuBackground.Size = new Vector2(base.Size.X, m_pcuBackground.Size.Y);
				m_PCUIcon.Position = value2 + new Vector2(0.0085f, -0.03f);
				m_PCULabel.Position = m_PCUIcon.Position + new Vector2(0.035f, 0.003f);
				m_PCULabel.Text = "PCU: " + BlockInfo.PCUCost;
			}
			if (m_progressMode)
			{
				m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_blockNameLabel.TextScale = 0.81f;
				m_blockNameLabel.Size = new Vector2(value.X - (m_blockIconImage.Position.X + m_blockIconImage.Size.X + 0.004f), m_blockNameLabel.Size.Y);
				m_blockNameLabel.Position = new Vector2(value3.X, m_blockIconImage.Position.Y + 0.022f);
				m_blockBuiltByLabel.Position = m_blockNameLabel.Position + new Vector2(0f, m_blockNameLabel.Size.Y + 0f);
				m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_blockBuiltByLabel.TextScale = 0.6f;
				m_componentsLabel.Position = value3 + new Vector2(0f, 0.0095f + num * baseScale);
				m_installedRequiredLabel.Position = value + new Vector2(-0.011f, 0.0095f + num * baseScale);
				m_blockTypeLabel.Visible = false;
				m_blockTypePanel.Visible = false;
			}
			else
			{
				m_blockTypePanel.Position = vector + new Vector2(0.01f, 0.012f);
				if (m_style.EnableBlockTypePanel)
				{
					m_blockTypePanel.Visible = true;
					m_blockNameLabel.Size = new Vector2(m_blockTypePanel.Position.X - m_blockTypePanel.Size.X - m_blockNameLabel.Position.X, m_blockNameLabel.Size.Y);
				}
				else
				{
					m_blockTypePanel.Visible = false;
					m_blockNameLabel.Size = new Vector2(value.X - (m_blockIconImage.Position.X + m_blockIconImage.Size.X + 0.004f) - 0.006f, m_blockNameLabel.Size.Y);
				}
				m_blockNameLabel.TextScale = 0.95f * baseScale;
				m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
				m_blockNameLabel.Position = new Vector2(value3.X + 0.006f, m_blockIconImage.Position.Y + m_blockIconImage.Size.Y);
				if (!m_style.EnableBlockTypeLabel)
				{
					m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
					m_blockNameLabel.Position -= new Vector2(0f, m_blockIconImage.Size.Y * 0.5f);
				}
				m_blockTypeLabel.Visible = true;
				m_blockTypeLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_blockTypeLabel.TextScale = m_smallerFontSize * baseScale;
				m_blockTypeLabel.Position = m_blockIconImage.Position + new Vector2(m_blockIconImage.Size.X, 0f) + new Vector2(0.004f, -0.0025f);
				m_componentsLabel.Position = value3 + new Vector2(0.006f, 0.015f + num * baseScale);
				m_installedRequiredLabel.Position = value + new Vector2(-0.011f, 0.015f + num * baseScale);
			}
			m_blockIconImage.Position = vector + new Vector2(0.005f, 0.005f);
			Vector2 value5 = (!m_progressMode) ? (vector + new Vector2(0.008f, 0.012f + m_componentsLabel.Size.Y + num * baseScale)) : (value3 + new Vector2(0f, 0.012f + m_componentsLabel.Size.Y + num * baseScale));
			for (int i = 0; i < BlockInfo.Components.Count; i++)
			{
				m_componentLines[i].Position = value5 + new Vector2(0f, (float)(BlockInfo.Components.Count - i - 1) * itemHeight);
				m_componentLines[i].IconPanelProgress.Visible = ShowComponentProgress;
				m_componentLines[i].IconImage.BorderColor = CriticalComponentColor;
				m_componentLines[i].NameLabel.TextScale = m_smallerFontSize * baseScale * 0.9f;
				m_componentLines[i].NumbersLabel.TextScale = m_smallerFontSize * baseScale * 0.9f;
				m_componentLines[i].NumbersLabel.PositionX = m_installedRequiredLabel.PositionX - m_installedRequiredLabel.Size.X * 0.1f;
				if (m_progressMode)
				{
					m_componentLines[i].IconImage.BackgroundTexture = null;
					m_componentLines[i].NameLabel.PositionX = (0f - m_componentLines[i].Size.X) / 2f + m_componentLines[i].IconImage.Size.X - 0.006f;
				}
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (BlockInfo != null)
			{
				EnsureLineControls(BlockInfo.Components.Count);
				Reposition();
				Vector2 value = new Vector2((0f - base.Size.X) / 2f, 0f);
				for (int i = 0; i < m_componentLines.Count; i++)
				{
					if (i < BlockInfo.Components.Count)
					{
						MyHudBlockInfo.ComponentInfo componentInfo = BlockInfo.Components[i];
						Vector4 colorMask = Vector4.One;
						string font;
						if (m_progressMode && BlockInfo.BlockIntegrity > 0f)
						{
							if (BlockInfo.MissingComponentIndex == i)
							{
								font = m_style.ComponentLineMissingFont;
							}
							else if (componentInfo.MountedCount == componentInfo.TotalCount)
							{
								font = m_style.ComponentLineAllMountedFont;
							}
							else if (componentInfo.InstalledCount == componentInfo.TotalCount)
							{
								font = m_style.ComponentLineAllInstalledFont;
							}
							else
							{
								font = m_style.ComponentLineDefaultFont;
								colorMask = m_style.ComponentLineDefaultColor;
							}
						}
						else
						{
							font = m_style.ComponentLineDefaultFont;
						}
						if (m_progressMode && BlockInfo.BlockIntegrity > 0f)
						{
							m_componentLines[i].SetProgress((float)componentInfo.MountedCount / (float)componentInfo.TotalCount);
						}
						else
						{
							m_componentLines[i].SetProgress(1f);
						}
						m_componentLines[i].Visible = true;
						m_componentLines[i].NameLabel.Font = font;
						if (m_progressMode)
						{
							m_componentLines[i].NameLabel.Position = value + new Vector2(-0.005f, 0f);
							_ = MyGuiManager.MeasureString(m_componentLines[i].NameLabel.Font, m_componentLines[i].NameLabel.TextToDraw, m_componentLines[i].NameLabel.TextScale).X;
							_ = 0.1f;
							m_componentLines[i].NameLabel.TextScale = 0.6f;
						}
						m_componentLines[i].NameLabel.ColorMask = colorMask;
						m_componentLines[i].NameLabel.TextToDraw.Clear();
						m_componentLines[i].NameLabel.TextToDraw.Append(componentInfo.ComponentName);
						m_componentLines[i].IconImage.SetTextures(componentInfo.Icons);
						m_componentLines[i].NumbersLabel.Font = font;
						m_componentLines[i].NumbersLabel.ColorMask = colorMask;
						m_componentLines[i].NumbersLabel.TextToDraw.Clear();
						if (m_progressMode && BlockInfo.BlockIntegrity > 0f)
						{
							m_componentLines[i].NumbersLabel.TextToDraw.AppendInt32(componentInfo.InstalledCount).Append(" / ").AppendInt32(componentInfo.TotalCount);
							if (m_style.ShowAvailableComponents)
							{
								m_componentLines[i].NumbersLabel.TextToDraw.Append(" / ").AppendInt32(componentInfo.AvailableAmount);
							}
						}
						else if (BlockInfo.ShowAvailable)
						{
							m_componentLines[i].NumbersLabel.TextToDraw.AppendInt32(componentInfo.TotalCount);
							if (m_style.ShowAvailableComponents)
							{
								m_componentLines[i].NumbersLabel.TextToDraw.Append(" / ").AppendInt32(componentInfo.AvailableAmount);
							}
						}
						else
						{
							m_componentLines[i].NumbersLabel.TextToDraw.AppendInt32(componentInfo.TotalCount);
						}
						float num = 1f;
						if (MyGuiManager.MeasureString(m_componentLines[i].NumbersLabel.Font, m_componentLines[i].NumbersLabel.TextToDraw, m_componentLines[i].NumbersLabel.TextScale).X > 0.06f)
						{
							num = 0.8f;
						}
						m_componentLines[i].NumbersLabel.TextScale = 0.6f;
						m_componentLines[i].NumbersLabel.TextScale *= num;
						m_componentLines[i].NumbersLabel.Size = m_componentLines[i].NumbersLabel.GetTextSize();
						m_componentLines[i].IconImage.BorderEnabled = (ShowCriticalComponent && BlockInfo.CriticalComponentIndex == i);
						m_componentLines[i].RecalcTextSize(m_progressMode);
					}
					else
					{
						m_componentLines[i].Visible = false;
					}
				}
				m_blockNameLabel.TextToDraw.Clear();
				if (BlockInfo.BlockName != null)
				{
					m_blockNameLabel.TextToDraw.Append(BlockInfo.BlockName);
				}
				m_blockNameLabel.TextToDraw.ToUpper();
				m_blockNameLabel.Autowrap(0.25f);
				m_blockBuiltByLabel.TextToDraw.Clear();
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(BlockInfo.BlockBuiltBy);
				if (myIdentity != null)
				{
					m_blockBuiltByLabel.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.BuiltBy));
					m_blockBuiltByLabel.TextToDraw.Append(": ");
					m_blockBuiltByLabel.TextToDraw.Append(myIdentity.DisplayName);
				}
				if (m_progressMode)
				{
					m_blockBuiltByLabel.Visible = false;
				}
				m_blockIconImage.SetTextures(BlockInfo.BlockIcons);
				if (BlockInfo.Components.Count == 0)
				{
					m_installedRequiredLabel.Visible = false;
					m_componentsLabel.Visible = false;
				}
				else
				{
					m_installedRequiredLabel.Visible = true;
					m_componentsLabel.Visible = true;
				}
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
			if (BlockInfo != null && m_integrityForeground != null)
			{
				m_integrityForeground.Draw(BlockInfo.BlockIntegrity, m_integrityForegroundColorMask);
			}
			if (m_separator != null)
			{
				m_separator.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
			if (ShowCriticalIntegrity && m_criticalIntegrityLabel != null && m_criticalIntegrityLabel.Visible)
			{
				m_criticalIntegrityLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
			if (ShowOwnershipIntegrity && m_ownershipIntegrityLabel != null && m_ownershipIntegrityLabel.Visible)
			{
				m_ownershipIntegrityLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
		}
	}
}
