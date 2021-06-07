using System;
using System.Collections.Specialized;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyToolTips
	{
		private MyGuiControlBase m_tooltipControl;

		public readonly ObservableCollection<MyColoredText> ToolTips;

		public bool RecalculateOnChange = true;

		public Vector2 Size;

		public string Background;

		public Color? ColorMask;

		public bool Highlight
		{
			get;
			set;
		}

		public Vector4 HighlightColor
		{
			get;
			set;
		}

		public MyGuiControlBase TooltipControl
		{
			get
			{
				return m_tooltipControl;
			}
			set
			{
				m_tooltipControl = value;
				if (m_tooltipControl != null)
				{
					Size = m_tooltipControl.Size;
				}
				else
				{
					RecalculateSize();
				}
			}
		}

		public bool HasContent
		{
			get
			{
				if (m_tooltipControl == null)
				{
					return ToolTips.Count > 0;
				}
				return true;
			}
		}

		public MyToolTips()
		{
			Background = null;
			ColorMask = null;
			ToolTips = new ObservableCollection<MyColoredText>();
			ToolTips.CollectionChanged += ToolTips_CollectionChanged;
			Size = new Vector2(-1f);
			HighlightColor = Color.Orange.ToVector4();
		}

		private void ToolTips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (RecalculateOnChange)
			{
				RecalculateSize();
			}
		}

		public MyToolTips(string toolTip)
			: this()
		{
			AddToolTip(toolTip);
		}

		public void AddToolTip(string toolTip, float textScale = 0.7f, string font = "Blue")
		{
			if (toolTip != null)
			{
				ToolTips.Add(new MyColoredText(toolTip, Color.White, null, font, textScale));
			}
		}

		public void RecalculateSize()
		{
			float x = 0f;
			float num = 4f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
			bool flag = true;
			for (int i = 0; i < ToolTips.Count; i++)
			{
				if (ToolTips[i].Text.Length > 0)
				{
					flag = false;
				}
				Vector2 vector = MyGuiManager.MeasureString("Blue", ToolTips[i].Text, ToolTips[i].ScaleWithLanguage);
				x = Math.Max(Size.X, vector.X);
				num += vector.Y;
			}
			if (flag)
			{
				Size.X = -1f;
				Size.Y = -1f;
			}
			else
			{
				Size.X = x;
				Size.Y = num;
			}
		}

		public void Draw(Vector2 mousePosition)
		{
			Vector2 value = mousePosition + MyGuiConstants.TOOL_TIP_RELATIVE_DEFAULT_POSITION;
			if (Size.X > -1f)
			{
				Vector2 value2 = new Vector2(0.005f, 0.002f);
				Vector2 vector = Size + 2f * value2;
				Vector2 vector2 = value - new Vector2(value2.X, 0f);
				Rectangle rectangle = MyGuiManager.FullscreenHudEnabled ? MyGuiManager.GetFullscreenRectangle() : MyGuiManager.GetSafeFullscreenRectangle();
				Vector2 vector3 = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(new Vector2(rectangle.Left, rectangle.Top)) + new Vector2(MyGuiConstants.TOOLTIP_DISTANCE_FROM_BORDER);
				Vector2 vector4 = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(new Vector2(rectangle.Right, rectangle.Bottom)) - new Vector2(MyGuiConstants.TOOLTIP_DISTANCE_FROM_BORDER);
				if (vector2.X + vector.X > vector4.X)
				{
					vector2.X = vector4.X - vector.X;
				}
				if (vector2.Y + vector.Y > vector4.Y)
				{
					vector2.Y = vector4.Y - vector.Y;
				}
				if (vector2.X < vector3.X)
				{
					vector2.X = vector3.X;
				}
				if (vector2.Y < vector3.Y)
				{
					vector2.Y = vector3.Y;
				}
				if (Highlight)
				{
					Vector2 vector5 = new Vector2(0.003f, 0.004f);
					Vector2 positionLeftTop = vector2 - vector5;
					Vector2 size = vector + 2f * vector5;
					MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.Draw(positionLeftTop, size, HighlightColor);
				}
				if (TooltipControl != null)
				{
					TooltipControl.Position = vector2;
					TooltipControl.Update();
					TooltipControl.Draw(1f, 1f);
				}
				else
				{
					Color color = ColorMask ?? MyGuiConstants.THEMED_GUI_BACKGROUND_COLOR;
					color.A = 230;
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\TooltipBackground.dds", vector2, vector, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					Vector2 normalizedPosition = vector2 + new Vector2(value2.X, vector.Y / 2f - Size.Y / 2f);
					foreach (MyColoredText toolTip in ToolTips)
					{
						toolTip.Draw(normalizedPosition, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 1f, isHighlight: false);
						normalizedPosition.Y += toolTip.Size.Y;
					}
				}
			}
		}
	}
}
