using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage.Input;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public abstract class MyDebugComponent
	{
		private class MyShortcutComparer : IComparer<MyShortcut>
		{
			public static MyShortcutComparer Static = new MyShortcutComparer();

			public int Compare(MyShortcut x, MyShortcut y)
			{
				return x.GetId().CompareTo(y.GetId());
			}
		}

		private struct MyShortcut
		{
			public MyKeys Key;

			public bool NewPress;

			public bool Control;

			public bool Shift;

			public bool Alt;

			public Func<string> Description;

			public Func<bool> _Action;

			public string GetKeysString()
			{
				string text = "";
				if (Control)
				{
					text += "Ctrl";
				}
				if (Shift)
				{
					text += (string.IsNullOrEmpty(text) ? "Shift" : "+Shift");
				}
				if (Alt)
				{
					text += (string.IsNullOrEmpty(text) ? "Alt" : "+Alt");
				}
				return text + (string.IsNullOrEmpty(text) ? MyInput.Static.GetKeyName(Key) : ("+" + MyInput.Static.GetKeyName(Key)));
			}

			public ushort GetId()
			{
				return (ushort)((ushort)((ushort)((ushort)((uint)Key << 8) + (ushort)(Control ? 4 : 0)) + (ushort)(Shift ? 2 : 0)) + (ushort)(Alt ? 1 : 0));
			}
		}

		public class MyRef<T>
		{
			private Action<T> modify;

			private Func<T> getter;

			public T Value
			{
				get
				{
					return getter();
				}
				set
				{
					modify(value);
				}
			}

			public MyRef(Func<T> getter, Action<T> modify)
			{
				this.modify = modify;
				this.getter = getter;
			}
		}

		private class MySwitch
		{
			public MyKeys Key;

			public Func<MyKeys, bool> Action;

			public string Note;

			private MyRef<bool> m_boolReference;

			private bool m_value;

			public bool IsSet
			{
				get
				{
					if (m_boolReference != null)
					{
						return m_boolReference.Value;
					}
					return m_value;
				}
				set
				{
					if (m_boolReference != null)
					{
						m_boolReference.Value = value;
					}
					else
					{
						m_value = value;
					}
				}
			}

			public MySwitch(MyKeys key, Func<MyKeys, bool> action, string note = "")
			{
				Key = key;
				Action = action;
				Note = note;
			}

			public MySwitch(MyKeys key, Func<MyKeys, bool> action, string note = "", bool defaultValue = false)
			{
				Key = key;
				Action = action;
				Note = note;
				IsSet = defaultValue;
			}

			public MySwitch(MyKeys key, Func<MyKeys, bool> action, MyRef<bool> field, string note = "")
			{
				m_boolReference = field;
				Key = key;
				Action = action;
				Note = note;
			}

			public ushort GetId()
			{
				return (ushort)((uint)Key << 8);
			}
		}

		public enum MyDebugComponentInfoState
		{
			NoInfo,
			EnabledInfo,
			FullInfo
		}

		private static float m_textOffset = 0f;

		private const int LINE_OFFSET = 15;

		private const int LINE_BREAK_OFFSET = 17;

		private static HashSet<ushort> m_enabledShortcutKeys = new HashSet<ushort>();

		private SortedSet<MyShortcut> m_shortCuts = new SortedSet<MyShortcut>(MyShortcutComparer.Static);

		private HashSet<MySwitch> m_switches = new HashSet<MySwitch>();

		private bool m_enabled = true;

		public int m_frameCounter;

		public static float VerticalTextOffset => m_textOffset;

		protected static float NextVerticalOffset
		{
			get
			{
				float textOffset = m_textOffset;
				m_textOffset += 15f;
				return textOffset;
			}
		}

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
			}
		}

		public virtual object InputData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IMyInput Input => MyInput.Static;

		public static float NextTextOffset(float scale)
		{
			float textOffset = m_textOffset;
			m_textOffset += 15f * scale;
			return textOffset;
		}

		protected void Text(string message, params object[] arguments)
		{
			Text(Color.White, 1f, message, arguments);
		}

		protected void Text(Color color, string message, params object[] arguments)
		{
			Text(color, 1f, message, arguments);
		}

		protected void Text(Color color, float scale, string message, params object[] arguments)
		{
			if (arguments.Length != 0)
			{
				message = string.Format(message, arguments);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, NextTextOffset(scale)), message, color, 0.6f * scale);
		}

		protected void MultilineText(string message, params object[] arguments)
		{
			MultilineText(Color.White, 1f, message, arguments);
		}

		protected void MultilineText(Color color, string message, params object[] arguments)
		{
			MultilineText(color, 1f, message, arguments);
		}

		protected void MultilineText(Color color, float scale, string message, params object[] arguments)
		{
			if (arguments.Length != 0)
			{
				message = string.Format(message, arguments);
			}
			int num = 0;
			string text = message;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					num++;
				}
			}
			message = message.Replace("\t", "    ");
			float num2 = 15 + 17 * num;
			num2 *= scale;
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, m_textOffset), message, color, 0.6f * scale);
			m_textOffset += num2;
		}

		public void Section(string text, params object[] formatArgs)
		{
			VSpace(5f);
			Text(Color.Yellow, 1.5f, text, formatArgs);
			VSpace(5f);
		}

		protected void VSpace(float space)
		{
			m_textOffset += space;
		}

		public MyDebugComponent()
			: this(enabled: false)
		{
		}

		public MyDebugComponent(bool enabled)
		{
			Enabled = enabled;
		}

		protected void Save()
		{
			SerializableDictionary<string, MyConfig.MyDebugInputData> debugInputComponents = MySandboxGame.Config.DebugInputComponents;
			string name = GetName();
			MyConfig.MyDebugInputData value = debugInputComponents[name];
			value.Enabled = Enabled;
			value.Data = InputData;
			debugInputComponents[name] = value;
			MySandboxGame.Config.Save();
		}

		public virtual bool HandleInput()
		{
			foreach (MyShortcut shortCut in m_shortCuts)
			{
				bool flag = true;
				flag &= (shortCut.Control == MyInput.Static.IsAnyCtrlKeyPressed());
				flag &= (shortCut.Shift == MyInput.Static.IsAnyShiftKeyPressed());
				flag &= (shortCut.Alt == MyInput.Static.IsAnyAltKeyPressed());
				if (flag)
				{
					flag = ((!shortCut.NewPress) ? (flag & MyInput.Static.IsKeyPress(shortCut.Key)) : (flag & MyInput.Static.IsNewKeyPressed(shortCut.Key)));
				}
				if (flag && shortCut._Action != null)
				{
					return shortCut._Action();
				}
			}
			foreach (MySwitch @switch in m_switches)
			{
				if ((1 & (MyInput.Static.IsNewKeyPressed(@switch.Key) ? 1 : 0)) != 0 && @switch.Action != null)
				{
					return @switch.Action(@switch.Key);
				}
			}
			return false;
		}

		public abstract string GetName();

		public static void ResetFrame()
		{
			m_textOffset = 0f;
			m_enabledShortcutKeys.Clear();
		}

		public virtual void DispatchUpdate()
		{
			if (m_frameCounter % 10 == 0)
			{
				Update10();
			}
			if (m_frameCounter >= 100)
			{
				Update100();
				m_frameCounter = 0;
			}
			m_frameCounter++;
		}

		public virtual void Draw()
		{
			if (MySandboxGame.Config.DebugComponentsInfo == MyDebugComponentInfoState.FullInfo)
			{
				float scale = 0.6f;
				MyRenderProxy.DebugDrawText2D(new Vector2(0.1f, m_textOffset), GetName() + " debug input:", Color.Gold, scale);
				m_textOffset += 15f;
				foreach (MyShortcut shortCut in m_shortCuts)
				{
					string keysString = shortCut.GetKeysString();
					string text = shortCut.Description();
					Color color = m_enabledShortcutKeys.Contains(shortCut.GetId()) ? Color.Red : Color.White;
					MyRenderProxy.DebugDrawText2D(new Vector2(100f, m_textOffset), keysString + ":", color, scale, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					MyRenderProxy.DebugDrawText2D(new Vector2(105f, m_textOffset), text, Color.White, scale);
					m_enabledShortcutKeys.Add(shortCut.GetId());
					m_textOffset += 15f;
				}
				foreach (MySwitch @switch in m_switches)
				{
					Color color2 = GetSwitchValue(@switch.Key) ? Color.Red : Color.White;
					MyRenderProxy.DebugDrawText2D(new Vector2(30f, m_textOffset), string.Concat("switch ", @switch.Key, (@switch.Note.Length == 0) ? "" : (" " + @switch.Note), " is ", GetSwitchValue(@switch.Key) ? "On" : "Off"), color2, scale);
					m_textOffset += 15f;
				}
				m_textOffset += 5f;
			}
		}

		public virtual void Update10()
		{
		}

		public virtual void Update100()
		{
		}

		protected void AddShortcut(MyKeys key, bool newPress, bool control, bool shift, bool alt, Func<string> description, Func<bool> action)
		{
			m_shortCuts.Add(new MyShortcut
			{
				Key = key,
				NewPress = newPress,
				Control = control,
				Shift = shift,
				Alt = alt,
				Description = description,
				_Action = action
			});
		}

		protected void AddSwitch(MyKeys key, Func<MyKeys, bool> action, string note = "", bool defaultValue = false)
		{
			MySwitch item = new MySwitch(key, action, note, defaultValue);
			m_switches.Add(item);
		}

		protected void AddSwitch(MyKeys key, Func<MyKeys, bool> action, MyRef<bool> boolRef, string note = "")
		{
			MySwitch item = new MySwitch(key, action, boolRef, note);
			m_switches.Add(item);
		}

		protected void SetSwitch(MyKeys key, bool value)
		{
			foreach (MySwitch @switch in m_switches)
			{
				if (@switch.Key == key)
				{
					@switch.IsSet = value;
					break;
				}
			}
		}

		public bool GetSwitchValue(MyKeys key)
		{
			foreach (MySwitch @switch in m_switches)
			{
				if (@switch.Key == key)
				{
					return @switch.IsSet;
				}
			}
			return false;
		}

		public bool GetSwitchValue(string note)
		{
			foreach (MySwitch @switch in m_switches)
			{
				if (@switch.Note == note)
				{
					return @switch.IsSet;
				}
			}
			return false;
		}
	}
}
