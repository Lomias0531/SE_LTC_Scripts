using System;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlCheckbox))]
	public class MyGuiControlCheckbox : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture NormalCheckedTexture;

			public MyGuiCompositeTexture NormalUncheckedTexture;

			public MyGuiCompositeTexture HighlightCheckedTexture;

			public MyGuiCompositeTexture HighlightUncheckedTexture;

			public MyGuiHighlightTexture CheckedIcon;

			public MyGuiHighlightTexture UncheckedIcon;

			public bool CheckedBorder;

			public Vector2 BorderMargin;

			public Vector2? SizeOverride;
		}

		private static StyleDefinition[] m_styles;

		public Action<MyGuiControlCheckbox> IsCheckedChanged;

		private bool m_isChecked;

		private MyGuiControlCheckboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private MyGuiHighlightTexture m_icon;

		public bool IsChecked
		{
			get
			{
				return m_isChecked;
			}
			set
			{
				if (m_isChecked != value)
				{
					m_isChecked = value;
					RefreshInternals();
					if (IsCheckedChanged != null)
					{
						IsCheckedChanged(this);
					}
				}
			}
		}

		public MyGuiControlCheckboxStyleEnum VisualStyle
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

		static MyGuiControlCheckbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlCheckboxStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED
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
				HighlightCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED.LeftTop
				},
				HighlightUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED.LeftTop
				},
				SizeOverride = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.MinSizeGui * 0.65f
			};
			m_styles[2] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_NORMAL,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_NORMAL,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_HIGHLIGHT,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_HIGHLIGHT,
				BorderMargin = new Vector2(0.004f),
				CheckedBorder = true
			};
			m_styles[3] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_RIGHT_NORMAL,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_RIGHT_NORMAL,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_RIGHT_HIGHLIGHT,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_SWITCHONOFF_RIGHT_HIGHLIGHT,
				BorderMargin = new Vector2(0.004f),
				CheckedBorder = true
			};
			m_styles[4] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				CheckedIcon = MyGuiConstants.TEXTURE_BUTTON_ICON_REPEAT,
				UncheckedIcon = MyGuiConstants.TEXTURE_BUTTON_ICON_REPEAT_INACTIVE,
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_ICON_REPEAT.SizeGui * 1.4f
			};
			m_styles[5] = new StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				CheckedIcon = MyGuiConstants.TEXTURE_BUTTON_ICON_SLAVE,
				UncheckedIcon = MyGuiConstants.TEXTURE_BUTTON_ICON_SLAVE_INACTIVE,
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_ICON_SLAVE.SizeGui * 1.4f
			};
			m_styles[6] = new StyleDefinition
			{
				NormalCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED.LeftTop
				},
				NormalUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.LeftTop
				},
				HighlightCheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED.LeftTop
				},
				HighlightUncheckedTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED.LeftTop
				},
				SizeOverride = MyGuiConstants.TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED.MinSizeGui * 0.65f
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlCheckboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlCheckbox(Vector2? position = null, Vector4? color = null, string toolTip = null, bool isChecked = false, MyGuiControlCheckboxStyleEnum visualStyle = MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			: base(position ?? Vector2.Zero, null, color, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_ACTIVE, originAlign)
		{
			base.Name = "CheckBox";
			m_isChecked = isChecked;
			VisualStyle = visualStyle;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlCheckbox obj = (MyObjectBuilder_GuiControlCheckbox)base.GetObjectBuilder();
			obj.IsChecked = m_isChecked;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlCheckbox myObjectBuilder_GuiControlCheckbox = (MyObjectBuilder_GuiControlCheckbox)objectBuilder;
			m_isChecked = myObjectBuilder_GuiControlCheckbox.IsChecked;
			VisualStyle = myObjectBuilder_GuiControlCheckbox.VisualStyle;
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
			if (!IsHitTestVisible)
			{
				return null;
			}
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null && base.Enabled && ((base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed()) || (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))))
			{
				UserCheck();
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
			if (IsChecked)
			{
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
				BorderEnabled = m_styleDef.CheckedBorder;
				BorderMargin = m_styleDef.BorderMargin;
			}
			else
			{
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
				BorderEnabled = false;
			}
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
		}

		private void UserCheck()
		{
			MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			IsChecked = !IsChecked;
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
