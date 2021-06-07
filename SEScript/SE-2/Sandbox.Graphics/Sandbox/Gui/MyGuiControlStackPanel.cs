using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Gui
{
	public class MyGuiControlStackPanel : MyGuiControlBase
	{
		private List<MyGuiControlBase> m_controls;

		public MyGuiOrientation Orientation
		{
			get;
			set;
		}

		public bool ArrangeInvisible
		{
			get;
			set;
		}

		public MyGuiControlStackPanel()
		{
			m_controls = new List<MyGuiControlBase>();
			ArrangeInvisible = false;
		}

		public void Add(MyGuiControlBase control)
		{
			m_controls.Add(control);
		}

		public void AddAt(int idx, MyGuiControlBase control)
		{
			m_controls.Insert(idx, control);
		}

		public void Remove(MyGuiControlBase control)
		{
			if (m_controls.Contains(control))
			{
				m_controls.Remove(control);
			}
		}

		public void RemoveAt(int idx)
		{
			if (idx >= 0 && idx < m_controls.Count)
			{
				m_controls.RemoveAt(idx);
			}
		}

		public MyGuiControlBase GetAt(int idx)
		{
			if (idx < 0 || idx >= m_controls.Count)
			{
				return null;
			}
			return m_controls[idx];
		}

		public List<MyGuiControlBase> GetControls(bool onlyVisible = true)
		{
			List<MyGuiControlBase> list = new List<MyGuiControlBase>();
			foreach (MyGuiControlBase control in m_controls)
			{
				if (control.Visible || !onlyVisible)
				{
					list.Add(control);
				}
			}
			return list;
		}

		public int GetControlCount()
		{
			return m_controls.Count;
		}

		public override void UpdateMeasure()
		{
			base.UpdateMeasure();
			Vector2 size = default(Vector2);
			for (int i = 0; i < m_controls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = m_controls[i];
				if (myGuiControlBase.Visible || ArrangeInvisible)
				{
					myGuiControlBase.UpdateMeasure();
					if (Orientation == MyGuiOrientation.Horizontal)
					{
						size.Y = Math.Max(size.Y, myGuiControlBase.Size.Y + myGuiControlBase.Margin.Top + myGuiControlBase.Margin.Bottom);
						size.X += myGuiControlBase.Size.X + myGuiControlBase.Margin.Left + myGuiControlBase.Margin.Right;
					}
					else
					{
						size.X = Math.Max(size.X, myGuiControlBase.Size.X + myGuiControlBase.Margin.Right + myGuiControlBase.Margin.Left);
						size.Y += myGuiControlBase.Size.Y + myGuiControlBase.Margin.Top + myGuiControlBase.Margin.Bottom;
					}
				}
			}
			base.Size = size;
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
			MyGuiDrawAlignEnum originAlign = base.OriginAlign;
			Vector2 vector = (originAlign != MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER) ? base.Position : (base.Position - new Vector2(0f, base.Size.Y / 2f));
			for (int i = 0; i < m_controls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = m_controls[i];
				if (myGuiControlBase.Visible || ArrangeInvisible)
				{
					if (Orientation == MyGuiOrientation.Horizontal)
					{
						vector.X += myGuiControlBase.Margin.Left;
						myGuiControlBase.Position = new Vector2(vector.X, vector.Y + myGuiControlBase.Margin.Top);
						vector.X += myGuiControlBase.Margin.Right + myGuiControlBase.Size.X;
					}
					else
					{
						vector.Y += myGuiControlBase.Margin.Top;
						myGuiControlBase.Position = new Vector2(base.PositionX + myGuiControlBase.Margin.Left, vector.Y);
						vector.Y += myGuiControlBase.Margin.Bottom + myGuiControlBase.Size.Y;
					}
					myGuiControlBase.UpdateArrange();
				}
			}
		}

		public override MyGuiControlBase GetMouseOverControl()
		{
			for (int num = m_controls.Count - 1; num >= 0; num--)
			{
				if (m_controls[num].Visible && m_controls[num].IsHitTestVisible && m_controls[num].IsMouseOver)
				{
					return m_controls[num];
				}
			}
			return null;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = null;
			base.IsMouseOver = CheckMouseOver();
			foreach (MyGuiControlBase control in m_controls)
			{
				if (!(control is MyGuiControlParent))
				{
					myGuiControlBase = control.HandleInput();
					if (myGuiControlBase != null)
					{
						break;
					}
				}
			}
			if (myGuiControlBase != null)
			{
				myGuiControlBase = base.HandleInput();
			}
			return myGuiControlBase;
		}

		public override bool IsMouseOverAnyControl()
		{
			foreach (MyGuiControlBase control in m_controls)
			{
				if (control.IsHitTestVisible && control.IsMouseOver)
				{
					return true;
				}
			}
			return false;
		}
	}
}
