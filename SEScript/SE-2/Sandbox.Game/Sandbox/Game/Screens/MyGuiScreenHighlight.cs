using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenHighlight : MyGuiScreenBase
	{
		public struct MyHighlightControl
		{
			public MyGuiControlBase Control;

			public int[] Indices;

			public Color? Color;

			public MyToolTips CustomToolTips;
		}

		private uint m_closeInFrames = uint.MaxValue;

		private readonly MyGuiControls m_highlightedControls;

		private readonly MyHighlightControl[] m_highlightedControlsData;

		private static readonly Vector2 HIGHLIGHT_TEXTURE_SIZE = new Vector2(MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.LeftCenter.SizeGui.X + MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.RightCenter.SizeGui.X, MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterTop.SizeGui.Y + MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterBottom.SizeGui.Y);

		private static readonly Vector2 HIGHLIGHT_TEXTURE_OFFSET = new Vector2(MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.LeftCenter.SizeGui.X, MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterTop.SizeGui.Y);

		public override MyGuiControls Controls => m_highlightedControls;

		public override string GetFriendlyName()
		{
			return "HighlightScreen";
		}

		public override int GetTransitionOpeningTime()
		{
			return 500;
		}

		public override int GetTransitionClosingTime()
		{
			return 500;
		}

		public static void HighlightControls(MyHighlightControl[] controlsData)
		{
			MyScreenManager.AddScreen(new MyGuiScreenHighlight(controlsData));
		}

		public static void HighlightControl(MyHighlightControl control)
		{
			HighlightControls(new MyHighlightControl[1]
			{
				control
			});
		}

		private MyGuiScreenHighlight(MyHighlightControl[] controlsData)
			: base(Vector2.Zero, null, Vector2.One * 2.5f)
		{
			m_highlightedControlsData = controlsData;
			m_highlightedControls = new MyGuiControls(this);
			MyHighlightControl[] highlightedControlsData = m_highlightedControlsData;
			for (int i = 0; i < highlightedControlsData.Length; i++)
			{
				MyHighlightControl myHighlightControl = highlightedControlsData[i];
				if (myHighlightControl.CustomToolTips != null)
				{
					myHighlightControl.CustomToolTips.Highlight = true;
					myHighlightControl.CustomToolTips.HighlightColor = (myHighlightControl.Color ?? Color.Yellow);
				}
				m_highlightedControls.AddWeak(myHighlightControl.Control);
			}
			m_backgroundColor = (Vector4.One * 0.86f).ToSRGB();
			m_backgroundFadeColor = (Vector4.One * 0.86f).ToSRGB();
			base.CanBeHidden = false;
			base.CanHaveFocus = true;
			m_canShareInput = false;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			base.DrawMouseCursor = true;
			base.CloseButtonEnabled = false;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			UniversalInputHandling();
			foreach (MyGuiControlBase highlightedControl in m_highlightedControls)
			{
				highlightedControl.IsMouseOver = MyGuiControlBase.CheckMouseOver(highlightedControl.Size, highlightedControl.GetPositionAbsolute(), highlightedControl.OriginAlign);
				for (MyGuiControlBase myGuiControlBase = highlightedControl.Owner as MyGuiControlBase; myGuiControlBase != null; myGuiControlBase = (myGuiControlBase.Owner as MyGuiControlBase))
				{
					myGuiControlBase.IsMouseOver = MyGuiControlBase.CheckMouseOver(myGuiControlBase.Size, myGuiControlBase.GetPositionAbsolute(), myGuiControlBase.OriginAlign);
				}
				if (m_closeInFrames == uint.MaxValue && highlightedControl.IsMouseOver && MyInput.Static.IsNewLeftMousePressed())
				{
					m_closeInFrames = 10u;
				}
			}
			base.HandleInput(receivedFocusInThisUpdate);
			if (m_closeInFrames == 0)
			{
				CloseScreen();
			}
			else if (m_closeInFrames < uint.MaxValue)
			{
				m_closeInFrames--;
			}
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			UniversalInputHandling();
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		public override bool Draw()
		{
			MyHighlightControl[] highlightedControlsData = m_highlightedControlsData;
			for (int i = 0; i < highlightedControlsData.Length; i++)
			{
				MyHighlightControl myHighlightControl = highlightedControlsData[i];
				MyGuiControlGrid myGuiControlGrid = myHighlightControl.Control as MyGuiControlGrid;
				if (myGuiControlGrid == null)
				{
					continue;
				}
				if (myGuiControlGrid.ModalItems == null)
				{
					myGuiControlGrid.ModalItems = new Dictionary<int, Color>();
				}
				else
				{
					myGuiControlGrid.ModalItems.Clear();
				}
				if (myHighlightControl.Indices != null)
				{
					int[] indices = myHighlightControl.Indices;
					foreach (int key in indices)
					{
						myGuiControlGrid.ModalItems.Add(key, myHighlightControl.Color.HasValue ? myHighlightControl.Color.Value : Color.Yellow);
					}
				}
			}
			base.Draw();
			highlightedControlsData = m_highlightedControlsData;
			for (int i = 0; i < highlightedControlsData.Length; i++)
			{
				MyHighlightControl myHighlightControl2 = highlightedControlsData[i];
				MyGuiControlGrid myGuiControlGrid2 = myHighlightControl2.Control as MyGuiControlGrid;
				if (myGuiControlGrid2 != null && myGuiControlGrid2.ModalItems != null)
				{
					myGuiControlGrid2.ModalItems.Clear();
				}
				foreach (MyGuiControlBase element in myHighlightControl2.Control.Elements)
				{
					MyGuiControlGrid myGuiControlGrid3 = element as MyGuiControlGrid;
					if (myGuiControlGrid3 != null && myGuiControlGrid3.ModalItems != null)
					{
						myGuiControlGrid3.ModalItems.Clear();
					}
				}
			}
			highlightedControlsData = m_highlightedControlsData;
			for (int i = 0; i < highlightedControlsData.Length; i++)
			{
				MyHighlightControl myHighlightControl3 = highlightedControlsData[i];
				if (base.State == MyGuiScreenState.OPENED && myHighlightControl3.CustomToolTips != null)
				{
					Vector2 positionAbsoluteTopRight = myHighlightControl3.Control.GetPositionAbsoluteTopRight();
					positionAbsoluteTopRight.Y -= myHighlightControl3.CustomToolTips.Size.Y + 0.045f;
					positionAbsoluteTopRight.X -= 0.01f;
					myHighlightControl3.CustomToolTips.Draw(positionAbsoluteTopRight);
				}
				if (!(myHighlightControl3.Control is MyGuiControlGrid) && !(myHighlightControl3.Control is MyGuiControlGridDragAndDrop))
				{
					MyGuiControlBase control = myHighlightControl3.Control;
					Vector2 size = control.Size + HIGHLIGHT_TEXTURE_SIZE;
					Vector2 positionLeftTop = control.GetPositionAbsoluteTopLeft() - HIGHLIGHT_TEXTURE_OFFSET;
					Color colorMask = myHighlightControl3.Color.HasValue ? myHighlightControl3.Color.Value : Color.Yellow;
					colorMask.A = (byte)((float)(int)colorMask.A * m_transitionAlpha);
					MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.Draw(positionLeftTop, size, colorMask);
					control.Draw(m_transitionAlpha, m_backgroundTransition);
				}
			}
			return true;
		}

		private void UniversalInputHandling()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
			{
				CloseScreen();
			}
		}

		public override bool CloseScreen()
		{
			m_highlightedControls.ClearWeaks();
			return base.CloseScreen();
		}
	}
}
