using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Gui
{
	public class MyGuiControlWrapPanel : MyGuiControlBase
	{
		private List<MyGuiControlBase> m_controls;

		public Vector2 ItemSize;

		private int m_itemsPerRow;

		public bool ArrangeInvisible
		{
			get;
			set;
		}

		public Vector2 InnerOffset
		{
			get;
			set;
		}

		public MyGuiControlWrapPanel(Vector2 itemSize)
		{
			m_controls = new List<MyGuiControlBase>();
			ArrangeInvisible = false;
			ItemSize = itemSize;
		}

		public void Add(MyGuiControlBase control)
		{
			m_controls.Add(control);
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

		public override void UpdateMeasure()
		{
			base.UpdateMeasure();
			float num = base.Size.X - base.Margin.Left - base.Margin.Right - ItemSize.X;
			int count = GetControls().Count;
			m_itemsPerRow = (int)(num / (ItemSize.X + InnerOffset.X)) + 1;
			int num2 = (count + m_itemsPerRow - 1) / m_itemsPerRow;
			Vector2 vector2 = base.Size = new Vector2(base.Size.X, (float)num2 * (ItemSize.Y + InnerOffset.Y) - InnerOffset.Y + base.Margin.Top + base.Margin.Bottom);
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
			List<MyGuiControlBase> controls = GetControls();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < controls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = controls[i];
				Vector2 vector2 = myGuiControlBase.Position = new Vector2(base.Margin.Left + (ItemSize.X + InnerOffset.X) * (float)num, base.Margin.Top + (ItemSize.Y + InnerOffset.Y) * (float)num2) + base.Position;
				num++;
				if (num >= m_itemsPerRow)
				{
					num = 0;
					num2++;
				}
				myGuiControlBase.UpdateArrange();
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
				myGuiControlBase = control.HandleInput();
				if (myGuiControlBase != null)
				{
					break;
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
