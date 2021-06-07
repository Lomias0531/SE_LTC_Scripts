using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlResearchGraph : MyGuiControlParent
	{
		public class GraphNode
		{
			public Vector2 Position
			{
				get;
				set;
			}

			public Vector2 Size
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}

			public List<MyGuiGridItem> Items
			{
				get;
				set;
			}

			public GraphNode Parent
			{
				get;
				set;
			}

			public List<GraphNode> Children
			{
				get;
				set;
			}

			public string UnlockedBy
			{
				get;
				set;
			}

			public GraphNode()
			{
				Items = new List<MyGuiGridItem>();
				Children = new List<GraphNode>();
			}
		}

		private readonly string simpleTexture = "Textures\\Fake.dds";

		public Dictionary<MyGuiGridItem, GraphNode> ItemToNode = new Dictionary<MyGuiGridItem, GraphNode>();

		private MyGuiStyleDefinition m_styleDef;

		private bool m_itemsLayoutInitialized;

		private MyGuiGridItem m_mouseOverItem;

		private MyGuiGridItem m_selectedItem;

		private Vector2 m_doubleClickFirstPosition;

		private int? m_doubleClickStarted;

		private Vector2 m_mouseDragStartPosition;

		private bool m_isItemDraggingLeft;

		public List<GraphNode> Nodes
		{
			get;
			set;
		}

		public Vector4 ItemBackgroundColorMask
		{
			get;
			set;
		}

		public Vector2 ItemSize
		{
			get;
			set;
		}

		public Vector2 NodePadding
		{
			get;
			set;
		}

		public Vector2 NodeMargin
		{
			get;
			set;
		}

		public MyGuiGridItem MouseOverItem
		{
			get
			{
				return m_mouseOverItem;
			}
			private set
			{
				if (m_mouseOverItem != value)
				{
					m_mouseOverItem = value;
					if (this.MouseOverItemChanged != null)
					{
						this.MouseOverItemChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		public MyGuiGridItem SelectedItem
		{
			get
			{
				return m_selectedItem;
			}
			set
			{
				if (m_selectedItem != value)
				{
					m_selectedItem = value;
					if (this.SelectedItemChanged != null)
					{
						this.SelectedItemChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		public override RectangleF FocusRectangle
		{
			get
			{
				if (SelectedItem == null)
				{
					return base.FocusRectangle;
				}
				return GetItemRectangle(SelectedItem);
			}
		}

		public event EventHandler<MySharedButtonsEnum> ItemClicked;

		public event EventHandler MouseOverItemChanged;

		public event EventHandler SelectedItemChanged;

		public event EventHandler ItemDoubleClicked;

		public event EventHandler<MyGuiGridItem> ItemDragged;

		public MyGuiControlResearchGraph()
			: base(Vector2.Zero, Vector2.Zero, MyGuiConstants.LISTBOX_BACKGROUND_COLOR)
		{
			ItemBackgroundColorMask = Vector4.One;
			MyGuiBorderThickness itemPadding = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			MyGuiBorderThickness itemMargin = new MyGuiBorderThickness(2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			MyGuiStyleDefinition customStyleDefinition = new MyGuiStyleDefinition
			{
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SizeOverride = MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui * new Vector2(10f, 1f),
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				ItemTextScale = 0.6f,
				FitSizeToItems = true
			};
			SetCustomStyleDefinition(customStyleDefinition);
		}

		public void SetCustomStyleDefinition(MyGuiStyleDefinition styleDef)
		{
			m_styleDef = styleDef;
			BorderEnabled = m_styleDef.BorderEnabled;
			BorderColor = m_styleDef.BorderColor;
			if (!m_itemsLayoutInitialized)
			{
				InitializeItemsLayout();
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (!base.Enabled)
			{
				return captureInput;
			}
			if (!base.IsMouseOver)
			{
				return captureInput;
			}
			Vector2 mouseCursorPosition = MyGuiManager.MouseCursorPosition;
			Vector2 value = GetPositionAbsoluteTopLeft() + m_styleDef.BackgroundPaddingSize + m_styleDef.ContentPadding.TopLeftOffset + m_styleDef.ItemMargin.TopLeftOffset;
			Vector2 controlMousePosition = mouseCursorPosition - value;
			MyGuiGridItem mouseOverItem = MouseOverItem;
			MouseOverItem = null;
			foreach (GraphNode node in Nodes)
			{
				CheckMouseOverNode(controlMousePosition, node);
			}
			if (mouseOverItem != MouseOverItem)
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			if (captureInput == null)
			{
				captureInput = HandleNewMousePress();
				HandleMouseDrag(ref captureInput, MySharedButtonsEnum.Primary, ref m_isItemDraggingLeft);
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT))
			{
				this.ItemDoubleClicked(this, EventArgs.Empty);
			}
			return captureInput;
		}

		private void CheckMouseOverNode(Vector2 controlMousePosition, GraphNode node)
		{
			foreach (MyGuiGridItem item in node.Items)
			{
				if (new RectangleF(item.Position, ItemSize).Contains(controlMousePosition))
				{
					MouseOverItem = item;
					break;
				}
			}
			foreach (GraphNode child in node.Children)
			{
				CheckMouseOverNode(controlMousePosition, child);
			}
		}

		private MyGuiControlBase HandleNewMousePress()
		{
			if (MouseOverItem == null)
			{
				return null;
			}
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed())
			{
				SelectedItem = MouseOverItem;
				MySharedButtonsEnum e = MySharedButtonsEnum.None;
				if (MyInput.Static.IsNewPrimaryButtonPressed())
				{
					e = MySharedButtonsEnum.Primary;
				}
				else if (MyInput.Static.IsNewSecondaryButtonPressed())
				{
					e = MySharedButtonsEnum.Secondary;
				}
				else if (MyInput.Static.IsNewMiddleMousePressed())
				{
					e = MySharedButtonsEnum.Ternary;
				}
				if (this.ItemClicked != null)
				{
					this.ItemClicked(this, e);
				}
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				if (!m_doubleClickStarted.HasValue)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
				else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
				{
					if (this.ItemDoubleClicked != null)
					{
						this.ItemDoubleClicked(this, EventArgs.Empty);
						MyGuiSoundManager.PlaySound(GuiSounds.Item);
					}
					m_doubleClickStarted = null;
					return this;
				}
			}
			return null;
		}

		private void HandleMouseDrag(ref MyGuiControlBase captureInput, MySharedButtonsEnum button, ref bool isDragging)
		{
			if (MyInput.Static.IsNewButtonPressed(button))
			{
				isDragging = true;
				m_mouseDragStartPosition = MyGuiManager.MouseCursorPosition;
			}
			else if (MyInput.Static.IsButtonPressed(button))
			{
				if (!isDragging || SelectedItem == null)
				{
					return;
				}
				if ((MyGuiManager.MouseCursorPosition - m_mouseDragStartPosition).Length() != 0f)
				{
					if (this.ItemDragged != null)
					{
						this.ItemDragged(this, SelectedItem);
					}
					isDragging = false;
				}
				captureInput = this;
			}
			else
			{
				isDragging = false;
			}
		}

		public override void Update()
		{
			base.Update();
			if (!m_itemsLayoutInitialized)
			{
				InitializeItemsLayout();
			}
		}

		public void InitializeItemsLayout()
		{
			if (m_styleDef != null && Nodes != null && Nodes.Count != 0)
			{
				Vector2 maxPosition = default(Vector2);
				Vector2 position = default(Vector2);
				for (int i = 0; i < Nodes.Count; i++)
				{
					GraphNode node = Nodes[i];
					InitializeNode(node, ref position, ref maxPosition);
					position.Y = maxPosition.Y + ItemSize.Y + NodePadding.Y + NodeMargin.Y;
					position.X = 0f;
				}
				Vector2 vector = maxPosition + ItemSize + NodePadding;
				Vector2 size = base.Size;
				if (vector.X > size.X)
				{
					size.X = vector.X;
				}
				if (vector.Y > size.Y)
				{
					size.Y = vector.Y;
				}
				base.Size = size;
				m_itemsLayoutInitialized = true;
			}
		}

		private void InitializeNode(GraphNode node, ref Vector2 position, ref Vector2 maxPosition)
		{
			float num = 0f - ItemSize.X + NodePadding.X;
			float num2 = NodePadding.Y;
			float num3 = float.MinValue;
			for (int i = 0; i < node.Items.Count; i++)
			{
				MyGuiGridItem myGuiGridItem = node.Items[i];
				num += ItemSize.X;
				if (position.X + num + ItemSize.X > base.Size.X)
				{
					num = NodePadding.X;
					num2 += ItemSize.Y;
				}
				myGuiGridItem.Position = position + new Vector2(num, num2);
				if (num > num3)
				{
					num3 = num;
				}
				if (i == 0)
				{
					node.Position = myGuiGridItem.Position - NodePadding - NodeMargin;
				}
				if (myGuiGridItem.Position.X > maxPosition.X)
				{
					maxPosition.X = myGuiGridItem.Position.X;
				}
				if (myGuiGridItem.Position.Y > maxPosition.Y)
				{
					maxPosition.Y = myGuiGridItem.Position.Y;
				}
			}
			node.Size = position + new Vector2(num3, num2) + ItemSize - node.Position + NodePadding - NodeMargin;
			position.Y += ItemSize.Y + num2 + NodePadding.Y + NodeMargin.Y;
			position.X += ItemSize.X;
			foreach (GraphNode child in node.Children)
			{
				InitializeNode(child, ref position, ref maxPosition);
			}
			position.X -= ItemSize.X;
		}

		public void InvalidateItemsLayout()
		{
			m_itemsLayoutInitialized = false;
		}

		public override void ShowToolTip()
		{
			base.ShowToolTip();
			if (MouseOverItem != null)
			{
				m_toolTip = MouseOverItem.ToolTip;
			}
			else
			{
				m_toolTip = null;
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			DrawGraph(backgroundTransitionAlpha);
		}

		private void DrawGraph(float transitionAlpha)
		{
			if (m_itemsLayoutInitialized)
			{
				Vector2 position = GetPositionAbsoluteTopLeft() + m_styleDef.BackgroundPaddingSize + m_styleDef.ContentPadding.TopLeftOffset + m_styleDef.ItemMargin.TopLeftOffset;
				for (int i = 0; i < Nodes.Count; i++)
				{
					GraphNode node = Nodes[i];
					position = DrawNode(node, null, position, transitionAlpha);
				}
			}
		}

		private Vector2 DrawNode(GraphNode node, GraphNode parentNode, Vector2 position, float transitionAlpha)
		{
			string normal = m_styleDef.ItemTexture.Normal;
			string highlight = m_styleDef.ItemTexture.Highlight;
			Vector4 sourceColorMask = new Vector4(61f / 255f, 74f / 255f, 82f / 255f, 1f);
			MyGuiManager.DrawSpriteBatch(simpleTexture, position + node.Position + NodeMargin, node.Size, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, 0.9f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (parentNode != null)
			{
				Vector2 position2 = parentNode.Position;
				position2.X += ItemSize.X / 2f + NodePadding.X;
				position2.Y = node.Position.Y + node.Size.Y / 2f;
				Vector2 normalizedSize = new Vector2(node.Position.X, position2.Y) - position2;
				normalizedSize.Y = 0.004f;
				position2.Y -= normalizedSize.Y;
				MyGuiManager.DrawSpriteBatch(simpleTexture, position + position2 + NodeMargin, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, 0.9f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			for (int i = 0; i < node.Items.Count; i++)
			{
				MyGuiGridItem myGuiGridItem = node.Items[i];
				Vector2 vector = position + myGuiGridItem.Position;
				bool flag = base.Enabled && (myGuiGridItem?.Enabled ?? true);
				bool flag2 = false;
				float num = 1f;
				flag2 = (MyGuiManager.TotalTimeInMilliseconds - myGuiGridItem.blinkCount <= 400);
				if (flag2)
				{
					num = myGuiGridItem.blinkingTransparency();
				}
				bool num2 = flag && (MouseOverItem == myGuiGridItem || SelectedItem == myGuiGridItem || flag2);
				Vector4 itemBackgroundColorMask = ItemBackgroundColorMask;
				MyGuiManager.DrawSpriteBatch(num2 ? highlight : normal, vector, ItemSize, MyGuiControlBase.ApplyColorMaskModifiers(itemBackgroundColorMask, flag, 0.9f * num), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				if (myGuiGridItem.OverlayPercent != 0f)
				{
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", vector, ItemSize * new Vector2(myGuiGridItem.OverlayPercent, 1f), MyGuiControlBase.ApplyColorMaskModifiers(itemBackgroundColorMask * myGuiGridItem.OverlayColorMask, flag, transitionAlpha * 0.5f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
				if (myGuiGridItem.Icons != null)
				{
					for (int j = 0; j < myGuiGridItem.Icons.Length; j++)
					{
						MyGuiManager.DrawSpriteBatch(myGuiGridItem.Icons[j], vector, ItemSize, MyGuiControlBase.ApplyColorMaskModifiers(itemBackgroundColorMask * myGuiGridItem.IconColorMask, flag, 0.9f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
					}
				}
				if (!string.IsNullOrWhiteSpace(myGuiGridItem.SubIcon))
				{
					MyGuiManager.DrawSpriteBatch(normalizedCoord: vector + new Vector2(ItemSize.X - ItemSize.X / 3f, 0f), texture: myGuiGridItem.SubIcon, normalizedSize: ItemSize / 3f, color: MyGuiControlBase.ApplyColorMaskModifiers(itemBackgroundColorMask * myGuiGridItem.IconColorMask, flag, 0.9f), drawAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
				if (!string.IsNullOrWhiteSpace(myGuiGridItem.SubIcon2))
				{
					MyGuiManager.DrawSpriteBatch(normalizedCoord: vector + new Vector2(ItemSize.X - ItemSize.X / 3.5f, ItemSize.Y - ItemSize.Y / 3.5f), texture: myGuiGridItem.SubIcon2, normalizedSize: ItemSize / 3.5f, color: MyGuiControlBase.ApplyColorMaskModifiers(itemBackgroundColorMask * myGuiGridItem.IconColorMask, flag, 0.9f), drawAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
			}
			float num3 = float.MinValue;
			float num4 = float.MaxValue;
			foreach (GraphNode child in node.Children)
			{
				DrawNode(child, node, position, transitionAlpha);
				if (child.Position.Y > num3)
				{
					num3 = child.Position.Y + child.Size.Y / 2f;
				}
				if (num4 > child.Position.X)
				{
					num4 = child.Position.X;
				}
			}
			if (node.Children.Count != 0)
			{
				Vector2 position3 = node.Position;
				position3.X += ItemSize.X / 2f + NodePadding.X;
				position3.Y += node.Size.Y;
				Vector2 normalizedSize2 = new Vector2(position3.X, num3) - position3;
				normalizedSize2.X = 0.003f;
				MyGuiManager.DrawSpriteBatch(simpleTexture, position + position3 + NodeMargin, normalizedSize2, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, 0.9f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			return position;
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (page)
			{
				if (direction == MyDirection.Down)
				{
					(base.Owner as MyGuiControlScrollablePanel).ScrollbarVPosition += 0.05f;
					return this;
				}
				if (direction == MyDirection.Up)
				{
					(base.Owner as MyGuiControlScrollablePanel).ScrollbarVPosition -= 0.05f;
					return this;
				}
				return base.GetNextFocusControl(currentFocusControl, direction, page);
			}
			if (currentFocusControl == this)
			{
				do
				{
					if (!ItemToNode.TryGetValue(SelectedItem, out GraphNode value) || !MoveSelected(value))
					{
						return base.GetNextFocusControl(currentFocusControl, direction, page);
					}
				}
				while (!SelectedItem.Enabled);
				MouseOverItem = SelectedItem;
				base.Owner?.OnFocusChanged(this, focus: true);
			}
			return this;
			GraphNode GetNextNode(GraphNode selectedNode, bool down)
			{
				if (down && selectedNode.Children.Count != 0)
				{
					return selectedNode.Children[0];
				}
				while (selectedNode.Parent != null)
				{
					int num = selectedNode.Parent.Children.IndexOf(selectedNode);
					num += (down ? 1 : (-1));
					selectedNode = selectedNode.Parent;
					if (num < selectedNode.Children.Count && num >= 0)
					{
						if (down)
						{
							return selectedNode.Children[num];
						}
						selectedNode = selectedNode.Children[num];
						while (selectedNode.Children.Count > 0)
						{
							selectedNode = selectedNode.Children[selectedNode.Children.Count - 1];
						}
						return selectedNode;
					}
					if (!down)
					{
						return selectedNode;
					}
				}
				int num2 = Nodes.IndexOf(selectedNode);
				num2 += (down ? 1 : (-1));
				if (num2 < Nodes.Count && num2 >= 0)
				{
					if (down)
					{
						return Nodes[num2];
					}
					selectedNode = Nodes[num2];
					while (selectedNode.Children.Count > 0)
					{
						selectedNode = selectedNode.Children[selectedNode.Children.Count - 1];
					}
					return selectedNode;
				}
				return null;
			}
			bool MoveSelected(GraphNode selectedNode)
			{
				int num3 = selectedNode.Items.IndexOf(SelectedItem);
				switch (direction)
				{
				case MyDirection.None:
				case MyDirection.Down:
					selectedNode = GetNextNode(selectedNode, down: true);
					num3 = 0;
					break;
				case MyDirection.Up:
					selectedNode = GetNextNode(selectedNode, down: false);
					num3 = 0;
					break;
				case MyDirection.Right:
					num3++;
					break;
				case MyDirection.Left:
					num3--;
					break;
				}
				if (selectedNode == null)
				{
					return false;
				}
				if (num3 >= selectedNode.Items.Count)
				{
					num3 = 0;
					selectedNode = GetNextNode(selectedNode, down: true);
				}
				if (num3 < 0)
				{
					num3 = 0;
					selectedNode = GetNextNode(selectedNode, down: false);
				}
				if (selectedNode == null)
				{
					return false;
				}
				SelectedItem = selectedNode.Items[num3];
				return true;
			}
		}

		private RectangleF GetItemRectangle(MyGuiGridItem item)
		{
			return new RectangleF(GetPositionAbsoluteTopLeft() + item.Position, ItemSize);
		}

		internal override void OnFocusChanged(bool focus)
		{
			base.OnFocusChanged(focus);
			base.Owner?.OnFocusChanged(this, focus);
		}
	}
}
