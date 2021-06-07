using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudNameValueData
	{
		public class Data
		{
			public StringBuilder Name;

			public StringBuilder Value;

			public string NameFont;

			public string ValueFont;

			public bool Visible;

			public Data()
			{
				Name = new StringBuilder();
				Value = new StringBuilder();
				Visible = true;
			}
		}

		private readonly List<Data> m_items;

		private int m_count;

		public string DefaultNameFont;

		public string DefaultValueFont;

		public float LineSpacing;

		public bool ShowBackgroundFog;

		public int Count
		{
			get
			{
				return m_count;
			}
			set
			{
				m_count = value;
				EnsureItemsExist();
			}
		}

		public Data this[int i] => m_items[i];

		public int GetVisibleCount()
		{
			int num = 0;
			for (int i = 0; i < m_count; i++)
			{
				if (m_items[i].Visible)
				{
					num++;
				}
			}
			return num;
		}

		public float GetGuiHeight()
		{
			return (float)(GetVisibleCount() + 1) * LineSpacing;
		}

		public MyHudNameValueData(int itemCount, string defaultNameFont = "Blue", string defaultValueFont = "White", float lineSpacing = 0.025f, bool showBackgroundFog = false)
		{
			DefaultNameFont = defaultNameFont;
			DefaultValueFont = defaultValueFont;
			LineSpacing = lineSpacing;
			m_count = itemCount;
			m_items = new List<Data>(itemCount);
			ShowBackgroundFog = showBackgroundFog;
			EnsureItemsExist();
		}

		public void DrawTopDown(Vector2 namesTopLeft, Vector2 valuesTopRight, float textScale)
		{
			Color white = Color.White;
			if (ShowBackgroundFog)
			{
				DrawBackgroundFog(namesTopLeft, valuesTopRight, topDown: true);
			}
			for (int i = 0; i < Count; i++)
			{
				Data data = m_items[i];
				if (data.Visible)
				{
					MyGuiManager.DrawString(data.NameFont ?? DefaultNameFont, data.Name, namesTopLeft, textScale, white);
					MyGuiManager.DrawString(data.ValueFont ?? DefaultValueFont, data.Value, valuesTopRight, textScale, white, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					namesTopLeft.Y += LineSpacing;
					valuesTopRight.Y += LineSpacing;
				}
			}
		}

		public void DrawBottomUp(Vector2 namesBottomLeft, Vector2 valuesBottomRight, float textScale)
		{
			Color white = Color.White;
			if (ShowBackgroundFog)
			{
				DrawBackgroundFog(namesBottomLeft, valuesBottomRight, topDown: false);
			}
			for (int num = Count - 1; num >= 0; num--)
			{
				Data data = m_items[num];
				if (data.Visible)
				{
					MyGuiManager.DrawString(data.NameFont ?? DefaultNameFont, data.Name, namesBottomLeft, textScale, white, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
					MyGuiManager.DrawString(data.ValueFont ?? DefaultValueFont, data.Value, valuesBottomRight, textScale, white, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
					namesBottomLeft.Y -= LineSpacing;
					valuesBottomRight.Y -= LineSpacing;
				}
			}
		}

		internal float ComputeMaxLineWidth(float textScale)
		{
			float num = 0f;
			for (int i = 0; i < Count; i++)
			{
				Data data = m_items[i];
				string font = data.NameFont ?? DefaultNameFont;
				string font2 = data.ValueFont ?? DefaultValueFont;
				Vector2 vector = MyGuiManager.MeasureString(font, data.Name, textScale);
				Vector2 vector2 = MyGuiManager.MeasureString(font2, data.Value, textScale);
				num = Math.Max(num, vector.X + vector2.X);
			}
			return num;
		}

		private void DrawBackgroundFog(Vector2 namesTopLeft, Vector2 valuesTopRight, bool topDown)
		{
			float num;
			int num2;
			int num3;
			int num4;
			if (topDown)
			{
				num = LineSpacing;
				num2 = 0;
				num3 = Count;
				num4 = 1;
			}
			else
			{
				num = 0f - LineSpacing;
				num2 = Count - 1;
				num3 = -1;
				num4 = -1;
			}
			for (int i = num2; i != num3; i += num4)
			{
				if (m_items[i].Visible)
				{
					Vector2 position = new Vector2((namesTopLeft.X + valuesTopRight.X) * 0.5f, namesTopLeft.Y + 0.5f * num);
					Vector2 textSize = new Vector2(Math.Abs(namesTopLeft.X - valuesTopRight.X), LineSpacing);
					MyGuiTextShadows.DrawShadow(ref position, ref textSize);
					namesTopLeft.Y += num;
					valuesTopRight.Y += num;
				}
			}
		}

		private void EnsureItemsExist()
		{
			m_items.Capacity = Math.Max(Count, m_items.Capacity);
			while (m_items.Count < Count)
			{
				m_items.Add(new Data());
			}
		}
	}
}
