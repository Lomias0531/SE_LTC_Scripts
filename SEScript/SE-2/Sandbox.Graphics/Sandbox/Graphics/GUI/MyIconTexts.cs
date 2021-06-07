using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyIconTexts : Dictionary<MyGuiDrawAlignEnum, MyColoredText>
	{
		private Vector2 GetPosition(Vector2 iconPosition, Vector2 iconSize, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return iconPosition + new Vector2(iconSize.X / 2f, iconSize.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return iconPosition + new Vector2(iconSize.X / 2f, iconSize.Y / 2f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return iconPosition + new Vector2(iconSize.X / 2f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return iconPosition + new Vector2(0f, iconSize.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return iconPosition + new Vector2(0f, iconSize.Y / 2f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return iconPosition;
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return iconPosition + new Vector2(iconSize.X, iconSize.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return iconPosition + new Vector2(iconSize.X, iconSize.Y / 2f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return iconPosition + new Vector2(iconSize.X, 0f);
			default:
				throw new Exception();
			}
		}

		public void Draw(Vector2 iconPosition, Vector2 iconSize, float backgroundAlphaFade, float colorMultiplicator = 1f)
		{
			Draw(iconPosition, iconSize, backgroundAlphaFade, isHighlight: false, colorMultiplicator);
		}

		public void Draw(Vector2 iconPosition, Vector2 iconSize, float backgroundAlphaFade, bool isHighlight, float colorMultiplicator = 1f)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<MyGuiDrawAlignEnum, MyColoredText> current = enumerator.Current;
					Vector2 position = GetPosition(iconPosition, iconSize, current.Key);
					current.Value.Draw(position, current.Key, backgroundAlphaFade, isHighlight, colorMultiplicator);
				}
			}
		}
	}
}
