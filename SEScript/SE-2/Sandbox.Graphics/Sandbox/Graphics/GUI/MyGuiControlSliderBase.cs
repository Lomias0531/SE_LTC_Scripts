using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlSliderBase : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture RailTexture;

			public MyGuiCompositeTexture RailHighlightTexture;

			public MyGuiHighlightTexture ThumbTexture;
		}

		private static StyleDefinition[] m_styles;

		public Action<MyGuiControlSliderBase> ValueChanged;

		private bool m_controlCaptured;

		private string m_thumbTexture;

		private MyGuiControlLabel m_label;

		private bool showLabel = true;

		private MyGuiCompositeTexture m_railTexture;

		private float m_labelSpaceWidth;

		private float m_debugScale = 1f;

		public float? DefaultRatio;

		private MyGuiControlSliderStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private MyGuiSliderProperties m_props;

		private float m_ratio;

		public Func<MyGuiControlSliderBase, bool> SliderClicked;

		private int m_lastTimeArrowPressed;

		private bool m_lastMoveDirection;

		private float m_consecutiveSliderMoves;

		public MyGuiControlSliderStyleEnum VisualStyle
		{
			get
			{
				return m_visualStyle;
			}
			set
			{
				m_visualStyle = value;
				RefreshVisualStyle();
			}
		}

		public MyGuiSliderProperties Propeties
		{
			get
			{
				return m_props;
			}
			set
			{
				m_props = value;
				Ratio = m_ratio;
			}
		}

		public float Ratio
		{
			get
			{
				return m_ratio;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, 1f);
				if (m_ratio != value)
				{
					m_ratio = m_props.RatioFilter(value);
					UpdateLabel();
					OnValueChange();
				}
			}
		}

		protected virtual float MinimumStep => 0f;

		public float DebugScale
		{
			get
			{
				return m_debugScale;
			}
			set
			{
				if (m_debugScale != value)
				{
					m_debugScale = value;
					RefreshInternals();
				}
			}
		}

		public float Value
		{
			get
			{
				return m_props.RatioToValue(m_ratio);
			}
			set
			{
				float arg = m_props.ValueToRatio(value);
				arg = m_props.RatioFilter(arg);
				arg = MathHelper.Clamp(arg, 0f, 1f);
				if (arg != m_ratio)
				{
					m_ratio = arg;
					UpdateLabel();
					OnValueChange();
				}
			}
		}

		static MyGuiControlSliderBase()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlSliderStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				RailTexture = MyGuiConstants.TEXTURE_SLIDER_RAIL,
				RailHighlightTexture = MyGuiConstants.TEXTURE_SLIDER_RAIL_HIGHLIGHT,
				ThumbTexture = MyGuiConstants.TEXTURE_SLIDER_THUMB_DEFAULT
			};
			m_styles[1] = new StyleDefinition
			{
				RailTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_RAIL,
				RailHighlightTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_RAIL_HIGHLIGHT,
				ThumbTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_THUMB_DEFAULT
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlSliderStyleEnum style)
		{
			return m_styles[(int)style];
		}

		protected virtual void OnValueChange()
		{
			ValueChanged.InvokeIfNotNull(this);
		}

		public MyGuiControlSliderBase(Vector2? position = null, float width = 0.29f, MyGuiSliderProperties props = null, float? defaultRatio = null, Vector4? color = null, float labelScale = 0.8f, float labelSpaceWidth = 0f, string labelFont = "White", string toolTip = null, MyGuiControlSliderStyleEnum visualStyle = MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, bool showLabel = true)
			: base(position, null, null, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_ACTIVE, originAlign)
		{
			this.showLabel = showLabel;
			if (defaultRatio.HasValue)
			{
				defaultRatio = MathHelper.Clamp(defaultRatio.Value, 0f, 1f);
			}
			if (props == null)
			{
				props = MyGuiSliderProperties.Default;
			}
			m_props = props;
			DefaultRatio = defaultRatio;
			m_ratio = (defaultRatio.HasValue ? defaultRatio.Value : 0f);
			m_labelSpaceWidth = labelSpaceWidth;
			m_label = new MyGuiControlLabel(null, null, string.Empty, null, labelScale, labelFont, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			if (showLabel)
			{
				Elements.Add(m_label);
			}
			VisualStyle = visualStyle;
			base.Size = new Vector2(width, base.Size.Y);
			UpdateLabel();
		}

		public override void OnRemoving()
		{
			SliderClicked = null;
			ValueChanged = null;
			base.OnRemoving();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase ret = base.HandleInput();
			if (ret != null)
			{
				return ret;
			}
			if (!base.Enabled)
			{
				return null;
			}
			if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed() && !OnSliderClicked())
			{
				m_controlCaptured = true;
			}
			if (MyInput.Static.IsNewPrimaryButtonReleased())
			{
				m_controlCaptured = false;
			}
			if (base.IsMouseOver)
			{
				if (m_controlCaptured)
				{
					float start = GetStart();
					float end = GetEnd();
					Ratio = (MyGuiManager.MouseCursorPosition.X - start) / (end - start);
					ret = this;
				}
				else if (MyInput.Static.IsNewSecondaryButtonPressed() && DefaultRatio.HasValue)
				{
					Ratio = DefaultRatio.Value;
					ret = this;
				}
			}
			else if (m_controlCaptured)
			{
				ret = this;
			}
			if (base.HasFocus && MyGuiManager.TotalTimeInMilliseconds - m_lastTimeArrowPressed > MyGuiConstants.REPEAT_PRESS_DELAY)
			{
				if (MyInput.Static.IsKeyPress(MyKeys.Left) || MyInput.Static.IsGamepadKeyLeftPressed())
				{
					MoveSlider(direction: false);
				}
				else if (MyInput.Static.IsKeyPress(MyKeys.Right) || MyInput.Static.IsGamepadKeyRightPressed())
				{
					MoveSlider(direction: true);
				}
				else
				{
					m_consecutiveSliderMoves = 0f;
				}
			}
			return ret;
			void MoveSlider(bool direction)
			{
				if (direction != m_lastMoveDirection)
				{
					m_consecutiveSliderMoves = 0f;
				}
				m_consecutiveSliderMoves += 1f;
				m_lastMoveDirection = direction;
				if (m_consecutiveSliderMoves < 5f)
				{
					m_lastTimeArrowPressed = MyGuiManager.TotalTimeInMilliseconds;
				}
				Ratio += Math.Max(m_consecutiveSliderMoves / 3f * 0.0001f, MinimumStep) * (float)(direction ? 1 : (-1));
				ret = this;
			}
		}

		protected virtual bool OnSliderClicked()
		{
			if (SliderClicked != null)
			{
				return SliderClicked(this);
			}
			return false;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_railTexture.Draw(GetPositionAbsoluteTopLeft(), base.Size - new Vector2(m_labelSpaceWidth, 0f), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), DebugScale);
			DrawThumb(transitionAlpha);
			if (showLabel)
			{
				m_label.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		private void DrawThumb(float transitionAlpha)
		{
			float y = GetPositionAbsoluteTopLeft().Y + base.Size.Y / 2f;
			float start = GetStart();
			float end = GetEnd();
			float x = MathHelper.Lerp(start, end, m_ratio);
			MyGuiManager.DrawSpriteBatch(m_thumbTexture, new Vector2(x, y), m_styleDef.ThumbTexture.SizeGui * ((DebugScale != 1f) ? (DebugScale * 0.5f) : DebugScale), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}

		private float GetStart()
		{
			return GetPositionAbsoluteTopLeft().X + MyGuiConstants.SLIDER_INSIDE_OFFSET_X;
		}

		private float GetEnd()
		{
			return GetPositionAbsoluteTopLeft().X + (base.Size.X - (MyGuiConstants.SLIDER_INSIDE_OFFSET_X + m_labelSpaceWidth));
		}

		protected void UpdateLabel()
		{
			m_label.Text = m_props.FormatLabel(Value);
			RefreshInternals();
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (m_styleDef == null)
			{
				m_styleDef = m_styles[0];
			}
			if (base.HasHighlight)
			{
				m_railTexture = m_styleDef.RailHighlightTexture;
				m_thumbTexture = m_styleDef.ThumbTexture.Highlight;
			}
			else
			{
				m_railTexture = m_styleDef.RailTexture;
				m_thumbTexture = m_styleDef.ThumbTexture.Normal;
			}
			base.MinSize = new Vector2(m_railTexture.MinSizeGui.X + m_labelSpaceWidth, Math.Max(m_railTexture.MinSizeGui.Y, m_label.Size.Y)) * DebugScale;
			base.MaxSize = new Vector2(m_railTexture.MaxSizeGui.X + m_labelSpaceWidth, Math.Max(m_railTexture.MaxSizeGui.Y, m_label.Size.Y)) * DebugScale;
			m_label.Position = new Vector2(base.Size.X * 0.5f, 0f);
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDef = style;
				RefreshInternals();
			}
		}
	}
}
