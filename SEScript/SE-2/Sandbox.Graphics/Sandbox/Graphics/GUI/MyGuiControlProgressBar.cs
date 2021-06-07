using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlProgressBar))]
	public class MyGuiControlProgressBar : MyGuiControlBase
	{
		public Color ProgressColor;

		private float m_value = 1f;

		public bool IsHorizontal = true;

		public bool EnableBorderAutohide;

		public float BorderAutohideThreshold = 0.01f;

		private MyGuiControlPanel m_potentialBar;

		private MyGuiControlPanel m_progressForeground;

		private MyGuiControlPanel m_progressBarLine;

		private static readonly Color DEFAULT_PROGRESS_COLOR = Color.White;

		public float Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		public MyGuiControlPanel PotentialBar => m_potentialBar;

		public MyGuiControlPanel ForegroundBar => m_progressForeground;

		public MyGuiControlPanel ForegroundBarEndLine => m_progressBarLine;

		public MyGuiControlProgressBar(Vector2? position = null, Vector2? size = null, Color? progressBarColor = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiCompositeTexture backgroundTexture = null, bool isHorizontal = true, bool potentialBarEnabled = true, bool enableBorderAutohide = false, float borderAutohideThreshold = 0.01f)
			: base(position, size, null, null, backgroundTexture, isActiveControl: true, canHaveFocus: false, MyGuiControlHighlightType.WHEN_ACTIVE, originAlign)
		{
			ProgressColor = (progressBarColor.HasValue ? progressBarColor.Value : DEFAULT_PROGRESS_COLOR);
			IsHorizontal = isHorizontal;
			EnableBorderAutohide = enableBorderAutohide;
			BorderAutohideThreshold = borderAutohideThreshold;
			m_progressForeground = new MyGuiControlPanel(new Vector2((0f - base.Size.X) / 2f, 0f), null, ProgressColor, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_progressForeground.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
			m_potentialBar = new MyGuiControlPanel(null, new Vector2(0f, base.Size.Y), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_potentialBar.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
			m_potentialBar.ColorMask = new Vector4(ProgressColor, 0.7f);
			m_potentialBar.Visible = false;
			m_potentialBar.Enabled = potentialBarEnabled;
			Elements.Add(m_potentialBar);
			Elements.Add(m_progressForeground);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			Vector2 size = base.Size * new Vector2(IsHorizontal ? Value : 1f, IsHorizontal ? 1f : Value);
			m_progressForeground.Size = size;
			if (EnableBorderAutohide && Value <= BorderAutohideThreshold)
			{
				m_progressForeground.BorderEnabled = false;
			}
			else
			{
				m_progressForeground.BorderEnabled = true;
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public Vector2 CalculatePotentialBarPosition()
		{
			return new Vector2(m_progressForeground.Position.X + m_progressForeground.Size.X, 0f);
		}
	}
}
