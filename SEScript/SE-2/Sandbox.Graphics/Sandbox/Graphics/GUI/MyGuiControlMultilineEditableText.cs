using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlMultilineEditableLabel))]
	public class MyGuiControlMultilineEditableText : MyGuiControlMultilineText, IMyImeActiveControl
	{
		private int m_previousTextSize;

		private List<string> m_undoCache = new List<string>();

		private List<string> m_redoCache = new List<string>();

		private const int TAB_SIZE = 4;

		private const int MAX_UNDO_HISTORY = 50;

		private const char NEW_LINE = '\n';

		private const char BACKSPACE = '\b';

		private const char TAB = '\t';

		private const char CTLR_Z = '\u001a';

		private const char CTLR_Y = '\u0019';

		private int m_currentCarriageLine;

		private int m_previousCarriagePosition;

		private float m_fontHeight;

		private int m_currentCarriageColumn;

		protected List<int> m_lineInformation = new List<int>();

		public bool TextWrap
		{
			get;
			set;
		}

		public int MaxCharacters
		{
			get;
			set;
		} = int.MaxValue;


		public override StringBuilder Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_lineInformation.Clear();
				m_text.Clear();
				if (value != null)
				{
					StringBuilder stringBuilder = value;
					if (stringBuilder.Length > MaxCharacters)
					{
						stringBuilder = new StringBuilder(stringBuilder.ToString().Substring(0, MaxCharacters));
					}
					m_text.AppendStringBuilder(stringBuilder);
				}
				OnTextChanged();
				RefreshText(useEnum: false);
			}
		}

		public event Action<MyGuiControlMultilineEditableText> TextChanged;

		public MyGuiControlMultilineEditableText(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string font = "Blue", float textScale = 0.8f, MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, StringBuilder contents = null, bool drawScrollbarV = true, bool drawScrollbarH = true, MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, int? visibleLinesCount = null, MyGuiCompositeTexture backgroundTexture = null, MyGuiBorderThickness? textPadding = null)
			: base(position, size, backgroundColor, font, textScale, textAlign, contents, drawScrollbarV, drawScrollbarH, textBoxAlign, visibleLinesCount, selectable: true, showTextShadow: false, backgroundTexture, textPadding)
		{
			m_fontHeight = MyGuiManager.GetFontHeight(base.Font, base.TextScaleWithLanguage);
			base.CanHaveFocus = true;
		}

		protected override float ComputeRichLabelWidth()
		{
			return float.MaxValue;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase ret = base.HandleInput();
			if (base.HasFocus && base.Selectable)
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Back))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						if (!base.IsImeActive)
						{
							base.CarriagePositionIndex = GetPreviousSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						return this;
					}
					switch (m_keyThrottler.GetKeyStatus(MyKeys.Delete))
					{
					case ThrottledKeyStatus.PRESSED_AND_WAITING:
						return this;
					case ThrottledKeyStatus.PRESSED_AND_READY:
						if (!base.IsImeActive)
						{
							base.CarriagePositionIndex = GetNextSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						return this;
					}
				}
				if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.X) && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					if (MyVRage.Platform.ImeProcessor != null)
					{
						MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
					}
					AddToUndo(m_text.ToString());
					m_selection.CutText(this);
					m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
					m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
					return this;
				}
				if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					if (MyVRage.Platform.ImeProcessor != null)
					{
						MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
					}
					AddToUndo(m_text.ToString());
					m_selection.PasteText(this);
					m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
					m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
					return this;
				}
				if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.Home))
				{
					int lineStartIndex = GetLineStartIndex(base.CarriagePositionIndex);
					int i;
					for (i = lineStartIndex; i < Text.Length && Text[i] == ' '; i++)
					{
					}
					if (base.CarriagePositionIndex == i || i == Text.Length)
					{
						base.CarriagePositionIndex = lineStartIndex;
					}
					else
					{
						base.CarriagePositionIndex = i;
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						base.CarriagePositionIndex = 0;
					}
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						m_selection.SetEnd(this);
					}
					else
					{
						m_selection.Reset(this);
					}
					m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
					return this;
				}
				if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.End))
				{
					int num = base.CarriagePositionIndex = GetLineEndIndex(base.CarriagePositionIndex);
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						base.CarriagePositionIndex = Text.Length;
					}
					if (MyInput.Static.IsAnyShiftKeyPressed())
					{
						m_selection.SetEnd(this);
					}
					else
					{
						m_selection.Reset(this);
					}
					m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
					return this;
				}
				if ((MyInput.Static.IsKeyPress(MyKeys.Left) || MyInput.Static.IsKeyPress(MyKeys.Right)) && !base.IsImeActive)
				{
					m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
					m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
				}
				HandleTextInputBuffered(ref ret);
			}
			if (MyInput.Static.IsAnyMouseOrJoystickPressed() && !base.IsMouseOver && ret == null)
			{
				return ret;
			}
			if (!base.HasFocus || !MyInput.Static.IsAnyKeyPress() || m_keyThrottler.IsNewPressAndThrottled(MyKeys.Escape))
			{
				return ret;
			}
			return this;
		}

		protected void HandleTextInputBuffered(ref MyGuiControlBase ret)
		{
			bool flag = false;
			foreach (char item in MyInput.Static.TextInput)
			{
				if (char.IsControl(item))
				{
					switch (item)
					{
					case '\u001a':
						if (MyVRage.Platform.ImeProcessor != null)
						{
							MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
						}
						Undo();
						m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
						m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
						break;
					case '\u0019':
						if (MyVRage.Platform.ImeProcessor != null)
						{
							MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
						}
						Redo();
						m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
						m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
						break;
					case '\b':
						AddToUndo(m_text.ToString());
						if (m_selection.Length == 0)
						{
							ApplyBackspace();
						}
						else
						{
							m_selection.EraseText(this);
						}
						m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
						m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
						flag = true;
						break;
					case '\r':
						AddToUndo(m_text.ToString());
						if (m_selection.Length != 0)
						{
							m_selection.EraseText(this);
						}
						InsertCharInternal('\n');
						flag = true;
						m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
						m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
						break;
					case '\t':
					{
						m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
						m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
						AddToUndo(m_text.ToString());
						int num = 4 - m_currentCarriageColumn % 4;
						for (int i = 0; i < num; i++)
						{
							InsertCharInternal(' ');
						}
						flag = (num > 0);
						break;
					}
					}
				}
				else
				{
					AddToUndo(m_text.ToString());
					if (m_selection.Length > 0)
					{
						m_selection.EraseText(this);
					}
					if (m_text != null && m_text.Length < MaxCharacters)
					{
						InsertCharInternal(item);
						flag = true;
					}
				}
			}
			if (m_keyThrottler.GetKeyStatus(MyKeys.Delete) == ThrottledKeyStatus.PRESSED_AND_READY)
			{
				m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
				AddToUndo(m_text.ToString());
				if (m_selection.Length == 0)
				{
					ApplyDelete();
				}
				else
				{
					m_selection.EraseText(this);
				}
				flag = true;
			}
			if (flag)
			{
				OnTextChanged();
				m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
				ret = this;
			}
		}

		public void KeypressBackspace(bool compositionEnd)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
			}
			if (m_selection.Length == 0)
			{
				ApplyBackspace();
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
			m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
			m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
		}

		public void KeypressBackspaceMultiple(bool compositionEnd, int count)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
			}
			if (m_selection.Length == 0)
			{
				ApplyBackspaceMultiple(count);
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
			m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
			m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
		}

		public void InsertChar(bool compositionEnd, char character)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
			}
			if (character == '\t')
			{
				m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
				m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
				AddToUndo(m_text.ToString());
				int num = 4 - m_currentCarriageColumn % 4;
				for (int i = 0; i < num; i++)
				{
					InsertCharInternal(' ');
				}
			}
			else
			{
				if (m_selection.Length != 0)
				{
					m_selection.EraseText(this);
				}
				m_text.Insert(base.CarriagePositionIndex, character);
				int num2 = ++base.CarriagePositionIndex;
			}
			OnTextChanged();
		}

		public void InsertCharMultiple(bool compositionEnd, string chars)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
			}
			if (m_selection.Length != 0)
			{
				m_selection.EraseText(this);
			}
			m_text.Insert(base.CarriagePositionIndex, chars);
			base.CarriagePositionIndex += chars.Length;
			OnTextChanged();
		}

		public void KeypressDelete(bool compositionEnd)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
			}
			if (m_selection.Length == 0)
			{
				ApplyDelete();
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChanged();
			m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
			m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
		}

		private void OnTextChanged()
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this);
			}
			if (TextWrap)
			{
				m_label.MaxLineWidth = base.Size.X - base.TextPadding.Left - base.TextPadding.Right - m_scrollbarV.Size.X;
			}
			m_selection.Reset(this);
			m_label.Clear();
			AppendText(m_text);
			BuildLineInformation();
			m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
			ScrollToShowCarriage();
			DelayCaretBlink();
		}

		private void InsertCharInternal(char character)
		{
			m_text.Insert(base.CarriagePositionIndex, character);
			int num = ++base.CarriagePositionIndex;
		}

		private void ApplyBackspace()
		{
			if (base.CarriagePositionIndex > 0)
			{
				int num = --base.CarriagePositionIndex;
				m_text.Remove(base.CarriagePositionIndex, 1);
				BuildLineInformation();
			}
		}

		private void ApplyBackspaceMultiple(int count)
		{
			if (base.CarriagePositionIndex >= count)
			{
				base.CarriagePositionIndex -= count;
				m_text.Remove(base.CarriagePositionIndex, count);
				BuildLineInformation();
			}
		}

		private void ApplyDelete()
		{
			if (base.CarriagePositionIndex < m_text.Length)
			{
				m_text.Remove(base.CarriagePositionIndex, 1);
				BuildLineInformation();
			}
		}

		protected override void DrawSelectionBackgrounds(MyRectangle2D textArea, float transitionAlpha)
		{
			List<string> list = new List<string>();
			int num = m_selection.Start;
			int num2 = m_selection.Length;
			int num3 = -1;
			for (int i = 0; i < m_lineInformation.Count; i++)
			{
				if (num < m_lineInformation[i])
				{
					continue;
				}
				int num4 = i + 1;
				if (num4 == m_lineInformation.Count)
				{
					break;
				}
				if (num < m_lineInformation[num4])
				{
					if (num3 == -1)
					{
						num3 = i;
					}
					if (num + num2 < m_lineInformation[num4])
					{
						string item = Text.ToString(num, num2);
						list.Add(item);
						break;
					}
					int num5 = m_lineInformation[num4] - num;
					string item2 = Text.ToString(num, num5);
					list.Add(item2);
					num = m_lineInformation[num4];
					num2 -= num5;
				}
			}
			int num6 = m_selection.Start;
			for (int j = 0; j < list.Count; j++)
			{
				string text = list[j];
				if (!(text == string.Empty))
				{
					Vector2 normalizedCoord = textArea.LeftTop + GetCarriageOffset(num6);
					int length = text.Length;
					Vector2 carriageOffset = GetCarriageOffset(num6);
					if (num6 != 0 && num6 == m_lineInformation[j + num3])
					{
						carriageOffset.Y += m_fontHeight;
						normalizedCoord.Y += m_fontHeight;
						normalizedCoord.X -= carriageOffset.X;
						carriageOffset.X = 0f;
					}
					Vector2 vector = GetCarriageOffset(num6 + length) - carriageOffset;
					MyGuiManager.DrawSpriteBatch(normalizedSize: new Vector2(vector.X, GetCarriageHeight()), texture: "Textures\\GUI\\Blank.dds", normalizedCoord: normalizedCoord, color: MyGuiControlBase.ApplyColorMaskModifiers(new Vector4(1f, 1f, 1f, 0.5f), base.Enabled, transitionAlpha), drawAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					num6 = m_lineInformation[j + num3 + 1];
				}
			}
		}

		protected override Vector2 GetCarriageOffset(int idx)
		{
			if (m_lineInformation.Count == 0)
			{
				return new Vector2(0f, 0f);
			}
			int num = m_lineInformation[m_lineInformation.Count - 1];
			Vector2 result = new Vector2(0f - base.ScrollbarValueH, 0f - base.ScrollbarValueV);
			int i;
			for (i = 0; i < m_lineInformation.Count; i++)
			{
				if (idx <= m_lineInformation[i])
				{
					i = Math.Max(0, --i);
					num = m_lineInformation[i];
					break;
				}
			}
			if (idx - num > 0)
			{
				m_tmpOffsetMeasure.Clear();
				m_tmpOffsetMeasure.AppendSubstring(Text, num, idx - num);
				result.X = MyGuiManager.MeasureString(base.Font, m_tmpOffsetMeasure, base.TextScaleWithLanguage).X - base.ScrollbarValueH;
			}
			result.Y = (float)Math.Min(i, m_lineInformation.Count - 1) * m_fontHeight - base.ScrollbarValueV;
			return result;
		}

		protected override int GetLineStartIndex(int idx)
		{
			if (idx < 0)
			{
				return 0;
			}
			for (int i = 0; i < m_lineInformation.Count; i++)
			{
				if (idx <= m_lineInformation[i])
				{
					if (i - 1 >= 0)
					{
						return m_lineInformation[i - 1];
					}
					return 0;
				}
			}
			return 0;
		}

		protected override int GetLineEndIndex(int idx)
		{
			if (idx < 0)
			{
				return 0;
			}
			for (int i = 0; i < m_lineInformation.Count; i++)
			{
				if (idx <= m_lineInformation[i])
				{
					return m_lineInformation[i];
				}
			}
			return 0;
		}

		public int GetCurrentCarriageLine()
		{
			return m_currentCarriageLine;
		}

		private int CalculateNewCarriageLine(int idx)
		{
			for (int i = 1; i < m_lineInformation.Count; i++)
			{
				if (idx <= m_lineInformation[i])
				{
					return Math.Max(0, i);
				}
			}
			return m_lineInformation.Count;
		}

		public int MeasureNumLines(string text)
		{
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					num++;
				}
			}
			return num;
		}

		public bool CarriageMoved()
		{
			if (m_previousCarriagePosition != m_carriagePositionIndex)
			{
				m_previousCarriagePosition = m_carriagePositionIndex;
				return true;
			}
			return false;
		}

		public int GetTotalNumLines()
		{
			return m_lineInformation.Count - 1;
		}

		protected override int GetCarriagePositionFromMouseCursor()
		{
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
			}
			Vector2 vector = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft() - m_textPadding.TopLeftOffset;
			vector.X += base.ScrollbarValueH;
			vector.Y += base.ScrollbarValueV;
			int num = 0;
			int num2 = 0;
			bool flag = true;
			for (num2 = 0; num2 < m_lineInformation.Count; num2++)
			{
				float num3 = m_fontHeight * (float)num2;
				if (!(vector.Y > num3) || !(vector.Y < num3 + m_fontHeight))
				{
					continue;
				}
				int num4 = (num2 + 1 >= m_lineInformation.Count) ? m_text.Length : m_lineInformation[num2 + 1];
				num4 -= m_lineInformation[num2];
				int num5 = m_lineInformation[num2];
				bool flag2 = true;
				for (int i = 0; i < num4; i++)
				{
					m_tmpOffsetMeasure.Clear();
					int num6 = i + 1;
					if (num5 + num6 >= m_text.Length)
					{
						num6 = num5 + num6 - m_text.Length;
					}
					m_tmpOffsetMeasure.AppendSubstring(m_text, num5, num6);
					float x = MyGuiManager.MeasureString(base.Font, m_tmpOffsetMeasure, base.TextScaleWithLanguage).X;
					if (vector.X < x)
					{
						num = num5 + i;
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					num = num5 + num4;
				}
				flag = false;
				break;
			}
			if (m_lineInformation.Count > 0 && flag)
			{
				num = m_lineInformation[m_lineInformation.Count - 1];
			}
			m_currentCarriageColumn = GetCarriageColumn(num);
			m_currentCarriageLine = num2 + 1;
			return num;
		}

		private void AddToUndo(string text, bool clearRedo = true)
		{
			if (clearRedo)
			{
				m_redoCache.Clear();
			}
			m_undoCache.Add(text);
			if (m_undoCache.Count > 50)
			{
				m_undoCache.RemoveAt(0);
			}
		}

		private void AddToRedo(string text)
		{
			m_redoCache.Add(text);
			if (m_redoCache.Count > 50)
			{
				m_redoCache.RemoveAt(50);
			}
		}

		private void Undo()
		{
			if (m_undoCache.Count > 0)
			{
				int currentIndex = UpdateCarriage(m_undoCache);
				AddToRedo(m_text.ToString());
				UpdateEditorText(currentIndex, m_undoCache);
			}
		}

		private void Redo()
		{
			if (m_redoCache.Count > 0)
			{
				int currentIndex = UpdateCarriage(m_redoCache);
				base.CarriagePositionIndex--;
				AddToUndo(m_text.ToString(), clearRedo: false);
				UpdateEditorText(currentIndex, m_redoCache);
			}
		}

		private int UpdateCarriage(List<string> array)
		{
			int num = array.Count - 1;
			int num2 = GetFirstDiffIndex(array[num], m_text.ToString());
			if (array[num].Length < m_text.Length)
			{
				num2--;
			}
			if (array[num].Length > m_text.Length)
			{
				num2++;
			}
			base.CarriagePositionIndex = ((num2 == -1) ? array[num].Length : num2);
			return num;
		}

		private void UpdateEditorText(int currentIndex, List<string> array)
		{
			m_text.Clear();
			m_text.Append(array[currentIndex]);
			OnTextChanged();
			array.RemoveAt(currentIndex);
		}

		private int GetFirstDiffIndex(string str1, string str2)
		{
			if (str1 == null || str2 == null)
			{
				return -1;
			}
			int num = Math.Min(str1.Length, str2.Length);
			for (int i = 0; i < num; i++)
			{
				if (str1[i] != str2[i])
				{
					return i + 1;
				}
			}
			return -1;
		}

		protected override int GetIndexUnderCarriage(int idx)
		{
			int lineEndIndex = GetLineEndIndex(idx);
			int lineEndIndex2 = GetLineEndIndex(Math.Min(Text.Length, lineEndIndex + 1));
			int lineStartIndex = GetLineStartIndex(Math.Min(Text.Length, lineEndIndex + 1));
			return CalculateNewCarriagePos(lineEndIndex2, lineStartIndex);
		}

		private int CalculateNewCarriagePos(int newRowEnd, int newRowStart)
		{
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
			}
			int num = Math.Min(newRowEnd - newRowStart, m_currentCarriageColumn);
			int num2 = newRowStart + num;
			m_currentCarriageLine = CalculateNewCarriageLine(num2);
			return num2;
		}

		protected override int GetIndexOverCarriage(int idx)
		{
			int lineStartIndex = GetLineStartIndex(idx);
			int lineEndIndex = GetLineEndIndex(Math.Max(0, lineStartIndex - 1));
			int lineStartIndex2 = GetLineStartIndex(Math.Max(0, lineStartIndex - 1));
			return CalculateNewCarriagePos(lineEndIndex, lineStartIndex2);
		}

		private int GetCarriageColumn(int idx)
		{
			int lineStartIndex = GetLineStartIndex(idx);
			return idx - lineStartIndex;
		}

		private void BuildLineInformation()
		{
			m_previousTextSize = m_text.Length;
			m_currentCarriageLine = 0;
			m_carriagePositionIndex = MathHelper.Clamp(m_carriagePositionIndex, 0, Text.Length);
			m_lineInformation.Clear();
			m_lineInformation.Add(0);
			int num = 0;
			foreach (MyRichLabelLine line in m_label.GetLines())
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerable<MyRichLabelPart> parts = line.GetParts();
				int num2 = 0;
				foreach (MyRichLabelPart item in parts)
				{
					item.AppendTextTo(stringBuilder);
					num2++;
				}
				if (stringBuilder.Length != 0 || num2 != 0)
				{
					num += stringBuilder.Length;
					m_lineInformation.Add(num);
					if (num < Text.Length)
					{
						if (Text[num] == '\n')
						{
							num++;
						}
						else if (num + 1 < Text.Length && Text.ToString().Substring(num, 2) == Environment.NewLine)
						{
							num += 2;
						}
					}
				}
			}
			ScrollToShowCarriage();
		}

		private void UpdateLineWrapping()
		{
			int num = m_lineInformation[m_lineInformation.Count - 2];
			int num2 = m_lineInformation[m_lineInformation.Count - 1];
			int num3 = num;
			List<string> list = new List<string>(m_text.ToString(num, num2 - num).Split(new char[1]
			{
				' '
			}));
			float num4 = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				list[i] += " ";
				float num5 = num4;
				float num6 = 0f;
				bool flag = false;
				for (int j = 0; j < list[i].Length; j++)
				{
					string value = list[i].Substring(0, j + 1);
					Vector2 vector = MyGuiManager.MeasureString(base.Font, new StringBuilder(value), base.TextScaleWithLanguage);
					num6 = num4 + vector.X;
					num3++;
					if (num6 > m_label.MaxLineWidth)
					{
						int num7 = 0;
						if (num5 == 0f)
						{
							_ = list[i].Length;
							num3--;
							value = list[i].Substring(j);
							num7 = i + 1;
							list.Insert(num7, value);
						}
						else
						{
							num3 -= j + 1;
							num7 = i;
							i--;
						}
						list[num7] = list[num7].Remove(list[num7].Length - 1);
						m_lineInformation.Insert(m_lineInformation.Count - 1, num3);
						num4 = 0f;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num4 = num6;
				}
			}
		}

		public void FocusEnded()
		{
			OnFocusChanged(focus: false);
		}

		internal override void OnFocusChanged(bool focus)
		{
			base.OnFocusChanged(focus);
			if (MyVRage.Platform.ImeProcessor != null)
			{
				if (focus)
				{
					MyVRage.Platform.ImeProcessor.Activate(this);
				}
				else
				{
					MyVRage.Platform.ImeProcessor.Deactivate();
				}
			}
		}

		public Vector2 GetCornerPosition()
		{
			return GetPositionAbsoluteTopLeft();
		}

		public Vector2 GetCarriagePosition(int shiftX)
		{
			Vector2 carriageOffset = GetCarriageOffset(base.CarriagePositionIndex - shiftX);
			carriageOffset.Y += 0.025f;
			return carriageOffset;
		}

		public void KeypressEnter(bool compositionEnd)
		{
			if (compositionEnd)
			{
				AddToUndo(m_text.ToString());
				if (m_selection.Length != 0)
				{
					m_selection.EraseText(this);
				}
			}
			InsertCharInternal('\n');
			m_currentCarriageLine = CalculateNewCarriageLine(base.CarriagePositionIndex);
			m_currentCarriageColumn = GetCarriageColumn(base.CarriagePositionIndex);
			OnTextChanged();
		}

		public void KeypressRedo()
		{
			Redo();
		}

		public void KeypressUndo()
		{
			Undo();
		}

		public int GetMaxLength()
		{
			return int.MaxValue;
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
