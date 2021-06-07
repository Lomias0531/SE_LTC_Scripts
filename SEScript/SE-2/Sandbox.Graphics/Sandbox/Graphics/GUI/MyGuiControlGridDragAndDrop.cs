using System.Collections.Generic;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlGridDragAndDrop : MyGuiControlBase
	{
		private MyGuiGridItem m_draggingGridItem;

		private List<MyGuiControlListbox> m_listboxesToDrop;

		private MyDragAndDropInfo m_draggingFrom;

		private Vector4 m_textColor;

		private float m_textScale;

		private Vector2 m_textOffset;

		private bool m_supportIcon;

		private MyDropHandleType? m_currentDropHandleType;

		private MySharedButtonsEnum? m_dragButton;

		private List<MyGuiControlBase> m_dropToControls = new List<MyGuiControlBase>();

		public bool DrawBackgroundTexture
		{
			get;
			set;
		}

		public event OnItemDropped ItemDropped;

		public MyGuiControlGridDragAndDrop(Vector4 backgroundColor, Vector4 textColor, float textScale, Vector2 textOffset, bool supportIcon)
			: base(new Vector2(0f, 0f), MyGuiConstants.DRAG_AND_DROP_SMALL_SIZE, backgroundColor)
		{
			m_textColor = textColor;
			m_textScale = textScale;
			m_textOffset = textOffset;
			m_supportIcon = supportIcon;
			DrawBackgroundTexture = true;
		}

		public override bool CheckMouseOver()
		{
			return IsActive();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (!IsActive())
			{
				return;
			}
			if (DrawBackgroundTexture)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", MyGuiManager.MouseCursorPosition, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask * new Color(50, 66, 70, 255).ToVector4(), enabled: true, backgroundTransitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
			Vector2 vector = MyGuiManager.MouseCursorPosition - base.Size / 2f;
			Vector2 vector2 = vector + m_textOffset;
			vector2.Y += base.Size.Y / 2f;
			if (m_supportIcon && m_draggingGridItem.Icons != null)
			{
				for (int i = 0; i < m_draggingGridItem.Icons.Length; i++)
				{
					MyGuiManager.DrawSpriteBatch(m_draggingGridItem.Icons[i], vector, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, enabled: true, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
			}
			ShowToolTip();
		}

		public override void ShowToolTip()
		{
			if (IsActive() && m_toolTip != null && m_toolTip.HasContent)
			{
				m_toolTipPosition = MyGuiManager.MouseCursorPosition;
				m_toolTip.Draw(m_toolTipPosition);
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput == null && IsActive())
			{
				switch (m_currentDropHandleType.Value)
				{
				case MyDropHandleType.MouseRelease:
					HandleButtonPressedDrop(m_dragButton.Value, ref captureInput);
					break;
				case MyDropHandleType.MouseClick:
					HandleButtonClickDrop(m_dragButton.Value, ref captureInput);
					break;
				}
			}
			return null;
		}

		public override MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			return this;
		}

		public void StartDragging(MyDropHandleType dropHandleType, MySharedButtonsEnum dragButton, MyGuiGridItem draggingItem, MyDragAndDropInfo draggingFrom, bool includeTooltip = true)
		{
			m_currentDropHandleType = dropHandleType;
			m_dragButton = dragButton;
			m_draggingGridItem = draggingItem;
			m_draggingFrom = draggingFrom;
			m_toolTip = (includeTooltip ? draggingItem.ToolTip : null);
		}

		public void Stop()
		{
			m_draggingFrom = null;
			m_draggingGridItem = null;
			m_currentDropHandleType = null;
			m_dragButton = null;
		}

		public bool IsActive()
		{
			if (m_draggingGridItem != null && m_draggingFrom != null && m_currentDropHandleType.HasValue)
			{
				return m_dragButton.HasValue;
			}
			return false;
		}

		public bool IsEmptySpace()
		{
			if (m_dropToControls != null)
			{
				return m_dropToControls.Count == 0;
			}
			return false;
		}

		public void Drop()
		{
			if (IsActive())
			{
				MyDragAndDropInfo myDragAndDropInfo = null;
				m_dropToControls.Clear();
				MyScreenManager.GetControlsUnderMouseCursor(m_dropToControls, visibleOnly: true);
				foreach (MyGuiControlBase dropToControl in m_dropToControls)
				{
					MyGuiControlGrid myGuiControlGrid = dropToControl as MyGuiControlGrid;
					if (myGuiControlGrid != null && myGuiControlGrid.Enabled && myGuiControlGrid.MouseOverIndex >= 0 && myGuiControlGrid.MouseOverIndex < myGuiControlGrid.MaxItemCount)
					{
						myDragAndDropInfo = new MyDragAndDropInfo();
						myDragAndDropInfo.Grid = myGuiControlGrid;
						myDragAndDropInfo.ItemIndex = myGuiControlGrid.MouseOverIndex;
						break;
					}
				}
				this.ItemDropped(this, new MyDragAndDropEventArgs
				{
					DragFrom = m_draggingFrom,
					DropTo = myDragAndDropInfo,
					Item = m_draggingGridItem,
					DragButton = m_dragButton.Value
				});
				Stop();
			}
		}

		private void HandleButtonPressedDrop(MySharedButtonsEnum button, ref MyGuiControlBase captureInput)
		{
			if (MyInput.Static.IsButtonPressed(button))
			{
				captureInput = this;
			}
			else
			{
				HandleDropingItem();
			}
		}

		private void HandleButtonClickDrop(MySharedButtonsEnum button, ref MyGuiControlBase captureInput)
		{
			if (MyInput.Static.IsNewButtonPressed(button))
			{
				HandleDropingItem();
				captureInput = this;
			}
		}

		private void HandleDropingItem()
		{
			if (IsActive())
			{
				Drop();
			}
		}
	}
}
