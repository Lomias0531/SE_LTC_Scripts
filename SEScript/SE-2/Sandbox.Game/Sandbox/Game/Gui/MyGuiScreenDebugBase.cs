using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public abstract class MyGuiScreenDebugBase : MyGuiScreenBase
	{
		private static Vector4 m_defaultColor = Color.Yellow.ToVector4();

		private static Vector4 m_defaultTextColor = new Vector4(1f, 1f, 0f, 1f);

		protected Vector2 m_currentPosition;

		protected float m_scale = 1f;

		protected float m_buttonXOffset;

		protected float m_sliderDebugScale = 1f;

		private float m_maxWidth;

		protected float Spacing;

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugBase";
		}

		protected MyGuiScreenDebugBase(Vector4? backgroundColor = null, bool isTopMostScreen = false)
			: this(new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.16f, 0.5f), new Vector2(0.32f, 1f), backgroundColor ?? (0.85f * Color.Black.ToVector4()), isTopMostScreen)
		{
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			m_isTopMostScreen = false;
			base.CanHaveFocus = false;
			m_isTopScreen = true;
		}

		protected MyGuiScreenDebugBase(Vector2 position, Vector2? size, Vector4? backgroundColor, bool isTopMostScreen)
			: base(position, backgroundColor, size, isTopMostScreen)
		{
			base.CanBeHidden = false;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = false;
			m_canShareInput = true;
			m_isTopScreen = true;
		}

		protected MyGuiControlMultilineText AddMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(0.5f, 0.5f);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(m_currentPosition + vector / 2f + (offset ?? Vector2.Zero), vector, m_defaultColor, "Debug", m_scale * textScale, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable);
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}

		private MyGuiControlCheckbox AddCheckBox(string text, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, text, color ?? m_defaultTextColor, 0.8f * MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * m_scale, "Debug");
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			float val = myGuiControlLabel.GetTextSize().X + 0.02f;
			m_maxWidth = Math.Max(m_maxWidth, val);
			myGuiControlLabel.Enabled = enabled;
			Controls.Add(myGuiControlLabel);
			Vector2? size = GetSize();
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, color ?? m_defaultColor, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Debug, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlCheckbox.Position = m_currentPosition + new Vector2(size.Value.X - myGuiControlCheckbox.Size.X, 0f) + (checkBoxOffset ?? Vector2.Zero);
			myGuiControlCheckbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlCheckbox.Enabled = enabled;
			Controls.Add(myGuiControlCheckbox);
			m_currentPosition.Y += Math.Max(myGuiControlCheckbox.Size.Y, myGuiControlLabel.Size.Y) + Spacing;
			if (controlGroup != null)
			{
				controlGroup.Add(myGuiControlLabel);
				controlGroup.Add(myGuiControlCheckbox);
			}
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox AddCheckBox(string text, bool enabled)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text);
			myGuiControlCheckbox.IsChecked = enabled;
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox AddCheckBox(string text, MyDebugComponent component, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, enabled: true, controlGroup, color, checkBoxOffset);
			myGuiControlCheckbox.IsChecked = component.Enabled;
			myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox sender)
			{
				component.Enabled = sender.IsChecked;
			};
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox AddCheckBox(MyStringId textEnum, bool checkedState, Action<MyGuiControlCheckbox> checkBoxChange, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			return AddCheckBox(MyTexts.GetString(textEnum), checkedState, checkBoxChange, enabled, controlGroup, color, checkBoxOffset);
		}

		protected MyGuiControlCheckbox AddCheckBox(string text, bool checkedState, Action<MyGuiControlCheckbox> checkBoxChange, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
			myGuiControlCheckbox.IsChecked = checkedState;
			if (checkBoxChange != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox sender)
				{
					checkBoxChange(sender);
					ValueChanged(sender);
				};
			}
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox AddCheckBox(MyStringId textEnum, Func<bool> getter, Action<bool> setter, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			return AddCheckBox(MyTexts.GetString(textEnum), getter, setter, enabled, controlGroup, color, checkBoxOffset);
		}

		protected MyGuiControlCheckbox AddCheckBox(string text, Func<bool> getter, Action<bool> setter, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
			if (getter != null)
			{
				myGuiControlCheckbox.IsChecked = getter();
			}
			if (setter != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox sender)
				{
					setter(sender.IsChecked);
					ValueChanged(sender);
				};
			}
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox AddCheckBox(string text, object instance, MemberInfo memberInfo, bool enabled = true, List<MyGuiControlBase> controlGroup = null, Vector4? color = null, Vector2? checkBoxOffset = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				myGuiControlCheckbox.IsChecked = (bool)propertyInfo.GetValue(instance, new object[0]);
				myGuiControlCheckbox.UserData = new Tuple<object, PropertyInfo>(instance, propertyInfo);
				myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox sender)
				{
					Tuple<object, PropertyInfo> tuple2 = sender.UserData as Tuple<object, PropertyInfo>;
					tuple2.Item2.SetValue(tuple2.Item1, sender.IsChecked, new object[0]);
					ValueChanged(sender);
				};
			}
			else if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				myGuiControlCheckbox.IsChecked = (bool)fieldInfo.GetValue(instance);
				myGuiControlCheckbox.UserData = new Tuple<object, FieldInfo>(instance, fieldInfo);
				myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox sender)
				{
					Tuple<object, FieldInfo> tuple = sender.UserData as Tuple<object, FieldInfo>;
					tuple.Item2.SetValue(tuple.Item1, sender.IsChecked);
					ValueChanged(sender);
				};
			}
			return myGuiControlCheckbox;
		}

		protected virtual void ValueChanged(MyGuiControlBase sender)
		{
		}

		private MyGuiControlSliderBase AddSliderBase(string text, MyGuiSliderProperties props, Vector4? color = null)
		{
			MyGuiControlSliderBase myGuiControlSliderBase = new MyGuiControlSliderBase(m_currentPosition, 460f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, props, null, null, 0.75f * m_scale, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlSliderBase.DebugScale = m_sliderDebugScale;
			myGuiControlSliderBase.ColorMask = (color ?? m_defaultColor);
			Controls.Add(myGuiControlSliderBase);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition + new Vector2(0.015f, 0f), null, text, color ?? m_defaultTextColor, 0.640000045f * m_scale, "Debug");
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			float val = myGuiControlLabel.GetTextSize().X + 0.02f;
			m_maxWidth = Math.Max(m_maxWidth, val);
			Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += myGuiControlSliderBase.Size.Y + Spacing;
			return myGuiControlSliderBase;
		}

		private MyGuiControlSlider AddSlider(string text, float valueMin, float valueMax, Vector4? color = null)
		{
			Vector2? position = m_currentPosition;
			float width = 460f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			MyGuiControlSlider myGuiControlSlider = new MyGuiControlSlider(position, valueMin, valueMax, width, null, null, new StringBuilder(" {0}").ToString(), 3, 0.75f * m_scale, 0f, "Debug", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: false, showLabel: true);
			myGuiControlSlider.DebugScale = m_sliderDebugScale;
			myGuiControlSlider.ColorMask = (color ?? m_defaultColor);
			Controls.Add(myGuiControlSlider);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition + new Vector2(0.015f, 0f), null, text, color ?? m_defaultTextColor, 0.640000045f * m_scale, "Debug");
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			float val = myGuiControlLabel.GetTextSize().X + 0.02f;
			m_maxWidth = Math.Max(m_maxWidth, val);
			Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += myGuiControlSlider.Size.Y + Spacing;
			return myGuiControlSlider;
		}

		protected MyGuiControlSlider AddSlider(string text, float value, float valueMin, float valueMax, Vector4? color = null)
		{
			MyGuiControlSlider myGuiControlSlider = AddSlider(text, valueMin, valueMax, color);
			myGuiControlSlider.Value = value;
			return myGuiControlSlider;
		}

		protected MyGuiControlSlider AddSlider(string text, float value, float valueMin, float valueMax, Action<MyGuiControlSlider> valueChange, Vector4? color = null)
		{
			MyGuiControlSlider myGuiControlSlider = AddSlider(text, valueMin, valueMax, color);
			myGuiControlSlider.Value = value;
			myGuiControlSlider.ValueChanged = valueChange;
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, new Action<MyGuiControlSlider>(ValueChanged));
			return myGuiControlSlider;
		}

		protected MyGuiControlSlider AddSlider(string text, float valueMin, float valueMax, Func<float> getter, Action<float> setter, Vector4? color = null)
		{
			MyGuiControlSlider myGuiControlSlider = AddSlider(text, valueMin, valueMax, color);
			myGuiControlSlider.Value = getter();
			myGuiControlSlider.UserData = setter;
			myGuiControlSlider.ValueChanged = delegate(MyGuiControlSlider sender)
			{
				((Action<float>)sender.UserData)(sender.Value);
				ValueChanged(sender);
			};
			return myGuiControlSlider;
		}

		protected MyGuiControlSlider AddSlider(string text, float valueMin, float valueMax, float valueDefault, Func<float> getter, Action<float> setter, Vector4? color = null)
		{
			MyGuiControlSlider myGuiControlSlider = AddSlider(text, valueMin, valueMax, color);
			myGuiControlSlider.Value = getter();
			myGuiControlSlider.UserData = setter;
			myGuiControlSlider.DefaultValue = valueDefault;
			myGuiControlSlider.ValueChanged = delegate(MyGuiControlSlider sender)
			{
				((Action<float>)sender.UserData)(sender.Value);
				ValueChanged(sender);
			};
			return myGuiControlSlider;
		}

		protected MyGuiControlSliderBase AddSlider(string text, MyGuiSliderProperties properties, Func<float> getter, Action<float> setter, Vector4? color = null)
		{
			MyGuiControlSliderBase myGuiControlSliderBase = AddSliderBase(text, properties, color);
			myGuiControlSliderBase.Value = getter();
			myGuiControlSliderBase.UserData = setter;
			myGuiControlSliderBase.ValueChanged = delegate(MyGuiControlSliderBase sender)
			{
				((Action<float>)sender.UserData)(sender.Value);
				ValueChanged(sender);
			};
			return myGuiControlSliderBase;
		}

		protected MyGuiControlSlider AddSlider(string text, float valueMin, float valueMax, object instance, MemberInfo memberInfo, Vector4? color = null)
		{
			MyGuiControlSlider myGuiControlSlider = AddSlider(text, valueMin, valueMax, color);
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				myGuiControlSlider.Value = (float)propertyInfo.GetValue(instance, new object[0]);
				myGuiControlSlider.UserData = new Tuple<object, PropertyInfo>(instance, propertyInfo);
				myGuiControlSlider.ValueChanged = delegate(MyGuiControlSlider sender)
				{
					Tuple<object, PropertyInfo> tuple2 = sender.UserData as Tuple<object, PropertyInfo>;
					tuple2.Item2.SetValue(tuple2.Item1, sender.Value, new object[0]);
					ValueChanged(sender);
				};
			}
			else if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				myGuiControlSlider.Value = (float)fieldInfo.GetValue(instance);
				myGuiControlSlider.UserData = new Tuple<object, FieldInfo>(instance, fieldInfo);
				myGuiControlSlider.ValueChanged = delegate(MyGuiControlSlider sender)
				{
					Tuple<object, FieldInfo> tuple = sender.UserData as Tuple<object, FieldInfo>;
					tuple.Item2.SetValue(tuple.Item1, sender.Value);
					ValueChanged(sender);
				};
			}
			return myGuiControlSlider;
		}

		protected MyGuiControlTextbox AddTextbox(string value, Action<MyGuiControlTextbox> onTextChanged, Vector4? color = null, float scale = 1f, MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal, List<MyGuiControlBase> controlGroup = null, string font = "Debug", MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, bool addToControls = true)
		{
			MyGuiControlTextbox textbox = new MyGuiControlTextbox(m_currentPosition, value, 6, color, scale, type);
			textbox.OriginAlign = originAlign;
			if (onTextChanged != null)
			{
				textbox.EnterPressed += onTextChanged;
				textbox.FocusChanged += delegate(MyGuiControlBase _, bool hasFocus)
				{
					if (!hasFocus)
					{
						onTextChanged(textbox);
					}
				};
			}
			if (addToControls)
			{
				Controls.Add(textbox);
			}
			m_currentPosition.Y += textbox.Size.Y + 0.01f + Spacing;
			controlGroup?.Add(textbox);
			return textbox;
		}

		protected MyGuiControlLabel AddLabel(string text, Vector4 color, float scale, List<MyGuiControlBase> controlGroup = null, string font = "Debug")
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, text, color, 0.8f * MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * scale * m_scale, font);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			float val = myGuiControlLabel.GetTextSize().X + 0.02f;
			m_maxWidth = Math.Max(m_maxWidth, val);
			Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += myGuiControlLabel.Size.Y + Spacing;
			controlGroup?.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected MyGuiControlLabel AddSubcaption(MyStringId textEnum, Vector4? captionTextColor = null, Vector2? captionOffset = null, float captionScale = 0.8f)
		{
			return AddSubcaption(MyTexts.GetString(textEnum), captionTextColor, captionOffset, captionScale);
		}

		protected MyGuiControlLabel AddSubcaption(string text, Vector4? captionTextColor = null, Vector2? captionOffset = null, float captionScale = 0.8f)
		{
			float num = (!m_size.HasValue) ? 0f : (m_size.Value.X / 2f);
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
			m_currentPosition.X += num;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition + (captionOffset.HasValue ? captionOffset.Value : Vector2.Zero), null, text, captionTextColor ?? m_defaultColor, captionScale, "Debug", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			Elements.Add(myGuiControlLabel);
			m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + Spacing;
			m_currentPosition.X -= num;
			return myGuiControlLabel;
		}

		private MyGuiControlColor AddColor(string text)
		{
			MyGuiControlColor myGuiControlColor = new MyGuiControlColor(text, m_scale, m_currentPosition, Color.White, Color.White, MyCommonTexts.DialogAmount_AddAmountCaption, placeSlidersVertically: false, "Debug");
			myGuiControlColor.ColorMask = Color.Yellow.ToVector4();
			myGuiControlColor.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Controls.Add(myGuiControlColor);
			m_currentPosition.Y += myGuiControlColor.Size.Y;
			return myGuiControlColor;
		}

		protected MyGuiControlColor AddColor(string text, Func<Color> getter, Action<Color> setter)
		{
			return AddColor(text, getter(), delegate(MyGuiControlColor c)
			{
				setter(c.GetColor());
			});
		}

		protected MyGuiControlColor AddColor(string text, Color value, Action<MyGuiControlColor> setter)
		{
			MyGuiControlColor colorControl = AddColor(text);
			colorControl.SetColor(value);
			colorControl.OnChange += delegate
			{
				setter(colorControl);
			};
			return colorControl;
		}

		protected MyGuiControlColor AddColor(string text, object instance, MemberInfo memberInfo)
		{
			MyGuiControlColor myGuiControlColor = AddColor(text);
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				object value = propertyInfo.GetValue(instance, new object[0]);
				if (value is Color)
				{
					myGuiControlColor.SetColor((Color)value);
				}
				else if (value is Vector3)
				{
					myGuiControlColor.SetColor((Vector3)value);
				}
				else if (value is Vector4)
				{
					myGuiControlColor.SetColor((Vector4)value);
				}
				myGuiControlColor.UserData = new Tuple<object, PropertyInfo>(instance, propertyInfo);
				myGuiControlColor.OnChange += delegate(MyGuiControlColor sender)
				{
					Tuple<object, PropertyInfo> tuple2 = sender.UserData as Tuple<object, PropertyInfo>;
					if (tuple2.Item2.MemberType.GetType() == typeof(Color))
					{
						tuple2.Item2.SetValue(tuple2.Item1, sender.GetColor(), new object[0]);
						ValueChanged(sender);
					}
					else if (tuple2.Item2.MemberType.GetType() == typeof(Vector3))
					{
						tuple2.Item2.SetValue(tuple2.Item1, sender.GetColor().ToVector3(), new object[0]);
						ValueChanged(sender);
					}
					else if (tuple2.Item2.MemberType.GetType() == typeof(Vector4))
					{
						tuple2.Item2.SetValue(tuple2.Item1, sender.GetColor().ToVector4(), new object[0]);
						ValueChanged(sender);
					}
				};
			}
			else if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				object value2 = fieldInfo.GetValue(instance);
				if (value2 is Color)
				{
					myGuiControlColor.SetColor((Color)value2);
				}
				else if (value2 is Vector3)
				{
					myGuiControlColor.SetColor((Vector3)value2);
				}
				else if (value2 is Vector4)
				{
					myGuiControlColor.SetColor((Vector4)value2);
				}
				myGuiControlColor.UserData = new Tuple<object, FieldInfo>(instance, fieldInfo);
				myGuiControlColor.OnChange += delegate(MyGuiControlColor sender)
				{
					Tuple<object, FieldInfo> tuple = sender.UserData as Tuple<object, FieldInfo>;
					if (tuple.Item2.FieldType == typeof(Color))
					{
						tuple.Item2.SetValue(tuple.Item1, sender.GetColor());
						ValueChanged(sender);
					}
					else if (tuple.Item2.FieldType == typeof(Vector3))
					{
						tuple.Item2.SetValue(tuple.Item1, sender.GetColor().ToVector3());
						ValueChanged(sender);
					}
					else if (tuple.Item2.FieldType == typeof(Vector4))
					{
						tuple.Item2.SetValue(tuple.Item1, sender.GetColor().ToVector4());
						ValueChanged(sender);
					}
				};
			}
			return myGuiControlColor;
		}

		public MyGuiControlButton AddButton(string text, Action<MyGuiControlButton> onClick, List<MyGuiControlBase> controlGroup = null, Vector4? textColor = null, Vector2? size = null)
		{
			return AddButton(new StringBuilder(text), onClick, controlGroup, textColor, size);
		}

		public MyGuiControlButton AddButton(StringBuilder text, Action<MyGuiControlButton> onClick, List<MyGuiControlBase> controlGroup = null, Vector4? textColor = null, Vector2? size = null, bool increaseSpacing = true, bool addToControls = true)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(m_buttonXOffset, m_currentPosition.Y), MyGuiControlButtonStyleEnum.Debug, null, m_defaultColor, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			if (addToControls)
			{
				Controls.Add(myGuiControlButton);
			}
			if (increaseSpacing)
			{
				m_currentPosition.Y += myGuiControlButton.Size.Y + 0.01f + Spacing;
			}
			controlGroup?.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		protected MyGuiControlCombobox AddCombo(List<MyGuiControlBase> controlGroup = null, Vector4? textColor = null, Vector2? size = null, int openAreaItemsCount = 10, bool addToControls = true, Vector2? overridePos = null)
		{
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox((overridePos.HasValue && overridePos.HasValue) ? overridePos.Value : m_currentPosition, size, null, null, openAreaItemsCount, null, useScrollBarOffset: false, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, textColor)
			{
				VisualStyle = MyGuiControlComboboxStyleEnum.Debug,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			if (addToControls)
			{
				Controls.Add(myGuiControlCombobox);
			}
			if (!overridePos.HasValue || !overridePos.HasValue)
			{
				m_currentPosition.Y += myGuiControlCombobox.Size.Y + 0.01f + Spacing;
			}
			controlGroup?.Add(myGuiControlCombobox);
			return myGuiControlCombobox;
		}

		protected MyGuiControlCombobox AddCombo<TEnum>(TEnum selectedItem, Action<TEnum> valueChanged, bool enabled = true, int openAreaItemsCount = 10, List<MyGuiControlBase> controlGroup = null, Vector4? color = null) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			MyGuiControlCombobox combobox = AddCombo(controlGroup, color, null, openAreaItemsCount);
			TEnum[] values = MyEnum<TEnum>.Values;
			for (int i = 0; i < values.Length; i++)
			{
				TEnum val = values[i];
				combobox.AddItem((int)(object)val, new StringBuilder(MyTexts.TrySubstitute(val.ToString())));
			}
			combobox.SelectItemByKey((int)(object)selectedItem);
			combobox.ItemSelected += delegate
			{
				valueChanged(MyEnum<TEnum>.SetValue((ulong)combobox.GetSelectedKey()));
			};
			return combobox;
		}

		protected MyGuiControlCombobox AddCombo<TEnum>(object instance, MemberInfo memberInfo, bool enabled = true, int openAreaItemsCount = 10, List<MyGuiControlBase> controlGroup = null, Vector4? color = null) where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			MyGuiControlCombobox combobox = AddCombo(controlGroup, color, null, openAreaItemsCount);
			TEnum[] values = MyEnum<TEnum>.Values;
			for (int i = 0; i < values.Length; i++)
			{
				TEnum val = values[i];
				combobox.AddItem((int)(object)val, new StringBuilder(val.ToString()));
			}
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo property = memberInfo as PropertyInfo;
				combobox.SelectItemByKey((int)property.GetValue(instance, new object[0]));
				combobox.ItemSelected += delegate
				{
					property.SetValue(instance, Enum.ToObject(typeof(TEnum), combobox.GetSelectedKey()), new object[0]);
				};
			}
			else if (memberInfo is FieldInfo)
			{
				FieldInfo field = memberInfo as FieldInfo;
				combobox.SelectItemByKey((int)field.GetValue(instance));
				combobox.ItemSelected += delegate
				{
					field.SetValue(instance, Enum.ToObject(typeof(TEnum), combobox.GetSelectedKey()));
				};
			}
			return combobox;
		}

		protected MyGuiControlListbox AddListBox(float verticalSize, List<MyGuiControlBase> controlGroup = null)
		{
			MyGuiControlListbox myGuiControlListbox = new MyGuiControlListbox(m_currentPosition)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlListbox.Size = new Vector2(myGuiControlListbox.Size.X, verticalSize);
			Controls.Add(myGuiControlListbox);
			m_currentPosition.Y += myGuiControlListbox.Size.Y + 0.01f + Spacing;
			controlGroup?.Add(myGuiControlListbox);
			return myGuiControlListbox;
		}

		protected void AddShareFocusHint()
		{
			MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(0.01f, (0f - m_size.Value.Y) / 2f + 0.07f), null, "(press ALT to share focus)", Color.Yellow.ToVector4(), 0.56f, "Debug", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control);
		}

		protected void AddVerticalSpacing(float value = 0.01f)
		{
			m_currentPosition.Y += value;
		}

		public override bool Draw()
		{
			if (!MyGuiSandbox.IsDebugScreenEnabled())
			{
				return false;
			}
			if (!base.Draw())
			{
				return false;
			}
			return true;
		}
	}
}
