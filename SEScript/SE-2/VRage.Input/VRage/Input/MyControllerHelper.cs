using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace VRage.Input
{
	public static class MyControllerHelper
	{
		public interface IControl
		{
			byte Code
			{
				get;
			}

			Func<bool> Condition
			{
				get;
			}

			bool IsNewPressed();

			bool IsNewPressedRepeating();

			bool IsPressed();

			bool IsNewReleased();

			float AnalogValue();

			object ControlCode();
		}

		private class Context
		{
			public Context ParentContext;

			public Dictionary<MyStringId, IControl> Bindings;

			public static readonly Context Empty = new Context();

			public IControl this[MyStringId id]
			{
				get
				{
					if (Bindings.ContainsKey(id))
					{
						return Bindings[id];
					}
					if (ParentContext != null)
					{
						return ParentContext[id];
					}
					return m_nullControl;
				}
				set
				{
					Bindings[id] = value;
				}
			}

			public Context()
			{
				Bindings = new Dictionary<MyStringId, IControl>(MyStringId.Comparer);
			}
		}

		private class EmptyControl : IControl
		{
			public byte Code => 0;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public bool IsNewPressed()
			{
				return false;
			}

			public bool IsNewPressedRepeating()
			{
				return false;
			}

			public bool IsPressed()
			{
				return false;
			}

			public bool IsNewReleased()
			{
				return false;
			}

			public float AnalogValue()
			{
				return 0f;
			}

			public object ControlCode()
			{
				return " ";
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickAxis : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public MyJoystickAxesEnum Axis;

			public byte Code => (byte)Axis;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickAxis(MyJoystickAxesEnum axis, Func<bool> condition = null)
			{
				Axis = axis;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = MyInput.Static.IsJoystickAxisNewPressed(Axis);
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsJoystickAxisPressed(Axis);
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsNewJoystickAxisReleased(Axis);
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				return MyInput.Static.GetJoystickAxisStateForGameplay(Axis);
			}

			public object ControlCode()
			{
				return XBOX_AXES_CODES[Axis].ToString();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickButton : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public MyJoystickButtonsEnum Button;

			public byte Code => (byte)Button;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickButton(MyJoystickButtonsEnum button, Func<bool> condition = null)
			{
				Button = button;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = MyInput.Static.IsJoystickButtonNewPressed(Button);
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsJoystickButtonPressed(Button);
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				return MyInput.Static.IsNewJoystickButtonReleased(Button);
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				return IsPressed() ? 1 : 0;
			}

			public object ControlCode()
			{
				return XBOX_BUTTONS_CODES[Button].ToString();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		/// <summary>
		/// Loading hints require Controls for displaying, but some things cannot be binded to (any direction on Stick or DPad). For this reason fake controls exist, to be able to display control elements That cannot even have binding
		/// </summary>
		private class FakeControl : IControl
		{
			private string m_fakeCode;

			public byte Code => 0;

			public Func<bool> Condition
			{
				get;
			}

			public FakeControl(string fakecode)
			{
				m_fakeCode = fakecode;
			}

			public float AnalogValue()
			{
				return 0f;
			}

			public object ControlCode()
			{
				return m_fakeCode;
			}

			public bool IsNewPressed()
			{
				return false;
			}

			public bool IsNewPressedRepeating()
			{
				return false;
			}

			public bool IsNewReleased()
			{
				return false;
			}

			public bool IsPressed()
			{
				return false;
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickPressedModifier : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IControl Modifier;

			public IControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickPressedModifier(IControl modifier, IControl control, Func<bool> condition = null)
			{
				Modifier = modifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Modifier.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier.IsNewReleased() || !Control.IsPressed())
				{
					if (Modifier.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickReleasedModifier : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IControl Modifier;

			public IControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickReleasedModifier(IControl modifier, IControl control, Func<bool> condition = null)
			{
				Modifier = modifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = !Modifier.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier.IsNewPressed())
				{
					return Control.IsNewReleased();
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (Modifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return Control.ControlCode();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickPressedTwoModifiers : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IControl Modifier1;

			public IControl Modifier2;

			public IControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickPressedTwoModifiers(IControl modifier1, IControl modifier2, IControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = Modifier1.IsPressed() && Modifier2.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (Modifier1.IsPressed() && Modifier2.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if ((!Modifier1.IsNewReleased() || !Modifier2.IsPressed() || !Control.IsPressed()) && (!Modifier1.IsPressed() || !Modifier2.IsNewReleased() || !Control.IsPressed()))
				{
					if (Modifier1.IsPressed() && Modifier2.IsPressed())
					{
						return Control.IsNewReleased();
					}
					return false;
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!Modifier1.IsPressed() || !Modifier2.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(Modifier1.ControlCode(), " + ", Modifier2.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickReleasedTwoModifiers : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IControl Modifier1;

			public IControl Modifier2;

			public IControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickReleasedTwoModifiers(IControl modifier1, IControl modifier2, IControl control, Func<bool> condition = null)
			{
				Modifier1 = modifier1;
				Modifier2 = modifier2;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = !Modifier1.IsPressed() && !Modifier2.IsPressed() && Control.IsNewPressed();
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Modifier1.IsPressed() && !Modifier2.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!Control.IsPressed() || (!Modifier1.IsNewPressed() && !Modifier2.IsNewPressed()))
				{
					return Control.IsNewReleased();
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (Modifier1.IsPressed() || Modifier2.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return Control.ControlCode();
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class JoystickOneOfTwoModifiers : IControl
		{
			private DateTime m_lastNewPress;

			private bool m_repeatStarted;

			public IControl PressedModifier;

			public IControl ReleasedModifier;

			public IControl Control;

			public byte Code => Control.Code;

			public Func<bool> Condition
			{
				get;
				private set;
			}

			public JoystickOneOfTwoModifiers(IControl pressedModifier, IControl releasedModifier, IControl control, Func<bool> condition = null)
			{
				PressedModifier = pressedModifier;
				ReleasedModifier = releasedModifier;
				Control = control;
				Condition = condition;
			}

			public bool IsNewPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				bool num = (((PressedModifier.IsPressed() && Control.IsNewPressed()) || (PressedModifier.IsNewPressed() && Control.IsPressed())) && !ReleasedModifier.IsPressed()) || (PressedModifier.IsPressed() && Control.IsPressed() && ReleasedModifier.IsNewReleased());
				if (num)
				{
					m_lastNewPress = DateTime.Now;
				}
				return num;
			}

			public bool IsNewPressedRepeating()
			{
				bool flag = false;
				bool num = IsNewPressed();
				if (IsPressed())
				{
					flag = (DateTime.Now - m_lastNewPress > (m_repeatStarted ? REPEAT_TIME : REPEAT_START_TIME));
					if (flag)
					{
						m_repeatStarted = true;
						m_lastNewPress = DateTime.Now;
					}
				}
				else
				{
					m_repeatStarted = false;
				}
				return num || flag;
			}

			public bool IsPressed()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (PressedModifier.IsPressed() && !ReleasedModifier.IsPressed())
				{
					return Control.IsPressed();
				}
				return false;
			}

			public bool IsNewReleased()
			{
				if (Condition != null && !Condition())
				{
					return false;
				}
				if (!PressedModifier.IsNewReleased() && !ReleasedModifier.IsNewPressed())
				{
					return Control.IsNewReleased();
				}
				return true;
			}

			public float AnalogValue()
			{
				if (Condition != null && !Condition())
				{
					return 0f;
				}
				if (!PressedModifier.IsPressed() || ReleasedModifier.IsPressed())
				{
					return 0f;
				}
				return Control.AnalogValue();
			}

			public object ControlCode()
			{
				return string.Concat(PressedModifier.ControlCode(), " + ", Control.ControlCode());
			}

			public override string ToString()
			{
				return (string)ControlCode();
			}
		}

		private class ButtonEvaluator : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				if (ButtonCodes.TryGetValue(token, out string value))
				{
					return value;
				}
				return "<Invalid Button>";
			}
		}

		private static readonly Dictionary<string, string> ButtonCodes;

		private static readonly Dictionary<MyJoystickAxesEnum, char> XBOX_AXES_CODES;

		private static readonly Dictionary<MyJoystickButtonsEnum, char> XBOX_BUTTONS_CODES;

		public static readonly MyStringId CX_BASE;

		public static readonly MyStringId CX_GUI;

		public static readonly MyStringId CX_CHARACTER;

		private static readonly TimeSpan REPEAT_START_TIME;

		private static readonly TimeSpan REPEAT_TIME;

		private static EmptyControl m_nullControl;

		private static Dictionary<MyStringId, Context> m_bindings;

		/// <summary>
		/// Evaluator that can be used to replace button names with their icon.
		/// </summary>
		public static readonly ITextEvaluator ButtonTextEvaluator;

		static MyControllerHelper()
		{
			ButtonCodes = new Dictionary<string, string>();
			XBOX_AXES_CODES = new Dictionary<MyJoystickAxesEnum, char>
			{
				{
					MyJoystickAxesEnum.Xneg,
					'\ue016'
				},
				{
					MyJoystickAxesEnum.Xpos,
					'\ue015'
				},
				{
					MyJoystickAxesEnum.Ypos,
					'\ue014'
				},
				{
					MyJoystickAxesEnum.Yneg,
					'\ue017'
				},
				{
					MyJoystickAxesEnum.RotationXneg,
					'\ue020'
				},
				{
					MyJoystickAxesEnum.RotationXpos,
					'\ue019'
				},
				{
					MyJoystickAxesEnum.RotationYneg,
					'\ue021'
				},
				{
					MyJoystickAxesEnum.RotationYpos,
					'\ue018'
				},
				{
					MyJoystickAxesEnum.Zneg,
					'\ue007'
				},
				{
					MyJoystickAxesEnum.Zpos,
					'\ue008'
				}
			};
			XBOX_BUTTONS_CODES = new Dictionary<MyJoystickButtonsEnum, char>
			{
				{
					MyJoystickButtonsEnum.J01,
					'\ue001'
				},
				{
					MyJoystickButtonsEnum.J02,
					'\ue003'
				},
				{
					MyJoystickButtonsEnum.J03,
					'\ue002'
				},
				{
					MyJoystickButtonsEnum.J04,
					'\ue004'
				},
				{
					MyJoystickButtonsEnum.J05,
					'\ue005'
				},
				{
					MyJoystickButtonsEnum.J06,
					'\ue006'
				},
				{
					MyJoystickButtonsEnum.J07,
					'\ue00d'
				},
				{
					MyJoystickButtonsEnum.J08,
					'\ue00e'
				},
				{
					MyJoystickButtonsEnum.J09,
					'\ue00b'
				},
				{
					MyJoystickButtonsEnum.J10,
					'\ue00c'
				},
				{
					MyJoystickButtonsEnum.JDLeft,
					'\ue010'
				},
				{
					MyJoystickButtonsEnum.JDUp,
					'\ue011'
				},
				{
					MyJoystickButtonsEnum.JDRight,
					'\ue012'
				},
				{
					MyJoystickButtonsEnum.JDDown,
					'\ue013'
				},
				{
					MyJoystickButtonsEnum.J11,
					'\ue007'
				},
				{
					MyJoystickButtonsEnum.J12,
					'\ue008'
				}
			};
			CX_BASE = MyStringId.GetOrCompute("BASE");
			CX_GUI = MyStringId.GetOrCompute("GUI");
			CX_CHARACTER = MyStringId.GetOrCompute("CHARACTER");
			REPEAT_START_TIME = new TimeSpan(0, 0, 0, 0, 500);
			REPEAT_TIME = new TimeSpan(0, 0, 0, 0, 100);
			m_nullControl = new EmptyControl();
			m_bindings = new Dictionary<MyStringId, Context>(MyStringId.Comparer);
			ButtonTextEvaluator = new ButtonEvaluator();
			m_bindings.Add(MyStringId.NullOrEmpty, new Context());
			char v;
			foreach (KeyValuePair<MyJoystickAxesEnum, char> xBOX_AXES_CODE in XBOX_AXES_CODES)
			{
				LinqExtensions.Deconstruct(xBOX_AXES_CODE, out MyJoystickAxesEnum k, out v);
				MyJoystickAxesEnum myJoystickAxesEnum = k;
				char c = v;
				ButtonCodes.Add("AXIS_" + myJoystickAxesEnum.ToString().ToUpperInvariant(), c.ToString());
			}
			ButtonCodes.Add("AXIS_MOTION", "\ue009");
			ButtonCodes.Add("AXIS_ROTATION", "\ue00a");
			ButtonCodes.Add("AXIS_DPAD", "\ue00f");
			foreach (KeyValuePair<MyJoystickButtonsEnum, char> xBOX_BUTTONS_CODE in XBOX_BUTTONS_CODES)
			{
				LinqExtensions.Deconstruct(xBOX_BUTTONS_CODE, out MyJoystickButtonsEnum k2, out v);
				MyJoystickButtonsEnum myJoystickButtonsEnum = k2;
				char c2 = v;
				ButtonCodes.Add("BUTTON_" + myJoystickButtonsEnum.ToString().ToUpperInvariant(), c2.ToString());
			}
		}

		public static void AddContext(MyStringId context, MyStringId? parent = null)
		{
			if (!m_bindings.ContainsKey(context))
			{
				Context context2 = new Context();
				m_bindings.Add(context, context2);
				if (parent.HasValue && m_bindings.ContainsKey(parent.Value))
				{
					context2.ParentContext = m_bindings[parent.Value];
				}
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, string fakeCode)
		{
			m_bindings[context][stringId] = new FakeControl(fakeCode);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickAxesEnum axis, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickAxis(axis, condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum button, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickButton(button, condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier, MyJoystickButtonsEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedModifier(new JoystickButton(modifier), new JoystickButton(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedModifier(new JoystickButton(modifier), new JoystickButton(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier, MyJoystickAxesEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedModifier(new JoystickButton(modifier), new JoystickAxis(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedModifier(new JoystickButton(modifier), new JoystickAxis(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickButtonsEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickButton(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum modifier1, MyJoystickButtonsEnum modifier2, MyJoystickAxesEnum control, bool pressed, Func<bool> condition = null)
		{
			if (pressed)
			{
				m_bindings[context][stringId] = new JoystickPressedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickAxis(control), condition);
			}
			else
			{
				m_bindings[context][stringId] = new JoystickReleasedTwoModifiers(new JoystickButton(modifier1), new JoystickButton(modifier2), new JoystickAxis(control), condition);
			}
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum pressedModifier, MyJoystickButtonsEnum releasedModifier, MyJoystickAxesEnum control, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickOneOfTwoModifiers(new JoystickButton(pressedModifier), new JoystickButton(releasedModifier), new JoystickAxis(control), condition);
		}

		public static void AddControl(MyStringId context, MyStringId stringId, MyJoystickButtonsEnum pressedModifier, MyJoystickButtonsEnum releasedModifier, MyJoystickButtonsEnum control, Func<bool> condition = null)
		{
			m_bindings[context][stringId] = new JoystickOneOfTwoModifiers(new JoystickButton(pressedModifier), new JoystickButton(releasedModifier), new JoystickButton(control), condition);
		}

		public static void NullControl(MyStringId context, MyStringId stringId)
		{
			m_bindings[context][stringId] = m_nullControl;
		}

		public static void NullControl(MyStringId context, MyJoystickAxesEnum axis)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			foreach (KeyValuePair<MyStringId, IControl> binding in m_bindings[context].Bindings)
			{
				if (binding.Value is JoystickAxis && (uint)binding.Value.Code == (uint)axis)
				{
					myStringId = binding.Key;
					break;
				}
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				m_bindings[context][myStringId] = m_nullControl;
			}
		}

		public static bool IsNullControl(IControl control)
		{
			return control == m_nullControl;
		}

		public static IControl GetNullControl()
		{
			return m_nullControl;
		}

		public static void NullControl(MyStringId context, MyJoystickButtonsEnum button)
		{
			MyStringId myStringId = MyStringId.NullOrEmpty;
			foreach (KeyValuePair<MyStringId, IControl> binding in m_bindings[context].Bindings)
			{
				if (binding.Value is JoystickButton && (uint)binding.Value.Code == (uint)button)
				{
					myStringId = binding.Key;
					break;
				}
			}
			if (myStringId != MyStringId.NullOrEmpty)
			{
				m_bindings[context][myStringId] = m_nullControl;
			}
		}

		public static bool IsControl(MyStringId context, MyStringId stringId, MyControlStateType type = MyControlStateType.NEW_PRESSED, bool joystickOnly = false)
		{
			switch (type)
			{
			case MyControlStateType.NEW_PRESSED:
				if (joystickOnly || !MyInput.Static.IsNewGameControlPressed(stringId))
				{
					return m_bindings[context][stringId].IsNewPressed();
				}
				return true;
			case MyControlStateType.NEW_RELEASED:
				if (joystickOnly || !MyInput.Static.IsNewGameControlReleased(stringId))
				{
					return m_bindings[context][stringId].IsNewReleased();
				}
				return true;
			case MyControlStateType.PRESSED:
				if (joystickOnly || !MyInput.Static.IsGameControlPressed(stringId))
				{
					return m_bindings[context][stringId].IsPressed();
				}
				return true;
			case MyControlStateType.NEW_PRESSED_REPEATING:
				if (joystickOnly || !MyInput.Static.IsNewGameControlPressed(stringId))
				{
					return m_bindings[context][stringId].IsNewPressedRepeating();
				}
				return true;
			default:
				return false;
			}
		}

		public static float IsControlAnalog(MyStringId context, MyStringId stringId)
		{
			return MyInput.Static.GetGameControlAnalogState(stringId) + m_bindings[context][stringId].AnalogValue();
		}

		public static bool IsDefined(MyStringId contextId, MyStringId controlId)
		{
			if (m_bindings.TryGetValue(contextId, out Context value))
			{
				return value[controlId] != m_nullControl;
			}
			return false;
		}

		public static string GetCodeForControl(MyStringId context, MyStringId stringId)
		{
			return (string)m_bindings.GetValueOrDefault(context, Context.Empty)[stringId].ControlCode();
		}

		public static IControl GetControl(MyStringId context, MyStringId stringId)
		{
			return m_bindings[context][stringId];
		}

		public static IControl TryGetControl(MyStringId context, MyStringId stringId)
		{
			if (!m_bindings.ContainsKey(context))
			{
				return null;
			}
			return m_bindings[context][stringId];
		}
	}
}
