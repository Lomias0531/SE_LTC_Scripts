using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenDebugDeveloper : MyGuiScreenDebugBase
	{
		private class MyDevelopGroup
		{
			public string Name;

			public MyGuiControlBase GroupControl;

			public List<MyGuiControlBase> ControlList;

			public MyDevelopGroup(string name)
			{
				Name = name;
				ControlList = new List<MyGuiControlBase>();
			}
		}

		private class MyDevelopGroupTypes
		{
			public Type Name;

			public MyDirectXSupport DirectXSupport;

			public MyDevelopGroupTypes(Type name, MyDirectXSupport directXSupport)
			{
				Name = name;
				DirectXSupport = directXSupport;
			}
		}

		private class DevelopGroupComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				if (x == "Game" && y == "Game")
				{
					return 0;
				}
				if (x == "Game")
				{
					return -1;
				}
				if (y == "Game")
				{
					return 1;
				}
				if (x == "Render" && y == "Render")
				{
					return 0;
				}
				if (x == "Render")
				{
					return -1;
				}
				if (y == "Render")
				{
					return 1;
				}
				return x.CompareTo(y);
			}
		}

		private static MyGuiScreenBase s_activeScreen;

		private static List<MyGuiControlCheckbox> s_groupList;

		private static List<MyGuiControlCheckbox> s_inputList;

		private static MyDevelopGroup s_debugDrawGroup;

		private static MyDevelopGroup s_performanceGroup;

		private static List<MyDevelopGroup> s_mainGroups;

		private static MyDevelopGroup s_activeMainGroup;

		private static MyDevelopGroup s_debugInputGroup;

		private static MyDevelopGroup s_activeDevelopGroup;

		private static SortedDictionary<string, MyDevelopGroup> s_developGroups;

		private static Dictionary<string, SortedDictionary<string, MyDevelopGroupTypes>> s_developScreenTypes;

		private static bool m_profilerEnabled;

		private static bool EnableProfiler
		{
			get
			{
				return VRage.Profiler.MyRenderProfiler.ProfilerVisible;
			}
			set
			{
				if (VRage.Profiler.MyRenderProfiler.ProfilerVisible != value)
				{
					MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Enable, 0, null);
				}
			}
		}

		private static void RegisterScreensFromAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterScreensFromAssembly(assemblies[i]);
				}
			}
		}

		private static void RegisterScreensFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type typeFromHandle = typeof(MyGuiScreenBase);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!typeFromHandle.IsAssignableFrom(type))
				{
					continue;
				}
				object[] customAttributes = type.GetCustomAttributes(typeof(MyDebugScreenAttribute), inherit: false);
				if (customAttributes.Length != 0)
				{
					MyDebugScreenAttribute myDebugScreenAttribute = (MyDebugScreenAttribute)customAttributes[0];
					if (!s_developScreenTypes.TryGetValue(myDebugScreenAttribute.Group, out SortedDictionary<string, MyDevelopGroupTypes> value))
					{
						value = new SortedDictionary<string, MyDevelopGroupTypes>();
						s_developScreenTypes.Add(myDebugScreenAttribute.Group, value);
						s_developGroups.Add(myDebugScreenAttribute.Group, new MyDevelopGroup(myDebugScreenAttribute.Group));
					}
					MyDevelopGroupTypes value2 = new MyDevelopGroupTypes(type, myDebugScreenAttribute.DirectXSupport);
					value.Add(myDebugScreenAttribute.Name, value2);
				}
			}
		}

		static MyGuiScreenDebugDeveloper()
		{
			s_groupList = new List<MyGuiControlCheckbox>();
			s_inputList = new List<MyGuiControlCheckbox>();
			s_debugDrawGroup = new MyDevelopGroup("Debug draw");
			s_performanceGroup = new MyDevelopGroup("Performance");
			s_mainGroups = new List<MyDevelopGroup>
			{
				s_debugDrawGroup,
				s_performanceGroup
			};
			s_activeMainGroup = s_debugDrawGroup;
			s_debugInputGroup = new MyDevelopGroup("Debug Input");
			s_developGroups = new SortedDictionary<string, MyDevelopGroup>(new DevelopGroupComparer());
			s_developScreenTypes = new Dictionary<string, SortedDictionary<string, MyDevelopGroupTypes>>();
			m_profilerEnabled = false;
			RegisterScreensFromAssembly(Assembly.GetExecutingAssembly());
			RegisterScreensFromAssembly(MyPlugins.GameAssembly);
			RegisterScreensFromAssembly(MyPlugins.SandboxAssembly);
			RegisterScreensFromAssembly(MyPlugins.UserAssemblies);
			s_developGroups.Add(s_debugInputGroup.Name, s_debugInputGroup);
			SortedDictionary<string, MyDevelopGroup>.ValueCollection.Enumerator enumerator = s_developGroups.Values.GetEnumerator();
			enumerator.MoveNext();
			s_activeDevelopGroup = enumerator.Current;
		}

		public MyGuiScreenDebugDeveloper()
			: base(new Vector2(0.5f, 0.5f), new Vector2(0.35f, 1f), 0.35f * Color.Yellow.ToVector4(), isTopMostScreen: true)
		{
			m_backgroundColor = null;
			base.EnabledBackgroundFade = true;
			m_backgroundFadeColor = new Color(1f, 1f, 1f, 0.2f);
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			foreach (MyDevelopGroup value4 in s_developGroups.Values)
			{
				if (value4.ControlList.Count > 0)
				{
					EnableGroup(value4, enable: false);
					value4.ControlList.Clear();
				}
			}
			foreach (MyDevelopGroup s_mainGroup in s_mainGroups)
			{
				if (s_mainGroup.ControlList.Count > 0)
				{
					EnableGroup(s_mainGroup, enable: false);
					s_mainGroup.ControlList.Clear();
				}
			}
			float num = -0.02f;
			AddCaption("Developer screen", Color.Yellow.ToVector4(), new Vector2(0f, num));
			m_scale = 0.9f;
			m_closeOnEsc = true;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.03f, 0.1f);
			m_currentPosition.Y += num;
			float num2 = 0f;
			Vector2 vector = new Vector2(0.09f, 0.03f);
			foreach (MyDevelopGroup s_mainGroup2 in s_mainGroups)
			{
				Vector2 value = new Vector2(-0.03f + m_currentPosition.X + num2, m_currentPosition.Y);
				s_mainGroup2.GroupControl = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.Debug, null, new Vector4(1f, 1f, 0.5f, 1f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder(s_mainGroup2.Name), MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * m_scale * 1.2f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnClickMainGroup);
				num2 += s_mainGroup2.GroupControl.Size.X * 1.1f;
				Controls.Add(s_mainGroup2.GroupControl);
			}
			m_currentPosition.Y += vector.Y * 1.1f;
			float y = m_currentPosition.Y;
			float value2 = y;
			CreateDebugDrawControls();
			value2 = MathHelper.Max(value2, m_currentPosition.Y);
			m_currentPosition.Y = y;
			CreatePerformanceControls();
			m_currentPosition.Y = MathHelper.Max(value2, m_currentPosition.Y);
			foreach (MyDevelopGroup s_mainGroup3 in s_mainGroups)
			{
				EnableGroup(s_mainGroup3, enable: false);
			}
			EnableGroup(s_activeMainGroup, enable: true);
			m_currentPosition.Y += 0.02f;
			num2 = 0f;
			foreach (MyDevelopGroup value5 in s_developGroups.Values)
			{
				Vector2 value3 = new Vector2(-0.03f + m_currentPosition.X + num2, m_currentPosition.Y);
				value5.GroupControl = new MyGuiControlButton(value3, MyGuiControlButtonStyleEnum.Debug, null, new Vector4(1f, 1f, 0.5f, 1f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, new StringBuilder(value5.Name), 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale * 1.2f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnClickGroup);
				num2 += value5.GroupControl.Size.X * 1.1f;
				Controls.Add(value5.GroupControl);
			}
			num2 = (0f - num2) / 2f;
			foreach (MyDevelopGroup value6 in s_developGroups.Values)
			{
				value6.GroupControl.PositionX = num2;
				num2 += value6.GroupControl.Size.X * 1.1f;
			}
			m_currentPosition.Y += vector.Y * 1.1f;
			float y2 = m_currentPosition.Y;
			string a = MySandboxGame.Config.GraphicsRenderer.ToString();
			MyStringId directX11RendererKey = MySandboxGame.DirectX11RendererKey;
			bool flag = a == directX11RendererKey.ToString();
			foreach (KeyValuePair<string, SortedDictionary<string, MyDevelopGroupTypes>> s_developScreenType in s_developScreenTypes)
			{
				MyDevelopGroup myDevelopGroup = s_developGroups[s_developScreenType.Key];
				foreach (KeyValuePair<string, MyDevelopGroupTypes> item in s_developScreenType.Value)
				{
					if ((int)item.Value.DirectXSupport >= 1 && flag)
					{
						AddGroupBox(item.Key, item.Value.Name, myDevelopGroup.ControlList);
					}
				}
				m_currentPosition.Y = y2;
			}
			if (MyGuiSandbox.Gui is MyDX9Gui)
			{
				for (int i = 0; i < (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents.Count; i++)
				{
					AddGroupInput($"{(MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[i].GetName()} (Ctrl + numPad{i})", (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[i], s_debugInputGroup.ControlList);
				}
			}
			m_currentPosition.Y = y2;
			foreach (MyDevelopGroup value7 in s_developGroups.Values)
			{
				EnableGroup(value7, enable: false);
			}
			EnableGroup(s_activeDevelopGroup, enable: true);
		}

		private void CreateDebugDrawControls()
		{
			AddCheckBox("Debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW), enabled: true, s_debugDrawGroup.ControlList);
			AddCheckBox("Draw physics", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS), enabled: true, s_debugDrawGroup.ControlList);
			AddCheckBox("Audio debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_AUDIO), enabled: true, s_debugDrawGroup.ControlList);
			AddButton(new StringBuilder("Clear persistent"), delegate
			{
				MyRenderProxy.DebugClearPersistentMessages();
			}, s_debugDrawGroup.ControlList);
			m_currentPosition.Y += 0.01f;
		}

		private void CreatePerformanceControls()
		{
			AddCheckBox("Profiler", () => EnableProfiler, delegate(bool v)
			{
				EnableProfiler = v;
			}, enabled: true, s_performanceGroup.ControlList);
			AddCheckBox("Particles", null, MemberHelper.GetMember(() => MyParticlesManager.Enabled), enabled: true, s_performanceGroup.ControlList);
			m_currentPosition.Y += 0.01f;
		}

		protected void AddGroupInput(string text, MyDebugComponent component, List<MyGuiControlBase> controlGroup = null)
		{
			MyGuiControlCheckbox item = AddCheckBox(text, component, controlGroup);
			s_inputList.Add(item);
		}

		private void AddGroupBox(string text, Type screenType, List<MyGuiControlBase> controlGroup)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = AddCheckBox(text, checkedState: true, null, enabled: true, controlGroup);
			myGuiControlCheckbox.IsChecked = (s_activeScreen != null && s_activeScreen.GetType() == screenType);
			myGuiControlCheckbox.UserData = screenType;
			s_groupList.Add(myGuiControlCheckbox);
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox sender)
			{
				Type type = sender.UserData as Type;
				if (sender.IsChecked)
				{
					foreach (MyGuiControlCheckbox s_group in s_groupList)
					{
						if (s_group != sender)
						{
							s_group.IsChecked = false;
						}
					}
					MyGuiScreenBase obj = (MyGuiScreenBase)Activator.CreateInstance(type);
					obj.Closed += delegate(MyGuiScreenBase source)
					{
						if (source == s_activeScreen)
						{
							s_activeScreen = null;
						}
					};
					MyGuiSandbox.AddScreen(obj);
					s_activeScreen = obj;
				}
				else if (s_activeScreen != null && s_activeScreen.GetType() == type)
				{
					s_activeScreen.CloseScreen();
				}
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDeveloper";
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12))
			{
				CloseScreen();
			}
		}

		private void OnClickGroup(MyGuiControlButton sender)
		{
			EnableGroup(s_activeDevelopGroup, enable: false);
			foreach (MyDevelopGroup value in s_developGroups.Values)
			{
				if (value.GroupControl == sender)
				{
					s_activeDevelopGroup = value;
					break;
				}
			}
			EnableGroup(s_activeDevelopGroup, enable: true);
		}

		private void OnClickMainGroup(MyGuiControlButton sender)
		{
			EnableGroup(s_activeMainGroup, enable: false);
			foreach (MyDevelopGroup s_mainGroup in s_mainGroups)
			{
				if (s_mainGroup.GroupControl == sender)
				{
					s_activeMainGroup = s_mainGroup;
					break;
				}
			}
			EnableGroup(s_activeMainGroup, enable: true);
		}

		private void EnableGroup(MyDevelopGroup group, bool enable)
		{
			foreach (MyGuiControlBase control in group.ControlList)
			{
				control.Visible = enable;
			}
		}
	}
}
