using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlLabel))]
	public class MyGuiControlLabel : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public string Font = "Blue";

			public Vector4 ColorMask = Vector4.One;

			public float TextScale = 0.8f;
		}

		private StyleDefinition m_styleDefinition;

		private bool m_forceNewStringBuilder;

		private string m_font;

		private string m_text;

		private MyStringId m_textEnum;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		public StringBuilder TextToDraw;

		public bool AutoEllipsis;

		public string Font
		{
			get
			{
				return m_font;
			}
			set
			{
				m_font = value;
			}
		}

		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (m_text != value)
				{
					m_text = value;
					UpdateFormatParams(null);
				}
			}
		}

		public MyStringId TextEnum
		{
			get
			{
				return m_textEnum;
			}
			set
			{
				if (m_textEnum != value || m_text != null)
				{
					m_textEnum = value;
					m_text = null;
					UpdateFormatParams(null);
				}
			}
		}

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			set
			{
				if (m_textScale != value)
				{
					m_textScale = value;
					TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
					RecalculateSize();
				}
			}
		}

		public float TextScaleWithLanguage
		{
			get
			{
				return m_textScaleWithLanguage;
			}
			private set
			{
				m_textScaleWithLanguage = value;
			}
		}

		private StringBuilder TextForDraw => TextToDraw ?? MyTexts.Get(m_textEnum);

		public bool UseTextShadow
		{
			get;
			set;
		}

		public MyGuiControlLabel()
			: this(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
		{
		}

		public MyGuiControlLabel(Vector2? position = null, Vector2? size = null, string text = null, Vector4? colorMask = null, float textScale = 0.8f, string font = "Blue", MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			: base(position, size, colorMask, null, null, isActiveControl: false)
		{
			base.Name = "Label";
			Font = font;
			if (text != null)
			{
				m_text = text;
				TextToDraw = new StringBuilder(text);
			}
			base.OriginAlign = originAlign;
			TextScale = textScale;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlLabel myObjectBuilder_GuiControlLabel = (MyObjectBuilder_GuiControlLabel)objectBuilder;
			m_textEnum = MyStringId.GetOrCompute(myObjectBuilder_GuiControlLabel.TextEnum);
			TextScale = myObjectBuilder_GuiControlLabel.TextScale;
			m_text = (string.IsNullOrWhiteSpace(myObjectBuilder_GuiControlLabel.Text) ? null : myObjectBuilder_GuiControlLabel.Text);
			Font = myObjectBuilder_GuiControlLabel.Font;
			TextToDraw = new StringBuilder();
			UpdateFormatParams(null);
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlLabel obj = (MyObjectBuilder_GuiControlLabel)base.GetObjectBuilder();
			obj.TextEnum = m_textEnum.ToString();
			obj.TextScale = TextScale;
			obj.Text = m_text;
			obj.Font = Font;
			return obj;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			float maxTextWidth = AutoEllipsis ? base.Size.X : float.PositiveInfinity;
			if (TextForDraw == null)
			{
				MyLog.Default.WriteLine("text shouldn't be null! MyGuiContolLabel:" + this);
				return;
			}
			if (UseTextShadow)
			{
				Vector2 textSize = GetTextSize();
				Vector2 position = GetPositionAbsoluteTopLeft();
				MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, 1f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			MyGuiManager.DrawString(Font, TextForDraw, GetPositionAbsolute(), TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), base.OriginAlign, useFullClientArea: false, maxTextWidth);
		}

		public void Autowrap(float width)
		{
			if (TextToDraw != null)
			{
				TextToDraw.Autowrap(width, Font, TextScaleWithLanguage);
			}
		}

		public Vector2 GetTextSize()
		{
			return MyGuiManager.MeasureString(Font, TextForDraw, TextScaleWithLanguage);
		}

		public void UpdateFormatParams(params object[] args)
		{
			if (m_text == null)
			{
				if (TextToDraw == null || m_forceNewStringBuilder)
				{
					TextToDraw = new StringBuilder();
				}
				TextToDraw.Clear();
				if (args != null)
				{
					TextToDraw.AppendFormat(MyTexts.GetString(m_textEnum), args);
				}
				else
				{
					TextToDraw.Append(MyTexts.GetString(m_textEnum));
				}
			}
			else
			{
				if (TextToDraw == null || m_forceNewStringBuilder)
				{
					TextToDraw = new StringBuilder();
				}
				TextToDraw.Clear();
				if (args != null)
				{
					TextToDraw.AppendFormat(m_text.ToString(), args);
				}
				else
				{
					TextToDraw.Append(m_text);
				}
			}
			m_forceNewStringBuilder = false;
			RecalculateSize();
		}

		public void PrepareForAsyncTextUpdate()
		{
			m_forceNewStringBuilder = true;
		}

		public void RecalculateSize()
		{
			RefreshInternals();
			base.Size = GetTextSize();
		}

		public void RefreshInternals()
		{
			if (m_styleDefinition != null)
			{
				Font = m_styleDefinition.Font;
				base.ColorMask = m_styleDefinition.ColorMask;
				TextScale = m_styleDefinition.TextScale;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDefinition = style;
				RefreshInternals();
			}
		}
	}
}
