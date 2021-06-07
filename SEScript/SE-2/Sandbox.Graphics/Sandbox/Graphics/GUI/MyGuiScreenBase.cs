using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyGuiScreenBase : IMyGuiControlsParent, IMyGuiControlsOwner, IVRageGuiScreen
	{
		public delegate void ScreenHandler(MyGuiScreenBase source);

		private const bool GUI_SHOW_FOCUS = false;

		protected Action OnEnterCallback;

		public Action OnLoadingAction;

		protected float m_transitionAlpha;

		protected float m_backgroundTransition;

		protected float m_guiTransition;

		private MyGuiControls m_controls;

		protected Vector2 m_position;

		protected Color m_backgroundFadeColor;

		private int m_transitionFrameIndex;

		public static bool EnableSlowTransitionAnimations;

		protected bool m_isTopMostScreen;

		protected bool m_isAlwaysFirst;

		protected bool m_isTopScreen;

		protected bool m_isFirstForUnload;

		protected Vector4? m_backgroundColor;

		protected string m_backgroundTexture;

		protected bool m_canCloseInCloseAllScreenCalls = true;

		protected Vector2? m_size;

		protected bool m_closeOnEsc = true;

		private bool m_drawMouseCursor = true;

		protected bool m_joystickAsMouse;

		protected bool m_defaultJoystickDpadUse = true;

		protected bool m_defaultJoystickCancelUse = true;

		protected int m_lastTransitionTime;

		private bool m_isLoaded;

		private object m_isLoadedLock = new object();

		private bool m_firstUpdateServed;

		protected bool m_drawEvenWithoutFocus;

		protected bool m_canShareInput;

		protected bool m_allowUnhidePreviousScreen;

		protected GuiSounds? m_openingCueEnum;

		protected GuiSounds? m_closingCueEnum;

		private MyGuiControlBase m_draggingControl;

		private Vector2 m_draggingControlOffset;

		private StringBuilder m_drawPositionSb = new StringBuilder();

		protected MyGuiControlGridDragAndDrop m_gridDragAndDropHandlingNow;

		protected MyGuiControlBase m_comboboxHandlingNow;

		protected MyGuiControlBase m_lastHandlingControl;

		private DateTime m_screenCreation = DateTime.UtcNow;

		private bool m_useAnalytics;

		private MyGuiControlButton m_closeButton;

		public readonly MyGuiControls Elements;

		public bool IsHitTestVisible = true;

		private MyGuiScreenState m_state;

		private bool m_enabledBackgroundFade;

		private bool m_canBeHidden = true;

		private bool m_canHideOthers = true;

		private bool m_canHaveFocus = true;

		private Vector2 m_closeButtonOffset;

		private bool m_closeButtonEnabled;

		private MyGuiControlButtonStyleEnum m_closeButtonStyle = MyGuiControlButtonStyleEnum.Close;

		private MyGuiControlBase m_focusedControl;

		public Color BackgroundFadeColor
		{
			get
			{
				Color backgroundFadeColor = m_backgroundFadeColor;
				backgroundFadeColor.A = (byte)((float)(int)backgroundFadeColor.A * m_transitionAlpha);
				return backgroundFadeColor;
			}
		}

		public MyGuiDrawAlignEnum Align
		{
			get;
			set;
		}

		public bool SkipTransition
		{
			get;
			set;
		}

		public bool Cancelled
		{
			get;
			private set;
		}

		protected bool DrawMouseCursor
		{
			get
			{
				return m_drawMouseCursor;
			}
			set
			{
				m_drawMouseCursor = value;
			}
		}

		public bool JoystickAsMouse
		{
			get
			{
				return m_joystickAsMouse;
			}
			set
			{
				m_joystickAsMouse = value;
			}
		}

		public MyGuiScreenState State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (m_state == value)
				{
					return;
				}
				bool visible = Visible;
				m_state = value;
				if (this.VisibleChanged != null && Visible != visible)
				{
					this.VisibleChanged(this, Visible);
				}
				if (MyVRage.Platform.ImeProcessor != null && MyScreenManager.GetScreenWithFocus() == this)
				{
					if (value == MyGuiScreenState.OPENED)
					{
						MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
					}
					if (value == MyGuiScreenState.CLOSED || value == MyGuiScreenState.HIDDEN)
					{
						MyVRage.Platform.ImeProcessor.UnregisterActiveScreen(this);
					}
				}
			}
		}

		public bool IsLoaded
		{
			get
			{
				lock (m_isLoadedLock)
				{
					return m_isLoaded;
				}
			}
			set
			{
				lock (m_isLoadedLock)
				{
					m_isLoaded = value;
				}
			}
		}

		public bool EnabledBackgroundFade
		{
			get
			{
				return m_enabledBackgroundFade;
			}
			protected set
			{
				m_enabledBackgroundFade = value;
			}
		}

		public bool CanBeHidden
		{
			get
			{
				return m_canBeHidden;
			}
			protected set
			{
				m_canBeHidden = value;
			}
		}

		public bool CanHideOthers
		{
			get
			{
				return m_canHideOthers;
			}
			protected set
			{
				m_canHideOthers = value;
			}
		}

		public bool CanHaveFocus
		{
			get
			{
				return m_canHaveFocus;
			}
			protected set
			{
				m_canHaveFocus = value;
			}
		}

		public virtual MyGuiControls Controls => m_controls;

		public Vector4? BackgroundColor
		{
			get
			{
				return m_backgroundColor;
			}
			set
			{
				m_backgroundColor = value;
			}
		}

		public Vector2? Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
			}
		}

		public List<MyGuiControlBase> VisibleElements => Controls.GetVisibleControls();

		public bool Visible => State != MyGuiScreenState.HIDDEN;

		public Vector2 CloseButtonOffset
		{
			get
			{
				return m_closeButtonOffset;
			}
			set
			{
				if (m_closeButtonOffset != value)
				{
					m_closeButtonOffset = value;
					if (m_closeButton != null)
					{
						m_closeButton.Position = CalcCloseButtonPosition();
					}
				}
			}
		}

		public bool CloseButtonEnabled
		{
			get
			{
				return m_closeButtonEnabled;
			}
			set
			{
				m_closeButtonEnabled = value;
				if (m_closeButton != null)
				{
					m_closeButton.Visible = value;
					m_closeButton.Position = CalcCloseButtonPosition();
				}
			}
		}

		public MyGuiControlButtonStyleEnum CloseButtonStyle
		{
			get
			{
				return m_closeButtonStyle;
			}
			set
			{
				m_closeButtonStyle = value;
				if (m_closeButton != null)
				{
					m_closeButton.VisualStyle = value;
				}
			}
		}

		public MyGuiControlBase FocusedControl
		{
			get
			{
				return m_focusedControl;
			}
			set
			{
				if ((value == null || value.CanHaveFocus) && m_focusedControl != value && (MyVRage.Platform.ImeProcessor == null || !MyVRage.Platform.ImeProcessor.IsComposing))
				{
					MyGuiControlBase focusedControl = m_focusedControl;
					m_focusedControl = value;
					focusedControl?.OnFocusChanged(focus: false);
					if (m_focusedControl != null)
					{
						m_focusedControl.OnFocusChanged(focus: true);
					}
				}
			}
		}

		public string DebugNamePath => GetFriendlyName();

		public string Name => GetFriendlyName();

		public IMyGuiControlsOwner Owner => null;

		IVRageGuiControl IVRageGuiScreen.FocusedControl
		{
			get
			{
				return FocusedControl;
			}
			set
			{
				FocusedControl = (value as MyGuiControlBase);
			}
		}

		public bool IsOpened
		{
			get
			{
				if (State != MyGuiScreenState.OPENED)
				{
					return State == MyGuiScreenState.OPENING;
				}
				return true;
			}
		}

		public static event Action<string, Vector2, uint> MouseClickEvent;

		public event ScreenHandler Closed;

		public event VisibleChangedDelegate VisibleChanged;

		public event Action<MyGuiScreenBase> DataLoading;

		public event Action<MyGuiScreenBase> DataUnloading;

		private MyGuiScreenBase()
		{
		}

		protected MyGuiScreenBase(Vector2? position = null, Vector4? backgroundColor = null, Vector2? size = null, bool isTopMostScreen = false, string backgroundTexture = null, float backgroundTransition = 0f, float guiTransition = 0f, int? gamepadSlot = null)
		{
			m_controls = new MyGuiControls(this);
			m_backgroundFadeColor = Color.White;
			m_backgroundColor = backgroundColor;
			m_size = size;
			m_isTopMostScreen = isTopMostScreen;
			m_allowUnhidePreviousScreen = true;
			State = MyGuiScreenState.OPENING;
			m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
			m_position = (position ?? new Vector2(0.5f, 0.5f));
			m_useAnalytics = RegisterClicks();
			m_backgroundTexture = backgroundTexture;
			Elements = new MyGuiControls(this);
			m_backgroundTransition = backgroundTransition;
			m_guiTransition = guiTransition;
			SetDefaultCloseButtonOffset();
			CreateCloseButton();
			Align = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
		}

		public MyObjectBuilder_GuiScreen GetObjectBuilder()
		{
			MyObjectBuilder_GuiScreen myObjectBuilder_GuiScreen = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GuiScreen>();
			myObjectBuilder_GuiScreen.Controls = Controls.GetObjectBuilder();
			myObjectBuilder_GuiScreen.BackgroundColor = m_backgroundColor;
			myObjectBuilder_GuiScreen.BackgroundTexture = m_backgroundTexture;
			myObjectBuilder_GuiScreen.Size = m_size;
			myObjectBuilder_GuiScreen.CloseButtonEnabled = CloseButtonEnabled;
			myObjectBuilder_GuiScreen.CloseButtonOffset = CloseButtonOffset;
			return myObjectBuilder_GuiScreen;
		}

		public void Init(MyObjectBuilder_GuiScreen objectBuilder)
		{
			m_backgroundColor = objectBuilder.BackgroundColor;
			m_backgroundTexture = objectBuilder.BackgroundTexture;
			m_size = objectBuilder.Size;
			Controls.Init(objectBuilder.Controls);
			CloseButtonOffset = objectBuilder.CloseButtonOffset;
			CloseButtonEnabled = objectBuilder.CloseButtonEnabled;
		}

		public virtual void LoadContent()
		{
			IsLoaded = true;
			m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
		}

		public virtual void LoadData()
		{
			if (this.DataLoading != null)
			{
				this.DataLoading(this);
			}
		}

		public virtual void UnloadContent()
		{
			MyLog.Default.WriteLine("MyGuiScreenBase.UnloadContent - START");
			MyLog.Default.IncreaseIndent();
			IsLoaded = false;
			MyLog.Default.DecreaseIndent();
			MyLog.Default.WriteLine("MyGuiScreenBase.UnloadContent - END");
		}

		public virtual void UnloadData()
		{
			if (this.DataUnloading != null)
			{
				this.DataUnloading(this);
			}
		}

		public virtual void RunLoadingAction()
		{
			if (OnLoadingAction != null)
			{
				OnLoadingAction();
			}
		}

		public bool IsMouseOverAnyControl()
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			for (int num = visibleControls.Count - 1; num >= 0; num--)
			{
				if (visibleControls[num].IsHitTestVisible && visibleControls[num].IsMouseOverAnyControl())
				{
					return true;
				}
			}
			return false;
		}

		public MyGuiControlBase GetMouseOverControl()
		{
			for (int num = Controls.GetVisibleControls().Count - 1; num >= 0; num--)
			{
				MyGuiControlBase mouseOverControl = Controls.GetVisibleControls()[num].GetMouseOverControl();
				if (mouseOverControl != null)
				{
					return mouseOverControl;
				}
			}
			return null;
		}

		public virtual void GetControlsUnderMouseCursor(Vector2 position, List<MyGuiControlBase> controls, bool visibleOnly)
		{
			GetControlsUnderMouseCursor(this, position, controls, visibleOnly);
		}

		private static void GetControlsUnderMouseCursor(IMyGuiControlsParent parent, Vector2 position, List<MyGuiControlBase> controls, bool visibleOnly)
		{
			if (visibleOnly)
			{
				foreach (MyGuiControlBase visibleControl in parent.Controls.GetVisibleControls())
				{
					if (IsControlUnderCursor(position, visibleControl))
					{
						controls.Add(visibleControl);
						visibleControl.GetElementsUnderCursor(position, visibleOnly, controls);
						IMyGuiControlsParent myGuiControlsParent = visibleControl as IMyGuiControlsParent;
						if (myGuiControlsParent != null)
						{
							GetControlsUnderMouseCursor(myGuiControlsParent, position, controls, visibleOnly);
						}
					}
				}
			}
			else
			{
				foreach (MyGuiControlBase control in parent.Controls)
				{
					if (IsControlUnderCursor(position, control))
					{
						controls.Add(control);
						control.GetElementsUnderCursor(position, visibleOnly, controls);
						IMyGuiControlsParent myGuiControlsParent2 = control as IMyGuiControlsParent;
						if (myGuiControlsParent2 != null)
						{
							GetControlsUnderMouseCursor(myGuiControlsParent2, position, controls, visibleOnly);
						}
					}
				}
			}
		}

		private MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			for (int i = 0; i < Controls.GetVisibleControls().Count; i++)
			{
				MyGuiControlGridDragAndDrop dragAndDropHandlingNow = Controls.GetVisibleControls()[i].GetDragAndDropHandlingNow();
				if (dragAndDropHandlingNow != null)
				{
					return dragAndDropHandlingNow;
				}
			}
			return null;
		}

		private MyGuiControlBase GetExclusiveInputHandler(List<MyGuiControlBase> controls)
		{
			foreach (MyGuiControlBase control in controls)
			{
				MyGuiControlBase exclusiveInputHandler = control.GetExclusiveInputHandler();
				if (exclusiveInputHandler != null)
				{
					return exclusiveInputHandler;
				}
			}
			return null;
		}

		private bool HandleControlsInput(bool receivedFocusInThisUpdate)
		{
			MyGuiControlBase myGuiControlBase = null;
			if (m_lastHandlingControl != null && m_lastHandlingControl.Visible && m_lastHandlingControl.HandleInput() != null)
			{
				myGuiControlBase = m_lastHandlingControl;
			}
			if (myGuiControlBase == null && m_gridDragAndDropHandlingNow != null && m_gridDragAndDropHandlingNow.HandleInput() != null)
			{
				myGuiControlBase = m_gridDragAndDropHandlingNow;
			}
			if (myGuiControlBase == null && m_comboboxHandlingNow != null && m_comboboxHandlingNow.HandleInput() != null)
			{
				myGuiControlBase = m_comboboxHandlingNow;
			}
			MyGuiControlBase myGuiControlBase2 = null;
			if (myGuiControlBase == null)
			{
				List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
				for (int i = 0; i < visibleControls.Count; i++)
				{
					MyGuiControlBase myGuiControlBase3 = visibleControls[i];
					if (myGuiControlBase3 != m_comboboxHandlingNow && myGuiControlBase3 != m_gridDragAndDropHandlingNow && myGuiControlBase3.CheckMouseOver())
					{
						myGuiControlBase2 = myGuiControlBase3;
						myGuiControlBase = myGuiControlBase3.HandleInput();
						break;
					}
				}
			}
			if (myGuiControlBase == null)
			{
				List<MyGuiControlBase> visibleControls2 = Controls.GetVisibleControls();
				for (int num = visibleControls2.Count - 1; num >= 0; num--)
				{
					if (num < visibleControls2.Count)
					{
						MyGuiControlBase myGuiControlBase4 = visibleControls2[num];
						if (myGuiControlBase4 != m_comboboxHandlingNow && myGuiControlBase4 != m_gridDragAndDropHandlingNow && myGuiControlBase4 != myGuiControlBase2)
						{
							myGuiControlBase = myGuiControlBase4.HandleInput();
							if (myGuiControlBase != null)
							{
								break;
							}
						}
					}
				}
			}
			if (myGuiControlBase == null)
			{
				foreach (MyGuiControlBase element in Elements)
				{
					if (element.Visible && element.CanHaveFocus)
					{
						myGuiControlBase = element.HandleInput();
						if (myGuiControlBase != null)
						{
							break;
						}
					}
				}
			}
			if (myGuiControlBase != null && myGuiControlBase.Owner != null && myGuiControlBase.Visible)
			{
				FocusedControl = myGuiControlBase;
			}
			m_lastHandlingControl = myGuiControlBase;
			return myGuiControlBase != null;
		}

		protected MyGuiControlBase GetFirstFocusableControl()
		{
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				if (CanHaveFocusRightNow(visibleControl))
				{
					return visibleControl;
				}
			}
			foreach (MyGuiControlBase element in Elements)
			{
				if (CanHaveFocusRightNow(element))
				{
					return element;
				}
			}
			return null;
		}

		internal static bool CanHaveFocusRightNow(MyGuiControlBase control)
		{
			if (control.Enabled && control.Visible)
			{
				return control.CanHaveFocus;
			}
			return false;
		}

		internal static MyGuiControlBase GetNextFocusControl(ref RectangleF fRect, MyGuiControlBase currentFocusControl, MyGuiControlBase skipControl, List<MyGuiControlBase> visibleControls, List<MyGuiControlBase> visibleElements, MyDirection direction, bool page)
		{
			int num = visibleControls.Count + (visibleElements?.Count ?? 0);
			List<MyGuiControlBase> list = new List<MyGuiControlBase>();
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				MyGuiControlBase myGuiControlBase = (i < visibleControls.Count) ? visibleControls[i] : visibleElements[i - visibleControls.Count];
				if (myGuiControlBase == skipControl || !CanHaveFocusRightNow(myGuiControlBase))
				{
					continue;
				}
				RectangleF next = myGuiControlBase.Rectangle;
				float num3 = direction.ControlDistance(ref fRect, ref next);
				if (!(num3 >= 0f) || !(num3 < num2 + 0.001f))
				{
					continue;
				}
				if (myGuiControlBase.CanFocusChildren && currentFocusControl != null)
				{
					myGuiControlBase = myGuiControlBase.GetNextFocusControl(currentFocusControl, direction, page);
				}
				if (myGuiControlBase != null)
				{
					if (num3 < num2 - 0.001f)
					{
						list.Clear();
					}
					num2 = num3;
					list.Add(myGuiControlBase);
				}
			}
			bool flag = direction == MyDirection.Up || direction == MyDirection.Down;
			float num4 = float.MaxValue;
			MyGuiControlBase result = null;
			foreach (MyGuiControlBase item in list)
			{
				if (flag)
				{
					if (item.Rectangle.X < num4)
					{
						num4 = item.Rectangle.X;
						result = item;
					}
				}
				else if (item.Rectangle.Y < num4)
				{
					num4 = item.Rectangle.Y;
					result = item;
				}
			}
			return result;
		}

		internal static MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyGuiControlBase skipControl, MyGuiControlBase thisControl, List<MyGuiControlBase> visibleControls, List<MyGuiControlBase> visibleElements, MyDirection direction, bool page)
		{
			RectangleF fRect = currentFocusControl.FocusRectangle;
			return GetNextFocusControl(ref fRect, currentFocusControl, skipControl, visibleControls, visibleElements, direction, page);
		}

		public void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
		}

		public MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			List<MyGuiControlBase> visibleControls2 = Elements.GetVisibleControls();
			return GetNextFocusControl(currentFocusControl, m_closeButton, null, visibleControls, visibleControls2, direction, page);
		}

		protected virtual bool HandleKeyboardActiveIndex(MyDirection direction, bool page, bool loop)
		{
			MyGuiControlBase myGuiControlBase = FocusedControl;
			if (FocusedControl == null)
			{
				myGuiControlBase = GetFirstFocusableControl();
				if (myGuiControlBase == null)
				{
					return false;
				}
			}
			MyGuiControlBase focusControl = myGuiControlBase.GetFocusControl(direction, page, loop);
			if (focusControl != null)
			{
				FocusedControl = focusControl;
			}
			return true;
		}

		public virtual void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (!IsLoaded || State != MyGuiScreenState.OPENED)
			{
				return;
			}
			_ = m_firstUpdateServed;
			if (!m_firstUpdateServed && FocusedControl == null)
			{
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
				}
				FocusedControl = GetFirstFocusableControl();
				m_firstUpdateServed = true;
			}
			if (HandleControlsInput(receivedFocusInThisUpdate))
			{
				return;
			}
			bool flag = false;
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_UP, MyControlStateType.NEW_PRESSED_REPEATING) && m_defaultJoystickDpadUse))
			{
				flag = HandleKeyboardActiveIndex(MyDirection.Up, page: false, loop: false);
			}
			else if (MyInput.Static.IsNewKeyPressed(MyKeys.Down) || (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_DOWN, MyControlStateType.NEW_PRESSED_REPEATING) && m_defaultJoystickDpadUse))
			{
				flag = HandleKeyboardActiveIndex(MyDirection.Down, page: false, loop: false);
			}
			else if ((MyInput.Static.IsNewKeyPressed(MyKeys.Left) || (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT, MyControlStateType.NEW_PRESSED_REPEATING) && m_defaultJoystickDpadUse)) && !(FocusedControl is MyGuiControlSlider))
			{
				flag = HandleKeyboardActiveIndex(MyDirection.Left, page: false, loop: false);
			}
			else if ((MyInput.Static.IsNewKeyPressed(MyKeys.Right) || (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT, MyControlStateType.NEW_PRESSED_REPEATING) && m_defaultJoystickDpadUse)) && !(FocusedControl is MyGuiControlSlider))
			{
				flag = HandleKeyboardActiveIndex(MyDirection.Right, page: false, loop: false);
			}
			else if (m_closeOnEsc && (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MAIN_MENU) || (m_defaultJoystickCancelUse && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))))
			{
				if (MyVRage.Platform.ImeProcessor == null || !MyVRage.Platform.ImeProcessor.IsComposing)
				{
					Canceling();
				}
			}
			else
			{
				bool flag2 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_UP, MyControlStateType.NEW_PRESSED_REPEATING);
				bool flag3 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_DOWN, MyControlStateType.NEW_PRESSED_REPEATING);
				if (flag2 || (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Tab)))
				{
					flag = HandleKeyboardActiveIndex(MyDirection.Up, page: true, !flag2);
				}
				else if (flag3 || MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
				{
					flag = HandleKeyboardActiveIndex(MyDirection.Down, page: true, !flag3);
				}
				else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_LEFT, MyControlStateType.NEW_PRESSED_REPEATING))
				{
					flag = HandleKeyboardActiveIndex(MyDirection.Left, page: true, loop: false);
				}
				else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_RIGHT, MyControlStateType.NEW_PRESSED_REPEATING))
				{
					flag = HandleKeyboardActiveIndex(MyDirection.Right, page: true, loop: false);
				}
			}
			if (!flag)
			{
				HandleUnhandledInput(receivedFocusInThisUpdate);
			}
		}

		public virtual void InputLost()
		{
		}

		private static bool IsControlUnderCursor(Vector2 mousePosition, MyGuiControlBase control)
		{
			if (!control.IsHitTestVisible)
			{
				return false;
			}
			Vector2? size = control.GetSize();
			if (size.HasValue)
			{
				Vector2 coordCenterFromAligned = MyUtils.GetCoordCenterFromAligned(control.GetPositionAbsolute(), size.Value, control.OriginAlign);
				Vector2 vector = coordCenterFromAligned - size.Value / 2f;
				Vector2 vector2 = coordCenterFromAligned + size.Value / 2f;
				if (mousePosition.X >= vector.X && mousePosition.X <= vector2.X && mousePosition.Y >= vector.Y)
				{
					return mousePosition.Y <= vector2.Y;
				}
				return false;
			}
			return false;
		}

		protected bool IsMouseOver()
		{
			if (!IsHitTestVisible)
			{
				return false;
			}
			Vector2 value = (Align == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP) ? m_position : (m_position - m_size.Value / 2f);
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			Vector2 vector = value + zero;
			Vector2 vector2 = value + m_size.Value - zero2;
			if (MyGuiManager.MouseCursorPosition.X >= vector.X && MyGuiManager.MouseCursorPosition.X <= vector2.X && MyGuiManager.MouseCursorPosition.Y >= vector.Y)
			{
				return MyGuiManager.MouseCursorPosition.Y <= vector2.Y;
			}
			return false;
		}

		public virtual void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			if (OnEnterCallback != null && MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
			{
				OnEnterCallback();
			}
		}

		public virtual bool HandleInputAfterSimulation()
		{
			return false;
		}

		protected MyGuiControlLabel AddCaption(MyStringId textEnum, Vector4? captionTextColor = null, Vector2? captionOffset = null, float captionScale = 0.8f)
		{
			return AddCaption(MyTexts.GetString(textEnum), captionTextColor, captionOffset, captionScale);
		}

		protected MyGuiControlLabel AddCaption(string text, Vector4? captionTextColor = null, Vector2? captionOffset = null, float captionScale = 0.8f)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f, (0f - m_size.Value.Y) / 2f + MyGuiConstants.SCREEN_CAPTION_DELTA_Y) + (captionOffset.HasValue ? captionOffset.Value : Vector2.Zero), null, text, captionTextColor ?? Vector4.One, captionScale, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel.Name = "CaptionLabel";
			myGuiControlLabel.Font = "ScreenCaption";
			Elements.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected virtual void Canceling()
		{
			Cancelled = true;
			if (m_closingCueEnum.HasValue)
			{
				MyGuiSoundManager.PlaySound(m_closingCueEnum.Value);
			}
			else
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			CloseScreen();
		}

		public virtual bool CloseScreen()
		{
			if (State == MyGuiScreenState.CLOSING || State == MyGuiScreenState.CLOSED)
			{
				return false;
			}
			State = MyGuiScreenState.CLOSING;
			m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
			return true;
		}

		public virtual bool HideScreen()
		{
			if (State == MyGuiScreenState.HIDING || State == MyGuiScreenState.HIDDEN)
			{
				return false;
			}
			if (State != MyGuiScreenState.UNHIDING)
			{
				m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
				State = MyGuiScreenState.HIDING;
			}
			else
			{
				State = MyGuiScreenState.HIDDEN;
			}
			return true;
		}

		public virtual bool UnhideScreen()
		{
			if (State == MyGuiScreenState.UNHIDING || State == MyGuiScreenState.OPENED)
			{
				return false;
			}
			State = MyGuiScreenState.UNHIDING;
			m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
			return true;
		}

		public virtual void CloseScreenNow()
		{
			if (State != MyGuiScreenState.CLOSED)
			{
				State = MyGuiScreenState.CLOSED;
				OnClosed();
				if (this.Closed != null)
				{
					this.Closed(this);
					this.Closed = null;
				}
			}
		}

		private void UpdateControls()
		{
			List<MyGuiControlBase> list = Controls.GetVisibleControls().ToList();
			foreach (MyGuiControlBase item in list)
			{
				item.Update();
			}
			foreach (MyGuiControlBase element in Elements)
			{
				element.Update();
			}
			m_comboboxHandlingNow = GetExclusiveInputHandler(list);
			m_gridDragAndDropHandlingNow = GetDragAndDropHandlingNow();
		}

		public virtual bool Update(bool hasFocus)
		{
			if (FocusedControl != null)
			{
				if (!FocusedControl.Visible)
				{
					FocusedControl = null;
				}
				else
				{
					IMyGuiControlsOwner owner = FocusedControl.Owner;
					if (owner == null)
					{
						FocusedControl = null;
					}
					while (owner != null)
					{
						if (!owner.Visible)
						{
							FocusedControl = null;
							break;
						}
						owner = owner.Owner;
					}
				}
			}
			if (m_useAnalytics && hasFocus && MyInput.Static.IsNewLeftMousePressed() && MyGuiScreenBase.MouseClickEvent != null)
			{
				MyGuiScreenBase.MouseClickEvent(Name, MyGuiManager.MouseCursorPosition, (uint)(DateTime.UtcNow - m_screenCreation).TotalSeconds);
			}
			if (UpdateTransition())
			{
				UpdateControls();
				return true;
			}
			return false;
		}

		private bool UpdateTransition()
		{
			if (m_lastTransitionTime == 0)
			{
				m_lastTransitionTime = MyGuiManager.TotalTimeInMilliseconds;
			}
			if (State == MyGuiScreenState.OPENING || State == MyGuiScreenState.UNHIDING)
			{
				int transitionOpeningTime = GetTransitionOpeningTime();
				int num = MyGuiManager.TotalTimeInMilliseconds - m_lastTransitionTime;
				if (EnableSlowTransitionAnimations)
				{
					if (num == 0)
					{
						m_transitionFrameIndex = 0;
					}
					num = m_transitionFrameIndex++ * 2;
				}
				if (State == MyGuiScreenState.OPENING && m_openingCueEnum.HasValue)
				{
					MyGuiSoundManager.PlaySound(m_openingCueEnum.Value);
				}
				if (num >= transitionOpeningTime)
				{
					State = MyGuiScreenState.OPENED;
					m_transitionAlpha = 1f;
					OnShow();
				}
				else
				{
					float num2 = MathHelper.Clamp((float)num / (float)transitionOpeningTime, 0f, 1f);
					num2 *= num2;
					m_transitionAlpha = MathHelper.Lerp(0f, 1f, num2);
				}
			}
			else if (State == MyGuiScreenState.CLOSING || State == MyGuiScreenState.HIDING)
			{
				int num3 = MyGuiManager.TotalTimeInMilliseconds - m_lastTransitionTime;
				if (num3 >= GetTransitionClosingTime())
				{
					m_transitionAlpha = 0f;
					if (State == MyGuiScreenState.CLOSING)
					{
						CloseScreenNow();
						return false;
					}
					if (State == MyGuiScreenState.HIDING)
					{
						State = MyGuiScreenState.HIDDEN;
						OnHide();
					}
				}
				else
				{
					m_transitionAlpha = MathHelper.Lerp(1f, 0f, MathHelper.Clamp((float)num3 / (float)GetTransitionClosingTime(), 0f, 1f));
				}
			}
			return true;
		}

		public virtual bool Draw()
		{
			if (m_backgroundColor.HasValue && m_size.HasValue)
			{
				if (m_backgroundTexture == null && m_size.HasValue)
				{
					m_backgroundTexture = MyGuiManager.GetBackgroundTextureFilenameByAspectRatio(m_size.Value);
				}
				MyGuiManager.DrawSpriteBatch(m_backgroundTexture, m_position, m_size.Value, ApplyTransitionAlpha(m_backgroundColor.Value, (m_guiTransition != 0f) ? (m_backgroundTransition * m_transitionAlpha) : m_transitionAlpha), Align);
			}
			if (m_guiTransition != 0f)
			{
				float transitionAlpha = m_guiTransition * m_transitionAlpha;
				float backgroundTransitionAlpha = m_backgroundTransition * m_transitionAlpha;
				DrawElements(transitionAlpha, backgroundTransitionAlpha);
				DrawControls(transitionAlpha, backgroundTransitionAlpha);
			}
			else
			{
				DrawElements(m_transitionAlpha, m_transitionAlpha);
				DrawControls(m_transitionAlpha, m_transitionAlpha);
			}
			return true;
		}

		private void DrawElements(float transitionAlpha, float backgroundTransitionAlpha)
		{
			foreach (MyGuiControlBase element in Elements)
			{
				if (element.Visible)
				{
					element.Draw(transitionAlpha * element.Alpha, backgroundTransitionAlpha * element.Alpha);
				}
			}
		}

		private void DrawControls(float transitionAlpha, float backgroundTransitionAlpha)
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			for (int i = 0; i < visibleControls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = visibleControls[i];
				if (myGuiControlBase != m_comboboxHandlingNow && myGuiControlBase != m_gridDragAndDropHandlingNow && !(myGuiControlBase is MyGuiControlGridDragAndDrop))
				{
					myGuiControlBase.Draw(transitionAlpha * myGuiControlBase.Alpha, backgroundTransitionAlpha * myGuiControlBase.Alpha);
				}
			}
			if (m_comboboxHandlingNow != null)
			{
				m_comboboxHandlingNow.Draw(transitionAlpha * m_comboboxHandlingNow.Alpha, backgroundTransitionAlpha * m_comboboxHandlingNow.Alpha);
			}
			if (m_gridDragAndDropHandlingNow != null)
			{
				m_gridDragAndDropHandlingNow.Draw(transitionAlpha * m_gridDragAndDropHandlingNow.Alpha, backgroundTransitionAlpha * m_gridDragAndDropHandlingNow.Alpha);
			}
		}

		public void HideTooltips()
		{
			foreach (MyGuiControlBase control in Controls)
			{
				control.HideToolTip();
			}
		}

		public Vector2 GetPositionAbsolute()
		{
			return m_position;
		}

		public Vector2 GetPositionAbsoluteCenter()
		{
			return GetPositionAbsolute();
		}

		public Vector2 GetPositionAbsoluteTopLeft()
		{
			if (Size.HasValue)
			{
				return GetPositionAbsolute() - Size.Value * 0.5f;
			}
			return GetPositionAbsolute();
		}

		public bool GetDrawMouseCursor()
		{
			return m_drawMouseCursor;
		}

		public bool IsTopMostScreen()
		{
			return m_isTopMostScreen;
		}

		public bool IsAlwaysFirst()
		{
			return m_isAlwaysFirst;
		}

		public bool IsTopScreen()
		{
			return m_isTopScreen;
		}

		public bool IsFirstForUnload()
		{
			return m_isFirstForUnload;
		}

		public bool GetDrawScreenEvenWithoutFocus()
		{
			return m_drawEvenWithoutFocus;
		}

		public Vector2 GetPosition()
		{
			return m_position;
		}

		protected Color ApplyTransitionAlpha(Vector4 color, float transition)
		{
			Vector4 vector = color;
			vector.W *= transition;
			return new Color(vector);
		}

		public Vector2? GetSize()
		{
			return m_size;
		}

		public bool CanShareInput()
		{
			return m_canShareInput;
		}

		public bool CanCloseInCloseAllScreenCalls()
		{
			return m_canCloseInCloseAllScreenCalls;
		}

		public abstract string GetFriendlyName();

		public virtual void RecreateControls(bool constructor)
		{
			Controls.Clear();
			Elements.Clear();
			Elements.Add(m_closeButton);
			FocusedControl = null;
			m_firstUpdateServed = false;
			m_screenCreation = DateTime.UtcNow;
		}

		public virtual int GetTransitionOpeningTime()
		{
			if (SkipTransition)
			{
				return 0;
			}
			return 200;
		}

		public virtual int GetTransitionClosingTime()
		{
			if (SkipTransition)
			{
				return 0;
			}
			return 200;
		}

		public virtual bool RegisterClicks()
		{
			return false;
		}

		protected virtual void OnShow()
		{
		}

		protected virtual void OnHide()
		{
		}

		public virtual void OnRemoved()
		{
		}

		protected virtual void OnClosed()
		{
			Controls.Clear();
			foreach (MyGuiControlBase element in Elements)
			{
				element.OnRemoving();
			}
			Elements.Clear();
		}

		protected static string MakeScreenFilepath(string name)
		{
			return Path.Combine("Data", "Screens", name + ".gsc");
		}

		private void closeButton_OnButtonClick(MyGuiControlButton sender)
		{
			Canceling();
		}

		private void CreateCloseButton()
		{
			m_closeButton = new MyGuiControlButton
			{
				Name = "CloseButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				VisualStyle = m_closeButtonStyle,
				TextScale = 0f,
				Position = CalcCloseButtonPosition(),
				Visible = CloseButtonEnabled
			};
			m_closeButton.ButtonClicked += closeButton_OnButtonClick;
			Elements.Add(m_closeButton);
		}

		private Vector2 CalcCloseButtonPosition()
		{
			if (Align == MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				return (Size ?? Vector2.One) * new Vector2(1f, 0f) + CloseButtonOffset;
			}
			return (Size ?? Vector2.One) * new Vector2(0.5f, -0.5f) + CloseButtonOffset;
		}

		protected void SetDefaultCloseButtonOffset()
		{
			CloseButtonOffset = new Vector2(-0.013f, 0.015f);
		}

		protected void SetCloseButtonOffset_5_to_4()
		{
			CloseButtonOffset = new Vector2(-0.05f, 0.015f);
		}

		public void AddControl(IVRageGuiControl control)
		{
			Controls.Add(control as MyGuiControlBase);
		}

		public bool RemoveControl(IVRageGuiControl control)
		{
			return Controls.Remove(control as MyGuiControlBase);
		}

		public bool ContainsControl(IVRageGuiControl control)
		{
			return Controls.Contains(control as MyGuiControlBase);
		}
	}
}
