using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlMultilineLabel))]
	public class MyGuiControlMultilineText : MyGuiControlBase
	{
		protected class MyGuiControlMultilineSelection
		{
			protected int m_startIndex;

			protected int m_endIndex;

			private string ClipboardText;

			private bool m_dragging;

			public bool Dragging
			{
				get
				{
					return m_dragging;
				}
				set
				{
					m_dragging = value;
				}
			}

			public int Start => Math.Min(m_startIndex, m_endIndex);

			public int End => Math.Max(m_startIndex, m_endIndex);

			public int Length => End - Start;

			public MyGuiControlMultilineSelection()
			{
				m_startIndex = 0;
				m_endIndex = 0;
			}

			public void SetEnd(MyGuiControlMultilineText sender)
			{
				m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.Text.Length);
			}

			public void Reset(MyGuiControlMultilineText sender)
			{
				m_startIndex = (m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.Text.Length));
			}

			public void SelectAll(MyGuiControlMultilineText sender)
			{
				m_startIndex = 0;
				m_endIndex = sender.Text.Length;
				sender.CarriagePositionIndex = sender.Text.Length;
			}

			public void EraseText(MyGuiControlMultilineText sender)
			{
				if (Start != End)
				{
					StringBuilder stringBuilder = new StringBuilder(sender.Text.ToString().Substring(0, Start));
					StringBuilder value = new StringBuilder(sender.Text.ToString().Substring(End));
					sender.CarriagePositionIndex = Start;
					sender.Text = stringBuilder.Append((object)value);
				}
			}

			public void CopyText(MyGuiControlMultilineText sender)
			{
				ClipboardText = Regex.Replace(sender.Text.ToString().Substring(Start, Length), "\n", "\r\n");
				if (!string.IsNullOrEmpty(ClipboardText))
				{
					MyVRage.Platform.Clipboard = ClipboardText;
				}
			}

			public void CutText(MyGuiControlMultilineText sender)
			{
				CopyText(sender);
				EraseText(sender);
			}

			public void PasteText(MyGuiControlMultilineText sender)
			{
				EraseText(sender);
				string text = sender.Text.ToString().Substring(0, sender.CarriagePositionIndex);
				string value = sender.Text.ToString().Substring(sender.CarriagePositionIndex);
				Thread thread = new Thread(PasteFromClipboard);
				thread.ApartmentState = ApartmentState.STA;
				thread.Start();
				thread.Join();
				sender.Text = new StringBuilder(text).Append(Regex.Replace(ClipboardText, "\r\n", "\n")).Append(value);
				sender.CarriagePositionIndex = text.Length + ClipboardText.Length;
				Reset(sender);
			}

			private void PasteFromClipboard()
			{
				string clipboard = MyVRage.Platform.Clipboard;
				for (int i = 0; i < clipboard.Length; i++)
				{
					if (!XmlConvert.IsXmlChar(clipboard[i]))
					{
						ClipboardText = string.Empty;
						return;
					}
				}
				ClipboardText = clipboard;
			}
		}

		protected MyGuiBorderThickness m_textPadding;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		private static readonly StringBuilder m_letterA = new StringBuilder("A");

		private static readonly StringBuilder m_lineHeightMeasure = new StringBuilder("Ajqypdbfgjl");

		protected readonly StringBuilder m_tmpOffsetMeasure = new StringBuilder();

		protected readonly MyVScrollbar m_scrollbarV;

		protected readonly MyHScrollbar m_scrollbarH;

		private Vector2 m_scrollbarSizeV;

		private Vector2 m_scrollbarSizeH;

		protected MyRichLabel m_label;

		private bool m_drawScrollbarV;

		private bool m_drawScrollbarH;

		private float m_scrollbarOffsetV;

		private float m_scrollbarOffsetH;

		private bool m_showTextShadow;

		private bool m_selectable;

		protected MyKeyThrottler m_keyThrottler;

		protected int m_carriageBlinkerTimer;

		protected int m_carriagePositionIndex;

		protected MyGuiControlMultilineSelection m_selection;

		protected StringBuilder m_text;

		private MyStringId m_textEnum;

		private bool m_useEnum = true;

		private bool m_isImeActive;

		private string m_font;

		protected int CarriagePositionIndex
		{
			get
			{
				return m_carriagePositionIndex;
			}
			set
			{
				int num = MathHelper.Clamp(value, 0, Text.Length);
				if (m_carriagePositionIndex != num)
				{
					m_carriagePositionIndex = num;
					if (!CarriageVisible())
					{
						ScrollToShowCarriage();
					}
				}
			}
		}

		public bool Selectable => m_selectable;

		public virtual StringBuilder Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_text.Clear();
				if (value != null)
				{
					m_text.AppendStringBuilder(value);
				}
				RefreshText(useEnum: false);
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
				m_textEnum = value;
				RefreshText(useEnum: true);
			}
		}

		public bool IsImeActive
		{
			get
			{
				return m_isImeActive;
			}
			set
			{
				m_isImeActive = value;
			}
		}

		public string Font
		{
			get
			{
				return m_font;
			}
			set
			{
				if (m_font != value)
				{
					m_font = value;
					RefreshText(m_useEnum);
				}
			}
		}

		public Color TextColor
		{
			get;
			set;
		}

		public Vector2 TextSize => m_label.Size;

		public Vector2 TextSizeWithScrolling
		{
			get
			{
				Vector2 size = base.Size;
				if (m_scrollbarV.Visible)
				{
					size.X -= m_scrollbarV.Size.X;
				}
				if (m_scrollbarH.Visible)
				{
					size.Y -= m_scrollbarH.Size.Y;
				}
				return size;
			}
		}

		public int NumberOfRows => m_label.NumberOfRows;

		public MyGuiBorderThickness TextPadding
		{
			get
			{
				return m_textPadding;
			}
			set
			{
				m_textPadding = value;
				RecalculateScrollBar();
			}
		}

		public int CharactersDisplayed
		{
			get
			{
				return m_label.CharactersDisplayed;
			}
			set
			{
				m_label.CharactersDisplayed = value;
			}
		}

		public float ScrollbarOffsetV
		{
			get
			{
				return m_scrollbarOffsetV;
			}
			set
			{
				m_scrollbarOffsetV = value;
				m_scrollbarV.ChangeValue(m_scrollbarOffsetV);
				RecalculateScrollBar();
			}
		}

		public float ScrollbarOffsetH
		{
			get
			{
				return m_scrollbarOffsetH;
			}
			set
			{
				m_scrollbarOffsetH = value;
				m_scrollbarH.ChangeValue(m_scrollbarOffsetH);
				RecalculateScrollBar();
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
				m_textScale = value;
				TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
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

		public bool ShowTextShadow => m_showTextShadow;

		public MyGuiDrawAlignEnum TextAlign
		{
			get
			{
				return m_label.TextAlign;
			}
			set
			{
				m_label.TextAlign = value;
			}
		}

		public MyGuiDrawAlignEnum TextBoxAlign
		{
			get;
			set;
		}

		protected float ScrollbarValueV => m_scrollbarV.Value;

		protected float ScrollbarValueH => m_scrollbarH.Value;

		protected float SetScrollbarValueV
		{
			set
			{
				m_scrollbarV.Value = value;
			}
		}

		protected float SetScrollbarValueH
		{
			set
			{
				m_scrollbarH.Value = value;
			}
		}

		public event LinkClicked OnLinkClicked;

		public void DeactivateIme()
		{
			m_isImeActive = false;
		}

		public void SetScrollbarPageV(float page)
		{
			m_scrollbarOffsetV = 0f;
			m_scrollbarV.SetPage(page);
			RecalculateScrollBar();
		}

		public void SetScrollbarPageH(float page)
		{
			m_scrollbarOffsetH = 0f;
			m_scrollbarH.SetPage(page);
			RecalculateScrollBar();
		}

		public MyGuiControlMultilineText()
			: this(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
		{
		}

		public MyGuiControlMultilineText(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string font = "Blue", float textScale = 0.8f, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, StringBuilder contents = null, bool drawScrollbarV = true, bool drawScrollbarH = true, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int? visibleLinesCount = null, bool selectable = false, bool showTextShadow = false, MyGuiCompositeTexture backgroundTexture = null, MyGuiBorderThickness? textPadding = null)
			: base(position, size, backgroundColor, null, backgroundTexture)
		{
			Font = font;
			TextScale = textScale;
			m_drawScrollbarV = drawScrollbarV;
			m_drawScrollbarH = drawScrollbarH;
			TextColor = new Color(Vector4.One);
			TextBoxAlign = textBoxAlign;
			m_selectable = selectable;
			m_textPadding = (textPadding ?? new MyGuiBorderThickness(0f, 0f, 0f, 0f));
			m_scrollbarV = new MyVScrollbar(this);
			m_scrollbarSizeV = new Vector2(0.0334f, MyGuiConstants.COMBOBOX_VSCROLLBAR_SIZE.Y);
			m_scrollbarSizeV = MyGuiConstants.COMBOBOX_VSCROLLBAR_SIZE;
			m_scrollbarH = new MyHScrollbar(this);
			m_scrollbarSizeH = new Vector2(MyGuiConstants.COMBOBOX_HSCROLLBAR_SIZE.X, 0.0334f);
			m_scrollbarSizeH = MyGuiConstants.COMBOBOX_HSCROLLBAR_SIZE;
			float y = MyGuiManager.MeasureString(Font, m_lineHeightMeasure, TextScaleWithLanguage).Y;
			m_label = new MyRichLabel(this, ComputeRichLabelWidth(), y, visibleLinesCount)
			{
				ShowTextShadow = showTextShadow
			};
			m_label.AdjustingScissorRectangle += AdjustScissorRectangleLabel;
			m_label.TextAlign = textAlign;
			m_label.CharactersDisplayed = -1;
			m_text = new StringBuilder();
			m_selection = new MyGuiControlMultilineSelection();
			if (contents != null && contents.Length > 0)
			{
				Text = contents;
			}
			m_keyThrottler = new MyKeyThrottler();
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			m_label.MaxLineWidth = ComputeRichLabelWidth();
			MyObjectBuilder_GuiControlMultilineLabel myObjectBuilder_GuiControlMultilineLabel = (MyObjectBuilder_GuiControlMultilineLabel)objectBuilder;
			TextAlign = (MyGuiDrawAlignEnum)myObjectBuilder_GuiControlMultilineLabel.TextAlign;
			TextBoxAlign = (MyGuiDrawAlignEnum)myObjectBuilder_GuiControlMultilineLabel.TextBoxAlign;
			TextScale = myObjectBuilder_GuiControlMultilineLabel.TextScale;
			TextColor = new Color(myObjectBuilder_GuiControlMultilineLabel.TextColor);
			Font = myObjectBuilder_GuiControlMultilineLabel.Font;
			if (Enum.TryParse(myObjectBuilder_GuiControlMultilineLabel.Text, out MyStringId result))
			{
				TextEnum = result;
			}
			else
			{
				Text = new StringBuilder(myObjectBuilder_GuiControlMultilineLabel.Text);
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlMultilineLabel myObjectBuilder_GuiControlMultilineLabel = (MyObjectBuilder_GuiControlMultilineLabel)base.GetObjectBuilder();
			myObjectBuilder_GuiControlMultilineLabel.TextScale = TextScale;
			myObjectBuilder_GuiControlMultilineLabel.TextColor = TextColor.ToVector4();
			myObjectBuilder_GuiControlMultilineLabel.TextAlign = (int)TextAlign;
			myObjectBuilder_GuiControlMultilineLabel.TextBoxAlign = (int)TextBoxAlign;
			myObjectBuilder_GuiControlMultilineLabel.Font = Font;
			if (m_useEnum)
			{
				myObjectBuilder_GuiControlMultilineLabel.Text = TextEnum.ToString();
			}
			else
			{
				myObjectBuilder_GuiControlMultilineLabel.Text = Text.ToString();
			}
			return myObjectBuilder_GuiControlMultilineLabel;
		}

		public void RefreshText(bool useEnum)
		{
			if (m_label != null)
			{
				m_label.Clear();
				m_useEnum = useEnum;
				if (useEnum)
				{
					AppendText(MyTexts.Get(TextEnum));
				}
				else
				{
					AppendText(Text);
				}
				if (Text.Length < CarriagePositionIndex)
				{
					CarriagePositionIndex = Text.Length;
				}
				m_selection.Reset(this);
			}
		}

		public void AppendText(StringBuilder text)
		{
			AppendText(text, Font, TextScaleWithLanguage, TextColor.ToVector4());
		}

		public void AppendText(StringBuilder text, string font, float scale, Vector4 color)
		{
			m_label.Append(text, font, scale, color);
			RecalculateScrollBar();
		}

		public void AppendText(string text)
		{
			AppendText(text, Font, TextScaleWithLanguage, TextColor.ToVector4());
		}

		public void AppendText(string text, string font, float scale, Vector4 color)
		{
			m_label.Append(text, font, scale, color);
			m_useEnum = false;
			RecalculateScrollBar();
		}

		public void AppendImage(string texture, Vector2 size, Vector4 color)
		{
			m_label.Append(texture, size, color);
			m_useEnum = false;
			RecalculateScrollBar();
		}

		public void AppendLink(string url, string text)
		{
			m_label.AppendLink(url, text, TextScaleWithLanguage, OnLinkClickedInternal);
			m_useEnum = false;
			RecalculateScrollBar();
		}

		private void OnLinkClickedInternal(string url)
		{
			if (this.OnLinkClicked != null)
			{
				this.OnLinkClicked(this, url);
			}
		}

		public void AppendLine()
		{
			m_label.AppendLine();
			RecalculateScrollBar();
		}

		public new void Clear()
		{
			m_label.Clear();
			m_scrollbarV.SetPage(0f);
			m_scrollbarH.SetPage(0f);
			RecalculateScrollBar();
		}

		public void RecalculateScrollBar()
		{
			float y = m_label.Size.Y;
			bool flag = base.Size.Y - m_textPadding.SizeChange.Y < y;
			float x = m_label.Size.X;
			bool flag2 = base.Size.X - m_textPadding.SizeChange.X < x;
			m_scrollbarV.Visible = flag;
			m_scrollbarV.Init(y, base.Size.Y - (flag2 ? m_scrollbarH.Size.Y : 0f) - m_textPadding.SizeChange.Y);
			m_scrollbarV.Layout(new Vector2(0.5f * base.Size.X - m_scrollbarV.Size.X, -0.5f * base.Size.Y), flag2 ? (base.Size.Y - m_scrollbarH.Size.Y) : base.Size.Y);
			if (!m_drawScrollbarV)
			{
				if (TextAlign == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM)
				{
					m_scrollbarV.Value = 0f;
				}
				else if (TextAlign == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
				{
					m_scrollbarV.Value = y;
				}
			}
			m_scrollbarH.Visible = flag2;
			m_scrollbarH.Init(x, base.Size.X - (flag ? m_scrollbarV.Size.X : 0f) - m_textPadding.SizeChange.X);
			m_scrollbarH.Layout(new Vector2(-0.5f * base.Size.X, 0.5f * base.Size.Y - m_scrollbarH.Size.Y), base.Size.X - m_scrollbarV.Size.X);
			if (!m_drawScrollbarH)
			{
				if (TextAlign == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM)
				{
					m_scrollbarH.Value = 0f;
				}
				else if (TextAlign == MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER || TextAlign == MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM)
				{
					m_scrollbarH.Value = x;
				}
			}
		}

		protected virtual void DrawSelectionBackgrounds(MyRectangle2D textArea, float transitionAlpha)
		{
			string[] array = Text.ToString().Substring(m_selection.Start, m_selection.Length).Split(new char[1]
			{
				'\n'
			});
			int num = m_selection.Start;
			string[] array2 = array;
			foreach (string text in array2)
			{
				Vector2 normalizedCoord = textArea.LeftTop + GetCarriageOffset(num);
				Vector2 vector = GetCarriageOffset(num + text.Length) - GetCarriageOffset(num);
				Vector2 normalizedSize = new Vector2(vector.X, GetCarriageHeight());
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(new Vector4(1f, 1f, 1f, 0.5f), base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				num += text.Length + 1;
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			MyRectangle2D textArea = new MyRectangle2D(m_textPadding.TopLeftOffset, base.Size - m_textPadding.SizeChange);
			textArea.LeftTop += GetPositionAbsoluteTopLeft();
			Vector2 carriageOffset = GetCarriageOffset(CarriagePositionIndex);
			RectangleF rectangle = new RectangleF(textArea.LeftTop, textArea.Size);
			rectangle.X -= 0.001f;
			rectangle.Y -= 0.001f;
			AdjustScissorRectangle(ref rectangle);
			using (MyGuiManager.UsingScissorRectangle(ref rectangle))
			{
				DrawSelectionBackgrounds(textArea, backgroundTransitionAlpha);
				DrawText(m_scrollbarV.Value, m_scrollbarH.Value, transitionAlpha);
				if (base.HasFocus && Selectable)
				{
					int num = m_carriageBlinkerTimer % 60;
					if (num >= 0 && num <= 45)
					{
						MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", textArea.LeftTop + carriageOffset, 1, GetCarriageHeight(), MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
				}
				m_carriageBlinkerTimer++;
			}
			if (m_drawScrollbarV)
			{
				m_scrollbarV.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			}
			if (m_drawScrollbarH)
			{
				m_scrollbarH.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			}
		}

		private void AdjustScissorRectangle(ref RectangleF rectangle)
		{
		}

		private void AdjustScissorRectangleLabel(ref RectangleF rectangle)
		{
		}

		private void AdjustScissorRectangle(ref RectangleF rectangle, float multWidth, float multHeight)
		{
			float width = rectangle.Width;
			float height = rectangle.Height;
			rectangle.Width *= multWidth;
			rectangle.Height *= multHeight;
			float num = rectangle.Width - width;
			float num2 = rectangle.Height - height;
			rectangle.Position.X -= num / 2f;
			rectangle.Position.Y -= num2 / 2f;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase result = base.HandleInput();
			if (base.Enabled)
			{
				if (base.HasFocus && Selectable)
				{
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Left))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						DelayCaretBlink();
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						DelayCaretBlink();
						if (!IsImeActive)
						{
							if (MyInput.Static.IsAnyCtrlKeyPressed())
							{
								CarriagePositionIndex = GetPreviousSpace();
							}
							else
							{
								CarriagePositionIndex--;
							}
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
						}
						return this;
					}
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Right))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						DelayCaretBlink();
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						DelayCaretBlink();
						if (!IsImeActive)
						{
							if (!MyInput.Static.IsAnyCtrlKeyPressed())
							{
								int num = ++CarriagePositionIndex;
							}
							else
							{
								CarriagePositionIndex = GetNextSpace();
							}
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
						}
						return this;
					}
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Down))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						DelayCaretBlink();
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						DelayCaretBlink();
						if (!IsImeActive)
						{
							CarriagePositionIndex = GetIndexUnderCarriage(CarriagePositionIndex);
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
						}
						return this;
					}
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Up))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						DelayCaretBlink();
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						DelayCaretBlink();
						if (!IsImeActive)
						{
							CarriagePositionIndex = GetIndexOverCarriage(CarriagePositionIndex);
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
						}
						return this;
					}
					if (!IsImeActive)
					{
						if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.C) && MyInput.Static.IsAnyCtrlKeyPressed())
						{
							m_selection.CopyText(this);
						}
						if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.A) && MyInput.Static.IsAnyCtrlKeyPressed())
						{
							m_selection.SelectAll(this);
							return this;
						}
					}
				}
				bool flag = false;
				int num2 = MyInput.Static.DeltaMouseScrollWheelValue();
				if (base.IsMouseOver && num2 != 0 && (m_scrollbarV.Visible || m_scrollbarH.Visible))
				{
					m_scrollbarV.ChangeValue(-0.0005f * (float)num2);
					flag = true;
				}
				if (m_drawScrollbarV && (m_scrollbarV.HandleInput() || flag))
				{
					return this;
				}
				if (m_drawScrollbarH && (m_scrollbarH.HandleInput() || flag))
				{
					return this;
				}
				if (base.IsMouseOver && m_label.HandleInput(GetPositionAbsoluteTopLeft(), m_scrollbarV.Value, m_scrollbarH.Value))
				{
					return this;
				}
				if (Selectable)
				{
					if (MyInput.Static.IsNewLeftMousePressed())
					{
						if (base.IsMouseOver)
						{
							m_selection.Dragging = true;
							CarriagePositionIndex = GetCarriagePositionFromMouseCursor();
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
							return this;
						}
						m_selection.Reset(this);
					}
					else if (MyInput.Static.IsNewLeftMouseReleased())
					{
						m_selection.Dragging = false;
					}
					else if (m_selection.Dragging)
					{
						if (base.IsMouseOver)
						{
							CarriagePositionIndex = GetCarriagePositionFromMouseCursor();
							m_selection.SetEnd(this);
						}
						else if (base.HasFocus)
						{
							Vector2 mouseCursorPosition = MyGuiManager.MouseCursorPosition;
							Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
							if (mouseCursorPosition.Y < positionAbsoluteTopLeft.Y)
							{
								m_scrollbarV.ChangeValue(base.Position.Y - mouseCursorPosition.Y);
							}
							else if (mouseCursorPosition.Y > positionAbsoluteTopLeft.Y + base.Size.Y)
							{
								m_scrollbarV.ChangeValue(mouseCursorPosition.Y - positionAbsoluteTopLeft.Y - base.Size.Y);
							}
							if (mouseCursorPosition.X < positionAbsoluteTopLeft.X)
							{
								m_scrollbarH.ChangeValue(base.Position.X - mouseCursorPosition.X);
							}
							else if (mouseCursorPosition.X > positionAbsoluteTopLeft.X + base.Size.X)
							{
								m_scrollbarH.ChangeValue(mouseCursorPosition.X - positionAbsoluteTopLeft.X - base.Size.X);
							}
						}
					}
				}
			}
			return result;
		}

		protected void DelayCaretBlink()
		{
			m_carriageBlinkerTimer = 0;
		}

		private void DrawText(float offsetY, float offsetX, float alphamask)
		{
			Vector2 position = GetPositionAbsoluteTopLeft() + m_textPadding.TopLeftOffset;
			Vector2 drawSizeMax = base.Size - m_textPadding.SizeChange;
			if (m_drawScrollbarV && m_scrollbarV.Visible)
			{
				drawSizeMax.X -= m_scrollbarV.Size.X;
			}
			if (m_drawScrollbarH && m_scrollbarH.Visible)
			{
				drawSizeMax.Y -= m_scrollbarH.Size.Y;
			}
			Vector2 textSize = TextSize;
			if (textSize.X < drawSizeMax.X)
			{
				switch (TextBoxAlign)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
					position.X += (drawSizeMax.X - textSize.X) * 0.5f;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					position.X += drawSizeMax.X - textSize.X;
					break;
				}
				drawSizeMax.X = textSize.X;
			}
			if (textSize.Y < drawSizeMax.Y)
			{
				switch (TextBoxAlign)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
					position.Y += (drawSizeMax.Y - textSize.Y) * 0.5f;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					position.Y += drawSizeMax.Y - textSize.Y;
					break;
				}
				drawSizeMax.Y = textSize.Y;
			}
			m_label.Draw(position, offsetY, offsetX, drawSizeMax, alphamask);
		}

		protected override void OnSizeChanged()
		{
			if (m_label != null)
			{
				m_label.MaxLineWidth = ComputeRichLabelWidth();
				RefreshText(m_useEnum);
			}
			if (m_drawScrollbarV || m_drawScrollbarH)
			{
				RecalculateScrollBar();
			}
			base.OnSizeChanged();
		}

		protected virtual float ComputeRichLabelWidth()
		{
			float num = base.Size.X - MyGuiConstants.MULTILINE_LABEL_BORDER.X;
			if (m_drawScrollbarV)
			{
				num -= m_scrollbarSizeV.X;
			}
			return num;
		}

		private bool CarriageVisible()
		{
			Vector2 carriageOffset = GetCarriageOffset(CarriagePositionIndex);
			float carriageHeight = GetCarriageHeight();
			if (carriageOffset.Y < 0f || carriageOffset.X < 0f)
			{
				return false;
			}
			Vector2 textSizeWithScrolling = TextSizeWithScrolling;
			if (carriageOffset.X <= textSizeWithScrolling.X)
			{
				return carriageOffset.Y + carriageHeight <= textSizeWithScrolling.Y;
			}
			return false;
		}

		protected virtual int GetCarriagePositionFromMouseCursor()
		{
			Vector2 value = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft() - m_textPadding.TopLeftOffset;
			int result = 0;
			float num = float.MaxValue;
			for (int i = 0; i <= m_text.Length; i++)
			{
				float num2 = Vector2.Distance(GetCarriageOffset(i), value);
				if (num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}

		protected virtual Vector2 GetCarriageOffset(int idx)
		{
			Vector2 result = new Vector2(0f - m_scrollbarH.Value, 0f - m_scrollbarV.Value) + m_textPadding.TopLeftOffset;
			int lineStartIndex = GetLineStartIndex(idx);
			if (idx - lineStartIndex > 0)
			{
				m_tmpOffsetMeasure.Clear();
				m_tmpOffsetMeasure.AppendSubstring(Text, lineStartIndex, idx - lineStartIndex);
				result.X = MyGuiManager.MeasureString(Font, m_tmpOffsetMeasure, TextScaleWithLanguage).X - m_scrollbarH.Value;
			}
			if (lineStartIndex - 1 > 0)
			{
				m_tmpOffsetMeasure.Clear();
				m_tmpOffsetMeasure.AppendSubstring(Text, 0, lineStartIndex - 1);
				result.Y = MyGuiManager.MeasureString(Font, m_tmpOffsetMeasure, TextScaleWithLanguage).Y - m_scrollbarV.Value;
			}
			return result;
		}

		protected float GetCarriageHeight()
		{
			return MyGuiManager.MeasureString(Font, m_letterA, TextScaleWithLanguage).Y;
		}

		public void ScrollToShowCarriage()
		{
			Vector2 carriageOffset = GetCarriageOffset(CarriagePositionIndex);
			float carriageHeight = GetCarriageHeight();
			Vector2 textSizeWithScrolling = TextSizeWithScrolling;
			if (carriageOffset.Y + carriageHeight > textSizeWithScrolling.Y - 0.01f)
			{
				m_scrollbarV.ChangeValue(carriageOffset.Y + carriageHeight - textSizeWithScrolling.Y);
			}
			if (carriageOffset.Y < 0f)
			{
				m_scrollbarV.ChangeValue(carriageOffset.Y);
			}
			if (carriageOffset.X > textSizeWithScrolling.X - 0.01f)
			{
				m_scrollbarH.ChangeValue(carriageOffset.X - textSizeWithScrolling.X);
			}
			if (carriageOffset.X < 0f)
			{
				m_scrollbarH.ChangeValue(carriageOffset.X);
			}
		}

		protected virtual int GetLineStartIndex(int idx)
		{
			int num = Text.ToString().Substring(0, idx).LastIndexOf('\n');
			if (num != -1)
			{
				return num;
			}
			return 0;
		}

		protected virtual int GetLineEndIndex(int idx)
		{
			if (idx == Text.Length)
			{
				return Text.Length;
			}
			int num = Text.ToString().Substring(idx).IndexOf('\n');
			if (num != -1)
			{
				return idx + num;
			}
			return Text.Length;
		}

		protected virtual int GetIndexUnderCarriage(int idx)
		{
			int lineStartIndex = GetLineStartIndex(idx);
			return GetLineEndIndex(idx) + idx - lineStartIndex + ((lineStartIndex == 0) ? 1 : 0);
		}

		protected virtual int GetIndexOverCarriage(int idx)
		{
			int lineStartIndex = GetLineStartIndex(idx);
			int num = lineStartIndex;
			if (lineStartIndex > 0)
			{
				num = GetLineStartIndex(lineStartIndex - 1);
			}
			GetLineEndIndex(idx);
			return num + idx - lineStartIndex - ((num == 0) ? 1 : 0);
		}

		protected int GetPreviousSpace()
		{
			if (CarriagePositionIndex == 0)
			{
				return 0;
			}
			int num = m_text.ToString().Substring(0, CarriagePositionIndex).LastIndexOf(" ");
			int num2 = m_text.ToString().Substring(0, CarriagePositionIndex).LastIndexOf("\n");
			if (num == -1 && num2 == -1)
			{
				return 0;
			}
			return Math.Max(num, num2);
		}

		protected int GetNextSpace()
		{
			if (CarriagePositionIndex == m_text.Length)
			{
				return m_text.Length;
			}
			int num = m_text.ToString().Substring(CarriagePositionIndex + 1).IndexOf(" ");
			int num2 = m_text.ToString().Substring(CarriagePositionIndex + 1).IndexOf("\n");
			if (num == -1 && num2 == -1)
			{
				return m_text.Length;
			}
			if (num == -1)
			{
				num = int.MaxValue;
			}
			if (num2 == -1)
			{
				num2 = int.MaxValue;
			}
			return CarriagePositionIndex + Math.Min(num, num2) + 1;
		}

		public void Parse()
		{
			Clear();
			Parse(Text.ToString(), Font, TextScale, TextColor);
		}

		public void Parse(string text, MyFontEnum font, float textScale, Color textColor)
		{
			string[] array = text.ToString().Replace("[[", "\\u005B").Replace("]]", "\\u005D")
				.ToString()
				.Split('[', ']');
			bool flag = false;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i].Replace("\\u005B", "[");
				text2 = text2.Replace("\\u005D", "]");
				if (flag)
				{
					AppendText(text2, font, textScale, Color.Yellow.ToVector4());
				}
				else
				{
					AppendText(text2, font, textScale, textColor);
				}
				flag = !flag;
			}
		}
	}
}
