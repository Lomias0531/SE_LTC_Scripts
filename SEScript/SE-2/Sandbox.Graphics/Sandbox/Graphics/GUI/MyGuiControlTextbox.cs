using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlTextbox))]
	public class MyGuiControlTextbox : MyGuiControlBase, IMyImeActiveControl
	{
		public class StyleDefinition
		{
			public string NormalFont;

			public string HighlightFont;

			public MyGuiCompositeTexture NormalTexture;

			public MyGuiCompositeTexture HighlightTexture;
		}

		public struct MySkipCombination
		{
			public bool Alt;

			public bool Ctrl;

			public bool Shift;

			public MyKeys[] Keys;
		}

		private class MyGuiControlTextboxSelection
		{
			private int m_startIndex;

			private int m_endIndex;

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

			public MyGuiControlTextboxSelection()
			{
				m_startIndex = 0;
				m_endIndex = 0;
			}

			public void SetEnd(MyGuiControlTextbox sender)
			{
				m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.Text.Length);
			}

			public void Reset(MyGuiControlTextbox sender)
			{
				m_startIndex = (m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.Text.Length));
			}

			public void SelectAll(MyGuiControlTextbox sender)
			{
				m_startIndex = 0;
				m_endIndex = sender.Text.Length;
				sender.CarriagePositionIndex = sender.Text.Length;
			}

			public void EraseText(MyGuiControlTextbox sender)
			{
				if (Start != End)
				{
					StringBuilder stringBuilder = new StringBuilder(sender.Text.Substring(0, Start));
					StringBuilder value = new StringBuilder(sender.Text.Substring(End));
					sender.CarriagePositionIndex = Start;
					sender.Text = stringBuilder.Append((object)value).ToString();
				}
			}

			public void CutText(MyGuiControlTextbox sender)
			{
				CopyText(sender);
				EraseText(sender);
			}

			public void CopyText(MyGuiControlTextbox sender)
			{
				ClipboardText = sender.Text.Substring(Start, Length);
				if (!string.IsNullOrEmpty(ClipboardText))
				{
					MyVRage.Platform.Clipboard = ClipboardText;
				}
			}

			public void PasteText(MyGuiControlTextbox sender)
			{
				EraseText(sender);
				string text = sender.Text.Substring(0, sender.CarriagePositionIndex);
				string value = sender.Text.Substring(sender.CarriagePositionIndex);
				Thread thread = new Thread(PasteFromClipboard);
				thread.ApartmentState = ApartmentState.STA;
				thread.Start();
				thread.Join();
				string text2 = ClipboardText.Replace("\n", "");
				string text3;
				if (text2.Length + sender.Text.Length <= sender.MaxLength)
				{
					text3 = text2;
				}
				else
				{
					int num = sender.MaxLength - sender.Text.Length;
					text3 = ((num <= 0) ? "" : text2.Substring(0, num));
				}
				sender.Text = new StringBuilder(text).Append(text3).Append(value).ToString();
				sender.CarriagePositionIndex = text.Length + text3.Length;
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

		private static StyleDefinition[] m_styles;

		private int m_carriageBlinkerTimer;

		private int m_carriagePositionIndex;

		private bool m_drawBackground;

		private bool m_formattedAlready;

		private int m_maxLength;

		private List<MyKeys> m_pressedKeys = new List<MyKeys>(10);

		private Vector4 m_textColor;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		private bool m_hadFocusLastTime;

		private float m_slidingWindowOffset;

		private MyRectangle2D m_textAreaRelative;

		private MyGuiCompositeTexture m_compositeBackground;

		private StringBuilder m_text = new StringBuilder();

		private MyGuiControlTextboxSelection m_selection = new MyGuiControlTextboxSelection();

		private bool m_isImeActive;

		public MyGuiControlTextboxType Type;

		public bool TruncateDecimalDigits = true;

		private MyGuiControlTextboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private StyleDefinition m_customStyle;

		private bool m_useCustomStyle;

		private static MyKeyThrottler m_keyThrottler;

		private string m_virtualKeyboardPendingData;

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

		public int MaxLength
		{
			get
			{
				return m_maxLength;
			}
			set
			{
				m_maxLength = value;
				if (m_text.Length > m_maxLength)
				{
					m_text.Remove(m_maxLength, m_text.Length - m_maxLength);
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

		[Obsolete("Do not use this, it allocates! Use SetText instead!")]
		public string Text
		{
			get
			{
				return m_text.ToString();
			}
			set
			{
				m_text.Clear().Append(value);
				if (CarriagePositionIndex >= m_text.Length)
				{
					CarriagePositionIndex = m_text.Length;
				}
				OnTextChanged();
			}
		}

		public TextAlingmentMode TextAlignment
		{
			get;
			set;
		}

		public MyGuiControlTextboxStyleEnum VisualStyle
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

		public int CarriagePositionIndex
		{
			get
			{
				return m_carriagePositionIndex;
			}
			private set
			{
				m_carriagePositionIndex = MathHelper.Clamp(value, 0, Text.Length);
			}
		}

		public MySkipCombination[] SkipCombinations
		{
			get;
			set;
		}

		public string TextFont
		{
			get;
			private set;
		}

		public event Action<MyGuiControlTextbox> TextChanged;

		public event Action<MyGuiControlTextbox> EnterPressed;

		static MyGuiControlTextbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlTextboxStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_TEXTBOX,
				HighlightTexture = MyGuiConstants.TEXTURE_TEXTBOX_HIGHLIGHT,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[1] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_TEXTBOX,
				HighlightTexture = MyGuiConstants.TEXTURE_TEXTBOX_HIGHLIGHT,
				NormalFont = "Debug",
				HighlightFont = "Debug"
			};
			m_keyThrottler = new MyKeyThrottler();
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlTextboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public bool TextEquals(StringBuilder text)
		{
			return m_text.CompareTo(text) == 0;
		}

		public void GetText(StringBuilder result)
		{
			result.AppendStringBuilder(m_text);
		}

		public void SetText(StringBuilder source)
		{
			m_text.Clear().AppendStringBuilder(source);
			if (CarriagePositionIndex >= m_text.Length)
			{
				CarriagePositionIndex = m_text.Length;
			}
			OnTextChanged();
		}

		private void RefreshVisualStyle()
		{
			if (m_useCustomStyle)
			{
				m_styleDef = m_customStyle;
			}
			else
			{
				m_styleDef = GetVisualStyle(VisualStyle);
			}
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (base.HasHighlight)
			{
				m_compositeBackground = m_styleDef.HighlightTexture;
				base.MinSize = m_compositeBackground.MinSizeGui * TextScale;
				base.MaxSize = m_compositeBackground.MaxSizeGui * TextScale;
				TextFont = m_styleDef.HighlightFont;
			}
			else
			{
				m_compositeBackground = m_styleDef.NormalTexture;
				base.MinSize = m_compositeBackground.MinSizeGui * TextScale;
				base.MaxSize = m_compositeBackground.MaxSizeGui * TextScale;
				TextFont = m_styleDef.NormalFont;
			}
			RefreshTextArea();
		}

		public MyGuiControlTextbox()
			: this(null, null, 512, null, 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Default)
		{
		}

		public MyGuiControlTextbox(Vector2? position = null, string defaultText = null, int maxLength = 512, Vector4? textColor = null, float textScale = 0.8f, MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum visualStyle = MyGuiControlTextboxStyleEnum.Default)
			: base(position, new Vector2(512f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			base.Name = "Textbox";
			Type = type;
			m_carriagePositionIndex = 0;
			m_carriageBlinkerTimer = 0;
			m_textColor = (textColor ?? Vector4.One);
			TextScale = textScale;
			TextAlignment = TextAlingmentMode.Left;
			m_maxLength = maxLength;
			Text = (defaultText ?? "");
			m_visualStyle = visualStyle;
			RefreshVisualStyle();
			m_slidingWindowOffset = 0f;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			m_slidingWindowOffset = 0f;
			m_carriagePositionIndex = 0;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (base.Visible)
			{
				m_compositeBackground.Draw(GetPositionAbsoluteTopLeft(), base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
				base.Draw(transitionAlpha, backgroundTransitionAlpha);
				MyRectangle2D textAreaRelative = m_textAreaRelative;
				textAreaRelative.LeftTop += GetPositionAbsoluteTopLeft();
				float num = GetCarriageOffset(CarriagePositionIndex);
				RectangleF normalizedRectangle = new RectangleF(textAreaRelative.LeftTop, new Vector2(textAreaRelative.Size.X, textAreaRelative.Size.Y * 2f));
				using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
				{
					RefreshSlidingWindow();
					if (m_selection.Length > 0)
					{
						float num2 = GetCarriageOffset(m_selection.End) - GetCarriageOffset(m_selection.Start);
						MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", new Vector2(textAreaRelative.LeftTop.X + GetCarriageOffset(m_selection.Start), textAreaRelative.LeftTop.Y), new Vector2(num2 + 0.002f, textAreaRelative.Size.Y * 1.38f), MyGuiControlBase.ApplyColorMaskModifiers(new Vector4(1f, 1f, 1f, 0.5f), base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
					StringBuilder text = new StringBuilder(GetModifiedText());
					Vector2 vector = MyGuiManager.MeasureString(TextFont, text, TextScaleWithLanguage);
					Vector2 normalizedCoord = new Vector2(textAreaRelative.LeftTop.X + m_slidingWindowOffset, textAreaRelative.LeftTop.Y);
					if (TextAlignment == TextAlingmentMode.Right)
					{
						normalizedCoord = new Vector2(textAreaRelative.LeftTop.X + m_slidingWindowOffset + textAreaRelative.Size.X - vector.X, textAreaRelative.LeftTop.Y);
					}
					MyGuiManager.DrawString(TextFont, text, normalizedCoord, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(m_textColor, base.Enabled, transitionAlpha));
					if (base.HasFocus)
					{
						int num3 = m_carriageBlinkerTimer % 60;
						if (num3 >= 0 && num3 <= 45)
						{
							if (TextAlignment == TextAlingmentMode.Left && CarriagePositionIndex == 0)
							{
								num += 0.0005f;
							}
							else if (TextAlignment == TextAlingmentMode.Right && (CarriagePositionIndex == 0 || CarriagePositionIndex == m_text.Length))
							{
								num -= 0.0005f;
							}
							Vector2 normalizedCoord2 = new Vector2(textAreaRelative.LeftTop.X + num, GetPositionAbsoluteTopLeft().Y);
							MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", normalizedCoord2, 1, base.Size.Y, MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						}
					}
					m_carriageBlinkerTimer++;
				}
			}
		}

		private void DebugDraw()
		{
			MyRectangle2D textAreaRelative = m_textAreaRelative;
			textAreaRelative.LeftTop += GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(textAreaRelative.LeftTop, textAreaRelative.Size, Color.White, 1);
		}

		private int GetPreviousSpace()
		{
			if (CarriagePositionIndex == 0)
			{
				return 0;
			}
			int num = m_text.ToString().Substring(0, CarriagePositionIndex).LastIndexOf(" ");
			if (num == -1)
			{
				return 0;
			}
			return num;
		}

		private int GetNextSpace()
		{
			if (CarriagePositionIndex == m_text.Length)
			{
				return m_text.Length;
			}
			int num = m_text.ToString().Substring(CarriagePositionIndex + 1).IndexOf(" ");
			if (num == -1)
			{
				return m_text.Length;
			}
			return CarriagePositionIndex + num + 1;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase ret = base.HandleInput();
			try
			{
				HandleVirtualKeyboardInput();
				if (ret == null && base.Enabled)
				{
					if (MyInput.Static.IsNewLeftMousePressed())
					{
						if (base.IsMouseOver)
						{
							if (MyVRage.Platform.ImeProcessor != null)
							{
								MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
							}
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
							ret = this;
						}
						else
						{
							m_selection.Reset(this);
						}
					}
					else if (MyInput.Static.IsNewLeftMouseReleased())
					{
						m_selection.Dragging = false;
					}
					else if (m_selection.Dragging)
					{
						if (MyVRage.Platform.ImeProcessor != null)
						{
							MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
						}
						CarriagePositionIndex = GetCarriagePositionFromMouseCursor();
						m_selection.SetEnd(this);
						ret = this;
					}
					if (base.HasFocus)
					{
						if (!MyInput.Static.IsAnyCtrlKeyPressed())
						{
							HandleTextInputBuffered(ref ret);
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Left) == ThrottledKeyStatus.PRESSED_AND_READY)
						{
							ret = this;
							if (!m_isImeActive)
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
								DelayCaretBlink();
							}
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Right) == ThrottledKeyStatus.PRESSED_AND_READY)
						{
							ret = this;
							if (!m_isImeActive)
							{
								if (MyInput.Static.IsAnyCtrlKeyPressed())
								{
									CarriagePositionIndex = GetNextSpace();
								}
								else
								{
									CarriagePositionIndex++;
								}
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								DelayCaretBlink();
							}
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Back) == ThrottledKeyStatus.PRESSED_AND_READY && MyInput.Static.IsAnyCtrlKeyPressed() && !m_isImeActive)
						{
							ret = this;
							CarriagePositionIndex = GetPreviousSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Delete) == ThrottledKeyStatus.PRESSED_AND_READY && MyInput.Static.IsAnyCtrlKeyPressed() && !m_isImeActive)
						{
							ret = this;
							CarriagePositionIndex = GetNextSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						if (!IsImeActive)
						{
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.Home))
							{
								CarriagePositionIndex = 0;
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								ret = this;
								DelayCaretBlink();
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.End))
							{
								CarriagePositionIndex = m_text.Length;
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								ret = this;
								DelayCaretBlink();
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.X) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.CutText(this);
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.C) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.CopyText(this);
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.PasteText(this);
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.A) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.SelectAll(this);
							}
							if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
							{
								this.EnterPressed.InvokeIfNotNull(this);
							}
							if (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
							{
								ret = this;
								MyVRage.Platform.Input2.ShowVirtualKeyboardIfNeeded(OnVirtualKeyboardDataReceived, null, m_text.ToString(), null, m_maxLength);
							}
						}
						m_formattedAlready = false;
					}
					else if (Type == MyGuiControlTextboxType.DigitsOnly && !m_formattedAlready && m_text.Length != 0)
					{
						decimal decimalFromString = MyValueFormatter.GetDecimalFromString(Text, 1);
						int decimalDigits = (!TruncateDecimalDigits || decimalFromString - decimal.Truncate(decimalFromString) > 0m) ? 1 : 0;
						m_text.Clear().Append(MyValueFormatter.GetFormatedFloat((float)decimalFromString, decimalDigits, ""));
						CarriagePositionIndex = m_text.Length;
						m_formattedAlready = true;
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			m_hadFocusLastTime = base.HasFocus;
			return ret;
		}

		private void OnVirtualKeyboardDataReceived(string text)
		{
			m_virtualKeyboardPendingData = text;
		}

		private void HandleVirtualKeyboardInput()
		{
			string text = Interlocked.Exchange(ref m_virtualKeyboardPendingData, null);
			if (text != null)
			{
				if (!m_text.EqualsStrFast(text))
				{
					m_text.Clear().Append(text);
					CarriagePositionIndex = m_text.Length;
					m_selection.Reset(this);
					OnTextChanged();
				}
				this.EnterPressed.InvokeIfNotNull(this);
			}
		}

		public bool IsSkipCharacter(MyKeys character)
		{
			if (SkipCombinations != null)
			{
				MySkipCombination[] skipCombinations = SkipCombinations;
				for (int i = 0; i < skipCombinations.Length; i++)
				{
					MySkipCombination mySkipCombination = skipCombinations[i];
					if (mySkipCombination.Alt == MyInput.Static.IsAnyAltKeyPressed() && mySkipCombination.Ctrl == MyInput.Static.IsAnyCtrlKeyPressed() && mySkipCombination.Shift == MyInput.Static.IsAnyShiftKeyPressed() && (mySkipCombination.Keys == null || mySkipCombination.Keys.Contains(character)))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void HandleTextInputBuffered(ref MyGuiControlBase ret)
		{
			bool flag = false;
			foreach (char item in MyInput.Static.TextInput)
			{
				if (!IsSkipCharacter((MyKeys)item))
				{
					if (char.IsControl(item))
					{
						if (item == '\b')
						{
							KeypressBackspace(compositionEnd: true);
							flag = true;
						}
					}
					else
					{
						if (m_selection.Length > 0)
						{
							m_selection.EraseText(this);
						}
						InsertChar(conpositionEnd: true, item);
						flag = true;
					}
				}
			}
			if (m_keyThrottler.GetKeyStatus(MyKeys.Delete) == ThrottledKeyStatus.PRESSED_AND_READY)
			{
				KeypressDelete(compositionEnd: true);
				flag = true;
			}
			if (flag)
			{
				OnTextChanged();
				ret = this;
			}
		}

		public void InsertChar(bool conpositionEnd, char character)
		{
			if (m_selection.Length > 0)
			{
				m_selection.EraseText(this);
			}
			if (m_text.Length < m_maxLength)
			{
				m_text.Insert(CarriagePositionIndex, character);
				int num = ++CarriagePositionIndex;
				OnTextChanged();
			}
		}

		public void InsertCharMultiple(bool conpositionEnd, string chars)
		{
			if (m_selection.Length > 0)
			{
				m_selection.EraseText(this);
			}
			if (m_text.Length + chars.Length <= m_maxLength)
			{
				m_text.Insert(CarriagePositionIndex, chars);
				CarriagePositionIndex += chars.Length;
			}
		}

		public void KeypressBackspace(bool compositionEnd)
		{
			if (m_selection.Length == 0)
			{
				ApplyBackspace();
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
		}

		public void KeypressBackspaceMultiple(bool conpositionEnd, int count)
		{
			if (m_selection.Length == 0)
			{
				ApplyBackspaceMultiple(count);
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
		}

		public void KeypressDelete(bool compositionEnd)
		{
			if (m_selection.Length == 0)
			{
				ApplyDelete();
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
		}

		private void ApplyBackspace()
		{
			if (CarriagePositionIndex > 0)
			{
				int num = --CarriagePositionIndex;
				m_text.Remove(CarriagePositionIndex, 1);
			}
		}

		private void ApplyBackspaceMultiple(int count)
		{
			if (CarriagePositionIndex >= count)
			{
				CarriagePositionIndex -= count;
				m_text.Remove(CarriagePositionIndex, count);
			}
		}

		private void ApplyDelete()
		{
			if (CarriagePositionIndex < m_text.Length)
			{
				m_text.Remove(CarriagePositionIndex, 1);
			}
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshTextArea();
			RefreshSlidingWindow();
		}

		public void FocusEnded()
		{
			OnFocusChanged(focus: false);
		}

		internal override void OnFocusChanged(bool focus)
		{
			if (focus)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
				{
					MoveCarriageToEnd();
					m_selection.SelectAll(this);
				}
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.Activate(this);
				}
			}
			else
			{
				m_selection.Reset(this);
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.Deactivate();
				}
			}
			base.OnFocusChanged(focus);
		}

		public void MoveCarriageToEnd()
		{
			CarriagePositionIndex = m_text.Length;
		}

		public float GetCarriageOffset(int index)
		{
			string value = GetModifiedText().Substring(0, index);
			Vector2 vector = MyGuiManager.MeasureString("Blue", new StringBuilder(value), TextScaleWithLanguage);
			if (TextAlignment == TextAlingmentMode.Left)
			{
				return vector.X + m_slidingWindowOffset;
			}
			string modifiedText = GetModifiedText();
			Vector2 vector2 = MyGuiManager.MeasureString("Blue", new StringBuilder(modifiedText), TextScaleWithLanguage);
			return vector.X + m_slidingWindowOffset + m_textAreaRelative.Size.X - vector2.X;
		}

		private int GetCarriagePositionFromMouseCursor()
		{
			RefreshSlidingWindow();
			float num = MyGuiManager.MouseCursorPosition.X - GetPositionAbsoluteTopLeft().X - m_textAreaRelative.LeftTop.X;
			int result = 0;
			float num2 = float.MaxValue;
			for (int i = 0; i <= m_text.Length; i++)
			{
				float carriageOffset = GetCarriageOffset(i);
				float num3 = Math.Abs(num - carriageOffset);
				if (num3 < num2)
				{
					num2 = num3;
					result = i;
				}
			}
			return result;
		}

		private void RefreshTextArea()
		{
			m_textAreaRelative = new MyRectangle2D(MyGuiConstants.TEXTBOX_TEXT_OFFSET, base.Size - 2f * MyGuiConstants.TEXTBOX_TEXT_OFFSET);
		}

		private string GetModifiedText()
		{
			switch (Type)
			{
			case MyGuiControlTextboxType.Normal:
			case MyGuiControlTextboxType.DigitsOnly:
				return Text;
			case MyGuiControlTextboxType.Password:
				return new string('*', m_text.Length);
			default:
				return Text;
			}
		}

		private void OnTextChanged()
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this);
			}
			RefreshSlidingWindow();
			m_selection.Reset(this);
			DelayCaretBlink();
		}

		private void DelayCaretBlink()
		{
			m_carriageBlinkerTimer = 0;
		}

		private void RefreshSlidingWindow()
		{
			float carriageOffset = GetCarriageOffset(CarriagePositionIndex);
			MyRectangle2D textAreaRelative = m_textAreaRelative;
			if (carriageOffset < 0f)
			{
				m_slidingWindowOffset -= carriageOffset;
			}
			else if (carriageOffset > textAreaRelative.Size.X)
			{
				m_slidingWindowOffset -= carriageOffset - textAreaRelative.Size.X;
			}
		}

		public void SelectAll()
		{
			if (m_selection != null)
			{
				m_selection.SelectAll(this);
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_useCustomStyle = true;
			m_customStyle = style;
			RefreshVisualStyle();
		}

		public Vector2 GetCornerPosition()
		{
			return GetPositionAbsoluteBottomLeft();
		}

		public Vector2 GetCarriagePosition(int shiftX)
		{
			int num = Text.Length - shiftX;
			Vector2 result = new Vector2(GetCarriageOffset((num >= 0) ? num : 0), 0f);
			result.X += 0.009f;
			return result;
		}

		public void KeypressEnter(bool compositionEnd)
		{
			if (this.EnterPressed != null)
			{
				this.EnterPressed(this);
			}
		}

		public void DeactivateIme()
		{
			m_isImeActive = false;
		}

		public void KeypressRedo()
		{
		}

		public void KeypressUndo()
		{
		}

		public int GetMaxLength()
		{
			return m_maxLength;
		}

		public int GetSelectionLength()
		{
			if (m_selection == null)
			{
				return 0;
			}
			return m_selection.Length;
		}

		public int GetTextLength()
		{
			return Text.Length;
		}
	}
}
