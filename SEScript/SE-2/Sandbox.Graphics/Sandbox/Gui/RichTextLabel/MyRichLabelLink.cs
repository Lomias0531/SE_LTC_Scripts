using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRage.Input;
using VRageMath;

namespace Sandbox.Gui.RichTextLabel
{
	internal class MyRichLabelLink : MyRichLabelText
	{
		private Action<string> m_onClick;

		private bool m_highlight;

		private int m_lastTimeClicked;

		private const string m_linkImgTex = "Textures\\GUI\\link.dds";

		private MyRichLabelImage m_linkImg;

		private const float m_linkImgSpace = 0.008f;

		public string Url
		{
			get;
			set;
		}

		public override Vector2 Size
		{
			get
			{
				Vector2 size = base.Size;
				Vector2 size2 = m_linkImg.Size;
				return new Vector2(size.X + 0.008f + size2.X, Math.Max(size.Y, size2.Y));
			}
		}

		public MyRichLabelLink(string url, string text, float scale, Action<string> onClick)
		{
			Init(text, "Blue", scale, Vector4.Zero);
			Url = url;
			m_onClick = onClick;
			Vector2 normalizedSizeFromScreenSize = MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(0.015f * scale)).X));
			m_linkImg = new MyRichLabelImage("Textures\\GUI\\link.dds", normalizedSizeFromScreenSize, Vector4.One);
		}

		public override bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			MyFontEnum input;
			Color value;
			if (m_highlight)
			{
				input = "White";
				value = MyGuiConstants.LABEL_TEXT_COLOR;
			}
			else
			{
				input = "Blue";
				value = VRageMath.Color.PowderBlue;
			}
			value *= alphamask;
			string value2 = base.Text.ToString(0, Math.Min(charactersLeft, base.Text.Length));
			charactersLeft -= base.Text.Length;
			m_tmpText.Clear();
			m_tmpText.Append(value2);
			MyGuiManager.DrawString(input, m_tmpText, position, base.Scale, value);
			m_linkImg.Draw(position + new Vector2(base.Size.X + 0.004f, 0.008f), alphamask, ref charactersLeft);
			m_highlight = false;
			return true;
		}

		public override bool HandleInput(Vector2 position)
		{
			Vector2 mouseCursorPosition = MyGuiManager.MouseCursorPosition;
			if (mouseCursorPosition.X > position.X + 0.001f && mouseCursorPosition.Y > position.Y && mouseCursorPosition.X < position.X + Size.X && mouseCursorPosition.Y < position.Y + Size.Y)
			{
				m_highlight = true;
				if (MyInput.Static.IsLeftMousePressed() && MyGuiManager.TotalTimeInMilliseconds - m_lastTimeClicked > MyGuiConstants.REPEAT_PRESS_DELAY)
				{
					m_onClick(Url);
					m_lastTimeClicked = MyGuiManager.TotalTimeInMilliseconds;
					return true;
				}
			}
			else
			{
				m_highlight = false;
			}
			return false;
		}
	}
}
