using System;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlIndeterminateCheckbox))]
	public class MyGuiControlIndeterminateCheckbox : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture NormalCheckedTexture;

			public MyGuiCompositeTexture NormalUncheckedTexture;

			public MyGuiCompositeTexture NormalIndeterminateTexture;

			public MyGuiCompositeTexture HighlightCheckedTexture;

			public MyGuiCompositeTexture HighlightUncheckedTexture;

			public MyGuiCompositeTexture HighlightIndeterminateTexture;

			public MyGuiHighlightTexture CheckedIcon;

			public MyGuiHighlightTexture UncheckedIcon;

			public MyGuiHighlightTexture IndeterimateIcon;

			public Vector2? SizeOverride;
		}

		private static StyleDefinition[] m_styles;

		public Action<MyGuiControlIndeterminateCheckbox> IsCheckedChanged;

		private CheckStateEnum m_state;

		private MyGuiControlIndeterminateCheckboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private MyGuiHighlightTexture m_icon;

		public CheckStateEnum State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (m_state != value)
				{
					m_state = value;
					RefreshInternals();
					if (IsCheckedChanged != null)
					{
						IsCheckedChanged(this);
					}
				}
			}
		}

		public MyGuiControlIndeterminateCheckboxStyleEnum VisualStyle
		{
			get
			{
				return m_visualStyle;
			}
			set
			{
				m_visualStyle = value;
				RefreshVisualStyle();
			}
		}

		static MyGuiControlIndeterminateCheckbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlIndeterminateCheckboxStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED,
				NormalIndeterminateTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_INDETERMINATE,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED,
				HighlightIndeterminateTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_INDETERMINATE
			};
			m_styles[1] = new StyleDefinition
			{
				NormalCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED.LeftTop
				},
				NormalUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.LeftTop
				},
				NormalIndeterminateTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_INDETERMINATE.LeftTop
				},
				HighlightCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED.LeftTop
				},
				HighlightUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED.LeftTop
				},
				HighlightIndeterminateTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_INDETERMINATE.LeftTop
				},
				SizeOverride = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.MinSizeGui * 0.65f
			};
			m_styles[2] = new StyleDefinition
			{
				NormalCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED.LeftTop
				},
				NormalUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.LeftTop
				},
				NormalIndeterminateTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_INDETERMINATE.LeftTop
				},
				HighlightCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED.LeftTop
				},
				HighlightUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED.LeftTop
				},
				HighlightIndeterminateTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_INDETERMINATE.LeftTop
				},
				SizeOverride = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.MinSizeGui * 0.65f
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlIndeterminateCheckboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlIndeterminateCheckbox(Vector2? position = null, Vector4? color = null, string toolTip = null, CheckStateEnum state = CheckStateEnum.Unchecked, MyGuiControlIndeterminateCheckboxStyleEnum visualStyle = MyGuiControlIndeterminateCheckboxStyleEnum.Default, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			: base(position ?? Vector2.Zero, null, color, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_ACTIVE, originAlign)
		{
			base.Name = "CheckBox";
			m_state = state;
			VisualStyle = visualStyle;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlIndeterminateCheckbox obj = (MyObjectBuilder_GuiControlIndeterminateCheckbox)base.GetObjectBuilder();
			obj.State = m_state;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlIndeterminateCheckbox myObjectBuilder_GuiControlIndeterminateCheckbox = (MyObjectBuilder_GuiControlIndeterminateCheckbox)objectBuilder;
			m_state = myObjectBuilder_GuiControlIndeterminateCheckbox.State;
			VisualStyle = myObjectBuilder_GuiControlIndeterminateCheckbox.VisualStyle;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, transitionAlpha);
			string text = base.HasHighlight ? m_icon.Highlight : m_icon.Normal;
			if (!string.IsNullOrEmpty(text))
			{
				MyGuiManager.DrawSpriteBatch(text, GetPositionAbsoluteCenter(), m_icon.SizeGui, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			if (!base.Enabled)
			{
				return null;
			}
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase != null)
			{
				return myGuiControlBase;
			}
			if ((base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed()) || (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))))
			{
				UserCheck();
				myGuiControlBase = this;
			}
			else if (base.IsMouseOver && MyInput.Static.IsNewSecondaryButtonPressed())
			{
				UserCheck(primary: false);
				myGuiControlBase = this;
			}
			return myGuiControlBase;
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (m_styleDef == null)
			{
				m_styleDef = m_styles[0];
			}
			switch (State)
			{
			case CheckStateEnum.Checked:
				if (base.HasHighlight)
				{
					BackgroundTexture = m_styleDef.HighlightCheckedTexture;
				}
				else
				{
					BackgroundTexture = m_styleDef.NormalCheckedTexture;
				}
				m_icon = m_styleDef.CheckedIcon;
				base.Size = (m_styleDef.SizeOverride ?? BackgroundTexture.MinSizeGui);
				break;
			case CheckStateEnum.Unchecked:
				if (base.HasHighlight)
				{
					BackgroundTexture = m_styleDef.HighlightUncheckedTexture;
				}
				else
				{
					BackgroundTexture = m_styleDef.NormalUncheckedTexture;
				}
				m_icon = m_styleDef.UncheckedIcon;
				base.Size = (m_styleDef.SizeOverride ?? BackgroundTexture.MinSizeGui);
				break;
			case CheckStateEnum.Indeterminate:
				if (base.HasHighlight)
				{
					BackgroundTexture = m_styleDef.HighlightIndeterminateTexture;
				}
				else
				{
					BackgroundTexture = m_styleDef.NormalIndeterminateTexture;
				}
				m_icon = m_styleDef.IndeterimateIcon;
				base.Size = (m_styleDef.SizeOverride ?? BackgroundTexture.MinSizeGui);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
		}

		private void UserCheck(bool primary = true)
		{
			MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			if (primary)
			{
				switch (State)
				{
				case CheckStateEnum.Checked:
					State = CheckStateEnum.Indeterminate;
					break;
				case CheckStateEnum.Unchecked:
					State = CheckStateEnum.Checked;
					break;
				case CheckStateEnum.Indeterminate:
					State = CheckStateEnum.Unchecked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				State = CheckStateEnum.Unchecked;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDef = style;
				RefreshInternals();
			}
		}
	}
}
