using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelLine
	{
		private readonly float m_minLineHeight;

		private List<MyRichLabelPart> m_parts;

		private Vector2 m_size;

		public Vector2 Size => m_size;

		public string DebugText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < m_parts.Count; i++)
				{
					m_parts[i].AppendTextTo(stringBuilder);
				}
				return stringBuilder.ToString();
			}
		}

		public MyRichLabelLine(float minLineHeight)
		{
			m_minLineHeight = minLineHeight;
			m_parts = new List<MyRichLabelPart>(8);
			RecalculateSize();
		}

		public void AddPart(MyRichLabelPart part)
		{
			m_parts.Add(part);
			RecalculateSize();
		}

		public void ClearParts()
		{
			m_parts.Clear();
			RecalculateSize();
		}

		public IEnumerable<MyRichLabelPart> GetParts()
		{
			return m_parts;
		}

		private void RecalculateSize()
		{
			Vector2 size = new Vector2(0f, m_minLineHeight);
			for (int i = 0; i < m_parts.Count; i++)
			{
				Vector2 size2 = m_parts[i].Size;
				size.Y = Math.Max(size2.Y, size.Y);
				size.X += size2.X;
			}
			m_size = size;
		}

		public bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			Vector2 position2 = position;
			float num = position.Y + m_size.Y / 2f;
			for (int i = 0; i < m_parts.Count; i++)
			{
				MyRichLabelPart myRichLabelPart = m_parts[i];
				Vector2 size = myRichLabelPart.Size;
				position2.Y = num - size.Y / 2f;
				if (!(position2.Y + m_size.Y < 0f) && !(position2.Y > 1f))
				{
					if (!myRichLabelPart.Draw(position2, alphamask, ref charactersLeft))
					{
						return false;
					}
					position2.X += size.X;
					if (charactersLeft <= 0)
					{
						return true;
					}
				}
			}
			return true;
		}

		public bool IsEmpty()
		{
			return m_parts.Count == 0;
		}

		public bool HandleInput(Vector2 position)
		{
			for (int i = 0; i < m_parts.Count; i++)
			{
				MyRichLabelPart myRichLabelPart = m_parts[i];
				if (myRichLabelPart.HandleInput(position))
				{
					return true;
				}
				position.X += myRichLabelPart.Size.X;
			}
			return false;
		}
	}
}
