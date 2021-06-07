using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public class MyBrushGUIPropertyNumberCombo : IMyVoxelBrushGUIProperty
	{
		private MyGuiControlLabel m_label;

		private MyGuiControlCombobox m_combo;

		public Action ItemSelected;

		public long SelectedKey;

		public MyBrushGUIPropertyNumberCombo(MyVoxelBrushGUIPropertyOrder order, MyStringId labelText)
		{
			Vector2 position = new Vector2(-0.1f, -0.15f);
			Vector2 position2 = new Vector2(-0.1f, -0.12f);
			switch (order)
			{
			case MyVoxelBrushGUIPropertyOrder.Second:
				position.Y = -0.07f;
				position2.Y = -0.04f;
				break;
			case MyVoxelBrushGUIPropertyOrder.Third:
				position.Y = 0.01f;
				position2.Y = 0.04f;
				break;
			}
			m_label = new MyGuiControlLabel
			{
				Position = position,
				TextEnum = labelText,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_combo = new MyGuiControlCombobox();
			m_combo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_combo.Position = position2;
			m_combo.Size = new Vector2(0.263f, 0.1f);
			m_combo.ItemSelected += Combo_ItemSelected;
		}

		public void AddItem(long key, MyStringId text)
		{
			m_combo.AddItem(key, text);
		}

		public void SelectItem(long key)
		{
			m_combo.SelectItemByKey(key);
		}

		private void Combo_ItemSelected()
		{
			SelectedKey = m_combo.GetSelectedKey();
			if (ItemSelected != null)
			{
				ItemSelected();
			}
		}

		public void AddControlsToList(List<MyGuiControlBase> list)
		{
			list.Add(m_label);
			list.Add(m_combo);
		}
	}
}
