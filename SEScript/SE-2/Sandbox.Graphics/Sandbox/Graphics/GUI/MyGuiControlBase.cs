using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyGuiControlBase : IMyGuiControlsOwner, IVRageGuiControl
	{
		public class Friend
		{
			protected static void SetOwner(MyGuiControlBase control, IMyGuiControlsOwner owner)
			{
				control.Owner = owner;
			}
		}

		public struct NameChangedArgs
		{
			public string OldName;
		}

		private float m_alpha = 1f;

		private const bool DEBUG_CONTROL_FOCUS = false;

		public static bool DEBUG_CONTROL_BORDERS;

		private bool m_isMouseOver;

		private bool m_isMouseOverInPrevious;

		private bool m_canPlaySoundOnMouseOver = true;

		private bool m_canHaveFocus;

		private Vector2 m_minSize = Vector2.Zero;

		private Vector2 m_maxSize = Vector2.PositiveInfinity;

		private string m_name;

		protected bool m_mouseButtonPressed;

		private int m_showToolTipDelay;

		protected bool m_showToolTip;

		private bool m_showToolTipByFocus;

		protected internal MyToolTips m_toolTip;

		protected Vector2 m_toolTipPosition;

		public bool m_canFocusChildren;

		public readonly MyGuiControls Elements;

		private Thickness m_margin;

		private Vector2 m_position;

		private Vector2 m_size;

		private Vector4 m_colorMask;

		public MyGuiCompositeTexture BackgroundTexture;

		public Vector4 BorderColor;

		public bool BorderEnabled;

		public bool BorderHighlightEnabled;

		public bool DrawWhilePaused;

		public bool SkipForMouseTest;

		private bool m_enabled;

		public bool ShowTooltipWhenDisabled;

		public bool IsHitTestVisible = true;

		public bool IsActiveControl;

		private MyGuiDrawAlignEnum m_originAlign;

		private bool m_visible;

		public MyGuiControlHighlightType HighlightType;

		private bool m_hasHighlight;

		public Vector2 BorderMargin;

		public List<MyGuiControlBase> VisibleElements => Elements.GetVisibleControls();

		public float Alpha
		{
			get
			{
				return m_alpha;
			}
			set
			{
				m_alpha = value;
			}
		}

		public bool CanFocusChildren
		{
			get
			{
				return m_canFocusChildren;
			}
			set
			{
				m_canFocusChildren = value;
				m_canHaveFocus |= value;
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (m_name != value)
				{
					string name = m_name;
					m_name = value;
					if (this.NameChanged != null)
					{
						this.NameChanged(this, new NameChangedArgs
						{
							OldName = name
						});
					}
				}
			}
		}

		public IMyGuiControlsOwner Owner
		{
			get;
			private set;
		}

		public MyToolTips Tooltips => m_toolTip;

		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				if (m_position != value)
				{
					m_position = value;
					OnPositionChanged();
				}
			}
		}

		public Thickness Margin
		{
			get
			{
				return m_margin;
			}
			set
			{
				m_margin = value;
			}
		}

		public float PositionY
		{
			get
			{
				return m_position.Y;
			}
			set
			{
				if (m_position.Y != value)
				{
					m_position.Y = value;
					OnPositionChanged();
				}
			}
		}

		public float PositionX
		{
			get
			{
				return m_position.X;
			}
			set
			{
				if (m_position.X != value)
				{
					m_position.X = value;
					OnPositionChanged();
				}
			}
		}

		public Vector2 Size
		{
			get
			{
				return m_size;
			}
			set
			{
				value = Vector2.Clamp(value, MinSize, MaxSize);
				if (m_size != value)
				{
					m_size = value;
					OnSizeChanged();
				}
			}
		}

		public RectangleF Rectangle => new RectangleF(GetPositionAbsoluteTopLeft(), m_size);

		public virtual RectangleF FocusRectangle => Rectangle;

		public Vector2 MinSize
		{
			get
			{
				return m_minSize;
			}
			protected set
			{
				if (m_minSize != value)
				{
					m_minSize = value;
					Size = m_size;
				}
			}
		}

		public Vector2 MaxSize
		{
			get
			{
				return m_maxSize;
			}
			protected set
			{
				if (m_maxSize != value)
				{
					m_maxSize = value;
					Size = m_size;
				}
			}
		}

		public Vector4 ColorMask
		{
			get
			{
				return m_colorMask;
			}
			set
			{
				if (m_colorMask != value)
				{
					m_colorMask = value;
					OnColorMaskChanged();
				}
			}
		}

		public int BorderSize
		{
			get;
			set;
		}

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
					OnEnabledChanged();
				}
			}
		}

		public MyGuiDrawAlignEnum OriginAlign
		{
			get
			{
				return m_originAlign;
			}
			set
			{
				if (m_originAlign != value)
				{
					m_originAlign = value;
					OnOriginAlignChanged();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				if (m_visible != value)
				{
					m_visible = value;
					OnVisibleChanged();
				}
			}
		}

		public bool HasHighlight
		{
			get
			{
				return m_hasHighlight;
			}
			set
			{
				if (m_hasHighlight != value)
				{
					m_hasHighlight = value;
					OnHasHighlightChanged();
					if (this.HightlightChanged != null)
					{
						this.HightlightChanged(this);
					}
				}
			}
		}

		public bool HasFocus => MyScreenManager.FocusedControl == this;

		public bool IsMouseOver
		{
			get
			{
				return m_isMouseOver;
			}
			set
			{
				m_isMouseOver = value;
			}
		}

		public bool CanHaveFocus
		{
			get
			{
				return m_canHaveFocus;
			}
			set
			{
				m_canHaveFocus = value;
			}
		}

		public bool CanPlaySoundOnMouseOver
		{
			get
			{
				return m_canPlaySoundOnMouseOver;
			}
			set
			{
				m_canPlaySoundOnMouseOver = value;
			}
		}

		public object UserData
		{
			get;
			set;
		}

		public string DebugNamePath => Path.Combine((Owner != null) ? Owner.DebugNamePath : "null", Name);

		public event Action<MyGuiControlBase, NameChangedArgs> NameChanged;

		public event Action<MyGuiControlBase> SizeChanged;

		public event VisibleChangedDelegate VisibleChanged;

		public event Action<MyGuiControlBase> HightlightChanged;

		public event Action<MyGuiControlBase, bool> FocusChanged;

		protected MyGuiControlBase(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, string toolTip = null, MyGuiCompositeTexture backgroundTexture = null, bool isActiveControl = true, bool canHaveFocus = false, MyGuiControlHighlightType highlightType = MyGuiControlHighlightType.WHEN_ACTIVE, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
		{
			m_canPlaySoundOnMouseOver = true;
			Name = GetType().Name;
			Visible = true;
			m_enabled = true;
			m_position = (position ?? Vector2.Zero);
			m_canHaveFocus = canHaveFocus;
			m_size = (size ?? Vector2.One);
			m_colorMask = (colorMask ?? Vector4.One);
			BackgroundTexture = backgroundTexture;
			IsActiveControl = isActiveControl;
			HighlightType = highlightType;
			m_originAlign = originAlign;
			BorderSize = 1;
			BorderColor = new Vector4(1f, 1f, 1f, 0.5f);
			BorderEnabled = false;
			BorderHighlightEnabled = false;
			DrawWhilePaused = true;
			Elements = new MyGuiControls(this);
			if (toolTip != null)
			{
				m_toolTip = new MyToolTips(toolTip);
			}
		}

		public virtual void Init(MyObjectBuilder_GuiControlBase builder)
		{
			m_position = builder.Position;
			Size = builder.Size;
			Name = builder.Name;
			if (builder.BackgroundColor != Vector4.One)
			{
				ColorMask = builder.BackgroundColor;
			}
			if (builder.ControlTexture != null)
			{
				BackgroundTexture = new MyGuiCompositeTexture
				{
					Center = new MyGuiSizedTexture
					{
						Texture = builder.ControlTexture
					}
				};
			}
			OriginAlign = builder.OriginAlign;
		}

		public virtual MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlBase myObjectBuilder_GuiControlBase = MyGuiControlsFactory.CreateObjectBuilder(this);
			myObjectBuilder_GuiControlBase.Position = m_position;
			myObjectBuilder_GuiControlBase.Size = Size;
			myObjectBuilder_GuiControlBase.Name = Name;
			myObjectBuilder_GuiControlBase.BackgroundColor = ColorMask;
			myObjectBuilder_GuiControlBase.ControlTexture = ((BackgroundTexture != null) ? BackgroundTexture.Center.Texture : null);
			myObjectBuilder_GuiControlBase.OriginAlign = OriginAlign;
			return myObjectBuilder_GuiControlBase;
		}

		public static void ReadIfHasValue<T>(ref T target, T? source) where T : struct
		{
			if (source.HasValue)
			{
				target = source.Value;
			}
		}

		public static void ReadIfHasValue(ref Color target, Vector4? source)
		{
			if (source.HasValue)
			{
				target = new Color(source.Value);
			}
		}

		public virtual void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			DrawBackground(backgroundTransitionAlpha);
			DrawElements(transitionAlpha, backgroundTransitionAlpha);
			DrawBorder(transitionAlpha);
		}

		protected void DrawBackground(float transitionAlpha)
		{
			if (BackgroundTexture != null && ColorMask.W > 0f)
			{
				BackgroundTexture.Draw(GetPositionAbsoluteTopLeft(), Size, ApplyColorMaskModifiers(ColorMask, Enabled, transitionAlpha));
			}
		}

		protected void DrawBorder(float transitionAlpha)
		{
			if (DEBUG_CONTROL_BORDERS)
			{
				float num = (float)(MyGuiManager.TotalTimeInMilliseconds % 5000) / 5000f;
				Color color = new Vector3((PositionY + num) % 1f, PositionX / 2f + 0.5f, 1f).HSVtoColor();
				MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft(), Size, color, 1);
			}
			else if (BorderEnabled || (BorderHighlightEnabled && HasHighlight))
			{
				Color color2 = ApplyColorMaskModifiers(BorderColor * ColorMask, Enabled, transitionAlpha);
				MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + BorderMargin, Size - BorderMargin * 2f, color2, BorderSize);
			}
		}

		public virtual MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			return null;
		}

		public virtual MyGuiControlBase GetExclusiveInputHandler()
		{
			return GetExclusiveInputHandler(Elements);
		}

		public static MyGuiControlBase GetExclusiveInputHandler(MyGuiControls controls)
		{
			foreach (MyGuiControlBase visibleControl in controls.GetVisibleControls())
			{
				MyGuiControlBase exclusiveInputHandler = visibleControl.GetExclusiveInputHandler();
				if (exclusiveInputHandler != null)
				{
					return exclusiveInputHandler;
				}
			}
			return null;
		}

		public virtual MyGuiControlBase GetMouseOverControl()
		{
			if (IsMouseOver)
			{
				return this;
			}
			return null;
		}

		public virtual MyGuiControlBase HandleInput()
		{
			bool isMouseOver = IsMouseOver;
			IsMouseOver = CheckMouseOver();
			if (IsActiveControl)
			{
				m_mouseButtonPressed = (IsMouseOver && MyInput.Static.IsPrimaryButtonPressed());
				if (IsMouseOver && !isMouseOver && Enabled && CanPlaySoundOnMouseOver)
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
				}
			}
			if ((IsMouseOver && isMouseOver) || (HasFocus && MyInput.Static.IsJoystickLastUsed))
			{
				m_showToolTipByFocus = (!IsMouseOver || !isMouseOver);
				if (!m_showToolTip)
				{
					m_showToolTipDelay = MyGuiManager.TotalTimeInMilliseconds + MyGuiConstants.SHOW_CONTROL_TOOLTIP_DELAY;
					m_showToolTip = true;
				}
			}
			else if (m_showToolTip)
			{
				m_showToolTip = false;
			}
			return null;
		}

		public virtual void HideToolTip()
		{
			m_showToolTip = false;
		}

		public virtual bool IsMouseOverAnyControl()
		{
			return IsMouseOver;
		}

		public virtual void ShowToolTip()
		{
			foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
			{
				visibleControl.ShowToolTip();
			}
			if (!m_showToolTip || (!Enabled && !ShowTooltipWhenDisabled) || MyGuiManager.TotalTimeInMilliseconds <= m_showToolTipDelay || m_toolTip == null || !m_toolTip.HasContent)
			{
				return;
			}
			if (m_showToolTipByFocus)
			{
				RectangleF focusRectangle = FocusRectangle;
				m_toolTipPosition = focusRectangle.Position + focusRectangle.Size / 2f;
				if (HasFocus)
				{
					m_toolTip.Draw(m_toolTipPosition);
				}
				else
				{
					m_showToolTip = false;
				}
			}
			else
			{
				m_toolTipPosition = MyGuiManager.MouseCursorPosition;
				if (CheckMouseOver(Size, GetPositionAbsolute(), OriginAlign))
				{
					m_toolTip.Draw(m_toolTipPosition);
				}
				else
				{
					m_showToolTip = false;
				}
			}
		}

		public virtual void Update()
		{
			HasHighlight = ShouldHaveHighlight();
			foreach (MyGuiControlBase element in Elements)
			{
				element.Update();
			}
		}

		protected virtual bool ShouldHaveHighlight()
		{
			if (HighlightType == MyGuiControlHighlightType.CUSTOM)
			{
				return HasHighlight;
			}
			if (Enabled && HighlightType != 0 && IsMouseOverOrKeyboardActive())
			{
				if (HighlightType != MyGuiControlHighlightType.WHEN_ACTIVE && (HighlightType != MyGuiControlHighlightType.WHEN_CURSOR_OVER || !IsMouseOver))
				{
					return HasFocus;
				}
				return true;
			}
			return false;
		}

		public virtual bool CheckMouseOver()
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			RectangleF value = new RectangleF(positionAbsoluteTopLeft, m_size);
			RectangleF result = new RectangleF(0f, 0f, 0f, 0f);
			MyGuiControlBase myGuiControlBase = Owner as MyGuiControlBase;
			bool flag = true;
			while (myGuiControlBase != null && flag)
			{
				flag &= myGuiControlBase.IsMouseOver;
				Vector2 positionAbsoluteTopLeft2 = myGuiControlBase.GetPositionAbsoluteTopLeft();
				RectangleF value2 = new RectangleF(positionAbsoluteTopLeft2, myGuiControlBase.m_size);
				if (!myGuiControlBase.SkipForMouseTest && (!RectangleF.Intersect(ref value, ref value2, out result) || !IsPointInside(MyGuiManager.MouseCursorPosition, result.Size, result.Position, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)))
				{
					return false;
				}
				myGuiControlBase = (myGuiControlBase.Owner as MyGuiControlBase);
			}
			if (flag)
			{
				return CheckMouseOver(Size, GetPositionAbsolute(), OriginAlign);
			}
			return false;
		}

		protected virtual void OnHasHighlightChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.HasHighlight = HasHighlight;
			}
		}

		protected virtual void OnPositionChanged()
		{
		}

		protected virtual void OnSizeChanged()
		{
			if (this.SizeChanged != null)
			{
				this.SizeChanged(this);
			}
		}

		protected virtual void OnVisibleChanged()
		{
			if (this.VisibleChanged != null)
			{
				this.VisibleChanged(this, m_visible);
			}
		}

		protected virtual void OnOriginAlignChanged()
		{
		}

		protected virtual void OnEnabledChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.Enabled = m_enabled;
			}
		}

		protected virtual void OnColorMaskChanged()
		{
			foreach (MyGuiControlBase element in Elements)
			{
				element.ColorMask = ColorMask;
			}
		}

		internal virtual void OnFocusChanged(bool focus)
		{
			this.FocusChanged.InvokeIfNotNull(this, focus);
			Owner?.OnFocusChanged(this, focus);
		}

		public static Color ApplyColorMaskModifiers(Vector4 sourceColorMask, bool enabled, float transitionAlpha)
		{
			Vector4 vector = sourceColorMask;
			if (!enabled)
			{
				vector.X *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.X;
				vector.Y *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.Y;
				vector.Z *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.Z;
				vector.W *= MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER.W;
			}
			vector *= transitionAlpha;
			return new Color(vector);
		}

		public virtual string GetMouseCursorTexture()
		{
			string mouseCursorTexture = MyGuiManager.GetMouseCursorTexture();
			_ = IsMouseOver;
			return mouseCursorTexture;
		}

		public Vector2 GetPositionAbsolute()
		{
			if (Owner != null)
			{
				return Owner.GetPositionAbsoluteCenter() + m_position;
			}
			return m_position;
		}

		public Vector2 GetPositionAbsoluteBottomLeft()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(0f, Size.Y);
		}

		public Vector2 GetPositionAbsoluteBottomRight()
		{
			return GetPositionAbsoluteTopLeft() + Size;
		}

		public Vector2 GetPositionAbsoluteCenterLeft()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(0f, Size.Y * 0.5f);
		}

		public Vector2 GetPositionAbsoluteCenter()
		{
			return MyUtils.GetCoordCenterFromAligned(GetPositionAbsolute(), Size, OriginAlign);
		}

		public Vector2 GetPositionAbsoluteTopLeft()
		{
			return MyUtils.GetCoordTopLeftFromAligned(GetPositionAbsolute(), Size, OriginAlign);
		}

		public Vector2 GetPositionAbsoluteTopRight()
		{
			return GetPositionAbsoluteTopLeft() + new Vector2(Size.X, 0f);
		}

		public Vector2? GetSize()
		{
			return Size;
		}

		public virtual void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
		}

		public void SetToolTip(MyToolTips toolTip)
		{
			m_toolTip = toolTip;
		}

		public void SetToolTip(string text)
		{
			SetToolTip(new MyToolTips(text));
		}

		public void SetToolTip(MyStringId text)
		{
			SetToolTip(MyTexts.GetString(text));
		}

		public static bool CheckMouseOver(Vector2 size, Vector2 position, MyGuiDrawAlignEnum originAlign)
		{
			return IsPointInside(MyGuiManager.MouseCursorPosition, size, position, originAlign);
		}

		public static bool IsPointInside(Vector2 queryPoint, Vector2 size, Vector2 position, MyGuiDrawAlignEnum originAlign)
		{
			Vector2 coordCenterFromAligned = MyUtils.GetCoordCenterFromAligned(position, size, originAlign);
			Vector2 vector = coordCenterFromAligned - size / 2f;
			Vector2 vector2 = coordCenterFromAligned + size / 2f;
			if (queryPoint.X >= vector.X && queryPoint.X <= vector2.X && queryPoint.Y >= vector.Y)
			{
				return queryPoint.Y <= vector2.Y;
			}
			return false;
		}

		protected MyGuiScreenBase GetTopMostOwnerScreen()
		{
			try
			{
				IMyGuiControlsOwner owner = Owner;
				while (!(owner is MyGuiScreenBase))
				{
					owner = ((MyGuiControlBase)owner).Owner;
				}
				return owner as MyGuiScreenBase;
			}
			catch (NullReferenceException)
			{
				MyLog.Default.WriteLine("NullReferenceException in " + DebugNamePath + " trying to reach top most owner.");
				return null;
			}
		}

		protected bool IsMouseOverOrKeyboardActive()
		{
			MyGuiScreenBase topMostOwnerScreen = GetTopMostOwnerScreen();
			if (topMostOwnerScreen != null)
			{
				MyGuiScreenState state = topMostOwnerScreen.State;
				if ((uint)state <= 1u || state == MyGuiScreenState.UNHIDING)
				{
					if (!IsMouseOver)
					{
						return HasFocus;
					}
					return true;
				}
				return false;
			}
			return false;
		}

		protected virtual void DrawElements(float transitionAlpha, float backgroundTransitionAlpha)
		{
			foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
			{
				if (visibleControl.GetExclusiveInputHandler() != visibleControl)
				{
					visibleControl.Draw(transitionAlpha * visibleControl.Alpha, backgroundTransitionAlpha * visibleControl.Alpha);
				}
			}
		}

		protected MyGuiControlBase HandleInputElements()
		{
			MyGuiControlBase myGuiControlBase = null;
			MyGuiControlBase[] array = Elements.GetVisibleControls().ToArray();
			for (int num = array.Length - 1; num >= 0; num--)
			{
				myGuiControlBase = array[num].HandleInput();
				if (myGuiControlBase != null)
				{
					break;
				}
			}
			return myGuiControlBase;
		}

		protected virtual void ClearEvents()
		{
			this.SizeChanged = null;
			this.VisibleChanged = null;
			this.NameChanged = null;
		}

		public virtual void OnRemoving()
		{
			if (HasFocus)
			{
				GetTopMostOwnerScreen().FocusedControl = null;
			}
			Elements.Clear();
			Owner = null;
			ClearEvents();
		}

		public void GetElementsUnderCursor(Vector2 position, bool visibleOnly, List<MyGuiControlBase> controls)
		{
			if (visibleOnly)
			{
				foreach (MyGuiControlBase visibleControl in Elements.GetVisibleControls())
				{
					if (IsPointInside(position, visibleControl.Size, visibleControl.GetPositionAbsolute(), visibleControl.OriginAlign))
					{
						visibleControl.GetElementsUnderCursor(position, visibleOnly, controls);
						controls.Add(visibleControl);
					}
				}
			}
			else
			{
				foreach (MyGuiControlBase element in Elements)
				{
					if (IsPointInside(position, element.Size, element.GetPositionAbsolute(), element.OriginAlign))
					{
						element.GetElementsUnderCursor(position, visibleOnly, controls);
						controls.Add(element);
					}
				}
			}
		}

		public MyGuiControlBase GetFocusControl(MyDirection direction, bool page, bool loop)
		{
			if (CanFocusChildren)
			{
				MyGuiControlBase nextFocusControl = GetNextFocusControl(this, direction, page);
				if (nextFocusControl != null)
				{
					return nextFocusControl;
				}
			}
			if (Owner == null)
			{
				return null;
			}
			for (IMyGuiControlsOwner owner = Owner; owner != null; owner = owner.Owner)
			{
				MyGuiControlBase nextFocusControl2 = owner.GetNextFocusControl(this, direction, page);
				if (nextFocusControl2 != null)
				{
					return nextFocusControl2;
				}
			}
			if (loop)
			{
				for (IMyGuiControlsOwner owner = Owner; owner != null; owner = owner.Owner)
				{
					List<MyGuiControlBase> visibleElements = owner.VisibleElements;
					if (visibleElements.Count > 0)
					{
						RectangleF fRect = (direction == MyDirection.Up || direction == MyDirection.Left) ? new RectangleF(new Vector2(-0.5f, 1.5f), new Vector2(1f, 0.01f)) : new RectangleF(new Vector2(-0.5f, -0.5f), new Vector2(1f, 0.01f));
						MyGuiControlBase nextFocusControl3 = MyGuiScreenBase.GetNextFocusControl(ref fRect, null, null, visibleElements, null, direction, page);
						if (nextFocusControl3 != null)
						{
							return nextFocusControl3;
						}
					}
				}
			}
			return this;
		}

		public virtual MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			return MyGuiScreenBase.GetNextFocusControl(currentFocusControl, null, this, Elements.GetVisibleControls(), null, direction, page);
		}

		public virtual void Clear()
		{
		}

		public override string ToString()
		{
			return DebugNamePath;
		}

		public virtual void UpdateMeasure()
		{
		}

		public virtual void UpdateArrange()
		{
		}
	}
}
