using Sandbox.Game.Localization;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyGuiReputationProgressBar : MyGuiControlBase
	{
		public static readonly Color COL_BACKGROUND = new Color(0.25f, 0.3f, 0.35f, 1f);

		public static readonly Color COL_HOSTILE = new Color(228, 62, 62);

		public static readonly Color COL_NEUTRAL = new Color(149, 169, 179);

		public static readonly Color COL_FRIENDLY = new Color(101, 178, 91);

		public static readonly Color COL_BORDER = new Color(100, 120, 130);

		private static readonly string BAR_TEXTURE = "Textures/GUI/Controls/progressRect.dds";

		private static readonly float HEIGHT_BAR = 0.6f;

		private static readonly float HEIGHT_BORDER = 0.65f;

		private static readonly float THICKNESS_BORDER = 0.01f;

		private static readonly float OFFSET_DOWN_TEXT = 0.75f;

		private static readonly float SIZE_TEXT = 0.55f;

		private int m_current;

		private int m_repMin = -10;

		private int m_repMax = 10;

		private int m_border1 = -5;

		private int m_border2 = 5;

		private int m_offerBonus;

		private int m_orderBonus;

		private int m_offerBonusMax;

		private int m_orderBonusMax;

		private string m_tooltipText_Hostile;

		private string m_tooltipText_Neutral;

		private string m_tooltipText_Friendly;

		public Color ColorBackground;

		public Color ColorHostile;

		public Color ColorNeutral;

		public Color ColorFriendly;

		public Color ColorBorder;

		public MyGuiReputationProgressBar(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, float textScale = 0.8f, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			: base(position, size, colorMask, null, null, isActiveControl: false)
		{
			base.OriginAlign = originAlign;
			ColorBackground = COL_BACKGROUND;
			ColorHostile = COL_HOSTILE;
			ColorNeutral = COL_NEUTRAL;
			ColorFriendly = COL_FRIENDLY;
			ColorBorder = COL_BORDER;
			m_tooltipText_Hostile = string.Empty;
			m_tooltipText_Neutral = string.Empty;
			m_tooltipText_Friendly = string.Empty;
		}

		public void SetBonusValues(int offerBonus, int orderBonus, int offerbonusMax, int orderBonusMax)
		{
			m_offerBonus = offerBonus;
			m_orderBonus = orderBonus;
			m_offerBonusMax = offerbonusMax;
			m_orderBonusMax = orderBonusMax;
			UpdateTexts();
		}

		public void SetBorderValues(int min, int max, int border1, int border2)
		{
			m_repMin = min;
			m_repMax = max;
			m_border1 = border1;
			m_border2 = border2;
			UpdateTexts();
		}

		private void UpdateTexts()
		{
			m_tooltipText_Hostile = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Hostile), m_repMin, m_border1);
			m_tooltipText_Neutral = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Neutral), m_border1, m_border2);
			m_tooltipText_Friendly = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Friendly), m_border2, m_repMax, m_offerBonus, m_orderBonus, m_offerBonusMax, m_orderBonusMax);
		}

		public void SetCurrentValue(int value)
		{
			m_current = value;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 vector = new Vector2((float)(m_border1 - m_repMin) / (float)(m_repMax - m_repMin) * base.Size.X, 0f);
			Vector2 vector2 = new Vector2((float)(m_border2 - m_repMin) / (float)(m_repMax - m_repMin) * base.Size.X, 0f);
			Color cOL_BACKGROUND = COL_BACKGROUND;
			Color cOL_BACKGROUND2 = COL_BACKGROUND;
			float num = (m_current < m_repMin) ? m_repMin : ((m_current > m_repMax) ? m_repMax : m_current);
			float num2 = (num - (float)m_repMin) / (float)(m_repMax - m_repMin);
			Vector2 normalizedSize = new Vector2(base.Size.X, HEIGHT_BAR * base.Size.Y);
			Vector2 normalizedSize2 = new Vector2(num2 * base.Size.X, HEIGHT_BAR * base.Size.Y);
			Vector2 normalizedSize3 = new Vector2(THICKNESS_BORDER * base.Size.X, HEIGHT_BORDER * base.Size.Y);
			Vector2 value = new Vector2(0f, 0.5f * (HEIGHT_BORDER - HEIGHT_BAR) * base.Size.Y);
			cOL_BACKGROUND2 = ((num < (float)m_border1) ? COL_HOSTILE : ((!(num < (float)m_border2)) ? COL_FRIENDLY : COL_NEUTRAL));
			MyGuiManager.DrawSpriteBatch(BAR_TEXTURE, GetPositionAbsolute() + value, normalizedSize, cOL_BACKGROUND, base.OriginAlign);
			MyGuiManager.DrawSpriteBatch(BAR_TEXTURE, GetPositionAbsolute() + value, normalizedSize2, cOL_BACKGROUND2, base.OriginAlign);
			MyGuiManager.DrawSpriteBatch(BAR_TEXTURE, GetPositionAbsolute() + vector, normalizedSize3, COL_BORDER, base.OriginAlign);
			MyGuiManager.DrawSpriteBatch(BAR_TEXTURE, GetPositionAbsolute() + vector2, normalizedSize3, COL_BORDER, base.OriginAlign);
			MyGuiManager.DrawString("Blue", new StringBuilder($"{m_repMin}"), GetPositionAbsolute() + new Vector2(0f, OFFSET_DOWN_TEXT * base.Size.Y), SIZE_TEXT, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("Blue", new StringBuilder($"{m_border1}"), GetPositionAbsolute() + new Vector2(vector.X, OFFSET_DOWN_TEXT * base.Size.Y), SIZE_TEXT, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("Blue", new StringBuilder($"{m_border2}"), GetPositionAbsolute() + new Vector2(vector2.X, OFFSET_DOWN_TEXT * base.Size.Y), SIZE_TEXT, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("Blue", new StringBuilder($"{m_repMax}"), GetPositionAbsolute() + new Vector2(base.Size.X, OFFSET_DOWN_TEXT * base.Size.Y), SIZE_TEXT, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			UpdateAndDrawTooltip(vector, vector2, new Vector2(base.Size.X - vector2.X, 0f), HEIGHT_BAR);
		}

		private void UpdateAndDrawTooltip(Vector2 bar1, Vector2 bar2, Vector2 bar3, float height)
		{
			if (!MyGuiControlBase.CheckMouseOver(base.Size, GetPositionAbsolute(), base.OriginAlign))
			{
				m_showToolTip = true;
				return;
			}
			Vector2 positionAbsolute = GetPositionAbsolute();
			Vector2 size = bar1;
			size.Y = height;
			Vector2 position = GetPositionAbsolute() + bar1;
			Vector2 size2 = bar2 - bar1;
			size2.Y = height;
			Vector2 position2 = GetPositionAbsolute() + bar2;
			Vector2 size3 = bar3;
			size3.Y = height;
			string toolTip = "Tooltip";
			if (MyGuiControlBase.CheckMouseOver(size, positionAbsolute, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
			{
				toolTip = m_tooltipText_Hostile;
			}
			if (MyGuiControlBase.CheckMouseOver(size2, position, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
			{
				toolTip = m_tooltipText_Neutral;
			}
			if (MyGuiControlBase.CheckMouseOver(size3, position2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
			{
				toolTip = m_tooltipText_Friendly;
			}
			m_toolTip = new MyToolTips(toolTip);
			m_showToolTip = true;
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + base.Position, base.Size, Color.White, 1);
		}
	}
}
