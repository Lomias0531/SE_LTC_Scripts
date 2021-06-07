using VRage.Game;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlParent))]
	public class MyGuiControlParent : MyGuiControlBase, IMyGuiControlsParent, IMyGuiControlsOwner
	{
		private MyGuiControls m_controls;

		public MyGuiControls Controls => m_controls;

		public MyGuiControlParent()
			: this(null, null, null, null)
		{
		}

		public MyGuiControlParent(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string toolTip = null)
			: base(position, size, backgroundColor, toolTip, null, isActiveControl: true, canHaveFocus: true)
		{
			m_controls = new MyGuiControls(this);
			base.CanFocusChildren = true;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlParent myObjectBuilder_GuiControlParent = builder as MyObjectBuilder_GuiControlParent;
			if (myObjectBuilder_GuiControlParent.Controls != null)
			{
				m_controls.Init(myObjectBuilder_GuiControlParent.Controls);
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlParent obj = base.GetObjectBuilder() as MyObjectBuilder_GuiControlParent;
			obj.Controls = Controls.GetObjectBuilder();
			return obj;
		}

		public override void Clear()
		{
			Controls.Clear();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				if (visibleControl.GetExclusiveInputHandler() != visibleControl && !(visibleControl is MyGuiControlGridDragAndDrop))
				{
					visibleControl.Draw(transitionAlpha * visibleControl.Alpha, backgroundTransitionAlpha * visibleControl.Alpha);
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = null;
			myGuiControlBase = base.HandleInput();
			base.IsMouseOver = true;
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				myGuiControlBase = visibleControl.HandleInput();
				if (myGuiControlBase != null)
				{
					return myGuiControlBase;
				}
			}
			return myGuiControlBase;
		}

		public override MyGuiControlBase GetExclusiveInputHandler()
		{
			MyGuiControlBase exclusiveInputHandler = MyGuiControlBase.GetExclusiveInputHandler(Controls);
			if (exclusiveInputHandler == null)
			{
				exclusiveInputHandler = base.GetExclusiveInputHandler();
			}
			return exclusiveInputHandler;
		}

		public override bool IsMouseOverAnyControl()
		{
			for (int num = Controls.GetVisibleControls().Count - 1; num >= 0; num--)
			{
				if (!Controls.GetVisibleControls()[num].IsHitTestVisible && Controls.GetVisibleControls()[num].IsMouseOver)
				{
					return true;
				}
			}
			return false;
		}

		public override MyGuiControlBase GetMouseOverControl()
		{
			for (int num = Controls.GetVisibleControls().Count - 1; num >= 0; num--)
			{
				if (Controls.GetVisibleControls()[num].IsHitTestVisible && Controls.GetVisibleControls()[num].IsMouseOver)
				{
					return Controls.GetVisibleControls()[num];
				}
			}
			return null;
		}

		public override MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			for (int i = 0; i < Controls.GetVisibleControls().Count; i++)
			{
				MyGuiControlBase myGuiControlBase = Controls.GetVisibleControls()[i];
				if (myGuiControlBase is MyGuiControlGridDragAndDrop)
				{
					MyGuiControlGridDragAndDrop myGuiControlGridDragAndDrop = (MyGuiControlGridDragAndDrop)myGuiControlBase;
					if (myGuiControlGridDragAndDrop.IsActive())
					{
						return myGuiControlGridDragAndDrop;
					}
				}
			}
			return null;
		}

		public override void HideToolTip()
		{
			foreach (MyGuiControlBase control in Controls)
			{
				control.HideToolTip();
			}
		}

		public override void ShowToolTip()
		{
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				visibleControl.ShowToolTip();
			}
		}

		public override void Update()
		{
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				visibleControl.Update();
			}
			base.Update();
		}

		public override void OnRemoving()
		{
			Controls.Clear();
			base.OnRemoving();
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			return MyGuiScreenBase.GetNextFocusControl(currentFocusControl, null, this, Controls.GetVisibleControls(), Elements.GetVisibleControls(), direction, page);
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
		}

		public override void UpdateMeasure()
		{
			base.UpdateMeasure();
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			base.OnFocusChanged(control, focus);
			base.Owner?.OnFocusChanged(control, focus);
		}
	}
}
