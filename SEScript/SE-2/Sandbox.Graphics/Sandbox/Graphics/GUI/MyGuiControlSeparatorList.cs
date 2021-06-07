using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlSeparatorList))]
	public class MyGuiControlSeparatorList : MyGuiControlBase
	{
		public struct Separator
		{
			public Vector2 Start;

			public Vector2 Size;

			public Vector4 Color;

			public bool Visible;
		}

		private List<Separator> m_separators;

		public MyGuiControlSeparatorList()
			: base(null, null, null, null, null, isActiveControl: false, canHaveFocus: false, MyGuiControlHighlightType.NEVER)
		{
			m_separators = new List<Separator>();
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlSeparatorList myObjectBuilder_GuiControlSeparatorList = (MyObjectBuilder_GuiControlSeparatorList)builder;
			m_separators.Clear();
			m_separators.Capacity = myObjectBuilder_GuiControlSeparatorList.Separators.Count;
			foreach (MyObjectBuilder_GuiControlSeparatorList.Separator separator in myObjectBuilder_GuiControlSeparatorList.Separators)
			{
				m_separators.Add(new Separator
				{
					Start = new Vector2(separator.StartX, separator.StartY),
					Size = new Vector2(separator.SizeX, separator.SizeY)
				});
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlSeparatorList myObjectBuilder_GuiControlSeparatorList = (MyObjectBuilder_GuiControlSeparatorList)base.GetObjectBuilder();
			myObjectBuilder_GuiControlSeparatorList.Separators = new List<MyObjectBuilder_GuiControlSeparatorList.Separator>(m_separators.Count);
			foreach (Separator separator in m_separators)
			{
				myObjectBuilder_GuiControlSeparatorList.Separators.Add(new MyObjectBuilder_GuiControlSeparatorList.Separator
				{
					StartX = separator.Start.X,
					StartY = separator.Start.Y,
					SizeX = separator.Size.X,
					SizeY = separator.Size.Y
				});
			}
			return myObjectBuilder_GuiControlSeparatorList;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			GetPositionAbsoluteCenter();
			foreach (Separator separator in m_separators)
			{
				if (separator.Visible)
				{
					Color color = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask * separator.Color, base.Enabled, transitionAlpha);
					Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(GetPositionAbsoluteCenter() + separator.Start);
					Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(separator.Size);
					if (screenSizeFromNormalizedSize.X == 0f)
					{
						screenSizeFromNormalizedSize.X += 1f;
					}
					else if (screenSizeFromNormalizedSize.Y == 0f)
					{
						screenSizeFromNormalizedSize.Y += 1f;
					}
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y, (int)screenSizeFromNormalizedSize.X, (int)screenSizeFromNormalizedSize.Y, color);
				}
			}
		}

		public Separator AddHorizontal(Vector2 start, float length, float width = 0f, Vector4? color = null)
		{
			Separator separator = default(Separator);
			separator.Start = start;
			separator.Size = new Vector2(length, width);
			separator.Color = (color ?? MyGuiConstants.THEMED_GUI_LINE_COLOR.ToVector4());
			separator.Visible = true;
			Separator separator2 = separator;
			m_separators.Add(separator2);
			return separator2;
		}

		public Separator AddVertical(Vector2 start, float length, float width = 0f, Vector4? color = null)
		{
			Separator separator = default(Separator);
			separator.Start = start;
			separator.Size = new Vector2(width, length);
			separator.Color = (color ?? MyGuiConstants.THEMED_GUI_LINE_COLOR.ToVector4());
			separator.Visible = true;
			Separator separator2 = separator;
			m_separators.Add(separator2);
			return separator2;
		}

		public new void Clear()
		{
			m_separators.Clear();
		}
	}
}
