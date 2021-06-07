using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Gui
{
	public class MyGuiControlLayoutGrid : MyGuiControlBase
	{
		private GridLength[] m_columns;

		private GridLength[] m_rows;

		private List<MyGuiControlBase>[,] m_controlTable;

		public MyGuiControlLayoutGrid(GridLength[] columns, GridLength[] rows)
		{
			m_columns = columns;
			m_rows = rows;
			m_controlTable = new List<MyGuiControlBase>[rows.Length, columns.Length];
		}

		public bool Add(MyGuiControlBase control, int column, int row)
		{
			if (row >= 0 && row < m_rows.Length && column >= 0 && column < m_columns.Length)
			{
				if (m_controlTable[row, column] == null)
				{
					m_controlTable[row, column] = new List<MyGuiControlBase>();
				}
				m_controlTable[row, column].Add(control);
				return true;
			}
			return false;
		}

		public List<MyGuiControlBase> GetControlsAt(int column, int row)
		{
			if (row >= 0 && row < m_rows.Length && column >= 0 && column < m_columns.Length)
			{
				return m_controlTable[row, column];
			}
			return null;
		}

		public MyGuiControlBase GetFirstControlAt(int column, int row)
		{
			if (row >= 0 && row < m_rows.Length && column >= 0 && column < m_columns.Length && m_controlTable[row, column].Count > 0)
			{
				return m_controlTable[row, column][0];
			}
			return null;
		}

		public List<MyGuiControlBase> GetAllControls()
		{
			List<MyGuiControlBase> list = new List<MyGuiControlBase>();
			for (int i = 0; i < m_rows.Length; i++)
			{
				for (int j = 0; j < m_columns.Length; j++)
				{
					if (m_controlTable[i, j] != null)
					{
						foreach (MyGuiControlBase item in m_controlTable[i, j])
						{
							list.Add(item);
						}
					}
				}
			}
			return list;
		}

		public override void UpdateMeasure()
		{
			base.UpdateMeasure();
			for (int i = 0; i < m_rows.Length; i++)
			{
				for (int j = 0; j < m_columns.Length; j++)
				{
					if (m_controlTable[i, j] != null)
					{
						foreach (MyGuiControlBase item in m_controlTable[i, j])
						{
							item?.UpdateMeasure();
						}
					}
				}
			}
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
			Vector2 size = base.Size;
			Vector2 zero = Vector2.Zero;
			GridLength[] rows = m_rows;
			for (int i = 0; i < rows.Length; i++)
			{
				GridLength gridLength = rows[i];
				if (gridLength.UnitType == GridUnitType.FixValue)
				{
					size.Y -= gridLength.Size;
				}
				else
				{
					zero.Y += gridLength.Size;
				}
			}
			rows = m_columns;
			for (int i = 0; i < rows.Length; i++)
			{
				GridLength gridLength2 = rows[i];
				if (gridLength2.UnitType == GridUnitType.FixValue)
				{
					size.X -= gridLength2.Size;
				}
				else
				{
					zero.X += gridLength2.Size;
				}
			}
			Vector2 vector = new Vector2((zero.X > 0f) ? (size.X / zero.X) : 0f, (zero.Y > 0f) ? (size.Y / zero.Y) : 0f);
			Vector2 position = base.Position;
			for (int j = 0; j < m_rows.Length; j++)
			{
				position.X = base.PositionX;
				for (int k = 0; k < m_columns.Length; k++)
				{
					if (m_controlTable[j, k] != null)
					{
						foreach (MyGuiControlBase item in m_controlTable[j, k])
						{
							if (item != null)
							{
								item.Position = new Vector2(position.X + item.Margin.Left, position.Y + item.Margin.Top);
								item.UpdateArrange();
							}
						}
					}
					if (m_columns[k].UnitType == GridUnitType.FixValue)
					{
						position.X += m_columns[k].Size;
					}
					else
					{
						position.X += m_columns[k].Size * vector.X;
					}
				}
				if (m_rows[j].UnitType == GridUnitType.FixValue)
				{
					position.Y += m_rows[j].Size;
				}
				else
				{
					position.Y += m_rows[j].Size * vector.Y;
				}
			}
		}
	}
}
