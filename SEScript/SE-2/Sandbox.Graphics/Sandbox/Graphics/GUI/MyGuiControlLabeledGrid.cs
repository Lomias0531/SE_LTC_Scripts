using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlLabeledGrid : MyGuiControlGrid
	{
		public List<string> Labels = new List<string>();

		private StringBuilder textBuilder = new StringBuilder();

		public float TextScale = 1f;

		public MyGuiControlLabeledGrid()
		{
			m_styleDef.FitSizeToItems = false;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			DrawLabels(transitionAlpha);
		}

		private void DrawLabels(float transitionAlpha)
		{
			MyGuiBorderThickness itemPadding = m_styleDef.ItemPadding;
			string itemFontNormal = m_styleDef.ItemFontNormal;
			for (int i = 0; i < base.RowsCount; i++)
			{
				for (int j = 0; j < base.ColumnsCount; j++)
				{
					int num = ComputeIndex(i, j);
					MyGuiGridItem myGuiGridItem = TryGetItemAt(num);
					if (myGuiGridItem != null && Labels.IsValidIndex(num))
					{
						string value = Labels[num];
						textBuilder.Clear();
						textBuilder.Append(value);
						Vector2 normalizedCoord = m_itemsRectangle.Position + m_itemStep * new Vector2(j, i);
						normalizedCoord.X += m_itemStep.X + itemPadding.MarginStep.X;
						normalizedCoord.Y += m_itemStep.Y * 0.5f;
						if (base.Enabled)
						{
							_ = myGuiGridItem.Enabled;
						}
						else
							_ = 0;
						float maxTextWidth = Math.Abs(base.Size.X - normalizedCoord.X);
						MyGuiManager.DrawString(itemFontNormal, textBuilder, normalizedCoord, TextScale, MyGuiControlBase.ApplyColorMaskModifiers(myGuiGridItem.IconColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, useFullClientArea: false, maxTextWidth);
					}
				}
			}
		}

		public override void Clear()
		{
			base.Clear();
			Labels.Clear();
		}

		public void AddLabeledItem(MyGuiGridItem gridItem, string label)
		{
			Add(gridItem);
			Labels.Add(label);
		}
	}
}
